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
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
            
         
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                Login_Classification form = new Login_Classification();
                form.ShowDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btn_createAccount_Click(object sender, EventArgs e)
        {
            try
            {
                Account_Classification form = new Account_Classification();
                form.ShowDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

      
        
    }
}
