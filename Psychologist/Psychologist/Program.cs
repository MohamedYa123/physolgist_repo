using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Psychologist
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            getdata();
            form = new Form3();
            Application.Run(form);
        }
        public static Form3 form;
        public static app App_data;
        public static string datanamme = "Data.x";
        public  enum Gender { Male, female };
        public static void getdata()
        {
            if (!File.Exists(datanamme))
            {
                //App_data = new app();
            }
            else
            {
                App_data = app.load(datanamme);
            }
        }
        public static void store()
        {

        }
    }
}
