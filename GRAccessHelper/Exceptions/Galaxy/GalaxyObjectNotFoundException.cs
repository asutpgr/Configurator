using System;

namespace GRAccessHelper.Exceptions.Galaxy
{
    public class GalaxyObjectNotFoundException : GalaxyExceptions 
    {
        public GalaxyObjectNotFoundException(string objectName) : base($"Объект {objectName} не найден!")
        {

        }
    }
}
