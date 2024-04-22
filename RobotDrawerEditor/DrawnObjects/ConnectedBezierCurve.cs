using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    public class ConnectedBezierCurve : DrawnObject
    {
        public List<BezierCurve> Curves { get; private set; }
        public override Color Color
        {
            get => base.Color;
            
            set
            {
                base.Color = value;

                foreach (BezierCurve curve in Curves)
                    curve.Color = value;
            }
        }

        public ConnectedBezierCurve(Color color)
        {
            Curves = new List<BezierCurve>();
            Color = color;

            InitializeAfterConstructor();
        }

        public ConnectedBezierCurve(List<BezierCurve> curves, Color color)
        {
            Curves = curves.Select(x => x is BezierCurve3 ?
                                          new BezierCurve3(x as BezierCurve3) :
                                          new BezierCurve4(x as BezierCurve4) as BezierCurve).ToList();
            Color = color;

            InitializeAfterConstructor();
        }

        public ConnectedBezierCurve(ConnectedBezierCurve connectedCurve)
        {
            Curves = connectedCurve.Curves.Select(c => c is BezierCurve3 ?
                                                       new BezierCurve3(c as BezierCurve3) :
                                                       new BezierCurve4(c as BezierCurve4) as BezierCurve).ToList();

            Color = connectedCurve.Color;
            InitializeAfterConstructor();
        }

        protected override void InitializeAfterConstructor()
        {
            ComputeBoundingRectangleF();
            PlaceSelectionPointsOnBoundingRectangle();
        }

        public void AddCurve(BezierCurve curve)
        {
            curve.Color = Color;
            Curves.Add(curve);

            ComputeBoundingRectangleF();
        }

        public void AddCurveByPoints(PointF point0, PointF point1, PointF point2)
        {
            if (!Curves.Any())
                return;

            BezierCurve3 curve = new BezierCurve3(point0, point1, point2, Color);
            AddCurve(curve);
        }

        public void AddCurveByPoints(PointF point0, PointF point1, PointF point2, PointF point3)
        {
            if (!Curves.Any())
                return;

            BezierCurve4 curve = new BezierCurve4(point0, point1, point2, point3, Color);
            AddCurve(curve);
        }

        protected override DrawnObject Flip(float viewHeight)
        {
            return FlipYAxis(viewHeight);
        }

        public override DrawnObject FlipYAxis(float viewHeight)
        {
            return new ConnectedBezierCurve(Curves.Select(c => c.FlipYAxis(viewHeight) as BezierCurve).ToList(), Color);
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            foreach (BezierCurve curve in Curves)
                if (curve.HoveredOver(globalMousePosition))
                    return true;

            return false;
        }

        public override void ComputeBoundingRectangleF()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (BezierCurve curve in Curves)
            {
                if (curve.BoundingRectangle.X < minX)
                    minX = curve.BoundingRectangle.X;

                if (curve.BoundingRectangle.Y < minY)
                    minY = curve.BoundingRectangle.Y;

                if (curve.BoundingRectangle.X + curve.BoundingRectangle.Width > maxX)
                    maxX = curve.BoundingRectangle.X + curve.BoundingRectangle.Width;

                if (curve.BoundingRectangle.Y + curve.BoundingRectangle.Height > maxY)
                    maxY = curve.BoundingRectangle.Y + curve.BoundingRectangle.Height;
            }

            BoundingRectangle = new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            foreach (BezierCurve curve in Curves)
                curve.PaintObject(pen, e, programLogic);
        }

        protected override void ChangeShapeBySelectionPoints()
        {

        }

        public override void SetPositionAndShapeFromPoints(PointF point0, PointF point1)
        {
            
        }

        public override object Clone()
        {
            return new ConnectedBezierCurve(this);
        }
    }
}
