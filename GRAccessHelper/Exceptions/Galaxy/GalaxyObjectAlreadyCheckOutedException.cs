using System;

namespace GRAccessHelper.Exceptions.Galaxy
{
    public class GalaxyObjectAlreadyCheckOutedException : GalaxyExceptions
    {
        public GalaxyObjectAlreadyCheckOutedException(string objectName) : base($"Объект {objectName} уже редактируется!")
        {

        }
    }
    
}
