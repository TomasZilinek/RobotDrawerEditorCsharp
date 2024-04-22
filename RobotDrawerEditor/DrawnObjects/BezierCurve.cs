using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor.DrawnObjects
{
    public abstract class BezierCurve : DrawnObject
    {
        public List<ControlPoint> ControlPoints { get; protected set; } = new List<ControlPoint>();

        public abstract PointF PointAtCurve(float t);

        public double DistanceFromPoint(PointF point, float from = 0, float to = 1, int depth = 1)
        {
            int chunks = 10;
            PointF previous = PointAtCurve(0);
            PointF first, second;
            Tuple<float, double> closestT = new Tuple<float, double>(0, double.MaxValue);  // Tuple(t, distance)

            for (int i = 0; i < chunks; i++)
            {
                float t = i / (float)chunks;

                first = previous;
                second = PointAtCurve(t + 1f / chunks);

                double distance = new StraightLine(first, second, Color.Black).DistanceFromPoint(point);

                if (distance < closestT.Item2)
                    closestT = new Tuple<float, double>(t, distance);

                //Console.WriteLine($"closestT = {closestT}, first = {first}, second = {second}, mouse position = {point}");
                //Console.WriteLine($"from = {t}, to = {t + 1f / chunks}");
                previous = second;
            }

            return closestT.Item2;
        }

        public override void ComputeBoundingRectangleF()
        {
            Tuple<float, float, float, float> minMaxValues = Geometry.GetMinAndMaxValuesOfBezierCurve(this);

            BoundingRectangle = new RectangleF(minMaxValues.Item1,
                                               minMaxValues.Item2,
                                               minMaxValues.Item3 - minMaxValues.Item1,
                                               minMaxValues.Item4 - minMaxValues.Item2);
        }

        public override void SetPositionAndShapeFromPoints(PointF point0, PointF point1)
        {
            // netreba
        }
    }
}
