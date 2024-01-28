using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Testing;

namespace Funx.Analyzer.Tests;

public class OptionSemanticAnalyzerTests
{
    [Fact]
    public async Task Detects_diagnostic_for_throwing_error_when_a_method_returns_option_of_T()
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

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<OptionSemanticAnalyzer, XUnitVerifier>(
            source, [AnalyzerVerifier<OptionSemanticAnalyzer>.Diagnostic().WithLocation(7, 8)]);

        // Assert
        await analyserTest.RunAsync();
    }
    
    [Fact]
    public async Task Detects_diagnostic_for_throwing_error_on_a_nested_level_when_a_method_returns_option_of_T()
    {
        const string source = """
                              using System;

                              public class Program
                              {
                                  public Funx.Option<int> GetValue(string number)
                                  {
                                      var parsed = int.TryParse(number, out var n);
                                      if (!parsed)
                                          throw new InvalidOperationException("Could not parse the number");
                              
                                      return n;
                                  }
                              }
                              """;

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<OptionSemanticAnalyzer, XUnitVerifier>(
            source, [AnalyzerVerifier<OptionSemanticAnalyzer>.Diagnostic().WithLocation(9, 13)]);

        // Assert
        await analyserTest.RunAsync();
    }
    
    [Fact]
    public async Task Detects_diagnostic_for_a_local_function_throwing_error_on_a_nested_level_when_a_method_returns_option_of_T()
    {
        const string source = """
                              using System;

                              public class Program
                              {
                                  public int GetValue(string number)
                                  {
                                      var value = GetValueInternally(number);
                                      return value.UnwrappedValue;
                                      
                                      Funx.Option<int> GetValueInternally(string number)
                                      {
                                          var parsed = int.TryParse(number, out var n);
                                          if (!parsed)
                                              throw new InvalidOperationException("Could not parse the number");
                                      
                                          return n;
                                      }
                                  }
                              }
                              """;

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<OptionSemanticAnalyzer, XUnitVerifier>(
            source, [AnalyzerVerifier<OptionSemanticAnalyzer>.Diagnostic().WithLocation(9, 13)]);

        // Assert
        await analyserTest.RunAsync();
    }
    
    [Fact]
    public async Task No_diagnostic_for_a_local_function_with_normal_return_type_throwing_error_on_a_nested_level_when_a_method_returns_option_of_T()
    {
        const string source = """
                              using System;

                              public class Program
                              {
                                  public Funx.Option<int> GetValue(string number)
                                  {
                                      var value = GetValueInternally(number);
                                      return value;
                                      
                                      int GetValueInternally(string number)
                                      {
                                          var parsed = int.TryParse(number, out var n);
                                          if (!parsed)
                                              throw new InvalidOperationException("Could not parse the number");
                                      
                                          return n;
                                      }
                                  }
                              }
                              """;

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<OptionSemanticAnalyzer, XUnitVerifier>(
            source, ImmutableArray<DiagnosticResult>.Empty);

        // Assert
        await analyserTest.RunAsync();
    }
    
    [Fact]
    public async Task XX_No_diagnostic_for_a_local_function_with_normal_return_type_throwing_error_on_a_nested_level_when_a_method_returns_option_of_T()
    {
        const string source = """
                              using System;

                              public class Program
                              {
                                  public static double Bar(int i , Func<int, Funx.Option<double>> converter)
                                  {
                                      var converted = converter(i);
                                      return converted.UnwrappedValue;
                                  }
                                  
                                  public void Foo()
                                  {
                                     var x = Bar(11, x =>
                                     {
                                         if (x == 0)
                                             throw new Exception("Exception from lambda");
                              
                                         return x;
                                     });
                                  }
                              }
                              """;

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<OptionSemanticAnalyzer, XUnitVerifier>(
            source, [AnalyzerVerifier<OptionSemanticAnalyzer>.Diagnostic().WithLocation(16, 15)]);

        // Assert
        await analyserTest.RunAsync();
    }
}
