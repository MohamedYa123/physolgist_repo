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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            rates1 = new double[Controls.Count, 2];
            rates2 = new double[Controls.Count, 2];
            try
            {
                int i = 0;
                foreach (Control c in Controls)
                {
                    rates1[i, 0] = Convert.ToDouble(c.Width) / Convert.ToDouble(Width);
                    rates1[i, 1] = Convert.ToDouble(c.Height) / Convert.ToDouble(Height);
                    rates2[i, 0] = Convert.ToDouble(c.Top) / Convert.ToDouble(Height);
                    rates2[i, 1] = Convert.ToDouble(c.Left) / Convert.ToDouble(Width);
                    i += 1;
                }
                first = true;
            }
            catch
            {

            }
        }

        private void combousers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combousers.SelectedIndex != -1)
            {
                myuser = Program.App_data.users[combousers.SelectedIndex];
               // show_user(combousers.SelectedIndex);
            }
        }
        user myuser;
        private void Form5_Load(object sender, EventArgs e)
        {

            refresh_users();
        }
        void refresh_users()
        {
            combousers.Items.Clear();
            foreach (var u in Program.App_data.users)
            {
                combousers.Items.Add((combousers.Items.Count + 1) + " " + u.name);
            }
            if (combousers.Items.Count > 0)
            {
                //    combousers.SelectedIndex = 0;
            }
            else
            {
                combousers.Text = "";
            }
        }
        DateTime dtnow;
        private double[,] rates1;
        private bool first;

        public double[,] rates2 { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            dtnow = DateTime.Now;
            textBox3.Text = dtnow.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            session s = new session();
            s.kind = textBox1.Text;
            s.desc = textBox2.Text;
            s.dtime = dtnow;
            if (myuser == null)
            {
                MessageBox.Show("Please select a user");
                return;
            }
            if (myuser.user_sessions == null)
            {
                myuser.user_sessions = new List<session>();
            }
            myuser.user_sessions.Add(s);
            MessageBox.Show("Session added");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.App_data.saveme(Program.datanamme);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form5_Resize(object sender, EventArgs e)
        {
            try
            {
                if (first)
                {
                    int i = 0;
                    foreach (Control c in Controls)
                    {
                        if (c.GetType() == (new RadioButton()).GetType())
                        {
                            continue;
                        }
                        c.Width = Convert.ToInt32(rates1[i, 0] * Convert.ToDouble(Width));
                        c.Height = Convert.ToInt32(rates1[i, 1] * Convert.ToDouble(Height));
                        c.Top = Convert.ToInt32(rates2[i, 0] * Convert.ToDouble(Height));
                        c.Left = Convert.ToInt32(rates2[i, 1] * Convert.ToDouble(Width));
                        i += 1;
                    }
                }
            }
            catch
            {

            }
        }
    }
}
