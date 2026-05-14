#if BHSDK_ROSLYN
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BHSDK.Roslyn
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RuleContainerAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "BHSDK.RuleContainerAnalyzer";

        private const string RuleContainerAttributeName = "RuleContainerAttribute";
        private const string RuleContainerName = "RuleContainer";

        private const string Title = "Class with attribute [" + RuleContainerName + "] must apply dependencies";
        private const string MessageFormat = "Class {0} must be non-static, non-abstract " +
                                             "and with public parameterless constructor";
        private const string Description = "All classes must be instantiatable";
        private const string Category = "BHSDK.Rules";

        private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
        
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            if (classDeclaration.AttributeLists.Count == 0) return;

            var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
            if (classSymbol == null) return;

            var hasRuleAttribute = classSymbol.GetAttributes().Any(attributeData =>
                attributeData.AttributeClass?.Name switch
                {
                    RuleContainerName => true,
                    RuleContainerAttributeName => true,
                    _ => false,
                });
            if (!hasRuleAttribute) return;

            if (classSymbol.IsStatic)
            {
                var diagnostic = Diagnostic.Create(Rule,
                    classDeclaration.Identifier.GetLocation(), classSymbol.Name);
                context.ReportDiagnostic(diagnostic);
                return;
            }
            if (classSymbol.IsAbstract)
            {
                var diagnostic = Diagnostic.Create(Rule,
                    classDeclaration.Identifier.GetLocation(), classSymbol.Name);
                context.ReportDiagnostic(diagnostic);
                return;
            }
            
            var hasParameterlessConstructor = classSymbol.Constructors
                .Any(ctor => ctor.DeclaredAccessibility == Accessibility.Public && ctor.Parameters.IsEmpty);
            if (!hasParameterlessConstructor)
            {
                var diagnostic = Diagnostic.Create(Rule,
                    classDeclaration.Identifier.GetLocation(), classSymbol.Name);
                context.ReportDiagnostic(diagnostic);
                return;
            }
        }
    }
}
#endif