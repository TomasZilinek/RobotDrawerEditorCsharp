using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    public class StraightLine : DrawnObject
    {
        public ControlPoint ControlPoint0 { get; private set; }
        public ControlPoint ControlPoint1 { get; private set; }

        private Dictionary<ControlPoint, SelectionPoint> controlPointsToSelectionPoints = new Dictionary<ControlPoint, SelectionPoint>();

        public StraightLine(ControlPoint cp0, ControlPoint cp1, Color color)
        {
            ControlPoint0 = cp0.X < cp1.X ? cp0 : cp1;
            ControlPoint1 = ControlPoint0 == cp0 ? cp1 : cp0;

            ControlPoint0 = new ControlPoint(ControlPoint0);
            ControlPoint1 = new ControlPoint(ControlPoint1);

            ControlPoint minXPoint = ControlPoint0.X < ControlPoint1.X ? ControlPoint0 : ControlPoint1;
            ControlPoint maxXPoint = minXPoint == ControlPoint0 ? ControlPoint1 : ControlPoint0;

            ControlPoint minYPoint = ControlPoint0.Y < ControlPoint1.Y ? ControlPoint0 : ControlPoint1;
            ControlPoint maxYPoint = minYPoint == ControlPoint0 ? ControlPoint1 : ControlPoint0;

            foreach (ControlPoint point in new ControlPoint[] { ControlPoint0, ControlPoint1 })
            {
                if (point == minXPoint)
                {
                    if (point == maxYPoint)
                        controlPointsToSelectionPoints[point] = SelectionPoints[5];
                    else
                        controlPointsToSelectionPoints[point] = SelectionPoints[0];
                }
                else if (point == maxXPoint)
                {
                    if (point == maxYPoint)
                        controlPointsToSelectionPoints[point] = SelectionPoints[7];
                    else
                        controlPointsToSelectionPoints[point] = SelectionPoints[2];
                }
            }

            Color = color;
            InitializeAfterConstructor();
        }

        protected override void InitializeAfterConstructor()
        {
            ComputeBoundingRectangleF();
            PlaceSelectionPointsOnBoundingRectangle();
        }

        public StraightLine(float x0, float y0, float x1, float y1, Color color)
            : this(new ControlPoint(x0, y0), new ControlPoint(x1, y1), color)
        {

        }

        public StraightLine(StraightLine line) : this(line.ControlPoint0, line.ControlPoint1, line.Color)
        {

        }

        protected override DrawnObject Flip(float viewHeight)
        {
            return FlipYAxis(viewHeight);
        }

        public override DrawnObject FlipYAxis(float viewHeight)
        {
            return new StraightLine(ControlPoint0.FlipYAxis(), ControlPoint1.FlipYAxis(), Color);
        }

        public override string ToString()
        {
            return "StraightLine:{P0=[" + ControlPoint0.X + "," + ControlPoint0.Y + "], P1=[" + ControlPoint1.X + "," + ControlPoint1.Y + "]}";
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            return DistanceFromPoint(globalMousePosition) <= globalAllowedHoverDistance;
        }
        
        public double DistanceFromPoint(PointF point)
        {
            PointF p1 = ControlPoint0;
            PointF p2 = ControlPoint1;

            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            if (dx == 0 && dy == 0)
            {
                dx = point.X - p1.X;
                dy = point.Y - p1.Y;

                return Math.Sqrt(dx * dx + dy * dy);
            }
            
            float t = ((point.X - p1.X) * dx + (point.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);
            
            if (t < 0)
            {
                dx = point.X - p1.X;
                dy = point.Y - p1.Y;
            }
            else if (t > 1)
            {
                dx = point.X - p2.X;
                dy = point.Y - p2.Y;
            }
            else
            {
                PointF closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = point.X - closest.X;
                dy = point.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            pen.Color = Color;
            View view = ProgramLogic.View;

            // draw line
            StraightLine viewLine = view.GlobalToViewObject(this) as StraightLine;
            viewLine = viewLine.FlipYAxis(view.CanvasUCHeight) as StraightLine;

            e.Graphics.DrawLine(pen, viewLine.ControlPoint0, viewLine.ControlPoint1);
        }

        protected override void ChangeShapeBySelectionPoints()
        {
            PointF prevPoint0 = ControlPoint0;
            PointF prevPoint1 = ControlPoint1;

            SelectionPoint cp0SelectionPoint = controlPointsToSelectionPoints[ControlPoint0];
            SelectionPoint cp1SelectionPoint = controlPointsToSelectionPoints[ControlPoint1];

            ControlPoint0 = cp0SelectionPoint.Position;
            ControlPoint1 = cp1SelectionPoint.Position;

            controlPointsToSelectionPoints.Remove(prevPoint0);
            controlPointsToSelectionPoints.Remove(prevPoint1);

            controlPointsToSelectionPoints[ControlPoint0] = cp0SelectionPoint;
            controlPointsToSelectionPoints[ControlPoint1] = cp1SelectionPoint;

            ComputeBoundingRectangleF();
        }

        public override void ComputeBoundingRectangleF()
        {
            BoundingRectangle = Geometry.GetRectangleFromPoints(ControlPoint0, ControlPoint1);
        }

        public override void SetPositionAndShapeFromPoints(PointF point0, PointF point1)
        {
            ControlPoint0 = point0;
            ControlPoint1 = point1;

            ComputeBoundingRectangleF();
        }

        public override object Clone()
        {
            return new StraightLine(this);
        }
    }
}
