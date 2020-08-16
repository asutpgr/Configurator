using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configurator.Model
{
    [Table("Экземпляры")]
    public class Instance 
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя тега не может быть пустым")]
        [Column("Имя тега")]
        public string TagName { get; private set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать время созданич")]
        [Column("Дата создания")]
        public DateTime DateCreation { get; private set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать, от какого шаблона создаг тег")]
        [Column("Название производящего шаблона")]
        public string DerivedFrom { get; private set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать, базовый шаблон")]
        [Column("Базовый шаблон")]
        public string BasedOn { get; private set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать, это экземрпляр или шаблон")]
        public bool IsTemplate { get; private set; }

        [Column("Краткое описание")]
        public string ShortDesc { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать катеорию тега")]
        [Column("Категория")]
        public ECATEGORY CategoryTag { get; private set; }

        [Column("Имя Host")]
        public string HostName { get; set; }

        [Column("Имя Area")]
        public string AreaName { get; set; }

        [Column("Имя контейнера")]
        public string ContainerName { get; set; }

        [Column("Содержит теги")]
        public string ContainedName { get; set; }

        [Column("Иерархическое имя")]
        public string HierarchicalName { get; set; }

        [Column("Список атрибутов")]
        public virtual ICollection<GAttribute> AttributesList { get; set; }

        [Column("Список скриптов")]
        public virtual ICollection<Script> Script { get; set; }
                
        public Instance(string tagname, 
                        string derFrom, 
                        string basedOn, 
                        string host = null, 
                        string area = null,
                        string container = null,
                        string containedname = null, 
                        ECATEGORY category = ECATEGORY.idxCategoryUndefined, 
                        ICollection<GAttribute> attrs = null,
                        string shortDesc = null, 
                        bool isnew = true)
        {
            if (string.IsNullOrWhiteSpace(tagname))
                throw new ArgumentNullException($"Имя тега не может быть пустым");
            if (string.IsNullOrWhiteSpace(derFrom))
                throw new ArgumentNullException($"Имя шаблона не может быть пустым");
            if (string.IsNullOrWhiteSpace(basedOn))
                throw new ArgumentNullException($"Имя базового шаблона не может быть пустым");
            if (string.IsNullOrWhiteSpace(shortDesc))
                throw new ArgumentNullException($"Имя краткого описания не может быть пустым");
            DateCreation = DateTime.Now;
            TagName = tagname;
            DerivedFrom = derFrom;
            BasedOn = basedOn;
            IsTemplate = false;
            ShortDesc = shortDesc;
            AttributesList = attrs;
            CategoryTag = category;
            HostName = host;
            AreaName = area;
            ContainerName = container;
            HierarchicalName = $"{HostName ?? null}.{AreaName ?? null}.";
        }
    }
}
