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
        // Allows access to database tools
        DataClasses1DataContext db = new DataClasses1DataContext();

        public Form1()
        {
            InitializeComponent();
        }

        // Will allow user to see the current full database on click
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instantiate Database
            db = new DataClasses1DataContext();

            // Fill View with actively stored phone numbers
            dataGridView1.DataSource = db.Phones;
            dataGridView1.Columns["dbName"].Visible = true;
            dataGridView1.Columns["dbNumber"].Visible = true;
            dataGridView1.Columns["Name1"].Visible = false;
            dataGridView1.Columns["Number"].Visible = false;
            saveButton.Enabled = false;
        }

        // Allows the user to insert new items into the database to be seen on next View click
        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Disconnect from the database and clear in a way to allow inserts
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Initialize columns that communicate to the database
            dataGridView1.Columns["dbName"].Visible = false;
            dataGridView1.Columns["dbNumber"].Visible = false;
            dataGridView1.Columns["Name1"].Visible = true;
            dataGridView1.Columns["Number"].Visible = true;
            saveButton.Enabled = true;

            // Refresh the databsae
            dataGridView1.Refresh();
        }

        // Saves the data that has been inserted
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Save each phone object to the database
            Phone[] newNumber = new Phone[dataGridView1.RowCount - 1];
            for (int i = 0; i < newNumber.Length; i++)
            {
                newNumber[i] = new Phone();
                newNumber[i].Name = dataGridView1[3, i].Value.ToString();
                newNumber[i].Number = dataGridView1[4, i].Value.ToString();
                db.Phones.InsertOnSubmit(newNumber[i]);
            }

            // Commit all changes
            db.SubmitChanges();

            // Refresh and re-show updtated database
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        // Searches the database for anything in the search bar
        private void searchButton_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;

            //Creates an SQL query that will search both columns to find anything that 
            //matches what the user types in.
            var query = from phone in db.Phones
                        where phone.Name.Contains(search) || phone.Number.Contains(search)
                        select phone;

            //Change the source to the results of the query and then show them in the UI
            dataGridView1.DataSource = query;
            dataGridView1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // This line of code loads data into the 'database1DataSet.Table' table. 
            // It can be moved or removed as needed.
            this.tableTableAdapter.Fill(this.database1DataSet.Table);
        }
    }
}
