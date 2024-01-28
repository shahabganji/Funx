using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;

namespace Funx.Analyzer.Tests;

public class OptionCodeFixProviderTests
{
    [Fact]
    public async Task Suggests_code_fix_for_throwing_exception_when_method_returns_Option_T()
    {
        const string source = """
                              using System;

                              public class Program
                              {
                                  public Funx.Option<int> GetValue(string number)
                                  {
                                     throw new InvalidOperationException("Could not parse the number");
                                  }
                              }
                              """;

        const string newSource = """
                                 using System;

                                 public class Program
                                 {
                                     public Funx.Option<int> GetValue(string number)
                                     {
                                        return Funx.Factories.None;
                                     }
                                 }
                                 """;

        var expected = CodeFixVerifier<OptionSemanticAnalyzer, OptionCodeFixProvider>.Diagnostic()
            .WithLocation(7, 8)
            .WithMessage("The exception must be replaced by Option.None");

        var codeFixTester = CSharpAnalyzerTestHelper
            .GetCodeFixAnalyzerForOption<OptionSemanticAnalyzer, OptionCodeFixProvider, XUnitVerifier>(
                source, [expected], newSource);

        await codeFixTester.RunAsync();
    }
}
