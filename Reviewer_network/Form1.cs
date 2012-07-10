using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;


namespace Reviewer_network
{
    public partial class Form1 : Form
    {
        string[] Developer = new string[1500];
        int NumberOfDevelopers = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private int Find_index(string Dev)
        // this method find developer in their index
        {
            for (int i = 0; i < NumberOfDevelopers; i++)
            {
                if (Developer[i] == Dev)
                    return i;
            }
            
            NumberOfDevelopers++;
            Developer[NumberOfDevelopers - 1] = Dev;
            return NumberOfDevelopers - 1;
          
        }
        private void output_index()
        //output a file with every developer index
        {
            string outputLines = "";
            for (int i = 0; i < NumberOfDevelopers; i++)
            {
                outputLines = outputLines + i + "," + Developer[i] + "\r\n";
            }
            
            System.IO.File.WriteAllText(@"Dev_Index.txt", outputLines);
            MessageBox.Show("Dev index done!!");

        }

        private void Generate_Adj_matrix()
        // this method will generate an ajd matrix for reviewer network
        {
            int[,] Adj = new int[NumberOfDevelopers, NumberOfDevelopers];
            double[,] Adj_prob = new double[NumberOfDevelopers, NumberOfDevelopers];

            var sr = new StreamReader(File.OpenRead(@"Input_Index.txt"));
            using (sr)
            {
                string line;
                List<string> list = new List<string>();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] row1 = line.Split(',');
                    int s = row1.Length;
                    if (row1.Length > 1)
                    {
                        for (int i = 0; i < s; i++)
                            for (int j = i; j < s; j++)
                            {
                                int Val1 = Int32.Parse(row1[i]);
                                int Val2 = Int32.Parse(row1[j]);
                                if (Val1 != Val2)
                                {
                                    Adj[Val1, Val2] = Adj[Val1, Val2]+1;
                                    Adj[Val2, Val1] = Adj[Val2, Val1]+1;
                                }

                            }
                    }
                }
            }
                    //Now we will calculate the probablities on another array Adj_prob
                    //Adj_prob
                    for (int i = 0; i < NumberOfDevelopers; i++)
                    { 
                        int sum=0;
                        //sum the total of each row
                        for (int j = 0; j < NumberOfDevelopers; j++)
                            sum = sum + Adj[i, j];
                        for (int j = 0; j < NumberOfDevelopers; j++)
                            if ((i != j) && (Adj[i, j] != 0))
                            {
                                Adj_prob[i, j] = (double)Adj[i, j] / sum;
                                //Adj_prob[j, i] = Adj[i, j] / sum;
                            }

                    }
                    
                    // now output two files for adj and adj_prob
                    //string output_adj = "";
                    //string output_adj_prob = "";
                    bool started=true;
                    string[] Adj_lines= new string[NumberOfDevelopers];
                    string[] Adj_prob_lines = new string[NumberOfDevelopers];
                    for (int i = 0; i < NumberOfDevelopers; i++)
                    {
                        for (int j = 0; j < NumberOfDevelopers; j++)
                        {
                            if (!started)
                            {
                                Adj_lines[i] = Adj_lines[i] + "," + Convert.ToString(Adj[i, j]);
                                Adj_prob_lines[i] = Adj_prob_lines[i] + "," + Convert.ToString(Adj_prob[i, j]);
                            }
                            else
                            {
                                Adj_lines[i] = Adj_lines[i] + Convert.ToString(Adj[i, j]);
                                Adj_prob_lines[i] = Adj_prob_lines[i] + Convert.ToString(Adj_prob[i, j]);
                                started = false;
                            }
                        }
                        
                        //output_adj_prob = output_adj_prob + "\r\n";
                        //output_adj = output_adj + "\r\n";
                        started = true;
                    }
                    MessageBox.Show("finish output prep");
                    //IO stream to text files
                    System.IO.File.WriteAllLines(@"Adj.txt", Adj_lines);
                    System.IO.File.WriteAllLines(@"Adj_prob.txt", Adj_prob_lines);
                    MessageBox.Show("Adj's are Done!");
                    textBox1.Text = Adj_prob[0, 1].ToString("G");
                    
                

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* this method will construct an index of all developers and
               will output two files:
               A- Developers_index.txt -> has an index no. for each developer
               B- A new input file which has index numbers for developers instead of emails 
             */ 

            string outputLines="";
            var sr = new StreamReader(File.OpenRead(@"reviews_input_file.csv"));
            using (sr)
            {
                string line;
                List<string> list = new List<string>();
                bool comma_flag = false;
                while ((line = sr.ReadLine()) != null)
                {
                    foreach (string D in line.Split(','))
                    {

                        if (D != "")
                        {
                            if (comma_flag)
                                outputLines = outputLines + ",";
                            //string myDev = D;
                            outputLines = outputLines + Find_index(D);
                            comma_flag = true;
                        }
                    }
                    outputLines = outputLines + "\r\n";
                    comma_flag = false;
                }
                System.IO.File.WriteAllText(@"Input_Index.txt", outputLines);
                MessageBox.Show("Input index is Done!");
                output_index();
                Generate_Adj_matrix();

            }
           
            
            
        
        }
    }
}
