using System;
using ArchestrA.GRAccess;
namespace GRAccessHelper.Exceptions.Galaxy
{
    
    public class GalaxyExceptions : Exception
    {
        public GalaxyExceptions() : base()
        { }

        public GalaxyExceptions(string message) : base(message)
        { }

        public GalaxyExceptions(ICommandResult res) : this("", res)
        { }

        public GalaxyExceptions(string message, ICommandResult res) : base(message + " (" + ((res?.Text + " - ")) + (res?.CustomMessage ?? "") + ")")
        { }
   

        public static void ThrowIfNoSuccess(ICommandResult res, string message = null)
        {
            if (res == null) return;
            if (!res.Successful) throw new GalaxyExceptions(message, res);
        }
    }
}
