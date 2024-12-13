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
    public partial class Student_Dashboard : Form
    {

        public Student_Dashboard()
        {
            InitializeComponent();

        }

       
        private void btn_contribute_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_studentId.Text != string.Empty && cmb_money.Text != string.Empty)
                {
                    string studentId = tbx_studentId.Text;
                    string newContribution = cmb_money.Text;

                    MySqlConnection con = Database.dbConnection.GetConnection();
                    string retrieveQuery = "SELECT contribution FROM user_accounts WHERE studentId = @studentId";
                    MySqlCommand retrieveCmd = new MySqlCommand(retrieveQuery, con);
                    retrieveCmd.Parameters.AddWithValue("@studentId", studentId);

                    object currentContribution = retrieveCmd.ExecuteScalar();
                    int existingContribution = (currentContribution != null) ? Convert.ToInt32(currentContribution) : 0;
                    int totalContribution = existingContribution + Convert.ToInt32(newContribution);

                    // Update the contribution in the database
                    string updateQuery = "UPDATE user_accounts SET contribution = @totalContribution WHERE studentId = @studentId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@studentId", studentId);
                    updateCmd.Parameters.AddWithValue("@totalContribution", totalContribution);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thank You For Your Contribution!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        tbx_contribution.Text = totalContribution.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Oops! Something is wrong. Please try again.");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill up the missing information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }


        }

        private void Student_Dashboard_Load(object sender, EventArgs e)
        {
            MySqlConnection con = Database.dbConnection.GetConnection();

            string selectQueryActivity = "SELECT title FROM activity_table WHERE date = @date";
            MySqlCommand cmdActivity = new MySqlCommand(selectQueryActivity, con);
            cmdActivity.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            object title = cmdActivity.ExecuteScalar();

            string selectQueryOfficers = "SELECT president, vicePresident, secretary, treasurer, auditor, pio FROM organization_officers";
            MySqlCommand cmdOfficers = new MySqlCommand(selectQueryOfficers, con);
            MySqlDataReader readerOfficers = cmdOfficers.ExecuteReader();

            if (readerOfficers.Read())
            {
                
                object president = readerOfficers["president"];
                object vicePresident = readerOfficers["vicePresident"];
                object secretary = readerOfficers["secretary"];
                object treasurer = readerOfficers["treasurer"];
                object auditor = readerOfficers["auditor"];
                object pio = readerOfficers["pio"];

                lbl_president.Text = president.ToString();
                lbl_vicePresident.Text = vicePresident.ToString();
                lbl_secretary.Text = secretary.ToString();
                lbl_treasurer.Text = treasurer.ToString();
                lbl_auditor.Text = auditor.ToString();
                lbl_pio.Text = pio.ToString();
         
                rchbox_title.Text = title.ToString();
            }
            else
            {
                MessageBox.Show("No records found.");
            }

            // Close the reader and the connection
            readerOfficers.Close();
            con.Close();

        }

        private void btn_loginActivity_Click(object sender, EventArgs e)
        {
            {
                MySqlConnection con = Database.dbConnection.GetConnection();

                string insertQuery = "INSERT INTO attendance (title, studentId, date) " +
                                    "SELECT @title, @studentId, @date FROM activity_table " +
                                    "WHERE title = @activityTitle";

                MySqlCommand cmd = new MySqlCommand(insertQuery, con);

                cmd.Parameters.AddWithValue("@title", rchbox_title.Text);
                cmd.Parameters.AddWithValue("@studentId", tbx_studentIdActivity.Text);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@activityTitle", rchbox_title.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Successfully joined a new Activity!");
                    tbx_studentIdActivity.Clear();
                }
                else
                {
                    MessageBox.Show("Insertion failed. Please try again.");
                }



            }
        }
    }
}
