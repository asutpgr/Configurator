using System;

namespace GRAccessHelper.Exceptions.Galaxy
{
    public class GalaxyNullReferenceException : GalaxyExceptions
    {
        public GalaxyNullReferenceException() : base($"Ссылка на экземпляр Галактики равен null(возможно вход в глактику не выполнен или был неудачен)!")
        {

        }
        public GalaxyNullReferenceException(string msg) : base(msg)
        {

        }
    }
}
