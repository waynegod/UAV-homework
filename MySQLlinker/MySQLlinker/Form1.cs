using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MySQLlinker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public MySqlConnection mySql;
        private void Form1_Load(object sender, EventArgs e)
        {


        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mySql = new MySqlConnection
                {
                    ConnectionString = "server=" + textBox1.Text + ";uid=" + textBox2.Text + ";pwd=" + textBox3.Text +
                    ";database=" + textBox4.Text
                };

                /*mySql.Open();
                MessageBox.Show("OK");
                mySql.Close();*/
                mySql.Open();
                if (mySql.State == ConnectionState.Open)
                {
                    checkedListBox1.Items.Clear();
                    try
                    {
                        var commandStr = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='" + "air" + "'";
                        var sqlCommand = new MySqlCommand(commandStr, mySql);
                        var dr = sqlCommand.ExecuteReader();
                        while (dr.Read())
                            checkedListBox1.Items.Add(dr["TABLE_NAME"], true);
                        MessageBox.Show("OK");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("讀取資料失敗 原因為 " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("開啟失敗，請確定您輸入的資料庫資訊", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                mySql.Close();
            }
            catch
            {
                mySql.Close();
                MessageBox.Show("ERROR");
            }

        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem != null)
            {
                var tableName = checkedListBox1.SelectedItem.ToString();
                try
                {
                    mySql.Open();
                    string commandStr = "SELECT * FROM air." + tableName + "";
                    //string commandStr = "select COLUMN_KEY,COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH  from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + tableName + "'";
                    var sqlAdapter = new MySqlDataAdapter(commandStr, mySql);
                    var ds = new DataSet();
                    sqlAdapter.Fill(ds, tableName);
                    dataGridView1.DataSource = ds.Tables[0];
                    mySql.Close();

                    label5.Text = "總數：" + Datacount();

                }
                catch (Exception ex)
                {
                    mySql.Close();
                    MessageBox.Show(@"讀取資料失敗 原因為 " + ex.Message, @"錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show(@"Choose a table :)", @"info");
            }
        }

        private void button2_Click(object sender, EventArgs e)//
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int max = 0;

            mySql.Open();
            if (Datacount() != 0)
            {
                string sql = "SELECT MAX(A) FROM air.test;";
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                max = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (mySql.State.ToString().ToUpper() != "OPEN")
                mySql.Open();

            for (int con = 0; con < Convert.ToInt16(textBox8.Text); con++)
            {
                string sqadds = "Insert into test(A,B,C) values( " + (++max) + ",'" + rnd.Next(0, 100) + "','" + rnd.Next(0, 100) + "')";
                MySqlCommand cmd = new MySqlCommand(sqadds, mySql);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("" + max);
            DGView();
            mySql.Close();
            try
            {


            }
            catch
            {
                Console.Write("err");
                mySql.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("{0}", checkedListBox1.Items[0]);
        }
        private void button4_Click(object sender, EventArgs e)//ADD
        {
            try
            {
                mySql.Open();
                string sql = "Insert into test(A,B,C) values( " + int.Parse(textBox5.Text) + ",'" + textBox6.Text +"','"+ textBox7.Text + "')";
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                cmd.ExecuteNonQuery();
                DGView();
                mySql.Close();
            }
            catch
            {
                Console.Write("err");
                mySql.Close();
            }
        }
        private void DGView()
        {
            try
            {
                var tableName = checkedListBox1.SelectedItem.ToString();
                string commandStr = "SELECT * FROM air."+ tableName + "";
                //string commandStr = "select COLUMN_KEY,COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH  from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + tableName + "'";
                var sqlAdapter = new MySqlDataAdapter
                {
                    SelectCommand = new MySqlCommand(commandStr, mySql)
                };
                var ds = new DataSet();
                sqlAdapter.Fill(ds, tableName);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
            }
            catch
            {
                Console.Write("err");
                mySql.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mySql.Open();

            string sql = "delete from test";
            MySqlCommand cmd = new MySqlCommand(sql, mySql);
            cmd.ExecuteNonQuery();
            DGView();
            mySql.Close();
        }

        private int Datacount()
        {
            int count = 0;

            if (mySql.State.ToString().ToUpper() != "OPEN")
                mySql.Open();
            try
            {
                string sql = "SELECT COUNT(*) FROM air.test;";
                MySqlCommand cmd = new MySqlCommand(sql, mySql);
                count = (int)(long)cmd.ExecuteScalar();

                mySql.Close();
            }
            catch
            {
                Console.Write("err");
                mySql.Close();
            }
            return count;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
