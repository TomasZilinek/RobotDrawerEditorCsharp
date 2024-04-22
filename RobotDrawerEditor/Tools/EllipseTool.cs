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
    public class EllipseTool : Tool
    {
        private Ellipse drawnEllipse = new Ellipse(new ControlPoint(0, 0), 0, 0, Color.Black);
        private PointF startingPoint;

        public EllipseTool()
        {
            drawnEllipse.Deselect();
        }

        public override void MouseLeftButtonDown()
        {
            base.MouseLeftButtonDown();

            startingPoint = Mouse.CurrentGlobalPosition;
            drawnEllipse.SetPositionAndShapeFromPoints(startingPoint, Mouse.CurrentGlobalPosition);

            drawnEllipse.Color = ProgramLogic.Instance.DrawingColor;
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
            PointF endingPoint;

            if (MainForm.ControlPressed)
            {
                PointF oppositeControlPoint = startingPoint;

                float xDiff = Math.Abs(Mouse.CurrentGlobalPosition.X - oppositeControlPoint.X);
                float yDiff = Math.Abs(Mouse.CurrentGlobalPosition.Y - oppositeControlPoint.Y);
                float offset = Math.Min(xDiff, yDiff);

                float resX, resY;

                if (Mouse.CurrentGlobalPosition.X < oppositeControlPoint.X)
                    resX = oppositeControlPoint.X - offset;
                else
                    resX = oppositeControlPoint.X + offset;

                if (Mouse.CurrentGlobalPosition.Y < oppositeControlPoint.Y)
                    resY = oppositeControlPoint.Y - offset;
                else
                    resY = oppositeControlPoint.Y + offset;

                endingPoint = new PointF(resX, resY);
            }
            else
            {
                endingPoint = Mouse.CurrentGlobalPosition;
            }

            drawnEllipse.SetPositionAndShapeFromPoints(startingPoint, endingPoint);
        }

        public override void Paint(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            if (ProgramLogic.Mouse.LeftButtonDown)
                drawnEllipse.PaintObject(pen, e, programLogic);
        }

        protected override void AddDrawnObjectToCanvas()
        {
            RectangleF boundingRectangle = drawnEllipse.BoundingRectangle;

            if (boundingRectangle.Width != 0 && boundingRectangle.Height != 0)
            {
                DrawnObject added;

                if (MainForm.ControlPressed)
                    added = new Circle(drawnEllipse.Centre, drawnEllipse.RadiusX, drawnEllipse.Color);
                else
                    added = new Ellipse(drawnEllipse);

                MainForm.ProgramLogic.Canvas.AddDrawnObject(added);
                MainForm.ProgramLogic.DeselectAllDrawnObjects();
            }
        }
    }
}
