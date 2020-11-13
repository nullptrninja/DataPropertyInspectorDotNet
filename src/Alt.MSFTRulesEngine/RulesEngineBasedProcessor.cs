using System.Linq;
using RulesEngine.Models;

namespace Alt.MSFTRulesEngine
{
    public class RulesEngineBasedProcessor<TDataModel>
        where TDataModel : class
    {
        private WorkflowRules[] mWorkflows;
        private RulesEngine.RulesEngine mRulesEngine;

        public RulesEngineBasedProcessor(WorkflowRules[] workflows)
        {
            mWorkflows = workflows;
            mRulesEngine = new RulesEngine.RulesEngine(mWorkflows, null);
        }

        public bool MatchesAnyRule(TDataModel targetObject)
        {
            var results = mRulesEngine.ExecuteRule("PreprocessorWorkflowChecks", new dynamic[] { targetObject });
            return results.Any(r => r.IsSuccess);
        }
    }
}
