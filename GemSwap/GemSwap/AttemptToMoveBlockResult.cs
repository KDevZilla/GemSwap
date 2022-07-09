using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace GemSwap
{
    public class AttemptToMoveBlockResult
    {
        private bool _CanMove = false;
        public bool CanMove
        {
            get { return _CanMove; }
        }
        List<Point> _lstNeedToRemove = null;
      
        public List<Point> lstNeedToRemove
        {
            get
            {
                if (_lstNeedToRemove == null)
                {
                    _lstNeedToRemove = new List<Point>();
                }
                return _lstNeedToRemove;
            }
        }

        public AttemptToMoveBlockResult(bool pCanMove, List<Point> plstNeedToRemove)
        {
            _CanMove = pCanMove;
            _lstNeedToRemove = plstNeedToRemove;
        }
    }
}
