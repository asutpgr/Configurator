using System.IO;
namespace ExcelHelper
{
    using Exceptions;
    public sealed class ExcelFile
    {
        private string _fullpath;
        private string _filename;
        private string _extension;
        private string _constr;
        private string _sheetnames;
        private string _collists;
        public enum Status { None, Initialized, Ready, Reading}
        public ExcelFile(string fullpath)
        {
            ExcelFileException.ThrowIfFileNotExsist(fullpath, out FileInfo file);
            _extension = file.Extension ;
            _fullpath = file.FullName;
            _filename = default(string);
            _constr = default(string);
            _sheetnames = default(string);
            _collists = default(string); 
        }
        public string Extension 
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(_extension)); return _extension; }
            private set 
            {
                ExcelFileException.ThrowIfStringNull(nameof(_extension), (object)value);
                _extension = value;
            }
        }
        public string FullPath
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(_fullpath)); return _fullpath; }
            private set
            {
                ExcelFileException.ThrowIfStringNull(nameof(_fullpath), (object)value);
                _fullpath = value;
            }
        }
        public string FileName
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(_filename)); return _filename; }
            private set
            {
                ExcelFileException.ThrowIfStringNull(nameof(_filename), (object)value);
                _filename = value;
            }
        }
        public string ConnectionStr 
        {
            get { ExcelFileException.ThrowIfStringNull(nameof(_constr)); return _constr; } // установить через делегат и сделать private
            set 
            {
                ExcelFileException.ThrowIfStringNull(nameof(_constr), (object)value);
                _constr = value;
            }
        }
        public string ColumnsNameList
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(_collists)); return _collists; } // установить через делегат и сделать private
            set 
            {
                ExcelFileException.ThrowIfStringNull(nameof(_collists), (object)value);
                _collists = value;
            }
         }
        public string SheetNames
        {
            get { ExcelFileException.ThrowIfObjNull(nameof(_sheetnames)); return _sheetnames; } // установить через делегат и сделать private
            set 
            { 
                ExcelFileException.ThrowIfStringNull(nameof(_sheetnames),(object) value);
                _sheetnames = value;
            }
        }
        public bool IsExistSheet(string name)
        {
            int i = 0;
            ExcelFileException.ThrowIfStringNull(name);     
            foreach (string sheet in _sheetnames.Split(';'))
                if (sheet == name) i++;
            return i > 0 ? true : false;
        }

        public bool IsExistColumn(string name)
        {
            int i = 0;
            ExcelFileException.ThrowIfStringNull(name);
            foreach (string colname in _collists.Split(';'))
                if (colname == name) i++;
            return i > 0 ? true : false;
        }

        
    }
 }


