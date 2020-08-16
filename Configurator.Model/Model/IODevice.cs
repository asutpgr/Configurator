using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.Model.Model
{
    public class IODevice
    {
        public string Path { get; set; }
        public TypeIO TypeIO { get; set; }
    }

    public enum TypeIO
    {
        Read,
        Write,
        Both
    }
}
