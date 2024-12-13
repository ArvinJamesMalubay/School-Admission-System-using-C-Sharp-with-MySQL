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

namespace School_Admission_System
{
    public partial class Edit_Information_form : Form
    {
        public Edit_Information_form()
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
        private void btn_close_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to close?", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.Hide();
                }

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
                MySqlConnection con = Database.dbConnection.GetConnection();

                string updateQuery = "UPDATE student_profile SET firstName = @FirstName, middleName = @MiddleName, lastName = @LastName, " +
                                         "age = @age, sex = @Sex, campus = @campus, department = @department, course = @course, " +
                                         "major = @major, status = @status, year = @year, photo = @Photo WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
                {

                    cmd.Parameters.AddWithValue("@id", lbl_id.Text);
                    cmd.Parameters.AddWithValue("@FirstName", tbx_firstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", tbx_lastName.Text);
                    cmd.Parameters.AddWithValue("@MiddleName", tbx_middleName.Text);
                    cmd.Parameters.AddWithValue("@age", tbx_Age.Text);
                    cmd.Parameters.AddWithValue("@Sex", rbt_Male.Checked ? "Male" : "Female");
                    cmd.Parameters.AddWithValue("@campus", cmb_campus.Text);
                    cmd.Parameters.AddWithValue("@department", cmb_department.Text);
                    cmd.Parameters.AddWithValue("@course", cmb_course.Text);
                    cmd.Parameters.AddWithValue("@major", cmb_major.Text);
                    cmd.Parameters.AddWithValue("@status", rbt_continuing.Checked ? "Continuing" : (rbt_Transferee.Checked ? "Transferee" : (rbt_underGraduate.Checked ? "Under Graduate" : "Graduate")));
                    cmd.Parameters.AddWithValue("@year", cmb_year.Text);
                    cmd.Parameters.AddWithValue("@Photo", SaveImageToDisk(picbox_photo.Image));

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }


            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to cancel?", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    clear();
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
    }
}
