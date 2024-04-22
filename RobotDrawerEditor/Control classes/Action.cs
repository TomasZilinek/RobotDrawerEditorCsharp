using RobotDrawerEditor.DrawnObjects;
using System.Drawing;

namespace RobotDrawerEditor
{
    public class Action
    {
        public string Description { get; private set; }
        public Image CorrespondingImage { get; private set; }

        public Action(string desc, Image img)
        {
            Description = desc;
            CorrespondingImage = img;
        }
    }

    public class MainActionInherited : Action
    {
        public MyRectangle rectangle;

        public MainActionInherited(string desc, Image img, MyRectangle rect)
            : base(desc, img)
        {
            rectangle = rect;
        }
    }

    public class ActionChangeBrightness : MainActionInherited
    {
        public int NewBrightness { get; private set; }

        public ActionChangeBrightness(Image img, int brightness, MyRectangle rect)
            : base("Change brightness", img, rect)
        {
            NewBrightness = brightness;
        }
    }

    public class ActionApplyColorFilter : MainActionInherited
    {
        public Color FilterColor { get; private set; }

        public ActionApplyColorFilter(Image img, Color color, MyRectangle rect)
            : base("Apply color filter", img, rect)
        {
            FilterColor = color;
        }
    }

    public class ActionBlur : MainActionInherited
    {
        public int Radius { get; private set; }

        public ActionBlur(Image img, int radius, MyRectangle rect) : base("Apply blur", img, rect)
        {
            Radius = radius;
        }
    }

    public class ActionCrop : MainActionInherited
    {
        public ActionCrop(Image img, MyRectangle rect)
            : base("Apply crop", img, rect)
        {

        }
    }
}
