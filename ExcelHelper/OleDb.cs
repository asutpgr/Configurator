using System;
using System.Data;
using System.Data.OleDb;

namespace ExcelHelper
{
    using Exceptions;
    public class OleDb 
    {
        public static event EventHandler<string> GetSheetsNameCompleted;
        public static event EventHandler<string[]> GetOleDbProvidersCompleted;
        public static event EventHandler<string> ConnectionStringGenerated;
        public static event EventHandler<ReadDataEventArgs> ReadDataFinished;
        public static event EventHandler<DataTable> GetElementsCompleted;
        private static string _ole_providers;
                
        public static string OleDbProvider
        {
            get { return _ole_providers ?? throw new ArgumentNullException(nameof(_ole_providers)); }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
                _ole_providers = value;
            }
        }
        public static string GetOleDBProviders()
        {
            OleDbDataReader reader = OleDbEnumerator.GetRootEnumerator();
            DataTable dt = new DataTable();
            dt.Load(reader);
            string list = null;
            for (int i = 0; i < dt.Rows.Count; i++)
                list += $"{dt.Rows[i]["SOURCES_NAME"].ToString()};";
            string[] mlist = list.Split(';');
            GetOleDbProvidersCompleted?.Invoke(null, mlist);
            return list;
        }
        public static string GenerateConnectionString(ExcelFile excel)
        {
            if (excel == null) throw new ExcelFileException($"Ссылна на объект {nameof(excel)} равна NULL ");
            string ext = excel.Extension.ToLower();
            OleDbConnectionStringBuilder strcon = new OleDbConnectionStringBuilder();
            switch (ext)  
            {
                case ".xlsx":
                {
                   strcon.Add("Provider", $"{OleDbProvider}");
                   strcon.Add("Extended Properties", "Excel 12.0;HDR=YES;IMEX=1");
                   break;     
                }
                case ".xls":
                {
                    strcon.Add("Provider", $"{OleDbProvider}");
                    strcon.Add("Extended Properties", "Excel 8.0;HDR=YES;IMEX=1");
                    break;
                }
                default:
                    throw new Exception("Неверный формат файла.");
            }
            strcon.Add("Data Source",excel.FullPath);
            ConnectionStringGenerated?.Invoke(null, strcon.ToString());
            return strcon.ToString();
        }
        public static string GetSheetsNames(ExcelFile excel)
        {
            string sheets = null;
            using (OleDbConnection connection = new OleDbConnection(excel.ConnectionStr))
            {
                OleDbCommand cmd = new OleDbCommand() { Connection = connection };
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    DataTable dtsheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    for (var i = 0; i < dtsheets.Rows.Count; i++)
                        sheets += $"{dtsheets.Rows[i]["TABLE_NAME"]};";
                }
                connection.Close();
            }
            GetSheetsNameCompleted?.Invoke(null, sheets);
            return sheets;
        }
        public static DataTable ReadData(ExcelFile excel,string sheetname)
        {
            sheetname += "$";
            ExcelFileException.ThrowIfSheetNotExist(excel, sheetname);
            var res = new DataTable();
            using (var connection = new OleDbConnection(excel.ConnectionStr))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var cmd = new OleDbCommand() { Connection = connection, CommandText = $"SELECT *FROM [{sheetname}]" };
                    OleDbDataReader reader = cmd.ExecuteReader();
                    res.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }
            ReadDataFinished?.Invoke(null, new ReadDataEventArgs(sheetname, excel.FileName,res)); 
            return res;
        }
        
    }
}
