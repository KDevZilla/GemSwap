using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GemSwap
{
    class CellDecoratorForRaining
    {
          public System.Drawing.Bitmap B;
        public bool IsFalling = false;

        private Cell _Cell = null;
        public Cell Cell
        {
            get { return _Cell; }
        }
        private Cell _OldCell = null;
        public Cell OldCell
        {
            get { return _OldCell; }
        }
        public CellDecoratorForRaining(Cell pCell, Cell pOldCell)
        {
            _Cell = pCell;
            _OldCell = pOldCell;
        }
        public void setOldCellValue()
        {
            _OldCell.Value = _Cell.Value;
        }

        public bool  HasReachedGoal
        {
            get
            {
                return RenderPosition.X == GoalPosition.X &&
                    RenderPosition.Y == GoalPosition.Y;
            }
        }
        public System.Drawing.Point RenderPosition = new System.Drawing.Point();
        public System.Drawing.Point GoalPosition = new System.Drawing.Point();

    }
}
