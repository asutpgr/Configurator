using System;

namespace GRAccessHelper.Exceptions.IgObject
{
    public class IgObjectsNullReferenceExceptions : IgObjectExceptions
    {
        public IgObjectsNullReferenceExceptions() : base($"Ссылка на объекты Галактики не сущесствует.")
        { }
    }
}
