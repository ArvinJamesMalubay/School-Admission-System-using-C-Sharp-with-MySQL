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
    public partial class Register_teacher_form : Form
    {
        public Register_teacher_form()
        {
            InitializeComponent();
        }
        void clear()
        {
            tbx_firstName.Clear();
            tbx_middleName.Clear();
            tbx_lastName.Clear();
            cmb_department.Text = "";
            tbx_password.Clear();
            tbx_teacherId.Clear();

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

        private void btn_register_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_firstName.Text != string.Empty || tbx_lastName.Text != string.Empty
                    || tbx_middleName.Text != string.Empty || tbx_password.Text != string.Empty
                    || tbx_teacherId.Text != string.Empty || tbx_confirmPassword.Text != string.Empty)
                {
                    string firstName = tbx_firstName.Text;
                    string middleName = tbx_middleName.Text;
                    string lastName = tbx_lastName.Text;
                    string campus = cmb_department.Text;
                    string studentId = tbx_teacherId.Text;
                    string password = tbx_password.Text;
                    string confirmPassword = tbx_confirmPassword.Text;

                    if (!chkbox_Agreement.Checked)
                    {
                        MessageBox.Show("Please agree to the terms before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (password != confirmPassword)
                    {
                        MessageBox.Show("Password and Confirm Password do not match. Please try again.");
                        return;
                    }
                    else
                    {
                        MySqlConnection con = Database.dbConnection.GetConnection();
                        string query = "INSERT INTO teacher_accounts (firstName, middleName, lastName, department, teacherId, password) " +
                            "VALUES (@firstName,@middleName, @lastName, @deparment, @teacherId, @password)";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@middleName", middleName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@deparment", campus);
                        cmd.Parameters.AddWithValue("@teacherId", studentId);
                        cmd.Parameters.AddWithValue("@password", password);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful!");
                            clear();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Fill up the missing information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
