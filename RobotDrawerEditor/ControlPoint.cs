using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor
{
    public class ControlPoint
    {
        public PointF Position { get; set; }

        public ControlPoint(PointF point)
        {
            Position = point;
        }

        public ControlPoint(float x, float y) : this(new PointF(x, y))
        {

        }

        public float X
        {
            get
            {
                return Position.X;
            }

            set
            {
                Position = new PointF(value, Position.Y);
            }
        }

        public float Y
        {
            get
            {
                return Position.Y;
            }

            set
            {
                Position = new PointF(Position.X, value);
            }
        }

        public PointF FlipYAxis()
        {
            return new ControlPoint(X, ProgramLogic.View.CanvasUCHeight - Y);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ControlPoint p = (ControlPoint)obj;
                return (Position.X == p.Position.X) && (Position.Y == p.Position.Y);
            }
        }

        public override int GetHashCode()
        {
            return ((int)Position.X << 2) ^ (int)Position.Y;
        }

        public static implicit operator PointF(ControlPoint p) => p.Position;
        public static implicit operator ControlPoint(PointF p) => new ControlPoint(p);
    }
}
