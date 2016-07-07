
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace SQLDeploy
{
    public class ADWServices
    {
        private string _connectionString;

        public ADWServices(string connectionString)
        {
            this._connectionString = connectionString;
        }

        List<string> GetObjectBySchema(string schema, DBObjectType objectType)
        {
               List<string> objectNames = new List<string>();

            if (objectType == DBObjectType.StoredProcedure || objectType==DBObjectType.Function)
            {
                var type = "FUNCTION";
                if (objectType == DBObjectType.StoredProcedure)
                    type = "PROCEDURE";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = @Schema AND ROUTINE_TYPE=@Type;";
                    command.Parameters.AddWithValue("@Schema", schema);
                    command.Parameters.AddWithValue("@Type", type);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objectNames.Add(reader.GetString(0));
                        }
                    }

                }
            }
            else
            {
                string tableType = objectType == DBObjectType.Table ? "BASE TABLE" : "VIEW";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @Schema AND TABLE_TYPE = @TableType;";
                    command.Parameters.AddWithValue("@Schema", schema);
                    command.Parameters.AddWithValue("@TableType", tableType);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objectNames.Add(reader.GetString(0));
                        }
                    }

                }
            }

            return objectNames;
        }

        public void DropAllObjectsBySchema(string schema,DBObjectType dbObbjectType)
        {
 
            List<string> objectNames = GetObjectBySchema(schema,dbObbjectType);

            string objectType = dbObbjectType == DBObjectType.StoredProcedure ? "Procedure" : dbObbjectType.ToString();
            
            foreach (string objectName in objectNames)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    string externalIdentifer = schema == "ext" ? "EXTERAL" : "";
                    command.CommandText = $"DROP {externalIdentifer} {objectType} [{schema}].[{objectName}];";

                    Console.WriteLine(command.CommandText);

                    connection.Open();
                    command.CommandTimeout = 0;
                    command.ExecuteNonQuery();


                    using (var writer = new System.IO.StreamWriter(Program.outputFile, true))
                    {
                        writer.WriteLine(command.CommandText);
                        writer.Flush();
                        writer.Close();
                    }

                }

            }
        }

        public  void TruncateAllTablesByschema(string schema)
        {
            List<string> tableNames = GetObjectBySchema(schema,DBObjectType.Table);

            foreach (string tableName in tableNames)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"Truncate Table [{schema }].[{tableName}];";

                    connection.Open();
                    command.CommandTimeout = 0;
                    command.ExecuteNonQuery();
                }

            }
        }

        public void ExeuteSQLScript(string sqlSript)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = $"{sqlSript} ";


                //Console.WriteLine(command.CommandText);

                connection.Open();
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();


                using (var writer = new System.IO.StreamWriter(Program.outputFile, true))
                {
                    writer.WriteLine(sqlSript);
                    writer.Flush();
                    writer.Close();
                }

            }
        }



    }
}
