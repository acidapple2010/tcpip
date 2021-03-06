﻿using System.Data;
using System.Data.SQLite;
//using Mono.Data.Sqlite;

namespace TcpipClient
{
    public class ConnectionToDB
    {
        readonly SQLiteConnection connection;

        public ConnectionToDB(string filename)
        {
            connection = new SQLiteConnection("data source=" + filename + ";version=3;failifmissing=true;");
        }

        public DataSet DSsqlcmdToDB(string nameTable, DataSet dataset, string sqlcmd)
        {
            connection.Open();
            var da = new SQLiteDataAdapter(sqlcmd, connection);
            connection.Close();
            da.Fill(dataset, nameTable);
            return dataset;
        }

        public void DSsqlcmdToDB(string sqlcmd)
        {
            connection.Open();
            var command = new SQLiteCommand(sqlcmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}