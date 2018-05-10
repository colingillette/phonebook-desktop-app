using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpProject1
{
    public partial class Form1 : Form
    {
        enum View {View, Edit, Insert};
        View currentView;
        DataClasses1DataContext db = new DataClasses1DataContext();
        public Form1()
        {
            InitializeComponent();
        }

        private void insertToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            currentView = View.Insert;
            dataGridView1.DataSource = null;
            dataGridView1.Columns["firstname"].Visible = false;
            dataGridView1.Columns["phone"].Visible = false;
            dataGridView1.Columns["name"].Visible = true;
            dataGridView1.Columns["Number"].Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            button1.Enabled = true;
            dataGridView1.Refresh();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentView != View.View)
            {
                dataGridView1.DataSource = db.PhoneNumbers;
                dataGridView1.Columns["firstname"].Visible = true;
                dataGridView1.Columns["phone"].Visible = true;
                dataGridView1.Columns["name"].Visible = false;
                dataGridView1.Columns["Number"].Visible = false;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            PhoneNumber[] newNumbers = new PhoneNumber[dataGridView1.RowCount - 1];
            for (int i = 0; i < newNumbers.Length; i++ )
            {
                newNumbers[i] = new PhoneNumber();
                newNumbers[i].firstname = dataGridView1[3, i].Value.ToString();
                newNumbers[i].phone = dataGridView1[4, i].Value.ToString();
                db.PhoneNumbers.InsertOnSubmit(newNumbers[i]);
            }
            db.SubmitChanges();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void search_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var dbquery =
                from phone in db.PhoneNumbers
                where (phone.firstname.Contains(textBox1.Text) ||
                phone.phone.Contains(textBox1.Text))
                select phone;
            int i = 0;
            foreach (PhoneNumber p in dbquery)
            {
                dataGridView1.Rows.Add();
                dataGridView1[0, i].Value = p.Id;
                dataGridView1[1, i].Value = p.firstname;
                dataGridView1[2, i].Value = p.phone;
                i++;
            }
        }
    }
}