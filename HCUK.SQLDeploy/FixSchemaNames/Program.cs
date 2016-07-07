using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FixSchemaNames
{
    class Program
    {
        const string vsFolder = @"C:\Users\ksalama\Source\Workspaces\Single Customer View\ArgosSCV\ArgosSCV\dv\Tables";

        static void Main(string[] args)
        {
            foreach (string file in Directory.GetFiles(vsFolder))
            {
                string content = File.ReadAllText(file);
                content = content.Replace("BatchId", "BATCH_ID");
                content = content.Replace("LoadDate", "LOAD_DATE");


                using (StreamWriter writer = new StreamWriter(file, false))
                {
                    writer.Write(content);
                    writer.Flush();
                    writer.Close();
                }

            }


            //foreach (string file in Directory.GetFiles(vsFolder))
            //{
            //    StringBuilder content = new StringBuilder();

            //    if (file.Contains("LNK")) //&& file.Contains("HUB"))
            //    {
            //        var lines = File.ReadAllLines(file);

            //        for (int i = 0; i < lines.Length; i++)
            //        {
            //            string line = lines[i].Trim();

            //            //if (line.Contains("SELECT") && !line.Contains("DISTINCT"))
            //            //{
            //            //    content.AppendLine(line.Replace("SELECT", "SELECT DISTINCT"));
            //            //}
            //            //else 
            //            if (line.Contains("DLV_"))
            //            {
            //                int startIndex = line.IndexOf("vw_");
            //                int endIndex = line.IndexOf("_LNK_", 20);

            //                content.AppendLine("[stg].[" + line.Substring(startIndex, endIndex - startIndex) + "]");
            //            }
            //            else
            //                content.AppendLine(line.Replace("lnd.", "stg.").Replace("[lnd].", "[stg]."));

            //        }

            //        //string value = content.ToString();


            //        using (StreamWriter writer = new StreamWriter(file, false))
            //        {
            //            writer.Write(content.ToString());
            //            writer.Flush();
            //            writer.Close();
            //        }
            //    }
            //}
        }
    }
}
