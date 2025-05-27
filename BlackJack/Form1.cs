using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class Form1 : Form
    {
        public Deck deck = new Deck();
        public List<Card> playerHand = new List<Card>();
        public List<Card> dealerHand = new List<Card>();
        public List<PictureBox> placedCards = new List<PictureBox>();
        public List<PictureBox> dealerPlaced = new List<PictureBox>();
        public bool stayed = false;
        public PictureBox hidden;
        bool isFirst = true;

        public Button hitButton = new Button();
        public Button stayButton = new Button();
        public Label winner = new Label();
        public Label playerScore = new Label();


        public Form1()
        {
            InitializeComponent();
        }

        public async void StartGame()
        {
            Setup();

            await DealCards();

            if (CheckOverflow())
            {
                GameOver();
            }
        }

        public async Task DealCards()
        {
            await Task.Delay(200);
            playerHand.Add(deck.DealCard());
            Render();
            await Task.Delay(200);
            dealerHand.Add(deck.DealCard());
            Render();
            await Task.Delay(200);
            playerHand.Add(deck.DealCard());
            Render();
            await Task.Delay(200);
            dealerHand.Add(deck.DealCard());
            Render();
        }

        public void Render()
        {
            int playerOffset = 0;
            int dealerOffset = 0;

            foreach (var card in playerHand)
            {
                PictureBox cardPicture = new PictureBox();
                cardPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                cardPicture.Image = Image.FromFile(@"..\..\Cards\" + card.url);
                cardPicture.BackColor = Color.DarkGreen;

                cardPicture.Width = 100;
                cardPicture.Height = 150;

                cardPicture.Location = new System.Drawing.Point(50 + playerOffset, 350);
                playerOffset += 150;

                this.Controls.Add(cardPicture);
                placedCards.Add(cardPicture);
            }
            
            if (stayed) { isFirst = false; }

            foreach (var card in dealerHand)
            {
                PictureBox cardPicture = new PictureBox();
                cardPicture.SizeMode = PictureBoxSizeMode.StretchImage;

                if (stayed)
                {
                    Controls.Remove(hidden);
                    hidden.Dispose();
                    cardPicture.Image = Image.FromFile(@"..\..\Cards\" + card.url);
                }
                else if (isFirst)
                {
                    isFirst = false;
                    cardPicture.Image = Image.FromFile(@"..\..\Cards\" + "red_back.png");
                    hidden = cardPicture;
                }
                else
                {
                    cardPicture.Image = Image.FromFile(@"..\..\Cards\" + card.url);
                }

                cardPicture.BackColor = Color.DarkGreen;

                cardPicture.Width = 100;
                cardPicture.Height = 150;

                cardPicture.Location = new System.Drawing.Point(47 + dealerOffset, 150);
                dealerOffset += 150;

                this.Controls.Add(cardPicture);
                placedCards.Add(cardPicture);
                dealerPlaced.Add(cardPicture);
            }


            playerScore.Text = "Player Score: (" + HandSum(playerHand).ToString() + ")";
        }

        public void Setup()
        {
            this.BackgroundImage = Image.FromFile(@"..\..\Cards\" + "background3.png");

            stayed = false;
            isFirst = true;

            foreach (PictureBox p in placedCards)
            {
                this.Controls.Remove(p);
                p.Dispose();
            }

            Controls.Remove(winner);

            this.Controls.Remove(playerScore);
            playerScore.Dispose();

            deck = new Deck();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();

            playerScore = new Label();
            playerScore.Location = new System.Drawing.Point(75, 540);
            playerScore.Text = "Player Score: (" + HandSum(playerHand).ToString() + ")";
            playerScore.Font = new Font("Arial", 16);
            playerScore.Width = 500;
            playerScore.BackColor = Color.Transparent;
            playerScore.ForeColor = Color.White;
            Controls.Add(playerScore);


            hitButton.Enabled = true;
            stayButton.Enabled = true;

            if (hitButton.Text != "Hit")
            {
                hitButton.Text = "Hit";
                hitButton.Location = new System.Drawing.Point(50, 600);
                hitButton.Width = 100;
                hitButton.Height = 50;
                hitButton.Font = new Font("Arial", 12);
                hitButton.FlatStyle = FlatStyle.Popup;
                hitButton.BackColor = Color.Coral;

                hitButton.Click += HitButton_Click;

                this.Controls.Add(hitButton);

                stayButton.Text = "Stay";
                stayButton.Location = new System.Drawing.Point(200, 600);
                stayButton.Width = 100;
                stayButton.Height = 50;
                stayButton.Font = new Font("Arial", 12);
                stayButton.FlatStyle = FlatStyle.Popup;
                stayButton.BackColor = Color.AliceBlue;

                stayButton.Click += StayButton_Click;

                this.Controls.Add(stayButton);
            }

            if (CheckOverflow())
            {
                GameOver();
            }
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            StartGame();

            btnStartGame.Text = "New Hand";
        }


        private async void HitButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            playerHand.Add(deck.DealCard());
            Render();
            if (CheckOverflow())
            {
                GameOver();
            }
        }

        private void StayButton_Click(object sender, EventArgs e)
        {
            hitButton.Enabled = false;
            stayButton.Enabled = false;

            stayed = true;

            Reveal();
        }

        public bool CheckOverflow()
        {
            int playerSum = HandSum(playerHand);
            int dealerSum = HandSum(dealerHand);

            if (playerSum > 21)
            {
                return true;
            }
            else if (playerSum == 21)
            {
                return true;
            }

            if (dealerSum > 21)
            {
                return true;
            }
            else if (dealerSum == 21)
            {
                return true;
            }

            return false;
        }

        public int HandSum(List<Card> hand)
        {
            int sum = 0;

            foreach (var card in hand)
            {
                if (card.value == 1)
                {
                    if (sum + 11 > 21)
                    {
                        sum += 1;
                    }
                    else
                    {
                        sum += 11;
                    }
                }
                else
                {
                    sum += card.value;
                }
            }

            return sum;
        }

        public async Task Reveal()
        {
            Render();

            int playerSum = HandSum(playerHand);

            while (HandSum(dealerHand) < playerSum)
            {
                await Task.Delay(200);
                dealerHand.Add(deck.DealCard());
                Render();
                if (CheckOverflow())
                {
                    GameOver();
                }
            }

            GameOver();
        }

        public async void GameOver()
        {
            await Task.Delay(300);

            hitButton.Enabled = false;
            stayButton.Enabled = false;

            int playerSum = HandSum(playerHand);
            int dealerSum = HandSum(dealerHand);

            bool win = false;

            winner.ForeColor = Color.White;

            if (playerSum == dealerSum)
            {
                winner.Text = $"It's a push! ({playerSum})";
                winner.ForeColor = Color.Orange;
            }
            else if (playerSum > 21)
            {
                winner.Text = $"You Bust! ({playerSum}) You Lose.";
            }
            else if (dealerSum > 21)
            {
                winner.Text = $"Dealer Busts! ({dealerSum}) You Win!";
                win = true;
            }
            else if (playerSum == 21 && dealerSum == 21)
            {
                if (playerHand.Count > dealerHand.Count)
                {
                    winner.Text = $"2 Card Blackjack! You Win! ({playerSum})";
                    win = true;
                } else if (dealerHand.Count > playerHand.Count)
                {
                    winner.Text = $"Dealer 2 Card Blackjack! You Lose! ({playerSum})";
                } else
                {
                    winner.Text = $"It's a push! ({playerSum})";
                    winner.ForeColor = Color.Orange;
                }
            }
            else if (playerSum == 21)
            {
                winner.Text = $"Blackjack! You Win! ({playerSum})";
                win = true;
            }
            else if (dealerSum == 21)
            {
                winner.Text = $"Dealer Blackjack! You Lose. ({dealerSum})";
            }
            else if (dealerSum > playerSum)
            {
                winner.Text = $"Dealer's Hand ({dealerSum}) is bigger. You Lose.";
            }
            else
            {
                winner.Text = $"Your Hand ({playerSum}) is bigger. You Win!";
                win = true;
            }

            winner.Width = 1000;
            winner.Height = 100;
            winner.Font = new Font("Arial", 30);
            winner.BackColor = Color.Transparent;

            if (winner.ForeColor != Color.Orange)
            {
                if (win)
                {
                    winner.ForeColor = Color.White;
                }
                else
                {
                    winner.ForeColor = Color.Red;
                }
            }

            winner.Location = new System.Drawing.Point(40, 50);
            Controls.Add(winner);

            stayed = true;

            Render();
        }
    }
}
