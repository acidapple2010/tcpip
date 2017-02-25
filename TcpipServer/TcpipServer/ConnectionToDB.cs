using System.Data;
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

		public DataSet DataSetDB(string name_table, DataSet dataset, string sqlcmd)
		{
			connection.Open();
			//var cmd = "select * from " + name_table;
			var da = new SqliteDataAdapter(sqlcmd, connection);
			connection.Close();

			da.Fill(dataset, name_table);

			return dataset;
		}
	}
}
