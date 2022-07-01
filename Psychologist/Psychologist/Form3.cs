using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Psychologist
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        //    pictureBox1.BackgroundImage = BackgroundImage;
          //  BackgroundImage = null;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (Program.App_data == null)
            {
                button1.Text = "Create";

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Program.App_data == null)
            {
                app a = new app(textBox1.Text, textBox2.Text);
                Program.App_data = a;
                a.saveme(Program.datanamme);
                Form4 f = new Form4();
                f.Show();
                Hide();
            }
            else
            {
                app a = app.load(Program.datanamme);
                if(a.check(textBox1.Text, textBox2.Text))
                {
                    Program.App_data = a;
                    Form4 f = new Form4();
                    f.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("كود خاطيء - برجاء التواصل علي هذا الرقم لتفعيل البرنامج - 01032936147");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }
    }
}
