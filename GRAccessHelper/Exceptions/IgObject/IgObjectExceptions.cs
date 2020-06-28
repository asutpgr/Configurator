using System;

namespace GRAccessHelper.Exceptions.IgObject
{

    using ArchestrA.GRAccess;
    public class IgObjectExceptions : Exception
    {
        public static void ThrowIfNoSuccess(ICommandResult res, string message = null)
        {
            if (res == null) return;
            if (!res.Successful) throw new IgObjectExceptions(message, res);
        }
        public IgObjectExceptions() : base()
        { }

        public IgObjectExceptions(string message) : base(message)
        { }

        public IgObjectExceptions(ICommandResult res) : this("", res)
        { }

        public IgObjectExceptions(string message, ICommandResult res) : base(message + " (" + ((res?.Text + " - ")) + (res?.CustomMessage ?? "") + ")")
        { }
    }
}
