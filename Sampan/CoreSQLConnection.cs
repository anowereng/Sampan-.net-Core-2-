using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;

namespace Sampan
{
    public class CoreSQLConnection
    {
        private string dbConnection { get; set; }
        public SqlConnection Connection { set; get; }
        public SqlDataReader Reader { get; set; }
        public IDataReader DataReader { get; set; }
        public SqlCommand Command { get; set; }
        public SqlDataAdapter Adapter { get; set; }
        public DataTable DataTable { get; set; }
        public string Query { set; get; }

        public CoreSQLConnection(string dbname = "")
        {
            StreamReader r = new StreamReader("server.json");
            EncryptDecryptData encrdecr = new EncryptDecryptData();
            string json = r.ReadToEnd();
            List<connection> file = JsonConvert.DeserializeObject<List<connection>>(json);
            string constring = "";
            foreach (var data in file)
            {
                if (dbname == "")
                {
                    constring = "Server=" + data.PCName + ";Database=" + data.Database + ";User Id=" + data.UserId + ";Password=" + encrdecr.DecryptString(data.Password);
                }
                else
                {
                    constring = "Server=" + data.PCName + ";Database=" + dbname + ";User Id=" + data.UserId + ";Password=" + data.Password;
                }
            }
            dbConnection = constring;
        }


        // FOR  DATA READ USING SQL COMMAND
        //public IDataReader CoreSQL_GetReader(string connstring, string Query)
        //{
        //    Connection = new SqlConnection(connstring);
        //    Command = new SqlCommand(Query, Connection);
        //    Connection.Open();
        //    Reader = Command.ExecuteReader();
        //    Connection.Close();
        //    return Reader;
        //}
        public IDataReader CoreSQL_GetReader(string Query)
        {
            Connection = new SqlConnection(dbConnection);
            Command = new SqlCommand(Query, Connection);
            Connection.Open();
            DataReader = Command.ExecuteReader();
            //Connection.Close();
            //DataReader.Close();
            return DataReader;
        }

        // FOR GET DATA USING SQL COMMAND
        public double CoreSQL_GetDoubleData(string Query)
        {
            Connection = new SqlConnection(dbConnection);
            Command = new SqlCommand(Query, Connection);
            Connection.Open();
            var variable = (double)Command.ExecuteScalar();
            Connection.Close();
            return variable;
        }

        // FOR DATA SAVE USING SQL COMMAND
        public void CoreSQL_SaveDataUseSQLCommand(ArrayList queryList)
        {
            Connection = new SqlConnection(dbConnection);
            try
            {
                foreach (string query in queryList)
                {
                    Command = new SqlCommand(query, Connection);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        // DATA SET RETURN 
        public DataSet CoreSQL_GetDataSet(string Query)
        {
            Connection = new SqlConnection(dbConnection);
            try
            {
                DataSet dsList = new DataSet();
                Adapter = new SqlDataAdapter(Query, Connection);
                Connection.Open();
                Adapter.Fill(dsList);
                Connection.Close();
                return dsList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public string GetEncryptedData(string data)
        {
            EncryptDecryptData model = new EncryptDecryptData();
            //var key = "E546C8DF278CD5931069B522E695D4F2";
            return model.EncryptString(data);
        }
        public string GetDecryptedData(string data)
        {
            EncryptDecryptData model = new EncryptDecryptData();
            //var key = "E546C8DF278CD5931069B522E695D4F2";
            return model.DecryptString(data);
        }
    }
}
