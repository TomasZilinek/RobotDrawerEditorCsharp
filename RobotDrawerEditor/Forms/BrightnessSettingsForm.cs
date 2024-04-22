using System;
using System.Drawing;

namespace RobotDrawerEditor
{
    public partial class BrightnessSettingsForm : ParentForm
    {
        private static int trackBarInitialValue = 0;
        
        public BrightnessSettingsForm(MainForm myParent, ProgramLogic pl)
            : base(myParent, pl, FormTypeEnum.brightnessSettingsForm)
        {
            InitializeComponent();
        }
        /*
        private void BrightnessSettingsForm_Load(object sender, EventArgs e)
        {
            confirmButton.Enabled = false;
        }

        private void BightnessLevelTrackBar_Scroll(object sender, EventArgs e)
        {
            reactOnTextChanged = false;
            brightnessValueTextBox.Text = bightnessLevelTrackBar.Value.ToString();
            reactOnTextChanged = true;

            DrawPictureBoxImage();

            confirmButton.Enabled = bightnessLevelTrackBar.Value != trackBarInitialValue &&
                                    selectedRectangle != new Rectangle(-1, -1, -1, -1);
        }

        private void BrightnessValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (reactOnTextChanged)
            {
                FormBinder.BindTextBoxTextChangedToTrackBar(
                                brightnessValueTextBox, bightnessLevelTrackBar, -255, 255);

                DrawPictureBoxImage();
                confirmButton.Enabled = bightnessLevelTrackBar.Value != trackBarInitialValue;
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (bightnessLevelTrackBar.Value != trackBarInitialValue)
            {
                programLogic.ChangeBrightness(bightnessLevelTrackBar.Value, selectedRectangle);
            }

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            programLogic.ActiveEditor.PictureBox.Image = programLogic.ActiveEditor.EditedImage;
            Close();
        }

        private void DrawPictureBoxImage()
        {
            if (selectedRectangle != new Rectangle(-1, -1, -1, -1))
            {
                if (previewCheckBox.Checked)
                {
                    if (bightnessLevelTrackBar.Value != trackBarInitialValue)
                        programLogic.ChangeBrightness(bightnessLevelTrackBar.Value, selectedRectangle, true);
                }
                else
                    programLogic.ActiveEditor.PictureBox.Image = programLogic.ActiveEditor.EditedImage;
            }
        }

        public override void RectangleSelected(Rectangle rect)
        {
            base.RectangleSelected(rect);

            DrawPictureBoxImage();
            confirmButton.Enabled = bightnessLevelTrackBar.Value != trackBarInitialValue;
        }

        private void WholeImageButton_Click(object sender, EventArgs e)
        {
            selectedRectangle = new Rectangle(0, 0,
                                              programLogic.ActiveEditor.PictureBox.Image.Width - 1,
                                              programLogic.ActiveEditor.PictureBox.Image.Height - 1);

            DrawPictureBoxImage();
            programLogic.ActiveEditor.PictureBox.SettingsFormWholeImageSelected();
        }

        private void PreviewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DrawPictureBoxImage();
        }

        override public void DisableConfirmButton()
        {
            confirmButton.Enabled = false;
        }
        */
    }
}
