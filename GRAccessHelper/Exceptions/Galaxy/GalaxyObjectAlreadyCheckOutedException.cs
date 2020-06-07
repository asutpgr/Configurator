using System;

namespace GRAccessHelper.Exceptions
{
    public class GalaxyObjectAlreadyCheckOutedException : GalaxyExceptions
    {
        public GalaxyObjectAlreadyCheckOutedException(string objectName) : base($"Для объекта {objectName} уже выполнен CheckOut!")
        {

        }
    }
    
}
