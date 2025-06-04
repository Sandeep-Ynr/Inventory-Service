using MilkMatrix.Logging.Config;
using Serilog.Enrichers.Sensitive;
using System.Text.RegularExpressions;

namespace MilkMatrix.Logging.Extensions;

public sealed class RegexPatternMaskingOperator : RegexMaskingOperator
{
    private readonly string keyword;
    private readonly string[] keywords;
    private readonly Func<string, bool> shouldMaskInput;

    public RegexPatternMaskingOperator(string pattern, string[] keywords = null)
        : base(pattern, RegexOptions.Compiled)
    {
        var validKeywords = keywords?.Where(k => !k.IsNullOrWhiteSpace()).ToArray()
                            ?? Array.Empty<string>();

        switch (validKeywords.Length)
        {
            case 0:
                shouldMaskInput = MustMaskInput;
                break;

            case 1:
                keyword = validKeywords[0];
                shouldMaskInput = ShouldMaskInputWithKeyword;
                break;

            default:
                this.keywords = validKeywords;
                shouldMaskInput = ShouldMaskInputWithKeywords;
                break;
        }
    }

    public RegexPatternMaskingOperator(string pattern, string keyword = null)
        : this(pattern, new[]
        {
    keyword
        })
    {
    }

    public RegexPatternMaskingOperator(RegexPatternMaskingOperatorConfig config)
        : this(config.Pattern, config.Keywords)
    {
    }

    protected override bool ShouldMaskInput(string input)
        => shouldMaskInput(input);

    private bool ShouldMaskInputWithKeywords(string input)
        => input.ContainsAny(keywords);

    private bool ShouldMaskInputWithKeyword(string input)
        => input.Contains(keyword);

    private bool MustMaskInput(string input)
        => true;
}
