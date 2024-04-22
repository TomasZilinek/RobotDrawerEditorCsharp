namespace RobotDrawerEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFileToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStrip = new System.Windows.Forms.ToolStrip();
            this.SelectionButton = new System.Windows.Forms.ToolStripButton();
            this.DrawStraightLinesButton = new System.Windows.Forms.ToolStripButton();
            this.DrawRectanglesButton = new System.Windows.Forms.ToolStripButton();
            this.DrawEllipsesButton = new System.Windows.Forms.ToolStripButton();
            this.DrawCurvesButton = new System.Windows.Forms.ToolStripButton();
            this.EditPathsButton = new System.Windows.Forms.ToolStripButton();
            this.DrawTextButton = new System.Windows.Forms.ToolStripButton();
            this.colorPickerButton = new System.Windows.Forms.ToolStripButton();
            this.horizontalToolStrip = new System.Windows.Forms.ToolStrip();
            this.UndoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.drawingColorButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.canvasUserControl1 = new RobotDrawerEditor.CanvasUserControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.mouseXPosLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mouseYPosLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip.SuspendLayout();
            this.verticalToolStrip.SuspendLayout();
            this.horizontalToolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.EditToolStripMenuItem,
            this.ViewToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1372, 28);
            this.MenuStrip.TabIndex = 3;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFileToolstripMenuItem,
            this.loadFileToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.AaveAsToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // NewFileToolstripMenuItem
            // 
            this.NewFileToolstripMenuItem.Name = "NewFileToolstripMenuItem";
            this.NewFileToolstripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.NewFileToolstripMenuItem.Text = "New file";
            this.NewFileToolstripMenuItem.Click += new System.EventHandler(this.NewFileToolstripMenuItem_Click);
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.loadFileToolStripMenuItem.Text = "Load file";
            this.loadFileToolStripMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // AaveAsToolStripMenuItem
            // 
            this.AaveAsToolStripMenuItem.Name = "AaveAsToolStripMenuItem";
            this.AaveAsToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.AaveAsToolStripMenuItem.Text = "Save as";
            this.AaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.UndoButton,
            this.RedoButton});
            this.EditToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            this.EditToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.EditToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.Enabled = false;
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(128, 26);
            this.UndoButton.Text = "Undo";
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // RedoButton
            // 
            this.RedoButton.Enabled = false;
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(128, 26);
            this.RedoButton.Text = "Redo";
            this.RedoButton.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ViewToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.ViewToolStripMenuItem.Text = "View";
            this.ViewToolStripMenuItem.Click += new System.EventHandler(this.ViewToolStripMenuItem_Click);
            // 
            // verticalToolStrip
            // 
            this.verticalToolStrip.AutoSize = false;
            this.verticalToolStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.verticalToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.verticalToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.verticalToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.verticalToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectionButton,
            this.DrawStraightLinesButton,
            this.DrawRectanglesButton,
            this.DrawEllipsesButton,
            this.DrawCurvesButton,
            this.EditPathsButton,
            this.DrawTextButton,
            this.colorPickerButton});
            this.verticalToolStrip.Location = new System.Drawing.Point(0, 28);
            this.verticalToolStrip.MaximumSize = new System.Drawing.Size(133, 1354);
            this.verticalToolStrip.Name = "verticalToolStrip";
            this.verticalToolStrip.Padding = new System.Windows.Forms.Padding(0, 12, 1, 0);
            this.verticalToolStrip.Size = new System.Drawing.Size(71, 791);
            this.verticalToolStrip.TabIndex = 4;
            this.verticalToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStrip_ItemClicked);
            // 
            // SelectionButton
            // 
            this.SelectionButton.AccessibleName = "selectionButton";
            this.SelectionButton.AutoSize = false;
            this.SelectionButton.Checked = true;
            this.SelectionButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectionButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectionButton.Image")));
            this.SelectionButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SelectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectionButton.Name = "SelectionButton";
            this.SelectionButton.RightToLeftAutoMirrorImage = true;
            this.SelectionButton.Size = new System.Drawing.Size(50, 50);
            this.SelectionButton.Text = "brightnessButton";
            this.SelectionButton.ToolTipText = "Normal selection";
            this.SelectionButton.Click += new System.EventHandler(this.SelectionButton_Click);
            // 
            // DrawStraightLinesButton
            // 
            this.DrawStraightLinesButton.AccessibleName = "drawStraightLinesButton";
            this.DrawStraightLinesButton.AutoSize = false;
            this.DrawStraightLinesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DrawStraightLinesButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawStraightLinesButton.Image")));
            this.DrawStraightLinesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DrawStraightLinesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawStraightLinesButton.Name = "DrawStraightLinesButton";
            this.DrawStraightLinesButton.RightToLeftAutoMirrorImage = true;
            this.DrawStraightLinesButton.Size = new System.Drawing.Size(50, 50);
            this.DrawStraightLinesButton.Text = "cropButton";
            this.DrawStraightLinesButton.ToolTipText = "Draw straight lines";
            this.DrawStraightLinesButton.Click += new System.EventHandler(this.DrawStraightLinesButton_Click);
            // 
            // DrawRectanglesButton
            // 
            this.DrawRectanglesButton.AccessibleName = "drawRectanglesButton";
            this.DrawRectanglesButton.AutoSize = false;
            this.DrawRectanglesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DrawRectanglesButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawRectanglesButton.Image")));
            this.DrawRectanglesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DrawRectanglesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawRectanglesButton.Name = "DrawRectanglesButton";
            this.DrawRectanglesButton.RightToLeftAutoMirrorImage = true;
            this.DrawRectanglesButton.Size = new System.Drawing.Size(50, 50);
            this.DrawRectanglesButton.ToolTipText = "Draw rectangles";
            this.DrawRectanglesButton.Click += new System.EventHandler(this.DrawRectanglesButton_Click);
            // 
            // DrawEllipsesButton
            // 
            this.DrawEllipsesButton.AccessibleName = "drawEllipsesButton";
            this.DrawEllipsesButton.AutoSize = false;
            this.DrawEllipsesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DrawEllipsesButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawEllipsesButton.Image")));
            this.DrawEllipsesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DrawEllipsesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawEllipsesButton.Name = "DrawEllipsesButton";
            this.DrawEllipsesButton.RightToLeftAutoMirrorImage = true;
            this.DrawEllipsesButton.Size = new System.Drawing.Size(50, 50);
            this.DrawEllipsesButton.ToolTipText = "Draw ellipses and circles";
            this.DrawEllipsesButton.Click += new System.EventHandler(this.DrawEllipsesButton_Click);
            // 
            // DrawCurvesButton
            // 
            this.DrawCurvesButton.AccessibleName = "drawCurvesButton";
            this.DrawCurvesButton.AutoSize = false;
            this.DrawCurvesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DrawCurvesButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawCurvesButton.Image")));
            this.DrawCurvesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DrawCurvesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawCurvesButton.Name = "DrawCurvesButton";
            this.DrawCurvesButton.RightToLeftAutoMirrorImage = true;
            this.DrawCurvesButton.Size = new System.Drawing.Size(50, 50);
            this.DrawCurvesButton.Text = "blurButton";
            this.DrawCurvesButton.ToolTipText = "Draw curves";
            this.DrawCurvesButton.Click += new System.EventHandler(this.DrawCurvesButton_Click);
            // 
            // EditPathsButton
            // 
            this.EditPathsButton.AccessibleName = "editPathsButton";
            this.EditPathsButton.AutoSize = false;
            this.EditPathsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditPathsButton.Image = ((System.Drawing.Image)(resources.GetObject("EditPathsButton.Image")));
            this.EditPathsButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.EditPathsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditPathsButton.Name = "EditPathsButton";
            this.EditPathsButton.RightToLeftAutoMirrorImage = true;
            this.EditPathsButton.Size = new System.Drawing.Size(50, 50);
            this.EditPathsButton.Text = "RGBAfilterButton";
            this.EditPathsButton.ToolTipText = "Edit paths";
            // 
            // DrawTextButton
            // 
            this.DrawTextButton.AccessibleName = "drawTextButton";
            this.DrawTextButton.AutoSize = false;
            this.DrawTextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DrawTextButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawTextButton.Image")));
            this.DrawTextButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DrawTextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawTextButton.Name = "DrawTextButton";
            this.DrawTextButton.RightToLeftAutoMirrorImage = true;
            this.DrawTextButton.Size = new System.Drawing.Size(50, 50);
            this.DrawTextButton.Text = "RGBAfilterButton";
            this.DrawTextButton.ToolTipText = "Edit paths";
            this.DrawTextButton.Click += new System.EventHandler(this.drawTextButton_Click);
            // 
            // colorPickerButton
            // 
            this.colorPickerButton.AccessibleName = "colorPickerButton";
            this.colorPickerButton.AutoSize = false;
            this.colorPickerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.colorPickerButton.Enabled = false;
            this.colorPickerButton.Image = ((System.Drawing.Image)(resources.GetObject("colorPickerButton.Image")));
            this.colorPickerButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.colorPickerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.colorPickerButton.Name = "colorPickerButton";
            this.colorPickerButton.RightToLeftAutoMirrorImage = true;
            this.colorPickerButton.Size = new System.Drawing.Size(50, 50);
            this.colorPickerButton.ToolTipText = "Draw rectangles";
            this.colorPickerButton.Click += new System.EventHandler(this.colorPickerButton_Click);
            // 
            // horizontalToolStrip
            // 
            this.horizontalToolStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.horizontalToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.horizontalToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.horizontalToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoToolStripButton,
            this.toolStripButton1,
            this.drawingColorButton});
            this.horizontalToolStrip.Location = new System.Drawing.Point(71, 28);
            this.horizontalToolStrip.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.horizontalToolStrip.Name = "horizontalToolStrip";
            this.horizontalToolStrip.Size = new System.Drawing.Size(1301, 27);
            this.horizontalToolStrip.TabIndex = 6;
            this.horizontalToolStrip.Text = "toolStrip1";
            // 
            // UndoToolStripButton
            // 
            this.UndoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UndoToolStripButton.Enabled = false;
            this.UndoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("UndoToolStripButton.Image")));
            this.UndoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UndoToolStripButton.Name = "UndoToolStripButton";
            this.UndoToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.UndoToolStripButton.Text = "toolStripButton1";
            this.UndoToolStripButton.ToolTipText = "Undo";
            this.UndoToolStripButton.Click += new System.EventHandler(this.UndoToolStripButton_Click_1);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton1.Text = "toolStripButton2";
            this.toolStripButton1.ToolTipText = "Redo";
            // 
            // drawingColorButton
            // 
            this.drawingColorButton.BackColor = System.Drawing.Color.Black;
            this.drawingColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.drawingColorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.drawingColorButton.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.drawingColorButton.Name = "drawingColorButton";
            this.drawingColorButton.Size = new System.Drawing.Size(29, 24);
            this.drawingColorButton.Text = "toolStripButton2";
            this.drawingColorButton.ToolTipText = "Redo";
            this.drawingColorButton.Click += new System.EventHandler(this.drawingColorButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // canvasUserControl1
            // 
            this.canvasUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvasUserControl1.AutoSize = true;
            this.canvasUserControl1.BackColor = System.Drawing.Color.White;
            this.canvasUserControl1.Location = new System.Drawing.Point(74, 61);
            this.canvasUserControl1.Name = "canvasUserControl1";
            this.canvasUserControl1.Size = new System.Drawing.Size(1292, 730);
            this.canvasUserControl1.TabIndex = 5;
            this.canvasUserControl1.SizeChanged += new System.EventHandler(this.canvasUserControl1_SizeChanged);
            this.canvasUserControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasUserControl1_MouseDown);
            this.canvasUserControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasUserControl1_MouseMove);
            this.canvasUserControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasUserControl1_MouseUp);
            this.canvasUserControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.canvasUserControl1_MouseWheel);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouseXPosLabel,
            this.mouseYPosLabel});
            this.statusStrip.Location = new System.Drawing.Point(71, 793);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1301, 26);
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusStrip1";
            // 
            // mouseXPosLabel
            // 
            this.mouseXPosLabel.ForeColor = System.Drawing.Color.Black;
            this.mouseXPosLabel.Name = "mouseXPosLabel";
            this.mouseXPosLabel.Size = new System.Drawing.Size(21, 20);
            this.mouseXPosLabel.Text = "X:";
            // 
            // mouseYPosLabel
            // 
            this.mouseYPosLabel.ForeColor = System.Drawing.Color.Black;
            this.mouseYPosLabel.Name = "mouseYPosLabel";
            this.mouseYPosLabel.Size = new System.Drawing.Size(20, 20);
            this.mouseYPosLabel.Text = "Y:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1372, 819);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.horizontalToolStrip);
            this.Controls.Add(this.canvasUserControl1);
            this.Controls.Add(this.verticalToolStrip);
            this.Controls.Add(this.MenuStrip);
            this.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "RobotDrawerEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.verticalToolStrip.ResumeLayout(false);
            this.verticalToolStrip.PerformLayout();
            this.horizontalToolStrip.ResumeLayout(false);
            this.horizontalToolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewFileToolstripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem UndoButton;
        public System.Windows.Forms.ToolStripMenuItem RedoButton;
        private System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        private System.Windows.Forms.ToolStrip verticalToolStrip;
        private System.Windows.Forms.ToolStripButton SelectionButton;
        private System.Windows.Forms.ToolStripButton EditPathsButton;
        private System.Windows.Forms.ToolStripButton DrawCurvesButton;
        private System.Windows.Forms.ToolStripButton DrawStraightLinesButton;
        private System.Windows.Forms.ToolStripButton DrawEllipsesButton;
        private System.Windows.Forms.ToolStripButton DrawRectanglesButton;
        private System.Windows.Forms.ToolStrip horizontalToolStrip;
        public System.Windows.Forms.ToolStripButton UndoToolStripButton;
        public System.Windows.Forms.ToolStripButton drawingColorButton;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        public CanvasUserControl canvasUserControl1;
        public System.Windows.Forms.ToolStripButton colorPickerButton;
        public System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel mouseYPosLabel;
        public System.Windows.Forms.ToolStripStatusLabel mouseXPosLabel;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton DrawTextButton;
    }
}

