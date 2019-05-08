using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;



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

            dbPath = "URI=file:" + Application.persistentDataPath + "/DATA.db";
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
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'products' ( " +
                                      "'Id' INTEGER PRIMARY KEY," +
                                      "  'ProductCode' TEXT," +
                                      "'Upload' INTEGER" +
                                      ");";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertCodes(int userId, string nombre)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO products ( Id, ProductCode, Upload ) " +
                                  "VALUES ( @Id, @ProductCode, @Upload);";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Id",
                    Value = userId
                });
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ProductCode",
                    Value = nombre
                });
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Upload",
                    Value = Estados.Updated
                });
                cmd.ExecuteNonQuery();
            }
            }
        }

        public void InsertCodesLocal(string nombre)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO products (ProductCode, Upload ) " +
                                      "VALUES (@ProductCode, @Upload);";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "ProductCode",
                        Value = nombre
                    });
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Upload",
                        Value = Estados.Local
                    });
                cmd.ExecuteNonQuery();
                }
            }
        }

    public List<TestModel> GetCodes(int _id)
        {
        string query = "";
        if (_id == 0)
        { 
            query = "SELECT * FROM products;";
        }
        else
        { 
            query = "SELECT * FROM products WHERE Id="+_id+";";
        }

        using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    var reader = cmd.ExecuteReader();

                List<TestModel> modelList = new List<TestModel>();
                while (reader.Read())
                    {
                        TestModel model = new TestModel()
                        {
                            Id = reader.GetInt32(0),
                            ProductCode = reader.GetString(1),
                            Estado = (Estados)reader.GetInt32(2)
                        };
                    modelList.Add(model);
                    }
                    return modelList;
                }
            }
        }

        public void ClearData()
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM products;";
                    cmd.ExecuteReader();
                }
            }
        }

        public void UpdateData(int id, string nombre)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE products SET ProductCode ='" + nombre + "', Upload=2" + " WHERE Id =" +id+";";
                    cmd.ExecuteReader();
                }
            }
        }
    }

