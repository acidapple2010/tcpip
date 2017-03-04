using System.Data;
//using Mono.Data.Sqlite;
using System.Data.SQLite;

namespace TcpipServer
{
	public class ConnectionToDB
	{
		readonly SQLiteConnection connection;

		public ConnectionToDB(string filename)
		{
			connection = new SQLiteConnection("data source=" + filename + ";version=3;failifmissing=true;");
		}

		public DataSet DSsqlcmdToDB(string name_table, DataSet dataset, string sqlcmd)
		{
			connection.Open();
			var da = new SQLiteDataAdapter(sqlcmd, connection);
			connection.Close();
			da.Fill(dataset, name_table);
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
