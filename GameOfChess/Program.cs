using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfChess
{
    class program
    {
        static void Main(string[] args)
        {
            ChessGame chessGame = new ChessGame();
            chessGame.play();
        }

    }
    class ChessGame
    {

        int countGameMovments = 0;
        Tool[,] board = new Tool[8, 8];
        Location whiteKingsLocation = new Location(7, 4);
        Location blackKingsLocation = new Location(0, 4);
        int count_50_moves_without_kill = 0;
        Location[] toolOptions;
        Tool[] count__tools_while_no_killing_or_moving_pawn;
        string correnntBoardDescription;
        bool canMove;
        bool checkmate;
        bool draw;
        bool check;
        bool isWhiteTurn = true;
        string[] allBoardPositionsDescription = new string[50];
        int correntColumn;
        int correntRow;
        int destinationColumn;
        int destinationRow;

        string inputCorrentColumn = "Please enter the letter representing the column number where the tool you want to move is located\n";
        string inputCorrentRow = "Please enter a number representing the row  where the tool you want to move is located\n";
        string inputDestinationColumn = " Please enter the letter representing the column which you want to move to\n";
        string inputDestinationRow = "Please enter a number representing the row which you want to move to\n";
        public void play()
        {

            this.createBoard();
            this.printBoard();

            setAllToolsLocation();
            setAllToolsOption();

            while (true)
            {
                string whichTurn = isWhiteTurn ? "  **  WHITE TURN!!! \n" : "  ** BLACK TURN!!! \n";
                if (count_50_moves_without_kill % 50 == 0)
                    count__tools_while_no_killing_or_moving_pawn = getNumOfTools();

                Console.WriteLine(whichTurn);

                Console.WriteLine(inputCorrentColumn);
                correntColumn = Utils.ConvertColumnInput(Console.ReadLine());
                if (correntColumn >= 8 || correntColumn < 0)
                    continue;

                Console.WriteLine(inputCorrentRow);
                correntRow = Utils.ConvertRowInput(Console.ReadLine());
                if ((correntRow >= 8 || correntRow < 0) || board[correntRow, correntColumn] == null)
                    continue;

                Console.WriteLine(inputDestinationColumn);
                destinationColumn = Utils.ConvertColumnInput(Console.ReadLine());
                if (destinationColumn >= 8 || destinationColumn < 0)
                    continue;

                Console.WriteLine(inputDestinationRow);
                destinationRow = Utils.ConvertRowInput(Console.ReadLine());

                if (destinationRow >= 8 || destinationRow < 0)
                {
                    Console.WriteLine("Invalid move");
                    continue;
                }
                if ((board[correntRow, correntColumn].getColor() == "W" && !isWhiteTurn) ||
                    (board[correntRow, correntColumn].getColor() == "B" && isWhiteTurn))
                {
                    Console.WriteLine("Hey, it's not your turn. Invalid move");
                    continue;
                }

                Tool tool = getToolByIndex(correntRow, correntColumn);
                toolOptions = tool.getToolOptions();
                canMove = CanMove();
                if (!canMove)
                {
                    Console.WriteLine("Invalid move");
                    continue;
                }
                printBoard();

                isWhiteTurn = isWhiteTurn == true ? false : true;
                correnntBoardDescription = getBoardDescription();
                allBoardPositionsDescription[count_50_moves_without_kill] = correnntBoardDescription;
                count_50_moves_without_kill++;

                bool is_Checkmate_Or_Draw = is_Check_Checkmate_Draw_AfterAMove();
                if (is_Checkmate_Or_Draw)
                    break;
            }
            Console.ReadLine();
        }

        public bool is_Check_Checkmate_Draw_AfterAMove()
        {
            check = isCheck();
            if (check)
            {
                checkmate = isCheckmate();
                if (checkmate)
                {
                    Console.WriteLine(check ? "\n Checkmate, game over!!! \n" : "");
                    Console.ReadLine();
                    return true;
                }
                Console.WriteLine("\n Check! \n");
            }
            else
            {
                count__tools_while_no_killing_or_moving_pawn = getNumOfTools();
                draw = isDraw();
                if (draw)
                {
                    Console.WriteLine("\n DRAW,  game over!!! \n");
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }



        public Tool getToolByIndex(int correntRow, int correntColumn)
        {
            if (board[correntRow, correntColumn] is Pawn)
            {
                Pawn pawn = ((Pawn)board[correntRow, correntColumn]);
                return pawn;
            }
            if (board[correntRow, correntColumn] is King)
            {
                King king = ((King)board[correntRow, correntColumn]);
                return king;
            }
            if (board[correntRow, correntColumn] is Queen)
            {
                Queen queen = ((Queen)board[correntRow, correntColumn]);
                return queen;
            }
            if (board[correntRow, correntColumn] is Knight)
            {
                Knight knight = ((Knight)board[correntRow, correntColumn]);
                return knight;
            }
            if (board[correntRow, correntColumn] is Rook)
            {
                Rook rook = ((Rook)board[correntRow, correntColumn]);
                return rook;
            }
            if (board[correntRow, correntColumn] is Bishop)
            {
                Bishop bishop = ((Bishop)board[correntRow, correntColumn]);
                return bishop;
            }
            return null;
        }




        public bool isCheck()

        {
            Location[] indextoolOptions;
            Tool[] activTools;
            activTools = getNumOfTools();
            string color = isWhiteTurn ? "W" : "B";
            Location kingsLocation = getKingsLocation();
            bool isCheck = false;

            int x_pos = kingsLocation.getRow();
            int y_pos = kingsLocation.getColumn();
            for (int i = 0; i < activTools.Length; i++)
            {
                if (activTools[i] != null && activTools[i].getColor() != color)
                {
                    indextoolOptions = activTools[i].getToolOptions();
                    if (indextoolOptions != null)
                        for (int j = 0; j < indextoolOptions.Length; j++)
                        {
                            if ((indextoolOptions[j] != null) && (indextoolOptions[j].getRow() == x_pos &&
                        indextoolOptions[j].getColumn() == y_pos))
                            {
                                isCheck = true;
                                return isCheck;
                            }
                        }


                }

            }
            return false;
        }

        public void setKingsLocation(int row, int column)
        {

            if ((isWhiteTurn) && (board[row, column] != null) && (board[row, column].getName() == "WK"))
                this.whiteKingsLocation = new Location(row, column);

            else if ((!(isWhiteTurn)) && (board[row, column] != null) && (board[row, column].getName() == "BK"))
                this.blackKingsLocation = new Location(row, column);


        }
        public Location getKingsLocation()
        {
            if (isWhiteTurn)
                return whiteKingsLocation;

            else
                return blackKingsLocation;
        }

        public bool isDraw()
        {
            bool isDraw = false;

            isDraw = Draw_equal_tools();
            if (isDraw)
                return isDraw;

            isDraw = draw_3_seame_moves();
            if (isDraw)
                return isDraw;


            if (count_50_moves_without_kill % 50 == 0 && count_50_moves_without_kill > 0)
            {
                Tool[] CorrentActivTools = getNumOfTools();
                if (CorrentActivTools == count__tools_while_no_killing_or_moving_pawn)
                    return true;
            }

            isDraw = is_draw_pat();
            return isDraw;
        }
        public bool isCheckmate()
        {
            Location kingsLocation = getKingsLocation();
            bool Check;
            bool isCheckmate = true;
            int kx_pos = kingsLocation.getRow();
            int ky_pos = kingsLocation.getColumn();
            int ex_row_pos;
            int ex_column_pos;
            Tool[] activTools = getNumOfTools();
            for (int i = 0; i < activTools.Length; i++)
            {
                if ((activTools[i] != null) && (activTools[i].getColor() == board[kx_pos, ky_pos].getColor()))
                {
                    Location[] toolOptions = activTools[i].getToolOptions();
                    for (int index = 0; index < toolOptions.Length; index++)
                    {
                        if (toolOptions[index] != null)
                        {
                            int row = toolOptions[index].getRow();
                            int column = toolOptions[index].getColumn();
                            ex_row_pos = activTools[i].getToolLocation().getRow();
                            ex_column_pos = activTools[i].getToolLocation().getColumn();


                            Tool tool = null;
                            if (board[row, column] != null && board[row, column].getColor() != board[kx_pos, ky_pos].getColor())
                            {
                                tool = getToolByIndex(row, column);
                            }
                            board[row, column] = activTools[i];
                            board[ex_row_pos, ex_column_pos] = null;
                            activTools[i].setToolSLocation(row, column);

                            if (activTools[i] is King)
                                setKingsLocation(row, column);
                            setAllToolsOption();
                            Check = isCheck();

                            if (!Check)
                            {
                                board[ex_row_pos, ex_column_pos] = activTools[i];
                                board[row, column] = tool;
                                activTools[i].setToolSLocation(ex_row_pos, ex_column_pos);
                                if (activTools[i] is King)
                                    setKingsLocation(ex_row_pos, ex_column_pos);
                                setAllToolsOption();
                                isCheckmate = false;
                                return isCheckmate;
                            }
                            board[ex_row_pos, ex_column_pos] = activTools[i];
                            board[row, column] = tool;
                            activTools[i].setToolSLocation(ex_row_pos, ex_column_pos);
                            if (activTools[i] is King)
                            {
                                setKingsLocation(ex_row_pos, ex_column_pos);
                            }
                            setAllToolsOption();


                        }
                    }
                }
            }
            return isCheckmate;
        }
        public Tool[] getNumOfTools()
        {
            Tool[] activTools = new Tool[32];
            int index = 0;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != null)
                    {
                        activTools[index] = getToolByIndex(i, j);
                        index++;
                    }
                }
            }
            return activTools;
        }

        public string getBoardDescription()
        {
            string boardDescription = "";

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != null)
                    {
                        boardDescription += i + "" + j + " " + board[i, j].getName() + " ";
                    }
                }

            }
            return boardDescription;
        }
        public bool is_draw_pat()
        {
            int ex_row_pos;
            int ex_column_pos;
            Location[] indextoolOptions;
            bool check;
            string color = isWhiteTurn ? "W" : "B";


            Tool[] activTools = getNumOfTools();
            for (int i = 0; i < activTools.Length; i++)
            {

                if (activTools[i] != null && activTools[i].getColor() == color)
                {
                    indextoolOptions = activTools[i].getToolOptions();
                    if (indextoolOptions != null)
                    {
                        for (int index = 0; index < indextoolOptions.Length; index++)
                        {
                            if (indextoolOptions[index] != null)
                            {
                                int row = indextoolOptions[index].getRow();
                                int column = indextoolOptions[index].getColumn();
                                ex_row_pos = activTools[i].getToolLocation().getRow();
                                ex_column_pos = activTools[i].getToolLocation().getColumn();
                                Tool tool = null;
                                if (board[row, column] != null && board[row, column].getColor() != color)
                                {
                                    tool = getToolByIndex(row, column);
                                }
                                board[row, column] = activTools[i];
                                board[ex_row_pos, ex_column_pos] = null;
                                activTools[i].setToolSLocation(row, column);
                                if (activTools[i] is King)
                                    setKingsLocation(row, column);
                                setAllToolsOption();
                                check = isCheck();

                                if (!check)
                                {
                                    board[ex_row_pos, ex_column_pos] = getToolByIndex(row, column);
                                    board[row, column] = tool;
                                    activTools[i].setToolSLocation(ex_row_pos, ex_column_pos);
                                    if (activTools[i] is King)
                                        setKingsLocation(ex_row_pos, ex_column_pos);
                                    setAllToolsOption();

                                    return false;
                                }
                                board[ex_row_pos, ex_column_pos] = getToolByIndex(row, column);
                                board[row, column] = tool;
                                activTools[i].setToolSLocation(ex_row_pos, ex_column_pos);

                                if (activTools[i] is King)
                                    setKingsLocation(ex_row_pos, ex_column_pos);
                                setAllToolsOption();
                            }
                        }
                    }
                }
            }
            return true;
        }
        public bool draw_3_seame_moves()
        {
            if (count_50_moves_without_kill > 0)
            {
                int count_draw_3_moves = 0;

                foreach (string s in allBoardPositionsDescription)
                {
                    if (correnntBoardDescription == s)
                        count_draw_3_moves++;
                }
                if (count_draw_3_moves >= 3)
                    return true;
            }
            return false;
        }
        public bool Draw_equal_tools()
        {
            Tool[] countTools = getNumOfTools();
            if (countTools.Length < 4)
            {
                if (countTools.Contains(new Bishop("BB", "B", true || false)) && countTools.Contains(new Bishop("WB", "W", true || false)))
                    return true;
                if (countTools.Contains(new Bishop("BN", "B", true || false)) && countTools.Contains(new Bishop("WN", "W", true || false)))
                    return true;
            }
            if (countTools.Length < 2)
            {
                return true;
            }

            return false;
        }

        public bool CanMove()
        {

            bool check;
            Tool tool;
            tool = getToolByIndex(correntRow, correntColumn);
            for (int i = 0; i < toolOptions.Length; i++)
            {
                if (toolOptions[i] != null)
                {

                    int row = toolOptions[i].getRow();
                    int column = toolOptions[i].getColumn();
                    if (row == destinationRow && column == destinationColumn)
                    {

                        board[correntRow, correntColumn] = null;
                        board[destinationRow, destinationColumn] = tool;
                        if (tool is King)
                        {
                            setKingsLocation(destinationRow, destinationColumn);
                        }
                        tool.setToolSLocation(destinationRow, destinationColumn);
                        setAllToolsOption();
                        check = isCheck();
                        if (check)
                        {
                            Console.WriteLine("\n Check! \n");
                            board[correntRow, correntColumn] = tool;
                            board[destinationRow, destinationColumn] = null;
                            tool.setToolSLocation(correntRow, correntColumn);
                            setAllToolsOption();
                            if (tool is King)
                            {
                                setKingsLocation(correntRow, correntColumn);
                            }
                            return false;
                        }

                        setAllToolsOption();
                        countGameMovments++;

                        if (tool is Pawn)
                        {
                            bool isenPassent = i == 2 || i == 3;//En Passant
                            if (isenPassent)
                            {
                                board[correntRow, destinationColumn] = null;
                            }
                            count_50_moves_without_kill = 0;
                            ((Pawn)board[destinationRow, destinationColumn]).setCountPawnMovments();
                            ((Pawn)board[destinationRow, destinationColumn]).setNumberOfMoveInGame(countGameMovments);
                            crowningPawn();
                        }


                        tool.SetFirstMove(false);
                        return true;
                    }
                }
            }

            if (tool is King)
            {
                bool isCastling = castling();
                return isCastling;
            }
            return false;
        }
        public Tool[,] createBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (i == 2)
                    {
                        i = 5;
                    }
                    if (i == 0)
                    {
                        if (j == 0)
                            board[i, j] = new Rook("BR", "B", true);
                        if (j == 1)
                            board[i, j] = new Knight("BN", "B", true);
                        if (j == 2)
                            board[i, j] = new Bishop("BB", "B", true);
                        if (j == 3)
                            board[i, j] = new Queen("BQ", "B", true);
                        if (j == 4)
                            board[i, j] = new King("BK", "B", true);
                        if (j == 5)
                            board[i, j] = new Bishop("BB", "B", true);
                        if (j == 6)
                            board[i, j] = new Knight("BN", "B", true);
                        if (j == 7)
                            board[i, j] = new Rook("BR", "B", true);
                    }
                    else if (i == 1)
                    {
                        board[i, j] = new Pawn("BP", "B", true);
                    }
                    else if (i == 6)
                    {
                        board[i, j] = new Pawn("WP", "W", true);
                    }
                    else if (i == 7)
                    {
                        if (j == 0)
                            board[i, j] = new Rook("WR", "W", true);
                        if (j == 1)
                            board[i, j] = new Knight("WN", "W", true);
                        if (j == 2)
                            board[i, j] = new Bishop("WB", "W", true);
                        if (j == 3)
                            board[i, j] = new Queen("WQ", "W", true);
                        if (j == 4)
                            board[i, j] = new King("WK", "W", true);
                        if (j == 5)
                            board[i, j] = new Bishop("WB", "W", true);
                        if (j == 6)
                            board[i, j] = new Knight("WN", "W", true);
                        if (j == 7)
                            board[i, j] = new Rook("WR", "W", true);
                    }

                }
            }
            return board;
        }
        
        public bool castling()
        {
            bool check;
            check = isCheck();
            Tool king;
            Tool rook;
            if (destinationRow == correntRow && correntColumn == destinationColumn - 2 && !check)
            {
                king = getToolByIndex(correntRow, correntColumn);
                rook = getToolByIndex(correntRow, correntColumn + 3);
                if (king.isFirstMove() && rook is Rook &&
                   rook.isFirstMove() && (board[correntRow, correntColumn + 1] == null) && (board[correntRow, correntColumn + 2] == null))
                {
                    for (int i = 1; i >= 0; i--)
                    {
                        board[correntRow, correntColumn] = null;
                        board[destinationRow, destinationColumn - i] = king;
                        setKingsLocation(destinationRow, destinationColumn - i);

                        check = isCheck();
                        if (check)
                        {
                            board[correntRow, correntColumn] = king;
                            board[destinationRow, destinationColumn - i] = null;
                            setKingsLocation(correntRow, correntColumn);
                            Console.WriteLine("Invalid move Please try again!!!");
                            return false;
                        }
                        board[correntRow, correntColumn] = king;
                        board[destinationRow, destinationColumn - i] = null;
                        setKingsLocation(correntRow, correntColumn);
                    }

                    board[correntRow, correntColumn + 1] = rook;
                    board[correntRow, correntColumn + 3] = null;
                    board[destinationRow, destinationColumn] = king;
                    board[correntRow, correntColumn] = null;
                    rook.setToolSLocation(correntRow, correntColumn + 1);
                    king.setToolSLocation(destinationRow, destinationColumn);
                    rook.SetFirstMove(false);
                    king.SetFirstMove(false);
                    setKingsLocation(destinationRow, destinationColumn);
                    setAllToolsOption();
                    return true;
                }
            }
            else if (destinationRow == correntRow && correntColumn == destinationColumn + 2 && !check)
            {
                king = getToolByIndex(correntRow, correntColumn);
                rook = getToolByIndex(correntRow, correntColumn - 4);
                if (king.isFirstMove() && rook is Rook &&
                    rook.isFirstMove() && (board[correntRow, correntColumn - 1] == null) && (board[correntRow, correntColumn - 2] == null) && (board[correntRow, correntColumn - 3] == null))
                {
                    for (int i = 1; i >= 0; i--)
                    {
                        board[correntRow, correntColumn] = null;
                        board[destinationRow, destinationColumn + i] = king;
                        setKingsLocation(destinationRow, destinationColumn + i);
                        check = isCheck();
                        if (check)
                        {
                            board[correntRow, correntColumn] = king;
                            board[destinationRow, destinationColumn + i] = null;
                            setKingsLocation(correntRow, correntColumn);
                            Console.WriteLine("Invalid move Please try again!!!");
                            return false;
                        }
                        board[correntRow, correntColumn] = getToolByIndex(destinationRow, destinationColumn + i);
                        board[destinationRow, destinationColumn + i] = null;
                        setKingsLocation(correntRow, correntColumn);
                    }

                    board[correntRow, correntColumn - 1] = rook;
                    board[correntRow, correntColumn - 4] = null;
                    board[destinationRow, destinationColumn] = king;
                    board[correntRow, correntColumn] = null;

                    rook.setToolSLocation(correntRow, correntColumn + 1);
                    setKingsLocation(destinationRow, destinationColumn);
                    king.setToolSLocation(destinationRow, destinationColumn);
                    setAllToolsOption();
                    return true;
                }
            }
            return false;
        }
        public void EnPassant()
        {
            if (correntColumn != destinationColumn && board[destinationRow, destinationColumn] == null)//??? ??????            
                board[correntRow, destinationColumn] = null;
        }
        public void crowningPawn()
        {
            string color = isWhiteTurn ? "W" : "B";
            if (destinationRow == 0 && isWhiteTurn || destinationRow == 7 && !isWhiteTurn)
            {

                Console.WriteLine("Please enter a number Which represents the tool you want to crown");
                Console.WriteLine("1-Queen,  2-Rook,  3-bishop,  4-Knight");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1": board[destinationRow, destinationColumn] = new Queen(isWhiteTurn ? "WQ" : "BQ", color, false); break;
                    case "2": board[destinationRow, destinationColumn] = new Rook(isWhiteTurn ? "WR" : "BR", color, false); break;
                    case "3": board[destinationRow, destinationColumn] = new Bishop(isWhiteTurn ? "WB" : "BB", color, false); break;
                    case "4": board[destinationRow, destinationColumn] = new Knight(isWhiteTurn ? "WN" : "BN", color, false); break;
                    default:
                        board[destinationRow, destinationColumn] = new Queen(isWhiteTurn ? "WQ" : "BQ", color, false); break;
                }
                board[destinationRow, destinationColumn].setToolSLocation(destinationRow, destinationColumn);
            }
        }
        public void setAllToolsOption()
        {
            Tool[] allTools = getNumOfTools();
            for (int i = 0; i < allTools.Length; i++)
            {
                if (allTools[i] != null)
                {


                    allTools[i].setToolOptions(board, isWhiteTurn, countGameMovments, allTools[i].getToolLocation().getRow(),
                        allTools[i].getToolLocation().getColumn());
                }
            }
        }
        public void setAllToolsLocation()
        {
            Tool[] allTools = getNumOfTools();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != null)
                    {
                        board[i, j].setToolSLocation(i, j);
                    }
                }
            }


        }
        public void printBoard()
        {

            Console.WriteLine("     A    B    C    D    E    F    G    H");
            Console.WriteLine();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.Write(i + 1 + "   ");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (!(board[i, j] == null))
                        Console.Write(" " + board[i, j].getName() + "  ");
                    if (board[i, j] == null)
                        Console.Write(" EE  ");
                    if (j == 7)
                        Console.WriteLine();

                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

    }

    class Tool
    {
        protected Location[] toolOptions;
        string name;
        string color;
        bool firstMove;
        Location location;

        public Tool(string name, string color, bool firstMove)
        {
            this.name = name;
            this.color = color;
            this.firstMove = firstMove;

        }
        public string getColor()
        {
            return this.color;
        }
        public string getName()
        {
            return this.name;
        }
        public virtual void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            this.toolOptions = toolOptions;
        }


        public virtual Location[] getToolOptions()
        {
            return toolOptions;
        }
        public void setToolSLocation(int row, int column)
        {

            this.location = new Location(row, column);
        }
        public Location getToolLocation()
        {
            return location;
        }
        public bool isFirstMove()
        {
            return firstMove;
        }
        public void SetFirstMove(bool first)
        {
            this.firstMove = first;
        }
    }
    class King : Tool
    {

        public King(string name, string color, bool firstMove) : base(name, color, firstMove)
        {
        }

        public override void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            int options = 0;
            Location[] toolOptions = new Location[8];
            for (int x = row - 1; x <= row + 1; x++)
            {
                for (int y = column - 1; y <= column + 1; y++)
                {
                    if (((x >= 0 && x < 8) && (y >= 0 && y < 8)) && (x != row || y != column) && ((board[x, y] == null) || (board[x, y].getColor() != board[row, column].getColor())))
                    {
                        toolOptions[options] = new Location(x, y);
                        options++;
                    }
                }
            }
            this.toolOptions = toolOptions;
        }
        public override Location[] getToolOptions()
        {
            return toolOptions;
        }


    }
    class Queen : Tool
    {
        public Queen(string name, string color, bool firstMove) : base(name, color, firstMove)
        {
        }
        public override void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            int options = 0;
            Location[] queenOptions = new Location[32];
            Bishop bishop = new Bishop(getName(), getColor(), isFirstMove());
            Rook rook = new Rook(getName(), getColor(), isFirstMove());

            bishop.setToolOptions(board, iswhiteTurn, countGameMovments, row, column);
            Location[] toolOptions1 = bishop.getToolOptions();
            if (toolOptions1 != null)
            {
                foreach (Location move in toolOptions1)
                {
                    queenOptions[options] = move;
                    options++;
                }
            }
            rook.setToolOptions(board, iswhiteTurn, countGameMovments, row, column);
            Location[] toolOptions2 = rook.getToolOptions();
            if (toolOptions2 != null)
            {
                foreach (Location move in toolOptions2)
                {
                    queenOptions[options] = move;
                    options++;
                }
            }
            this.toolOptions = queenOptions;
        }
        public override Location[] getToolOptions()
        {

            return toolOptions;
        }



    }
    class Rook : Tool
    {
        public Rook(string name, string color, bool firstMove) : base(name, color, firstMove)
        {
        }
        public override void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            int options = 0;
            Location[] toolOptions = new Location[16];

            for (int r = row, c = column; c > 0; c--)
            {
                if ((c - 1 >= 0) && ((board[r, c - 1] == null) ||

                    (board[r, c - 1] != null && board[row, column].getColor() != board[r, c - 1].getColor())))
                {
                    if (c < column && board[r, c] != null)
                    {
                        break;
                    }
                    toolOptions[options] = new Location(r, c - 1);
                    options++;
                }
            }
            for (int r = row, c = column; c < 8; c++)
            {
                if ((c + 1 < 8) && ((board[r, c + 1] == null) || (board[r, c + 1] != null && board[row, column].getColor() != board[r, c + 1].getColor())))
                {
                    if (c > column && board[r, c] != null)
                    {
                        break;
                    }

                    toolOptions[options] = new Location(r, c + 1);
                    options++;
                }
            }
            for (int r = row, c = column; r >= 0; r--)
            {
                if ((r - 1 >= 0) && ((board[r - 1, c] == null) || (board[r - 1, c] != null && board[row, column].getColor() != board[r - 1, c].getColor())))
                {
                    if (r < row && board[r, c] != null)
                    {
                        break;
                    }
                    toolOptions[options] = new Location(r - 1, c);
                    options++;

                }
            }
            for (int r = row, c = column; r < 8; r++)
            {
                if ((r + 1 < 8) && ((board[r + 1, c] == null) || (board[r + 1, c] != null && board[row, column].getColor() != board[r + 1, c].getColor())))
                {
                    if (r > row && board[r, c] != null)
                    {
                        break;
                    }
                    toolOptions[options] = new Location(r + 1, c);
                    options++;
                }
            }
            this.toolOptions = toolOptions;
        }
        public override Location[] getToolOptions()
        {
            return toolOptions;
        }

    }
    class Bishop : Tool
    {
        public Bishop(string name, string color, bool firstMove) : base(name, color, firstMove)
        {
        }
        public override void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            int options = 0;
            Location[] toolOptions = new Location[16];

            for (int r = row, c = column; r >= 0 && c >= 0; r--, c--)
            {
                if ((r - 1 >= 0 && c - 1 >= 0) && ((board[r - 1, c - 1] == null ||
                    board[r - 1, c - 1].getColor() != board[row, column].getColor())))
                {
                    if (board[r, c] == null || c == column)
                    {
                        toolOptions[options] = new Location(r - 1, c - 1);
                        options++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int r = row, c = column; r < 8 && c < 8; r++, c++)
            {
                if ((r + 1 < 8 && c + 1 < 8) &&
                    ((board[r + 1, c + 1] == null || board[r + 1, c + 1].getColor() != board[row, column].getColor())))
                {
                    if ((board[r, c] == null || c == column))
                    {

                        toolOptions[options] = new Location(r + 1, c + 1);
                        options++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int r = row, c = column; r >= 0 && c < 8; r--, c++)
            {
                if ((r - 1 >= 0 && c + 1 < 8) && ((board[r - 1, c + 1] == null || board[r - 1, c + 1].getColor() != board[row, column].getColor())))
                {
                    if (board[r, c] == null || c == column)
                    {

                        toolOptions[options] = new Location(r - 1, c + 1);
                        options++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int r = row, c = column; r < 8 && c >= 0; r++, c--)
            {
                if ((r + 1 < 8 && c - 1 >= 0) && ((board[r + 1, c - 1] == null || board[r + 1, c - 1].getColor() != board[row, column].getColor())))
                {
                    if (board[r, c] == null || c == column)
                    {
                        toolOptions[options] = new Location(r + 1, c - 1);
                        options++;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            this.toolOptions = toolOptions;
        }
        public override Location[] getToolOptions()
        {
            return toolOptions;
        }




    }
    class Knight : Tool
    {
        public Knight(string name, string color, bool firstMove) : base(name, color, firstMove)
        {

        }
        public override void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            Location[] toolOptions = new Location[8];
            if ((row - 2 >= 0 && column + 1 < 8) && ((board[row - 2, column + 1] == null) || (board[row - 2, column + 1].getColor() != board[row, column].getColor())))
                toolOptions[0] = new Location(row - 2, column + 1);

            if ((row - 2 >= 0 && column - 1 >= 0) && ((board[row - 2, column - 1] == null) || (board[row - 2, column - 1].getColor() != board[row, column].getColor())))
                toolOptions[1] = new Location(row - 2, column - 1);

            if ((row + 2 < 8 && column - 1 >= 0) && ((board[row + 2, column - 1] == null) || (board[row + 2, column - 1].getColor() != board[row, column].getColor())))
                toolOptions[2] = new Location(row + 2, column - 1);

            if ((row + 2 < 8 && column + 1 < 8) && ((board[row + 2, column + 1] == null) || (board[row + 2, column + 1].getColor() != board[row, column].getColor())))
                toolOptions[3] = new Location(row + 2, column + 1);



            if ((row - 1 >= 0 && column + 2 < 8) && ((board[row - 1, column + 2] == null) || (board[row - 1, column + 2].getColor() != board[row, column].getColor())))
                toolOptions[4] = new Location(row - 1, column + 2);

            if ((row - 1 >= 0 && column - 2 >= 0) && ((board[row - 1, column - 2] == null) || (board[row - 1, column - 2].getColor() != board[row, column].getColor())))
                toolOptions[5] = new Location(row - 1, column - 2);

            if ((row + 1 < 8 && column - 2 >= 0) && ((board[row + 1, column - 2] == null) || (board[row + 1, column - 2].getColor() != board[row, column].getColor())))
                toolOptions[6] = new Location(row + 1, column - 2);

            if ((row + 1 < 8 && column + 2 < 8) && ((board[row + 1, column + 2] == null) || (board[row + 1, column + 2].getColor() != board[row, column].getColor())))
                toolOptions[7] = new Location(row + 1, column + 2);

            this.toolOptions = toolOptions;
        }
        public override Location[] getToolOptions()
        {
            return toolOptions;
        }



    }
    class Pawn : Tool
    {
        int countPawnMovments = 0;
        int numberOfMoveInGame = 0;
        public Pawn(string name, string color, bool firstMove) : base(name, color, firstMove)
        {
        }

        public void setNumberOfMoveInGame(int countGameMovments)
        {
            this.numberOfMoveInGame = countGameMovments;
        }
        public int getNumberOfMoveInGame()
        {
            return numberOfMoveInGame;
        }
        public void setCountPawnMovments()
        {
            this.countPawnMovments++;
        }
        public int getCountPawnMovments()
        {
            return countPawnMovments;
        }

        public override void setToolOptions(Tool[,] board, bool iswhiteTurn, int countGameMovments, int row, int column)
        {
            Location[] toolOptions = new Location[4];


            if (board[row, column].getColor() == "W")
            {
                if (row - 1 >= 0 && board[row - 1, column] == null)
                    toolOptions[0] = new Location(row - 1, column);

                if ((row - 1 >= 0 && row - 2 >= 0) && board[row - 1, column] == null && board[row - 2, column] == null && isFirstMove())
                    toolOptions[1] = new Location(row - 2, column);

                if ((((row - 1 >= 0 && column - 1 >= 0) && board[row - 1, column - 1] != null) && (board[row - 1, column - 1].getColor() == "B")) || (((column - 1 >= 0 && board[row, column - 1] != null) &&
                    board[row, column - 1].getName() == "BP" && row == 3) &&
                        (((Pawn)board[row, column - 1]).getCountPawnMovments() == 1 &&
                        ((Pawn)board[row, column - 1]).getNumberOfMoveInGame() == countGameMovments)))
                    toolOptions[2] = new Location(row - 1, column - 1);

                if (((row - 1 >= 0 && column + 1 < 8) && board[row - 1, column + 1] != null && (board[row - 1, column + 1].getColor() == "B")) || (((column + 1 < 8 && board[row, column + 1] != null) && board[row, column + 1].getName() == "BP" && row == 3) &&
                        (((Pawn)board[row, column + 1]).getCountPawnMovments() == 1 && ((Pawn)board[row, column + 1]).getNumberOfMoveInGame() == countGameMovments)))
                    toolOptions[3] = new Location(row - 1, column + 1);

                this.toolOptions = toolOptions;
            }


            else if (board[row, column].getColor() == "B")
            {
                if (row + 1 < 8 && board[row + 1, column] == null)
                    toolOptions[0] = new Location(row + 1, column);

                if ((row + 1 < 8 && row + 2 < 8) && (board[row + 1, column] == null && board[row + 2, column] == null && isFirstMove()))
                    toolOptions[1] = new Location(row + 2, column);

                if ((((row + 1 < 8 && column - 1 >= 0) && board[row + 1, column - 1] != null) &&
                    (board[row + 1, column - 1].getColor() == "W")) || (((column - 1 >= 0 && board[row, column - 1] != null) && board[row, column - 1].getName() == "WP" && row == 4) &&
                        (((Pawn)board[row, column - 1]).getCountPawnMovments() == 1 &&
                        ((Pawn)board[row, column - 1]).getNumberOfMoveInGame() == countGameMovments)))
                    toolOptions[2] = new Location(row + 1, column - 1);

                if ((((row + 1 < 8 && column + 1 < 8) && board[row + 1, column + 1] != null &&
                    (board[row + 1, column + 1].getColor() == "W")) || (((column + 1 < 8 && board[row, column + 1] != null) && board[row, column + 1].getName() == "WP" && row == 4) &&
                        (((Pawn)board[row, column + 1]).getCountPawnMovments() == 1 &&
                        ((Pawn)board[row, column + 1]).getNumberOfMoveInGame() == countGameMovments))))
                    toolOptions[3] = new Location(row + 1, column + 1);

                this.toolOptions = toolOptions;
            }

        }
    }
    class Location
    {
        int row;
        int colomn;

        public Location(int row, int colomn)
        {
            this.row = row;
            this.colomn = colomn;
        }
        public int getRow()
        {
            return this.row;
        }
        public int getColumn()
        {
            return this.colomn;
        }
    }
    class Utils
    {
        public static int ConvertColumnInput(string s)
        {
            switch (s)
            {
                case "A": case "a": return 0;
                case "B": case "b": return 1;
                case "C": case "c": return 2;
                case "D": case "d": return 3;
                case "E": case "e": return 4;
                case "F": case "f": return 5;
                case "G": case "g": return 6;
                case "H": case "h": return 7;

                default:
                    Console.WriteLine("The entered letter is invalid  ");
                    return 8;

            }
        }
        public static int ConvertRowInput(string s)
        {
            int row = 8;
            switch (s)
            {
                case "1": row = 0; break;
                case "2": row = 1; break;
                case "3": row = 2; break;
                case "4": row = 3; break;
                case "5": row = 4; break;
                case "6": row = 5; break;
                case "7": row = 6; break;
                case "8": row = 7; break;

                default:
                    Console.WriteLine("The entered number is invalid  ");
                    return row;
            }

            return row;
        }
    }

}



