using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _7X7
{
    public partial class Form1 : Form
    {
        int[,] num = new int[7, 7];
        Button[,] cell = new Button[7, 7];
        Color[] cellcolor = new Color[5] { Color.Red, Color.Gold, Color.GreenYellow, Color.DeepSkyBlue, Color.BlueViolet };
        Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        string tempcolor = null;
        List<Button> temp_list = new List<Button>();
        List<Button> remove_list = new List<Button>();
        bool erase = false;
        int score = 0;

        public Form1()
        {
            InitializeComponent();
            CreateField();
            AddCell(3); // новая игра            
        }

        private void CreateField()
        {
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    cell[i, j] = new Button();
                    cell[i, j].Size = new Size(50, 50);
                    cell[i, j].BackColor = Color.White;
                    cell[i, j].FlatStyle = FlatStyle.Flat;
                    cell[i, j].FlatAppearance.BorderSize = 2;
                    cell[i, j].FlatAppearance.BorderColor = Color.DarkGray;
                    cell[i, j].Top = 75 + j * 51;
                    cell[i, j].Left = 25 + i * 51;
                    this.Controls.Add(cell[i, j]);
                    this.cell[i, j].Click += new System.EventHandler(this.cell_Click);
                }
            }
        }

        private void cell_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            SelectCell(b);

            if (b.BackColor != Color.White)
            {
                temp_list.Clear();
                temp_list.Add(b);
                tempcolor = b.BackColor.Name;
            }
            if (tempcolor != null && b.BackColor == Color.White)
            {
                b.BackColor = Color.FromName(tempcolor);
                tempcolor = null;
                temp_list[0].BackColor = Color.White;
                CheckBlocks();

                if (erase)
                    erase = false;
                else
                    AddCell(3);
            }
        }

        private void ColorRandomCell()
        {
            int x = rnd.Next(7);
            int y = rnd.Next(7);
            int c = rnd.Next(0, cellcolor.Length);
            if (cell[x, y].BackColor == Color.White)
                cell[x, y].BackColor = cellcolor[c];
            else
                ColorRandomCell();
        }

        private void AddCell(int counter)
        {
            while (counter > 0)
            {
                if (GameOver())
                {
                    MessageBox.Show("Your Score: " + score.ToString());
                    break;
                }

                ColorRandomCell();
                counter--;
            }
            CheckBlocks();
        }

        private void SelectCell(Button button)
        {
            foreach (Button cell in this.Controls.OfType<Button>())
            {
                cell.FlatAppearance.BorderColor = Color.DarkGray;
                cell.FlatAppearance.BorderSize = 2;
            }
            if (button.BackColor != Color.White && button.Focused)
            {
                button.FlatAppearance.BorderSize = 4;
                button.FlatAppearance.BorderColor = Color.Black;
            }
        }

        private void CheckBlocks()
        {
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (cell[i, j].BackColor != Color.White)
                    {
                        //проверка по вертикали

                        if (j < 4 && cell[i, j].BackColor == cell[i, j + 1].BackColor
                            && cell[i, j + 1].BackColor == cell[i, j + 2].BackColor
                            && cell[i, j + 2].BackColor == cell[i, j + 3].BackColor)
                        {
                            remove_list.Add(cell[i, j]);
                            remove_list.Add(cell[i, j + 1]);
                            remove_list.Add(cell[i, j + 2]);
                            remove_list.Add(cell[i, j + 3]);
                        }

                        //проверка по горизонтали

                        if (i < 4 && cell[i, j].BackColor == cell[i + 1, j].BackColor
                          && cell[i + 1, j].BackColor == cell[i + 2, j].BackColor
                          && cell[i + 2, j].BackColor == cell[i + 3, j].BackColor)
                        {
                            remove_list.Add(cell[i, j]);
                            remove_list.Add(cell[i + 1, j]);
                            remove_list.Add(cell[i + 2, j]);
                            remove_list.Add(cell[i + 3, j]);
                        }

                        //по диагонали сверху вниз

                        if (i < 4 && j < 4 && cell[i, j].BackColor == cell[i + 1, j + 1].BackColor
                         && cell[i + 1, j + 1].BackColor == cell[i + 2, j + 2].BackColor
                         && cell[i + 2, j + 2].BackColor == cell[i + 3, j + 3].BackColor)
                        {
                            remove_list.Add(cell[i, j]);
                            remove_list.Add(cell[i + 1, j + 1]);
                            remove_list.Add(cell[i + 2, j + 2]);
                            remove_list.Add(cell[i + 3, j + 3]);
                        }

                        //по диагонали снизу вверх

                        if (i > 2 && j < 4 && cell[i, j].BackColor == cell[i - 1, j + 1].BackColor
                         && cell[i - 1, j + 1].BackColor == cell[i - 2, j + 2].BackColor
                         && cell[i - 2, j + 2].BackColor == cell[i - 3, j + 3].BackColor)
                        {
                            remove_list.Add(cell[i, j]);
                            remove_list.Add(cell[i - 1, j + 1]);
                            remove_list.Add(cell[i - 2, j + 2]);
                            remove_list.Add(cell[i - 3, j + 3]);
                        }
                    }
                }
            RemoveBlocks();
        }

        private void RemoveBlocks()
        {
            if (remove_list.Count != 0)
            {
                foreach (Button b in remove_list)
                    b.BackColor = Color.White;
                UpdateScore();
                erase = true;
                System.Media.SystemSounds.Exclamation.Play();
                remove_list.Clear();
            }
        }

        private bool GameOver()
        {
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                    if (cell[i, j].BackColor == Color.White)
                        return false;
            return true;
        }

        private void UpdateScore()
        {
            score += remove_list.Distinct().Count() * 10;
            labelScore.Text = score.ToString();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            score = 0;
            labelScore.Text = score.ToString();
            tempcolor = null;
            erase = false;
            temp_list.Clear();
            remove_list.Clear();
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    cell[i, j].BackColor = Color.White;
                }
            AddCell(3);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("arthur56betw1n, 2018");
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Make horizontal, vertical or diagonal rows of the same color by dragging pieces across the board and get points.");
        }
    }
}
