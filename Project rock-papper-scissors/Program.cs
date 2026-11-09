using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_1_Stone_Paper_Scissor
{
    internal class Program
    {
        enum enWinner { player = 1, computer = 2, draw = 3 };

        enum enChoice { stone = 1, papper = 2, scissors = 3 };

        struct stGameInfo
        {
            public short numberOfTheRounds;
            public short playerWonTimes;
            public short computerWonTimes;
            public short DrawTimes;
            public enWinner finalWinner;
        }

        struct stRoundInfo
        {
            public short numberOfTheRounds;
            public enChoice playerCoice;
            public enChoice computerChoice;
            public enWinner roundWinner;
            public string winnerName;
        }

        static void ResetScreen()
        {
            Console.ResetColor();
            Console.Clear();
        }

        static short ReadHowManyRounds()
        {
            short howManyRounds;

            do
            {
                Console.Write("How Many Rounds Do you Want To Play (1 to 10) ? ");
                howManyRounds = short.Parse(Console.ReadLine());
            }
            while (howManyRounds < 1 || howManyRounds > 10);

            return howManyRounds;
        }

        static enChoice PlayerChoice()
        {
            short playerChoice;

            do
            {
                Console.Write("\nYour Choice [1]Stone, [2]Papper, [3]Scissors ? ");
                playerChoice = short.Parse(Console.ReadLine());
            }
            while (playerChoice < 1 || playerChoice > 3);

            return (enChoice)playerChoice;
        }

        static Random rnd = new Random();

        static int RandomNumber(short from, short to)
        {
            int randomNumber;

            randomNumber = rnd.Next(from, to + 1);

            return randomNumber;
        }

        static enChoice ComputerChoice()
        {
            return (enChoice)RandomNumber(1, 3);
        }

        static enWinner WhoWinInTheRound(stRoundInfo roundInfo)
        {
            if (roundInfo.computerChoice == roundInfo.playerCoice)
                return enWinner.draw;

            switch (roundInfo.playerCoice)
            {
                case enChoice.stone:
                    if (roundInfo.computerChoice == enChoice.papper)
                        return enWinner.computer;
                    break;

                case enChoice.papper:
                    if (roundInfo.computerChoice == enChoice.scissors)
                        return enWinner.computer;
                    break;

                case enChoice.scissors:
                    if (roundInfo.computerChoice == enChoice.stone)
                        return enWinner.computer;
                    break;
            }
            return enWinner.player;
        }

        static string Tabs(short numberOfTabs)
        {
            string tab = "";

            for (int i = 0; i < numberOfTabs; i++)
            {
                tab += "\t";
            }
            return tab;
        }

        static void ShowColorInScreen(stRoundInfo roundInfo)
        {
            if (roundInfo.roundWinner == enWinner.player)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
            }

            else if (roundInfo.roundWinner == enWinner.computer)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\a");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Clear();

        }

        static void PrintRoundResult(stRoundInfo roundInfo)
        {
            ShowColorInScreen(roundInfo);

            Console.WriteLine("\n-----------Round[" + roundInfo.numberOfTheRounds + "]-------------");
            Console.WriteLine("Player Choice: " + ChoiceName(roundInfo.playerCoice));
            Console.WriteLine("Computer Choice: " + ChoiceName(roundInfo.computerChoice));
            Console.WriteLine("Round Winner: [" + WinnerName(roundInfo.roundWinner) + "]");
            Console.WriteLine("--------------------------------");

        }

        static string ChoiceName(enChoice choice)
        {
            string[] arrStringName = { "Stone", "Papper", "Scissors" };

            return arrStringName[(short)choice - 1];
        }

        static string WinnerName(enWinner winner)
        {
            string[] arrStringName = { "Player", "Computer", "Draw" };

            return arrStringName[(short)winner - 1];
        }

        static enWinner WhoWinTheGame(stGameInfo gameInfo)
        {
            if (gameInfo.playerWonTimes > gameInfo.computerWonTimes)
                return enWinner.player;

            else if (gameInfo.computerWonTimes > gameInfo.playerWonTimes)
                return enWinner.computer;

            else
                return enWinner.draw;
        }

        static stGameInfo FillGameInfo(short playerWonTimes, short computerWonTimes, short drawTimes, short howManyRounds)
        {
            stGameInfo gameInfo = new stGameInfo();

            gameInfo.numberOfTheRounds = howManyRounds;
            gameInfo.playerWonTimes = playerWonTimes;
            gameInfo.computerWonTimes = computerWonTimes;
            gameInfo.DrawTimes = drawTimes;
            gameInfo.finalWinner = WhoWinTheGame(gameInfo);

            return gameInfo;
        }

        static stGameInfo StartGame(short howManyRounds)
        {
            stRoundInfo roundInfo = new stRoundInfo();

            short playerWonTimes = 0, computerWonTimes = 0, drawTimes = 0;

            for (short gameRound = 1; gameRound <= howManyRounds; gameRound++)
            {
                Console.WriteLine("\nRound [" + gameRound + "] begins: ");

                roundInfo.numberOfTheRounds = gameRound;
                roundInfo.playerCoice = PlayerChoice();
                roundInfo.computerChoice = ComputerChoice();
                roundInfo.roundWinner = WhoWinInTheRound(roundInfo);
                roundInfo.winnerName = WinnerName(roundInfo.roundWinner);

                PrintRoundResult(roundInfo);

                if (roundInfo.roundWinner == enWinner.player)
                    playerWonTimes++;

                else if (roundInfo.roundWinner == enWinner.computer)
                    computerWonTimes++;

                else
                    drawTimes++;
            }

            return FillGameInfo(playerWonTimes, computerWonTimes, drawTimes, howManyRounds);
        }

        static void PrintGameOver()
        {
            Console.WriteLine("\n" + Tabs(3) + "-----------------------------------------\n");
            Console.WriteLine(Tabs(3) + "              +++Game Over+++\n             ");
            Console.WriteLine(Tabs(3) + "-----------------------------------------");
        }

        static void PrintGameInfo(stGameInfo gameInfo)
        {
            Console.WriteLine("\n" + Tabs(3) + "-------------[ Game Result ]-------------");
            Console.WriteLine(Tabs(3) + "Number Of The Round: " + gameInfo.numberOfTheRounds);
            Console.WriteLine(Tabs(3) + "Player Won Times: " + gameInfo.playerWonTimes);
            Console.WriteLine(Tabs(3) + "Player Computer Times: " + gameInfo.computerWonTimes);
            Console.WriteLine(Tabs(3) + "Draw Times: " + gameInfo.DrawTimes);
            Console.WriteLine(Tabs(3) + "Final Winner: [" + WinnerName(WhoWinTheGame(gameInfo)) + "]");
            Console.WriteLine(Tabs(3) + "-----------------------------------------");
        }

        static void PlayGame()
        {
            char playAgain;

            do
            {
                ResetScreen();

                stGameInfo gameInfo = StartGame(ReadHowManyRounds());

                PrintGameOver();

                PrintGameInfo(gameInfo);

                Console.Write("\n" + Tabs(3) + "Do You Want Play Again [y]:Yes, [n]No ? ");
                playAgain = char.Parse(Console.ReadLine());
            }
            while (playAgain == 'y' || playAgain == 'Y');
        }

        static void Main(string[] args)
        {
            PlayGame();
        }
    }
}

