using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Amazon_Expenses_Reporting_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            string SourceFln = "screen scrape, Amazon Orders.txt";
            string TargetMoneyOnly = "AmazonMoneyOnly.csv";
            string TargetCompleteCSV = "AmazonComplete.csv";
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(SourceFln))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                Boolean bFlag = false;
                String line = String.Empty;
                string currentdate = string.Empty;
                int LineCtr = 0;
                string BuildLine_MoneyOnlyCSV = "desc,payment" + Environment.NewLine;
                string PayAmt = string.Empty;
                string BuildLine_CompleteCSV = "date,payment,ordernum,vendor,pay amount" + Environment.NewLine;
                string MoneyOnlyCSV = string.Empty;
                while ((line = streamReader.ReadLine())
                    != null)
                {
                    // Process line
                    if (line.Contains("$"))
                    {
                        //money only
                        line = line.Replace(",", "").Trim() + ",";
                        BuildLine_MoneyOnlyCSV += line;
                        PayAmt = BuildLine_MoneyOnlyCSV.Substring(BuildLine_MoneyOnlyCSV.IndexOf("-$") + 2);
                        BuildLine_MoneyOnlyCSV += PayAmt;
                        MoneyOnlyCSV += BuildLine_MoneyOnlyCSV.Substring(0, BuildLine_MoneyOnlyCSV.Length) + Environment.NewLine;
                        BuildLine_MoneyOnlyCSV = string.Empty;
                    }

                    // full report
                    DateTime temp;
                    if (DateTime.TryParse(line, out temp))
                    {
                        currentdate = Convert.ToDateTime(line).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        LineCtr = 1;
                        BuildLine_CompleteCSV += currentdate + ",";
                    }
                    else
                    {
                        if (LineCtr.Equals(4))
                        {
                            LineCtr = 1;
                            //BuildLine_CompleteCSV += "no date" + ",";
                            BuildLine_CompleteCSV += currentdate + ",";
                        }

                        LineCtr += 1;
                        BuildLine_CompleteCSV += line.Replace(",", " ").Trim() + ",";

                        if (LineCtr.Equals(2) && (line.Contains("Amazon Gift Card used")))
                        {
                            LineCtr = 3;
                            bFlag = true;
                        }
                        else if (LineCtr.Equals(4))
                        {
                            if (bFlag)
                            {
                                bFlag = false;
                                BuildLine_CompleteCSV += "," + PayAmt + Environment.NewLine;
                            }
                            else
                            {
                                BuildLine_CompleteCSV += PayAmt + Environment.NewLine;
                            }

                        }
                    }
                }
                File.WriteAllText(TargetCompleteCSV, BuildLine_CompleteCSV.Substring(0, BuildLine_CompleteCSV.Length) + Environment.NewLine);
                File.WriteAllText(TargetMoneyOnly, MoneyOnlyCSV);

                Console.Write(TargetCompleteCSV + " created!");
                Console.ReadKey();
            }
        }
    }
}
