using RobotDrawerEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor
{
    public static class PointFextensions
    {
        public static PointF FlipYAxis(this PointF point)
        {
            return new PointF(point.X, ProgramLogic.View.CanvasUCHeight - point.Y);
        }

        public static Point FlipYAxis(this Point point)
        {
            return Point.Round(FlipYAxis(new PointF(point.X, point.Y)));
        }

        public static double TwoPointsDistance(this PointF point0, PointF point1)
        {
            return Math.Sqrt(Math.Pow(point0.X - point1.X, 2) + Math.Pow(point0.Y - point1.Y, 2));
        }

        public static PointF Multiply(this PointF point0, float constant)
        {
            var x = new PointF(point0.X * constant, point0.Y * constant);
            return x;
        }

        public static PointF Multiply(this PointF point0, PointF point1)
        {
            var x = new PointF(point0.X * point1.X, point0.Y * point1.Y);
            return x;
        }

        public static PointF Divide(this PointF point0, PointF point1)
        {
            var x = new PointF(point1.X == 0 ? float.PositiveInfinity : point0.X / point1.X,
                              point1.Y == 0 ? float.PositiveInfinity : point0.Y / point1.Y);
            return x;
        }

        public static PointF Add(this PointF point0, PointF point1)
        {
            var x = new PointF(point0.X + point1.X, point0.Y + point1.Y);
            return x;
        }

        public static PointF Subtract(this PointF point0, PointF point1)
        {
            var x =  new PointF(point0.X - point1.X, point0.Y - point1.Y);
            return x;
        }
    }
}
