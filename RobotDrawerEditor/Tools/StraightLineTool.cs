using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public class StraightLineTool : Tool
    {
        private StraightLine drawnLine = new StraightLine(new ControlPoint(0, 0), new ControlPoint(0, 0), Color.Black);

        public StraightLineTool()
        {
            drawnLine.Deselect();
        }

        public override void MouseLeftButtonDown()
        {
            base.MouseLeftButtonDown();

            if (Mouse.Instance.LeftButtonDown && Mouse.Instance.RightButtonDown)
                return;

            drawnLine.ControlPoint0.Position = Mouse.CurrentGlobalPosition;
            drawnLine.ControlPoint1.Position = Mouse.CurrentGlobalPosition;

            drawnLine.Color = ProgramLogic.Instance.DrawingColor;
        }

        public override void MouseLeftButtonUp()
        {
            if (!mouseDownRegistered)
                return;

            if (Mouse.Instance.LeftButtonDown && Mouse.Instance.RightButtonDown)
                return;

            AddDrawnObjectToCanvas();

            base.MouseLeftButtonUp();
        }

        public override void MouseMove()
        {
            if (Mouse.Instance.LeftButtonDown && Mouse.Instance.RightButtonDown)
                return;

            drawnLine.ControlPoint1.Position = Mouse.CurrentGlobalPosition;
        }

        public override void Paint(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            if (ProgramLogic.Mouse.LeftButtonDown)
                drawnLine.PaintObject(pen, e, programLogic);
        }

        protected override void AddDrawnObjectToCanvas()
        {
            if (drawnLine.ControlPoint0.Position != drawnLine.ControlPoint1.Position)
            {
                StraightLine added = new StraightLine(drawnLine);
                MainForm.ProgramLogic.Canvas.AddDrawnObject(added);
                MainForm.ProgramLogic.DeselectAllDrawnObjects();
            }
        }
    }
}
