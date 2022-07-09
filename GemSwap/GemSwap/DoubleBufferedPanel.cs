using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class DoubleBufferedPanel:System.Windows.Forms.Panel 
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }
    }

