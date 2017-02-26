using System.Data;
using System.Data.SQLite;

namespace TcpipClient
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
            var da = new SQLiteDataAdapter(sqlcmd, connection);
            connection.Close();
        }
    }
}