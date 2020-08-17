using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configurator.Model
{
    [Table("Templates")]
    public class Template
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя тега не может быть пустым")]
        [Column("Имя шаблона")]
        public string TemplateName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать время создания шаблона")]
        [Column("Дата создания")]
        public DateTime DateCreation { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать, от какого шаблона создаг тег")]
        [Column("Название производящего шаблона")]
        public string DerivedFrom { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать, базовый шаблон")]
        [Column("Базовый шаблон")]
        public string BasedOn { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать, это экземрпляр или шаблон")]
        public bool IsTemplate { get; set; }

        [Column("Краткое описание")]
        public string ShortDesc { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать катеорию тега")]
        [Column("Категория")]
        public ECATEGORY CategoryTag { get; set; }

        [Column("Иерархическое имя")]
        public string HierarchicalName { get; set; }


        public ICollection<GAttribute> AttributesList { get; set; }
        public ICollection<Script> Script { get; set; }

        public Template(string tagname, 
                        string derFrom, 
                        string basedOn,
                        ECATEGORY category = ECATEGORY.idxCategoryUndefined,
                        ICollection<GAttribute> attrs = null,
                        ICollection<Script> script = null,
                        string shortDesc = null)
        {
            if (string.IsNullOrWhiteSpace(tagname))
                throw new ArgumentNullException($"Имя тега не может быть пустым");
            if (string.IsNullOrWhiteSpace(derFrom))
                throw new ArgumentNullException($"Имя шаблона не может быть пустым");
            if (string.IsNullOrWhiteSpace(basedOn))
                throw new ArgumentNullException($"Имя базового шаблона не может быть пустым");
            if (string.IsNullOrWhiteSpace(shortDesc))
                throw new ArgumentNullException($"Имя краткого описания не может быть пустым");

            TemplateName = tagname;
            DerivedFrom = derFrom;
            BasedOn = basedOn;
            ShortDesc = shortDesc;
            DateCreation = DateTime.Now;
            IsTemplate = true;
            AttributesList = attrs;
            CategoryTag = category;
            Script = script;

        }
    }
}
