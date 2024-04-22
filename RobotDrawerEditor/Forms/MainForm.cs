using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public partial class MainForm : ParentForm
    {
        private ToolStripButton[] toolstripToolsButtons;
        private ColorDialog colorDialog = new ColorDialog();
        public static bool ControlPressed { get; private set; } = false;
        public static bool ShiftPressed { get; private set; } = false;

        public static MainForm Instance;
        public static ProgramLogic ProgramLogic { get; private set; }

        public MainForm(ProgramLogic programLogic) : base(null, programLogic, FormTypeEnum.mainForm)
        {
            InitializeComponent();
            Instance = this;
            ProgramLogic = programLogic;

            toolstripToolsButtons = new ToolStripButton[]
            {
                SelectionButton, EditPathsButton, DrawCurvesButton, DrawStraightLinesButton,
                DrawEllipsesButton, DrawRectanglesButton, DrawTextButton
            };

            base.programLogic.Initialize(this);
            canvasUserControl1.InitializeLogic(programLogic);

            WindowState = FormWindowState.Maximized;

            ProgramLogic.View.ChangeNearObjectDistances();
        }

        private void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripButton button in toolstripToolsButtons)
            {
                button.Checked = button.AccessibleName == e.ClickedItem.AccessibleName;
            }
        }

        private void NewFileToolstripMenuItem_Click(object sender, EventArgs e)
        {
            programLogic.LoadFileButtonClicked();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            programLogic.SaveButtonClicked();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            programLogic.SaveAsButtonClicked();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (programLogic.FileManager.NeedsSaving())
            {
                string message = "Do you want to save changes?";
                string caption = Text;
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;

                DialogResult messageBoxDialogResult = MessageBox.Show(message, caption, buttons);

                if (messageBoxDialogResult == DialogResult.Yes)
                {
                    if (!ProgramLogic.Instance.FileManager.SaveFile(false, out string fileName))
                    {
                        MessageBox.Show($"Could not save file '{fileName}'.");
                        e.Cancel = true;
                    }
                }
                else if (messageBoxDialogResult == DialogResult.Cancel)
                    e.Cancel = true;
            }
            else
                return;
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            programLogic.Undo();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            programLogic.Redo();
        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            programLogic.Undo();
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            programLogic.Redo();
        }

        private void UndoToolStripButton_Click_1(object sender, EventArgs e)
        {
            programLogic.Undo();
        }

        private void RedoToolStripButton_Click_1(object sender, EventArgs e)
        {
            programLogic.Redo();
        }

        private void canvasUserControl1_SizeChanged(object sender, EventArgs e)
        {
            ProgramLogic.View?.CanvasUCsizeChanged(canvasUserControl1.Width, canvasUserControl1.Height);
            ProgramLogic.View?.ChangeNearObjectDistances();
            canvasUserControl1.Invalidate();
        }

        private void canvasUserControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            programLogic.CanvasUCscroll(ControlPressed, ShiftPressed, e.Delta, e.Location);
        }

        private void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasUserControl1.Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            programLogic.KeyPressedMainForm(e.KeyCode);

            if (e.KeyCode == Keys.ControlKey)
                ControlPressed = true;

            if (e.KeyCode == Keys.ShiftKey)
                ShiftPressed = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                ControlPressed = false;

            if (e.KeyCode == Keys.ShiftKey)
                ShiftPressed = false;
        }

        private void canvasUserControl1_MouseMove(object sender, MouseEventArgs e)
        {
            programLogic.CanvasUCmouseMove(e);
        }

        private void canvasUserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("mouse down");
            programLogic.CanvasUCmouseDown(e);
        }

        private void canvasUserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            programLogic.CanvasUCmouseUp(e);
        }

        private void SelectionButton_Click(object sender, EventArgs e)
        {
            programLogic.ChooseTool(new SelectionTool());
        }

        private void DrawStraightLinesButton_Click(object sender, EventArgs e)
        {
            programLogic.ChooseTool(new StraightLineTool());
        }

        private void DrawRectanglesButton_Click(object sender, EventArgs e)
        {
            programLogic.ChooseTool(new RectangleTool());
        }

        private void DrawEllipsesButton_Click(object sender, EventArgs e)
        {
            programLogic.ChooseTool(new EllipseTool());
        }

        private void DrawCurvesButton_Click(object sender, EventArgs e)
        {
            programLogic.ChooseTool(new BezierCurveTool());
        }

        private void colorPickerButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                programLogic.SetSelectedDrawnObjectCOlor(colorDialog.Color);
                ProgramLogic.Instance.FileManager.ProgressSaved = false;
            }
        }

        private void drawingColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                programLogic.DrawingColor = colorDialog.Color;
                drawingColorButton.BackColor = colorDialog.Color;
            }
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            programLogic.LoadFileButtonClicked();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            programLogic.CopySelectedItem();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            programLogic.PasteSelectedItem();
        }

        private void drawTextButton_Click(object sender, EventArgs e)
        {

        }
    }
}
