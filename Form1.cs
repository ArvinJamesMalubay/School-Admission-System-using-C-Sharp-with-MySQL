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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_studentId.Text != string.Empty || tbx_password.Text != string.Empty)
                {
                    
                    MySqlConnection con = Database.dbConnection.GetConnection();
                    string querry = "Select campus ,studentId, password from user_accounts  Where campus = '" + cmb_campus.Text + "' and studentId = '" + tbx_studentId.Text + "' and password = '" + tbx_password.Text + "' ";
                    MySqlCommand cmd = new MySqlCommand(querry, con);

                    MySqlDataReader reader;

                    try
                    {

                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show("Welcome to the CYBER SMART SOCIETY ORGANIZATION");
                                Student_Dashboard form = new Student_Dashboard();
                                
                                form.Show();
                                Close();
                                Form Main_Form = Application.OpenForms["Main_Form"];
                                if (Main_Form != null)
                                {
                                    Main_Form.Hide();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Oops! Something went wrong.");
                        }
                        con.Close();

                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Please fill up the missing information!");
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
    }
}
