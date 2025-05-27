using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Card
    {
        public int index;
        public int value;
        public string url;

        public Card(int index, int value, string url)
        {
            this.index = index;
            this.value = value;
            this.url = url;
        }
    }
}
