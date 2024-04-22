using RobotDrawerEditor.DrawnObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RobotDrawerEditor.Control_classes
{
    public class FileManager
    {
        public bool ProgressSaved { get; set; } = true;
        public string saveAsFileName { get; set; } = null;
        public bool EditingInProgress { get; set; } = true;

        public bool SaveFile(bool saveAs, out string outFileName)
        {
            outFileName = null;
            bool res;

            if (!NeedsSaving())
                return true;

            if (saveAs || saveAsFileName == null || saveAsFileName == "")
            {
                res = SaveFileViaDialog(out _);
                ProgressSaved = res;
            }
            else
            {
                try
                {
                    res = WriteToFile(saveAsFileName);
                    ProgressSaved = res;
                }
                catch
                {
                    res = false;
                }    
            }

            return res;
        }

        public bool LoadFileByFileName(string fileName)
        {
            ProgramLogic.Instance.ClearEverythingForNewFile();
            saveAsFileName = fileName;

            XmlDocument document = new XmlDocument();

            string fileContent;

            try
            {
                fileContent = File.ReadAllText(fileName);
                document.LoadXml(fileContent);
            }
            catch 
            {
                return false;
            }

            LoadDrawnObjectsFromDocument(document);
            MainForm.Instance.canvasUserControl1.Invalidate();

            EditingInProgress = true;
            ProgressSaved = true;

            return true;
        }

        public bool LoadFile(out string fileName)
        {
            fileName = AskUserForFileNameViaDialog();

            if (fileName != null)
            {
                if (!LoadFileByFileName(fileName))
                    return false;

                return true;
            }

            return false;
        }

        public string AskUserForFileNameViaDialog()
        {
            MainForm.Instance.openFileDialog1.Filter = "Robot vector graphics|*.rvg";
            DialogResult newFileDialogResult = MainForm.Instance.openFileDialog1.ShowDialog();

            if (newFileDialogResult == DialogResult.OK)
                return MainForm.Instance.openFileDialog1.FileName;
            else
                return null;
        }

        public bool SaveFileViaDialog(out string outFileName)
        {
            outFileName = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Robot vector graphics|*.rvg";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    WriteToFile(sfd.FileName);
                    saveAsFileName = sfd.FileName;
                    outFileName = sfd.FileName;

                    return true;
                }
                catch
                {
                    string message = "Could not save changes.";
                    string caption = MainForm.Instance.Text;
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    MessageBox.Show(message, caption, buttons);

                    return false;
                }
            }
            else
                return false;
        }

        public bool NeedsSaving()
        {
            return EditingInProgress && !ProgressSaved;  // && ProgramLogic.Instance.UndoAvailable();  // add later
        }

        private void LoadDrawnObjectsFromDocument(XmlDocument document)
        {
            List<DrawnObject> drawnObjects = new List<DrawnObject>();

            foreach (XmlElement nodeElement in document.FirstChild.FirstChild.ChildNodes)
                drawnObjects.Add(XmlElementToDrawnObject(nodeElement));

            ProgramLogic.Instance.Canvas.Reset();

            foreach (DrawnObject drawnObject in drawnObjects)
                ProgramLogic.Instance.Canvas.AddDrawnObject(drawnObject);
        }

        public bool WriteToFile(string fileName)  // has to throw exception on failure, yes!!
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("RobotDrawingSvgFile");
            XmlElement drawnObjectsElements = document.CreateElement("DrawnObjects");

            root.AppendChild(drawnObjectsElements);
            document.AppendChild(root);

            foreach (DrawnObject drawnObject in ProgramLogic.Instance.Canvas.DrawnObjects)
            {
                drawnObjectsElements.AppendChild(DrawnObjectToXmlElement(document, drawnObject));
            }

            try
            {
                document.Save(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private XmlElement DrawnObjectToXmlElement(XmlDocument document, DrawnObject drawnObject)
        {
            XmlElement drawnObjectElement = document.CreateElement(drawnObject.GetType().Name);
            drawnObjectElement.SetAttribute("color", drawnObject.Color.Name);

            if (drawnObject is StraightLine straightLine)
            {
                drawnObjectElement.AppendChild(PointToXmlElement(document, straightLine.ControlPoint0));
                drawnObjectElement.AppendChild(PointToXmlElement(document, straightLine.ControlPoint1));
            }
            else if (drawnObject is MyRectangle myRectangle)
            {
                drawnObjectElement.AppendChild(PointToXmlElement(document, new PointF(myRectangle.X, myRectangle.Y)));
                drawnObjectElement.AppendChild(PrimitiveValueToXmlElement(document, "Width", myRectangle.Width));
                drawnObjectElement.AppendChild(PrimitiveValueToXmlElement(document, "Height", myRectangle.Height));
            }
            else if (drawnObject is Circle circle)
            {
                drawnObjectElement.AppendChild(PointToXmlElement(document, circle.Centre));
                drawnObjectElement.AppendChild(PrimitiveValueToXmlElement(document, "Radius", circle.Radius));
            }
            else if (drawnObject is Ellipse ellipse)
            {
                drawnObjectElement.AppendChild(PointToXmlElement(document, ellipse.Centre));
                drawnObjectElement.AppendChild(PrimitiveValueToXmlElement(document, "RadiusX", ellipse.RadiusX));
                drawnObjectElement.AppendChild(PrimitiveValueToXmlElement(document, "RadiusY", ellipse.RadiusY));
            }
            else if (drawnObject is BezierCurve bezierCurve)
            {
                foreach (ControlPoint controlPoint in bezierCurve.ControlPoints)
                    drawnObjectElement.AppendChild(PointToXmlElement(document, controlPoint));
            }
            else if (drawnObject is ConnectedBezierCurve connectedBezierCurve)
            {
                foreach (BezierCurve curve in connectedBezierCurve.Curves)
                    drawnObjectElement.AppendChild(DrawnObjectToXmlElement(document, curve));
            }

            return drawnObjectElement;
        }

        private XmlElement PointToXmlElement(XmlDocument document, PointF point)
        {
            XmlElement pointElement = document.CreateElement("Point");

            pointElement.AppendChild(PrimitiveValueToXmlElement(document, "X", point.X));
            pointElement.AppendChild(PrimitiveValueToXmlElement(document, "Y", point.Y));

            return pointElement;
        }

        private XmlElement PrimitiveValueToXmlElement<T>(XmlDocument document, string name, T value)
        {
            string stringValue;

            if (value is float floatValue)
                stringValue = floatValue.ToString(CultureInfo.InvariantCulture);
            else if (value is double doubleValue)
                stringValue = doubleValue.ToString(CultureInfo.InvariantCulture);
            else
                stringValue = value.ToString();

            XmlElement resElement = document.CreateElement(name);
            resElement.InnerText = stringValue;

            return resElement;
        }

        private DrawnObject XmlElementToDrawnObject(XmlElement element)
        {
            DrawnObject drawnObject = null;
            Color color = Color.FromName(element.GetAttribute("color").ToString());

            if (element.Name == typeof(StraightLine).Name)
            {
                PointF point0 = XmlElementToPointF(element.ChildNodes[0] as XmlElement);
                PointF point1 = XmlElementToPointF(element.ChildNodes[1] as XmlElement);

                drawnObject = new StraightLine(point0, point1, color);
            }
            else if (element.Name == typeof(MyRectangle).Name)
            {
                PointF point = XmlElementToPointF(element.ChildNodes[0] as XmlElement);
                float width = XmlElementToFloat(element.GetElementsByTagName("Width")[0] as XmlElement);
                float height = XmlElementToFloat(element.GetElementsByTagName("Height")[0] as XmlElement);

                drawnObject = new MyRectangle(point, width, height, color);
            }
            else if (element.Name == typeof(Circle).Name)
            {
                PointF centre = XmlElementToPointF(element.ChildNodes[0] as XmlElement);
                float radius = XmlElementToFloat(element.GetElementsByTagName("Radius")[0] as XmlElement);

                drawnObject = new Circle(centre, radius, color);
            }
            else if (element.Name == typeof(Ellipse).Name)
            {
                PointF centre = XmlElementToPointF(element.ChildNodes[0] as XmlElement);
                float radiusX = XmlElementToFloat(element.GetElementsByTagName("RadiusX")[0] as XmlElement);
                float radiusY = XmlElementToFloat(element.GetElementsByTagName("RadiusY")[0] as XmlElement);

                drawnObject = new Ellipse(centre, radiusX, radiusY, color);
            }
            else if (element.Name == typeof(BezierCurve3).Name || element.Name == typeof(BezierCurve4).Name)
            {
                int expectedOrder = element.Name == typeof(BezierCurve3).Name ? 3 : 4;
                List<PointF> points = new List<PointF>();

                foreach (XmlElement pointElement in element.GetElementsByTagName("Point"))
                    points.Add(XmlElementToPointF(pointElement));

                if (points.Count != expectedOrder)
                    throw new FileFormatException($"There should be {expectedOrder} points in BezierCurve{expectedOrder} object. Found {points.Count}.");

                if (expectedOrder == 3)
                    drawnObject = new BezierCurve3(points[0], points[1], points[2], color);
                else
                    drawnObject = new BezierCurve4(points[0], points[1], points[2], points[3], color);
            }
            else if (element.Name == typeof(ConnectedBezierCurve).Name)
            {
                List<BezierCurve> curves = new List<BezierCurve>();

                foreach (XmlElement childElement in element.ChildNodes)
                    curves.Add(XmlElementToDrawnObject(childElement) as BezierCurve);

                drawnObject = new ConnectedBezierCurve(curves, color);
            }

            return drawnObject;
        }

        private PointF XmlElementToPointF(XmlElement element)
        {
            float x = XmlElementToFloat(element.GetElementsByTagName("X")[0] as XmlElement);
            float y = XmlElementToFloat(element.GetElementsByTagName("Y")[0] as XmlElement);

            return new PointF(x, y);
        }

        private float XmlElementToFloat(XmlElement element)
        {
            if (!float.TryParse(element.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                throw new FileFormatException("Could not read float value from file.");

            return result;
        }
    }
}
