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

        string IssueReportDate = "";
        string FondinIT = "";
        string[] IssueNo = new string[341];
        string[] ReportDate = new string[341];
        string[] PreviousReleaseNo = new string[341];
        string[] FoundinIssueTracker = new string[341];
         

        private void IntalizeReleases()
        //This method will fill up issues info in three arrays
        {
            var sr = new StreamReader(File.OpenRead(@"Issues_nearest_release_new.txt"));
            using (sr)
                {
                    string line;
                    string[] row;

                List<string> list = new List<string>();
                int i = 0;
                int VulnerableFilesCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    row = line.Split(',');
                    IssueNo[i] = row[0];
                    ReportDate[i] = row[1];
                    PreviousReleaseNo[i] = row[2];
                    FoundinIssueTracker[i] = row[3];
                    i++;
                }
            }
            
        }
        private string IssuePreviousRelease(string Issue)
        { 
            bool found=false;
            int i=0;
            int index=-1;
            while (!found && i<342)
            {
                if (IssueNo[i]==Issue)
                {
                    index=i;
                    found=true;
                    IssueReportDate=ReportDate[i];
                    FondinIT = FoundinIssueTracker[i];
                }
                i++;
            }
            if (found)
                return PreviousReleaseNo[index];
            else
                return "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string outputLines="";
            IntalizeReleases();
            button1.Enabled = false;
            var sr = new StreamReader(File.OpenRead(@"Chrome.csv"));
            using (sr) 
            {
                string line;
                string[] row;
                
                List<string> list = new List<string>();

                int VulnerableFilesCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    row = line.Split(',');
                    string vulnerabilityNo = row[0];
                    foreach (string link in line.Split(','))
                    {
                        if (link.StartsWith("http://src.chromium.org"))
                        {
                            // fetch the revision from the website and get the files, author, and date
                            string myURL = link;
                            WebClient c = new WebClient();
                            var data = c.DownloadString(myURL);

                            outputLines = outputLines + vulnerabilityNo +",";

                            string revision = Funcs.Between(data, "<h1>Revi", "</h1>");
                            outputLines = outputLines + revision +",";

                            string Author = Funcs.Between(data, "<th>Author:</th>", "</td>");
                            outputLines = outputLines + Author + ",";
                            string IssueRel = IssuePreviousRelease(vulnerabilityNo);
                            outputLines = outputLines + IssueRel + "," + IssueReportDate + "," + FondinIT;

                             
                            //searching text for URLs (trunk/)
                            Regex mySearchTerm = new Regex(
                                            @"(trunk/*[\w|_|\-|/]+(\.)+\w+)");
                            string compareTxt = "";
                            MatchCollection mactches = mySearchTerm.Matches(data);
                            foreach (Match match in mactches)
                            {
                                if (compareTxt != match.Value)
                                    if (!match.Value.Contains("third_party") && !match.Value.Contains("WebKit") && !match.Value.Contains("webkit"))
                                    if (match.Value.EndsWith(".cc") || match.Value.EndsWith(".h") ||
                                            match.Value.EndsWith(".js") || match.Value.EndsWith(".cpp") ||
                                            match.Value.EndsWith(".mm") || match.Value.EndsWith(".c") ||
                                            match.Value.EndsWith(".py"))
                                {
                                    outputLines = outputLines + "," + match.Value;
                                    VulnerableFilesCount = VulnerableFilesCount + 1;
                                }

                                compareTxt = match.Value;

                            }
                            // for files under branches/
                            Regex mySearchTerm1 = new Regex(
                                            @"(branches/*[\w|_|\-|/]+(\.)+\w+)");
                            compareTxt = "";
                            MatchCollection mactches1 = mySearchTerm1.Matches(data);
                            foreach (Match match1 in mactches1)
                            {
                                if (compareTxt != match1.Value)
                                    if (!match1.Value.Contains("third_party") && !match1.Value.Contains("WebKit") && !match1.Value.Contains("webkit"))
                                    if (match1.Value.EndsWith(".cc") || match1.Value.EndsWith(".h") ||
                                        match1.Value.EndsWith(".js") || match1.Value.EndsWith(".cpp") ||
                                        match1.Value.EndsWith(".mm") || match1.Value.EndsWith(".c") ||
                                        match1.Value.EndsWith(".py"))
                                    {
                                    outputLines = outputLines + "," + match1.Value;
                                    VulnerableFilesCount = VulnerableFilesCount + 1;
                                    }
                                
                                compareTxt = match1.Value;

                            }
                            
                            outputLines = outputLines + "\r\n";

                        }
                    }
                }
                System.IO.File.WriteAllText(@"ChromiumVulToSourceFiles_7_21_12_new.txt", outputLines);
                richTextBox1.Text = "Vulnerable files=" + VulnerableFilesCount;
                button1.Enabled = true;
            }
           
            
            
        }
    }
}
