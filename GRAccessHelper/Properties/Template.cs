using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;

namespace Configurator.Model
{
    public sealed class Template : GObjectModel
    {
        public Template(string tagname, 
                        string derFrom, string basedOn, ECATEGORY category = ECATEGORY.idxCategoryUndefined, 
                        ICollection<GAttribute> attrs = null, string shortDesc = null, bool isnew = true )
        {
            DateCreation = DateTime.Now;
            TagName = tagname;
            DerivedFrom = derFrom;
            BasedOn = basedOn;
            IsTemplate = true;
            ShortDesc = shortDesc;
            AttributesList = attrs;
            IsNew = isnew;
            CategoryTag = category;
            HostName = null;
            AreaName = null;
            ContainerName = null;
            ContainedName = null;
        }
      
    }
}
