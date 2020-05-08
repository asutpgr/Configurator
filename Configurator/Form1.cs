using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace Configurator
{
    using ExcelHelper;
    using ExcelHelper.Exceptions;

    public partial class Form1 : Form
    {
        ExcelFile excel;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbProv.Items.AddRange(OleDb.GetOleDBProviders().ToArray());
            
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
            OleDb.ExcelProvider = cmbProv.SelectedItem.ToString();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            OleDb.GenerateConnectionString(excel);
            OleDb.GetSheetsNames(excel);
            dataGridView1.DataSource = await Task.Run(() => OleDb.ReadData(excel, txtnmlst.Text));
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }
    }
}
