using System;
using System.IO;
using System.Collections.Generic;

namespace ExcelHelper
{
    public sealed class ExcelFile
    {
        private string _fullpath;
        private string _filename;
        private string _extension;
        private string _constr;
        public List<string> _sheetnames;
        private List<string> _collists;
        public enum Status { None, Initialized, Ready, Reading}
        public ExcelFile(string fullpath)
        {
            if (string.IsNullOrEmpty(fullpath)) throw new ArgumentNullException(nameof(fullpath));
            FileInfo file = new FileInfo(fullpath);
            if (!file.Exists) throw new Exception("Файл не существует.");
            _extension = file.Extension ;
            _fullpath = file.FullName;
            _filename = file.Name;
            _constr = default(string);
            _sheetnames = new List<string>();
            _collists = new List<string>();
        }
        public string Extension 
        {
            get { return _extension ?? throw new ArgumentNullException(nameof(_extension)); }
            private set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
                _extension = value;
            }
        }
        public string FullPath
        {
            get { return _fullpath ?? throw new ArgumentNullException(nameof(_fullpath)); }
            private set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
                _fullpath = value;
            }
        }
        public string FileName
        {
            get { return _filename ?? throw new ArgumentNullException(nameof(_filename)); }
            private set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
                _filename = value;
            }
        }
        public string ConnectionStr
        {
            get { return _constr ?? throw new ArgumentNullException(nameof(_constr)); }
            set
            {
                if (string.IsNullOrEmpty(value))  throw new ArgumentNullException(nameof(value));
                _constr = value;
            }
        }
        public List<string> ColumnsNameList
        {
            get { return _collists ?? throw new ArgumentNullException(nameof(_collists)); }
            private set 
            {
                if (value == null | value.Count == 0) throw new ArgumentException(nameof(value));
                _collists = value;
            }
         }
        public List<string> SheetNames
        {
            get { return _sheetnames ?? throw new ArgumentNullException(nameof(_sheetnames)); }
            set
            {
                if (value == null | value.Count == 0) throw new ArgumentException(nameof(value));
                _sheetnames = value;
            }
        }
        public bool IsExistSheet(string name)
        {
            int i = 0;
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            foreach (string sheet in _sheetnames)
            {
                if (sheet == name) i++;
            }
            return i >= 1 ? true:false ;
        }
    }
    
}


