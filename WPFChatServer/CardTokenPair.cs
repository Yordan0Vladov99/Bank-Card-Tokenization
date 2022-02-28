using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPFChatServer
{
    public class CardTokenPair
    {

        private String card;
        private String token;

        public String Card
        {
            get
            {
                return card;
            }
            set
            {
                if (ValidCard(value))
                {
                    card = value;
                }
                
            }
        }
        public String Token
        {
            get
            {
                return token;
            }
            set
            {
                if (ValidToken(value, card))
                {
                    token = value;
                }
            }
        }
        public CardTokenPair(String card, String token)
        {
            Card = card;
            Token = token;
        }

        public CardTokenPair()
        {
            card = "0000000000000000";
            token = "0000000000000000";
        }

        public static bool ValidCard(String card)
        {
            bool matchFound = Regex.IsMatch(card, @"^[3-6][0-9]{15}$");
            if (!matchFound)
            {
                return false;
            }
            int nDigits = 16;
            int nSum = 0;
            bool isSecond = false;
            for(int i = nDigits - 1; i>=0; i--)
            {
                int d = card[i]-'0';
                if (isSecond)
                {
                    d = d * 2;
                }

                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }

            return (nSum % 10 == 0);
        }

        public static bool ValidToken(String token, String card)
        {
            bool matchFound = Regex.IsMatch(token, @"^[^3-6][0-9]{15}$");
            if (!matchFound)
            {
                return false;
            }
            int nDigits = 16;
            int nSum = 0;
            for (int i = nDigits - 1; i >= 0; i--)
            {
                int d = token[i] - '0';
                if( i <= nDigits - 1 && i>= nDigits - 4)
                {
                    if (token[i] != card[i])
                    {
                        return false;
                    }
                }
                else
                {
                    if(token[i] == card[i])
                    {
                        return false;
                    }
                }
                nSum += d;
            }

            return (nSum % 10 != 0);
        }
        public override string ToString()
        {
            return Card + "\'" + Token;
        }
    }
}
