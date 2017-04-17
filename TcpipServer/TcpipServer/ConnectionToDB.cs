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

        public DataSet dsTableFromDB(string nameTable, DataSet dataset, string sqlcmd)
        {
            connection.Open();
            using (var da = new SQLiteDataAdapter(sqlcmd, connection))
            {
                da.Fill(dataset, nameTable);
            }
            connection.Close();
            return dataset;
        }

        public DataSet dsFromDB(DataSet dataset, string sqlcmd)
        {
            connection.Open();
            using (var da = new SQLiteDataAdapter(sqlcmd, connection))
            {
                da.Fill(dataset);
            }
            connection.Close();
            return dataset;
        }

        public void sqlCmd(string sqlcmd)
        {
            connection.Open();
            using (var command = new SQLiteCommand(sqlcmd, connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
	}
}
