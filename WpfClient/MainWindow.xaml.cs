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

        //private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Key == Key.Return && TxtInput.IsEnabled == true)
        //        {
        //          
        //            bf.Serialize(output,"CLIENT>>> " + TxtInput.Text);
        //          
        //            TxtDisplay.Text += "\r\nCLIENT>>> " + TxtInput.Text + "\r\n";
        //            TxtInput.Clear();
        //        } 
        //    } 
        //    catch (SocketException)
        //    {
        //        TxtDisplay.Text += "\nError writing object\r\n";
        //    } 
        //} 

        private void SubmitUser(object sender, EventArgs e)
        {

            if (Regex.IsMatch(UserName.Text, @"\s") || Regex.IsMatch(Password.Password, @"\s"))
            {
                //DisplayMessage("Invalid username or password.\r\n");
                bool validUser = true;
                bool validPassword = true;
                if (Regex.IsMatch(UserName.Text, @"\s"))
                {
                    validUser = false;
                }

                if (Regex.IsMatch(Password.Password, @"\s"))
                {
                    Dispatcher.Invoke(new Action(() => { PasswordError.Content = "Invalid Password."; }));
                    validPassword = false;
                }
                DisplayError(validUser, validPassword);

            }
            else
            {
                SendMessage("Verify " + UserName.Text + " " + Password.Password);
            }

        }

        private void DisplayError(bool validUser, bool validPassword)
        {
            if (!validUser)
            {
                Dispatcher.Invoke(new Action(() => { UserNameError.Visibility = Visibility.Visible; }));
            }
            else if (UserNameError.Visibility == Visibility.Visible)
            {
                Dispatcher.Invoke(new Action(() => { UserNameError.Visibility = Visibility.Hidden; }));
            }

            if (!validPassword)
            {
                Dispatcher.Invoke(new Action(() => { PasswordError.Visibility = Visibility.Visible; }));
            }
            else if (PasswordError.Visibility == Visibility.Visible)
            {
                Dispatcher.Invoke(new Action(() => { PasswordError.Visibility = Visibility.Hidden; }));
            }
        }
        private void ExtractCard(object sender, EventArgs e)
        {
            String message = TxtInput.Text;

            if (!Regex.IsMatch(message, @"^[0-9]{16}$"))
            {
                DisplayInputError(true, "Invalid token number.");
            }
            else
            {
                DisplayInputError(false, "");
                SendMessage("Extract " + message);
            }
        }

        private void RegisterToken(object sender, EventArgs e)
        {
            String message = TxtInput.Text;

            if (!Regex.IsMatch(message, @"^[0-9]{16}$"))
            {
                DisplayInputError(true, "Invalid Card Number.\r\n");

            }
            else
            {
                DisplayInputError(false, "");
                SendMessage("Register " + message);
            }
        }

        private void DisplayInputError(bool isError, string errorMessage)
        {
            if (isError)
            {
                Dispatcher.Invoke(new Action(() => { InputError.Visibility = Visibility.Visible; InputError.Content = errorMessage; }));

            }
            else
            {
                Dispatcher.Invoke(new Action(() => { InputError.Visibility = Visibility.Hidden; }));

            }
        }
        private void SendMessage(String str)
        {
            try
            {
                DisplayInputError(false, "");
                bf.Serialize(output, str);
                // DisplayMessage("CLIENT>>> " + str +"\r\n");
            }
            catch (SocketException)
            {
                DisplayInputError(true, "Error writing object\r\n");
            }
        }
        private void RunClient()
        {
            TcpClient client = null;

            try
            {

                client = new TcpClient();
                client.Connect("127.0.0.1", 50000);

                output = client.GetStream();


                EnableInput(true);

                do
                {
                    try
                    {
                        message = (string)bf.Deserialize(output);

                        if (message == "Invalid User Name or Password.")
                        {
                            Dispatcher.Invoke(new Action(() => { PasswordError.Content = "Invalid User Name or Password."; }));
                            DisplayError(true, false);
                        }
                        else
                        {
                            checkValidUser(message);
                            DisplayError(true, true);
                            DisplayMessage(message);
                        }

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
            TxtDisplay.Dispatcher.Invoke(new Action(() => TxtDisplay.Text = message));
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

        //private void TxtInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //
        //        if (TxtInput.IsEnabled == true)
        //        {
        //            bf.Serialize(output,"CLIENT>>> " + e.Text);
        //            //writer.Write("CLIENT>>> " + e.Text);
        //            TxtDisplay.Text += "\r\nCLIENT>>> " + e.Text;
        //            TxtInput.Clear();
        //        } // end if
        //    } // end try
        //    catch (SocketException)
        //    {
        //        TxtDisplay.Text += "\nError writing object";
        //    } // end catch
        //}
        //
        //private void TxtInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //
        //} // end method TxtInput_KeyDown


    }
}
