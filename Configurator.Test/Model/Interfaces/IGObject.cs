using ArchestrA.GRAccess;
using GRAccessHelper.Attribute;

namespace Configurator.Test.Model
{
    public interface IGObject
    {
        string TagName { get; set; }
        string DerivedFrom { get; set; }
        string BasedOn { get; set; }
        bool IsTemplate { get; set; }
        UDAInfo UDA { get; set; }
        string ShortDesc { get; set; }
        ECATEGORY Category { get; set; }

        
    }
}
