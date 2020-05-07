using System;
using ExcelHelper;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Configurator
{
    
    public partial class Form1 : Form
    {
        ExcelFile excel;
        TaskScheduler _sh;
        SynchronizationContext _sc;
        public Form1()
        {
            InitializeComponent();
            _sh = TaskScheduler.FromCurrentSynchronizationContext();
            _sc = SynchronizationContext.Current;
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
            
            try
            {
                OleDb.GenerateConnectionString(excel);
                OleDb.GetSheetsNames(excel);
                var res = await Task.Run(() => OleDb.ReadData(excel, txtnmlst.Text));
                BeginInvoke(new MethodInvoker(() => dataGridView1.DataSource = res));
            }
            catch (Exception ex)
            {
                string message = $"Произошал ошибка. Сообщение:{ex.Message}.";
                MessageBox.Show(message);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
    }
}
