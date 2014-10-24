using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MasterProject
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            PasswordTxt.PasswordChar = '*';
        }

        int count = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (count < 3)
                {
                    if (String.IsNullOrEmpty(UsernameTxt.Text) || String.IsNullOrEmpty(PasswordTxt.Text))
                    {
                        MessageBox.Show("Please enter full information...");
                        return;
                    }

                    if (UsernameTxt.Text.ToLower() == "admin" && PasswordTxt.Text.ToLower() == "admin")
                    {
                        Form1 f = new Form1();
                        f.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password ! Please try again..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        count++;
                        return;
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
            catch
            {
                MessageBox.Show("Log in Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
