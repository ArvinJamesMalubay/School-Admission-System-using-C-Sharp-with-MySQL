using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Admission_System
{
    public partial class teacher_login_form : Form
    {
        public teacher_login_form()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to exit?", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.Hide();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void chkbox_password_CheckedChanged(object sender, EventArgs e)
        {
            tbx_password.PasswordChar = chkbox_password.Checked ? '\0' : '*';
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_teacherId.Text != string.Empty && tbx_password.Text != string.Empty && cmb_department.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();
                    string query = "SELECT department, teacherId, password FROM teacher_accounts WHERE department = @department AND teacherId = @teacherId AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@department", cmb_department.Text);
                    cmd.Parameters.AddWithValue("@teacherId", tbx_teacherId.Text);
                    cmd.Parameters.AddWithValue("@password", tbx_password.Text);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Welcome to the CYBER SMART SOCIETY ORGANIZATION");
                            Teacher_Dashboard form = new Teacher_Dashboard();
                            form.Show();
                            Close();
                            Form mainForm = Application.OpenForms["Main_Form"];
                            mainForm?.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Oops! Something went wrong.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Oops! Something went wrong.");
                }



            }
            catch (Exception err)
            {
                        MessageBox.Show(err.Message);
            }

               
        }
    }
}
