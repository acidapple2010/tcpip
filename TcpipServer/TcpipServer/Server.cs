using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
//using Mono.Data.Sqlite;
using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TcpipServer
{

	public partial class Server : Form
	{
		StateDB stateServDb = new StateDB(new UnlockDBState());
		TcpListener mTcpListener;
		TcpClient mTcpClient;
		TcpClient mTcpClientLock { get; set; }
		byte[] mRx, tx, txSize;
		string name_table;
		string sqlcmd;
		string strRecvSize { get; set; }
        bool isChangeNum { get; set; }
		bool updateTable { get; set; }
        bool isDataSet { get; set; }

		static DataSet dataset_serv = new DataSet();
		//string filename_dbinp = @"db/inpar.sqlite";
        //string filename_db_server = @"db/db_for_server.sqlite";
        public const string PARTH_DB_SERVER = @"db/db_for_server.sqlite";
        public const string NAME_TAB_DB_SERVER = "LST_CHANGE_NUM";

		public Server()
		{
			InitializeComponent();
			tbConsoleOut.TabStop = false;
		}

		//Находим IP адрес компьютера
		IPAddress FindMyIpv4Address()
		{
			string strThisHostName = string.Empty;
			IPHostEntry thisHostDNSEntry = null;
			IPAddress[] allIPsOfThisHost = null;
			IPAddress ipv4Ret = null;

			try
			{
				//получаем имя хоста системы и храним его в строке
				strThisHostName = Dns.GetHostName();
				PrintLine("Host Name: " + strThisHostName);
				//делаем хост именем домена сервера и храним его в IPHostEntry
				thisHostDNSEntry = Dns.GetHostEntry(strThisHostName);
				//получаем все IP-адрес и сохраним их в хост-объекта
				allIPsOfThisHost = thisHostDNSEntry.AddressList;

				//перебераем все адреса и оставляем первый IPv4
				for (int idx = allIPsOfThisHost.Length - 1; idx >= 0; idx--)
				{
					if (allIPsOfThisHost[idx].AddressFamily == AddressFamily.InterNetwork)
					{
						ipv4Ret = allIPsOfThisHost[idx];
						break;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "TCP/IP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return ipv4Ret;
		}

		private void Server_Load(object sender, EventArgs e)
		{
			IPAddress Ipa = null;
			Ipa = FindMyIpv4Address();

			if (Ipa != null)
			{
				tbIPAddress.Text = Ipa.ToString();
			}
		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			UpdateLocking(0);
		}

		private void btnStartListening_Click(object sender, EventArgs e)
		{
			IPAddress ipaddr;
			int nPort;

			//проверяем, если IP - адрес и номер порта действителен
			if (!int.TryParse(tbPort.Text, out nPort))
			{
				nPort = 9090;
				tbPort.Text = Convert.ToString(nPort);
			}

			if (!IPAddress.TryParse(tbIPAddress.Text, out ipaddr))
			{
				MessageBox.Show("Invalid IPv4 address supplied.", "TCP/IP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Мы дважды проверьте, если мы еще способны подключаться к tcplistener с
			try
			{
				//если все пойдет хорошо, то мы создадим tcplistener с разбора IP-адрес и порт
				mTcpListener = new TcpListener(ipaddr, nPort);

				//начинаем подключение
				mTcpListener.Start();

				//начинаем принимать подключения от клиента
				mTcpListener.BeginAcceptTcpClient(OnCompleteAcceptTcpClient, mTcpListener);

				PrintLine("Server is listening and waiting for client to connect...");

			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
				Application.Restart();
			}

		}

		//написать и отправить поток клиенту
		private void onCompleteWriteToClientStream(IAsyncResult iar)
		{
			//Вот все, что нам нужно сделать, это закончить операцию, когда пакеты посылаются
			try
			{
				var tcpc = (TcpClient)iar.AsyncState;
				tcpc.GetStream().EndWrite(iar);
			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
				//MessageBox.Show(ex.Message, "TCP/IP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//принять поток вызова
		void OnCompleteAcceptTcpClient(IAsyncResult iar)
		{   
			//преобразовываем статус иар для tcplistener
			TcpListener tcpl = (TcpListener)iar.AsyncState;
			try
			{
				//ловим каждый AsyncCall так, что соединение принимается непрерывно
				mTcpClient = tcpl.EndAcceptTcpClient(iar);
				//пошлем детали клиента к серверу
				tx = Encoding.Unicode.GetBytes("You has been connected to server. \nIP address: " + tbIPAddress.Text + "\n");
				txSize = Encoding.Unicode.GetBytes(tx.Length + " ");
				mTcpClient.GetStream().BeginWrite(txSize, 0, txSize.Length, onCompleteWriteToClientStream, mTcpClient);
				mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, mTcpClient);

				//в то время как один клиент подключен, мы можем позволить другим клиентам подключить
				tcpl.BeginAcceptTcpClient(OnCompleteAcceptTcpClient, tcpl);

				//установливаем количество разрешенных по умолчанию байт
				mRx = new byte[2];
				mTcpClient.GetStream().BeginRead(mRx, 0, mRx.Length, OnCompleteReadFromClientStream, mTcpClient);

			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
			}
		}

		//читаем поток данных
		void OnCompleteReadFromClientStream(IAsyncResult iar)
		{
			int nCountReadBytes = 0;
			string strRecv;
			TcpClient tcpc = null;

			try
			{
				//синхронизируем результат в объект tcpclient
				tcpc = (TcpClient)iar.AsyncState;
				//расчитываем количество байт в int
				nCountReadBytes = tcpc.GetStream().EndRead(iar);

				//проверяем, если есть еще пакеты
				if (nCountReadBytes == 0)
				{
					PrintLine("Client has been disconnected. Idle for too long...");
					if (tcpc == mTcpClientLock && mTcpClientLock != null)
					{
						mTcpClientLock = null;
						UpdateLocking(0);
					}
					return;
				}
				tx = null;
				txSize = null;
				//обновляем полученные таблицы в базе 
				strRecv = Encoding.Unicode.GetString(mRx, 0, nCountReadBytes);
                switch (strRecv)
                {
                    //просто текст 
                    case " ":
                        mRx = new byte[254];
                        strRecvSize = null;
                        break;

                    //дата сет
                    case "$":
                        mRx = new byte[Convert.ToInt32(strRecvSize)];
                        strRecvSize = null;
                        updateTable = true;
                        //isDataSet = true;
                        break;

                    //получение запроса для таблицы change_num
                    case "%":
                        mRx = new byte[Convert.ToInt32(strRecvSize)];
                        strRecvSize = null;
                        isChangeNum = true;
                        break;

                    case "#":
                        mRx = new byte[2];
                        strRecvSize = null;
                        tx = Encoding.Unicode.GetBytes("#");
                        tcpc.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, tcpc);
                        break;

                    //завершение размера пакета 
                    case "-":
                        mRx = new byte[2];
                        strRecvSize = null;
                        break;

                    //приходит если нужно блокировать/разблокировать
                    case "+":
                        mRx = new byte[2];
                        string flagToClient;
                        //считали с бд сервера флаг блокироки
                        var svLockingFlag = GetFlagLock();
                        stateServDb.Locking();
                        //блокировка
                        if (strRecvSize.Equals("0"))
                        {
                            if (!svLockingFlag && mTcpClientLock == null)
                            {
                                mTcpClientLock = tcpc;
                                flagToClient = "1";
                                UpdateLocking(1);
                            }
                            else
                                flagToClient = "Сервер заблокирован другим пользователем";
                        }
                        //разблокировка
                        else
                        {
                            if (svLockingFlag)
                            {
                                mTcpClientLock = null;
                                flagToClient = "0";
                                UpdateLocking(0);
                            }
                            else
                                flagToClient = "Ошибка блокировки";
                        }

                        if (flagToClient.Length != 1)
                        {
                            tx = Encoding.Unicode.GetBytes(flagToClient);
                            txSize = Encoding.Unicode.GetBytes(tx.Length + "+");
                            tcpc.GetStream().BeginWrite(txSize, 0, txSize.Length, onCompleteWriteToClientStream, tcpc);
                            tcpc.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, tcpc);
                        }
                        else
                        {
                            tx = Encoding.Unicode.GetBytes(flagToClient + "+");
                            tcpc.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, tcpc);
                        }

                        strRecvSize = null;
                        break;

                    default:
                        if (nCountReadBytes == 2)
                            strRecvSize += strRecv;
                        else if (updateTable)
                        {
                            var ds_server = new DataSet();
                            ds_server = Transformation.convertByteArrayToDataSet(mRx);

                            string[] split = ds_server.DataSetName.Split('|');
                            foreach (DataTable item in ds_server.Tables)
                            {
                                UpdateTable(split[0], item);                            //update change_num table

                            }

                            if (split[4].Equals("U"))
                            {
                                sqlcmd = "update " + NAME_TAB_DB_SERVER + " set CHANGE_NUM = '" + split[3] +
                                    "' where NAME_DB = '" + split[1] + "' and NAME_TABLE = '" + split[2] + "'";
                            }
                            else
                            {
                                sqlcmd = "insert into " + NAME_TAB_DB_SERVER +
                                    " values('" + split[1] + "','" + split[2] + "','" + split[3] + "')";
                            }
                            ConnectionToDB.sqlcmd(sqlcmd, PARTH_DB_SERVER);

                            tx = Encoding.Unicode.GetBytes(" " + ds_server.DataSetName);
                            tcpc.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, tcpc);
                            updateTable = false;
                        }
                        else if (isChangeNum)
                        {
                            var ds_server = new DataSet();
                            string change_num = null;

                            string str = Encoding.Unicode.GetString(mRx);
                            string[] split = str.Split('|');

                            try
                            {
                                ds_server = ConnectionToDB.dsFromDB(ds_server, split[0], PARTH_DB_SERVER);
                                foreach (DataTable item in ds_server.Tables)
                                {
                                    foreach (DataRow item2 in item.Rows)
                                    {
                                        change_num = item2["CHANGE_NUM"].ToString();
                                    }
                                }
                                //посылаем обратно клиенту данные по запросу, бд, таблице и номеру изменению
                                tx = Encoding.Unicode.GetBytes(str + '|' + change_num);
                                txSize = Encoding.Unicode.GetBytes(tx.Length + "%");
                                tcpc.GetStream().BeginWrite(txSize, 0, txSize.Length, onCompleteWriteToClientStream, tcpc);
                                tcpc.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, tcpc);
                                isChangeNum = false;
                            }
                            catch (Exception ex)
                            {
                                tx = Encoding.Unicode.GetBytes(" " + ex.Message);
                                tcpc.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, tcpc);
                            }

                        }
                        mRx = new byte[2];
                        break;
                }

				PrintLine(strRecv);
				tcpc.GetStream().BeginRead(mRx, 0, mRx.Length, OnCompleteReadFromClientStream, tcpc);

			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
				if (tcpc == mTcpClientLock && mTcpClientLock != null)
				{
					mTcpClientLock = null;
					UpdateLocking(0);
				}
			}
		}

		public void PrintLine(string strPrint)
		{
			tbConsoleOut.Invoke(new Action<string>(DoInvoke), strPrint);
		}

		public void DoInvoke(string strPrint)
		{
			tbConsoleOut.Text = tbConsoleOut.Text + strPrint + Environment.NewLine;
		}

		private void btnClean_Click(object sender, EventArgs e)
		{
			tbConsoleOut.Clear();
		}

		#region манипуляции с блокировкой
		private void SelectDataLock()
		{
			try
			{
				//var conToDb = new ConnectionToDB(filename_db_server);
				dataset_serv.Clear();
				name_table = "LST_LOCK";
				sqlcmd = "select * from " + name_table;
                dataset_serv = ConnectionToDB.dsTableFromDB(name_table, dataset_serv, sqlcmd, PARTH_DB_SERVER);
				//dataset_serv = connect_server.DataSetDB("LST_CHANGE_NUM",dataset_serv);
				/*var connect = new ConnectionToDB(filename_dbinp);
				dataset_inp.Clear();
				dataset_inp = connect.DataSetDB("LST_INPAR", dataset_inp);
				dataset_inp = connect.DataSetDB("LST_ITEM", dataset_inp);
				*/
			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
			}
		}

		private void UpdateLocking(int locking)
		{
			try
			{
				//var conToDb = new ConnectionToDB(filename_db_server);
				name_table = "LST_LOCK";
				sqlcmd = "update " + name_table + " set LOCK = " + locking;
                ConnectionToDB.sqlcmd(sqlcmd, PARTH_DB_SERVER);
			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
			}
		}

		public bool GetFlagLock()
		{
			var flag = false;
			SelectDataLock();
			foreach (DataRow dr in dataset_serv.Tables["LST_LOCK"].Rows)
			{
				if (dr["LOCK"].ToString().Equals("True"))
				{
					flag = true;
					PrintLine("Заблокирована");
				}
				else
				{
					flag = false;
					PrintLine("Не заблокирована");
				}
			}
			return flag;
		}
		#endregion

		#region махинации с базами данных
		public static void UpdateTable(string filename, DataTable dataTable)
		{
			using (var connect = new SQLiteConnection("data source=" + filename + ";version=3;failifmissing=true;"))
			{
				connect.Open();
				using (var sqlCommand = connect.CreateCommand())
				{
					sqlCommand.CommandText = "delete from " + dataTable.TableName;
					sqlCommand.ExecuteNonQuery();

					string values;
					foreach (DataRow item in dataTable.Rows)
					{
						values = null;
						values += "'" + item.ItemArray[0] + "'";
						for (int i = 1; i < item.ItemArray.Length; i++)
						{
							values += "," + "'" + item.ItemArray[i] + "'";
						}
						sqlCommand.CommandText = "INSERT INTO " + dataTable.TableName + " VALUES(" + values + ")";
						sqlCommand.ExecuteNonQuery();
					}
				}
				connect.Close();
			}
		}
		#endregion
	}
}