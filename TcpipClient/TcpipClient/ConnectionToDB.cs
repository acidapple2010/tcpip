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

        public DataSet DataSetDB(string name_table, DataSet dataset)
        {
            connection.Open();
            var sqlcmd = "select * from " + name_table;
            var da = new SQLiteDataAdapter(sqlcmd, connection);
            connection.Close();

            da.Fill(dataset, name_table);

            return dataset;
        }
    }
}