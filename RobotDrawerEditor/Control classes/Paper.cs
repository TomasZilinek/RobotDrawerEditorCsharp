using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor
{
    public class Paper
    {
        public PaperType PaperType { get; private set; }
        public PaperOrientation PaperOrientation { get; private set; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        static readonly int A4Width = 210;
        static readonly int A4Height = 297;

        public Paper(PaperType paperType, float positionX, float positionY,
                     PaperOrientation orientation = PaperOrientation.vertical)
        {
            PaperType = paperType;
            PaperOrientation = orientation;

            X = positionX;
            Y = positionY;

            if (paperType == PaperType.A4)
            {
                Width = A4Width;
                Height = A4Height;
            }
            else if (paperType == PaperType.A3)
            {
                Width = A4Height;
                Height = A4Width * 2;
            }
        }

        public float[] GetActualDimesions()
        {
            if (PaperOrientation == PaperOrientation.horizontal)
            {
                return new float[] { Height, Width };
            }

            return new float[] { Width, Height };
        }

        public RectangleF GetBoundingRectangle()
        {
            return new RectangleF(X, Y, Width, Height);
        }
    }

    public enum PaperType {
        A4, A3
    }

    public enum PaperOrientation
    {
        horizontal, vertical
    }
}
