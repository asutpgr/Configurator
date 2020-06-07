using System;
using ArchestrA.GRAccess;
namespace GRAccessHelper.Exceptions
{
    public class GalaxyCannotCreateInstanceException : GalaxyExceptions
    {
        public GalaxyCannotCreateInstanceException(string name, ITemplate template, ICommandResult commandResult) :
           base($"Ошибка создания экземпляра {name} на основе шаблона {template?.Tagname}", commandResult)
        {

        }
    }
}
