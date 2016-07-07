using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SQLDeploy
{
    public class VSFileServices
    {
        private string _rootFolder;

        public VSFileServices(string rootFolder)
        {
            this._rootFolder = rootFolder;
        }

        public IEnumerable<string> GetSQLScriptFiles(string schema, DBObjectType objectType)
        {
            string value = objectType == DBObjectType.StoredProcedure ? "Stored Procedures" : objectType.ToString()+"s";

            string path = _rootFolder + "\\" + schema + "\\" + value;

            if (Directory.Exists(path))
                return Directory.GetFiles(path);
            else return null;

        }

        public string GetSQLScriptFileContant(string filePath,DBObjectType objectType)
        {

            
            StringBuilder strBuilder = new StringBuilder();
            var lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {

                if (objectType!=DBObjectType.StoredProcedure && objectType!= DBObjectType.Function)
                {
                    if (lines[i].Trim().Length >= 4 && lines[i].Trim().Substring(0, 4).ToUpper() == "DROP")
                        continue;

                    if (lines[i].Trim().Length >= 9 && lines[i].Trim().Substring(0, 9) == "IF EXISTS")
                        continue;
                }
                
                if (lines[i].Trim().ToUpper().Contains("DROP") && lines[i].Trim().ToUpper().Contains("PROC"))
                    continue;


                if (lines[i].Trim().Length >= 2 && lines[i].Trim().Substring(0, 2).ToUpper() == "GO")
                    continue;




                strBuilder.Append(lines[i].Trim());
              
                strBuilder.Append(Environment.NewLine);
            }

            return strBuilder.ToString();
        }

        public string GetObjectName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }


  
    }
}
