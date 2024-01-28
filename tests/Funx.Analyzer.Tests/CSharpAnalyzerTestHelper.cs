using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Funx.Analyzer.Tests;

public class CSharpAnalyzerTestHelper
{
    public static CSharpAnalyzerTest<TAnalyzer, TVerifier> GetAnalyzerForOption<TAnalyzer, TVerifier>(
        string source, IEnumerable<DiagnosticResult> expectedDiagnostics)
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TVerifier : IVerifier, new()
    {
        var analyzerTest = new CSharpAnalyzerTest<TAnalyzer, TVerifier>
        {
            TestState =
            {
                Sources = { source },
                ReferenceAssemblies = new ReferenceAssemblies("net8.0",
                    new PackageIdentity("Microsoft.NETCore.App.Ref", "8.0.0"),
                    Path.Combine("ref", "net8.0")),
                AdditionalReferences =
                {
                    // To avoid => error CS0246: The type or namespace name 'Funx' could not be found (are you missing a using directive or an assembly reference? 
                    MetadataReference.CreateFromFile(typeof(Option.None).Assembly.Location),
                }
            }
        };
        analyzerTest.ExpectedDiagnostics.AddRange(expectedDiagnostics);

        return analyzerTest;
    }

    public static CSharpCodeFixTest<TAnalyzer,TCodeFix, TVerifier> GetCodeFixAnalyzerForOption<TAnalyzer, TCodeFix, TVerifier>(
        string source, IEnumerable<DiagnosticResult> expectedDiagnostics, string fixedCode)
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TVerifier : IVerifier, new()
        where TCodeFix : CodeFixProvider, new()
    {
        var codeFixTest = new CSharpCodeFixTest<TAnalyzer, TCodeFix, TVerifier>
        {
            TestState =
            {
                Sources = { source },
                ReferenceAssemblies = new ReferenceAssemblies("net8.0",
                    new PackageIdentity("Microsoft.NETCore.App.Ref", "8.0.0"),
                    Path.Combine("ref", "net8.0")),
                AdditionalReferences =
                {
                    // To avoid => error CS0246: The type or namespace name 'Funx' could not be found (are you missing a using directive or an assembly reference? 
                    MetadataReference.CreateFromFile(typeof(Option.None).Assembly.Location),
                }
            },
            FixedCode = fixedCode
            
        };
        codeFixTest.ExpectedDiagnostics.AddRange(expectedDiagnostics);

        return codeFixTest;
    }
}
