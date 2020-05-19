using System.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace ExcelHelper
{
    using Exceptions;

    public sealed class ExcelFile

    {
        private string fullpath;
        private string filename;
        private string extension;
        private string constr;
        private string sheetnames;
        private string columns;
        private int[]  rows = default;
        private DataTable data;
        private Status state;

        public enum Status { None, Initialized, Ready, ReadCompleted }
        public Status State
        {
            get { return state; }
            set { state = value;}
        }
        public ExcelFile(string fullpath)
        {
            FileInfo file = new FileInfo(fullpath);
            ExcelFileException.ThrowIfFileNotExsist(fullpath, file);
            extension = file.Extension;
            this.fullpath = file.FullName;
            filename = file.Name;
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
        public string SheetNames
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(sheetnames)); return sheetnames; } // установить через делегат и сделать private
            set
            {
                ExcelFileException.ThrowIfStringNull(nameof(sheetnames), (object)value);
                sheetnames = value;
            }
        }
        public int[] Rows 
        {
            get { return rows;}
            set
            {
                foreach (var item in value)
                    if (item < 0) throw new ExcelFileException($"индексы должны быть больше/равны нулю");
                rows = value;
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
        public DataTable GetElements(int from_col, int from_row, int to_col, int to_row) // с (row,col) по (row,col)
        {
            var tablefind = new DataTable() { TableName = "srch" };
            for (int i = from_row; i <= to_row; i++)
            {
                var row = tablefind.NewRow();
                for (int j = from_col; j <= to_col; j++)
                {
                    if (tablefind.Columns.Count != (to_col - from_col + 1))
                        tablefind.Columns.Add(new DataColumn($"{data.Columns[from_col + j].ColumnName}"));
                    row[j-from_col]= data.Rows[i].ItemArray[j].ToString();
                }
                tablefind.Rows.Add();
            }
            return tablefind;
        }
        public string GetElement(object col, int row)
        {
            ExcelFileException.ThrowIfStringNull((string)col);
            if ((int)col < 0 | row < 0) throw new ExcelFileException($"Столбцы могут быть только положительные.");
            string res = null;
            switch (col.GetType().ToString())
            {
                case "System.String":
                { 
                    if (IsExistColumn((string)col))
                    {
                        res = data.Rows[row][(string)col].ToString();
                    }
                    break;
                }
                case "System.Int32":
                {
                    res = data.Rows[row][(int)col].ToString();
                    break;
                }
                default: 
                    throw new Exception($"Форма колонки не правильно задан.");
            }
            return res;
        } 

       
        // реализовать преобразование таблицы с названием нужных столбцов 
    }
}


