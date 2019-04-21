using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace ExampleProject
{

    public class SqLiteDBManager : MonoBehaviour
    {
        public static SqLiteDBManager instance;
        private string dbPath;

        private void Awake()
        {
            if (instance != null)
                Destroy(this.gameObject);
            else
                instance = this;

            dbPath = "URI=file:" + Application.persistentDataPath + "/BarCodesDatabase.db";
            CreateSchema();
        }
        
        public void CreateSchema()
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'codes' ( " +
                                      "  'id' INTEGER PRIMARY KEY, " +
                                      "  'type' TEXT NOT NULL, " +
                                      "  'code' TEXT NOT NULL" +
                                      ");";
                    var result = cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertCodes(string type, string code)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO codes (type, code) " +
                                      "VALUES (@type, @code);";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "type",
                        Value = type
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "code",
                        Value = code
                    });
                    var result = cmd.ExecuteNonQuery();
                }
            }
        }

        public string[,] GetCodes(int limit)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM codes ORDER BY id DESC LIMIT @Count;";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Count",
                        Value = limit
                    });

                    var reader = cmd.ExecuteReader();
                    string[,] result = new String [limit, 2];
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var typeOfCode = reader.GetString(1);
                        var code = reader.GetString(2);
                        result[id, 0] = typeOfCode;
                        result[id, 1] = code;
                    }
                    return result;
                }
            }
        }
    }

}