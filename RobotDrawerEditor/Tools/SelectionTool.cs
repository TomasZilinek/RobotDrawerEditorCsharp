using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public class SelectionTool : Tool
    {
        public PointF ClickedGlobalPosition { get; private set; }
        private static float[] dashLineValues = { 4, 4 };

        public SelectionTool()
        {

        }

        public override void MouseLeftButtonDown()
        {
            base.MouseLeftButtonDown();

            // selection //
            DrawnObject selectedDrawObject = MainForm.ProgramLogic.GetSelectedDrawnObject();

            if ((selectedDrawObject == null || !selectedDrawObject.HoveredOverThisOrAnySelectionPoint()) &&
                !MainForm.ProgramLogic.HoveredOverAnyDrawnObject())
            {
                ClickedGlobalPosition = Mouse.CurrentGlobalPosition;
            }

            // drawn objects management //
            PointF globalPosition = Mouse.CurrentGlobalPosition;

            foreach (DrawnObject obj in MainForm.ProgramLogic.Canvas.GetObjectsInView())
            {
                obj.MouseDown(globalPosition, MainForm.ProgramLogic);
            }
        }

        public override void MouseLeftButtonUp()
        {
            // selection //
            if (ClickedGlobalPosition != PointF.Empty)
                MainForm.ProgramLogic.MouseSelectionConfirmed(GetSelectionRectangle());

            ClickedGlobalPosition = PointF.Empty;

            // drawn objects management //
            PointF globalPosition = Mouse.CurrentGlobalPosition;

            foreach (DrawnObject obj in MainForm.ProgramLogic.Canvas.GetObjectsInView())
            {
                obj.MouseUp(globalPosition, MainForm.ProgramLogic);
            }

            base.MouseLeftButtonUp();
        }

        public override void MouseMove()
        {
            PointF globalPosition = Mouse.CurrentGlobalPosition;

            foreach (DrawnObject obj in MainForm.ProgramLogic.Canvas.GetObjectsInView())
            {
                obj.MouseMove(globalPosition, MainForm.ProgramLogic);
            }
        }

        public RectangleF GetSelectionRectangle()
        {
            return Geometry.GetRectangleFromPoints(ClickedGlobalPosition, Mouse.CurrentGlobalPosition);
        }

        public override void Paint(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            if (!ProgramLogic.Mouse.LeftButtonDown || ClickedGlobalPosition == PointF.Empty)
                return;

            View view = ProgramLogic.View;
            Pen myPen = pen.Clone() as Pen;
            RectangleF selectionRectangle = GetSelectionRectangle();

            myPen.Color = Color.Black;
            myPen.DashStyle = DashStyle.DashDot;
            myPen.Width = 1;
            myPen.DashPattern = dashLineValues;

            MyRectangle selectionRectAsMyRectangle = MyRectangle.FromRectangleF(selectionRectangle, Color.Black);
            MyRectangle viewRect = view.GlobalToViewObject(selectionRectAsMyRectangle) as MyRectangle;
            viewRect = viewRect.FlipYAxis(view.CanvasUCHeight) as MyRectangle;
            viewRect = new MyRectangle(viewRect.X - 1, viewRect.Y - 1,
                                       viewRect.Width + 1, viewRect.Height + 1,
                                       viewRect.Color);
            e.Graphics.DrawRectangle(myPen, Geometry.ToRectangle(viewRect.ToRectangleF()));

            myPen.Dispose();
        }

        protected override void AddDrawnObjectToCanvas()
        {
            // does nothing in selection tool
        }
    }
}
