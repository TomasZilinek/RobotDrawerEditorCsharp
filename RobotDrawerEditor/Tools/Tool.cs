using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public abstract class Tool
    {
        protected bool mouseDownRegistered = false;

        public virtual void MouseLeftButtonDown()
        {
            mouseDownRegistered = true;
        }

        public virtual void MouseLeftButtonUp()
        {
            mouseDownRegistered = false;
        }

        public abstract void MouseMove();
        public abstract void Paint(Pen pen, PaintEventArgs e, ProgramLogic programLogic);

        protected abstract void AddDrawnObjectToCanvas();
    }
}
