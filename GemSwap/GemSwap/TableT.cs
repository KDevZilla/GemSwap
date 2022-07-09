using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTetris
{
    public class cRowT<T>
    {
        public List<T> Cols = new List<T>();
    }
    public class cTableT<T>
    {
        public List<cRowT<T>> Rows = new List<cRowT<T>>();
      
    }
}
