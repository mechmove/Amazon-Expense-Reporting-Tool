using System.Globalization;
using System.Text;

namespace Amazon_Expenses_Reporting_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            string SourceFln = "screen scrape, Amazon Orders.txt";
            string TargetCompleteCSV = "AmazonComplete.csv";
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(SourceFln))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                Boolean bGiftCard = false;
                String line = String.Empty;
                string currentdate = string.Empty;
                int LineCtr = 0;
                string PayAmt = string.Empty;
                string BuildLine_CompleteCSV = "date,payment,ordernum,vendor,pay amount" + Environment.NewLine;
                while ((line = streamReader.ReadLine())
                    != null)
                {
                    // Process line
                    if (line.Contains("$"))
                    {
                        //money only
                        line = line.Replace(",", "").Trim() + ",";

                        if (line.IndexOf("-$")>0)
                        {
                            PayAmt = line.Substring(line.IndexOf("-$") + 2);
                        }

                        if (line.IndexOf("+$") > 0)
                        {
                            PayAmt = "-" + line.Substring(line.IndexOf("+$") + 2);
                        }
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
                            BuildLine_CompleteCSV += currentdate + ",";
                        }

                        LineCtr += 1;
                        BuildLine_CompleteCSV += line.Replace(",", " ").Trim() + ",";

                        if (LineCtr.Equals(2) && (line.Contains("Amazon Gift Card used")))
                        {
                            LineCtr = 3;
                            bGiftCard = true;
                        }
                        else if (LineCtr.Equals(4))
                        {
                            if (bGiftCard)
                            {
                                bGiftCard = false;
                                BuildLine_CompleteCSV += ",-" + PayAmt + Environment.NewLine;
                            }
                            else
                            {
                                BuildLine_CompleteCSV += PayAmt + Environment.NewLine;
                            }
                        }
                    }
                }
                File.WriteAllText(TargetCompleteCSV, BuildLine_CompleteCSV.Substring(0, BuildLine_CompleteCSV.Length) + Environment.NewLine);

                Console.Write(TargetCompleteCSV + " created!");
                Console.ReadKey();
            }
        }
    }
}
