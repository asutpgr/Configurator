
namespace Configurator.Model
{
    public class GAttribute
    {
       
        public GAttribute (string attr_name, object value, GDataTypeEnum datatype, bool isuda = true,string attr_prname = null)
        {
            AttributeName = attr_name;
            Value = value;
            DataType = datatype;
            IsUDA = isuda;
            PrimitiveName = attr_name;
            FullName = PrimitiveName + $".{AttributeName}";
        }
        public string FullName { get; private set; } 
        public string PrimitiveName { get; private set; }
        public string AttributeName { get; private set; }
        public object Value { get; private set; }
        public GDataTypeEnum DataType { get; private set; }
        public bool IsUDA { get; private set; }
        public string TagName { get; private set; }
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
