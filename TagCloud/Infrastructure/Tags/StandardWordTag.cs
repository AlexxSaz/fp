using TagCloud.Model;

namespace TagCloud.Infrastructure.Tags;

public record StandardWordTag(string Value, Font Font, Point Location) : IWordTag;