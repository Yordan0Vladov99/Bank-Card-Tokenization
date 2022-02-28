using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace WPFChatServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread readThread; 
        private Dictionary<Thread, NetworkStream> writers;
        private Dictionary<Thread, Socket> sockets;
        private BinaryFormatter bf;
        private List<CardTokenPair> pairs;
        private List<User> users;
        private int counter;
        public MainWindow()
        {
            InitializeComponent();
            writers = new Dictionary<Thread, NetworkStream>();
            sockets = new Dictionary<Thread, Socket>();

            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );

            var pairsPath = Path.Combine(systemPath, "Pairs.xml");
            pairs = new List<CardTokenPair>();
            XmlSerializer serializer = new XmlSerializer(pairs.GetType());

            using ( Stream reader = new FileStream(pairsPath, FileMode.Open))
            {
                pairs = (List<CardTokenPair>)serializer.Deserialize(reader);
            }
            
            users = new List<User>();
            var usersPath = Path.Combine(systemPath, "Users.xml");
            serializer = new XmlSerializer(users.GetType());

            using (Stream reader = new FileStream(usersPath, FileMode.Open))
            {
                users = (List<User>)serializer.Deserialize(reader);
            }

            bf = new BinaryFormatter();
            readThread = new Thread(new ThreadStart(RunServer));
            readThread.Start();
            counter = 0;
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && TxtInput.IsEnabled == true)
                {
                    var keys = writers.Keys;
                    foreach (var key in keys)
                    {
                        bf.Serialize(writers[key], TxtInput.Text);
                    }

                    TxtDisplay.Text += "\r\nSERVER>>> " + TxtInput.Text + "\n";
                    if (TxtInput.Text == "TERMINATE")
                    {
                      var  socketKeys = sockets.Keys;
                        foreach (var key in socketKeys)
                        {
                            sockets[key].Close();
                        }
                        
                    }
                    TxtInput.Clear();
                } 
            } 
            catch (SocketException)
            {
                TxtDisplay.Text += "\nError writing object\n";
            } 
        } 
        public void RunServer()
        {
            TcpListener listener;

            
            try
            {
                IPAddress local = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(local, 50000);

                listener.Start();

                while (true)
                {
                    DisplayMessage("Waiting for connection\r\n");

                    var connection = listener.AcceptSocket();

                    ThreadPool.QueueUserWorkItem(RunClientInThread, connection);
                    counter++;
                } 
            } 
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            } 
        } 

        public void SortByToken(object sender, EventArgs e)
        {
            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );

            String path = Path.Combine(systemPath,"pairs_Sorted_By_Token.txt");

            try
            {
                using (FileStream fs = File.Create(path))
                {
                    List<CardTokenPair> list = pairs.OrderBy(p => p.Token).ToList();
                    String pairsStr = "";
                    foreach (CardTokenPair pair in list)
                    {
                        pairsStr += pair + "\n";
                    }
                    DisplayMessage(pairsStr + "\r\n");
                    byte[] info = new UTF8Encoding(true).GetBytes(pairsStr);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.ToString());

            }


        }
        public void SortByCard(object sender, EventArgs e)
        {
            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );

            String path = Path.Combine(systemPath, "pairs_Sorted_By_CardNo.txt");
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    List<CardTokenPair> list = pairs.OrderBy(p => p.Card).ToList();
                    String pairsStr = "";
                    foreach (CardTokenPair pair in list)
                    {
                        pairsStr += pair + "\n";
                    }
                    DisplayMessage(pairsStr+"\r\n");
                    byte[] info = new UTF8Encoding(true).GetBytes(pairsStr);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.ToString());

            }

        }
        private void DisplayMessage(string message)
        {
            
            if (!TxtDisplay.Dispatcher.CheckAccess())
            {
                TxtDisplay.Dispatcher.Invoke(new Action(() 
                                                 => TxtDisplay.Text += message));

            } 
            else 
                TxtDisplay.Text += message;
        } 
        private void EnableInput(bool value)
        {
            
            if (!TxtInput.Dispatcher.CheckAccess())
            {
                
                                        
                TxtInput.Dispatcher.Invoke(new Action(() => TxtInput.IsEnabled = value));
            } 
            else 
                TxtInput.IsEnabled = value;
        } 

        public void SendMessage(String message, Thread client)
        {
            try
            {

                bf.Serialize(writers[client], message);
                

                if (message.Equals("TERMINATE"))
                {
                    sockets[client].Close();
                    DisplayMessage("True");
                }
            } // end try
            catch (SocketException)
            {
                TxtDisplay.Text += "\nError writing object\n";
            }
        }

        public String GenerateToken(String card)
        {
            char[] tokenArray = new char[16];
            int indexSum = 0;

            for (int i = 12; i <= 15; i++)
            {
                tokenArray[i] = card[i];
                indexSum = tokenArray[i] - '0';
            }

            int newIndexSum;
            Random random = new Random();
            bool unique = true;
            String token="";

            do
            {
                int temp;
                newIndexSum = indexSum;
                do
                {
                    temp = random.Next(10);
                } while (temp == 3 || temp == 4 || temp == 5 || temp == 6);

                tokenArray[0] = (char)(temp + '0');
                newIndexSum += temp;

                for(int i = 1; i < 12; i++)
                {
                    do
                    {
                        temp = random.Next(10);
                    } while (temp == card[i] - '0');
                    tokenArray[i] = (char)(temp + '0');
                    newIndexSum += temp;
                }
                token = new string(tokenArray);
                foreach(CardTokenPair pair in pairs)
                {
                    if (token.Equals(pair.Token))
                    {
                        unique = false;
                        break;
                    }
                }

            }
            while (newIndexSum % 10 == 0 || !unique);


            return token;
        }
        private void RunClientInThread(object socket)
        {

            Socket connection = (Socket)socket;
            int cnt = counter;
            NetworkStream socketStream = new NetworkStream(connection);

            sockets.Add(Thread.CurrentThread, connection);
            writers.Add(Thread.CurrentThread, socketStream);

            DisplayMessage("Connection " + cnt + " received.\r\n");

            bf.Serialize(socketStream, " Connection successful");
            if (writers.Count == 1)
            {
                EnableInput(true); 
            }


            String message = "";
            do
            {
                try
                {
                    message = (string)bf.Deserialize(socketStream);
                    String[] parts = message.Split(' ');

                    if (parts[0].Equals("Verify"))
                    {
                        bool isValid = false;
                        DisplayMessage("\nCLIENT" + counter + ">>> " + message + "\r\n");
                        foreach( User user in users)
                        {
                            if (user.UserName.Equals(parts[1]) && user.Password.Equals(parts[2]))
                            {
                                isValid = true;
                                String reply = "Valid User.";
                                if (user.CanExtract)
                                {
                                    reply += "Can Extract.";
                                }
                                if (user.CanRegister)
                                {
                                    reply += "Can Register.";
                                }
                                SendMessage(reply, Thread.CurrentThread);

                            }

                        }
                        if (!isValid)
                        {
                            SendMessage("Invalid User Name or Password.", Thread.CurrentThread);
                        }
                    }
                    else if (parts[0].Equals("Extract"))
                    {
                        DisplayMessage("\nCLIENT " + counter + ">>> Extract " + parts[1] + "\r\n");
                        bool isRegistered = false;
                        foreach (CardTokenPair pair in pairs)
                        {
                            if (parts[1].Equals(pair.Token))
                            {
                                DisplayMessage("\nSERVER>>> " + pair.Card + "\r\n");
                                SendMessage(pair.Card, Thread.CurrentThread);
                                isRegistered = true;
                                break;
                            }
                        }
                        if (!isRegistered)
                        {
                            DisplayMessage("\nSERVER>>> Token isn't registered.\r\n");
                            SendMessage("Token isn't registered.", Thread.CurrentThread);
                        }
                    }
                    else if (parts[0].Equals("Register"))
                    {
                        DisplayMessage("\nCLIENT " + counter + ">>> Register " + parts[1] + "\r\n");
                        if (CardTokenPair.ValidCard(parts[1]))
                        {
                            String token = GenerateToken(parts[1]);
                            SendMessage(token, Thread.CurrentThread);
                            pairs.Add(new CardTokenPair(parts[1], token));

                            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );

                            var pairsPath = Path.Combine(systemPath, "Pairs.txt");

                            using (StreamWriter sw = File.AppendText(pairsPath))
                            {
                                sw.WriteLine(parts[1]+"'"+token);
                            }

                            var pairsPathXML = Path.Combine(systemPath, "Pairs.xml");
                            XmlSerializer serializer = new XmlSerializer(pairs.GetType());
                            using (StreamWriter writer = new StreamWriter(pairsPathXML))
                            {
                                serializer.Serialize(writer, pairs);
                            }

                        }
                        else
                        {
                            SendMessage("Invalid Card Number.", Thread.CurrentThread);
                            DisplayMessage("\nSERVER>>> Invalid Card Number.\r\n");
                        }

                    }
                    else
                    {
                        DisplayMessage(message);
                    }

                } 
                catch (Exception)
                {
                    break;
                } 
            } while (message != "TERMINATE" &&
               connection.Connected);

            DisplayMessage("\r\nUser terminated connection\r\n");

            writers.Remove(Thread.CurrentThread);
            sockets.Remove(Thread.CurrentThread);
            socketStream?.Close();
            connection?.Close();
            if (writers.Count == 0)
            {
                EnableInput(false); 
            }
          
        }
    }
}
