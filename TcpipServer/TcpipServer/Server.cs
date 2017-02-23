using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

//Imports for Sockets Programming
using System.Net;
using System.Net.Sockets;
using Mono.Data.Sqlite;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TcpipServer
{

	public partial class Server : Form
	{
		
		TcpListener mTcpListener;
		TcpClient mTcpClient;
		byte[] mRx;
		SqliteConnection connpar;
		DataSet dataset_serv = new DataSet();
		DataSet dataset_inp = new DataSet();

		string svChangeFlag, svChangeNum;
		string filename_dbinp = @"../../../inpar_Kulygin_1.sqlite";
		string filename_db_server = @"../../../db_for_server.sqlite";

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
				TcpClient tcpc = (TcpClient)iar.AsyncState;
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

				OpenData();
				GetFlagChange();
				var tx = Encoding.Unicode.GetBytes(svChangeFlag);
				mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToClientStream, mTcpClient);

				//в то время как один клиент подключен, мы можем позволить другим клиентам подключить
				tcpl.BeginAcceptTcpClient(OnCompleteAcceptTcpClient, tcpl);

				//установливаем количество разрешенных по умолчанию байт
				mRx = new byte[2];
				mTcpClient.GetStream().BeginRead(mRx, 0, mRx.Length, OnCompleteReadFromTcpClientStream, mTcpClient);

			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
			}
		}

		bool updateTable;
		string strRecvSize;
		//читаем поток данных
		void OnCompleteReadFromTcpClientStream(IAsyncResult iar)
		{
			TcpClient tcpc;
			int nCountReadBytes = 0;
			string strRecv;

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
					return;
				}

				//обновляем полученные таблицы в базе 
				//UpdateTable(filename_dbinp, ConvertByteArrayToDataTable(mRx));
				strRecv = Encoding.Unicode.GetString(mRx, 0, nCountReadBytes);

				//что-то придумать насчет большого количества передаваемых байтов в пакете, 
				//очищаем буфер, поэтому клиенты могут непрерывно посылать пакеты
				if (strRecv.Equals("-") || strRecv.Equals(" "))
				{
					if (!strRecvSize.Equals(null))
					{
						mRx = new byte[Convert.ToInt32(strRecvSize)];
						strRecvSize = null;
					}
					else
						mRx = new byte[254];
					//- для понимания, что этот массив байт таблица, которую нужно обновить
					updateTable |= strRecv.Equals("-");
				}
				else
				{
					if (nCountReadBytes == 2)
						strRecvSize += strRecv;
					else if (updateTable)
					{
						UpdateTable(filename_dbinp, ConvertByteArrayToDataTable(mRx));
						updateTable = false;
					}
					mRx = new byte[2];
				}
				PrintLine(strRecv);
				tcpc.GetStream().BeginRead(mRx, 0, mRx.Length, OnCompleteReadFromTcpClientStream, tcpc);

			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
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

		#region открытие бд
		private void OpenData()
		{

			try
			{
				ConnectionToDB connect_server = new ConnectionToDB(filename_db_server);
				dataset_serv.Clear();
				dataset_serv = connect_server.DataSetDB("LST_CHANGE",dataset_serv);
				dataset_serv = connect_server.DataSetDB("LST_CHANGE_NUM",dataset_serv);

				ConnectionToDB connect = new ConnectionToDB(filename_dbinp);
				dataset_inp.Clear();
				dataset_inp = connect.DataSetDB("LST_INPAR", dataset_inp);
				dataset_inp = connect.DataSetDB("LST_ITEM", dataset_inp);

			}
			catch (Exception ex)
			{
				PrintLine(ex.Message);
			}
		}
		#endregion

		public void GetFlagChange()
		{
			foreach (DataRow dr in dataset_serv.Tables["LST_CHANGE"].Rows)
			{
				svChangeFlag = dr["CHANGE"].ToString();
				if (svChangeFlag.Equals("False"))
					PrintLine("Можно обновлять.");
				else
					PrintLine("Нельзя обновлять.");
			}
		}

		private static DataTable ConvertByteArrayToDataTable(byte[] byteDataArray)
		{
			DataTable dataTable;
			var brFormatter = new BinaryFormatter();
			using (var memStream = new MemoryStream(byteDataArray))
			{
				dataTable = (DataTable)brFormatter.Deserialize(memStream);
				memStream.Close();
			}
			return dataTable;
		}

		public static void UpdateTable(string filename_dbinp, DataTable dataTable)
		{
			using (var connect = new SqliteConnection("data source=" + filename_dbinp + ";version=3;failifmissing=true;"))
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
	}
}