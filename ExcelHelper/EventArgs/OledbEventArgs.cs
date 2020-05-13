using ExcelHelper.Exceptions;
using System.Data;
namespace ExcelHelper
{
    public class ReadDataEventArgs
    {
        public DataTable Data { get; private set; }
        public string SheetName 
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(SheetName)); return SheetName; }
            private set
            {
                ExcelFileException.ThrowIfStringNull(nameof(SheetName),(object)value);
            }
        }
        public string ExcelFileName 
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(ExcelFileName)); return ExcelFileName; } 
            private set
            {
                ExcelFileException.ThrowIfStringNull(nameof(ExcelFileName), (object)value);
            }
        }

        public ReadDataEventArgs(string name_sh, string name_lst, DataTable data)
        {
            SheetName = name_sh;
            ExcelFileName = name_lst;
            Data = data;
        }
    }
}