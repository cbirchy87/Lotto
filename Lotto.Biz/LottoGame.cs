using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lotto.Biz
{
    public class LottoGame
    {
        public List<int> ResultBalls { get; set; }
        public List<int> PlayersBalls { get; }
        public Prize Prize { get; }
        public bool IsWin { get;  }
        public int PrizeMoney { get; }
        public int jackpot { get; }
        public bool WonLD { get;  }

        public LottoGame(List<int> playersBalls, int jackpotvalue)
        {
            PlayersBalls = playersBalls;
            ResultBalls = GenerateBalls(7);

            Prize = GetPrize(playersBalls, ResultBalls);
            IsWin = IsAWin();
            jackpot = jackpotvalue;

            if (IsWin)
            {
                PrizeMoney = GetPrizeMoney(Prize,jackpot);
                if (Prize == Prize.MatchTwo)
                {
                    WonLD = true;
                }
            }

        }
        public static List<int> GenerateBalls(int numberOfBalls)
        {
            //Create an arry for numberOfBalls
            var balls = new int[numberOfBalls];

            //Add Ball to array, not not dup.
            for (int i = 0; i < numberOfBalls; i++)
            {
                bool validBall = false;

                do
                {
                    var rnd = new Random();
                    var ball = rnd.Next(1, 60);
                    if (!balls.Contains(ball))
                    {
                        balls[i] = ball;
                        validBall = true;
                    }
                    if (i == numberOfBalls)
                    {
                        validBall = true;
                    }

                } while (validBall == false);
            }
            return balls.ToList();
        }
        public Prize GetPrize(List<int> PlayBalls, List<int> GameResult)
        {
            //Creates a copy of the lists
            List<int> _playBalls = new List<int>(PlayBalls);
            List<int> _gameBalls = new List<int>(GameResult);

            //Removed the last ball in the list (bonus ball)
            _playBalls.RemoveAt(PlayBalls.Count-1);
            _gameBalls.RemoveAt(GameResult.Count-1);

            //Gets matched balls no including the bouns
            var matchedBallsNoBouns = GetBallMatches(_playBalls, _gameBalls);

            if (matchedBallsNoBouns == 1)
            {
                return Prize.None;
            }
            if (matchedBallsNoBouns == 2)
            {
                return Prize.MatchTwo;
            }
            if (matchedBallsNoBouns == 3)
            {
                return Prize.MatchThree;
            }
            if (matchedBallsNoBouns == 4)
            {
                return Prize.MatchFour;
            }
            if (matchedBallsNoBouns == 5 && CheckBounsBallMatch(PlayBalls, GameResult) == false)
            {
                return Prize.MatchFive;
            }
            if (matchedBallsNoBouns == 5 && CheckBounsBallMatch(PlayBalls, GameResult) == true)
            {
                return Prize.FivePlusBonus;
            }
            if (matchedBallsNoBouns == 6)
            {
                return Prize.Jackpot;
            }
            else
            {
                return Prize.None;
            }
        }
        public static int GetPrizeMoney(Prize prize, int jackpot)
        {

            switch (prize)
            {
                case Prize.Jackpot:
                    return jackpot;
                case Prize.FivePlusBonus:
                    return 1000000;
                case Prize.MatchFive:
                    return 1750;
                case Prize.MatchFour:
                    return 140;
                case Prize.MatchThree:
                    return 30;
                case Prize.MatchTwo:
                    return 0;
                case Prize.None:
                    return 0;
                default:
                    return 0;
            }
        }
        public static int GetBallMatches(List<int> PlayBalls, List<int> GameResult)
        {
            return PlayBalls.Count(GameResult.Contains);
        }
        public  bool IsAWin()
        {
            if (Prize != Prize.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  bool CheckBounsBallMatch(List<int> PlayBalls, List<int> GameResult)
        {
            var playerBounsBall = PlayBalls[6];
            var GameBounsBall = GameResult[6];

            if (playerBounsBall == GameBounsBall)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
    public enum Prize
    {
        Jackpot, FivePlusBonus,MatchFive, MatchFour, MatchThree, MatchTwo, None
    }

}
