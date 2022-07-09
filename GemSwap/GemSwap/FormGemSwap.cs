using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GemSwap
{
    public partial class FormGemSwap : Form
    {
        public FormGemSwap()
        {
            InitializeComponent();
        }
     
      
        SwipPicGame Game = null;

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Show();

            

            
        }

        private void AddDoublePanelToForm()
        {
            this.Controls.Remove(Game.Pnl);
            this.Controls.Add(Game.Pnl);
        }
        private void CreateGame()
        {
            this.doubleBufferedPanel1.Visible = false;
            if (Game != null)
            {
                this.Controls.Remove(Game.Pnl);
            } 
            Game = new SwipPicGame(10, 10);
            Game.ScoreChangeEvent+=new Score.ScoreChangeEventHandler(Game_ScoreChangeEvent);
            Game.GameOverEvent+=new SwipPicGame.GameOverEventHandler(Game_GameOverEvent);
            Game.Pnl.Top = 27;

         
            this.Controls.Add(Game.Pnl);
            this.progressBar1.Value = 0;
            timer1.Enabled = true;



        }
        private void Game_GameOverEvent()
        {
            MessageBox.Show("Game over");
        }
        private void Game_ScoreChangeEvent(int NewScore)
        {
            this.lblScore.Text = NewScore.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateGame();
      
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Game.SaveCurrentBoard();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Game.IsEndGame().ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.progressBar1.Value >= this.progressBar1.Maximum)
            {

                timer1.Enabled = false;
                Game.EndGameFromTimeOut();
                
                return;
            }
            this.progressBar1.Value++;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGame();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }
    }
}
