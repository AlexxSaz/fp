using TagCloud.Model;

namespace TagCloud.Infrastructure.Tags;

public interface IWordTag
{
    string Value { get; }
    Font Font { get; }
    Point Location { get; }
}