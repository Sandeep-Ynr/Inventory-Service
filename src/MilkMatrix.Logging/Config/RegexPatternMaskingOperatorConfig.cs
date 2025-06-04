namespace MilkMatrix.Logging.Config;

public sealed record RegexPatternMaskingOperatorConfig
{
    public string Pattern { get; init; }
    public string[] Keywords { get; init; }
}
