using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FilterFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] VulFileList= new string[154];
        private void button1_Click(object sender, EventArgs e)
        {
            int FilesCount = 0;
            string outputLines="";
            button1.Enabled = false;
            string[] lines = System.IO.File.ReadAllLines(@"ChromeV10Files_not_filtered.csv");

            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                if (!line.Contains("third_party") && !line.Contains("WebKit") && !line.Contains("webkit") && !line.Contains("test"))
                                    if (line.EndsWith(".cc") || line.EndsWith(".h") ||
                                            line.EndsWith(".js") || line.EndsWith(".cpp") ||
                                            line.EndsWith(".mm") || line.EndsWith(".c") ||
                                            line.EndsWith(".py"))
                                {
                                    outputLines = outputLines + line+ "\r\n";
                                    FilesCount = FilesCount + 1;
                                }
            }

            System.IO.File.WriteAllText(@"ChromiumFilesV10.txt", outputLines);
            MessageBox.Show("Filter is done!");
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //V10_vul_files.csv
            int FilesCount = 0;
            string outputLines="";
            button1.Enabled = false;
            string[] lines = System.IO.File.ReadAllLines(@"V10_vul_files.csv");

            foreach (string line in lines)
            {
                string[] row = line.Split(',');
                string vulID=row[0];
                string ReportDate=row[3];
                string Version=row[4];
                foreach (string file in row)
                {
                    string filePath = "";
                    if (file.StartsWith("trunk/") || file.StartsWith("branches/"))
                    {
                        if (file.StartsWith("trunk/"))
                            filePath = file;
                        else
                        {
                            filePath = "trunk/" + file.Substring(file.IndexOf("src"));
                        }
                        if (outputLines.IndexOf(filePath) == -1)
                            {
                                outputLines = outputLines + vulID + "," + ReportDate + "," + Version + "," + filePath + "\r\n";
                                FilesCount = FilesCount + 1;
                            }
                    }
                }
            }
            System.IO.File.WriteAllText(@"ChromiumV10VulFiles.txt", outputLines);
            MessageBox.Show("Filter is done! Vulnerable Files="+ FilesCount);
        }

        private int IsVulnerable(string file)
        {
            bool found = false;
            for (int i = 0; i < 154; i++)
            {
                if (VulFileList[i] == file)
                    found = true;
            }
            if (found)
                return 1;
            else
                return 0;
        }
        private void button3_Click(object sender, EventArgs e)
        //classify files to netural or vulnerable: 0 nuetral 1 vulnerable
        {
            //fill up VulFileList array
            button1.Enabled = false;
            string[] lines = System.IO.File.ReadAllLines(@"vul_list_new.csv");
            int i = 0;
            foreach (string line in lines)
            {
                VulFileList[i] = line;
                i++;
            }

            //read list of files and mark each if vulnerable
            int FilesCount = 0;
            string outputLines = "";
            string[] lines1 = System.IO.File.ReadAllLines(@"ChromiumFilesV10.txt");

            foreach (string line in lines1)
            {
                if (IsVulnerable(line)==1)
                {
                    outputLines = outputLines + line + ",1" + "\r\n";
                    FilesCount = FilesCount + 1;
                }
                else
                    outputLines = outputLines + line + ",0" + "\r\n";
            }


            System.IO.File.WriteAllText(@"ChromiumFilesV10Classified.txt", outputLines);
            MessageBox.Show("Filter is done! files count="+FilesCount);
            
        }
        private bool DevExsist(string searchTerm, List<string> L)
        {
            bool found = false;
            int i=0;
            while (!found && i < L.Count)
            {
                if (L[i] == searchTerm)
                    found = true;
                i++;
            }
            return found;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            List<string> ReviewFile = new List<string>();
            List<string> ReviewDevs = new List<string>();
            List<string> FileList = new List<string>();
            string outputlines = "";
            //will associate every file with its review metric: #reviews, #review_devs, #review_distinct_devs
            //steps:
            //1- upload array of reviews and devs, and files
            button4.Enabled = false;
            //upload review files
            string[] lines = System.IO.File.ReadAllLines(@"ReviewSourceFiles_ALL.csv");
            foreach (string line in lines)
            {
                ReviewFile.Add(line);
            }
            //upload review devs
            string[] lines1 = System.IO.File.ReadAllLines(@"ChromeV10Reviewers.csv");
            foreach (string line in lines1)
            {
                ReviewDevs.Add(line);
            }
            //upload files list
            string[] lines2 = System.IO.File.ReadAllLines(@"ChromiumFilesV10Classified.txt");
            foreach (string file in lines2)
            {
                //ReviewDevs.Add(line);
                string[] myRow= file.Split(',');
                string fileName=myRow[0];
                string IsVulnernable=myRow[1];
                int reviewNum = 0;
                int reviewDevNum = 0;
                int reviewDistincDevNum = 0;
                List<string> myReviews = new List<string>();
                List<string> myDevs = new List<string>();
                for (int i = 0; i < ReviewFile.Count; i++)
                {
                    if (ReviewFile[i].Contains(fileName))
                    {
                        reviewNum = reviewNum + 1;
                        string[] row = ReviewFile[i].Split(',');
                        myReviews.Add(row[0]);
                    }
                }

                for (int i = 0; i < myReviews.Count; i++)
                {
                    bool found = false;
                    int j=0;
                    while (!found && j < ReviewDevs.Count)
                    {
                        string[] myR = ReviewDevs[j].Split(',');
                        if (myReviews[i]==myR[0])
                        {
                            found = true;
                            foreach (string N in myR)
                            {
                                if (N.Contains("@"))
                                {
                                    reviewDevNum = reviewDevNum + 1;
                                    //if Dev does not exsist then add
                                    if (!DevExsist(N, myDevs))
                                        myDevs.Add(N);

                                }
                            }       
                        }
                        
                        j++;
                    }
                     
                }
                reviewDistincDevNum = myDevs.Count;
                outputlines = outputlines + fileName + "," + IsVulnernable + "," + reviewNum + "," + reviewDevNum + "," + reviewDistincDevNum + "\r\n";
                
                
            }
            
            System.IO.File.WriteAllText(@"ReviewMetric_7_28.txt", outputlines);
            MessageBox.Show("Done Generating the metric");


        }
    }
}
