using System;
using System.Text;
using System.Text.Json;

namespace DataConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dataFolderPath = Directory.GetCurrentDirectory();
            StringBuilder sb = new StringBuilder();
            long num = 0;
            String FolderName = dataFolderPath;
            DirectoryInfo tartgetDirectory = new DirectoryInfo(FolderName + "\\data");
            FileInfo[] fileInfos = tartgetDirectory.GetFiles();
            Console.WriteLine("Target directory - " + tartgetDirectory.FullName);

            Console.WriteLine("Process files...");

            sb.AppendLine("No.,Size,Circularity");

            foreach (FileInfo infos in fileInfos)
            {
                if (infos.Extension.ToLower().CompareTo(".json") == 0)
                {
                    String fullFilePath = infos.FullName;
                    string content = File.ReadAllText(fullFilePath);

                    IList<CellData>? cellData = JsonSerializer.Deserialize<IList<CellData>>(content);

                    if (cellData != null)
                    {
                        Console.WriteLine("Valid file - " + infos.Name);

                        foreach (CellData cell in cellData ?? Enumerable.Empty<CellData>())
                        {
                            if (cell.CellType != null && cell.CellType == "Live")
                            {
                                foreach (Cell? data in cell.Cells ?? Enumerable.Empty<Cell>())
                                {
                                    num++;
                                    sb.AppendLine(num + "," + data.Size + "," + data.Circularity);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid file - " + infos.Name);
                    }
                }
            }
            string resultFile = dataFolderPath + "\\result_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            using (StreamWriter wr = new StreamWriter(resultFile))
            {
                wr.WriteLine(sb.ToString());
            }

            Console.WriteLine("Result file location - " + resultFile);
            Console.WriteLine("End process...");
            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }
    }

    public class CellData
    {
        public string? CellType { get; set; }
        public IList<Cell>? Cells { get; set; }
    }

    public class Cell
    {
        public float? Size { get; set; }
        public float? Circularity { get; set; }
    }
}