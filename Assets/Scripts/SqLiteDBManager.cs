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

            dbPath = "URI=file:" + Application.persistentDataPath + "/CodesData.db";
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
                                      "  'Id' INTEGER PRIMARY KEY, " +
                                      "  'Nombre' TEXT, " +
                                      "  'Rubro' TEXT, " +
                                      "  'Tipo' TEXT" +
                                      ");";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertCodes(int userId, string nombre, string rubro, string tipo)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO codes ( Id, Nombre, Rubro, Tipo ) " +
                                      "VALUES ( @id, @Nombre, @Rubro, @Tipo);";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "id",
                        Value = userId
                    });
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Nombre",
                        Value = nombre
                    });
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Rubro",
                        Value = rubro
                    });
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Tipo",
                        Value = tipo
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
            query = "SELECT * FROM codes;";
        }
        else
        { 
            query = "SELECT * FROM codes WHERE Id="+_id+";";
        }
        print(query);

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
                            Nombre = reader.GetString(1),
                            Rubro = reader.GetString(2),
                            Tipo = reader.GetString(3)
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
                    cmd.CommandText = "DELETE FROM codes;";
                    cmd.ExecuteReader();
                }
            }
        }

        public void UpdateData(int id, string nombre, string rubro, string tipo )
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE codes SET Nombre ='" + nombre + "', Rubro='" + rubro + "',Tipo='" + tipo + "' WHERE Id ="+id+";";
                    cmd.ExecuteReader();
                }
            }
        }
    }

