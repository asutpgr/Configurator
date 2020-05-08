using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace ExcelHelper
{
    using Exceptions;
    public static class OleDb
    {
        private static string _ole_providers;
        public static string ExcelProvider
        {
            get { return _ole_providers ?? throw new ArgumentNullException(nameof(_ole_providers)); }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
                _ole_providers = value;
            }
        }
        public static List<string> GetOleDBProviders()
        {
            OleDbDataReader reader = OleDbEnumerator.GetRootEnumerator();
            DataTable dt = new DataTable();
            dt.Load(reader);
            List<string> list = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["SOURCES_NAME"].ToString());
            }
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
                   strcon.Add("Provider", $"{ExcelProvider}");
                   strcon.Add("Extended Properties", "Excel 12.0;HDR=YES;IMEX=1");
                   break;     
                }
                case ".xls":
                {
                    strcon.Add("Provider", $"{ExcelProvider}");
                    strcon.Add("Extended Properties", "Excel 8.0;HDR=YES;IMEX=1");
                    break;
                }
                default:
                    throw new Exception("Неверный формат файла.");
            }
            strcon.Add("Data Source",excel.FullPath);
            excel.ConnectionStr = strcon.ToString();
            return strcon.ToString();
        }
        public static void GetSheetsNames(ExcelFile excel)
        {
            using (OleDbConnection connection = new OleDbConnection(excel.ConnectionStr))
            {
                OleDbCommand cmd = new OleDbCommand() { Connection = connection };
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    DataTable sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    for (var i = 0; i < sheets.Rows.Count; i++)
                        excel.SheetNames += $"{sheets.Rows[i]["TABLE_NAME"]};";
                }
                connection.Close();
            }
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
                    OleDbCommand cmd = new OleDbCommand() { Connection = connection, CommandText = $"SELECT *FROM [{sheetname}]" };
                    OleDbDataReader reader = cmd.ExecuteReader();
                    res.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }
            return res;
        }
    }
}
