using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    public class MyRectangle : DrawnObject
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public StraightLine[] Lines = new StraightLine[4];

        public MyRectangle(PointF point, float width, float height, Color color)
        {
            X = point.X;
            Y = point.Y;
            Width = width;
            Height = height;
            Color = color;

            InitializeAfterConstructor();
        }

        public MyRectangle(float x, float y, float width, float height, Color color)
            : this(new PointF(x, y), width, height, color)
        {

        }

        public MyRectangle(PointF point0, PointF point1, Color color)
        {
            X = Math.Min(point0.X, point1.X);
            Y = Math.Min(point0.Y, point1.Y);
            Width = Math.Abs(point0.X - point1.X);
            Height = Math.Abs(point0.Y - point1.Y);
            Color = color;

            InitializeAfterConstructor();
        }

        public MyRectangle(MyRectangle rectangle)
        {
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
            Color = rectangle.Color;

            InitializeAfterConstructor();
        }

        protected override void InitializeAfterConstructor()
        {
            CreateLinesFromCoordsAndDimensions();
            ComputeBoundingRectangleF();
            PlaceSelectionPointsOnBoundingRectangle();
        }

        protected override DrawnObject Flip(float viewHeight)
        {
            return FlipYAxis(viewHeight);
        }

        public override DrawnObject FlipYAxis(float viewHeight)
        {
            return new MyRectangle(new PointF(X, Y).FlipYAxis(),
                                   new PointF(X + Width, Y + Height).FlipYAxis(),
                                   Color);
        }

        private void CreateLinesFromCoordsAndDimensions()
        {
            Lines[0] = new StraightLine(X, Y, X + Width, Y, Color);
            Lines[1] = new StraightLine(X, Y, X, Y + Height, Color);
            Lines[2] = new StraightLine(X + Width, Y, X + Width, Y + Height, Color);
            Lines[3] = new StraightLine(X, Y + Height, X + Width, Y + Height, Color);
        }

        public RectangleF ToRectangleF()
        {
            return new RectangleF(X, Y, width, height);
        }

        public static MyRectangle FromRectangleF(RectangleF rectangle, Color color)
        {
            return new MyRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color);
        }

        public override string ToString()
        {
            return "MyRec:{X=" + X + ",Y=" + Y + ", W=" + Width + ",H=" + Height + "}";
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            foreach (StraightLine line in Lines)
            {
                if (line.HoveredOver(globalMousePosition))
                    return true;
            }

            return false;
        }

        public override void ComputeBoundingRectangleF()
        {
            BoundingRectangle = ToRectangleF();
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            Color previousColor = pen.Color;
            pen.Color = Color;

            MyRectangle rect = ProgramLogic.View.GlobalToViewObject(this) as MyRectangle;
            rect = rect.FlipYAxis(ProgramLogic.View.CanvasUCHeight) as MyRectangle;

            e.Graphics.DrawRectangle(pen, Geometry.ToRectangle(rect.ToRectangleF()));
            pen.Color = previousColor;
        }

        protected override void ChangeShapeBySelectionPoints()
        {
            X = SelectionPoints.Min(cp => cp.X);
            Y = SelectionPoints.Min(cp => cp.Y);

            Width = SelectionPoints.Max(cp => cp.X) - X;
            Height = SelectionPoints.Max(cp => cp.Y) - Y;

            CreateLinesFromCoordsAndDimensions();
            ComputeBoundingRectangleF();
        }

        public override void SetPositionAndShapeFromPoints(PointF point0, PointF point1)
        {
            RectangleF rect = Geometry.GetRectangleFromPoints(point0, point1);

            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;

            ComputeBoundingRectangleF();
        }

        public void SetPositionAndShapeFromRectangle(RectangleF rect)
        {
            PointF pos = new PointF(rect.X, rect.Y);

            SetPositionAndShapeFromPoints(pos, new PointF(pos.X + rect.Width, pos.Y + rect.Height));
        }

        public override object Clone()
        {
            return new MyRectangle(this);
        }
    }
}
