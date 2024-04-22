using RobotDrawerEditor.Control_classes;
using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public class ProgramLogic
    {
        public static ProgramLogic Instance { get; private set; }
        public static MainForm MainForm { get; private set; }
        public FileManager FileManager { get; private set; }
        public static Mouse Mouse { get; private set; }
        private UndoRedo undoRedo = new UndoRedo();
        public Canvas Canvas { get; private set; }
        public static View View { get; private set; }
        public Color DrawingColor { get; set; } = Color.Black;
        public Tool Tool { get; set; }
        public DrawnObject copiedDrawnObject { get; private set; }

        public ProgramLogic()
        {
            Instance = this;
            MainForm = null;
            Tool = new SelectionTool();
            Mouse = new Mouse(Control.MousePosition);
            Canvas = new Canvas();
            FileManager = new FileManager();

            /*
            Canvas.AddDrawnObject(new StraightLine(70, 120, 160, 70, Color.Blue));
            Canvas.AddDrawnObject(new Circle(new PointF(200, 200), 50, Color.Red));
            Canvas.AddDrawnObject(new MyRectangle(10, 90, 40, 60, Color.LightGreen));
            Canvas.AddDrawnObject(new Ellipse(new PointF(40, 40), 90, 30, Color.Black));

            //Canvas.AddDrawnObject(new BezierCurve(new PointF(140, 150), new PointF(130, 360),
            //                                      new PointF(220, 260), new PointF(90, 180)));
            Canvas.AddDrawnObject(new BezierCurve(new PointF(-20, 170), new PointF(190, 310),
                                                  new PointF(30, 310), new PointF(60, 160), Color.Black));
            */

            /*
            Canvas.AddDrawnObject(new BezierCurve4(new BezierCurve3(new PointF(0, 0),
                                                                    new PointF(50, 50),
                                                                    new PointF(100, 0),
                                                                    Color.Black)));
            */

            /*
            Canvas.AddDrawnObject(new BezierCurve4(new PointF(30, 0),
                                                   new PointF(180, 50),
                                                   new PointF(30, 60),
                                                   new PointF(120, -10),
                                                   Color.Black));
            */
            
            /*
            BezierCurve3 curve = new BezierCurve3(new PointF(200, 0),
                                                   new PointF(250, 50),
                                                   new PointF(300, 0),
                                                   Color.Black);
            Canvas.AddDrawnObject(curve);
            */
        }
        
        public void Initialize(MainForm mainForm)
        {
            MainForm = mainForm;

            CanvasUserControl canvausUC = MainForm.canvasUserControl1;
            float viewWidth = 800;
            float viewHeight = viewWidth / canvausUC.Width * canvausUC.Height;

            float viewX = Canvas.Paper.Width / 2 - viewWidth / 2;
            float viewY = Canvas.Paper.Height / 2 - viewHeight / 2;

            View = new View(viewX, viewY, viewWidth, viewHeight, canvausUC.Width, canvausUC.Height, this);
        }
        
        // sample
        /*
        public Image ChangeBrightness(int newBrightness, Rectangle rect, bool preview = false)
        {
            if (preview)
            {
                Image changed = ImageProcessingFunctions.ChangeBrightness(
                                    newBrightness, new Bitmap(ActiveEditor.EditedImage), rect);

                ActiveEditor.PictureBox.Image = changed;

                return changed;
            }
            else
            {
                var toReturn = ActiveEditor.ChangeBrightness(newBrightness, rect);
                UpdateMainFormUndoRedoButtons();

                return toReturn;
            }
        }
        */

        public void StartEditingNewFile(string fileName)
        {
            // ActiveEditor.StartEditingNewImage(img);
        }

        public void UpdateMainFormUndoRedoButtons()
        {
            bool undoAvailable = UndoAvailable();
            bool redoAvailable = RedoAvailable();

            MainForm.UndoToolStripButton.Enabled = undoAvailable;
            MainForm.drawingColorButton.Enabled = redoAvailable;

            MainForm.UndoButton.Enabled = undoAvailable;
            MainForm.RedoButton.Enabled = redoAvailable;
        }

        public object Undo()
        {
            var undone = undoRedo.Undo();

            if (undone == null)
            {
                // PictureBox.Image = new Bitmap(OriginalImage);
            }
            else
            {
                // PictureBox.Image = new Bitmap(undone);
            }

            UpdateMainFormUndoRedoButtons();

            return new object();
        }

        public object Redo()
        {
            var v = undoRedo.Redo();

            //PictureBox.Image = new Bitmap(img);

            UpdateMainFormUndoRedoButtons();

            return v;
        }

        public bool UndoAvailable()
        {
            return undoRedo.UndoAvailable();
        }

        public bool RedoAvailable()
        {
            return undoRedo.RedoAvailable();
        }

        public List<DrawnObject> GetObjectsToDraw()
        {
            // return Canvas.GetObjectsInView();
            return Canvas.DrawnObjects;
        }

        public void KeyPressedMainForm(Keys key)
        {
            Console.WriteLine("'{0}' key pressed", key);

            float multiple = 0.2f;

            if (key == Keys.Up)
                View.MoveByGlobalDistanceMultiple(multiple, ArrowDirection.Up);
            else if (key == Keys.Left)
                View.MoveByGlobalDistanceMultiple(multiple, ArrowDirection.Left);
            else if (key == Keys.Down)
                View.MoveByGlobalDistanceMultiple(multiple, ArrowDirection.Down);
            else if (key == Keys.Right)
                View.MoveByGlobalDistanceMultiple(multiple, ArrowDirection.Right);
            else if (key == Keys.Delete)
            {
                Canvas.DeleteSelectedDrawObject();
                FileManager.ProgressSaved = false;
                DecideColorPickerButtonEnabled();
            }
            else if (key == Keys.C && MainForm.ControlPressed)
            {
                CopySelectedItem();
            }
            else if (key == Keys.V && MainForm.ControlPressed)
            {
                PasteSelectedItem();
            }
        }

        public void DecideColorPickerButtonEnabled()
        {
            MainForm.colorPickerButton.Enabled = GetSelectedDrawnObject() != null;
        }

        public void CanvasUCscroll(bool controlPressed, bool shiftPressed, int delta, Point position)
        {
            if (controlPressed)
            {
                if (delta > 0)
                {
                    View.ZoomIn(1.25f, position);
                }
                else if (delta < 0)
                {
                    View.ZoomOut(0.8f, position);
                }

                View.ChangeNearObjectDistances();
            }
            else if (shiftPressed)
            {
                ArrowDirection direction = delta > 0 ? ArrowDirection.Right : ArrowDirection.Left;
                View.MoveByScreenDistance(View.CanvasUCHeight / 10f, direction);
            }
            else
            {
                ArrowDirection direction = delta > 0 ? ArrowDirection.Up : ArrowDirection.Down;
                View.MoveByScreenDistance(View.CanvasUCHeight / 10f, direction);
            }
        }

        public void CanvasUCmouseDown(MouseEventArgs e)
        {
            Mouse.MouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                Tool.MouseLeftButtonDown();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Tool is BezierCurveTool bezierCurveTool && !Mouse.Instance.LeftButtonDown)
                    bezierCurveTool.MouseRightButtonDown();
            }
        }

        public void CanvasUCmouseUp(MouseEventArgs e)
        {
            Mouse.MouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                Tool.MouseLeftButtonUp();
            }
        }

        public void CanvasUCmouseMove(MouseEventArgs e)
        {
            Mouse.MouseMove(new Point(e.X, e.Y));
            MainForm.mouseXPosLabel.Text = "X: " + Mouse.CurrentGlobalPosition.X.ToString("0.00");
            MainForm.mouseYPosLabel.Text = "Y: " + Mouse.CurrentGlobalPosition.Y.ToString("0.00");

            Tool.MouseMove();
            MainForm.canvasUserControl1.Invalidate();
        }

        public DrawnObject GetSelectedDrawnObject()
        {
            return Canvas.DrawnObjects.Where(x => x.Selected).FirstOrDefault();
        }

        public void MouseSelectionConfirmed(RectangleF globalSelectionRect)
        {
            bool onlyClick = false;

            if (Tool is SelectionTool selectionTool && selectionTool.ClickedGlobalPosition == Mouse.CurrentGlobalPosition)
                onlyClick = true;

            if (!onlyClick)
            {
                List<DrawnObject> inView = Canvas.GetObjectsInView();
                IEnumerable<DrawnObject> insideSelectionRect = inView.Where(x => globalSelectionRect.Contains(x.BoundingRectangle));

                // foreach (DrawnObject drawnObject in Canvas)

                // DrawnObject obj = Canvas.DrawnObjects[0];
                DrawnObject selected = null;

                if (insideSelectionRect.Any())
                {
                    selected = insideSelectionRect.OrderBy(x => Geometry.GetRectangleCentre(globalSelectionRect)
                                                                        .TwoPointsDistance(Geometry.GetRectangleCentre(x.BoundingRectangle))
                                                          )
                                                  .First();
                }

                if (selected != null)
                {
                    DeselectAllDrawnObjects();

                    selected.Select();
                    MainForm.canvasUserControl1.Invalidate();
                }
            }
            else
            {
                DrawnObject selected = MainForm.ProgramLogic.GetSelectedDrawnObject();

                if (selected != null && !selected.HoveredOverThisOrAnySelectionPoint())
                    selected.Deselect();
            }
        }

        public void DeselectAllDrawnObjects()
        {
            foreach (DrawnObject drawObject in Canvas.GetObjectsInView())
                drawObject.Deselect();
        }

        public bool HoveredOverAnyDrawnObject()
        {
            PointF globalMousePosition = Mouse.CurrentGlobalPosition;

            return Canvas.GetObjectsInView().Any(x => x.HoveredOver(globalMousePosition));
        }

        public void ChooseTool(Tool tool)
        {
            if (!(tool is SelectionTool))
            {
                DeselectAllDrawnObjects();
                MainForm.canvasUserControl1.Invalidate();
            }

            Tool = tool;

            if (tool is SelectionTool selectionTool)
            {

            }
            else if (tool is StraightLineTool straightLineTool)
            {

            }
            else if (tool is RectangleTool rectangleTool)
            {

            }
            else if (tool is EllipseTool ellipseTool)
            {

            }
            else if (tool is BezierCurveTool bezierCurveTool)
            {

            }
            else if (tool is EditPathsTool pathsTool)
            {

            }
        }

        public void SetSelectedDrawnObjectCOlor(Color color)
        {
            DrawnObject drawnObject = GetSelectedDrawnObject();

            drawnObject.Color = color;
            MainForm.canvasUserControl1.Invalidate();
        }

        public void ClearEverythingForNewFile()
        {

        }

        public void NewFileButtonClicked()
        {
            if (FileManager.NeedsSaving())
                if (!FileManager.SaveFile(false, out _))
                    return;

            ClearEverythingForNewFile();
        }

        public void LoadFileButtonClicked()
        {
            if (FileManager.NeedsSaving())
            {
                string message = "Do you want to save changes?";
                string caption = MainForm.Instance.Text;
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;

                DialogResult messageBoxDialogResult = MessageBox.Show(message, caption, buttons);

                if (messageBoxDialogResult == DialogResult.Yes)
                {
                    FileManager.SaveFile(false, out _);
                }
            }

            if (!FileManager.LoadFile(out string fileName) && fileName != "" && fileName != null)
                MessageBox.Show($"Could not load file '{fileName}'.");
        }

        public void SaveButtonClicked()
        {
            FileManager.SaveFile(false, out _);
        }

        public void SaveAsButtonClicked()
        {
            FileManager.SaveFile(true, out _);
        }

        public void DrawnObjectMoved()
        {
            FileManager.ProgressSaved = false;
        }

        public void CopySelectedItem()
        {
            DrawnObject selectedDrawnObject = GetSelectedDrawnObject();

            if (selectedDrawnObject != null)
            {
                copiedDrawnObject = selectedDrawnObject.Clone() as DrawnObject;
            }
        }

        public void PasteSelectedItem()
        {
            if (copiedDrawnObject != null)
            {
                DrawnObject newDrawnObject = copiedDrawnObject.Clone() as DrawnObject;

                if (MainForm.Instance.canvasUserControl1.MouseIsOver())
                {
                    newDrawnObject.Position = Mouse.CurrentGlobalPosition.Subtract(new PointF(copiedDrawnObject.Width / 2,
                                                                                              copiedDrawnObject.Height / 2));
                }
                else
                {
                    newDrawnObject.Position = copiedDrawnObject.Position.Add(new PointF(View.GlobalHeight * 0.03f * 3f,
                                                                                        View.GlobalHeight * 0.03f));
                }

                Canvas.AddDrawnObject(newDrawnObject);
                MainForm.canvasUserControl1.Invalidate();
            }
        }
    }
}
