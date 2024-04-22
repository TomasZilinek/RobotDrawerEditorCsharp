using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor.DrawnObjects
{
    public abstract class DrawnObject : ICloneable
    {
        protected float width, height;
        public bool Selected { get; protected set; } = false;
        public bool Clicked { get; protected set; } = false;
        public virtual Color Color { get; set; } = Color.Black;

        public PointF Position
        {
            get => BoundingRectangle.Location;
            set => MoveDrawnObject(value.X - Position.X, value.Y - Position.Y);
        }

        public virtual float Width
        {
            get
            {
                return BoundingRectangle.Width;
            }

            protected set
            {
                width = value;
            }
        }

        public virtual float Height
        {
            get
            {
                return BoundingRectangle.Height;
            }

            protected set
            {
                height = value;
            }
        }

        public RectangleF BoundingRectangle { get; protected set; } = RectangleF.Empty;
        protected PointF clickedPosition = new PointF();
        protected PointF selectedSelectionPointPreviousPosition = PointF.Empty;
        protected int selectedSelectionPointIndex = -1;
        private static float[] dashLineValues = { 4, 4 };
        protected int[] hiddenSelectionPointsIndexes = new int[] { };
        public List<SelectionPoint> SelectionPoints { get; private set; } = new List<SelectionPoint>();
        public static readonly Color selectionRectangleColor = Color.Black;
        public static float globalAllowedHoverDistance;

        protected static readonly Dictionary<int, int[]> selectionPointsGroupsDict = new Dictionary<int, int[]>();
        protected static int[] posXaffectingSelectionPointsIndexes = new int[] { 0, 3, 5 };
        protected static int[] posYaffectingSelectionPointsIndexes = new int[] { 0, 1, 2 };
        protected static int[] widthAffectingSelectionPointsIndexes = new int[] { 0, 3, 5, 2, 4, 7 };
        protected static int[] heightAffectingSelectionPointsIndexes = new int[] { 0, 1, 2, 5, 6, 7 };

        public DrawnObject()
        {
            selectionPointsGroupsDict[0] = new int[] { 0, 1, 2, 3, 5 };
            selectionPointsGroupsDict[1] = new int[] { 0, 1, 2 };
            selectionPointsGroupsDict[2] = new int[] { 0, 1, 2, 4, 7 };
            selectionPointsGroupsDict[3] = new int[] { 0, 3, 5 };
            selectionPointsGroupsDict[4] = new int[] { 2, 4, 7 };
            selectionPointsGroupsDict[5] = new int[] { 0, 3, 5, 6, 7 };
            selectionPointsGroupsDict[6] = new int[] { 5, 6, 7 };
            selectionPointsGroupsDict[7] = new int[] { 2, 4, 5, 6, 7 };

            for (int i = 0; i < 8; i++)
                SelectionPoints.Add(new SelectionPoint(i));
        }
        protected abstract void InitializeAfterConstructor();
        protected abstract DrawnObject Flip(float viewHeight);

        public virtual DrawnObject FlipYAxis(float viewHeight)
        {
            return Flip(viewHeight);
        }

        protected void PlaceSelectionPointsOnBoundingRectangle(RectangleF boundingRect = new RectangleF())
        {
            if (boundingRect == new RectangleF())
                boundingRect = BoundingRectangle;

            SelectionPoints[0].Position = boundingRect.Location;
            SelectionPoints[1].Position = PointF.Add(boundingRect.Location, new SizeF(boundingRect.Width / 2, 0));
            SelectionPoints[2].Position = PointF.Add(boundingRect.Location, new SizeF(boundingRect.Width, 0));
            SelectionPoints[3].Position = PointF.Add(boundingRect.Location, new SizeF(0, boundingRect.Height / 2));
            SelectionPoints[4].Position = PointF.Add(boundingRect.Location, new SizeF(boundingRect.Width, boundingRect.Height / 2));
            SelectionPoints[5].Position = PointF.Add(boundingRect.Location, new SizeF(0, boundingRect.Height));
            SelectionPoints[6].Position = PointF.Add(boundingRect.Location, new SizeF(boundingRect.Width / 2, boundingRect.Height));
            SelectionPoints[7].Position = PointF.Add(boundingRect.Location, new SizeF(boundingRect.Width, boundingRect.Height));
        }


        /// <summary>
        /// Meant to be overriden with a call to base at the beginning of the overriding method
        /// </summary>
        public abstract bool HoveredOver(PointF mousePosition);

        public abstract void SetPositionAndShapeFromPoints(PointF point0, PointF point1);

        public virtual void PaintObject(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            PaintShape(pen, e, programLogic);

            if (Selected)
            {
                PaintSelectionRectangle(pen, e, programLogic);
                PaintSelectionPoints(pen, e, programLogic);
            }
        }

        public abstract void ComputeBoundingRectangleF(); 
        protected abstract void PaintShape(Pen pen, PaintEventArgs e, ProgramLogic programLogic);
        protected virtual void PaintSelectionRectangle(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            View view = ProgramLogic.View;
            Pen myPen = pen.Clone() as Pen;
            RectangleF rectF = BoundingRectangle;

            myPen.Color = selectionRectangleColor;
            myPen.DashStyle = DashStyle.DashDot;
            myPen.Width = 1;
            myPen.DashPattern = dashLineValues;

            MyRectangle selectionRectAsMyRectangle = MyRectangle.FromRectangleF(rectF, selectionRectangleColor);
            MyRectangle viewRect = view.GlobalToViewObject(selectionRectAsMyRectangle) as MyRectangle;
            viewRect = viewRect.FlipYAxis(view.CanvasUCHeight) as MyRectangle;
            viewRect = new MyRectangle(viewRect.X - 1, viewRect.Y - 1,
                                       viewRect.Width + 1, viewRect.Height + 1,
                                       viewRect.Color);

            e.Graphics.DrawRectangle(myPen, Geometry.ToRectangle(viewRect.ToRectangleF()));

            myPen.Dispose();
        }

        protected virtual void PaintSelectionPoints(Pen pen, PaintEventArgs e, ProgramLogic programLogic)
        {
            if (Selected)
            {
                View view = ProgramLogic.View;
                Color selectionPointColor;

                for (int i = 0; i < SelectionPoints.Count; i++)
                {
                    SelectionPoint cPoint = SelectionPoints[i];

                    if (!hiddenSelectionPointsIndexes.Contains(i) && view.PointInView(cPoint.Position))
                    {
                        selectionPointColor = Color.Blue;

                        if (cPoint.HoveredOver())
                            selectionPointColor = Color.Red;

                        pen.Color = selectionPointColor;

                        PointF CpointViewPos = view.GlobalToViewPoint(cPoint.Position).FlipYAxis();
                        int selectionPointsRadius = (int)view.GlobalToViewLength(SelectionPoint.NEAR_POINT_DISTANCE);

                        e.Graphics.DrawRectangle(pen, new Rectangle((int)CpointViewPos.X - selectionPointsRadius,
                                                                    (int)CpointViewPos.Y - selectionPointsRadius,
                                                                    2 * selectionPointsRadius,
                                                                    2 * selectionPointsRadius));
                    }
                }
            }
        }

        public virtual void Select()
        {
            Selected = true;

            MainForm.ProgramLogic.DecideColorPickerButtonEnabled();
        }

        public virtual void Deselect()
        { 
            Selected = false;

            MainForm.ProgramLogic.DecideColorPickerButtonEnabled();
        }

        public bool HoveredOverAnySelectionPoint(out SelectionPoint hoveredSelectionPoint)
        {
            foreach (SelectionPoint cp in SelectionPoints)
                if (cp.HoveredOver())
                {
                    hoveredSelectionPoint = cp;
                    return true;
                }

            hoveredSelectionPoint = null;
            return false;
        }

        public void MoveDrawnObject(float xDiff, float yDiff)
        {
            foreach (SelectionPoint selectionPoint in SelectionPoints)
            {
                selectionPoint.X += xDiff;
                selectionPoint.Y += yDiff;
            }

            if (this is BezierCurve bezierCurve)
            {
                for (int i = 0; i < bezierCurve.ControlPoints.Count; i++)
                    bezierCurve.ControlPoints[i] = ((PointF)bezierCurve.ControlPoints[i]).Add(new PointF(xDiff, yDiff));
            }
            else if (this is ConnectedBezierCurve connectedBezierCurve)
            {
                foreach (BezierCurve curve in connectedBezierCurve.Curves)
                    curve.MoveDrawnObject(xDiff, yDiff);
            }
            else
            {
                ChangeShapeBySelectionPoints();
            }

            ComputeBoundingRectangleF();
        }

        public virtual void MouseDown(PointF globalMousePosition, ProgramLogic pl)
        {
            DrawnObject otherAlreadySelectedDrawnObject = pl.GetSelectedDrawnObject();

            if (otherAlreadySelectedDrawnObject == this)
                otherAlreadySelectedDrawnObject = null;

            // select selection point
            if (Selected)
            {
                for (int i = 0; i < SelectionPoints.Count; i++)
                {
                    SelectionPoint cPoint = SelectionPoints[i];

                    if (cPoint.HoveredOver())
                    {
                        selectedSelectionPointIndex = i;
                        break;
                    }
                }
            }

            bool hoveredOver = HoveredOver(globalMousePosition);

            if (hoveredOver && otherAlreadySelectedDrawnObject == null)
            {
                Clicked = true;
                clickedPosition = globalMousePosition;
            }

            if (!Selected && hoveredOver && otherAlreadySelectedDrawnObject == null)
            {
                Select();
            }
            else if (Selected && !hoveredOver && !HoveredOverAnySelectionPoint(out _))
            {
                Deselect();
            }

            ProgramLogic.MainForm.canvasUserControl1.Invalidate();
        }

        public virtual void MouseUp(PointF globalMousePosition, ProgramLogic pl)
        {
            selectedSelectionPointIndex = -1;
            Clicked = false;
        }

        public virtual void MouseMove(PointF globalMousePosition, ProgramLogic pl)
        {
            bool hoveredOverAnySelectionPoint = HoveredOverAnySelectionPoint(out SelectionPoint hoveredSelectionPoint);
            bool hoveredOver = HoveredOver(globalMousePosition);

            if (selectedSelectionPointIndex != -1)
            {
                selectedSelectionPointPreviousPosition = SelectionPoints[selectedSelectionPointIndex].Position;

                SetSelectedSelectionPointPositionFromMousePositionWhenDragged(globalMousePosition);
                AdjustOtherSelectionPoints();
                ChangeShapeBySelectionPoints();
                ComputeBoundingRectangleF();
            }
            else if (Clicked)
            {
                PointF positionsDiff = Geometry.PointsSubtraction(globalMousePosition, clickedPosition);
                clickedPosition = globalMousePosition;

                if (this is BezierCurve || this is ConnectedBezierCurve)
                {
                    MoveDrawnObject(Mouse.CurrentGlobalPosition.X - Mouse.PreviousGlobalPosition.X,
                                    Mouse.CurrentGlobalPosition.Y - Mouse.PreviousGlobalPosition.Y);
                }
                else
                {
                    foreach (SelectionPoint cPoint in SelectionPoints)
                        cPoint.Position = Geometry.PointsAddition(cPoint.Position, positionsDiff);
                    
                    ChangeShapeBySelectionPoints();
                }

                ComputeBoundingRectangleF();
                ProgramLogic.Instance.DrawnObjectMoved();
            }

            DrawnObject otherAlreadySelectedDrawObject = pl.GetSelectedDrawnObject();

            if (otherAlreadySelectedDrawObject == this)
                otherAlreadySelectedDrawObject = null;

            if (Selected && hoveredOverAnySelectionPoint)
                Cursor.Current = GetHoverCursorFromSelectionPointIndex(hoveredSelectionPoint.Index);
            else if (otherAlreadySelectedDrawObject == null ||
                     !otherAlreadySelectedDrawObject.HoveredOverThisOrAnySelectionPoint())
            {
                if (hoveredOver && Cursor.Current != Cursors.WaitCursor)
                    Cursor.Current = Cursors.SizeAll;
            }
        }

        private Cursor GetHoverCursorFromSelectionPointIndex(int selectionPointIndex)
        {
            if ((new int[] { 0, 7 }).Contains(selectionPointIndex))
                return Cursors.SizeNESW;
            else if ((new int[] { 1, 6 }).Contains(selectionPointIndex))
                return Cursors.SizeNS;
            else if ((new int[] { 2, 5 }).Contains(selectionPointIndex))
                return Cursors.SizeNWSE;
            else
                return Cursors.SizeWE;
        }

        protected virtual void SetSelectedSelectionPointPositionFromMousePositionWhenDragged(PointF globalMousePosition)
        {
            SelectionPoint selectedSelectionPoint = SelectionPoints[selectedSelectionPointIndex];
            selectedSelectionPoint.Position = globalMousePosition;
        }

        protected virtual void AdjustOtherSelectionPoints()
        {
            if (selectedSelectionPointIndex == -1)
                return;

            RectangleF newBoundingRectangle = GetBoundingRectangleAfterSelectionPointWasMoved(selectedSelectionPointIndex);
            PlaceSelectionPointsOnBoundingRectangle(newBoundingRectangle);
        }

        private RectangleF GetBoundingRectangleAfterSelectionPointWasMoved(int movedSelectionPointIndex)
        {
            float posX, posY, width, height;

            SelectionPoint movedSelectionPoint = SelectionPoints[movedSelectionPointIndex];
            RectangleF currentBoundingRectangle = BoundingRectangle;

            if (posXaffectingSelectionPointsIndexes.Contains(movedSelectionPointIndex))
                posX = movedSelectionPoint.X;
            else
                posX = currentBoundingRectangle.X;

            if (posYaffectingSelectionPointsIndexes.Contains(movedSelectionPointIndex))
                posY = movedSelectionPoint.Y;
            else
                posY = currentBoundingRectangle.Y;

            if (widthAffectingSelectionPointsIndexes.Contains(movedSelectionPointIndex))
            {
                if (!posXaffectingSelectionPointsIndexes.Contains(movedSelectionPointIndex))  // one of the right points
                    width = movedSelectionPoint.Position.X - posX;
                else
                    width = currentBoundingRectangle.X + currentBoundingRectangle.Width - posX;
            }
            else
                width = currentBoundingRectangle.Width;

            if (heightAffectingSelectionPointsIndexes.Contains(movedSelectionPointIndex))
            {
                if (!posYaffectingSelectionPointsIndexes.Contains(movedSelectionPointIndex))  // one of the right points
                    height = movedSelectionPoint.Position.Y - posY;
                else
                    height = currentBoundingRectangle.Y + currentBoundingRectangle.Height - posY;
            }
            else
                height = currentBoundingRectangle.Height;

            return new RectangleF(posX, posY, width, height);
        }

        protected bool CheckSelectionPointsRelativeRectPosition(int p1Index, int p2Index, bool checkHorizontal)
        {
            HashSet<int> p1GroupSet = new HashSet<int>(selectionPointsGroupsDict[p1Index]);
            HashSet<int> p2GroupSet = new HashSet<int>(selectionPointsGroupsDict[p2Index]);

            HashSet<int> intersection = new HashSet<int>(p1GroupSet);
            intersection.IntersectWith(p2GroupSet);
            int[] intersectionArray = intersection.ToArray();

            if (checkHorizontal)
                return Enumerable.SequenceEqual(intersectionArray, selectionPointsGroupsDict[1]) ||
                       Enumerable.SequenceEqual(intersectionArray, selectionPointsGroupsDict[6]);
            else
                return Enumerable.SequenceEqual(intersectionArray, selectionPointsGroupsDict[3]) ||
                       Enumerable.SequenceEqual(intersectionArray, selectionPointsGroupsDict[4]);
        }

        /// <summary>
        /// Sets points and selection points properties after their value were changed in List in MouseMove
        /// </summary>
        protected abstract void ChangeShapeBySelectionPoints();

        public int GetOppositeSelectionPointIndex(int selectionPointIndex)
        {
            return 7 - selectionPointIndex;
        }

        public bool HoveredOverThisOrAnySelectionPoint()
        {
            PointF globalMousePosition = Mouse.CurrentGlobalPosition;

            return HoveredOver(globalMousePosition) || HoveredOverAnySelectionPoint(out _);
        }

        public abstract object Clone();
    }
}
