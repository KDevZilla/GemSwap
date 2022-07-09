using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing ;
namespace GemSwap
{
    public class SwapBlockResult
    {
        private bool _CanSwap = false;
        public bool CanSwqp
        {
            get { return _CanSwap; }
        }
        List<Point> _lstNeedToRemoveForCell1 = null;
        List<Point> _lstNeedToRemoveForCell2 = null;
        public List<Point> lstNeedToRemoveForCell1
        {
            get {
                if (_lstNeedToRemoveForCell1 == null)
                {
                    _lstNeedToRemoveForCell1 = new List<Point>();
                }
                return _lstNeedToRemoveForCell1; 
            }
        }

        public List<Point> lstNeedToRemoveForCell2
        {
            get
            {
                if (_lstNeedToRemoveForCell2 == null)
                {
                    _lstNeedToRemoveForCell2 = new List<Point>();
                }
                return _lstNeedToRemoveForCell2;
            }
        }
        public SwapBlockResult(bool pCanSwap, List<Point> plstNeedToRemoveForCell1, List<Point> plstNeedToRemoveForCell2)
        {
            _CanSwap = pCanSwap;
            _lstNeedToRemoveForCell1 = plstNeedToRemoveForCell1;
            _lstNeedToRemoveForCell2 = plstNeedToRemoveForCell2;
        }
    }
}
