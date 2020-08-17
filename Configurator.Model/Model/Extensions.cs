using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configurator.Model
{
    [Table("Scrits")]
    public class Script
    {
        [Key]
        [Column("GUID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Идентификатор не может быть пустым")]
        public Guid ID { get; set; }

        [Column("Scrit Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя скрипта не может быть пустым")]
        public string ScriptName { get; set; }

        [Column("Scrit Text")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Текст скрипта не может быть пустым")]
        public string ExecutionText { get; set; }

        [Column("Trigger Expression")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Условие срабатывания скрипта не может быть пустым")]
        public string TriggetExpression { get; set; }

        [Column("Error Line")]
        public string ErrorLine { get; set; }
        [Column("Error Column")]
        public string ErrorColumn { get; set; }

        [Column("State")]
        public State? State { get; set; }

        [Column("Error Line")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Тип срабатывания не может быть пустым")]
        public TriggerType TriggerType { get; set; }

        [Column("Declaration Text")]
        public string DeclarationText { get; set; }

        [Column("Описание ошибки")]
        public string ExcutionErrorDesc { get; set; }

        [Column("IsInheritated")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Скрипт должен быть либо унаследованный либо принадлежать объекту")]
        public bool IsInheritated { get; set; }


        public ICollection<string> Aliases { get; set; }
        public ICollection<string> AliasesRef { get; set; }

        [ForeignKey("Instance")]
        [Column("Имя содержащего экземпляра")]
        public string InstanceName { get; set; }
        [ForeignKey("Template")]
        [Column("Имя содержащего экземпляра")]
        public string TemplateName { get; set; }
        public Template Template { get; set; }
        public Instance Instance { get; set; }


        public Script(string guid,
                      string scrname,
                      string executionText,
                      bool inheritated,
                      string triggerexpr,
                      TriggerType trigertype,
                      string errorline = null,
                      string errorcol = null,
                      State? state = null,
                      string executionerrordesc = null, 
                      string decltext = null)
        {
            if (string.IsNullOrWhiteSpace(guid))
                throw new ArgumentNullException($"Идентификатор не может быть пустым");
            if (string.IsNullOrWhiteSpace(scrname))
                throw new ArgumentNullException($"Имя скрипта не может быть пустым");
            if (string.IsNullOrWhiteSpace(executionText))
                throw new ArgumentNullException($"Текст скрипта не может быть пустым");
            if (string.IsNullOrWhiteSpace(triggerexpr))
                throw new ArgumentNullException($"Условие срабатывания скрипта не может быть пустым");

            Guid.TryParseExact(guid, "B", out Guid guid_out);
            ID = guid_out;
            ScriptName = scrname;
            ExecutionText = executionText;
            IsInheritated = inheritated;
            TriggetExpression = triggerexpr;
            TriggerType = trigertype;
            ErrorLine = errorline;
            ErrorColumn = errorcol;
            State = state;
            DeclarationText = decltext;
            ExcutionErrorDesc = executionerrordesc;

            if (InstanceName != null)
                if (Template != null)
                    throw new ArgumentException("Поля InstanceName и TemplateName не могут принимать значение однвременно.");
        }
    }
    public enum State
    {
        Unknown,
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