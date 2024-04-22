using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    // cubic bezier curve
    public class BezierCurve4 : BezierCurve
    {
        private static int[] outerPointsIndexes = new int[] { 0, 3 };

        public BezierCurve4(PointF point0, PointF point1, PointF point2, PointF point3, Color color)
        {
            ControlPoints.Add(point0);
            ControlPoints.Add(point1);
            ControlPoints.Add(point2);
            ControlPoints.Add(point3);

            Color = color;

            InitializeAfterConstructor();
        }

        public BezierCurve4(BezierCurve4 curve)
        {
            ControlPoints = curve.ControlPoints.Select(p => new ControlPoint(p)).ToList();
            Color = curve.Color;

            InitializeAfterConstructor();
        }

        public BezierCurve4(BezierCurve3 curve)
        {
            PointF p0 = curve.ControlPoints[0];
            PointF p1 = ((PointF)curve.ControlPoints[0]).Add(((PointF)curve.ControlPoints[1]).Subtract(p0).Multiply(2f / 3f));
            PointF p2 = ((PointF)curve.ControlPoints[2]).Add(((PointF)curve.ControlPoints[1]).Subtract(curve.ControlPoints[2]).Multiply(2f / 3f));
            PointF p3 = curve.ControlPoints[2];

            ControlPoints = new List<ControlPoint> { p0, p1, p2, p3 };
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
            return new BezierCurve4(ControlPoints[0].FlipYAxis(),
                                    ControlPoints[1].FlipYAxis(),
                                    ControlPoints[2].FlipYAxis(),
                                    ControlPoints[3].FlipYAxis(), Color);
        }

        public override PointF PointAtCurve(float t)
        {
            float rev = 1 - t;

            double px = Math.Pow(rev, 3) * ControlPoints[0].X +
                        3 * Math.Pow(rev, 2) * t * ControlPoints[1].X +
                        3 * rev * Math.Pow(t, 2) * ControlPoints[2].X +
                        Math.Pow(t, 3) * ControlPoints[3].X;

            double py = Math.Pow(rev, 3) * ControlPoints[0].Y +
                        3 * Math.Pow(rev, 2) * t * ControlPoints[1].Y +
                        3 * rev * Math.Pow(t, 2) * ControlPoints[2].Y +
                        Math.Pow(t, 3) * ControlPoints[3].Y;

            return new PointF((float)px, (float)py);
        }

        public StraightLine[] GetDescribingLines(int chunks)
        {
            StraightLine[] toReturn = new StraightLine[chunks];
            PointF previous = PointAtCurve(0);

            for (int i = 1; i <= chunks; i++)
            {
                float t = i / (float)chunks;

                PointF first = previous;
                PointF second = PointAtCurve(t);

                toReturn[i - 1] = new StraightLine(first, second, Color.Black);

                // Console.WriteLine($"from = {t}, to = {t + 1f / chunks}");
                previous = second;
            }

            return toReturn;
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            return DistanceFromPoint(globalMousePosition) <= globalAllowedHoverDistance;
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            Color previousColor = pen.Color;
            pen.Color = Color;

            BezierCurve4 bezCurve = ProgramLogic.View.GlobalToViewObject(this) as BezierCurve4;
            bezCurve = bezCurve.FlipYAxis(ProgramLogic.View.CanvasUCHeight) as BezierCurve4;

            e.Graphics.DrawBezier(pen, bezCurve.ControlPoints[0].X, bezCurve.ControlPoints[0].Y,
                                       bezCurve.ControlPoints[1].X, bezCurve.ControlPoints[1].Y,
                                       bezCurve.ControlPoints[2].X, bezCurve.ControlPoints[2].Y,
                                       bezCurve.ControlPoints[3].X, bezCurve.ControlPoints[3].Y);

            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[0].X - 2.5f, bezCurve.ControlPoints[0].Y - 2.5f, 5, 5));
            pen.Color = Color.Red;
            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[1].X - 2.5f, bezCurve.ControlPoints[1].Y - 2.5f, 5, 5));
            pen.Color = Color.Green;
            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[2].X - 2.5f, bezCurve.ControlPoints[2].Y - 2.5f, 5, 5));
            pen.Color = Color.Blue;
            e.Graphics.DrawEllipse(pen, new RectangleF(bezCurve.ControlPoints[3].X - 2.5f, bezCurve.ControlPoints[3].Y - 2.5f, 5, 5));

            pen.Color = previousColor;
        }

        protected override void ChangeShapeBySelectionPoints()
        {
            float xDiff, yDiff;

            if (selectedSelectionPointIndex != -1)
            {
                xDiff = SelectionPoints[selectedSelectionPointIndex].X - selectedSelectionPointPreviousPosition.X;
                yDiff = SelectionPoints[selectedSelectionPointIndex].Y - selectedSelectionPointPreviousPosition.Y;
            }
            else
            {
                xDiff = Mouse.CurrentGlobalPosition.X - Mouse.PreviousGlobalPosition.X;
                yDiff = Mouse.CurrentGlobalPosition.Y - Mouse.PreviousGlobalPosition.Y;
            }

            float currentWidth = BoundingRectangle.Width;
            float currentHeight = BoundingRectangle.Height;

            PointF[] controlPointsAsPointFArray = ControlPoints.ToList().Select(x => (PointF)x).ToArray();

            PointF minPoint = Geometry.MinPointComponentWise(controlPointsAsPointFArray);
            PointF maxPoint = Geometry.MaxPointComponentWise(controlPointsAsPointFArray);

            float newWidth = maxPoint.X - minPoint.X;
            float newHeight = maxPoint.Y - minPoint.Y;

            for (int i = 0; i < ControlPoints.Count(); i++)
            {
                ControlPoint controlPoint = ControlPoints[i];
                float innerPointsXDiff = selectedSelectionPointPreviousPosition.X - controlPoint.X;
                float innerPointsYDiff = selectedSelectionPointPreviousPosition.Y - controlPoint.Y;

                float newPosX = ControlPoints[i].X;
                float newPosY = ControlPoints[i].Y;

                if (xDiff != 0)
                {
                    float toadd = innerPointsXDiff - innerPointsXDiff / newWidth * currentWidth;

                    newPosX = controlPoint.X + (outerPointsIndexes.Contains(i) ? toadd : -toadd);
                }
                    

                if (yDiff != 0)
                {
                    float toadd = innerPointsYDiff - innerPointsYDiff / newHeight * currentHeight;

                    newPosY = controlPoint.Y + (outerPointsIndexes.Contains(i) ? toadd : -toadd);
                }

                ControlPoints[i] = new ControlPoint(newPosX, newPosY);
            }
        }

        public override object Clone()
        {
            return new BezierCurve4(this);
        }
    }
}
