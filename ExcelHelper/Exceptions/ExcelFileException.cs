using System;
using System.IO;

namespace ExcelHelper.Exceptions
{
    public sealed class ExcelFileException : Exception 
    {
        public ExcelFileException() : base()
        { }
        public ExcelFileException(string message) :base(message)
        {
            message = Message;
        }
        public ExcelFileException(string message, Exception inner) : base(message,inner) // реализовать по необходимости
        { }
       
        public static void ThrowIfStringNull(string param_name, object setvalue) 
        {
            if (string.IsNullOrEmpty((string)setvalue))  
                throw new ExcelFileException($"Присвоение в {param_name} значения равного NULL или Empty"); 
        }
        public static void ThrowIfStringNull(object valstr)
        {
            if (string.IsNullOrEmpty((string)valstr))
                throw new ExcelFileException($"Значение {valstr} равно NULL или Empty");
        }
        public static void ThrowIfObjNull(object obj)
        {
            if (obj == null) throw new ExcelFileException($"Ссылка на экземпляр {obj} равен NULL");
        }
        public static void ThrowIfFileNotExsist(string path, out FileInfo ofile)
        {
            ThrowIfStringNull(path);
            ofile = new FileInfo(path);
            if (!ofile.Exists) throw new ExcelFileException($"Файл <{ofile.FullName}> не существует");
        }
        public static void ThrowIfSheetNotExist(ExcelFile file,string name)
        {
            ThrowIfStringNull(name);
            ThrowIfObjNull(file);
            if (!file.IsExistSheet(name)) throw new ExcelFileException($"Таблица {name} не существует в файле {file.FileName}.");
        }

       
    }
}
