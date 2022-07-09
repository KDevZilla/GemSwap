using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GemSwap
{
    public class CellDecorator
    {
        public System.Drawing.Bitmap B;
        public bool IsFalling = false;

        private Cell _Cell = null;
        public Cell Cell
        {
            get { return _Cell; }
        }
        public CellDecorator(Cell pCell)
        {
            _Cell = pCell;
        }
        public System.Drawing.Point RenderPosition = new System.Drawing.Point();

    }
}
