using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ocr.UI
{
    class WindowDragger
    {
        private Control control;
        private Window window;

        private bool isDraggingWindow = false;
        private MouseButton windowDragButton;
        private Point windowDragLocation;

        public WindowDragger(Control control, Window window)
        {
            this.control = control;
            this.window = window;
        }

        public void Deactivated()
        {
            if (isDraggingWindow)
            {
                control.ReleaseMouseCapture();
                isDraggingWindow = false;
            }
        }

        public void MouseDown(MouseButtonEventArgs e)
        {
            if (!isDraggingWindow)
            {
                if (e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Right)
                {
                    isDraggingWindow = true;
                    windowDragButton = e.ChangedButton;
                    windowDragLocation = e.GetPosition(window);
                    control.CaptureMouse();
                }
            }
        }

        public void MouseUp(MouseButtonEventArgs e)
        {
            if (isDraggingWindow && e.ChangedButton == windowDragButton)
            {
                isDraggingWindow = false;
                control.ReleaseMouseCapture();
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (isDraggingWindow)
            {
                Point pos = window.PointToScreen(e.GetPosition(window));
                pos.X -= (int)windowDragLocation.X;
                pos.Y -= (int)windowDragLocation.Y;
                window.Top = (int)pos.Y;
                window.Left = (int)pos.X;
            }
        }
    }
}
