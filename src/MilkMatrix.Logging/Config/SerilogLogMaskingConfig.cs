namespace MilkMatrix.Logging.Config;

public sealed record SerilogLogMaskingConfig
{
  public const string SectionName = "Serilog:LogMasking";

  public bool Disabled { get; init; }

  public string? MaskText { get; init; }

  public string[] MaskProperties { get; init; }

  public bool EnableEmailAddressMaskingOperator { get; init; }

  public RegexPatternMaskingOperatorConfig[] RegexPatternMaskingOperators { get; init; }
}
