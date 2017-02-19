// Skeleton Program code for the AQA COMP1 Summer 2016 examination
// this code whould be used in conjunction with the Preliminary Material
// written by the AQA COMP1 Programmer Team
// developed in the Visual Studio 2012 programming environment

using System;
using System.IO;
namespace AQAReverse
{
    class Program
    {
        static void Main(string[] args)//exception handling added.
        {
            char Choice = ' ';
            string PlayerName;
            int BoardSize;
            BoardSize = 6;
            bool UserInputFailed = false;//
            PlayerName = "Human, ";
            string FilePath = @"C:\Users\amand_000\Documents\Visual Studio 2015\Projects\Save.txt";
            do
            {
                DisplayMenu();
                try//
                {
                    Choice = GetMenuChoice(PlayerName);
                    
                }
                catch
                {
                    UserInputFailed = true;
                }//

                switch (Choice)
                {
                    case 'p':
                        PlayGame(PlayerName, BoardSize, FilePath);
                        break;
                    case 'e':
                        PlayerName = GetPlayersName();
                        break;
                    case 'c':
                        BoardSize = ChangeBoardSize();
                        break;
                    case 's':
                        Console.WriteLine("Enter save file path:");
                        FilePath = Console.ReadLine();
                        break;

                }
            } while (Choice != 'q' || !UserInputFailed);//
        }

        static void SetUpGameBoard(char[,] Board, int BoardSize)
        {
            for (int Row = 1; Row <= BoardSize; Row++)
            {
                for (int Column = 1; Column <= BoardSize; Column++)
                {
                    if (Row == (BoardSize + 1) / 2 && Column == (BoardSize + 1) / 2 + 1 || Column == (BoardSize + 1) / 2 && Row == (BoardSize + 1) / 2 + 1)
                    {
                        Board[Row, Column] = 'C';
                    }
                    else if (Row == (BoardSize + 1) / 2 + 1 && Column == (BoardSize + 1) / 2 + 1 || Column == (BoardSize + 1) / 2 && Row == (BoardSize + 1) / 2)
                    {
                        Board[Row, Column] = 'H';
                    }
                    else
                    {
                        Board[Row, Column] = ' ';
                    }
                }
            }
        }

        static int ChangeBoardSize()//
        {
            int BoardSize = 0;
            bool UserInputFail = false;//
            
            do
            {
                Console.WriteLine("Enter a board size (between 4 and 9):");
                try//
                {
                    BoardSize = Convert.ToInt32(Console.ReadLine());
                }
                catch//
                {
                    Console.WriteLine("Incorrect Value Board size set to default (6).");
                    UserInputFail = true;
                }
            } while (BoardSize < 4 || BoardSize > 9 && !UserInputFail);//
            return BoardSize;
            
        }

        static int GetHumanPlayerMove(string PlayerName, int BoardSize, char[,] Board, string FilePath)//exception handling added.
        {
            bool UserIncompetent = false;//
            int Coordinates = 0;
            int Max = 66;
            Console.WriteLine("{0}Enter the coordinates of the square where you want to place your piece:", PlayerName);

            if(BoardSize == 4)//
            {
                Max = 44;
            }
            else if(BoardSize == 5)
            {
                Max = 55;
            }
            else if (BoardSize == 6)
            {
                Max = 66;
            }
            else if (BoardSize == 7)
            {
                Max = 77;
            }
            else if (BoardSize == 8)
            {
                Max = 88;
            }
            else if (BoardSize == 9)
            {
                Max = 99;
            }//

            while (!UserIncompetent)//
            {
                try
                {
                    Coordinates = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Incorrect Input. Input only Row and Column with no spaces.");
                    Console.WriteLine(BoardSize);
                    UserIncompetent = true;
                }
                if (Coordinates > Max || Coordinates < 11)//
                {
                    Console.WriteLine("Coordinates out of range, must be between 1-{0}.", Max / 10);
                    UserIncompetent = true;
                }
                if(Coordinates == 00)
                {
                    Console.WriteLine("Game saved");
                    SaveGame(Board, BoardSize, FilePath);
                }
                else//
                {
                    return Coordinates;
                }
            }//
            return 0;
        }

        static int GetComputerPlayerMove(int BoardSize)
        {
            Random RandomNo = new Random();
            return RandomNo.Next(1, BoardSize + 1) * 10 + RandomNo.Next(1, BoardSize + 1);
        }

        static bool GameOver(char[,] Board, int BoardSize)
        {
            for (int Row = 1; Row <= BoardSize; Row++)
            {
                for (int Column = 1; Column <= BoardSize; Column++)
                {
                    if (Board[Row, Column] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static string GetPlayersName()
        {
            string PlayerName;
            Console.WriteLine("What is your name?");
            PlayerName = Console.ReadLine();
            PlayerName = PlayerName + ", ";//Grammer is important. concatination
            return PlayerName;
        }

        static bool CheckIfMoveIsValid(char[,] Board, int Move)
        {
            int Row;
            int Column;
            bool MoveIsValid;
            Row = Move % 10;
            Column = Move / 10;
            MoveIsValid = false;
            if (Board[Row, Column] == ' ')
            {
                MoveIsValid = true;
            }

            return MoveIsValid;
        }

        static int GetPlayerScore(char[,] Board, int BoardSize, char Piece)
        {
            int Score;
            Score = 0;
            for (int Row = 1; Row <= BoardSize; Row++)
            {
                for (int Column = 1; Column <= BoardSize; Column++)
                {
                    if (Board[Row, Column] == Piece)
                    {
                        Score = Score + 1;
                    }
                }
            }
            return Score;
        }

        static bool CheckIfThereArePiecesToFlip(char[,] Board, int BoardSize, int StartRow, int StartColumn, int RowDirection, int ColumnDirection)
        {
            int RowCount;
            int ColumnCount;
            bool FlipStillPossible;
            bool FlipFound;
            bool OpponentPieceFound;
            RowCount = StartRow + RowDirection;
            ColumnCount = StartColumn + ColumnDirection;
            FlipStillPossible = true;
            FlipFound = false;
            OpponentPieceFound = false;
            while (RowCount <= BoardSize && RowCount >= 1 && ColumnCount >= 1 && ColumnCount <= BoardSize && FlipStillPossible && !FlipFound)
            {
                if (Board[RowCount, ColumnCount] == ' ')
                {
                    FlipStillPossible = false;
                }
                else if (Board[RowCount, ColumnCount] != Board[StartRow, StartColumn])
                {
                    OpponentPieceFound = true;
                }
                else if (Board[RowCount, ColumnCount] == Board[StartRow, StartColumn] && !OpponentPieceFound)
                {
                    FlipStillPossible = false;
                }
                else
                {
                    FlipFound = true;
                }
                RowCount = RowCount + RowDirection;
                ColumnCount = ColumnCount + ColumnDirection;
            }
            return FlipFound;
        }

        static void FlipOpponentPiecesInOneDirection(char[,] Board, int BoardSize, int StartRow, int StartColumn, int RowDirection, int ColumnDirection)
        {
            int RowCount;
            int ColumnCount;
            bool FlipFound;
            FlipFound = CheckIfThereArePiecesToFlip(Board, BoardSize, StartRow, StartColumn, RowDirection, ColumnDirection);
            if (FlipFound)
            {
                RowCount = StartRow + RowDirection;
                ColumnCount = StartColumn + ColumnDirection;
                while (Board[RowCount, ColumnCount] != ' ' && Board[RowCount, ColumnCount] != Board[StartRow, StartColumn])
                {
                    if (Board[RowCount, ColumnCount] == 'H')
                    {
                        Board[RowCount, ColumnCount] = 'C';
                    }
                    else
                    {
                        Board[RowCount, ColumnCount] = 'H';
                    }
                    RowCount = RowCount + RowDirection;
                    ColumnCount = ColumnCount + ColumnDirection;
                }
            }
        }

        static void MakeMove(char[,] Board, int BoardSize, int Move, bool HumanPlayersTurn)
        {
            int Row;
            int Column;
            Row = Move % 10;//takes the second number only. as MOD anything by 10 and second No. is result.
            Column = Move / 10;//relies on the principle that c# truncates integers therefore takes first number.

            if (HumanPlayersTurn)
            {
                Board[Row, Column] = 'H';
            }
            else
            {
                Board[Row, Column] = 'C';
            }
            FlipOpponentPiecesInOneDirection(Board, BoardSize, Row, Column, 1, 0);
            FlipOpponentPiecesInOneDirection(Board, BoardSize, Row, Column, -1, 0);
            FlipOpponentPiecesInOneDirection(Board, BoardSize, Row, Column, 0, 1);
            FlipOpponentPiecesInOneDirection(Board, BoardSize, Row, Column, 0, -1);
        }

        static void PrintLine(int BoardSize)
        {
            Console.Write("   ");
            for (int Count = 1; Count <= BoardSize * 2 - 1; Count++)
            {
                Console.Write("_");
            }
            Console.WriteLine();
        }

        static void DisplayGameBoard(char[,] Board, int BoardSize)
        {
            Console.WriteLine();
            Console.Write("  ");
            for (int Column = 1; Column <= BoardSize; Column++)
            {
                Console.Write(" ");
                Console.Write(Column);
            }
            Console.WriteLine();
            PrintLine(BoardSize);
            for (int Row = 1; Row <= BoardSize; Row++)
            {
                Console.Write(Row);
                Console.Write(" ");
                for (int Column = 1; Column <= BoardSize; Column++)
                {
                    Console.Write("|");
                    Console.Write(Board[Row, Column]);
                }
                Console.WriteLine("|");
                PrintLine(BoardSize);
                Console.WriteLine();
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("To save the game enter 00 when it is your turn.");
            Console.WriteLine("(P)lay game");
            Console.WriteLine("(E)nter name");
            Console.WriteLine("(C)hange board size");
            Console.WriteLine("(Q)uit");
            Console.WriteLine();
        }

        static char GetMenuChoice(string PlayerName)
        {
            char Choice;
            Console.Write("{0}Enter the letter of your chosen option (Lowercase):", PlayerName);
            Choice = Convert.ToChar(Console.ReadLine().ToLower());//
            return Choice;
        }

        static void PlayGame(string PlayerName, int BoardSize, string FilePath)
        {
            char[,] Board = new char[BoardSize + 1, BoardSize + 1];
            bool HumanPlayersTurn;
            int Move = 0;
            int HumanPlayerScore;
            int ComputerPlayerScore;
            bool MoveIsValid;
            SetUpGameBoard(Board, BoardSize);
            HumanPlayersTurn = false;
            do
            {
                HumanPlayersTurn = !HumanPlayersTurn;
                DisplayGameBoard(Board, BoardSize);
                MoveIsValid = false;
                do
                {
                    if (HumanPlayersTurn)
                    {
                        Move = GetHumanPlayerMove(PlayerName, BoardSize, Board, FilePath);
                    }
                    else
                    {
                        Move = GetComputerPlayerMove(BoardSize);
                    }
                    MoveIsValid = CheckIfMoveIsValid(Board, Move);
                } while (!MoveIsValid);
                if (!HumanPlayersTurn)
                {
                    Console.WriteLine("Press the Enter key and the computer will make its move");
                    Console.ReadLine();
                }
                MakeMove(Board, BoardSize, Move, HumanPlayersTurn);
            } while (!GameOver(Board, BoardSize));
            DisplayGameBoard(Board, BoardSize);
            HumanPlayerScore = GetPlayerScore(Board, BoardSize, 'H');
            ComputerPlayerScore = GetPlayerScore(Board, BoardSize, 'C');
            if (HumanPlayerScore > ComputerPlayerScore)
            {
                Console.WriteLine("Well done, " + PlayerName + ", you have won the game!");
            }
            else if (HumanPlayerScore == ComputerPlayerScore)
            {
                Console.WriteLine("That was a draw!");
            }
            else
            {
                Console.WriteLine("The computer has won the game!");
            }
            Console.WriteLine();
        }

        static void SaveGame(char[,] Board, int BoardSize, string FilePath)
        {
            string SavePath = FilePath;
         
            StreamWriter SaveFile = new StreamWriter(SavePath);

            for (int Row = 1; Row < BoardSize; Row++)
            {
                for (int Column = 1; Column < BoardSize; Column++)
                {
                    SaveFile.Write(Board[Row, Column]);
                }
                SaveFile.Write("\n");
            }
            SaveFile.Flush();
            SaveFile.Close();
        }
    }
}