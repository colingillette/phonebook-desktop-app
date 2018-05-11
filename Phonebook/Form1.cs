using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phonebook
{
    public partial class Form1 : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public Form1()
        {
            InitializeComponent();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            db = new DataClasses1DataContext();

            dataGridView1.DataSource = db.Phones;
            dataGridView1.Columns["dbName"].Visible = true;
            dataGridView1.Columns["dbNumber"].Visible = true;
            dataGridView1.Columns["Name1"].Visible = false;
            dataGridView1.Columns["Number"].Visible = false;
            saveButton.Enabled = false;
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns["dbName"].Visible = false;
            dataGridView1.Columns["dbNumber"].Visible = false;
            dataGridView1.Columns["Name1"].Visible = true;
            dataGridView1.Columns["Number"].Visible = true;
            saveButton.Enabled = true;
            dataGridView1.Refresh();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Phone[] newNumber = new Phone[dataGridView1.RowCount - 1];
            for (int i = 0; i < newNumber.Length; i++)
            {
                newNumber[i] = new Phone();
                newNumber[i].Name = dataGridView1[3, i].Value.ToString();
                newNumber[i].Number = dataGridView1[4, i].Value.ToString();
                db.Phones.InsertOnSubmit(newNumber[i]);
            }
            db.SubmitChanges();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter.Fill(this.database1DataSet.Table);

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;

            var query = from phone in db.Phones
                        where phone.Name.Contains(search) || phone.Number.Contains(search)
                        select phone;

            dataGridView1.DataSource = query;
            dataGridView1.Refresh();
        }
    }
}
