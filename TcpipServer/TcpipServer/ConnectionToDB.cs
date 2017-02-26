﻿using System.Data;
using Mono.Data.Sqlite;

namespace TcpipServer
{
	public class ConnectionToDB
	{
		readonly SqliteConnection connection;

		public ConnectionToDB(string filename)
		{
			connection = new SqliteConnection("data source=" + filename + ";version=3;failifmissing=true;");
		}

		public DataSet DSsqlcmdToDB(string name_table, DataSet dataset, string sqlcmd)
		{
			connection.Open();
			var da = new SqliteDataAdapter(sqlcmd, connection);
			connection.Close();
			da.Fill(dataset, name_table);
			return dataset;
		}

		public void DSsqlcmdToDB(string sqlcmd)
		{
			connection.Open();
			var da = new SqliteDataAdapter(sqlcmd, connection);
			connection.Close();
		}
	}
}
