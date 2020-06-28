using System;

namespace GRAccessHelper.Exceptions.IAttribute
{
    public class AttributeNullReferenceException : AttributeExceptions
    {
        public AttributeNullReferenceException() : base($"Ссылка на экземпляр атрибута равен null или атрибут не существует")
        {

        }
    }
}
