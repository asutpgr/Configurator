using System.Collections.Generic;
using System.Xml.Serialization;

namespace Galaxy.Model.Attributes
{
    [XmlRoot(ElementName = "Attribute")]
    public class Attribute
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "DataType")]
        public string DataType { get; set; }
        [XmlAttribute(AttributeName = "Category")]
        public string Category { get; set; }
        [XmlAttribute(AttributeName = "Security")]
        public string Security { get; set; }
        [XmlAttribute(AttributeName = "IsArray")]
        public string IsArray { get; set; }
        [XmlAttribute(AttributeName = "HasBuffer")]
        public string HasBuffer { get; set; }
        [XmlAttribute(AttributeName = "ArrayElementCount")]
        public string ArrayElementCount { get; set; }
        [XmlAttribute(AttributeName = "InheritedFromTagName")]
        public string InheritedFromTagName { get; set; }
    }

    [XmlRoot(ElementName = "UDAInfo")]
    public class UDAInfo
    {
        [XmlElement(ElementName = "Attribute")]
        public List<Attribute> Attribute { get; set; }
    }
}
