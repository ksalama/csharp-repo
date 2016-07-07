using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace CreateDBObjects
{
    class Program
    {
        const string dwConnectionString = "Server=tcp:avp01aadw.database.windows.net,1433; Database=AVP01AADW; User ID= ADWAdmin@avp01aadw; Password=A1*!v7Lz; Encrypt=True; TrustServerCertificate=False; Connection Timeout=0;";
        const string dbConnectionString = "Server=tcp:avp01aadb.database.windows.net,1433; Database=AVP01DADB; User ID= ADWAdmin@avp01aadb; Password=A1*!v7Lz; Encrypt=True; TrustServerCertificate=False; Connection Timeout=0;";

        const string destinationFolder = @"C:\Users\ksalama\Documents\Generated Scripts";
        static void Main(string[] args)
        {
            //GenerateDVCreateTableScripts();
            GenerateSPLoadScripts();
        }


        private static void GenerateSPLoadScripts()
        {
           

            var views = GetStgViews();

            foreach (var view in views)
            {
                var hubs = GetHubEntities(view);


                foreach (var hub in hubs)
                {
                    


                    Console.WriteLine(hub);
                    var hubScript = GetHubSPLoadScript(hub, view);

                    string name = view.Replace("vw_", "").ToUpper();

                    using (StreamWriter writer = new StreamWriter(destinationFolder + $"\\Stored Procedures\\sp_LOAD_{hub}_{name}.sql", false))
                    {
                        writer.Write(hubScript);
                        writer.Flush();
                        writer.Close();
                    }

                }

                string lnk = GetLNKName(view);
               

                if (lnk.Contains("REFERENCE"))
                    continue;

                Console.WriteLine(lnk);
                var lnkScript = GetLNKSPLoadScript(view);

                using (StreamWriter writer = new StreamWriter(destinationFolder + $"\\Stored Procedures\\sp_LOAD_{lnk}.sql", false))
                {
                    writer.Write(lnkScript);
                    writer.Flush();
                    writer.Close();
                }


            }


            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static string GetHubSPLoadScript(string hub, string view)
        {
            var columns = GetColumns(view);
            List<string> variations = GetHubVariations(hub, columns);

            StringBuilder sqlScriptBuilder = new StringBuilder();

            string name = view.Replace("vw_", "").ToUpper();

            sqlScriptBuilder.AppendLine($"CREATE PROCEDURE [stg].[sp_LOAD_{hub}_{name}]");
            sqlScriptBuilder.AppendLine($"AS");
            sqlScriptBuilder.AppendLine($"INSERT INTO [dv].[{hub}]");
            sqlScriptBuilder.AppendLine($"(");
            sqlScriptBuilder.AppendLine($"  [BATCH_ID],");
            sqlScriptBuilder.AppendLine($"  [RECORD_SOURCE],");
            sqlScriptBuilder.AppendLine($"  [LOAD_DATE],");
            sqlScriptBuilder.AppendLine($"  [HSK_{hub.Replace("HUB_", "")}],");
            sqlScriptBuilder.AppendLine($"  [SK_{hub.Replace("HUB_", "")}],");

            var fields = GetBusinessKeyFileds(hub);

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];

                if (field.Item1.ToUpper() == "BatchId".ToUpper() ||
                    field.Item1.ToUpper() == "RECORD_SOURCE".ToUpper() ||
                    field.Item1.ToUpper() == "LandingExecutionHistoryId".ToUpper())
                    continue;

                string dataType = FixDataType(field.Item2);

                sqlScriptBuilder.AppendLine($"  [{field.Item1}],");

            }

            sqlScriptBuilder.Remove(sqlScriptBuilder.Length - 3, 3);
            sqlScriptBuilder.AppendLine();
            sqlScriptBuilder.AppendLine($")");

            for (int i = 0; i < variations.Count; i++)
            {
                if(i!=0)
                    sqlScriptBuilder.AppendLine("UNION");

                string variation = variations[i].ToUpper();

                sqlScriptBuilder.AppendLine("SELECT DISTINCT");
                sqlScriptBuilder.AppendLine(" MIN([LandingExecutionHistoryId]),");
                sqlScriptBuilder.AppendLine(" MIN([RECORD_SOURCE]),");
                sqlScriptBuilder.AppendLine(" GETDATE(),");

                if (variation == "")
                {
                    sqlScriptBuilder.AppendLine($"  [HSK_{hub.Replace("HUB_", "")}],");
                    sqlScriptBuilder.AppendLine($"  [SK_{hub.Replace("HUB_", "")}],");
                }
                else
                {
                    sqlScriptBuilder.AppendLine($"  [HSK_{hub.Replace("HUB_", "")}_{variation}],");
                    sqlScriptBuilder.AppendLine($"  [SK_{hub.Replace("HUB_", "")}_{variation}],");
                }

                for (int j = 0; j < fields.Count; j++)
                {
                    var field = fields[j];

                    if (field.Item1.ToUpper() == "BatchId".ToUpper() ||
                        field.Item1.ToUpper() == "RECORD_SOURCE".ToUpper() ||
                        field.Item1.ToUpper() == "LandingExecutionHistoryId".ToUpper())
                        continue;

                    string dataType = FixDataType(field.Item2);

                    if (variation == "")
                    {
                        sqlScriptBuilder.AppendLine($"  [{field.Item1}],");
                    }
                    else
                    {
                        sqlScriptBuilder.AppendLine($"  [{field.Item1}_{variation}],");
                    }

                }

                sqlScriptBuilder.Remove(sqlScriptBuilder.Length - 3, 3);
                sqlScriptBuilder.AppendLine();

                sqlScriptBuilder.AppendLine("FROM");
                sqlScriptBuilder.AppendLine($"   [stg].[{view}] AS SRC");
                sqlScriptBuilder.AppendLine($"WHERE");

                if (variation == "")
                {
                    sqlScriptBuilder.AppendLine($"NOT EXISTS(SELECT DISTINCT 1 FROM [dv].[{hub}] AS DST WHERE SRC.[HSK_{hub.Replace("HUB_", "")}] = DST.[HSK_{hub.Replace("HUB_", "")}])");
                }
                else
                {
                    sqlScriptBuilder.AppendLine($"NOT EXISTS(SELECT DISTINCT 1 FROM [dv].[{hub}] AS DST WHERE SRC.[HSK_{hub.Replace("HUB_", "")}_{variation}] = DST.[HSK_{hub.Replace("HUB_", "")}])");
                }

            }
    

            return sqlScriptBuilder.ToString();


        }

        private static List<string> GetHubVariations(string hub, List<Tuple<string, string>> columns)
        {
            var list = new List<string>();


            foreach (var column in columns)
            {
                if (column.Item1.ToUpper().Contains(hub.ToUpper().Replace("HUB_", "") + "_"))
                {
                    var parts = column.Item1.Split('_');
                    list.Add(parts[parts.Length - 1]);
                }
            }

            if(list.Count==0)
                list.Add("");

            return list.Distinct().ToList();
        }

        private static void GenerateDVCreateTableScripts()
        {
            List<string> objects = new List<string>();

            var views = GetStgViews();

            foreach (var view in views)
            {
                var hubs = GetHubEntities(view);
               

                foreach (var hub in hubs)
                {
                    if (objects.Contains(hub))
                        continue;

        

                    Console.WriteLine(hub);
                    var hubScript = GetHubCreateTableScript(hub);

                    using (StreamWriter writer = new StreamWriter(destinationFolder + "\\Tables\\" + hub + ".sql", false))
                    {
                        writer.Write(hubScript);
                        writer.Flush();
                        writer.Close();
                    }

                    objects.Add(hub);

                }

                string lnk = GetLNKName(view);
                if (objects.Contains(lnk))
                    continue;

                if(lnk.Contains("REFERENCE"))
                    continue;

                Console.WriteLine(lnk);
                var lnkScript = GetLNKCreateTableScript(view);

                using (StreamWriter writer = new StreamWriter(destinationFolder + "\\Tables\\" + lnk + ".sql", false))
                {
                    writer.Write(lnkScript);
                    writer.Flush();
                    writer.Close();
                }

                objects.Add(lnk);

            }


            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static List<string> GetStgViews()
        {
            List<string> views = new List<string>();

            using (SqlConnection connetion = new SqlConnection(dwConnectionString))
            {
                using (SqlCommand command = connetion.CreateCommand())
                {
                    command.CommandText = "SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'stg' AND TABLE_TYPE = 'VIEW'";

                    connetion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            views.Add(reader.GetString(0));
                    }
                }
            }


            return views;
        }

        static List<Tuple<string, string>> GetColumns(string view)
        {
            List<Tuple<string, string>> columns = new List<Tuple<string, string>>();

            using (SqlConnection connetion = new SqlConnection(dwConnectionString))
            {
                using (SqlCommand command = connetion.CreateCommand())
                {
                    command.CommandText = $"SELECT COLUMN_NAME,CASE WHEN RIGHT(DATA_TYPE,4) = 'CHAR' THEN DATA_TYPE+'('+CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR)+')' ELSE Data_Type END As DataType FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'stg' AND TABLE_NAME = '{view}' ORDER BY ORDINAL_POSITION";

                    connetion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            while (reader.Read())
                                columns.Add(new Tuple<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }
            }


            return columns;
        }

        public static List<Tuple<string, string>> GetBusinessKeyFileds(string dvEntity)
        {
            List<Tuple<string, string>> fileds = new List<Tuple<string, string>>();

            using (SqlConnection connetion = new SqlConnection(dbConnectionString))
            {
                using (SqlCommand command = connetion.CreateCommand())
                {
                    command.CommandText = $"SELECT FieldName, DataType FROM mapping.vwDataVaultBusinessKeys WHERE DataVaultEntity = '{dvEntity}' Order by DataVaultEntity, Ordinal";

                    connetion.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            fileds.Add(new Tuple<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }
            }


            return fileds;
        }

        public static List<string> GetHubEntities(string view)
        {
            return GetHubEntities(GetColumns(view));
        }
        public static List<string> GetHubEntities(List<Tuple<string, string>> columns)
        {
            List<string> hubs = new List<string>();

            foreach (var column in columns)
            {
                if (column.Item1.Length >= 4 && column.Item1.Substring(0, 4) == "HSK_")
                {
                    string hub = column.Item1.Replace("HSK_", "HUB_").ToUpper();

                    if (hub.Contains("CHANNEL"))
                        hub = "HUB_CHANNEL";

                    if (hub.Contains("ADDRESS"))
                        hub = "HUB_ADDRESS";

                    if (hub.Contains("PHONE"))
                        hub = "HUB_PHONE";

                    if (hub.Contains("EMAIL"))
                        hub = "HUB_EMAIL";

                    if (hub.Contains("PAYMENT"))
                        hub = "HUB_PAYMENT";

                    if (hub.Contains("STORE"))
                        hub = "HUB_STORE";

                    hubs.Add(hub);
                }
            }

            return hubs.Distinct().ToList();
        }

        public static string GetHubCreateTableScript(string hub)
        {

            StringBuilder sqlScriptBuilder = new StringBuilder();

            sqlScriptBuilder.AppendLine($"CREATE TABLE [dv].[{hub}]");
            sqlScriptBuilder.AppendLine($"(");
            sqlScriptBuilder.AppendLine($"  [BATCH_ID] INT,");
            sqlScriptBuilder.AppendLine($"  [RECORD_SOURCE]   VARCHAR(100),");
            sqlScriptBuilder.AppendLine($"  [LOAD_DATE]  DATETIME,");
            sqlScriptBuilder.AppendLine($"  [HSK_{hub.Replace("HUB_", "")}]    CHAR(32),");
            sqlScriptBuilder.AppendLine($"  [SK_{hub.Replace("HUB_", "")}]   VARCHAR(500),");

            var fields = GetBusinessKeyFileds(hub);

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];

                if (field.Item1.ToUpper() == "BatchId".ToUpper() ||
                    field.Item1.ToUpper() == "RECORD_SOURCE".ToUpper() ||
                    field.Item1.ToUpper() == "LandingExecutionHistoryId".ToUpper())
                    continue;

                string dataType = FixDataType(field.Item2);

                sqlScriptBuilder.AppendLine($"  [{field.Item1}]  {dataType.ToUpper()},");

            }

            sqlScriptBuilder.Remove(sqlScriptBuilder.Length - 3, 3);
            sqlScriptBuilder.AppendLine();
            sqlScriptBuilder.AppendLine($")");

            sqlScriptBuilder.AppendLine($"WITH");
            sqlScriptBuilder.AppendLine($"(");
            sqlScriptBuilder.AppendLine($"  DISTRIBUTION = ROUND_ROBIN,");
            sqlScriptBuilder.AppendLine($"  CLUSTERED COLUMNSTORE INDEX");
            sqlScriptBuilder.AppendLine($")");

            return sqlScriptBuilder.ToString();
        }

        private static string FixDataType(string dataType)
        {
            string fix = dataType;

            if (dataType.Contains("VARCHAR"))
            {
                int value = int.Parse(dataType.Replace("VARCHAR(", "").Replace(")", ""));
                if (value > 500)
                    fix = "VARCHAR(500)";
            }

            return fix;
        }

        public static string GetLNKCreateTableScript(string view)
        {
            StringBuilder sqlScriptBuilder = new StringBuilder();

            string lnk = GetLNKName(view);

            var columns = GetColumns(view);

            List<string> businesskeys = new List<string>();
            foreach (var hub in GetHubEntities(view))
            {
                foreach (var field in GetBusinessKeyFileds(hub))
                    businesskeys.Add(field.Item1.ToUpper());
            }


            sqlScriptBuilder.AppendLine($"CREATE TABLE [dv].[{lnk}]");
            sqlScriptBuilder.AppendLine($"(");
            sqlScriptBuilder.AppendLine($"  [BATCH_ID] INT,");
            sqlScriptBuilder.AppendLine($"  [RECORD_SOURCE]   VARCHAR(100),");
            sqlScriptBuilder.AppendLine($"  [LOAD_DATE]   DATETIME,");


            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                if (column.Item1.ToUpper().Contains("HSK_"))
                    sqlScriptBuilder.AppendLine($"  [{column.Item1}]  {column.Item2.ToUpper()},");
            }

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                if (column.Item1.ToUpper() == "BatchId".ToUpper() ||
                    column.Item1.ToUpper() == "RECORD_SOURCE".ToUpper() ||
                    column.Item1.ToUpper() == "LandingExecutionHistoryId".ToUpper() ||
                    column.Item1.ToUpper().Contains("HSK_") ||
                    column.Item1.ToUpper().Contains("SK_")  ||
                    IsBusinessKey(column.Item1, businesskeys)
                    )
                    continue;

                 
                sqlScriptBuilder.AppendLine($"  [{column.Item1}]  {column.Item2.ToUpper()},");

            }

            sqlScriptBuilder.Remove(sqlScriptBuilder.Length - 3, 3);
            sqlScriptBuilder.AppendLine();
            sqlScriptBuilder.AppendLine($")");

            sqlScriptBuilder.AppendLine($"WITH");
            sqlScriptBuilder.AppendLine($"(");
            sqlScriptBuilder.AppendLine($"  DISTRIBUTION = ROUND_ROBIN,");
            sqlScriptBuilder.AppendLine($"  CLUSTERED COLUMNSTORE INDEX");
            sqlScriptBuilder.AppendLine($")");

            return sqlScriptBuilder.ToString();

        }
        
        public static string GetLNKSPLoadScript(string view)
        {
            StringBuilder sqlScriptBuilder = new StringBuilder();

            string lnk = GetLNKName(view);

            var columns = GetColumns(view);

            List<string> businesskeys = new List<string>();
            foreach (var hub in GetHubEntities(view))
            {
                foreach (var field in GetBusinessKeyFileds(hub))
                    businesskeys.Add(field.Item1.ToUpper());
            }


            sqlScriptBuilder.AppendLine($"CREATE PROCEDURE [stg].[sp_LOAD_{lnk}]");
            sqlScriptBuilder.AppendLine($"AS");
            sqlScriptBuilder.AppendLine($"INSERT INTO [dv].[{lnk}]");
            sqlScriptBuilder.AppendLine($"(");
            sqlScriptBuilder.AppendLine($"  [BATCH_ID],");
            sqlScriptBuilder.AppendLine($"  [RECORD_SOURCE],");
            sqlScriptBuilder.AppendLine($"  [LOAD_DATE],");


            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                if (column.Item1.ToUpper().Contains("HSK_"))
                    sqlScriptBuilder.AppendLine($"  [{column.Item1}],");
            }

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                if (column.Item1.ToUpper() == "BatchId".ToUpper() ||
                    column.Item1.ToUpper() == "RECORD_SOURCE".ToUpper() ||
                    column.Item1.ToUpper() == "LandingExecutionHistoryId".ToUpper() ||
                    column.Item1.ToUpper().Contains("HSK_") ||
                    column.Item1.ToUpper().Contains("SK_") ||
                    IsBusinessKey(column.Item1, businesskeys)
                    )
                    continue;


                sqlScriptBuilder.AppendLine($"  [{column.Item1}],");

            }

            sqlScriptBuilder.Remove(sqlScriptBuilder.Length - 3, 3);
            sqlScriptBuilder.AppendLine();
            sqlScriptBuilder.AppendLine($")");

            sqlScriptBuilder.AppendLine($"SELECT ");
            sqlScriptBuilder.AppendLine($"  [LandingExecutionHistoryId],");
            sqlScriptBuilder.AppendLine($"  [RECORD_SOURCE],");
            sqlScriptBuilder.AppendLine($"  GETDATE(),");

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                if (column.Item1.ToUpper().Contains("HSK_"))
                    sqlScriptBuilder.AppendLine($"  [{column.Item1}],");
            }

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                if (column.Item1.ToUpper() == "BatchId".ToUpper() ||
                    column.Item1.ToUpper() == "RECORD_SOURCE".ToUpper() ||
                    column.Item1.ToUpper() == "LandingExecutionHistoryId".ToUpper() ||
                    column.Item1.ToUpper().Contains("HSK_") ||
                    column.Item1.ToUpper().Contains("SK_") ||
                    IsBusinessKey(column.Item1, businesskeys)
                    )
                    continue;


                sqlScriptBuilder.AppendLine($"  [{column.Item1}],");

            }

            sqlScriptBuilder.Remove(sqlScriptBuilder.Length - 3, 3);
            sqlScriptBuilder.AppendLine();

            sqlScriptBuilder.AppendLine($"FROM");
            sqlScriptBuilder.AppendLine($"  [stg].[{view}]");

            return sqlScriptBuilder.ToString();

        }

        private static bool IsBusinessKey(string column, List<string> businesskeys)
        {
            bool isBusinesKey = false;

            if (column.ToUpper().Contains("address".ToUpper()))
            {

            }

            foreach (var field in businesskeys)
            {
                if (column.ToUpper().Contains(field.ToUpper()))
                {
  
                    isBusinesKey = true;
                    break;
                }
            }

            return isBusinesKey;

        }

        public static string GetLNKName(string view)
        {
            var parts = view.Replace("vw_", "")
                            .ToUpper()
                            .Replace("UK", "")
                            .Replace("ROI", "")
                            .Split('-');

            string domain = parts[1] == "ORDERS" ? "ORDER" : parts[1];

            string lnk = $"LNK_{domain}_{parts[0]}";
            return lnk;
        }

    }
}
