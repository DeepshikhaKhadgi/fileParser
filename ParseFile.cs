using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientFileParser
{
    public class ParseFile
    {
        public DataTable Parse(string filename, string sortByField)
        {
            //List of Patients Records
            List<Dictionary<string, string>> patientsFile = new List<Dictionary<string, string>>();

            //Adding patients records
            using (var reader = File.OpenText(filename))
            {
                //to read each line 
                String line = null;
                Dictionary<string, string> keyValue= new Dictionary<string, string>(); ;

                while ((line = reader.ReadLine()) != null)
                {

                    if (string.IsNullOrWhiteSpace(line)) //check for blank line
                        continue;
                    
                    if (line.StartsWith("="))//check for each record end
                    {
                        patientsFile.Add(keyValue); //if each patients record read complete, add that patients record to list of dictionary
                        keyValue = new Dictionary<string, string>(); ;
                        continue;
                    }

                    string[] parsedata = line.Split(new char[] { ':' }, 2);

                    string column = parsedata[0].Trim().ToLower();
                    string value = parsedata.Length < 2 ? string.Empty : parsedata[1].Trim();
                    keyValue.Add(column, value); //add each key value pair of patient's data

                }

            }

            //add in memory table
            var dtPatient = ToDataTable(patientsFile);

            //for sorting
            var dtSortedPatient =sortBy(dtPatient, sortByField.ToLower());

            
            return dtSortedPatient;

        }

        public DataTable sortBy(DataTable dt, string column)
        {
            DataTable sortedTable = null;
            dt.DefaultView.Sort = column;
            sortedTable = dt.DefaultView.ToTable();

            return sortedTable;
        }

        public DataTable ToDataTable(List<Dictionary<string, string>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            //to add distinct field of the report
            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();

            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, string> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }

            return result;
        }

        public void DisplayAllContent(DataTable dt)
        {
            foreach (DataRow dataRow in dt.Rows)
            {
                //foreach (var item in dataRow.ItemArray)
                //{
                //    Console.Write( item + "|");
                //}
                foreach (DataColumn column in dt.Columns)
                {
                    Console.WriteLine(column.ColumnName.ToUpper() + " : " + dataRow[column].ToString());
                }
                Console.WriteLine("\n ========= \n");
            }
        }
    }
}
