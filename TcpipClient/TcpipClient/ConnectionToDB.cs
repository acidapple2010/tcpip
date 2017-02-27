using System.Data;
using System.Data.SQLite;
using Mono.Data.Sqlite;

namespace TcpipClient
{
    public class ConnectionToDB
    {
        readonly SqliteConnection connection;

        public ConnectionToDB(string filename)
        {
            connection = new SqliteConnection("data source=" + filename + ";version=3;failifmissing=true;");
        }

        public DataSet DSsqlcmdToDB(string nameTable, DataSet dataset, string sqlcmd)
        {
            connection.Open();
            var da = new SqliteDataAdapter(sqlcmd, connection);
            connection.Close();
            da.Fill(dataset, nameTable);
            return dataset;
        }

        public void DSsqlcmdToDB(string sqlcmd)
        {
            connection.Open();
            var command = new SqliteCommand(sqlcmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}