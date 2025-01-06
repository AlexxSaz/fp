using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace FileSenderRailway;

public class FileSender
{
    private readonly ICryptographer cryptographer;
    private readonly IRecognizer recognizer;
    private readonly Func<DateTime> now;
    private readonly ISender sender;

    public FileSender(
        ICryptographer cryptographer,
        ISender sender,
        IRecognizer recognizer,
        Func<DateTime> now)
    {
        this.cryptographer = cryptographer;
        this.sender = sender;
        this.recognizer = recognizer;
        this.now = now;
    }

    public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
    {
        return files.Select(file =>
            new FileSendResult(file, PrepareFileToSend(file, certificate)
                .Then(doc => sender.Send(doc)).Error));
    }

    private Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate) =>
        recognizer.Recognize(file)
            .Then(x => IsValidTimestamp(x))
            .Then(x => IsValidFormatVersion(x))
            .Then(x => x with { Content = cryptographer.Sign(x.Content, certificate) })
            .RefineError("Can't prepare file to send");


    private static Result<Document> IsValidFormatVersion(Result<Document> doc) =>
        doc.Value.Format is "4.0" or "3.1" ? doc : Result.Fail<Document>("Invalid format version");


    private Result<Document> IsValidTimestamp(Result<Document> doc) =>
        doc.Value.Created > now().AddMonths(-1) ? doc : Result.Fail<Document>("Invalid timestamp");
}