using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XOXO
{
    public partial class Caro : Form
    {
        string turn = "X";
        int numberOfSquare = 20;
        int sizeSquare = 25;
        List<List<Button>> board = new List<List<Button>>();
        LinkedList<Point> moveTranscript = new LinkedList<Point>();
        bool saved = true;
        bool game = true; //when win game = false

        public Caro()
        {
            InitializeComponent();
            LoadBoard();
        }

        private void LoadBoard()
        {
            for (int column = 1; column <= numberOfSquare; column++)
            {
                board.Add(new List<Button>());
                for (int row = 1; row <= numberOfSquare; row++)
                {
                    board[column - 1].Add(new Button());
                    board[column - 1][row - 1].Location = new Point(column * sizeSquare, row * sizeSquare);
                    board[column - 1][row - 1].Width = sizeSquare;
                    board[column - 1][row - 1].Height = sizeSquare;
                    board[column - 1][row - 1].Margin = new Padding(0, 0, 0, 0);
                    this.Controls.Add(board[column - 1][row - 1]);
                    board[column - 1][row - 1].Click += button_click;
                }
            }
        }

        private void LoadBoard(LinkedList<Point> moveTranscript)
        {
            ChangeTurn("X");
            foreach (Point move in moveTranscript)
            {
                int x = move.X;
                int y = move.Y;
                board[x-1][y-1].Text = turn.ToString();
                ChangeTurn();
            }
        }

        private void ChangeTurn(string turn = "")
        {
            if (turn != "")
            {
                this.turn = turn;
            }
            else
            {
                if (this.turn == "X")
                    this.turn = "O";
                else this.turn = "X";
            }
        }

        private void button_click(object sender, EventArgs e)
        {
            if (game == true)
            {
                Button b = (Button)sender;
                if (b.Text != "")
                {
                    MessageBox.Show("Square is occupied.", "Caro");
                    return;
                }
                b.Text = turn.ToString();

                int column = b.Location.X / sizeSquare;
                int row = b.Location.Y / sizeSquare;
                moveTranscript.AddLast(new Point(column, row));

                if (CheckWinCon(column - 1, row - 1))
                {
                    if (turn.Equals("X"))
                        MessageBox.Show("Player 1 wins.", "Caro");
                    else
                        MessageBox.Show("Player 2 wins.", "Caro");
                    game = false;
                }
                ChangeTurn();
                saved = false;
            }
            else
            {
                MessageBox.Show("Game has ended. Create a new game if you want to continue playing.", "Caro");
            }
        }

        private bool CheckWinCon(int i, int j)
        {
            if (CheckUp(i, j)) return true;
            if (CheckDown(i, j)) return true;
            if (CheckLeft(i, j)) return true;
            if (CheckRight(i, j)) return true;
            if (CheckUpLeft(i, j)) return true;
            if (CheckUpRight(i, j)) return true;
            if (CheckDownLeft(i, j)) return true;
            if (CheckDownRight(i, j)) return true;

            if (CheckUp(i, j + 1)) return true;
            if (CheckDown(i, j - 1)) return true;
            if (CheckLeft(i + 1, j)) return true;
            if (CheckRight(i - 1, j)) return true;
            if (CheckUpLeft(i + 1, j + 1)) return true;
            if (CheckUpRight(i - 1, j + 1)) return true;
            if (CheckDownLeft(i + 1, j - 1)) return true;
            if (CheckDownRight(i - 1, j - 1)) return true;

            if (CheckUp(i, j + 2)) return true;
            if (CheckRight(i - 2, j)) return true;
            if (CheckUpLeft(i + 2, j + 2)) return true;
            if (CheckDownLeft(i + 2, j - 2)) return true;

            return false;
        }

        private bool CheckUpLeft(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i - a][j - a].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckDownLeft(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i - a][j + a].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckUpRight(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i + a][j - a].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckDownRight(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i + a][j + a].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckUp(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i][j - a].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckDown(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i][j + a].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckLeft(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i - a][j].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool CheckRight(int i, int j)
        {
            try
            {
                for (int a = 1; a <= 4; a++)
                    if (!board[i][j].Text.Equals(board[i + a][j].Text)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void undoCommand_Click(object sender, EventArgs e)
        {
            if (moveTranscript.Count == 0)
            {
                MessageBox.Show("Can't go back any further");
                return;
            }
            Point lastMove = moveTranscript.Last.Value;
            board[lastMove.X - 1][lastMove.Y - 1].Text = "";
            moveTranscript.RemoveLast();
            ChangeTurn();
            saved = false;
        }

        private void saveCommand_Click(object sender, EventArgs e)
        {
            if (moveTranscript.Count > 0)
            {
                SaveFileDialog f = new SaveFileDialog();
                f.ShowDialog();
                if (f.FileName != "")
                {
                    SaveTranscriptToFile(moveTranscript, f.FileName);
                }
            }
        }

        private void SaveTranscriptToFile(LinkedList<Point> moveTranscript, string fileName)
        {
            LinkedList<Point> tmp = new LinkedList<Point>(moveTranscript);
            StreamWriter sw = new StreamWriter(fileName);
            while (tmp.Count > 0)
            {
                sw.WriteLine(tmp.First.Value.X + " " + tmp.First.Value.Y);
                tmp.RemoveFirst();
            }
            sw.Close();
        }

        private void openCommand_Click(object sender, EventArgs e)
        {
            if (saved != true)
            {
                DialogResult result = MessageBox.Show("Do you want to save your game?", "Caro", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes) 
                    saveCommand_Click(sender, e);
            }
            OpenFileDialog f = new OpenFileDialog();
            f.ShowDialog();
            if (f.FileName != "")
            {
                OpenTranscriptFromFile(moveTranscript, f.FileName);
                LoadBoard(moveTranscript);
            }
        }

        private void OpenTranscriptFromFile(LinkedList<Point> moveTranscript, string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                string point = sr.ReadLine();
                string[] value = point.Split(' ');
                int x = int.Parse(value[0]);
                int y = int.Parse(value[1]);
                moveTranscript.AddLast(new Point(x, y));
            }
            sr.Close();
        }

        private void newCommand_Click(object sender, EventArgs e)
        {
            if (saved != true)
            {
                DialogResult result = MessageBox.Show("Do you want to save your game?", "Caro", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                    saveCommand_Click(sender, e);
            }
            for (int i = 0; i < numberOfSquare; i++)
            {
                for (int j = 0; j < numberOfSquare; j++)
                {
                    board[i][j].Text = "";
                }
            }
            ChangeTurn("X");
            moveTranscript.Clear();
            saved = true;
        }
    }
}
