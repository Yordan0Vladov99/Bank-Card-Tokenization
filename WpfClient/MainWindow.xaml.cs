using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace WpfChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkStream output;       
        private BinaryFormatter bf;   
        private Thread readThread; 
        private string message = "";
        public MainWindow()
        {
            InitializeComponent();
            bf = new BinaryFormatter();
            readThread = new Thread(new ThreadStart(RunClient));
            readThread.Start();
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
                  
                    bf.Serialize(output,"CLIENT>>> " + TxtInput.Text);
                  
                    TxtDisplay.Text += "\r\nCLIENT>>> " + TxtInput.Text + "\r\n";
                    TxtInput.Clear();
                } 
            } 
            catch (SocketException)
            {
                TxtDisplay.Text += "\nError writing object\r\n";
            } 
        } 

        private void SubmitUser(object sender, EventArgs e)
        {

            if (Regex.IsMatch(UserName.Text, @"\s") || Regex.IsMatch(Password.Text, @"\s"))
            {
                DisplayMessage("Invalid username or password.\r\n");
            }
            else
            {
                SendMessage("Verify " + UserName.Text + " " + Password.Text);
            }
           
        }
        public void ExtractCard(object sender, EventArgs e)
        {
            String message = TxtInput.Text;

            if(!Regex.IsMatch(message, @"^[0-9]{16}$"))
            {
                DisplayMessage("Invalid Token.\r\n");
            }
            else
            {
                SendMessage("Extract " + message);
            }
        }

        public void RegisterToken(object sender, EventArgs e)
        {
            String message = TxtInput.Text;

            if (!Regex.IsMatch(message, @"^[0-9]{16}$"))
            {
                DisplayMessage("Invalid Card Number.\r\n");
            }
            else
            {
                SendMessage("Register " + message);
            }
        }
        public void SendMessage(String str)
        {
            try
            {
                bf.Serialize(output, str);
                    DisplayMessage("CLIENT>>> " + str +"\r\n");
            } 
            catch (SocketException)
            {
                DisplayMessage("Error writing object\r\n");
            } 
        }
        public void RunClient()
        {
            TcpClient client=null;

            try
            {
                DisplayMessage("Attempting connection\r\n");

                client = new TcpClient();
                client.Connect("127.0.0.1", 50000);

                output = client.GetStream();


                DisplayMessage("\r\nGot I/O streams\r\n");
                EnableInput(true); 

                do
                {
                    try
                    {
                        message = (string)bf.Deserialize(output);
                        checkValidUser(message);
                        
                        DisplayMessage("\r\nSERVER>>>" + message + "\r\n");
                    } 
                    catch (Exception)
                    {
                        System.Environment.Exit(System.Environment.ExitCode);
                    } 
                } while (message != "SERVER>>> TERMINATE");

               
            } 
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "Connection Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
                
            } 
            finally
            {
                output?.Close();
                client?.Close();

                System.Environment.Exit(System.Environment.ExitCode);
            }
        } 

        private void checkValidUser(String message)
        {
            if (message.Equals("Valid User.Can Extract.Can Register."))
            {
                Dispatcher.Invoke(new Action(() => { UserInput.Visibility = Visibility.Hidden; }));
                Dispatcher.Invoke(new Action(() => { RegisteredUserInput.Visibility = Visibility.Visible; }));
            }
            else if (message.Equals("Valid User.Can Extract."))
            {

                Dispatcher.Invoke(new Action(() => { UserInput.Visibility = Visibility.Hidden; }));
                Dispatcher.Invoke(new Action(() => { RegisteredUserInput.Visibility = Visibility.Visible; }));
                Dispatcher.Invoke(new Action(() => { RegisterButton.Visibility = Visibility.Hidden; }));
                Dispatcher.Invoke(new Action(() => { ExtractButton.Width = 488; }));
            }
            else if (message.Equals("Valid User.Can Register."))
            {
                Dispatcher.Invoke(new Action(() => { UserInput.Visibility = Visibility.Hidden; }));
                Dispatcher.Invoke(new Action(() => { RegisteredUserInput.Visibility = Visibility.Visible; }));
                Dispatcher.Invoke(new Action(() => { ExtractButton.Visibility = Visibility.Hidden; }));
                Dispatcher.Invoke(new Action(() => { RegisterButton.Width = 488; }));
            }
        }
        private void DisplayMessage(string message)
        {
            if (!TxtDisplay.Dispatcher.CheckAccess())
            {
                TxtDisplay.Dispatcher.Invoke(new Action(() => TxtDisplay.Text += message));

            } 
            else 
                TxtDisplay.Text += message+"\n";
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

        private void TxtInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {

                if (TxtInput.IsEnabled == true)
                {
                    bf.Serialize(output,"CLIENT>>> " + e.Text);
                    //writer.Write("CLIENT>>> " + e.Text);
                    TxtDisplay.Text += "\r\nCLIENT>>> " + e.Text;
                    TxtInput.Clear();
                } // end if
            } // end try
            catch (SocketException)
            {
                TxtDisplay.Text += "\nError writing object";
            } // end catch
        }

        private void TxtInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        } // end method TxtInput_KeyDown


    }
}
