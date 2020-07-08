using System;
using ArchestrA.GRAccess;
namespace GRAccessHelper.Exceptions.Galaxy
{
    public class GalaxyObjectDeployException : GalaxyExceptions
    {
        public GalaxyObjectDeployException(string name, ICommandResult commandResult) :
                                base($"Ошибка при деплое/андеплое экземпляра {name}.", commandResult)
        {
        }
        public GalaxyObjectDeployException(ICommandResults commandResults) :
                        base($"Ошибка при деплое/андеплое коллекции объектов или части коллекции.")
        {
        }
    }
}
