using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotDrawerEditor.DrawnObjects;

namespace RobotDrawerEditor
{
    public partial class CanvasUserControl : Panel
    {
        private ProgramLogic programLogic;
        private PointF showPoint = new PointF(-1, -1);

        public CanvasUserControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void InitializeLogic(ProgramLogic pl)
        {
            programLogic = pl;
        }

        public void SetShowPoint(PointF point)
        {
            showPoint = point;
        }

        private void CanvasUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (IsInDesignMode(this))
                return;

            DrawPaper(e, new Pen(Color.Black, 1));

            View view = ProgramLogic.View;
            List<DrawnObject> objectsToDraw = programLogic.GetObjectsToDraw();

            Pen pen = new Pen(Color.Blue, 1);

            if (objectsToDraw.Any())
            {
                foreach (DrawnObject drawnObject in objectsToDraw)
                {
                    drawnObject.PaintObject(pen, e, programLogic);
                }
            }

            if (showPoint != new PointF(-1, -1))
            {
                PointF showPointView = view.GlobalToViewPoint(showPoint).FlipYAxis();

                e.Graphics.DrawEllipse(new Pen(Color.Black, 1),
                                       new Rectangle((int)showPointView.X - 2, (int)showPointView.Y - 2, 4, 4));
            }

            MainForm.ProgramLogic.Tool.Paint(pen, e, MainForm.ProgramLogic);
            pen.Dispose();
        }
        
        private void DrawPaper(PaintEventArgs e, Pen pen)
        {
            // View view = programLogic.View;
            RectangleF rectangle = programLogic.Canvas.Paper.GetBoundingRectangle();

            MyRectangle globalMyRect = MyRectangle.FromRectangleF(rectangle, Color.Black);
            MyRectangle viewMyRect = ProgramLogic.View.GlobalToViewObject(globalMyRect) as MyRectangle;

            //Rectangle r = Rectangle.Round(viewMyRect.ToRectangleF());
            Rectangle r = Rectangle.Round((viewMyRect.FlipYAxis(ProgramLogic.View.CanvasUCHeight) as MyRectangle).ToRectangleF());

            e.Graphics.DrawRectangle(pen, r);
        }

        private static bool IsInDesignMode(IComponent component)
        {
            bool res = false;

            if (null != component)
            {
                ISite site = component.Site;

                if (null != site)
                {
                    res = site.DesignMode;
                }
                else if (component is Control control)
                {
                    IComponent parent = control.Parent;
                    res = IsInDesignMode(parent);
                }
            }

            return res;
        }

        public bool MouseIsOver()
        {
            return ClientRectangle.Contains(PointToClient(Cursor.Position));
        }
    }
}
