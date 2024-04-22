using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotDrawerEditor
{
    public class Canvas
    {
        public Paper Paper;
        public List<DrawnObject> DrawnObjects { get; private set; }

        public Canvas(PaperType paperType=PaperType.A4)
        {
            DrawnObjects = new List<DrawnObject>();
            Paper = new Paper(paperType, 0, 0);
        }

        public void AddDrawnObject(DrawnObject obj)
        {
            DrawnObjects.Add(obj);
            ProgramLogic.Instance.FileManager.ProgressSaved = false;
        }

        public List<DrawnObject> GetObjectsInView()
        {
            return DrawnObjects.Where(obj => ProgramLogic.View.DrawnObjectInView(obj)).ToList();
        }

        public void ChangePaperType(PaperType paperType)
        {
            Paper = new Paper(paperType, 0, 0);
        }

        public void DeleteSelectedDrawObject()
        {
            DrawnObjects.Remove(MainForm.ProgramLogic.GetSelectedDrawnObject());
            ProgramLogic.MainForm.canvasUserControl1.Invalidate();
        }

        public void Reset()
        {
            DrawnObjects = new List<DrawnObject>();
        }
    }
}
