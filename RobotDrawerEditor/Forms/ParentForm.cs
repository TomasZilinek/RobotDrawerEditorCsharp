using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotDrawerEditor
{
    public partial class ParentForm : Form
    {
        public FormTypeEnum FormType { get; private set; }
        protected MainForm mainForm = null;
        protected ProgramLogic programLogic = null;
        public bool IsOpened { get; protected set; }
        private int flashesDone = 0;
        protected bool reactOnTextChanged = true;  // ?

        public ParentForm()
        {
            InitializeComponent();
        }

        public ParentForm(MainForm myParent, ProgramLogic pl, FormTypeEnum type)
        {
            InitializeComponent();

            mainForm = myParent;
            programLogic = pl;
            FormType = type;
        }

        private void ParentForm_Load(object sender, EventArgs e)
        {
            IsOpened = true;
        }

        private void ParentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnFormClose();
        }

        protected virtual void OnFormClose()
        {
            IsOpened = false;
            //programLogic.ActiveEditor.PictureBox.Image = programLogic.ActiveEditor.EditedImage;
        }

        public void StartFlashing()
        {
            flashesDone = 0;
            flashTimer.Start();
        }

        private void FlashTimer_Tick(object sender, EventArgs e)
        {
            flashesDone++;
            ControlBox = !ControlBox;

            if (flashesDone == 20)
            {
                flashTimer.Stop();
                flashesDone = 0;
                ControlBox = true;
            }
        }

        virtual public void DisableConfirmButton()
        {

        }
    }
}
