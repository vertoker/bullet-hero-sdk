using System.Collections.Generic;
using BH.SDK.Models;
using BH.SDK.Validations.Graph;

namespace BH.SDK.Validations
{
    // Until now the rule engine was library code nothing called: analyzer, fixer and rules all
    // existed, no consumer ever ran them, and "the format guarantees X" meant "the format documents
    // X". This is the entry point that makes the standard enforceable - one call a loader, an editor
    // or an external tool can make without knowing that validation has two halves, that the graph
    // pass only applies to a Level, or that repairs need several passes to settle.
    //
    // It deliberately does not decide what to DO about a report. A player's game refuses to start on
    // HasErrors; an editor shows everything and lets the author choose; an importer repairs first and
    // reports what it could not. Those are different policies over the same facts.

    /// <summary>
    /// The single entry point for validating and repairing save-format data.
    /// </summary>
    public class ValidationFacade
    {
        private readonly RuleAnalyzer _analyzer = new();
        private readonly RuleFixer _fixer = new();
        private readonly LevelGraphAnalyzer _graphAnalyzer = new();

        /// <summary> Validate any aggregate root - Level, LevelMeta, UserSettings, a standalone
        /// Prefab or EffectData. The graph pass runs only for a Level, since cross-object invariants
        /// need the whole level to resolve against. </summary>
        public ValidationReport Validate(object root, RuleAnalyzerSettings settings = null)
        {
            settings ??= new RuleAnalyzerSettings();

            var ruleIssues = _analyzer.Analyze(root, settings);
            var graphIssues = root is Level level
                ? _graphAnalyzer.Analyze(level)
                : new List<GraphIssue>();

            return new ValidationReport(ruleIssues, graphIssues);
        }

        /// <summary>
        /// Repair what can be repaired, then report what is left. Graph findings are never repaired -
        /// every fix for one is a content decision (which of two colliding objects keeps its id,
        /// where a broken parent chain should reattach) and guessing would rewrite the level.
        /// </summary>
        public ValidationReport ValidateAndFix(object root,
            RuleAnalyzerSettings analyzerSettings = null, RuleFixerSettings fixerSettings = null)
        {
            analyzerSettings ??= new RuleAnalyzerSettings();
            fixerSettings ??= new RuleFixerSettings();

            var ruleIssues = _fixer.FixUntilStable(_analyzer, root, analyzerSettings, fixerSettings);

            // The graph pass runs AFTER repairs, not before: a repair can create a graph violation
            // (giving an unset id a value can collide it with an existing one), so the report has to
            // describe the level as it now stands, not as it arrived.
            var graphIssues = root is Level level
                ? _graphAnalyzer.Analyze(level)
                : new List<GraphIssue>();

            return new ValidationReport(ruleIssues, graphIssues);
        }
    }
}
