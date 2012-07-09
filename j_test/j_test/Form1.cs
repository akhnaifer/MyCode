using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.IO;

namespace j_test
{
    public partial class Form1 : Form
    {
        public class Result
        {
            public string description { get; set; }
            public string created { get; set; }
            public List<string> cc { get; set; }
            public List<string> reviewers { get; set; }
            public string owner_email { get; set; }
            public List<string> patchsets { get; set; }
            public string modified { get; set; }
            public bool @private { get; set; }
            public string base_url { get; set; }
            public bool closed { get; set; }
            public string owner { get; set; }
            public bool commit { get; set; }
            public int issue { get; set; }
            public string subject { get; set; }
        }

        public class RootObject
        {
            public string cursor { get; set; }
            public List<Result> results { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string myURL = "http://codereview.chromium.org/search?closed=1&owner=&reviewer=&repo_guid=&base=svn%3A%2F%2Fsvn.chromium.org%2Fchrome%2Ftrunk%2Fsrc%2F&private=1&commit=1&created_before=&created_after=&modified_before=&modified_after=&format=json&keys_only=False&with_messages=False&cursor=&limit=50";
                //"http://codereview.chromium.org/search?closed=2&owner=&reviewer=&repo_guid=&base=svn%3A%2F%2Fsvn.chromium.org%2Fchrome%2Ftrunk%2F&private=1&commit=1&created_before=&created_after=&modified_before=&modified_after=&format=json&keys_only=False&with_messages=False&cursor=&limit=50";
            string mycursor = "x";
            
            WebClient c = new WebClient();
            
            string myLine = "";
            // create a writer and open the file
            TextWriter tw = new StreamWriter("myreviews_6_17_final.txt");
            while (mycursor != "")
            {
                var data = c.DownloadString(myURL);
                //var data = c.DownloadString("http://codereview.chromium.org/api/6351/");

                JavaScriptSerializer ser = new JavaScriptSerializer();
                RootObject Reviews = ser.Deserialize<RootObject>(data);

                mycursor = Convert.ToString(Reviews.cursor);
               
                myURL = "http://codereview.chromium.org/search?closed=1&owner=&reviewer=&repo_guid=&base=svn%3A%2F%2Fsvn.chromium.org%2Fchrome%2Ftrunk%2Fsrc%2F&private=1&commit=1&created_before=&created_after=&modified_before=&modified_after=&format=json&keys_only=False&with_messages=False&cursor=" + mycursor + "&limit=50";

                for (int i = 0; i < Reviews.results.Count(); i++)
                {
                    myLine = Reviews.results[i].issue + "\t" + Reviews.results[i].subject + "\t" +
                        /*Reviews.results[i].description +*/ "\t" + Reviews.results[i].created + "\t" +
                        Reviews.results[i].modified + "\t" + Reviews.results[i].owner + "\t" +
                        Reviews.results[i].owner_email + "\t" + Reviews.results[i].base_url + "\t" +
                        Reviews.results[i].closed + "\t" + Reviews.results[i].commit + "\t" +
                        Reviews.results[i].@private;

                    myLine = myLine + "\t" + string.Join("\t", Reviews.results[i].reviewers.ToArray());
                    //myLine = myLine + "\t" + string.Join("\t", Reviews.results[i].cc.ToArray());
                    //myLine = myLine + "\t" + string.Join("\t", Reviews.results[i].patchsets.ToArray());
                    
                    /*for (int j = 0; j < Reviews.results[i].reviewers.Count; j++)
                    //adding reviewers
                    {
                        myLine = myLine + "\t" + Reviews.results[i].reviewers[j];
                    }
                    
                    for (int j = 0; j < Reviews.results[i].cc.Count; j++)
                    //adding cc
                    {
                        myLine = myLine + "\t" + Reviews.results[i].cc[j];
                    }

                    for (int j = 0; j < Reviews.results[i].cc.Count; j++)
                    //adding cc
                    {
                        myLine = myLine + "\t" + Reviews.results[i].cc[j];
                    }
                    */
                    //MessageBox.Show("Round=" + i + " " + Convert.ToString(Reviews.results.Count()));
                    
                    // write a line of text to the file
                    tw.WriteLine(myLine);
                    myLine = "";
                }
                myLine = "";
            }
            // close the stream
            tw.Close();
            /*for (int i = 0; i < Reviews.results.Count; i++)
            {
                MessageBox.Show(Convert.ToString(Reviews.results[i].issue + " " + Reviews.results[i].description));
            }
            */
            /*var strLines = data.Split("\\n");
            foreach (var i in strLines)
            {
                //var obj = JSON.parse(strLines[i]);
                //console.log(obj.a);
                JObject o = JObject.Parse(i);
                string owner_email = Convert.ToString(o["owner_email"]);
                string cc = Convert.ToString(o["cc"]);
                string issue = Convert.ToString(o["issue"]);
                string subject = Convert.ToString(o["subject"]);
                string commit = Convert.ToString(o["commit"]);
                string owner = Convert.ToString(o["owner"]);
                string modified = Convert.ToString(o["modified"]);
                string patchsets = Convert.ToString(o["patchsets"]);
                string created = Convert.ToString(o["created"]);
                string description = Convert.ToString(o["description"]);
                MessageBox.Show(issue + " " + owner_email);
            
            }
             */ 
            /*for (var i in strLines) 
            {
             var obj = JSON.parse(strLines[i]);
             console.log(obj.a);
            }*/
            //Console.WriteLine(data);

            /*
            JObject o = JObject.Parse(data);
            string owner_email = Convert.ToString(o["owner_email"]); 
            string cc = Convert.ToString(o["cc"]);
            string issue = Convert.ToString(o["issue"]);
            string subject = Convert.ToString(o["subject"]);
            string commit = Convert.ToString(o["commit"]);
            string owner = Convert.ToString(o["owner"]);
            string modified = Convert.ToString(o["modified"]);
            string patchsets = Convert.ToString(o["patchsets"]);
            string created = Convert.ToString(o["created"]);
            string description = Convert.ToString(o["description"]);
            */
            
            
            /*Console.WriteLine("Name: " + o["name"]);
            Console.WriteLine("Email Address[1]: " + o["email"][0]);
            Console.WriteLine("Email Address[2]: " + o["email"][1]);
            Console.WriteLine("Website [home page]: " + o["websites"]["home page"]);
            Console.WriteLine("Website [blog]: " + o["websites"]["blog"]);
            Console.ReadLine();
            */

        }
    }
}
