using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Admission_System.Database
{
    class dbConnection
    {
        public static MySqlConnection GetConnection()
        {
            string sql = "datasource=localhost;port=3306;username=root;password=;database=school_admission_system";
            MySqlConnection connectionstring = new MySqlConnection(sql);
            try
            {
                connectionstring.Open();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show("MySQL Connection! \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return connectionstring;
        }
    }
}
