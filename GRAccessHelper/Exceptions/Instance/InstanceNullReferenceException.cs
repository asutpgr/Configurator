using System;

namespace GRAccessHelper.Exceptions.Instance
{ 
    public class InstanceNullReferenceException : InstanceException
    {
        public InstanceNullReferenceException() : base($"Ссылка на экземпляр объекта равен null или объект не существует")
        {

        }
    }
}