using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    public class Circle : DrawnObject
    {
        public PointF Centre { get; private set; }
        public float Radius { get; private set; }

        public Circle(PointF centre, float radius, Color color)
        {
            Centre = centre;
            Radius = radius;
            Color = color;

            hiddenSelectionPointsIndexes = new int[] { 1, 3, 4, 6 };
            InitializeAfterConstructor();
        }

        public Circle(Circle circle) : this(circle.Centre, circle.Radius, circle.Color)
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
            return new Circle(Centre.FlipYAxis(), Radius, Color);
        }

        public override bool HoveredOver(PointF globalMousePosition)
        {
            double distanceFromCentre = Centre.TwoPointsDistance(globalMousePosition);

            return Radius - globalAllowedHoverDistance <= distanceFromCentre &&
                   distanceFromCentre <= Radius + globalAllowedHoverDistance;
        }

        protected override void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            Color previousColor = pen.Color;
            pen.Color = Color;

            Circle circle = ProgramLogic.View.GlobalToViewObject(this) as Circle;
            circle = circle.FlipYAxis(ProgramLogic.View.CanvasUCHeight) as Circle;

            RectangleF boundingRect = circle.BoundingRectangle;

            e.Graphics.DrawEllipse(pen, boundingRect);

            pen.Color = previousColor;
        }

        protected override void ChangeShapeBySelectionPoints()
        {
            Radius = Math.Abs(SelectionPoints[0].X - SelectionPoints[1].X);
            Centre = new PointF(SelectionPoints[0].X + Radius, SelectionPoints[0].Y + Radius);

            ComputeBoundingRectangleF();
        }

        protected override void SetSelectedSelectionPointPositionFromMousePositionWhenDragged(PointF globalMousePosition)
        {
            int oppositeControlPointIndex = GetOppositeSelectionPointIndex(selectedSelectionPointIndex);
            SelectionPoint oppositeControlPoint = SelectionPoints[oppositeControlPointIndex];

            float xDiff = Math.Abs(globalMousePosition.X - oppositeControlPoint.X);
            float yDiff = Math.Abs(globalMousePosition.Y - oppositeControlPoint.Y);
            float offset = Math.Min(xDiff, yDiff);

            SelectionPoint selectedControlPoint = SelectionPoints[selectedSelectionPointIndex];

            float resX, resY;

            if (globalMousePosition.X < oppositeControlPoint.X)
                resX = oppositeControlPoint.X - offset;
            else
                resX = oppositeControlPoint.X + offset;

            if (globalMousePosition.Y < oppositeControlPoint.Y)
                resY = oppositeControlPoint.Y - offset;
            else
                resY = oppositeControlPoint.Y + offset;

            selectedControlPoint.Position = new PointF(resX, resY);
        }

        public override void ComputeBoundingRectangleF()
        {
            BoundingRectangle = new RectangleF(Centre.X - Radius, Centre.Y - Radius, 2 * Radius, 2 * Radius);
        }

        public override void SetPositionAndShapeFromPoints(PointF point0, PointF point1)
        {
            Radius = Math.Abs(Math.Abs(point0.X - point1.X) / 2);
            Centre = new PointF(Geometry.MinXPoint(point0, point1).X + Radius,
                                Geometry.MinYPoint(point0, point1).Y + Radius);

            ComputeBoundingRectangleF();
        }

        public override object Clone()
        {
            return new Circle(this);
        }
    }
}
