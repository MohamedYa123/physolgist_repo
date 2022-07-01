using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Psychologist
{
    [Serializable]
    public class app
    {
        public List<repositry> repos;
        public List<Test> tests;
        public admin a_user;
        public List<user> users;
        public app(string username,string password)
        {
            repos = new List<repositry>();
            tests = new List<Test>();
            a_user = new admin(username, password);
            users = new List<user>();
        }
        public bool check(string username, string password)
        {
            return a_user.check(username, password);
        }
            public void saveme(string datapath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream =
                 new FileStream(datapath, FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            formatter.Serialize(writerFileStream, this);
            // Close the writerFileStream when we are done.
            writerFileStream.Close();

        }

        public static app load(string datapath)
        {
            app tc;
            FileStream readerFileStream = new FileStream(datapath,
        FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            // Reconstruct information of our friends from file.
            tc = (app)formatter.Deserialize(readerFileStream);
            // Close the readerFileStream when we are done
            readerFileStream.Close();
            return tc;
        }
    }
    [Serializable]
    public class admin
    {
        string username;
        string password;
        public admin(string username,string password)
        {
            this.username = username;
            this.password = password;
        }
        public bool check(string username, string password)
        {
            bool h = (username == this.username && password == this.password);
            return h;
        }
    }
    [Serializable]
    public class Test
    {
        public List<question> questions;
        public List<repositry> repos;
        public string testName;
        public int result;
        public Test(string name)
        {
            questions = new List<question>();
            repos = new List<repositry>();
            testName = name;
        }
        public override string ToString()
        {
            return testName;
        }
        public void prepare()
        {
            
        }
        public int answer(int question_id,int answer_id)
        {
           return  questions[question_id].answer(answer_id); 
        }
        public void answer_all(List<int>records)
        {
            foreach(var k in repos)
            {
                k.reset();
            }
            int i = 0;
            foreach(var c in records)
            {
                answer(i, c);
                i++;
            }
        }
        public void addquestion(question q)
        {
            questions.Add(q);
            repos.Add(q.repo);
        }
        public string print_result()
        {
            string result = "";
            foreach(var r in repos)
            {
                result += r.print_result() + "\r\n";
            }
            return result;
        }
        public void saveme(string datapath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream =
                 new FileStream(datapath, FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            formatter.Serialize(writerFileStream, this);
            // Close the writerFileStream when we are done.
            writerFileStream.Close();

        }

        public static Test load(string datapath)
        {
            Test tc;
            FileStream readerFileStream = new FileStream(datapath,
        FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            // Reconstruct information of our friends from file.
            tc = (Test)formatter.Deserialize(readerFileStream);
            // Close the readerFileStream when we are done
            readerFileStream.Close();
            return tc;
        }
        }
    [Serializable]
        public class question
    {
           public List<string> answers;
         public   List<int> degrees;
        public repositry repo;
        int result;
        public    string qtext;
        public question(repositry repo,string qtext)
        {
            answers = new List<string>();
            degrees = new List<int>();
            this.repo = repo;
            this.qtext = qtext;
        }
        public int Result { get { return result; } }
        public int answer(int answer_id)
        {
            if (answer_id == -1)
            {
                return 0;
            }
            result = degrees[answer_id];
            repo.add_result(result);
            return result;
        }
        public override string ToString()
        {
            return qtext;
        }
    }
    [Serializable]
    public class repositry:Object
    {
        string repo_name;
        int result;
        public List<int> Result_ranges;
        public List<string> Result_strings;
        public repositry(string repo_name)
        {
            this.repo_name = repo_name;
            Result_ranges = new List<int>();
            Result_strings = new List<string>();
        }
        public override string ToString()
        {
            return repo_name;
        }
        public void add_result(int i)
        {
            result += i;
        }
        public void reset()
        {
            result = 0;
        }
        public static string operator+(string a,repositry b)
        {
            return a +" "+  b.ToString();
        }
        public static string operator +(repositry a, repositry b)
        {
            return a.ToString() +" "+ b.ToString();
        }
        public string print_result()
        {
            string rs = "";
            int id = 0;
            foreach(var r in Result_ranges)
            {
                if (result >= r)
                {
                    rs = Result_strings[id];
                }
                id++;
            }
            rs = "الدرجة : " + result + "\r\n" + rs;
            return rs;
        }
    }
    [Serializable]
    public class user
    {
        public string name;
        public int age;
        public Bitmap image;
        public Program.Gender gender;
        public List<Test> tests;
        public string desc;
        public List<string> userData=new List<string>();
        public List< session> user_sessions;
        
        public user (string name,int age,Program.Gender gender ,Bitmap image)
        {
            this.name = name;
            this.age = age;
            this.gender = gender;
            this.image = image;
            user_sessions = new List<session>();
        }
        public void record(string res,Test test)
        {
            if (userData == null)
            {
                userData = new List<string>();
            }
            userData.Add( DateTime.Now + "\r\n"+test.testName+"\r\n" + res);
        }
    }
    [Serializable]
    public class session
    {
        public DateTime dtime;
        public string kind;
        public string desc;
    }
}

