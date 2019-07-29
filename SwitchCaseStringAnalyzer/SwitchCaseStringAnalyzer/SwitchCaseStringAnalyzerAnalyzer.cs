using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SwitchCaseStringAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SwitchCaseStringAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "Naming";
        public const string DiagnosticId = "PHU0001";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Message = new LocalizableResourceString(nameof(Resources.AnalyzerMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSyntaxNodeAction(SwitchStatementSyntaxAction, SyntaxKind.SwitchStatement);
        }

        private void SwitchStatementSyntaxAction(SyntaxNodeAnalysisContext obj)
        {
            if (obj.Node is SwitchStatementSyntax sss)
            {
                var typeInfo = obj.SemanticModel.GetTypeInfo(sss.Expression, obj.CancellationToken);
                if (typeInfo.Type.SpecialType == SpecialType.System_String)
                {
                    var diag = Diagnostic.Create(Rule, obj.Node.GetLocation());
                    obj.ReportDiagnostic(diag);
                }
            }
        }
    }
}
