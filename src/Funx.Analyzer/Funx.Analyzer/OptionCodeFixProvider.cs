using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace Funx.Analyzer;

/// <summary>
/// A sample code fix provider that renames classes with the company name in their definition.
/// All code fixes must  be linked to specific analyzers.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(OptionCodeFixProvider)), Shared]
public class OptionCodeFixProvider : CodeFixProvider
{
    // Specify the diagnostic IDs of analyzers that are expected to be linked.
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(OptionSemanticAnalyzer.DiagnosticId);

    // If you don't need the 'fix all' behaviour, return null.
    public override FixAllProvider? GetFixAllProvider() => null;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // We link only one diagnostic and assume there is only one diagnostic in the context.
        var diagnostic = context.Diagnostics.Single();

        // 'SourceSpan' of 'Location' is the highlighted area. We're going to use this area to find the 'SyntaxNode' to rename.
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Get the root of Syntax Tree that contains the highlighted diagnostic.
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        // Find SyntaxNode corresponding to the diagnostic.
        var diagnosticNode = root?.FindNode(diagnosticSpan);

        // To get the required metadata, we should match the Node to the specific type: 'ClassDeclarationSyntax'.
        if (diagnosticNode is not ThrowStatementSyntax throwStatementSyntaxNode)
            return;

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Resources.FX0001CodeFixTitle, "Exception", "Option.None"),
                createChangedDocument: c => ReplaceThrowWithReturnAsync(context.Document, throwStatementSyntaxNode, c),
                equivalenceKey: nameof(Resources.FX0001CodeFixTitle)),
            diagnostic);
    }


    private static async Task<Document> ReplaceThrowWithReturnAsync(Document document, CSharpSyntaxNode throwStatement,
        CancellationToken cancellationToken)
    {
        var newReturnStatement = SyntaxFactory.ReturnStatement(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Funx"),
                        SyntaxFactory.IdentifierName("Factories")),
                    SyntaxFactory.IdentifierName("None")))
            .NormalizeWhitespace();
        
        // Keep the original formatting
        newReturnStatement = newReturnStatement
            .WithLeadingTrivia(throwStatement.GetLeadingTrivia())
            .WithTrailingTrivia(throwStatement.GetTrailingTrivia());

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = root?.ReplaceNode(throwStatement, newReturnStatement);
        
        var formattedRoot = Formatter.Format(newRoot!, Formatter.Annotation, document.Project.Solution.Workspace);
        return document.WithSyntaxRoot(formattedRoot);
    }
}
