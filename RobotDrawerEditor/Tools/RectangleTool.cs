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
    public class RectangleTool : Tool
    {
        private MyRectangle drawnRectangle = new MyRectangle(new ControlPoint(0, 0), new ControlPoint(0, 0), Color.Black);
        private PointF startingPoint;

        public RectangleTool()
        {
            drawnRectangle.Deselect();
        }

        public override void MouseLeftButtonDown()
        {
            base.MouseLeftButtonDown();

            startingPoint = Mouse.CurrentGlobalPosition;
            drawnRectangle.SetPositionAndShapeFromPoints(startingPoint, Mouse.CurrentGlobalPosition);

            drawnRectangle.Color = ProgramLogic.Instance.DrawingColor;
        }

        public override void MouseLeftButtonUp()
        {
            if (!mouseDownRegistered)
                return;

            AddDrawnObjectToCanvas();
            base.MouseLeftButtonUp();
        }

        public override void MouseMove()
        {
            drawnRectangle.SetPositionAndShapeFromPoints(startingPoint, Mouse.CurrentGlobalPosition);
        }

        public override void Paint(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            if (ProgramLogic.Mouse.LeftButtonDown)
                drawnRectangle.PaintObject(pen, e, programLogic);
        }

        protected override void AddDrawnObjectToCanvas()
        {
            RectangleF boundingRectangle = drawnRectangle.BoundingRectangle;

            if (boundingRectangle.Width != 0 && boundingRectangle.Height != 0)
            {
                MyRectangle added = new MyRectangle(drawnRectangle);
                MainForm.ProgramLogic.Canvas.AddDrawnObject(added);
                MainForm.ProgramLogic.DeselectAllDrawnObjects();
            }
        }
    }
}
