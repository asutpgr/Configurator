using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;
namespace Configurator.Model
{
    public sealed class Instance : GObjectModel
    {
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
            DateCreation = DateTime.Now;
            TagName = tagname;
            DerivedFrom = derFrom;
            BasedOn = basedOn;
            IsTemplate = false;
            ShortDesc = shortDesc;
            AttributesList = attrs;
            IsNew = isnew;
            CategoryTag = category;
            HostName = host;
            AreaName = area;
            ContainerName = container;
            ContainedName = containedname;
            HierarchicalName = $"{HostName ?? null}.{AreaName ?? null}."; //TODO: переделать после создания area,appeng
        }

    }
}
