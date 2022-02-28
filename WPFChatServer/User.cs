using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChatServer
{
    public class User
    {
        private String userName;
        private String password;
        private bool canRegister;
        private bool canExtract;

        public User(String userName, String password, bool canRegister, bool canExtract)
        {
            UserName = userName;
            Password = password;
            CanRegister = canRegister;
            CanExtract = canExtract;
        }
        public User(): this("", "", false, false)
        {
        }
        public String UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        public String Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public bool CanRegister
        {
            get
            {
                return canRegister;
            }
            set
            {
                canRegister = value;
            }
        }

        public bool CanExtract
        {
            get
            {
                return canExtract;
            }
            set
            {
                canExtract = value;
            }
        }
    }
}
