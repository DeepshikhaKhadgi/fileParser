using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientFileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //string fileLocation = "C:/medicalreports.txt";
                string fileName = String.Empty;
                string sortBy = String.Empty;

                while (String.IsNullOrEmpty(fileName))
                {
                    Console.Write("Enter File Name : ");
                    fileName = Console.ReadLine();
                }

                Console.Write("Enter sort by (Optional): ");
                sortBy = Console.ReadLine();

                ParseFile parseFile = new ParseFile();
                DataTable dt =parseFile.Parse(fileName, sortBy);

                Console.WriteLine();
                parseFile.DisplayAllContent(dt);
                Console.ReadLine();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File Not Found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : Please try again");
            }
        }
    }
}
