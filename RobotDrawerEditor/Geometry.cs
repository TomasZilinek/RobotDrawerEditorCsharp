using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor
{
    public static class Geometry
    {
        public static double DotProduct(PointF pointA, PointF pointB, PointF pointC)
        {
            PointF AB = new PointF(pointB.X - pointA.X, pointB.Y - pointA.Y);
            PointF BC = new PointF(pointC.X - pointB.X, pointC.Y - pointB.Y);

            return AB.X * BC.X + AB.Y * BC.Y;
        }
        
        public static double CrossProduct(PointF pointA, PointF pointB, PointF pointC)
        {
            PointF AB = new PointF(pointB.X - pointA.X, pointB.Y - pointA.Y);
            PointF AC = new PointF(pointC.X - pointA.X, pointC.Y - pointA.Y);

            return AB.X * AC.Y - AB.Y * AC.X;
        }

        public static Point ToPoint(PointF pointF)
        {
            return new Point() { X = (int)pointF.X, Y = (int)pointF.Y };
        }

        public static Rectangle ToRectangle(RectangleF rectF)
        {
            return Rectangle.Round(rectF);
        }

        public static PointF PointsAddition(PointF p0, PointF p1)
        {
            return new PointF(p0.X + p1.X, p0.Y + p1.Y);
        }

        public static PointF PointsSubtraction(PointF p0, PointF p1)
        {
            return new PointF(p0.X - p1.X, p0.Y - p1.Y);
        }

        public static RectangleF GetRectangleFromPoints(PointF p0, PointF p1)
        {
            return new RectangleF(Math.Min(p0.X, p1.X), Math.Min(p0.Y, p1.Y),
                                  Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y));
        }

        public static PointF GetRectangleCentre(RectangleF rectangle)
        {
            return new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        public static PointF MinXPoint(PointF p0, PointF p1)
        {
            return p0.X < p1.X ? p0 : p1;
        }

        public static PointF MinYPoint(PointF p0, PointF p1)
        {
            return p0.Y < p1.Y ? p0 : p1;
        }

        public static PointF MaxXPoint(PointF p0, PointF p1)
        {
            return p0.X > p1.X ? p0 : p1;
        }

        public static PointF MaxYPoint(PointF p0, PointF p1)
        {
            return p0.Y > p1.Y ? p0 : p1;
        }

        public static PointF MinPointComponentWise(params PointF[] points)
        {
            PointF minPoint = new PointF(float.MaxValue, float.MaxValue);

            foreach (PointF point in points)
                minPoint = new PointF(Math.Min(minPoint.X, point.X), Math.Min(minPoint.Y, point.Y));

            return minPoint;
        }

        public static PointF MaxPointComponentWise(params PointF[] points)
        {
            PointF maxPoint = new PointF(float.MinValue, float.MinValue);

            foreach (PointF point in points)
                maxPoint = new PointF(Math.Max(maxPoint.X, point.X), Math.Max(maxPoint.Y, point.Y));

            return maxPoint;
        }

        private static Vector2[] FindCoefficientsForRoots(PointF ip0, PointF ip1, PointF ip2, PointF ip3)
        {
            Vector2 p0 = new Vector2(ip0.X, ip0.Y);
            Vector2 p1 = new Vector2(ip1.X, ip1.Y);
            Vector2 p2 = new Vector2(ip2.X, ip2.Y);
            Vector2 p3 = new Vector2(ip3.X, ip3.Y);

            Vector2 a = 3 * (-p0 + 3 * p1 - 3 * p2 + p3);
            Vector2 b = 6 * (p0 - 2 * p1 + p2);
            Vector2 c = 3 * (p1 - p0);

            return new Vector2[] { a, b, c };
        }

        private static Vector2[] FindCoefficientsForRoots(PointF ip0, PointF ip1, PointF ip2)
        {
            Vector2 p0 = new Vector2(ip0.X, ip0.Y);
            Vector2 p1 = new Vector2(ip1.X, ip1.Y);
            Vector2 p2 = new Vector2(ip2.X, ip2.Y);

            Vector2 a = new Vector2(0, 0);
            Vector2 b = 2 * (p0 - 2 * p1 + p2);
            Vector2 c = 2 * (p1 - p0);

            return new Vector2[] { a, b, c };
        }

        /// <summary>
        /// Find the roots of a cubic bezier curve in order to find minimum and maximum
        /// </summary>
        private static List<double> FindRoots(Vector2[] coeficients, BezierCurve curve)
        {
            List<PointF> points = curve.ControlPoints.Select(p => (PointF)p).ToList();

            Vector2 p0 = new Vector2(points[0].X, points[0].Y);
            Vector2 p1 = new Vector2(points[1].X, points[1].Y);
            Vector2 p2 = new Vector2(points[2].X, points[2].Y);

            Vector2 a = coeficients[0];
            Vector2 b = coeficients[1];
            Vector2 c = coeficients[2];

            // https://snoozetime.github.io/2018/05/22/bezier-curve-bounding-box.html

            List<double> roots = new List<double>();

            if (curve is BezierCurve3)
            {
                Vector2 res = (p0 - p1) / (p0 - 2 * p1 + p2);

                if (res.X >= 0 && res.X <= 1)
                    roots.Add(res.X);

                if (res.Y >= 0 && res.Y <= 1)
                    roots.Add(res.Y);
            }
            else if (curve is BezierCurve4)
            {
                Vector2 p3 = new Vector2(points[3].X, points[3].Y);

                // along x
                double discriminantX = b.X * b.X - 4 * a.X * c.X;

                if (discriminantX < 0)
                {
                    // No roots
                }
                else if (discriminantX == 0)
                {
                    // one real root
                    double rootx = (-b.X) / (2 * a.X);

                    if (rootx >= 0 && rootx <= 1)
                        roots.Add(rootx);
                }
                else if (discriminantX > 0)
                {
                    // Two real roots
                    double rootx1 = (-b.X + Math.Sqrt(discriminantX)) / (2 * a.X);
                    double rootx2 = (-b.X - Math.Sqrt(discriminantX)) / (2 * a.X);

                    if (rootx1 >= 0 && rootx1 <= 1)
                        roots.Add(rootx1);

                    if (rootx2 >= 0 && rootx2 <= 1)
                        roots.Add(rootx2);
                }

                // along y
                double discriminantY = b.Y * b.Y - 4 * a.Y * c.Y;

                if (discriminantY < 0)
                {
                    // No roots
                }
                else if (discriminantY == 0)
                {
                    // one real root
                    double rooty = (-b.Y) / (2 * a.Y);

                    if (rooty >= 0 && rooty <= 1)
                        roots.Add(rooty);
                }
                else if (discriminantY > 0)
                {
                    // Two real roots
                    double rooty1 = (-b.Y + Math.Sqrt(discriminantY)) / (2 * a.Y);
                    double rooty2 = (-b.Y - Math.Sqrt(discriminantY)) / (2 * a.Y);

                    if (rooty1 >= 0 && rooty1 <= 1)
                        roots.Add(rooty1);

                    if (rooty2 >= 0 && rooty2 <= 1)
                        roots.Add(rooty2);
                }
            }

            return roots;
        }

        public static Tuple<float, float, float, float> GetMinAndMaxValuesOfBezierCurve(BezierCurve bezierCurve)
        {
            Vector2[] coefficient;
            List<double> roots;
            PointF startingPoint = bezierCurve.ControlPoints[0];
            PointF endingPoint;

            if (bezierCurve is BezierCurve3 bezierCurve3)
            {
                coefficient = FindCoefficientsForRoots(bezierCurve3.ControlPoints[0],
                                                       bezierCurve3.ControlPoints[1],
                                                       bezierCurve3.ControlPoints[2]);

                endingPoint = bezierCurve.ControlPoints[2];
            }
            else if (bezierCurve is BezierCurve4 bezierCurve4)
            {
                coefficient = FindCoefficientsForRoots(bezierCurve4.ControlPoints[0],
                                                       bezierCurve4.ControlPoints[1],
                                                       bezierCurve4.ControlPoints[2],
                                                       bezierCurve4.ControlPoints[3]);

                endingPoint = bezierCurve.ControlPoints[3];
            }
            else
                return null;

            roots = FindRoots(coefficient, bezierCurve);

            // Initialize min and max with the first point
            float min_x = Math.Min(startingPoint.X, endingPoint.X);
            float max_x = Math.Max(startingPoint.X, endingPoint.X);
            float min_y = Math.Min(startingPoint.Y, endingPoint.Y);
            float max_y = Math.Max(startingPoint.Y, endingPoint.Y);

            for (int i = 0; i < roots.Count; i++)
            {
                double param = roots[i];
                PointF point = bezierCurve.PointAtCurve((float)param);

                if (point.X > max_x)
                    max_x = point.X;

                if (point.X < min_x)
                    min_x = point.X;

                if (point.Y > max_y)
                    max_y = point.Y;

                if (point.Y < min_y)
                    min_y = point.Y;
            }

            return new Tuple<float, float, float, float>(min_x, min_y, max_x, max_y);
        }

        public static Vector2 PointFToVector2(PointF point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
