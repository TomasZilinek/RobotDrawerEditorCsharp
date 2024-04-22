using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor
{
    public class SelectionPoint
    {
        public PointF Position { get; set; }
        public bool Selected = false;
        public int Index { get; private set; }

        public static float NEAR_POINT_DISTANCE = 3;

        

        public SelectionPoint(int index, PointF position)
        {
            Position = position;
            Index = index;
        }

        public SelectionPoint(int index) : this(index, new PointF(0, 0))
        {

        }

        public SelectionPoint(int index, float x, float y) : this(index, new PointF(x, y))
        {

        }

        public SelectionPoint(int index, SelectionPoint point) : this(index, point.Position)
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

        public bool HoveredOver()
        {
            PointF globalMousePosition = Mouse.CurrentGlobalPosition;

            return Position.TwoPointsDistance(globalMousePosition) <= NEAR_POINT_DISTANCE;
        }

        public override string ToString()
        {
            return $"ControlPoint [X={Position.X}, Y={Position.Y}]";
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SelectionPoint p = (SelectionPoint)obj;
                return (Position.X == p.Position.X) && (Position.Y == p.Position.Y);
            }
        }

        public override int GetHashCode()
        {
            return ((int)Position.X << 2) ^ (int)Position.Y;
        }
    }
}
