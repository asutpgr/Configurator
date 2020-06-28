using System;
using ArchestrA.GRAccess;
namespace GRAccessHelper.Exceptions.Galaxy
{
    public class GalaxyCannotCreateInstanceException : GalaxyExceptions
    {
        public GalaxyCannotCreateInstanceException(string name, string msg, ITemplate template, ICommandResult commandResult) :
                                base($"Ошибка создания экземпляра {name} на основе шаблона {template?.Tagname}.{msg}", commandResult)
        {
        }
    }
}
