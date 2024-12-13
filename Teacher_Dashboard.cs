using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace School_Admission_System
{
    public partial class Teacher_Dashboard : Form
    {
        public Teacher_Dashboard()
        {
            InitializeComponent();
        }
        void clear()
        {
            tbx_firstName.Clear();
            tbx_middleName.Clear();
            tbx_lastName.Clear();
            tbx_Age.Clear();

            cmb_campus.Text = "";
            cmb_department.Text = "";
            cmb_course.Text = "";
            cmb_major.Text = "";
            picbox_photo.Image = null;
        }
        void display_student()
        {
            Function.display("Select Concat(firstName, ' ', lastName) as Full_Name, age, year, course, major " +
                "FROM student_profile ", dgv_studentList);
        }

        private void bnt_home_Click(object sender, EventArgs e)
        {
            try
            {
                Main_Form form = new Main_Form();
                form.Show();
                this.Hide();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private string SaveImageToDisk(System.Drawing.Image image)
        {

            string folderPath = "C:\\Users\\Dell\\Desktop\\Admission_System_Resources\\Images";
            string fileName = Guid.NewGuid().ToString() + ".png";

            string filePath = Path.Combine(folderPath, fileName);
            image.Save(filePath);

            return filePath;
        }
        private void btn_register_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_firstName.Text != string.Empty || tbx_lastName.Text != string.Empty
                    || tbx_middleName.Text != string.Empty || tbx_Age.Text != string.Empty
                    || cmb_campus.Text != string.Empty || cmb_department.Text != string.Empty
                    || cmb_major.Text != string.Empty || cmb_course.Text != string.Empty 
                    || cmb_year.Text != string.Empty || picbox_photo.Image != null)
                {
                    string firstName = tbx_firstName.Text;
                    string middleName = tbx_middleName.Text;
                    string lastName = tbx_lastName.Text;
                    string age = tbx_Age.Text;
                    string sex = rbt_Male.Checked ? "Male" : "Female";

                    string campus = cmb_campus.Text;
                    string department = cmb_department.Text;
                    string course = cmb_course.Text;
                    string major = cmb_major.Text;
                    string status = rbt_continuing.Checked ? "Continuing" : (rbt_Transferee.Checked ? "Transferee" : (rbt_underGraduate.Checked ? "Under Graduate" : "Graduate"));
                    string year = cmb_year.Text;

                    
                    string imagePath = SaveImageToDisk(picbox_photo.Image);

                    MySqlConnection con = Database.dbConnection.GetConnection();
                    string query = "INSERT INTO student_profile (firstName, middleName, lastName,age, sex, campus, department,course, major,status, year, photo ) " +
                            "VALUES (@firstName,@middleName, @lastName,@age, @sex, @campus, @department, @course, @major, @status,@year, @photo)";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@middleName", middleName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@sex", sex);
                    cmd.Parameters.AddWithValue("@campus", campus);
                    cmd.Parameters.AddWithValue("@department", department);
                    cmd.Parameters.AddWithValue("@course", course);
                    cmd.Parameters.AddWithValue("@major", major);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@photo", imagePath);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Added Successfully!");
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.");
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

        private void picbox_photo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Display the selected image in the PictureBox
                    picbox_photo.Image = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void tbx_searchBar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbx_searchBar.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();
                    Function.display("SELECT CONCAT(firstName, ' ', lastName) as Full_Name, age, year, course, major " +
                "FROM student_profile WHERE firstName LIKE '%" + tbx_searchBar.Text + "%' AND campus = '" + cmb_searchCampus.Text + "' AND year = '" + cmb_searchYear.Text + "'", dgv_studentList);

                }
                else
                {
                    display_student();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // You can adjust the margins and positioning based on your needs
            int x = 10;
            int y = 10;

            // Create a bitmap to hold the contents of the DataGridView
            Bitmap bitmap = new Bitmap(dgv_studentList.Width, dgv_studentList.Height);
            dgv_studentList.DrawToBitmap(bitmap, new Rectangle(0, 0, dgv_studentList.Width, dgv_studentList.Height));

            // Draw the bitmap on the printer graphics
            e.Graphics.DrawImage(bitmap, x, y);
        }

        private void menuStrip_print_Click_1(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPage);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Do you want to cancel it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    clear();
                }
                return;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void dgv_studentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 0)
            {
                //Edit
                MySqlConnection con = Database.dbConnection.GetConnection();
                Edit_Information_form form = new Edit_Information_form();

                string query = "Select * from student_profile where firstName = @firstName";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    string fullName = dgv_studentList.Rows[e.RowIndex].Cells[2].Value.ToString();


                    string[] names = fullName.Split(' ');
                    string firstName = names[0];
                    string lastName = names.Length > 1 ? names[1] : string.Empty;

                    cmd.Parameters.AddWithValue("@firstName", firstName);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            form.lbl_id.Text = reader["id"].ToString();
                            form.tbx_firstName.Text = reader["firstName"].ToString();
                            form.tbx_lastName.Text = reader["lastName"].ToString();
                            form.tbx_middleName.Text = reader["middleName"].ToString();
                            form.tbx_Age.Text = reader["age"].ToString();
                            form.cmb_campus.Text = reader["campus"].ToString();
                            form.cmb_department.Text = reader["department"].ToString();
                            form.cmb_course.Text = reader["course"].ToString();
                            form.cmb_major.Text = reader["major"].ToString();
                            form.cmb_year.Text = reader["year"].ToString();

                        }
                    }
                    con.Close();
                }
                form.btn_register.Text = "Update";
                form.ShowDialog();
                display_student();
            }

            if (e.ColumnIndex == 1)
            {
                // Delete
                if (MessageBox.Show("Are you sure you want to delete this field?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();


                    // Assuming first name is in the second column and last name is in the third column
                    string fullname = dgv_studentList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string[] names = fullname.Split(' ');
                    string firstName = names[0];
                    string lastName = names.Length > 1 ? names[1] : string.Empty;

                    // Perform the deletion operation
                    string deleteQuery = "DELETE FROM student_profile WHERE firstName = @firstName";
                    using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, con))
                    {
                        deleteCmd.Parameters.AddWithValue("@firstName", firstName);

                        int rowsAffected = deleteCmd.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Field deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            display_student();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            display_student();
        }
        void display_contribution()
        {
            Function.display("SELECT CONCAT(firstName, ' ', lastName) as Full_Name, campus, contribution " + "FROM user_accounts", dgv_contribution);

        }
        private void tbx_search_contribution_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbx_search_contribution.Text != string.Empty)
                {
                    Function.display("SELECT CONCAT(firstName, ' ', lastName) as Full_Name, campus, contribution " + "FROM user_accounts WHERE firstName LIKE '%" + tbx_search_contribution.Text + "%'", dgv_contribution);
                   
                }
                else
                {
                    display_contribution();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btn_refreshContribution_Click(object sender, EventArgs e)
        {
            display_contribution();
        }
        
        private void btn_president_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_president.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                   
                    string querry = "Update organization_officers set president = @president where id = '" + lbl_president.Text + "'";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);

                    string selectquerry = "Select president from organization_officers where id = '" + lbl_president.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(selectquerry, con);
                    cmd1.Parameters.AddWithValue("@president", tbx_president.Text);
                    object name = cmd.ExecuteScalar();

                    tbx_displayPresident.Text = name.ToString();
                    cmd1.ExecuteNonQuery();
                    
                    con.Close();
                    MessageBox.Show("Congratulations for the new President!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbx_president.Clear();
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

        private void tbn_cancelPresident_Click(object sender, EventArgs e)
        {
            tbx_president.Clear();
        }

        private void btn_vicePresident_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_vicePresident.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string querry = "Update organization_officers set vicePresident = @vicePresident where id = '" + lbl_vicePresident.Text + "'";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);
                    cmd1.Parameters.AddWithValue("@vicePresident", tbx_vicePresident.Text);

                    string selectquerry = "Select vicePresident from organization_officers where id = '" + lbl_vicePresident.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(selectquerry, con);
                    object name = cmd.ExecuteScalar();

                    cmd1.ExecuteNonQuery();

                    tbx_displayVicePresident.Text = name.ToString();
                    con.Close();
                    MessageBox.Show("Congratulations for the new Vice President!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbx_vicePresident.Clear();
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

        private void btn_cancelVicePresident_Click(object sender, EventArgs e)
        {
            tbx_vicePresident.Clear();
        }

        private void btn_secretary_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_secretary.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string querry = "Update organization_officers set secretary = @secretary where id = '" + lbl_secretary.Text + "'";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);
                    cmd1.Parameters.AddWithValue("@secretary", tbx_secretary.Text);
                    cmd1.ExecuteNonQuery();

                    string selectquerry = "Select secretary from organization_officers where id = '" + lbl_secretary.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(selectquerry, con);
                    object name = cmd.ExecuteScalar();
                 

                    tbx_displaySecretary.Text = name.ToString();
                    con.Close();
                    MessageBox.Show("Congratulations for the new Secretary!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbx_secretary.Clear();
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

        private void btn_cancelSecretary_Click(object sender, EventArgs e)
        {
            tbx_secretary.Clear();
        }

        private void btn_treasurer_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_treasurer.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string querry = "Update organization_officers set treasurer = @treasurer where id = '" + lbl_treasurer.Text + "'";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);
                    cmd1.Parameters.AddWithValue("@treasurer", tbx_treasurer.Text);
                    cmd1.ExecuteNonQuery();

                    string selectquerry = "Select treasurer from organization_officers where id = '" + lbl_treasurer.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(selectquerry, con);
                    object name = cmd.ExecuteScalar();
                    

                    tbx_displayTreasurer.Text = name.ToString();
                    con.Close();
                    MessageBox.Show("Congratulations for the new Treasurer!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbx_treasurer.Clear();
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

            private void btn_cancelTreasurer_Click(object sender, EventArgs e)
        {
            tbx_treasurer.Clear();
        }

        private void btn_auditor_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_auditor.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string querry = "Update organization_officers set auditor = @auditor where id = '" + lbl_auditor.Text + "'";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);
                    cmd1.Parameters.AddWithValue("@auditor", tbx_auditor.Text);
                    cmd1.ExecuteNonQuery();


                    string selectquerry = "Select auditor from organization_officers where id = '" + lbl_auditor.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(selectquerry, con);
                    object name = cmd.ExecuteScalar();

                    tbx_displayAuditor.Text = name.ToString();
                    con.Close();
                    MessageBox.Show("Congratulations for the new Auditor!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbx_auditor.Clear();
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

        private void btn_cancelAuditor_Click(object sender, EventArgs e)
        {
            tbx_auditor.Clear();
        }

        private void btn_PIO_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbx_pio.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string querry = "Update organization_officers set pio = @pio where id = '" + lbl_pio.Text + "'";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);
                    cmd1.Parameters.AddWithValue("@pio", tbx_pio.Text);
                    cmd1.ExecuteNonQuery();


                    string selectquerry = "Select pio from organization_officers where id = '" + lbl_pio.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(selectquerry, con);
                    object name = cmd.ExecuteScalar();

                    tbx_displayPIO.Text = name.ToString();
                    con.Close();
                    MessageBox.Show("Congratulations for the new P.I.O!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbx_pio.Clear();
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

        private void btn_cancelPIO_Click(object sender, EventArgs e)
        {
            tbx_pio.Clear();
        }

        private void Teacher_Dashboard_Load(object sender, EventArgs e)
        {

            MySqlConnection con = Database.dbConnection.GetConnection();
            string selectQueryOfficers = "SELECT president, vicePresident, secretary, treasurer, auditor, pio FROM organization_officers";
            MySqlCommand cmdOfficers = new MySqlCommand(selectQueryOfficers, con);
            MySqlDataReader readerOfficers = cmdOfficers.ExecuteReader();

            // Check if there are any records before accessing data
            if (readerOfficers.Read())
            {
                // Access data from the reader
                object president = readerOfficers["president"];
                object vicePresident = readerOfficers["vicePresident"];
                object secretary = readerOfficers["secretary"];
                object treasurer = readerOfficers["treasurer"];
                object auditor = readerOfficers["auditor"];
                object pio = readerOfficers["pio"];

                // Set values to your labels
                tbx_displayPresident.Text = president.ToString();
                tbx_displayVicePresident.Text = vicePresident.ToString();
                tbx_displaySecretary.Text = secretary.ToString();
                tbx_displayTreasurer.Text = treasurer.ToString();
                tbx_displayAuditor.Text = auditor.ToString();
                tbx_displayPIO.Text = pio.ToString();

            }
            else
            {
                MessageBox.Show("No records found.");
            }

            // Close the reader and the connection
            readerOfficers.Close();


            string selectquerry = "Select id from organization_officers ";
            MySqlCommand cmd = new MySqlCommand(selectquerry, con);
            object id = cmd.ExecuteScalar();
            lbl_president.Text = id.ToString();
            lbl_vicePresident.Text = id.ToString();
            lbl_secretary.Text = id.ToString();
            lbl_treasurer.Text = id.ToString();
            lbl_auditor.Text = id.ToString();
            lbl_pio.Text = id.ToString();
        }

        private void tbn_addActivity_Click(object sender, EventArgs e)
        {

            try
            {
                if (rchtbx_title.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string querry = "Insert into activity_table (title, date, time) Values( @title, @date, @time)";
                    MySqlCommand cmd1 = new MySqlCommand(querry, con);
                    cmd1.Parameters.AddWithValue("@title", rchtbx_title.Text);
                    cmd1.Parameters.AddWithValue("@date", dtp_activityDate.Value.ToString("yyyy-MM-dd"));
                    cmd1.Parameters.AddWithValue("@time", dtp_activityTime.Value.ToString("HH:mm:ss")); ;

                    int rowsAffected = cmd1.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Added a new Acitivty!");
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.");
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

        private void tbx_searchActivity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbx_searchActivity.Text != string.Empty)
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();

                    string searchTitle = tbx_searchActivity.Text; //

                    string searchQuery = "SELECT a.title, CONCAT(ua.firstname, ' ', ua.lastname) AS FullName FROM user_accounts ua " +
                                 "INNER JOIN attendance a ON ua.studentId = a.studentId " +
                                 "WHERE a.title LIKE @title";

                    MySqlCommand searchCmd = new MySqlCommand(searchQuery, con);
                    searchCmd.Parameters.AddWithValue("@title", $"%{searchTitle}%");


                    DataTable dataTable = new DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(searchCmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        dgv_activity.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No records found for the given title.");
                    }

                }
                else
                {
                    MySqlConnection con = Database.dbConnection.GetConnection();
                    string allRecordsQuery = "SELECT a.title, CONCAT(ua.firstname, ' ', ua.lastname) AS FullName FROM user_accounts ua " +
                                     "INNER JOIN attendance a ON ua.studentId = a.studentId";

                    MySqlCommand allRecordsCmd = new MySqlCommand(allRecordsQuery, con);

                    DataTable allRecordsTable = new DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(allRecordsCmd))
                    {
                        adapter.Fill(allRecordsTable);
                    }

                    if (allRecordsTable.Rows.Count > 0)
                    {
                        dgv_activity.DataSource = allRecordsTable;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btn_refreshOfficers_Click(object sender, EventArgs e)
        {
            MySqlConnection con = Database.dbConnection.GetConnection();
            string selectQueryOfficers = "SELECT president, vicePresident, secretary, treasurer, auditor, pio FROM organization_officers";
            MySqlCommand cmdOfficers = new MySqlCommand(selectQueryOfficers, con);
            MySqlDataReader readerOfficers = cmdOfficers.ExecuteReader();

            // Check if there are any records before accessing data
            if (readerOfficers.Read())
            {
                // Access data from the reader
                object president = readerOfficers["president"];
                object vicePresident = readerOfficers["vicePresident"];
                object secretary = readerOfficers["secretary"];
                object treasurer = readerOfficers["treasurer"];
                object auditor = readerOfficers["auditor"];
                object pio = readerOfficers["pio"];

                // Set values to your labels
                tbx_displayPresident.Text = president.ToString();
                tbx_displayVicePresident.Text = vicePresident.ToString();
                tbx_displaySecretary.Text = secretary.ToString();
                tbx_displayTreasurer.Text = treasurer.ToString();
                tbx_displayAuditor.Text = auditor.ToString();
                tbx_displayPIO.Text = pio.ToString();

            }
            else
            {
                MessageBox.Show("No records found.");
            }

            // Close the reader and the connection
            readerOfficers.Close();
        }
    }
}
