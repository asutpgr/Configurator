using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;

namespace Configurator.Model
{
    public class GObjectModel
    {
        public string TagName { get; protected set; }
        public  DateTime DateCreation { get; protected set; }
        public  string DerivedFrom { get; protected set; }
        public  string BasedOn { get; protected set; }
        public  bool IsTemplate { get; protected set; }
        public  string ShortDesc { get;  set; }
        public  ECATEGORY? CategoryTag { get; protected set; }
        public virtual ICollection<GAttribute> AttributesList { get; set; }
        public  bool IsNew { get; protected set; }
        public  string IODevie { get; set; }
        public  string HostName { get;  set; }
        public  string AreaName { get; set; }
        public  string ContainerName { get; set; }
        public  string HierarchicalName { get; set; }
        public  string ContainedName { get; set; }
        public Extensions Extensions { get; set; }
        
    }
    
}

