using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDeploy
{
    class Program
    {
        const string dwConnectionString = "Server=tcp:avp01aadw.database.windows.net,1433; Database=AVP01AADWDev; User ID= ADWAdmin@avp01aadw; Password=A1*!v7Lz; Encrypt=True; TrustServerCertificate=False; Connection Timeout=0;";
        const string vsRootFolder = @"C:\Users\ksalama\Documents\Argos Source Code\Single Customer View\Argos.SCV";
        public const string outputFile = @"C:\Users\ksalama\documents\outputscript.sql";
        public const string errorFile = @"C:\Users\ksalama\documents\errors.txt";
        const int trials = 10;


        static Queue<string> failedObjects = new Queue<string>();

        static void Main(string[] args)
        {
            int curretIteration = 0;

            if (System.IO.File.Exists(outputFile))
                System.IO.File.Delete(outputFile);

            if (System.IO.File.Exists(errorFile))
                System.IO.File.Delete(errorFile);

            List<string> schemas = new List<string>()
            {
                //"ref",
                "tmp",
                "mds",
                "etl",
                "lnd",
                //"ext",
                "stg",               
                "dv"

            };


            List<string> objects = new List<string>()
            {
                "tables",
                "functions",
                "views",
                "stored procedures"
               
            };

            
            Console.WriteLine(DateTime.Now.ToString() + ": START");

            DropEverythingObjects(schemas, objects);

            BuildEverything(schemas, objects);


            ADWServices adwServices = new ADWServices(dwConnectionString);

            if (failedObjects.Count > 10)
            {
                int max = trials * failedObjects.Count;

                while (curretIteration < max)
                {
                    curretIteration++;

                    if (failedObjects.Count == 0)
                        break;

                    string script = failedObjects.Dequeue();

                    try
                    {
                        adwServices.ExeuteSQLScript(script);
                    }
                    catch
                    {
                        failedObjects.Enqueue(script);
                    }


                }
            }


            foreach (string script in failedObjects)
            {
                try
                {
                    adwServices.ExeuteSQLScript(script);
                }
                catch(Exception ex)
                {
                   LogError("", ex.Message, script);
               }
            }

            Console.WriteLine(DateTime.Now.ToString() + ": END");
        }

        static void DropEverythingObjects(IEnumerable<string> schemas, IEnumerable<string> objects)
        {

            ADWServices adwServices = new ADWServices(dwConnectionString);

            foreach (string schema in schemas)
            {
                foreach (string objectType in objects)
                {
                    //if (schema == "lnd" && objectType == "tables")
                    //    continue;

                    var dbObjectType = GetDBObjectType(objectType);

                    try
                    {
                        adwServices.DropAllObjectsBySchema(schema, dbObjectType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR:" + ex.Message);
                        using (var writer = new System.IO.StreamWriter(Program.errorFile, true))
                        {
                            writer.WriteLine(ex.Message);
                            writer.Flush();
                            writer.Close();
                        }
                    }

                }
            }
        }

        static void BuildEverything(IEnumerable<string> schemas, IEnumerable<string> objects)
        {
            VSFileServices vsServices = new VSFileServices(vsRootFolder);
            ADWServices adwServices = new ADWServices(dwConnectionString);

            foreach (string schema in schemas)
            {

                foreach (string objectType in objects)
                {
                    if (schema == "lnd" && objectType == "tables")
                        continue;



                    var dbObjectType = GetDBObjectType(objectType);

                    var files = vsServices.GetSQLScriptFiles(schema, dbObjectType);

                    if (files != null)
                    {
                        foreach (string filePath in files)
                        {
                            string script = vsServices.GetSQLScriptFileContant(filePath,dbObjectType);
                            string objectName = vsServices.GetObjectName(filePath);

                            Console.WriteLine("Creating " + objectName);

                        

                            try
                            {


                                adwServices.ExeuteSQLScript(script);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("ERROR:" + ex.Message);

                                if (ex.Message.Contains("Invalid object"))
                                {
                                    failedObjects.Enqueue(script);

                                }
                                else
                                    LogError(objectName, ex.Message, script);
                            }
                        }
                    }
                }
            }
        }

        private static void LogError(string objectName, string message, string script)
        {
            using (var writer = new System.IO.StreamWriter(Program.errorFile, true))
            {
                writer.WriteLine(objectName);
                writer.WriteLine("Error:" + message);
                writer.WriteLine("-------");
                writer.WriteLine(script);
                writer.WriteLine("--------");
                writer.Flush();
                writer.Close();
            }
        }

        static DBObjectType GetDBObjectType(string value)
        {
            DBObjectType objectType = DBObjectType.Table;

            if (value == "tables")
                objectType = DBObjectType.Table;
            else if (value == "views")
                objectType = DBObjectType.View;
            else if (value == "stored procedures")
                objectType = DBObjectType.StoredProcedure;
            else if (value == "functions")
                objectType = DBObjectType.Function;

            return objectType;



        }
    }
}
