using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;
namespace Psychologist
{
    public partial class Form2 : Form
    {
        public static string[] text;
        public static string[] cat;
        public static string[] id;
        public static int questions_counter;
        public Form2()
        {
            InitializeComponent();

            listView1.Columns.Add("Question id");
            listView1.Columns.Add("Question text");
            listView1.Columns.Add("Category");
            listView1.Columns[0].Width = 200;
            listView1.Columns[1].Width = 200;
            listView1.Columns[2].Width = 200;
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
        public Test test;
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        void refreshrepos()
        {
            comboBox2.Items.Clear();
            foreach(var c in test.repos)
            {
                comboBox2.Items.Add(c.ToString());
            }
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
        }
        void refresh_questions()
        {
            listView1.Items.Clear();
            int i = 0;
            foreach(var q in test.questions)
            {
                ListViewItem lv = new ListViewItem(i+"");
                lv.SubItems.Add(q.ToString());
                lv.SubItems.Add(q.repo.ToString());
                listView1.Items.Add(lv);
                i++;
            }
        }
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog of = new SaveFileDialog();
            of.Filter = "Datafiles|*.Dat";
            of.ShowDialog();
            if (of.FileName != "")
            {
                test.testName = testn.Text;
                test.saveme(of.FileName);
                MessageBox.Show("Test saved !");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            repositry rep = new repositry(null);
             qq = new question(rep, "");
            //test.questions.Add(qq);
            panel1.Enabled = true;
            area.Text = "";
            list.Items.Clear();
            mod.Text = "add";
        }
        question qq;
        void show_question(question q)
        {
            area.Text = q.ToString();
            list.Items.Clear();
            refreshrepos();
            int indx =-1;
            int i = 0;
            foreach(var c in test.repos)
            {
                if (c == q.repo)
                {
                    indx = i;
                    break;
                }
                i++;
            }
            if (indx != -1)
            {
                comboBox2.SelectedIndex = indx;
            }
            foreach(var l in q.answers)
            {
                list.Items.Add(l);
            }
        }
        void show_question_without_refresh(question q)
        {
            area.Text = q.ToString();
            list.Items.Clear();
            int indx = -1;
            int i = 0;
            foreach (var c in test.repos)
            {
                if (c == q.repo)
                {
                    indx = i;
                    break;
                }
                i++;
            }
            if (indx != -1)
            {
                comboBox2.SelectedIndex = indx;
            }
            foreach (var l in q.answers)
            {
                list.Items.Add(l);
            }
        }
        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                mod.Text = "add";
                ans.Text = "";
                deg.Value = 1;
            }
            else
            {
                mod.Text = "edit";
                ans.Text = list.SelectedItem.ToString();
                deg.Value =qq.degrees[list.SelectedIndex];
            }
        }

        private void mod_Click(object sender, EventArgs e)
        {
            if (mod.Text == "edit")
            {
                qq.answers[list.SelectedIndex] = ans.Text;
                qq.degrees[list.SelectedIndex] = (int)deg.Value;
                qq.qtext = area.Text;
                mod.Text = "add";
                ans.Text = "";
                deg.Value = 1;
                show_question_without_refresh(qq);
            }
            else
            {
                qq.answers.Add(ans.Text);
                qq.degrees.Add((int)deg.Value);
                mod.Text = "add";
                ans.Text = "";
                qq.qtext = area.Text;
                deg.Value += 1;
                show_question_without_refresh(qq);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1 || comboBox2.SelectedItem ==null)
            {
                if (comboBox2.Text == "")
                {
                    MessageBox.Show("Please select a category or create one");
                    return;
                }
                
            }
           // else
            {
                repositry rep;
                if (comboBox2.SelectedIndex != -1)
                {
                     rep = test.repos[comboBox2.SelectedIndex];
                }
                else
                {
                    rep= new repositry(comboBox2.Text);
                    repocreate rp = new repocreate();
                    rp.repo = rep;
                    rp.ShowDialog();
                    //test.repos.Add(rep);
                }
                qq.qtext = area.Text;
                qq.repo = rep;
                
                bool r = false;
                bool r2=false;
                foreach(var c in test.questions)
                {
                    if (c == qq)
                    {
                        r = true;
                        break;
                    }
                }
                if (!r)
                {
                    test.questions.Add(qq);
                    
                }
                foreach (var c in test.repos)
                {
                    if (c == rep)
                    {
                        r2 = true;
                        break;
                    }
                }
                if (!r2)
                {
                    test.repos.Add(rep);
                }
                area.Text = "";
                list.Items.Clear();
                ans.Text = "";
                deg.Value = 1;
                refresh_questions();
                listView1.Select();
                listView1.Items[listView1.Items.Count - 1].Selected = true;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (list.SelectedIndex != -1)
            {
                qq.answers.RemoveAt(list.SelectedIndex);
                list.Items.RemoveAt(list.SelectedIndex);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                qq = test.questions[listView1.SelectedIndices[0]];
                show_question(qq);
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            test.testName = testn.Text;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        double[,] rates1;
        double[,] rates2;
        bool first;
        private void Form2_Enter(object sender, EventArgs e)
        {

            listView1.FullRowSelect = true;
            if (test == null)
            {
                test = new Test("Untitled test");

            }
            testn.Text = test.ToString();
            refreshrepos();
            refresh_questions();
            /*  for (int i = 0; i < Form4.questions_counter - 1; i++)
              {
                  string[] row = { Form4.id[i], Form4.text[i], Form4.cat[i] };
                  var listViewItem = new ListViewItem(row);
                  listView1.Items.Add(listViewItem);
              }*/

        }
        void readtest()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                InitialDirectory = @"D:\",
                Title = "Import Excel file.",

                CheckFileExists = true,
                CheckPathExists = true,

                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            })
            {
                openFileDialog.Title = "Import Excel file.";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filename = openFileDialog.FileName;
                        using (XLWorkbook workbookx = new XLWorkbook(filename))
                        {
                            IXLWorksheet cr = workbookx.Worksheet(1);

                            var qt = "";
                            for (questions_counter = 1; cr.Row(questions_counter).Cell(2).Value.ToString() != ""; questions_counter++)
                            {
                               qt = cr.Row(questions_counter).Cell(1).Value.ToString();
                                if (qt != "") {
                                    // add question
                                    if (qq != null)
                                    {
                                        guna2Button4_Click( new object(),  new EventArgs());
                                    }
                                    guna2Button1_Click(new object(), new EventArgs());
                                    area.Text= cr.Row(questions_counter).Cell(1).Value.ToString();
                                    var categ = cr.Row(questions_counter).Cell(2).Value.ToString();
                                    int i = 0;
                                    bool iselect=false;
                                    foreach(var c in comboBox2.Items)
                                    {
                                        if (c.ToString()==categ)
                                        {
                                            comboBox2.SelectedIndex = i;
                                            iselect = true;
                                            break;
                                        }
                                        i++;
                                    }
                                    if (!iselect)
                                    {
                                       
                                        comboBox2.SelectedIndex = -1;
                                        comboBox2.Text = categ;
                                    }
                                }
                                else
                                {
                                    ans.Text = cr.Row(questions_counter).Cell(2).Value.ToString();
                                    deg.Value = Convert.ToDecimal(cr.Row(questions_counter).Cell(3).Value.ToString());
                                    mod_Click(new object(), new EventArgs());
                                }

                            }
                            if (qt != "" || true)
                            {
                                // add question
                                if (qq != null)
                                {
                                    guna2Button4_Click(new object(), new EventArgs());
                                }
                            }
                            //    MessageBox.Show("Successfully import data from Excel file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            readtest();
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            try { 
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
