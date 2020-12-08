using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatvanyossagokApplication
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        void VarosBetoltes()
        {
            string sql = @"SELECT id, nev, lakossag FROM varosok ORDER BY nev";
            var comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string nev = reader.GetString("nev");
                    int lakossag=reader.GetInt32("lakossag");
                    
                    var varos = new Varos(id, nev, lakossag);
                    listBox1.Items.Add(varos);
                }
                reader.Close();
            }
        }
        void LatvanyossagBetoltes()
        {
            int vid = VarosID(Convert.ToString(listBox1.SelectedItem).Split(' ')[0]);
            if(vid !=-1)
            {
                string sql = "SELECT id, nev, leiras, ar, varos_id FROM latvanyossagok WHERE varos_id = " + vid + "";
                var comm = this.conn.CreateCommand();
                comm.CommandText = sql;
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string nev = reader.GetString("nev");
                        string leiras = reader.GetString("leiras");
                        int ar = reader.GetInt32("ar");
                        int varosId = reader.GetInt32("varos_id");
                        if (ar != 0)
                        {
                            listBox2.Items.Add(nev + ar);
                        }
                        else
                        {
                            listBox2.Items.Add(nev + "Ingyenes");
                        }
                    }
                    reader.Close();
                }
               
            }
            
        }
        int VarosID(string nev)
        {
            int ki=-1;
            string sql = "SELECT id FROM varosok WHERE nev LIKE '"+nev+"'";
            var comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            using (var reader = comm.ExecuteReader())
            {
                while(reader.Read())
                {
                     ki = reader.GetInt32("id");
                }
                reader.Close();
            }
            return ki;

        }
        public Form1()
        {
            
            InitializeComponent();
            conn = new MySqlConnection("Server=localhost; Database=latvanyossagokdb; Uid=root; Pwd=''");
            try { conn.Open(); }
            catch (Exception ex) { MessageBox.Show("Sikertelen csatlakozás"); }
            this.FormClosed += (sender, args) =>
            {
                if (conn != null)
                {
                    conn.Close();
                }
                
            };
            VarosBetoltes();
            string cv = @"CREATE TABLE IF NOT EXISTS `varosok` (
                    `id` INT(6) AUTO_INCREMENT PRIMARY KEY,
                    `nev` VARCHAR(30) UNIQUE NOT NULL,
                    `lakossag` INT(30) NOT NULL
                    )";
            string cl = @"CREATE TABLE IF NOT EXISTS latvanyossagok (
                    id INT(6) AUTO_INCREMENT PRIMARY KEY,
                    nev VARCHAR(30) NOT NULL,
                    leiras VARCHAR(80) NOT NULL,
                    ar INT(30) DEFAULT 0 NOT NULL,
                    varos_id INT(5) NOT NULL,
                    FOREIGN KEY(varos_id) REFERENCES varosok(id)
                    )";

            var comm = this.conn.CreateCommand();
            comm.CommandText = cv;
            comm.ExecuteNonQuery();
            var comm2 = this.conn.CreateCommand();
            comm2.CommandText = cl;
            comm2.ExecuteNonQuery();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "SELECT nev FROM varosok WHERE nev LIKE '" + textBox1.Text + "'";
            var comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            string nev = "";
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    nev = reader.GetString("nev");
                }
            }
            if(textBox1.Text.Equals(nev))
            {
                MessageBox.Show("Ez a város már meg van adva");
            }
            if(textBox1.Text.Equals("")||numericUpDown1.Value.Equals(null))
            {
                MessageBox.Show("Kérem adjon meg adatot");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "INSERT INTO `varosok`(`nev`, `lakossag`) VALUES '"+textBox1.Text+"','"+numericUpDown1.Value+"')";
            var comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            listBox1.Items.Clear();
            VarosBetoltes();

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            int varosid = VarosID(Convert.ToString(listBox1.SelectedItem).Split(' ')[0]);
            string sql = "INSERT INTO `latvanyossagok`(`nev`, `leiras`, `ar`, `varos_id`) VALUES (" + textBox2.Text + "," + textBox3.Text + "," + numericUpDown2.Value + "," +varosid+ ")";
        }

        private void button3_Click(object sender, EventArgs e)
        {

            int varosid = VarosID(Convert.ToString(listBox1.SelectedItem).Split(' ')[0]);
            string sql = "DELETE FROM `latvanyossagok` WHERE varos_id LIKE "+varosid+"";
            listBox1.Items.Clear();
            VarosBetoltes();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem!=null)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                panel3.Enabled = true;
                LatvanyossagBetoltes();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nev = Convert.ToString(listBox1.SelectedItem).Split(' ')[0];
            string sql = @"UPDATE `varosok` 
                            `nev`=" + textBox4.Text + ",`lakossag`=" + numericUpDown4.Value + "" +
                            " WHERE nev LIKE "+nev+"";
            listBox1.Items.Clear();
            VarosBetoltes();
            panel1.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

            panel2.Visible = false;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
