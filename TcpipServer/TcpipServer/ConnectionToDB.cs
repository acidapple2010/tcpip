using System.Data;
//using Mono.Data.Sqlite;
using System.Data.SQLite;

namespace TcpipServer
{
	public static class ConnectionToDB
	{
        public static DataSet dsTableFromDB(string nameTable, DataSet dataset, string sqlcmd, string filename)
        {
            using (SQLiteConnection con = new SQLiteConnection("data source=" + filename + ";version=3;failifmissing=true;"))
            {
                con.Open();
                using (var da = new SQLiteDataAdapter(sqlcmd, con))
                {
                    da.Fill(dataset, nameTable);
                }
                con.Close();
            }
            return dataset;
        }

        public static DataSet dsFromDB(DataSet dataset, string sqlcmd, string filename)
        {
            using (SQLiteConnection con = new SQLiteConnection("data source=" + filename + ";version=3;failifmissing=true;"))
            {
                con.Open();
                using (var da = new SQLiteDataAdapter(sqlcmd, con))
                {
                    da.Fill(dataset);
                }
                con.Close();
            }
            return dataset;
        }

        public static void sqlcmd(string sqlcmd, string filename)
        {
            using (SQLiteConnection con = new SQLiteConnection("data source=" + filename + ";version=3;failifmissing=true;"))
            {
                con.Open();
                using (var command = new SQLiteCommand(sqlcmd, con))
                {
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }
	}
}
