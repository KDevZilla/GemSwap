using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GemSwap
{
    public class Score
    {
        public delegate void ScoreChangeEventHandler(int NewScoreValue);
        public event ScoreChangeEventHandler  ScoreChangeEvent;
        private int _ScoreValue = 0;
        public int ScoreValue
        {
            get { return _ScoreValue; }
        }
        public void ClearScore()
        {
            _ScoreValue = 0;
        }
        public void AddScoreFromRemovedCell(int NoofCells, bool IsInFalling, int ChainLevel)
        {
            int iTempScore = 0;
            if (!IsInFalling)
            {
                iTempScore =(int) Math.Pow ( NoofCells , 3);
            }
            else
            {
                iTempScore =(int) Math.Pow ( NoofCells * ChainLevel  ,3);
            }
            _ScoreValue += iTempScore;
            if (ScoreChangeEvent != null)
            {
                ScoreChangeEvent(_ScoreValue);
            }
        }
    }
}
