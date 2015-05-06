namespace LibrariesTranscription.CollectingData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Linq;
    using System.Text;

    using LibrariesTranscription.Model;
    using LibrariesTranscription.Data;

    public class CollectingData
    {
        private const string FileName = "LibraryItems.txt";
        private const string InitFileName = "PageAndRowIndex.txt";
        private const int ProgramSpeedInSeconds = 15;

        private static List<LibraryItem> libraryItemsForAddingToDb;
        private static Database database;

        private static int startPage = 1;
        private static int startRow = 163;

        private static bool IsSaveToDatabase = false;

        public static void Main(string[] args)
        {
            LibraruDbContext context = new LibraruDbContext();
            database = new Database(context);

            InitProgram();
            StartingGetDataFromInternet();
        }

        private static void InitProgram()
        {
            try
            {
                var initData = File.ReadAllText(CollectingData.InitFileName);

                var tokens = initData.Split('/');

                CollectingData.startPage = int.Parse(tokens[0]);
                CollectingData.startRow = int.Parse(tokens[1]);
            }
            catch (Exception)
            {
            }
        }

        private static void StartingGetDataFromInternet()
        {
            libraryItemsForAddingToDb = new List<LibraryItem>(20);
            List<string> libraryItemLines = new List<string>();
            int enteredItemsCount = 1;

            // travers 47 pages
            for (int pageIndex = CollectingData.startPage; pageIndex <= 47; pageIndex++)
            {
                List<string> pageRows = GetPageRowsFrom("http://find.coloradolibraries.org/?page=" + pageIndex);

                // from current page get its library items
                for (int i = CollectingData.startRow; i < pageRows.Count; i++)
                {
                    var line = pageRows[i].Trim();

                    if (line == "<tr>")
                    {
                        i++;

                        // get rows whit data for current item
                        for (int j = i; j < i + 5; j++)
                        {
                            libraryItemLines.Add(pageRows[j].Trim());
                        }

                        LibraryItem libraryItem = GetLibraryItem(libraryItemLines);

                        if (IsSaveToDatabase)
                        {
                            AddToDb(libraryItem);
                        }

                        AddToFile(libraryItem, CollectingData.FileName);

                        // clear rows whit data for current item to use some list for next item
                        libraryItemLines.Clear();

                        SaveCurrentPageAndRowIndexToFile(pageIndex, i);

                        i += 5;

                        Console.WriteLine(enteredItemsCount);
                        enteredItemsCount += 1;

                        // slowing the proces of geting data from server
                        System.Threading.Thread.Sleep(1000 * CollectingData.ProgramSpeedInSeconds);
                    }

                    if (line == "</tbody>")
                    {
                        break;
                    }
                }
            }

            //var rows = new List<string>();

            //rows.Add("<td><a href=\"http://find.coloradolibraries.org/library/view/3154\">Adams County 14</a></td>");
            //rows.Add("<td><a href=\"http://find.coloradolibraries.org/library/view/3154\">Rose Hill Elementary School</a></td>");
            //rows.Add("<td><a href=\"http://find.coloradolibraries.org/library/view/3154\">School</td>");
            //rows.Add("<td><a href=\"http://find.coloradolibraries.org/library/view/3154\">Commerce City</a></td>");
            //rows.Add("<td><a href=\"http://find.coloradolibraries.org/library/view/3154\">Adams</a></td>");

            //var lI = GetLibraryItem(rows);

            //AddToFile(lI, Program.FileName);
            //AddToDb(lI);


            ////var a = GetData("<td><a href=\"http://find.coloradolibraries.org/library/view/101\">Adams County 14</a></td>");
            ////Console.WriteLine(a);
        }

        private static void SaveCurrentPageAndRowIndexToFile(int pageIndex, int rowIndex)
        {
            var data = string.Format("{0}/{1}", pageIndex, rowIndex);

            File.WriteAllText(CollectingData.InitFileName, data);
        }

        private static LibraryItem GetLibraryItem(List<string> libraryItemLines)
        {
            LibraryItem lItem = new LibraryItem();

            lItem.MoreInfoUrl = GetUrlToMoreInfo(libraryItemLines[1]);

            lItem.Organization = GetData(libraryItemLines[0]);
            lItem.Library = GetData(libraryItemLines[1]);
            lItem.Type = GetData(libraryItemLines[2]);
            lItem.City = GetData(libraryItemLines[3]);
            lItem.County = GetData(libraryItemLines[4]);

            var moreInfoLines = GetPageRowsFrom(lItem.MoreInfoUrl);

            lItem.Organization = GetRowMoreInfoData(moreInfoLines[89]);
            lItem.Name = GetRowMoreInfoData(moreInfoLines[95]);
            lItem.URL = GetRowMoreInfoData(moreInfoLines[101]);
            lItem.AddressLine1 = GetRowMoreInfoData(moreInfoLines[113]);
            lItem.AddressLine2 = GetRowMoreInfoData(moreInfoLines[119]);
            lItem.City = GetRowMoreInfoData(moreInfoLines[125]);
            lItem.State = GetRowMoreInfoData(moreInfoLines[131]);
            lItem.ZipCode = GetRowMoreInfoData(moreInfoLines[137]);
            lItem.ZipCode4 = GetRowMoreInfoData(moreInfoLines[143]);
            lItem.County = GetRowMoreInfoData(moreInfoLines[149]);
            lItem.MailingAddressLine1 = GetRowMoreInfoData(moreInfoLines[161]);
            lItem.MailingAddressLine2 = GetRowMoreInfoData(moreInfoLines[167]);
            lItem.MailingCity = GetRowMoreInfoData(moreInfoLines[173]);
            lItem.MailingState = GetRowMoreInfoData(moreInfoLines[179]);
            lItem.MailingZipCode = GetRowMoreInfoData(moreInfoLines[185]);
            lItem.MailingZipCode4 = GetRowMoreInfoData(moreInfoLines[191]);
            lItem.Email = GetRowMoreInfoData(moreInfoLines[203]);
            lItem.LibraryPhoneNumber = GetRowMoreInfoData(moreInfoLines[209]);
            lItem.LibraryFaxNumber = GetRowMoreInfoData(moreInfoLines[215]);
            lItem.CourierCode = GetRowMoreInfoData(moreInfoLines[227]);
            lItem.MarmotCode = GetRowMoreInfoData(moreInfoLines[233]);
            lItem.OCLCCode = GetRowMoreInfoData(moreInfoLines[239]);

            return lItem;
        }

        private static string GetRowMoreInfoData(string row)
        {
            if (row.Trim().StartsWith("<em>-</em>"))
            {
                return "-";
            }

            if (row.Trim().StartsWith("<a"))
            {
                return GetUrlToMoreInfo(row);
            }

            var tokens = row.Split(new char[] { '<' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "-";

            if (tokens.Length >= 1 && tokens[0].Trim() != "</div>")
            {
                result = tokens[0].Trim();
            }

            return result;
        }

        private static string GetUrlToMoreInfo(string str)
        {
            var url = new StringBuilder();
            int count = 0;

            foreach (var item in str)
            {
                if (count == 1 && item != '"')
                {
                    url.Append(item);
                }

                if (item == '"')
                {
                    count += 1;
                }

                if (count == 2)
                {
                    break;
                }
            }

            return url.ToString();
        }

        private static string GetData(string str)
        {
            var data = new StringBuilder();
            int count = 0;

            foreach (var item in str)
            {
                if (count == 2 && item != '<')
                {
                    data.Append(item);
                }

                if (item == '>')
                {
                    count += 1;
                }

                if (count == 2 && item == '<')
                {
                    break;
                }
            }

            return data.ToString();
        }

        private static List<string> GetPageRowsFrom(string pageUrl)
        {
            WebRequest wrGETURL = WebRequest.Create(pageUrl);

            Stream objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            //File.WriteAllText("Test.txt", objReader.ReadToEnd()); // can reed row by row!!!

            List<string> rows = new List<string>();

            while (!objReader.EndOfStream)
            {
                rows.Add(objReader.ReadLine());
            }

            return rows;
        }

        private static void AddToDb(LibraryItem libraryItem)
        {
            // fell buffer
            libraryItemsForAddingToDb.Add(libraryItem);

            if (libraryItemsForAddingToDb.Count >= 20)
            {
                // add to database
                database.Add(libraryItemsForAddingToDb);

                // clear buffer
                libraryItemsForAddingToDb.Clear();
            }
        }

        private static void AddToFile(LibraryItem libraryItem, string fileName)
        {
            File.AppendAllText(fileName, libraryItem.ToString());
        }
    }
}
