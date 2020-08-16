using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configurator.Model
{
    [Table("Scrits")]
    public class Script
    {
        public string ScriptName { get; set; }
        public string ExecutionText { get;  set; }
        public string TriggetExpression { get;  set; }
        public string ErrorLine { get; set; }
        public string ErrorColumn { get; set; }
        public State State { get; set; }
        public TriggerType TriggerType { get; set; }
        public ICollection<string> Aliases { get; set; }
        public ICollection<string> AliasesRef { get; set; }
        public string DeclarationText { get; set; }
        public bool ExcutionErrorCondition { get; set; }
        public string ExcutionErrorDesc { get; set; }
        public bool IsInheritated { get; set; }

        
        public string InstanceName { get; set; }
        public string TemplateName { get; set; }
        public Template Template { get; set; }
        public Instance Instance { get; set; }


        public Script(string executionText, string triggerexpr, TriggerType trigertype, 
                      bool excutionError,string executionerrordesc, string decltext, bool inheritated)
        {
            ExecutionText = executionText;
            TriggetExpression = triggerexpr;
            TriggerType = trigertype;
            DeclarationText = decltext;
            ExcutionErrorCondition = excutionError;
            ExcutionErrorDesc = executionerrordesc;
            IsInheritated = inheritated;
        }
    }
    public enum State
    {
        Initializing,
        Error,
        Disabled,
        Busy,
        Ready
    }
    public enum TriggerType
    {
        WhileTrue,
        WhileFalse,
        OnTrue,
        OnFalse,
        DataChange,
        Pereodic
    }
}