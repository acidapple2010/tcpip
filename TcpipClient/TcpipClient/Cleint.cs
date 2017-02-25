using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

//Imports for Sockets Programming
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SQLite;

namespace TcpipClient
{
    public partial class Cleint : Form
    {
        TcpClient mTcpClient;
        byte[] mRx, tx, txSize;
        //Name in ChatBox
        static string strName;

        //When connected, prevent from editing ip address port and name
		//При подключении, предотвратить от редактирования IP-адрес, порт и имя
        bool isAvailableToWrite = true;
		SQLiteConnection connpar;
		DataSet dspar;
		String clChangeFlag;
        DataSet dataset_client = new DataSet();
        string name_table;
        string sqlcmd;
		string filename_dbinp = @"../../../inpar_Kulygin_2.sqlite";
        string filename_db_client = @"../../../db_for_client.sqlite";

        public Cleint()
        {
            InitializeComponent();
            tbConsoleOut.TabStop = false;
            tbName.TabIndex = 1;
        }

        private void Client_Load(object sender, EventArgs e){ }

        //Get Clients IP Address
        IPAddress findmMyIPV4Address()
        {
			//Определить объекты и убедиться, что они полностью пустые
            string strThisHostName = string.Empty;
            IPHostEntry thisHostDNSEntry = null;
            IPAddress[] allIPsOfThisHost = null;
            IPAddress ipv4Ret = null;

            try
            {
				//Получить имя хоста системы и хранить его в строку
                strThisHostName = System.Net.Dns.GetHostName();
				//Сделать хост с именем домена сервера и храните его в IPHostEntry
                thisHostDNSEntry = System.Net.Dns.GetHostEntry(strThisHostName);
				//Получить все IP - адрес и сохранить его в хост - объекта
                allIPsOfThisHost = thisHostDNSEntry.AddressList;

				//Перебрать все адреса и найти первый Адрес IPv4
                for (int idx = allIPsOfThisHost.Length -1; idx >= 0; idx --)
                {
					//если нашли адрес IPv4-это, обратный адрес
                    if (allIPsOfThisHost[idx].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4Ret = allIPsOfThisHost[idx];
                        break;
                    }
                }

            } catch (Exception exc)
            {
                MessageBox.Show("IP Address cannot be detected. Program will still carry on running...", "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return ipv4Ret;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress ipa;
            int nPort;

			//мы проверяем, если мы можем все еще в состоянии исправить в поле конфигурации группы
            isAvailableToWrite = false;

            try
            {
				//Код если IP-адрес или порт пуст
                if (string.IsNullOrEmpty(tbServerIP.Text) || string.IsNullOrEmpty(tbName.Text))
                {
                    MessageBox.Show("You must enter your Name and provide an IP Address!", "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

				//Если он не может быть передан IP-адрес сервера
                if (!IPAddress.TryParse(tbServerIP.Text, out ipa))
                {
                    MessageBox.Show("Please supply an IP Address!", "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!int.TryParse(tbServerPort.Text, out nPort))
                {
                    nPort = 9090;
                    tbServerPort.Text = Convert.ToString(nPort);
                }

                if (!string.IsNullOrEmpty(tbName.Text))
                {
                    strName = tbName.Text;
                }

                mTcpClient = new TcpClient();
                //Создать подключение - IP-адрес, номер порта, при подключении и какой клиент
				mTcpClient.BeginConnect(ipa, nPort, onCompleteConnect, mTcpClient);

                checkConnection();
			}
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

		//проверяем, если соединение было успешным или нет
        void checkConnection()
        {
            if (isAvailableToWrite)
            {
                connectionGroup.Enabled = true;
            }
            else if (!isAvailableToWrite)
            {
                connectionGroup.Enabled = false;
            }
        }

		//метод обратного вызова, когда попытка соединения закончена
        void onCompleteConnect(IAsyncResult iar)
        {
			//открыть объект tcpclient связи
            TcpClient tcpc;
            IPAddress ipa = null;

			//петли обратной связи адреса
            ipa = findmMyIPV4Address();

            //установили попытку подключения ложью
            isAvailableToWrite = false;

			//Данные обратной связи на хранение строки в байтах
            tx = new byte[254];

            try
            {
				//синхронизируем результат в объект tcpclient
                tcpc = (TcpClient)iar.AsyncState;
                tcpc.EndConnect(iar);

				//пошлем детали клиента к серверу
                tx = Encoding.Unicode.GetBytes("Client has been connected. \r\nClient Name: " + strName + "\r\n"
                + "Clients IP Address: " + ipa.ToString() + "\r\n");
                
                txSize = Encoding.Unicode.GetBytes(tx.Length + " ");
                mTcpClient.GetStream().BeginWrite(txSize, 0, txSize.Length, onCompleteWriteToServer, mTcpClient);

                mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToServer, mTcpClient);

                //начинаем слушать отклики со стороны сервера на стороне клиента
                mRx = new byte[254];
                tcpc.GetStream().BeginRead(mRx, 0, mRx.Length, onCompleteReadFromServerStream, tcpc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Server is offline or has actively refused your connection, please try again later!",
                    "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Restart();
            }
        }

		//читаем поток с сервера
        void onCompleteReadFromServerStream(IAsyncResult iar)
        {
            TcpClient tcpc;
            int nCountBytesReceivedFromServer;
            string strReceived;

            try
            {
				//проверяем, если байты были получены от сервера
                tcpc = (TcpClient)iar.AsyncState;
                nCountBytesReceivedFromServer = tcpc.GetStream().EndRead(iar);

				//проверерка на отправку пустоты
                if (nCountBytesReceivedFromServer == 0)
                {
					//если пусто, соединение разрывается
                    MessageBox.Show("Connection broken, retry connecting to the server!", "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //если что-то есть, то мы храним данные в strReceived и выводим на экран 
				strReceived = Encoding.Unicode.GetString(mRx, 0, nCountBytesReceivedFromServer);
				printLine(strReceived);
				clChangeFlag = strReceived;
				mRx = new byte[254];
				tcpc.GetStream().BeginRead(mRx, 0, mRx.Length, onCompleteReadFromServerStream, tcpc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server has disconnected, please try again later!", "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Restart();
            }
        }

		//делаем, если написано на сервер
        void onCompleteWriteToServer(IAsyncResult iar)
        {
            TcpClient tcpc;
            try
            {
				//преобразовываем результаты iar и записываем его в объект tcpclient
                tcpc = (TcpClient)iar.AsyncState;
                tcpc.GetStream().EndWrite(iar);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void printLine(string strPrint)
        {
            tbConsoleOut.Invoke(new Action<string>(doInvoke), strPrint);
        }

        public void doInvoke(string strPrint)
        {
            tbConsoleOut.Text = tbConsoleOut.Text + strPrint + Environment.NewLine;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {            
            //возможность обновлять пока одну таблицу
            byte[] tx, txSize;
            OpenData();

            tx = ConvertDataToByteArray(dspar.Tables["LST_ITEM"]);
            txSize = Encoding.Unicode.GetBytes(tx.Length + "-");

            send_to_server(txSize);
            send_to_server(tx);
        }

        private void send_to_server(byte[] tx)
        {
            //проверяем, если есть соединение, также, если есть еще пакеты
            if (mTcpClient != null)
            {
                if (mTcpClient.Client.Connected)
                {
                    mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToServer, mTcpClient);
                    printLine("send to server " + tx.Length);
                }
                else if (!mTcpClient.Client.Connected)
                {
                    MessageBox.Show("You are not connected to a server!", "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnChange_Request_Click(object sender, EventArgs e)
        {
            //принять данные о возможности на изменения таблиц
            //и делать манипуляции с кнопкой и отправкой серверу согласие или не согласие

			OpenData();

			try
			{

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "TCP/IP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
        }

        //конвертация таблицы в байты для передачи на сервер
        private static byte[] ConvertDataToByteArray(DataTable dataTable)
        {
            byte[] binaryDataResult;
            var brFormatter = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                brFormatter.Serialize(memStream, dataTable);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }

		#region открытие бд
		private void OpenData()
		{

			try
			{
			    var connect_client = new ConnectionToDB(filename_db_client);
			    dataset_client.Clear();
			    name_table = "LST_CHANGE";
			    sqlcmd = "select * from " + name_table;
			    dataset_client = connect_client.DataSetSelect(name_table, dataset_client, sqlcmd);
//				connpar = new SQLiteConnection("data source=" + filename_dbinp + ";version=3;failifmissing=true;");
//				connpar.Open();
//				dspar = DataSetParsLoader(connpar);
//				connpar.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion
    }
}
