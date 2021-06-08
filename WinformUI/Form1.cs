using Lotto.Biz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WinformUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<int> playersNumbers;
        private int GameCounter;
        private int totalWinCount;
        private int JackpotCounter;
        private int FivePlusCoutner;
        private int FiveCounter;
        private int FourCounter;
        private int ThreeCoutner;
        private int TwoCounter;

        private int PrizePool;
        private decimal ticketPrice;
        private decimal totalSpent;
        private decimal net;

        private void btnGetRandomNumbers_Click(object sender, EventArgs e)
        {
            playersNumbers = LottoGame.GenerateBalls(7);
            var playNumbersMinusBouns = new List<int>(playersNumbers);
            playNumbersMinusBouns.RemoveAt(6);
            playNumbersMinusBouns.Sort();
            pBall1.Text = playNumbersMinusBouns[0].ToString();
            pBall2.Text = playNumbersMinusBouns[1].ToString();
            pBall3.Text = playNumbersMinusBouns[2].ToString();
            pBall4.Text = playNumbersMinusBouns[3].ToString();
            pBall5.Text = playNumbersMinusBouns[4].ToString();
            pBall6.Text = playNumbersMinusBouns[5].ToString();
            pBallBouns.Text = playersNumbers[6].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playersNumbers = new List<int>();
            try
            {
                playersNumbers.Add(Convert.ToInt32(pBall1.Text));
                playersNumbers.Add(Convert.ToInt32(pBall2.Text));
                playersNumbers.Add(Convert.ToInt32(pBall3.Text));
                playersNumbers.Add(Convert.ToInt32(pBall4.Text));
                playersNumbers.Add(Convert.ToInt32(pBall5.Text));
                playersNumbers.Add(Convert.ToInt32(pBall6.Text));
                playersNumbers.Add(Convert.ToInt32(pBallBouns.Text));

                if (NumberVaildator(playersNumbers)) { MessageBox.Show("Duplicated Numbers Entered"); };
            }
            catch (Exception)
            {
                MessageBox.Show($"Only numbers are allowed");
            }
        }

        private bool NumberVaildator(List<int> playerNumbers)
        {
            IEnumerable<int> dups = playerNumbers.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(x => x.Key);

            if (dups.Count() > 0) return true;
            return false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (playersNumbers == null)
            {
                MessageBox.Show("Please provide numbers");
            }
            else
            {
                if (NumberVaildator(playersNumbers))
                {
                    MessageBox.Show("Invalid Numbers");
                }
                else
                {
                    if (rdSingleGame.Checked)
                    {
                        PlayaGame();
                        DisplayStats();
                    };
                    if (rdNumberOfGames.Checked)
                    {
                        for (int i = 0; i < upDownGameCount.Value; i++)
                        {
                            PlayaGame();
                            //Populate Results
                            DisplayStats();
                        }
                    }
                    if (rdGross.Checked) 
                    {
                        while (PrizePool <= upDownGross.Value)
                        {
                            PlayaGame();
                            DisplayStats();
                        }
                    };
                    if (rdNet.Checked) 
                    {
                        while (net <= upDownNet.Value)
                        {
                            PlayaGame();
                            DisplayStats();
                        }
                    };
                    if (rdTillJackpot.Checked)
                    {
                        while (JackpotCounter == 0)
                        {
                            var lottoGame = new LottoGame(playersNumbers, 10000000);
                            GameCounter++;
                            tbResultPanel.AppendText($"Players Balls: {string.Join(", ", lottoGame.ResultBalls)}");
                            tbResultPanel.AppendText(Environment.NewLine);
                            tbResultPanel.AppendText($"Players Balls: {string.Join(", ", lottoGame.PlayersBalls)}");
                            tbResultPanel.AppendText(Environment.NewLine);
                            tbResultPanel.AppendText(Environment.NewLine);
                            if (lottoGame.IsWin)
                            {
                                switch (lottoGame.Prize)
                                {
                                    case Prize.Jackpot:
                                        JackpotCounter++;
                                        totalWinCount++;
                                        PrizePool += lottoGame.PrizeMoney;
                                        break;

                                    case Prize.FivePlusBonus:
                                        FivePlusCoutner++;
                                        totalWinCount++;
                                        PrizePool += lottoGame.PrizeMoney;
                                        break;

                                    case Prize.MatchFive:
                                        FiveCounter++;
                                        totalWinCount++;
                                        PrizePool += lottoGame.PrizeMoney;
                                        break;

                                    case Prize.MatchFour:
                                        FourCounter++;
                                        totalWinCount++;
                                        PrizePool += lottoGame.PrizeMoney;
                                        break;

                                    case Prize.MatchThree:
                                        totalWinCount++;
                                        ThreeCoutner++;
                                        PrizePool += lottoGame.PrizeMoney;
                                        break;

                                    case Prize.MatchTwo:
                                        totalWinCount++;
                                        TwoCounter++;
                                        break;

                                    case Prize.None:
                                        break;

                                    default:
                                        break;
                                }
                            }
                            //Populate Results
                            tbGameCount.Text = GameCounter.ToString();
                            tbTotalWins.Text = totalWinCount.ToString();
                            tbJackpotWins.Text = JackpotCounter.ToString();
                            tbFivePlusWins.Text = FivePlusCoutner.ToString();
                            tbFiveWins.Text = FiveCounter.ToString();
                            tbFourWins.Text = FourCounter.ToString();
                            tbThreeWins.Text = ThreeCoutner.ToString();
                            tbTwoWins.Text = TwoCounter.ToString();
                            tbPrizePool.Text = PrizePool.ToString();
                        }
                    }
                }
            }
        }

        private void PlayaGame()
        {
            if (chkbLuckyDip.Checked)
            {
                playersNumbers = LottoGame.GenerateBalls(7);
            }
            ticketPrice = Convert.ToDecimal(tbTicketPrice.Text);
            var lottoGame = new LottoGame(playersNumbers, 10000000);
            GameCounter++;
            tbResultPanel.AppendText($"Players Balls: {string.Join(", ", lottoGame.PlayersBalls)}");
            tbResultPanel.AppendText(Environment.NewLine);
            tbResultPanel.AppendText($"Results Balls: {string.Join(", ", lottoGame.ResultBalls)}");
            tbResultPanel.AppendText(Environment.NewLine);
            tbResultPanel.AppendText(Environment.NewLine);
            if (lottoGame.IsWin)
            {
                switch (lottoGame.Prize)
                {
                    case Prize.Jackpot:
                        JackpotCounter++;
                        totalWinCount++;
                        PrizePool += lottoGame.PrizeMoney;
                        break;

                    case Prize.FivePlusBonus:
                        FivePlusCoutner++;
                        totalWinCount++;
                        PrizePool += lottoGame.PrizeMoney;
                        break;

                    case Prize.MatchFive:
                        FiveCounter++;
                        totalWinCount++;
                        PrizePool += lottoGame.PrizeMoney;
                        break;

                    case Prize.MatchFour:
                        FourCounter++;
                        totalWinCount++;
                        PrizePool += lottoGame.PrizeMoney;
                        break;

                    case Prize.MatchThree:
                        totalWinCount++;
                        ThreeCoutner++;
                        PrizePool += lottoGame.PrizeMoney;
                        break;

                    case Prize.MatchTwo:
                        totalWinCount++;
                        TwoCounter++;
                        break;

                    case Prize.None:
                        break;

                    default:
                        break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ResetStats()
        {
            GameCounter = 0;
            totalWinCount = 0;
            JackpotCounter = 0;
            FivePlusCoutner = 0;
            FiveCounter = 0;
            FourCounter = 0;
            ThreeCoutner = 0;
            TwoCounter = 0;
            PrizePool = 0;
            totalSpent = ticketPrice * GameCounter;
            net = PrizePool - totalSpent;

            DisplayStats();
            tbResultPanel.Clear();
        }

        private void DisplayStats()
        {
            tbGameCount.Text = GameCounter.ToString();
            tbTotalWins.Text = totalWinCount.ToString();
            tbJackpotWins.Text = JackpotCounter.ToString();
            tbFivePlusWins.Text = FivePlusCoutner.ToString();
            tbFiveWins.Text = FiveCounter.ToString();
            tbFourWins.Text = FourCounter.ToString();
            tbThreeWins.Text = ThreeCoutner.ToString();
            tbTwoWins.Text = TwoCounter.ToString();
            tbPrizePool.Text = $"£{PrizePool.ToString()}";

            totalSpent = ticketPrice * GameCounter;
            tbTotalSpent.Text = totalSpent.ToString();
            net = PrizePool - totalSpent;
            tbNet.Text = net.ToString();
        }

        private void btnResetStats_Click(object sender, EventArgs e)
        {
            ResetStats();
        }
    }
}