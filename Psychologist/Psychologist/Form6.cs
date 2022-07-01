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
    public partial class Form6 : Form
    {
        public Form6()
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
            show_user(combousers.SelectedIndex);
        }
        void show_user(int id)
        {
            user u = Program.App_data.users[id];
            usern.Text = u.name;
            agen.Text = u.age + "";
            pictureBox1.Image = (Image)u.image;
            switch (u.gender)
            {
                case Program.Gender.Male:
                    gender.SelectedIndex = 0;
                    break;
                case Program.Gender.female:
                    gender.SelectedIndex = 1;
                    break;
            }
            if (u.desc != null)
            {
                desc.Text = u.desc;
            }
            myuser = u;
            show_sessions();
            show_results();
        }
        void show_sessions()
        {
            seslist.Items.Clear();
            foreach(var s in myuser.user_sessions)
            {
                seslist.Items.Add(s.kind + "           " + s.dtime);
            }
        }
        void show_results()
        {
            listBox1.Items.Clear();
            if (myuser.userData == null)
            {
                myuser.userData = new List<string>();
            }
            foreach (var s in myuser.userData)
            {
                listBox1.Items.Add(s);
            }
        }
        user myuser;
        void refresh_users()
        {
            combousers.Items.Clear();
            combousers.Text = "";
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

        private void Form6_Load(object sender, EventArgs e)
        {

            refresh_users();
        }

        private void newuser_Click(object sender, EventArgs e)
        {
            user u = new user(usern.Text, Convert.ToInt32(agen.Text), Program.Gender.Male, (Bitmap)pictureBox1.Image);
            switch (gender.SelectedIndex)
            {
                case 1:
                    u.gender = Program.Gender.female;
                    break;
            }
            if (Program.App_data.users == null)
            {
                Program.App_data.users = new List<user>();
            }
            Program.App_data.users.Add(u);
            refresh_users();
            myuser = u;
            combousers.SelectedIndex = combousers.Items.Count - 1;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (combousers.SelectedIndex != -1)
            {
                var c= MessageBox.Show("هل تريد حذف المستخدم الحالي؟", "?", MessageBoxButtons.YesNo);
                if (c != DialogResult.Yes) {
                    return;
                }
                Program.App_data.users.RemoveAt(combousers.SelectedIndex);
            }
            refresh_users();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files | *.jpg; *.jpeg; *.png; *.gif; *.tif";
            of.ShowDialog();
            //Bitmap bt=new Bitmap();
            if (of.FileName != "")
            {
                Bitmap bt = new Bitmap(of.FileName);
                pictureBox1.Image = (Image)bt;
            }
        }
        session myses;
        private double[,] rates1;
        private double[,] rates2;
        private bool first;

        void show_session(int id)
        {
            myses = myuser.user_sessions[id];
            kind.Text = myses.kind;
            notes.Text = myses.desc;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (combousers.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a user");
                return;
            }
            if (myuser == null)
            {
                // MessageBox.Show("Please select or add a user");
                //return;
                newuser.PerformClick();
            }
            myuser.name = usern.Text;
            myuser.age = Convert.ToInt32(agen.Text);
            myuser.image = (Bitmap)pictureBox1.Image;
            switch (gender.SelectedIndex)
            {
                case 0:
                    myuser.gender = Program.Gender.Male;
                    break;
                case 1:
                    myuser.gender = Program.Gender.female;
                    break;
            }
            myuser.desc = desc.Text;
            int i = combousers.SelectedIndex;
            refresh_users();
            show_user(i);
            combousers.SelectedIndex = i;
        }

        private void seslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            show_session(seslist.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (myses == null)
            {
                return;
            }
            myses.desc = notes.Text;
            myses.kind = kind.Text;
            show_sessions();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.App_data.saveme(Program.datanamme);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1) {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void Form6_Resize(object sender, EventArgs e)
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
