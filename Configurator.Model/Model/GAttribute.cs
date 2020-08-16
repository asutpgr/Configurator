using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configurator.Model
{
    [Table("Аттрибуты")]
    public class GAttribute
    { 
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Полный путь не может быть пустым")]
        [Column("Полный путь")]
        public string FullPath { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Полное имя не может быть пустым")]
        [Column("Полное имя")]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя атрибута не может быть пустым")]
        [Column("Имя атрибута")]
        public string AttributeName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя примитива не может быть пустым")]
        [Column("Имя примитива")] // o(primitive).val(AttributeName))
        public string PrimitiveName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Категория не может быть пустым")]
        [Column("Категория")]
        public GDataTypeEnum DataType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать атрибут пользовательский или системный")]
        [Column("IsUDA")]
        public bool IsUDA { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать атрибут унаслеован или принадлежит шблону")]
        [Column("IsInherited")]
        public bool IsInheritated { get;  set; }


        [ForeignKey("Instance")]
        [Column("Имя содержащего экземпляра")]
        public string InstanceName { get; set; }

        [ForeignKey("Template")]
        [Column("Имя содержащего шаблона")]
        public string TemplateName { get; set; }

        public Template Template { get; set; }
        public Instance Instance { get; set; }


        public GAttribute(string attr_name,
                          GDataTypeEnum datatype, 
                          bool isuda,
                          string primitive_name = null)
        {
            if (string.IsNullOrWhiteSpace(attr_name))
                throw new ArgumentNullException($"Имя атрибута не может быть пустым");
            if (string.IsNullOrWhiteSpace(primitive_name))
                throw new ArgumentNullException($"Имя примитива не может быть пустым");

            AttributeName = attr_name;
            PrimitiveName = primitive_name;
            DataType = datatype;
            IsUDA = isuda;

            if (PrimitiveName == null)
               FullName = AttributeName;   
            else
               FullName = $"{PrimitiveName}.{AttributeName}";

            if (InstanceName != null)
                FullPath = $"{InstanceName}.{FullName}";
            else 
                if (Template != null)
                    FullPath = $"{TemplateName}.{FullName}";
                else 
                    throw new ArgumentException("Поля InstanceName и TemplateName не могут принимать значение однвременно.");
        }
    }
    public enum GDataTypeEnum
    {
        #region Названия типов даннх
        Boolean,
        Integer,
        Float,
        Double,
        String,
        NoData,
        Time,
        ElapsedTime,
        ReferenceType,
        StatusType,
        DataTypeEnum,
        SecurityClassificationEnum,
        DataQualityType,
        QualifiedEnum,
        QualifiedStruct,
        InternationalizedString,
        BigString,
        DataTypeEND,
        Unknown
        #endregion
    }

}
