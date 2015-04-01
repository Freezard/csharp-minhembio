using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection; 

namespace TestApp
{
    class Program
    {
        public Reviews reviews;
        public ArrayList list = new ArrayList();

        public Program()
        {
            reviews = new Reviews();
            load();
        //    getAllReviews();
        //    System.Console.WriteLine(addAllReviews());
            updateInformation();
        //    printReviews();
            exportExcel();
            save();
        }

        static void Main(string[] args)
        {
            new Program();
            System.Console.ReadLine();
        }

        public void exportExcel()
        {
            try
            {
                Excel.Application oXL;
                Excel._Workbook oWB;
                Excel._Worksheet oSheet;
                Excel.Range oRng;

                oXL = new Excel.Application();
                oXL.DisplayAlerts = false;

                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                foreach (KeyValuePair<int, Game> game in reviews.getAllGames())
                {
                    int j = 2;
                    oSheet.Cells[game.Value.getNr() * 2, 1] = game.Value.getName();
                    foreach (KeyValuePair<DateTime, int> date in game.Value.getVisitors())
                    {
                        oSheet.Cells[(game.Value.getNr() * 2) - 1, j] = date.Key.ToString("dd/MM/yyyy");
                        oSheet.Cells[game.Value.getNr() * 2, j++] = date.Value;
                    }
                }

                oRng = oSheet.get_Range("B1");
                oRng.EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                oRng = oSheet.get_Range("A1", "B1");
                oRng.EntireColumn.AutoFit();

                oWB.SaveAs(@"E:\Excel.xls", Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
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

        public void getAllReviews()
        {
            string webContents = getPageSource("http://www.minhembio.com/spelrec");

            string expr = "\n<a href=\"/spelrec/(\\d\\d\\d\\d)\" class=\"bildlink\">";

            MatchCollection matches = Regex.Matches(webContents, expr);

            for (int i = matches.Count - 1; i >= 0; i--)
                list.Add(matches[i].Groups[1].Value);
        }

        public int addAllReviews()
        {
            int reviewsAdded = 0;

            foreach (string id in list)
                if (addReview(id))
                    reviewsAdded++;
            
            return reviewsAdded;
        }

        bool addReview(string id)
        {
           if (reviews.containsGame(int.Parse(id)))
               return false;

           string webContents = getPageSource("http://www.minhembio.com/spelrec/" + id);

           string expr;

           if (int.Parse(id) >= 2140)
               expr = "huvudrubrik\">([^<]+)";
           else if(int.Parse(id) >= 1953)
               expr = "huvudrubrik\">Recension: ([^<]+)";
           else expr = "huvudrubrik\">Spelrecension: ([^<]+)";

           string expr2 = "(\\d+) besökare";

           string name = matchExpr(expr, webContents);
           if (name == "")
               return false;
           int visitors = int.Parse(matchExpr(expr2, webContents));

           reviews.addGame(int.Parse(id), name, DateTime.Today, visitors);
           return true;
        }

        bool addInformation(string id)
        {
            foreach (KeyValuePair<DateTime, int> date in reviews.getGame(int.Parse(id)).getVisitors())
                if (date.Key.Year == DateTime.Today.Year && date.Key.Month == DateTime.Today.Month
                    && date.Key.Day == DateTime.Today.Day)
                    return false;

            string webContents = getPageSource("http://www.minhembio.com/spelrec/" + id);

            string expr = "(\\d+) besökare";
            int visitors = int.Parse(matchExpr(expr, webContents));

            reviews.addInformation(int.Parse(id), DateTime.Today, visitors);
            return true;
        }

        void updateInformation()
        {
            foreach (KeyValuePair<int, Game> game in reviews.getAllGames())
            {
                addInformation(Convert.ToString(game.Key));
            }
        }

        void printReviews()
        {
            foreach (KeyValuePair<int, Game> game in reviews.getAllGames())
            {
                Console.WriteLine(game.Key);
                Console.WriteLine(game.Value.getName());
                Console.WriteLine(game.Value.getNr());
                foreach (KeyValuePair<DateTime, int> date in game.Value.getVisitors())
                {
                    Console.WriteLine(date.Key.ToShortDateString());
                    Console.WriteLine(date.Value);
                }
            }
        }

        string getPageSource(string URL)
        {
            WebClient webClient = new WebClient();
            string strSource = webClient.DownloadString(URL);
            webClient.Dispose();

            return strSource;
        }

        string matchExpr(string expr, string text)
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

        void save()
        {
            Stream stream = File.Open("E:\\reviews.dat", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, reviews);
            stream.Close();
        }

        void load()
        {
            Stream stream = File.Open("E:\\reviews.dat", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            reviews = (Reviews)bformatter.Deserialize(stream);
            stream.Close();
        }
    }
}
