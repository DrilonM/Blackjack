using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Deck
    {
        List<Card> cardList = new List<Card>();
        Stack<Card> cards = new Stack<Card>();
        Random random = new Random();
        Dictionary<int, string> allCards = new Dictionary<int, string>
        {
            // Spades
            {1, "AS.png"},
            {2, "2S.png"},
            {3, "3S.png"},
            {4, "4S.png"},
            {5, "5S.png"},
            {6, "6S.png"},
            {7, "7S.png"},
            {8, "8S.png"},
            {9, "9S.png"},
            {10, "10S.png"},
            {11, "JS.png"},
            {12, "QS.png"},
            {13, "KS.png"},

            // Hearts
            {14, "AH.png"},
            {15, "2H.png"},
            {16, "3H.png"},
            {17, "4H.png"},
            {18, "5H.png"},
            {19, "6H.png"},
            {20, "7H.png"},
            {21, "8H.png"},
            {22, "9H.png"},
            {23, "10H.png"},
            {24, "JH.png"},
            {25, "QH.png"},
            {26, "KH.png"},

            // Diamonds
            {27, "AD.png"},
            {28, "2D.png"},
            {29, "3D.png"},
            {30, "4D.png"},
            {31, "5D.png"},
            {32, "6D.png"},
            {33, "7D.png"},
            {34, "8D.png"},
            {35, "9D.png"},
            {36, "10D.png"},
            {37, "JD.png"},
            {38, "QD.png"},
            {39, "KD.png"},

            // Clubs
            {40, "AC.png"},
            {41, "2C.png"},
            {42, "3C.png"},
            {43, "4C.png"},
            {44, "5C.png"},
            {45, "6C.png"},
            {46, "7C.png"},
            {47, "8C.png"},
            {48, "9C.png"},
            {49, "10C.png"},
            {50, "JC.png"},
            {51, "QC.png"},
            {52, "KC.png"}
        };
        public Deck()
        {
            for (int i = 0; i < 52; i++)
            {
                int value = translate(i + 1);
                
                cardList.Add(new Card(i + 1, value, allCards[i + 1]));

                Shuffle(cardList);

                foreach (var card in cardList)
                {
                    cards.Push(card);
                }
            }
        }
        public void Shuffle(List<Card> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                Card temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        public Card DealCard()
        {
            return cards.Pop();
        }


        public int translate(int x)
        {
            int value = 0;

            if (x < 14)
            {
                value = x;
            } else if (x < 27)
            {
                value = x - 13;
            } else if (x < 40)
            {
                value = x - 26;
            } else
            {
                value = x - 39;
            }

            if (value > 9)
            {
                return 10;
            }

            return value;
        }
    }
}
