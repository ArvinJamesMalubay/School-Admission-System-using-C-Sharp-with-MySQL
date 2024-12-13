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
    public partial class Account_Classification : Form
    {
        public Account_Classification()
        {
            InitializeComponent();
        }

        private void btn_student_Click(object sender, EventArgs e)
        {
            try
            {
                Register_form form = new Register_form();
                form.ShowDialog();
                this.Hide();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btn_teacher_Click(object sender, EventArgs e)
        {
            try
            {
                Register_teacher_form form = new Register_teacher_form();
                form.ShowDialog();
                this.Hide();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
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
