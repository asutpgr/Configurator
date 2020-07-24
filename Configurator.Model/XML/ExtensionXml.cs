using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Configurator.Model.XML
{
    [XmlRoot(ElementName = "Extension")]
    public class Extension
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ExtensionType")]
        public string ExtensionType { get; set; }

        [XmlAttribute(AttributeName = "InheritedFromTagName")]
        public string InheritedFromTagName{ get; set; }
    }
    [XmlRoot(ElementName = "ObjectExtension")]
    public class ObjectExtension
    {
        [XmlElement(ElementName ="Extension")]
        public List<Extension> Extension { get; set; }
    }
    [XmlRoot(ElementName = "ExtensionInfo")]
    public class ExtensionInfo
    {
        [XmlElement(ElementName = "ObjectExtension")]
        public ObjectExtension ObjectExtension { get; set; }
    }
}
