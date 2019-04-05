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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = SqlConn.myApp();
        }

        private void ShowItems()
        {
            if(SqlConn.dataSet.Tables["Mechanics"] != null)
            {
                SqlConn.dataSet.Tables["Mechanics"].Clear();
            }

            SqlConn.sqlQuery =  "SELECT * " +
                                "FROM Mechanic " +
                                "WHERE ServiceID = " +
                                "'" + textBox1.Text + "'";
            SqlConn.command.CommandText = SqlConn.sqlQuery;
            SqlConn.dataAdapter.SelectCommand = SqlConn.command;
            SqlConn.dataAdapter.Fill(SqlConn.dataSet, "Mechanics");

            dataGridView1.DataSource = SqlConn.dataSet.Tables["Mechanics"];

            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConn.sqlQuery = "SELECT * FROM Service";
            SqlConn.OpenConn();
            SqlConn.command = new SqlCommand(SqlConn.sqlQuery, SqlConn.connection);

            SqlConn.dataAdapter = new SqlDataAdapter(SqlConn.command);
            SqlConn.dataSet = new DataSet();
            SqlConn.bindingSource = new BindingSource();
            SqlConn.dataAdapter.Fill(SqlConn.dataSet, "Services");

            foreach (DataRow r in SqlConn.dataSet.Tables["Services"].Rows)
            {
                listBox1.Items.Add(r["Name"].ToString());
            }
            listBox1.ValueMember = "ServiceID";

            SqlConn.bindingSource.DataSource = SqlConn.dataSet.Tables["Services"];

            textBox1.DataBindings.Add("Text", SqlConn.bindingSource, "ServiceID");
            textBox2.DataBindings.Add("Text", SqlConn.bindingSource, "Name");
            textBox3.DataBindings.Add("Text", SqlConn.bindingSource, "Address");
            textBox4.DataBindings.Add("Text", SqlConn.bindingSource, "Service_TickID");

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
