using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public class Mouse
    {
        public static Point ScreenPosition;
        public static PointF PreviousGlobalPosition { get; private set; }
        public static PointF CurrentGlobalPosition { get; private set; }
        public bool LeftButtonDown { get; private set; } = false;
        public bool RightButtonDown { get; private set; } = false;
        public static Mouse Instance { get; private set; }

        public Mouse(Point point)
        {
            Instance = this;

            ScreenPosition = point;
            SetGlobalPositionFromViewPosition();
        }

        public Mouse(int x, int y) : this(new Point(x, y))
        {
            
        }

        public void MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                LeftButtonDown = true;
            else if (e.Button == MouseButtons.Right)
                RightButtonDown = true;
        }

        public void MouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                LeftButtonDown = false;
            else if (e.Button == MouseButtons.Right)
                RightButtonDown = false;
        }

        public void MouseMove(Point position)
        {
            ScreenPosition = position;
            PreviousGlobalPosition = CurrentGlobalPosition;

            SetGlobalPositionFromViewPosition();
        }

        private void SetGlobalPositionFromViewPosition()
        {
            if (ProgramLogic.View != null)
            {
                PointF flipped = new PointF(ScreenPosition.X, ScreenPosition.Y).FlipYAxis();
                CurrentGlobalPosition = ProgramLogic.View.ViewToGlobalPoint(flipped);
            }
        }
    }
}
