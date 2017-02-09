﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using HtmlAgilityPack;
using System.Configuration;
using DevComponents.DotNetBar;

namespace MinhembioStats
{
    public partial class mainForm : Form
    {
        private Reviews reviews;

        public mainForm()
        {
            InitializeComponent();
            initializeRangeControl();
            loadData();
            printReviews();
            printLastUpdated();
            this.FormClosing += mainForm_FormClosing;
        }

        private void mainForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            saveData();
        }

        private void buttonUpdateAllGames_Click(object sender, EventArgs e)
        {
            int added = addAllReviews(getAllReviews());

            MessageBox.Show(added + " recensioner tillagda");

            int updated = updateAllInformation();
            MessageBox.Show(updated + " recensioner uppdaterade");

            if (updated != 0 || added != 0)
            {
                printReviews();                
                reviews.setLastUpdated(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                printLastUpdated();
            }
        }

        private void buttonExportExcel_Click(object sender, EventArgs e)
        {
            exportExcel();
        }

        // Sets the maximum range to the number of review pages
        private void initializeRangeControl()
        {
            rangeSlider.Maximum = getPages();
            rangeSlider.Value = new RangeValue(int.Parse(ConfigurationManager.AppSettings["MinPage"]),
                int.Parse(ConfigurationManager.AppSettings["MaxPage"]));
        }

        // Returns the number of review pages
        private int getPages()
        {
            string webContents = "http://www.minhembio.com/spelrec";

            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load(webContents);

            HtmlNode mainNode = doc.DocumentNode.SelectSingleNode("//div[@id='artikel_lista']");
            HtmlNode pagesNode = mainNode.SelectSingleNode("..//div[@class='pagenavarea']");
            return int.Parse(pagesNode.InnerText.Split('&')[0]);
        }

        // Finds and returns all review ids on the net
        private ArrayList getAllReviews()
        {
            string webContents = "http://www.minhembio.com/spelrec/sida/";
            int minPage = rangeSlider.Value.Min;
            int maxPage = rangeSlider.Value.Max;

            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load(webContents);

            ArrayList reviews = new ArrayList();
            
            HtmlNode mainNode = doc.DocumentNode.SelectSingleNode("//div[@id='artikel_lista']");

            for (int i = minPage; i <= maxPage; i++)
            {
                doc = hw.Load(webContents + i);

                mainNode = doc.DocumentNode.SelectSingleNode("//div[@id='artikel_lista']");

                foreach (HtmlNode reviewNode in mainNode.SelectNodes("..//a[@class='litenrubrik']"))
                {
                    HtmlAttribute attribute = reviewNode.Attributes["href"];
                    reviews.Add(Regex.Split(attribute.Value, @"^\D*")[1]);
                }
            }

            return reviews;
        }

        // Adds a list of reviews
        private int addAllReviews(ArrayList list)
        {
            int reviewsAdded = 0;
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;
            progressBar.Visible = true;

            foreach (string id in list)
            {
                if (addReview(id))
                    reviewsAdded++;
                progressBar.Value++;
            }

            progressBar.Visible = false;
            return reviewsAdded;
        }

        // Adds a single review
        private bool addReview(string id)
        {
            try
            {
                if (reviews.containsGame(id))
                    return false;

                string webContents = "http://www.minhembio.com/spelrec/" + id;

                HtmlWeb hw = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hw.Load(webContents);
                HtmlNode nodeName = doc.DocumentNode.SelectSingleNode("//td[@class='article-head']//h1");
                HtmlNode nodeVisitors = doc.DocumentNode.SelectSingleNode("//td[@class='article-head']//span");
                HtmlNode nodeAuthor = doc.DocumentNode.SelectSingleNode("//td[@class='article-head']//a");
                HtmlNode nodeAuthorOld = doc.DocumentNode.SelectSingleNode("//p[contains(., 'Text av') or contains(.,'Skribent')]");

                int intID = int.Parse(id);
                string expr;
                string name;
                int visitors;
                string author = "";

                if (intID >= 2140)
                    name = nodeName.InnerText;
                else if (intID >= 1953)
                {
                    expr = "Recension: ";
                    name = Regex.Split(nodeName.InnerText, expr)[1];
                }
                else
                {
                    expr = "Spelrecension: ";
                    name = Regex.Split(nodeName.InnerText, expr)[1];
                }

                visitors = int.Parse(Regex.Split(nodeVisitors.InnerText, "(\\d+) besökare")[1]);

                if (intID >= 2343)
                    author = nodeAuthor.InnerText.Trim();
                else if (intID == 2304 ||intID == 2065)
                    author = "Zoiler/Filip_M";
                else if (intID >= 1954)
                    author = Regex.Split(nodeAuthorOld.InnerText, "Skribent: ")[1].Trim();
                else if (intID >= 1939)
                    author = Regex.Split(nodeAuthorOld.InnerText, "Text av: ")[1];
                else author = Regex.Split(nodeAuthorOld.InnerText, "Text av ")[1];

                reviews.addGame(id, name, author, DateTime.Today, visitors);
                return true;
            }
            catch (WebException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
        }

        // Updates information on a single review
        private bool updateInformation(string id)
        {
            try
            {
                foreach (KeyValuePair<DateTime, int> date in reviews.getGame(id).getVisitors())
                    if (date.Key.Year == DateTime.Today.Year && date.Key.Month == DateTime.Today.Month
                        && date.Key.Day == DateTime.Today.Day)
                        return false;

                string webContents = "http://www.minhembio.com/spelrec/" + id;

                HtmlWeb hw = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hw.Load(webContents);

                HtmlNode nodeVisitors = doc.DocumentNode.SelectSingleNode("//td[@class='article-head']//span");
                int visitors = int.Parse(Regex.Split(nodeVisitors.InnerText, "(\\d+) besökare")[1]);

                reviews.addInformation(id, DateTime.Today, visitors);
                return true;
            }
            catch (WebException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
        }

        // Updates information on every review
        private int updateAllInformation()
        {
            int gamesUpdated = 0;
            int mostVisitors = -1;
            int leastVisitors = int.MaxValue;
            string mostVisitorsGame = "";
            string leastVisitorsGame = "";
            progressBar.Value = 0;
            progressBar.Maximum = reviews.getAllGames().Count;
            progressBar.Visible = true;

            foreach (KeyValuePair<string, Game> game in reviews.getAllGames())
            {
                if (updateInformation(game.Key))
                {
                    gamesUpdated++;
                    int i = game.Value.getVisitors().Values[game.Value.getVisitors().Count - 1] - game.Value.getVisitors().Values[game.Value.getVisitors().Count - 2];
                    if (i > mostVisitors)
                    {
                        mostVisitors = i;
                        mostVisitorsGame = game.Value.getName();
                    }
                    if (i < leastVisitors)
                    {
                        leastVisitors = i;
                        leastVisitorsGame = game.Value.getName();
                    }
                }
                progressBar.Value++;
            }

            if (gamesUpdated != 0)
            {
                labelMostVisitors.Text = "Flest nya besökare:\n" + mostVisitorsGame + " (" + mostVisitors + ")";
                labelMostVisitors.Visible = true;
                labelLeastVisitors.Text = "Minst nya besökare:\n" + leastVisitorsGame + " (" + leastVisitors + ")";
                labelLeastVisitors.Visible = true;
            }
            progressBar.Visible = false;

            return gamesUpdated;
        }

        // Prints the last update
        private void printLastUpdated()
        {
            labelLastUpdated.Text = "Senast uppdaterad:\n" + reviews.getLastUpdated();
        }

        // Prints the name of all reviews
        private void printReviews()
        {
            listBox.Items.Clear();

            foreach (KeyValuePair<string, Game> game in reviews.getAllGames())
                listBox.Items.Add(game.Value.getName());
        }

        // Returns the page source as a string
        private string getPageSource(string URL)
        {
            WebClient webClient = new WebClient();
            string strSource = webClient.DownloadString(URL);
            webClient.Dispose();

            return strSource;
        }

        // Matches an expression and returns if successful
        private string matchExpr(string expr, string text)
        {
            Match m = Regex.Match(text, expr);

            if (m.Success)
                return m.Groups[1].Value;
            else
            {
                Console.WriteLine("No match!");
                return "";
            }
        }

        // Saves data to disk and config
        private void saveData()
        {
            // Save review data
            Stream stream = File.Open("mhstats.dat", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, reviews);
            stream.Close();

            // Save config
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

           confCollection["MinPage"].Value = rangeSlider.Value.Min.ToString();
           confCollection["MaxPage"].Value = rangeSlider.Value.Max.ToString();

           configManager.Save(ConfigurationSaveMode.Modified);
           ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }

        // Loads data from disk
        private void loadData()
        {
            try
            {
                Stream stream = File.Open("mhstats.dat", FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                reviews = (Reviews)bformatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException){
                reviews = new Reviews();}
        }

        // Exports data to excel
        private void exportExcel()
        {
            try
            {
                Excel.Application oXL;
                Excel._Workbook oWB;
                Excel._Worksheet oSheet;
                //Excel.Range oRng;

                oXL = new Excel.Application();
                oXL.DisplayAlerts = false;

                oWB = (Excel._Workbook)oXL.Workbooks.Add(Missing.Value);
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                
                Dictionary<string, int> dates = new Dictionary<string, int>();
                int row = 2;

                foreach (KeyValuePair<string, Game> game in reviews.getAllGames())
                {                    
                    oSheet.Cells[game.Value.getNr() + 1, 1] = game.Value.getName();
                    
                    if (game.Value.getAuthor().Equals("Zoiler"))
                        oSheet.Rows[game.Value.getNr() + 1].Font.Color = System.Drawing.Color.Blue;
                    else if (game.Value.getAuthor().Equals("Filip_M"))
                        oSheet.Rows[game.Value.getNr() + 1].Font.Color = System.Drawing.Color.Red;
                    else if (game.Value.getAuthor().Equals("Freezard"))
                        oSheet.Rows[game.Value.getNr() + 1].Font.Color = System.Drawing.Color.Green;
                    else oSheet.Rows[game.Value.getNr() + 1].Font.Color = System.Drawing.Color.Black;

                    int i = 0;

                    foreach (KeyValuePair<DateTime, int> visitors in game.Value.getVisitors())
                    {
                        string date = visitors.Key.ToString("yyyy/MM/dd");
                        if (!dates.ContainsKey(date))
                        {
                            dates.Add(date, row);
                            oSheet.Cells[1, row++] = date;
                        }

                        if (i > 0)
                        {
                            int deltaVisitors = visitors.Value - game.Value.getVisitors().Values[i - 1];
                            oSheet.Cells[game.Value.getNr() + 1, dates[date]] = visitors.Value + " (" + deltaVisitors + ")";
                        }
                        else oSheet.Cells[game.Value.getNr() + 1, dates[date]] = visitors.Value;
                        i++;
                    }
                }

                oSheet.Rows.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; 
                oSheet.Columns.AutoFit();

                if (Directory.GetCurrentDirectory().EndsWith(@"\"))
                    oWB.SaveAs("mhstats.xls", Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                else oWB.SaveAs(Directory.GetCurrentDirectory() + @"\mhstats.xls", Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
  
                oWB.Close(true);
                oXL.Quit();

                /*                oSheet.Cells[1, 1] = "First Name";
                                oSheet.Cells[1, 2] = "Last Name";
                                oSheet.Cells[1, 3] = "Full Name";
                                oSheet.Cells[1, 4] = "Salary";

                                oSheet.get_Range("A1", "D1").Font.Bold = true;
                                oSheet.get_Range("A1", "D1").VerticalAlignment =
                                    Excel.XlVAlign.xlVAlignCenter;

                                string[,] saNames = new string[5, 2];

                                saNames[0, 0] = "John";
                                saNames[0, 1] = "Smith";
                                saNames[1, 0] = "Tom";
                                saNames[1, 1] = "Brown";
                                saNames[2, 0] = "Sue";
                                saNames[2, 1] = "Thomas";
                                saNames[3, 0] = "Jane";
                                saNames[3, 1] = "Jones";
                                saNames[4, 0] = "Adam";
                                saNames[4, 1] = "Johnson";

                                oSheet.get_Range("A2", "B6").Value2 = saNames;

                                oRng = oSheet.get_Range("C2", "C6");
                                oRng.Formula = "=A2 & \" \" & B2";

                                oRng = oSheet.get_Range("D2", "D6");
                                oRng.Formula = "=RAND()*100000";
                                oRng.NumberFormat = "$0.00";

                                oRng = oSheet.get_Range("A1", "D1");
                               oRng.EntireColumn.AutoFit();
                */
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                System.Console.WriteLine(errorMessage, "Error");
            }
        }
    }
}
