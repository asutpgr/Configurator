using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data;

namespace Configurator
{
    using ExcelHelper;

    public partial class Form1 : Form
    {
        ExcelFile excel = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            OleDb.GetOleDbProvidersCompleted += OleDb_GetOleDbProvidersCompleted;
            OleDb.ConnectionStringGenerated += OleDb_ConnectionStringGenerated;
            OleDb.GetSheetsNameCompleted += OleDb_GetSheetsNameCompleted;
            OleDb.ReadDataFinished += OleDb_ReadDataFinished;


            OleDb.GetOleDBProviders();
        }

        private void OleDb_ReadDataFinished(object sender, ReadDataEventArgs e)
        {
            excel.State = ExcelFile.Status.ReadCompleted;
            excel.Data = e.Data;
            foreach (DataColumn col in e.Data.Columns)
                excel.ColumnsNameList += $"{col.ColumnName};";
        }

        private void OleDb_GetSheetsNameCompleted(object sender, string e)
        {
            excel.SheetNames = e;
        }

        private void OleDb_ConnectionStringGenerated(object sender, string e)
        {
            excel.ConnectionStr = e;
        }

        private void OleDb_GetOleDbProvidersCompleted(object sender, string[] e)
        {
            cmbProv.Items.AddRange(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                excel = new ExcelFile(openFileDialog1.FileName);
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            label1.Text = openFileDialog1.FileName;
        }
        private void cmbProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDb.OleDbProvider = cmbProv.SelectedItem.ToString();
            OleDb.GenerateConnectionString(excel);
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
               OleDb.GetSheetsNames(excel);
               dataGridView1.DataSource = await Task.Run(() => OleDb.ReadData(excel, txtnmlst.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
