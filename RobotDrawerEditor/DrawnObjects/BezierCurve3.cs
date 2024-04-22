using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    // quadratic bezier curve
    public class BezierCurve3 : BezierCurve
    {
        public BezierCurve3(PointF point0, PointF point1, PointF point2, Color color)
        {
            ControlPoints.Add(point0);
            ControlPoints.Add(point1);
            ControlPoints.Add(point2);

            Color = color;

            InitializeAfterConstructor();
        }

        public BezierCurve3(BezierCurve3 curve)
        {
            ControlPoints = curve.ControlPoints.Select(p => new ControlPoint(p)).ToList();
            Color = curve.Color;

            InitializeAfterConstructor();
        }

        protected override void InitializeAfterConstructor()
        {
            ComputeBoundingRectangleF();
            PlaceSelectionPointsOnBoundingRectangle();
        }

        protected override DrawnObject Flip(float viewHeight)
        {
            return FlipYAxis(viewHeight);
        }

        public override DrawnObject FlipYAxis(float viewHeight)
        {
            return new BezierCurve3(ControlPoints[0].FlipYAxis(),
                                    ControlPoints[1].FlipYAxis(),
                                    ControlPoints[2].FlipYAxis(), Color);
        }

        public override PointF PointAtCurve(float t)
        {
            float rev = 1 - t;

            double px = Math.Pow(rev, 2) * ControlPoints[0].X +
                        2 * rev * t * ControlPoints[1].X +
                        Math.Pow(t, 2) * ControlPoints[2].X;

            double py = Math.Pow(rev, 2) * ControlPoints[0].Y +
                        2 * rev * t * ControlPoints[1].Y +
                        Math.Pow(t, 2) * ControlPoints[2].Y;

            return new PointF((float)px, (float)py);
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            return DistanceFromPoint(globalMousePosition) <= globalAllowedHoverDistance;
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            Color previousColor = pen.Color;
            pen.Color = Color;

            BezierCurve3 bezCurve = ProgramLogic.View.GlobalToViewObject(this) as BezierCurve3;
            bezCurve = bezCurve.FlipYAxis(ProgramLogic.View.CanvasUCHeight) as BezierCurve3;

            GraphicsPath myPath = new GraphicsPath();

            List<PointF> toAdd = ControlPoints.Select(p => p.Position).ToList();
            toAdd.Add(toAdd.Last());

            myPath.AddBeziers(toAdd.Select(p => ProgramLogic.View.GlobalToViewPoint(p).FlipYAxis()).ToArray());

            e.Graphics.DrawPath(pen, myPath);

            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[0].X - 2.5f, bezCurve.ControlPoints[0].Y - 2.5f, 5, 5));
            pen.Color = Color.Red;
            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[1].X - 2.5f, bezCurve.ControlPoints[1].Y - 2.5f, 5, 5));
            pen.Color = Color.Green;
            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[2].X - 2.5f, bezCurve.ControlPoints[2].Y - 2.5f, 5, 5));
            
            pen.Color = previousColor;
        }

        protected override void ChangeShapeBySelectionPoints()
        {
            // not even working in bezier4
        }

        public override object Clone()
        {
            return new BezierCurve3(this);
        }
    }
}
