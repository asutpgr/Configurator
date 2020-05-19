using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data;

namespace Configurator
{
    using ExcelHelper;
    using System.Drawing;

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
        private void OleDb_ReadDataFinished(object sender, ReadDataEventArgs args)
        {
            if (!string.IsNullOrEmpty(txtRowName.Text))
            {
                int row = int.Parse(txtRowName.Text);
                for (var i = 0; i < args.Data.Columns.Count; i++)
                {
                    try
                    {
                       args.Data.Columns[i].ColumnName = args.Data.Rows[row].ItemArray[i].ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        args.Data.Columns[i].ColumnName = $"{args.Data.Rows[row].ItemArray[i]}{i}";
                    }
                }
                for (var i = 0; i <= row; i++)
                {
                    args.Data.Rows[i].Delete();
                }
            }
            excel.Data = args.Data;
            foreach (DataColumn col in args.Data.Columns)
            {
                excel.ColumnsNameList += $"{col.ColumnName};";
            }
            excel.State = ExcelFile.Status.ReadCompleted;
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
            OleDb.GenerateConnectionString(excel);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            label1.Text = openFileDialog1.FileName;
        }
        private void cmbProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDb.OleDbProvider = cmbProv.SelectedItem.ToString();
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            OleDb.GetSheetsNames(excel);
            var table = await Task.Run(() => OleDb.ReadData(excel, txtnmlst.Text));

            dataGridView1.DataSource = excel.Data;
           
            for (var i = 0; i < dataGridView1.Columns.Count;i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Gray;
                dataGridView1.Columns[i].HeaderCell.Style.ForeColor = Color.DarkGray;
            }
            for (var i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }

            lstColNames.Items.Clear();
            foreach(DataColumn col in excel.Data.Columns)
                lstColNames.Items.Add(col.ColumnName);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            int res = 1;
            label2.Text = res.GetType().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }
    }
}
