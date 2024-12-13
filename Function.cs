using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Admission_System
{
    class Function
    {
       
        public static void display(string querry, DataGridView dgv)
        {
            MySqlConnection con = Database.dbConnection.GetConnection();
            MySqlCommand cmd = new MySqlCommand(querry, con);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv.DataSource = dt;
            con.Close();
        }
        public static void Add(string querry)
        {
            MySqlConnection con = Database.dbConnection.GetConnection();
            MySqlCommand cmd = new MySqlCommand(querry, con);

            try
            {
                cmd.ExecuteNonQuery();

                MessageBox.Show("Added Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Fatal Error!. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

    }
}
