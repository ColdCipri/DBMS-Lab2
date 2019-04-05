using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMS_Lab2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Text = SqlConn.myApp();
        }

        private void ShowItems()
        {
            if (SqlConn.dataSet.Tables["myInvoices"] != null)
            {
                SqlConn.dataSet.Tables["myInvoices"].Clear();
            }

            SqlConn.sqlQuery =  "SELECT * " +
                                "FROM Invoice " +
                                "WHERE CarID = " +
                                "'" + textBox1.Text + "'";
            SqlConn.command.CommandText = SqlConn.sqlQuery;
            SqlConn.dataAdapter.SelectCommand = SqlConn.command;
            SqlConn.dataAdapter.Fill(SqlConn.dataSet, "myInvoices");

            dataGridView1.DataSource = SqlConn.dataSet.Tables["myInvoices"];

            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SqlConn.sqlQuery = "SELECT * FROM Car";
            SqlConn.OpenConn();
            SqlConn.command = new SqlCommand(SqlConn.sqlQuery, SqlConn.connection);

            SqlConn.dataAdapter = new SqlDataAdapter(SqlConn.command);
            SqlConn.dataSet = new DataSet();
            SqlConn.bindingSource = new BindingSource();
            SqlConn.dataAdapter.Fill(SqlConn.dataSet, "myCars");

            foreach (DataRow r in SqlConn.dataSet.Tables["myCars"].Rows)
            {
                //listBox1.Items.Add(r["Manufacturer"].ToString());
                listBox1.Items.Add(string.Format("{0} {1}", r["Manufacturer"].ToString(), r["Model"].ToString()));
            }
            listBox1.ValueMember = "CarID";

            SqlConn.bindingSource.DataSource = SqlConn.dataSet.Tables["myCars"];

            textBox1.DataBindings.Add("Text", SqlConn.bindingSource, "CarID");
            textBox2.DataBindings.Add("Text", SqlConn.bindingSource, "Manufacturer");
            textBox3.DataBindings.Add("Text", SqlConn.bindingSource, "Model");
            textBox4.DataBindings.Add("Text", SqlConn.bindingSource, "Color");
            textBox5.DataBindings.Add("Text", SqlConn.bindingSource, "SUV");
            textBox6.DataBindings.Add("Text", SqlConn.bindingSource, "Convertible");
            textBox7.DataBindings.Add("Text", SqlConn.bindingSource, "YearOfFabrication");
            textBox8.DataBindings.Add("Text", SqlConn.bindingSource, "ShowRoomID");

            listBox1.SelectedIndex = 0;

            var link = new DataGridViewLinkColumn();

            link.DisplayIndex = 0;
            link.DefaultCellStyle.NullValue = "SELECT";
            link.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            link.Width = 60;

            link.ActiveLinkColor = Color.White;
            link.LinkBehavior = LinkBehavior.HoverUnderline;
            link.LinkColor = Color.Crimson;
            link.TrackVisitedState = true;
            link.VisitedLinkColor = Color.YellowGreen;

            dataGridView1.Columns.Add(link);
            dataGridView1.AllowUserToAddRows = false;

            ShowItems();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SqlConn.bindingSource != null)
            {
                SqlConn.bindingSource.Position = listBox1.SelectedIndex;
            }
            ShowItems();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            SqlConn.CloseConn();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm childForm = new MainForm();
            this.Hide();
            childForm.Closed += (s, args) => this.Close();
            childForm.ShowDialog();
        }
    }
}
