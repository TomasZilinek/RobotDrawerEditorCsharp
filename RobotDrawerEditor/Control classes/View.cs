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
    public class View
    {
        public float GlobalX { get; private set; }
        public float GlobalY { get; private set; }
        public float GlobalWidth { get; private set; }
        public float GlobalHeight { get; private set; }
        public float CanvasUCWidth { get; private set; }
        public float CanvasUCHeight { get; private set; }

        private ProgramLogic programLogic;

        public readonly PointF nullPoint = new Point(-1, -1);

        public View(float x, float y, float width, float height, float canvasUCWidth, float canvasUCHeight,
                    ProgramLogic pl)
        {
            GlobalX = x;
            GlobalY = y;
            GlobalWidth = width;
            GlobalHeight = height;
            CanvasUCWidth = canvasUCWidth;
            CanvasUCHeight = canvasUCHeight;

            programLogic = pl;
        }

        public bool PointInView(PointF point)
        {
            return GetBoundingRectangle().Contains(point);
        }

        public RectangleF GetBoundingRectangle()
        {
            return new RectangleF(GlobalX, GlobalY, GlobalWidth, GlobalHeight);
        }

        public bool DrawnObjectInView(DrawnObject obj)
        {
            RectangleF viewBoundingRect = GetBoundingRectangle();

            return viewBoundingRect.IntersectsWith(obj.BoundingRectangle) ||
                   viewBoundingRect.Contains(obj.BoundingRectangle);
        }

        public float GlobalToViewLength(float length)
        {
            return length / GlobalWidth * CanvasUCWidth;
        }

        public PointF GlobalToViewPoint(PointF globalPoint)
        {
            float xDiff = globalPoint.X - GlobalX;
            float yDiff = globalPoint.Y - GlobalY;

            float x = xDiff / GlobalWidth * CanvasUCWidth;
            float y = yDiff / GlobalHeight * CanvasUCHeight;

            return new PointF(x, y);
        }

        public float ViewToGlobalLength(float screenLength)
        {
            return screenLength / CanvasUCWidth * GlobalWidth;
        }

        public PointF ViewToGlobalPoint(PointF viewPoint)
        {
            return new PointF(GlobalX + ViewToGlobalLength(viewPoint.X),
                              GlobalY + ViewToGlobalLength(viewPoint.Y));
        }

        public DrawnObject GlobalToViewObject(DrawnObject obj)
        {
            if (obj is StraightLine line)
            {
                return new StraightLine(GlobalToViewPoint(line.ControlPoint0), GlobalToViewPoint(line.ControlPoint1), line.Color);
            }
            else if (obj is Ellipse ellipse)
            {
                return new Ellipse(GlobalToViewPoint(ellipse.Centre),
                                   GlobalToViewLength(ellipse.RadiusX),
                                   GlobalToViewLength(ellipse.RadiusY),
                                   ellipse.Color);
            }
            else if (obj is Circle circle)
            {
                return new Circle(GlobalToViewPoint(circle.Centre), GlobalToViewLength(circle.Radius), circle.Color);
            }
            else if (obj is MyRectangle rectangle)
            {
                PointF point = GlobalToViewPoint(new PointF(rectangle.X, rectangle.Y));

                return new MyRectangle(point.X, point.Y,
                                       GlobalToViewLength(rectangle.Width),
                                       GlobalToViewLength(rectangle.Height),
                                       rectangle.Color);
            }
            else if (obj is BezierCurve3 bezierCurve3)
            {
                return new BezierCurve3(GlobalToViewPoint(bezierCurve3.ControlPoints[0]),
                                        GlobalToViewPoint(bezierCurve3.ControlPoints[1]),
                                        GlobalToViewPoint(bezierCurve3.ControlPoints[2]),
                                        bezierCurve3.Color);
            }
            else if (obj is BezierCurve4 bezierCurve4)
            {
                return new BezierCurve4(GlobalToViewPoint(bezierCurve4.ControlPoints[0]),
                                        GlobalToViewPoint(bezierCurve4.ControlPoints[1]),
                                        GlobalToViewPoint(bezierCurve4.ControlPoints[2]),
                                        GlobalToViewPoint(bezierCurve4.ControlPoints[3]),
                                        bezierCurve4.Color);
            }

            return null;
        }

        public void MoveByGlobalDistance(float distance, ArrowDirection direction)
        {
            if (distance > 0)
            {
                float moveX = 0;
                float moveY = 0;

                if (direction == ArrowDirection.Left)
                    moveX = -distance;
                else if (direction == ArrowDirection.Right)
                    moveX = distance;
                else if (direction == ArrowDirection.Down)
                    moveY = -distance;
                else if (direction == ArrowDirection.Up)
                    moveY = distance;

                GlobalX += moveX;
                GlobalY += moveY;

                ProgramLogic.MainForm.canvasUserControl1.Invalidate();
            }
        }

        public void MoveByGlobalDistanceMultiple(float multiple, ArrowDirection direction)
        {
            float distance;

            if (direction == ArrowDirection.Up || direction == ArrowDirection.Down)
                distance = multiple * GlobalHeight;
            else
                distance = multiple * GlobalWidth;

            MoveByGlobalDistance(distance, direction);
        }

        public void MoveByScreenDistance(float distance, ArrowDirection direction)
        {
            MoveByGlobalDistance(ViewToGlobalLength(distance), direction);
        }

        public void CanvasUCsizeChanged(int newWidth, int newHeight)
        {
            CanvasUCWidth = newWidth;
            CanvasUCHeight = newHeight;

            GlobalHeight = GlobalWidth / newWidth * newHeight;
            GlobalY = programLogic.Canvas.Paper.Height / 2 - GlobalHeight / 2;  // delete this and solve paper centering at construction
        }

        public void ChangeNearObjectDistances()
        {
            SelectionPoint.NEAR_POINT_DISTANCE = ViewToGlobalLength(3);
            DrawnObject.globalAllowedHoverDistance = ViewToGlobalLength(5);
        }

        public void ZoomIn(float ratio, Point screenMousePosition)
        {
            float real_ratio = 1 / ratio;

            PointF globalMousePosition
                = new PointF(GlobalX + ViewToGlobalLength(screenMousePosition.X),
                             GlobalY + ViewToGlobalLength(screenMousePosition.FlipYAxis().Y));

            float newWidth = GlobalWidth * real_ratio;
            float newHeight = GlobalHeight * real_ratio;

            float widthDiff = GlobalWidth - newWidth;
            float heightDiff = GlobalHeight - newHeight;

            GlobalX += widthDiff * (globalMousePosition.X - GlobalX) / GlobalWidth;
            GlobalY += heightDiff * (globalMousePosition.Y - GlobalY) / GlobalHeight;

            GlobalWidth = newWidth;
            GlobalHeight = newHeight;

            ProgramLogic.MainForm.canvasUserControl1.Invalidate();
        }

        public void ZoomOut(float ratio, Point screenMousePosition)
        {
            float real_ratio = 1 / ratio;

            PointF globalMousePosition
                = new PointF(GlobalX + ViewToGlobalLength(screenMousePosition.X),
                             GlobalY + ViewToGlobalLength(screenMousePosition.FlipYAxis().Y));

            float newWidth = GlobalWidth * real_ratio;
            float newHeight = GlobalHeight * real_ratio;

            float widthDiff = newWidth - GlobalWidth;
            float heightDiff = newHeight - GlobalHeight;

            GlobalX -= widthDiff * (globalMousePosition.X - GlobalX) / GlobalWidth;
            GlobalY -= heightDiff * (globalMousePosition.Y - GlobalY) / GlobalHeight;

            GlobalWidth = newWidth;
            GlobalHeight = newHeight;

            ProgramLogic.MainForm.canvasUserControl1.Invalidate();
        }
    }
}
