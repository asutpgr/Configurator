using System.IO;

namespace ExcelHelper
{
    using Exceptions;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class ExcelFile
    {
        private string fullpath;
        private string filename;
        private string extension;
        private string constr;
        private string sheetnames;
        private string columns;
        private DataTable data;
        private Status state;
       
        public enum Status { None, Initialized, Ready, ReadCompleted }
        public Status State
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(state)); return State; }
            set
            {
                ExcelFileException.ThrowIfStringNull(nameof(state),(object) value);
                state = value;
            }
        }
        public ExcelFile(string fullpath)
        {
            FileInfo file = new FileInfo(fullpath);
            ExcelFileException.ThrowIfFileNotExsist(fullpath, file);
            extension = file.Extension ;
            this.fullpath = file.FullName;
            filename = default(string);
            constr = default(string);
            sheetnames = default(string);
            columns = default(string);
            rows = default(string);
            data = null;
            State = Status.Initialized;
        }
        public string Extension 
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(extension)); return extension; }
            private set 
            {
                ExcelFileException.ThrowIfStringNull(nameof(extension), (object)value);
                extension = value;
            }
        }
        public string FullPath
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(fullpath)); return fullpath; }
            private set
            {
                ExcelFileException.ThrowIfStringNull(nameof(fullpath), (object)value);
                fullpath = value;
            }
        }
        public string FileName
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(filename)); return filename; }
            private set
            {
                ExcelFileException.ThrowIfStringNull(nameof(filename), (object)value);
                filename = value;
            }
        }
        public string ConnectionStr 
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(constr)); return constr; } // установить через делегат и сделать private
            set 
            {
                ExcelFileException.ThrowIfStringNull(nameof(constr), (object)value);
                constr = value;
                State = Status.Ready;
            }
        }
        public string ColumnsNameList
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(columns)); return columns; }
            set 
            {
                ExcelFileException.ThrowIfStringNull(nameof(columns), (object)value);
                columns = value;
            }
        }
        public string Rows
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(rows)); return rows; }
            set
            {
                ExcelFileException.ThrowIfStringNull(nameof(rows), (object)value);
                columns = value;
            }
        }
        public string SheetNames
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(sheetnames)); return sheetnames; } // установить через делегат и сделать private
            set 
            { 
                ExcelFileException.ThrowIfStringNull(nameof(sheetnames),(object) value);
                sheetnames = value;
            }
        }
        public DataTable Data
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(data)); return data; }
            set
            {
                ExcelFileException.ThrowIfObjNull(value);
                data = value;
            }
        }
        public bool IsExistSheet(string name)
        {
            int i = 0;
            ExcelFileException.ThrowIfStringNull(name);     
            foreach (string sheet in sheetnames.Split(';'))
                if (sheet == name) i++;
            return i > 0 ? true : false;
        }
        public bool IsExistColumn(string name)
        {
            int i = 0;
            ExcelFileException.ThrowIfStringNull(name);
            foreach (string colname in columns.Split(';'))
                if (colname == name) i++;
            return i > 0 ? true : false;
        }
        public List<List<string>> GetElements(int from_col, int from_row, int to_col, int to_row) // создать перегрузки
        {
            var res = new List<List<string>>();
            for (int i = from_row; i <= to_row; i++)
            {
                List<string> item = new List<string>();
                for (int j = from_col; j <= to_col; j++)
                {
                    item.Add(Data.Rows[i].ItemArray[j].ToString());
                }
                res.Add(item);
            }
            return res;
        }
        public string GetElements(object col, int row) { string res = null; return res; } // реализовать поиск элемента по индексу
  
    }
 }


