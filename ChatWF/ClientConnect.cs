using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWF
{
    class ClientConnect
    {
        string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;
        bool isName = true;
        Panel panel;
        delegate void MoveCallBack(string message);
        public ClientConnect(Panel panel)
        {
            this.panel = panel;
            panel.listBox1.Items.Add("Введите свое имя: ");
        }
        // получение сообщений
        void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    SendMessage(message);

                }
                catch
                {
                    SendMessage("Подключение прервано!"); //соединение было прервано
                    Disconnect();
                }
            }
        }
        void SendMessage(string message)
        {
            if (panel.listBox1.InvokeRequired)
            {
                MoveCallBack d = new MoveCallBack(SendMessage);
                panel.listBox1.Invoke(d, message);
            }
            else
                panel.listBox1.Items.Add(message); //вывод сообщения
            
        }
        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            //Environment.Exit(0); //завершение процесса
        }
        public void Go()
        {
            if (!isName)
            {
                string message = panel.textBox2.Text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            else
            {
                userName = panel.textBox2.Text;
                isName = false;
                client = new TcpClient();
                try
                {
                    client.Connect(host, port); //подключение клиента
                    stream = client.GetStream(); // получаем поток

                    string message = userName;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // запускаем новый поток для получения данных
                    Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                    receiveThread.Start(); //старт потока

                    SendMessage("Добро пожаловать, " + userName);
                    SendMessage("Введите сообщение: ");
                }
                catch (Exception ex)
                {
                    Disconnect();
                }
                finally
                {

                }
            }
        }
    }
}
