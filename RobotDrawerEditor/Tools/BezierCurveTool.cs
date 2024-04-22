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
    public class BezierCurveTool : Tool
    {
        private List<PointF> Points = new List<PointF>();
        private BezierCurve bezierCurve = null;
        private ConnectedBezierCurve connectedBezierCurve = new ConnectedBezierCurve(addedCurveColor);
        private static Color addedCurveColor = Color.Green;
        private static Color currentDrawnCurveColor = Color.Red;

        public override void MouseLeftButtonDown()
        {
            base.MouseLeftButtonDown();

            Points.Add(Mouse.CurrentGlobalPosition);
            Points.Add(Mouse.CurrentGlobalPosition);
        }

        public override void MouseLeftButtonUp()
        {
            if (!mouseDownRegistered || Points.Count == 0)
                return;

            if (Points.Count != 2 && Points[Points.Count - 2] == Mouse.CurrentGlobalPosition)
            {
                Points.RemoveAt(Points.Count - 1);

                BezierCurve toAdd = new BezierCurve3(Points[0], Points[1], Points[2], addedCurveColor);
                connectedBezierCurve.AddCurve(toAdd);

                PointF point = Points.Last();
                Points.Clear();

                Points.Add(point);
                Points.Add(point);
            }
            else if (Points.Count == 3 || Points.Count == 4)
            {
                BezierCurve toAdd;

                if (Points.Count == 3)
                    toAdd = new BezierCurve3(Points[0], Points[1], Points[2], addedCurveColor);
                else
                    toAdd = new BezierCurve4(Points[0], Points[1], Points[2], Points[3], addedCurveColor);
                                                            
                connectedBezierCurve.AddCurve(toAdd);

                PointF point = Points.Last();

                Points.Clear();
                Points.Add(point);
                Points.Add(Mouse.CurrentGlobalPosition);
            }

            base.MouseLeftButtonUp();
        }

        public override void MouseMove()
        {
            if (Points.Count < 2)
                return;

            if (Mouse.Instance.LeftButtonDown)
            {
                if (Points.Count == 2)
                    Points[Points.Count - 1] = Mouse.CurrentGlobalPosition;
                else if (Points.Count == 3 || Points.Count == 4)
                {
                    Points[Points.Count - 2] = Points[Points.Count - 1].Add(Points[Points.Count - 1].Subtract(Mouse.CurrentGlobalPosition));
                }
            }
        }

        public override void Paint(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            Color prevColor = pen.Color;

            connectedBezierCurve.PaintObject(pen, e, programLogic);

            if (Points.Count == 0)
                return;
            else if (Points.Count == 1 || (Points.Count == 2 && Mouse.Instance.LeftButtonDown))
            {
                pen.Color = currentDrawnCurveColor;
                e.Graphics.DrawLine(pen, ProgramLogic.View.GlobalToViewPoint(Points[0]).FlipYAxis(), Mouse.ScreenPosition);
            }
            else if (Points.Count == 2)
            {
                PointF p0 = Points[0];
                PointF p1 = Points[1];
                PointF p2 = Mouse.CurrentGlobalPosition;

                bezierCurve = new BezierCurve3(p0, p1, p2, currentDrawnCurveColor);
                bezierCurve.PaintObject(pen, e, programLogic);
            }
            else if (Points.Count == 3)
            {
                bezierCurve = new BezierCurve3(Points[0], Points[1], Points[2], currentDrawnCurveColor);

                bezierCurve.PaintObject(pen, e, programLogic);
            }
            else if (Points.Count == 4)
            {
                bezierCurve = new BezierCurve4(Points[0], Points[1], Points[2], Points[3], currentDrawnCurveColor);
                bezierCurve.PaintObject(pen, e, programLogic);
            }

            pen.Color = prevColor;
        }

        protected override void AddDrawnObjectToCanvas()
        {
            connectedBezierCurve.Color = ProgramLogic.Instance.DrawingColor;
            MainForm.ProgramLogic.Canvas.AddDrawnObject(connectedBezierCurve);
            MainForm.ProgramLogic.DeselectAllDrawnObjects();
        }

        public void CancelDrawingShape()
        {
            connectedBezierCurve = new ConnectedBezierCurve(addedCurveColor);
            bezierCurve = null;
            Points.Clear();
        }

        public void MouseRightButtonDown()
        {
            AddDrawnObjectToCanvas();
            CancelDrawingShape();
        }
    }
}
