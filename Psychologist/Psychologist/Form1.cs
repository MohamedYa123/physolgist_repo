using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Psychologist
{
    public partial class Form1 : Form
    {
        public Form1()
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void importTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        double[,] rates1;
        double[,] rates2;
        bool first;
        private void Form1_Load(object sender, EventArgs e)
        {

            gender.SelectedIndex = 0;
            foreach(Control c in Controls)
            {
                //if (c.GetType() == tests.GetType() || c.GetType()==panel1.GetType())
                {
                  //  if (c.GetType() == panel1.GetType())
                    {
                       // Panel p = (Panel)c;
                      //  foreach(Control c2 in p.Controls)
                        {

                            if (c.GetType() == tests.GetType())
                            {
                                ComboBox cm = (ComboBox)c;
                                cm.ContextMenu = new ContextMenu();
                                cm.ContextMenuStrip = new ContextMenuStrip();
                            }
                        }
                    }
                   // else
                   // {
                     //   ComboBox cm = (ComboBox)c;
                       // cm.ContextMenu = new ContextMenu();
                       // cm.ContextMenuStrip = new ContextMenuStrip();
                   // }
                }
            }
            gettests();
            refresh_users();
        }
        void refresh_users()
        {
            combousers.Items.Clear();
            combousers.Text = "";
            foreach(var u in Program.App_data.users)
            {
                combousers.Items.Add((combousers.Items.Count+1)+" "+u.name);
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
        void refresh_test()
        {
            tests.Items.Clear();
            foreach(var s in Program.App_data.tests)
            {
                tests.Items.Add(s.ToString());
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.App_data.saveme(Program.datanamme);
            
        }

        private void createTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }
        void gettests()
        {
            tests.Items.Clear();
            foreach(var tt in Program.App_data.tests)
            {
                tests.Items.Add(tt.testName);
            }
        }
        private void modifyATestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Datafiles|*.Dat";
            of.ShowDialog();
            if (of.FileName != "")
            {
                Test rt = Test.load(of.FileName);
                Form2 f = new Form2();
                f.test = rt;
                f.ShowDialog();
            }
        }

        private void importTestToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Datafiles|*.Dat";
            of.ShowDialog();
            if (of.FileName != "")
            {
                Test rt = Test.load(of.FileName);
                var d= MessageBox.Show("Do u want to include test " + rt.ToString() + " in yor data?","", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    Program.App_data.tests.Add(rt);
                    gettests();
                    MessageBox.Show("Test Imported !","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
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
            myuser = u;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combousers.SelectedIndex != -1)
            {
                show_user(combousers.SelectedIndex);
            }
        }

        private void agen_TextChanged(object sender, EventArgs e)
        {

        }

        private void agen_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void agen_KeyPress(object sender, KeyPressEventArgs e)
        {
            char[] allowed = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '\b' };
            bool allow=false;
            foreach(var c in allowed)
            {
                if (c == e.KeyChar)
                {
                    allow = true;
                }
            }
            if (!allow)
            {
                e.Handled = true;
            }
        }
        user myuser;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (tests.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a test !");
                return;
            }
            if (myuser == null)
            {
                // MessageBox.Show("Please select or add a user");
                //return;
                newuser.PerformClick();
            }
            
            test = Program.App_data.tests[tests.SelectedIndex];
            records = new List<int>();
            foreach(var c in test.questions)
            {
                records.Add(-1);
            }
            Index = 0;
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
        }
        void represent_test(Test test)
        {
            foreach(var c in test.questions)
            {

            }
        }
        private int index;
        Test test;
        public int Index
        {
            get { return index; }
            set {
                List<Control> cs = new List<Control>();
                foreach (Control item in Controls)
                {
                    if (item.GetType() == (new RadioButton()).GetType())
                    {
                        cs.Add(item);
                    }
                }

                int idx = records[value];
                int ii = 0;
                foreach (Control c in cs)
                {
                    RadioButton r = (RadioButton)c;
                    if (r.Checked)
                    {
                        records[index] = ii;
                    }
                    Controls.Remove(c);
                    ii++;
                }
                qq = test.questions[value];
                int lefts = testq.Left+testq.Width/2;
                int tops = testq.Top;
                tops += 40;
                
               // Qarea.RightToLeft = RightToLeft.Inherit;
               // RightToLeft = RightToLeft.Inherit;
                int i = 0;
                foreach (var c in qq.answers)
                {
                    RadioButton r = new RadioButton();
                    r.Left = lefts;
                    if (idx == i)
                    {
                        r.Checked = true;
                    }
                    r.Top = tops;
                    r.AutoSize = true;
                    r.Font = new Font("tahoma", 12, FontStyle.Regular);
                    r.Text = c;
                    r.RightToLeft = RightToLeft.Yes;
                    //r.BringToFront();
                    Label l = new Label();
                    l.AutoSize = true;
                    l.Text = r.Text;
                    r.Left -= (int)( r.CreateGraphics().MeasureString(r.Text, r.Font).Width);
                    Controls.Add(r);
                    tops += 40;
                    i++;
                }
                //Qarea.Location = new Point(593, 105);

                pictureBox2.SendToBack();
                index = value;
                Qarea.Text =   (index + 1)+" - "+   qq.qtext ;
               
                // Qarea.RightToLeft = RightToLeft.Yes;
                if (index == test.questions.Count() - 1)
                {
                    nextButton.Text = "عرض النتيجة";
                }
                else
                {
                    nextButton.Text = "التالي";
                }
                testnames.Text = test.testName;
                testnames.Show();
            }
        }
        question qq;
        List<int> records;
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (nextButton.Text == "التالي")
            {
                Index++;
            }
            else
            {
                List<Control> cs = new List<Control>();
                foreach (Control item in Controls)
                {
                    if (item.GetType() == (new RadioButton()).GetType())
                    {
                        cs.Add(item);
                    }
                }
                int ii = 0;
                foreach (Control c in cs)
                {
                    RadioButton r = (RadioButton)c;
                    if (r.Checked)
                    {
                        records[index] = ii;
                    }
                  //  testarea.Controls.Remove(c);
                    ii++;
                }
                test.answer_all(records);
                myuser.record(test.print_result(),test);
                timer1_Tick(sender, e);
                MessageBox.Show(test.print_result());
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (Index  > 0)
                {
                    Index--;
                }
            }
            catch
            {

            }
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
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
            combousers.SelectedIndex = combousers.Items.Count - 1;
            
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (combousers.SelectedIndex != -1)
            {
                var c = MessageBox.Show("هل تريد حذف المستخدم الحالي؟", "?", MessageBoxButtons.YesNo);
                if (c != DialogResult.Yes)
                {
                    return;
                }
                Program.App_data.users.RemoveAt(combousers.SelectedIndex);
            }
            refresh_users();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(myuser.userData + "");
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.App_data.saveme(Program.datanamme);
        }

        private void Qarea_Click(object sender, EventArgs e)
        {

        }

        private void tPMSystemForClincalPsychologistToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usern_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tests_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void testnames_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                if (first)
                {
                    int i = 0;
                    foreach (Control c in Controls)
                    {
                        if (c.GetType()==(new RadioButton()).GetType())
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
