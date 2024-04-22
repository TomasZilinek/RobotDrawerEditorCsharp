using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RobotDrawerEditor
{
    public class UndoRedo
    {
        private Stack<MainActionInherited> undoActions = new Stack<MainActionInherited>();
        private Stack<MainActionInherited> redoActions = new Stack<MainActionInherited>();

        public void AddNewAction(MainActionInherited action)
        {
            undoActions.Push(action);
            redoActions.Clear();
        }

        public void Clear()
        {
            undoActions.Clear();
            redoActions.Clear();
        }

        public Image Undo(int howMany = 1)
        {
            for (int i = 1; i <= howMany; i++)
            {
                if (undoActions.Count != 0)
                {
                    MainActionInherited action = undoActions.Pop();
                    redoActions.Push(action);
                }
            }

            if (undoActions.Count == 0)
                return null;
            else
                return new Bitmap(undoActions.Peek().CorrespondingImage);
        }

        public Image Redo(int howMany = 1)
        {
            MainActionInherited lastAction = null;

            for (int i = 1; i <= howMany; i++)
            {
                if (redoActions.Count != 0)
                {
                    MainActionInherited action = redoActions.Pop();
                    lastAction = action;
                    undoActions.Push(action);
                }
            }

            return lastAction.CorrespondingImage;
        }

        public bool UndoAvailable()
        {
            return undoActions.Count > 0;
        }

        public bool RedoAvailable()
        {
            return redoActions.Count > 0;
        }

        public List<MainActionInherited> GetAllActions()
        {
            return undoActions.ToList();
        }
    }
}
