using System;

namespace GRAccessHelper.Exceptions.Instance
{
    using ArchestrA.GRAccess;
    public class InstanceException : Exception
    {
        public static void ThrowIfNoSuccess(ICommandResult res, string message = null)
        {
            if (res == null) return;
            if (!res.Successful) throw new InstanceException(message, res);
        }
        public InstanceException() : base()
        { }

        public InstanceException(string message) : base(message)
        { }

        public InstanceException(ICommandResult res) : this("", res)
        { }

        public InstanceException(string message, ICommandResult res) : base(message + " (" + ((res?.Text + " - ")) + (res?.CustomMessage ?? "") + ")")
        { }
    }
}
