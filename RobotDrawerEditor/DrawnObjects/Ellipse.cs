using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    // https://stackoverflow.com/questions/14169234/the-relation-of-the-bezier-curve-and-ellipse
    // http://jsfiddle.net/wtVZW/3/

    public class Ellipse : DrawnObject
    {
        public PointF Centre { get; private set; }
        public float RadiusX { get; private set; }
        public float RadiusY { get; private set; }

        public Ellipse(PointF centre, float radiusX, float radiusY, Color color)
        {
            Centre = centre;
            RadiusX = radiusX;
            RadiusY = radiusY;
            Color = color;

            InitializeAfterConstructor();
        }

        public Ellipse(Ellipse e) : this(e.Centre, e.RadiusX, e.RadiusY, e.Color)
        {

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
            return new Ellipse(Centre.FlipYAxis(), RadiusX, RadiusY, Color);
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            double px = Math.Abs(globalMousePosition.X - Centre.X);
            double py = Math.Abs(globalMousePosition.Y - Centre.Y);

            double a = RadiusX;
            double b = RadiusY;

            double tx = 0.70710678118;
            double ty = 0.70710678118;

            double x, y, ex, ey, rx, ry, qx, qy, r, q, t;

            for (int i = 0; i < 3; i++)
            {
                x = a * tx;
                y = b * ty;

                ex = (a * a - b * b) * (tx * tx * tx) / a;
                ey = (b * b - a * a) * (ty * ty * ty) / b;

                rx = x - ex;
                ry = y - ey;

                qx = px - ex;
                qy = py - ey;

                r = Math.Sqrt(rx * rx + ry * ry);
                q = Math.Sqrt(qy * qy + qx * qx);

                tx = Math.Min(1, Math.Max(0, (qx * r / q + ex) / a));
                ty = Math.Min(1, Math.Max(0, (qy * r / q + ey) / b));

                t = Math.Sqrt(tx * tx + ty * ty);

                tx /= t;
                ty /= t;
            }

            PointF resPoint = new PointF
            {
                X = (float)(a * (globalMousePosition.X - Centre.X < 0 ? -tx : tx)) + Centre.X,
                Y = (float)(b * (globalMousePosition.Y - Centre.Y < 0 ? -ty : ty)) + Centre.Y
            };

            // Debug line to the edge of the ellipse
            // MainForm.ProgramLogic.Canvas.AddDrawnObject(new StraightLine(resPoint, globalMousePosition, Color.Black));

            return resPoint.TwoPointsDistance(globalMousePosition) < globalAllowedHoverDistance;
        }

        public override void ComputeBoundingRectangleF()
        {
            BoundingRectangle = new RectangleF(Centre.X - RadiusX, Centre.Y - RadiusY, 2 * RadiusX, 2 * RadiusY);
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            Color previousColor = pen.Color;
            pen.Color = Color;

            Ellipse ellipse = ProgramLogic.View.GlobalToViewObject(this) as Ellipse;
            ellipse = ellipse.FlipYAxis(ProgramLogic.View.CanvasUCHeight) as Ellipse;

            e.Graphics.DrawEllipse(pen, ellipse.BoundingRectangle);

            pen.Color = previousColor;
        }

        protected override void ChangeShapeBySelectionPoints()
        {
            RadiusX = Math.Abs(SelectionPoints[0].X - SelectionPoints[1].X);
            RadiusY = Math.Abs(SelectionPoints[0].Y - SelectionPoints[3].Y);
            Centre = new PointF(SelectionPoints[0].X + RadiusX, SelectionPoints[0].Y + RadiusY);

            ComputeBoundingRectangleF();
        }

        public override void SetPositionAndShapeFromPoints(PointF point0, PointF point1)
        {
            RadiusX = Math.Abs(Math.Abs(point0.X - point1.X) / 2);
            RadiusY = Math.Abs(Math.Abs(point0.Y - point1.Y) / 2);

            Centre = new PointF(Geometry.MinXPoint(point0, point1).X + RadiusX,
                                Geometry.MinYPoint(point0, point1).Y + RadiusY);

            ComputeBoundingRectangleF();
        }

        public override object Clone()
        {
            return new Ellipse(this);
        }
    }
}
