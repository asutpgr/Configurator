using System;

namespace GRAccessHelper.Exceptions.IAttribute
{
    using ArchestrA.GRAccess;
    public class AttributeExceptions : Exception
    {
        public static void ThrowIfNoSuccess(ICommandResult res, string message = null)
        {
            if (res == null) return;
            if (!res.Successful) throw new AttributeExceptions(message, res);
        }
        public AttributeExceptions() : base()
        { }

        public AttributeExceptions(string message) : base(message)
        { }    

        public AttributeExceptions(ICommandResult res) : this("", res)
        { }

        public AttributeExceptions(string message, ICommandResult res) : base(message + " (" + ((res?.Text + " - ")) + (res?.CustomMessage ?? "") + ")")
        { }    
    }
}
