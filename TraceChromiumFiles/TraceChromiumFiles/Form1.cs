using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace TraceChromiumFiles
{

        
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            string outputLines="";
            var sr = new StreamReader(File.OpenRead(@"Chrome.csv"));
            using (sr) 
            {
                string line;
                string[] row;
                
                List<string> list = new List<string>();
                
                while ((line = sr.ReadLine()) != null)
                {
                    row = line.Split(',');
                    string vulnerabilityNo = row[0];
                    foreach (string link in line.Split(','))
                    {
                        if (link.StartsWith("http://src.chromium.org"))
                        {
                            // fetch the revision from the website and get the files, author, and date
                            //richTextBox1.Text = richTextBox1.Text + link + "\r\n";
                            string myURL = link;
                            WebClient c = new WebClient();
                            var data = c.DownloadString(myURL);

                            outputLines = outputLines + vulnerabilityNo +",";

                            string revision = Funcs.Between(data, "<h1>Revi", "</h1>");
                            outputLines = outputLines + revision +",";

                            string Author = Funcs.Between(data, "<th>Author:</th>", "</td>");
                            outputLines = outputLines + Author + ",";

                            string Date = Funcs.Between(data, "<th>Date:</th>", "UTC <em>");
                            outputLines = outputLines + Date ;

                             
                            //searching text for URLs
                            Regex mySearchTerm = new Regex(
                                            @"(trunk/*[\w|_|\-|/]+(\.)+\w+)");
                            string compareTxt = "";
                            MatchCollection mactches = mySearchTerm.Matches(data);
                            foreach (Match match in mactches)
                            {
                                if (compareTxt != match.Value)
                                    outputLines = outputLines + "," + match.Value ;

                                compareTxt = match.Value;

                            }
                            outputLines = outputLines + "\r\n";

                        }
                    }
                }
                System.IO.File.WriteAllText(@"ChromiumVulToFiles.txt", outputLines);
                MessageBox.Show("Done!");
            }
           
            
            
        }
    }
}
