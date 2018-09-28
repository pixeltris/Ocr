using Ocr.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    class Automation
    {
        public string Path { get; set; }
        public bool UpdateReferenceImage { get; set; }
        public bool UseImageCaching { get; set; }
        public int Index { get; set; }

        private const int attempts = 150;
        private const int attemptFailWait = 100;
        private static TimeSpan timeout = TimeSpan.FromSeconds(15);

        private List<AutomationState> states = new List<AutomationState>();

        private string CombinePath(string path)
        {
            if (string.IsNullOrEmpty(Path))
                return path;
            return System.IO.Path.Combine(Path, path);
        }

        public void Clear()
        {
            states.Clear();
        }

        private AutomationState AddState(AutomationType type, string path)
        {
            AutomationState state = new AutomationState(type, path);
            states.Add(state);
            return state;
        }

        public AutomationState AddText(string text, bool enter)
        {            
            if (enter)
            {
                return AddState(AutomationType.Text, text);
            }
            else
            {
                return AddState(AutomationType.TextReturn, text);
            }
        }

        public AutomationState AddRelativeClick()
        {
            return AddState(AutomationType.RelativeClick, null);
        }

        public AutomationState AddRelativeClick(int x, int y)
        {
            AutomationState state = AddState(AutomationType.RelativeClick, null);
            state.AddElement(x, y, null);
            return state;
        }

        public AutomationState AddOpenImage()
        {
            return AddState(AutomationType.OpenImage, null);
        }

        public AutomationState AddKey(Keys key)
        {
            AutomationState state = AddState(AutomationType.Key, null);
            state.Key = key;
            return state;
        }

        public AutomationState AddSelectAll()
        {
            return AddState(AutomationType.SelectAll, null);
        }

        public AutomationState AddCopy()
        {
            return AddState(AutomationType.Copy, null);
        }

        public AutomationState AddWaitImage(string path)
        {
            return AddState(AutomationType.WaitImage, CombinePath(path));
        }

        public AutomationState AddWaitRelativeImage()
        {
            return AddState(AutomationType.WaitRelativeImage, null);
        }

        public AutomationState AddReferenceImage(string path)
        {
            return AddState(AutomationType.ReferenceImage, CombinePath(path));
        }

        public AutomationState AddStartImage(string path)
        {
            return AddState(AutomationType.StartImage, CombinePath(path));
        }

        public AutomationState AddClickableImage(string path)
        {
            return AddState(AutomationType.ClickableImage, CombinePath(path));
        }

        public AutomationState AddClickableImage(string path, int clickCount)
        {
            AutomationState state = AddState(AutomationType.ClickableImage, CombinePath(path));
            state.Value = clickCount;
            return state;
        }

        public AutomationState AddClickableImageOffset(string path, int x, int y)
        {
            AutomationState state = AddState(AutomationType.ClickableImageOffset, CombinePath(path));
            state.AddElement(x, y, null);
            return state;
        }

        public AutomationState AddMoveMouseImage(string path)
        {
            return AddState(AutomationType.MoveMouseImage, CombinePath(path));
        }

        public AutomationState AddMoveMouseImageOffset(string path, int x, int y)
        {
            AutomationState state = AddState(AutomationType.MoveMouseImageOffset, CombinePath(path));
            state.AddElement(x, y, null);
            return state;
        }

        public AutomationState AddWait(int milliseconds)
        {
            AutomationState state = AddState(AutomationType.Wait, null);
            state.Value = milliseconds;
            return state;
        }

        public AutomationState GetCurrentState(Bitmap screenshot, Rectangle referenceRect)
        {
            return GetCurrentState(screenshot, referenceRect, true);
        }

        public AutomationState GetCurrentState(Bitmap screenshot, Rectangle referenceRect, bool imageOnly)
        {
            Point referencePos = Point.Empty;
            Bitmap image = null;

            try
            {
                AutomationState result = null;
                foreach (AutomationState state in states)
                {
                    switch (state.Type)
                    {
                        //StartImage / ReferenceImage are special states and treated separate
                        //case AutomationType.StartImage:
                        //case AutomationType.ReferenceImage:
    
                        case AutomationType.AbsoluteClick:
                        case AutomationType.Wait:
                        case AutomationType.OpenImage:
                        case AutomationType.Text:
                        case AutomationType.TextReturn:
                        case AutomationType.Copy:
                        case AutomationType.SelectAll:
                        case AutomationType.Key:
                            if (imageOnly)
                                continue;
                            return state;                        

                        case AutomationType.WaitRelativeImage:
                        case AutomationType.RelativeClick:
                            if (state.IsMatch(screenshot, referenceRect))
                            {
                                return state;
                            }
                            break;

                        case AutomationType.MoveMouseImage:
                        case AutomationType.MoveMouseImageOffset:
                        case AutomationType.WaitImage:
                        case AutomationType.ClickableImage:
                        case AutomationType.ClickableImageOffset:
                            try
                            {
                                DisposeBitmap(image);
                            }
                            catch { }
                            try
                            {
                                image = (Bitmap)Bitmap.FromFile(state.Path);
                                Rectangle rect = state.Find(screenshot, image);
                                if (rect != Rectangle.Empty)
                                {
                                    return state;
                                }
                            }
                            catch { }
                            break;
                    }
                }
                return result;
            }
            finally
            {
                DisposeBitmap(image);
            }
        }

        public AutomationState GetFirstState()
        {
            return states.FirstOrDefault(x => x.Type != AutomationType.StartImage && 
                x.Type != AutomationType.ReferenceImage);
        }

        private Rectangle GetReferenceRect(Bitmap screenshot)
        {
            return GetRect(screenshot, AutomationType.ReferenceImage);
        }

        private Rectangle GetStartRect(Bitmap screenshot)
        {
            return GetRect(screenshot, AutomationType.StartImage);
        }

        private Rectangle GetRect(Bitmap screenshot, AutomationType type)
        {
            Bitmap referenceImage = null;

            try
            {
                foreach (AutomationState state in states)
                {
                    if (state.Type == type)
                    {
                        DisposeBitmap(referenceImage);
                        try
                        {
                            referenceImage = (Bitmap)Bitmap.FromFile(state.Path);
                            Rectangle rect = state.Find(screenshot, referenceImage);
                            if (rect != Rectangle.Empty)
                            {
                                return rect;
                            }
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                DisposeBitmap(referenceImage);
            }

            return Rectangle.Empty;
        }

        private bool Run(bool secondAttempt)
        {
            Index = 0;

            Bitmap screenshot = null;
            Bitmap image = null;
            Stopwatch stopwatch = new Stopwatch();

            string lastImagePath = null;
            Rectangle lastImageRect = Rectangle.Empty;

            try
            {
                screenshot = UpdateScreenshot(screenshot);
                Rectangle referenceRect = GetReferenceRect(screenshot);
                Rectangle startRect = GetStartRect(screenshot);

                if (referenceRect == Rectangle.Empty)
                {
                    return false;
                }

                // If we can't find a state spam escape and try again
                if (startRect == Rectangle.Empty)
                {
                    if (secondAttempt)
                    {
                        return false;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        SendKey(Keys.Escape);
                        System.Threading.Thread.Sleep(50);
                    }

                    System.Threading.Thread.Sleep(1000);
                    screenshot = UpdateScreenshot(screenshot);
                    referenceRect = GetReferenceRect(screenshot);
                    if (referenceRect == Rectangle.Empty)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }

                    return Run(true);
                }

                AutomationState currentState = GetFirstState();//GetCurrentState(screenshot, referenceRect);
                if (currentState != null)
                {                    
                    int index = states.IndexOf(currentState);
                    for (int i = index; i < states.Count; i++)
                    {
                        Index = i;
                        stopwatch.Stop();
                        stopwatch.Reset();
                        stopwatch.Start();                        

                        bool success = true;
                        AutomationState state = states[i];
                        switch (state.Type)
                        {
                            case AutomationType.StartImage:
                                break;
                            case AutomationType.ReferenceImage:
                                break;

                            case AutomationType.Wait:
                                if (state.Value > 0)
                                    Thread.Sleep(state.Value);
                                break;

                            case AutomationType.OpenImage:
                                SendOpenImage();
                                break;
                            case AutomationType.Copy:
                                SendCopy();
                                break;
                            case AutomationType.SelectAll:
                                SendSelectAll();
                                break;                            
                            case AutomationType.Text:
                                SendKeys(state.Path);
                                break;
                            case AutomationType.TextReturn:
                                SendKeys(state.Path);
                                SendKey(Keys.Return);
                                break;
                            case AutomationType.Key:
                                SendKey(state.Key);
                                break;

                            case AutomationType.AbsoluteClick:
                                Click(state.X, state.Y);
                                break;                            

                            case AutomationType.WaitRelativeImage:
                            case AutomationType.RelativeClick:
                                success = false;
                                for (int j = 0; j < attempts; j++)
                                {
                                    screenshot = UpdateScreenshot(screenshot);
                                    if (UpdateReferenceImage)
                                    {
                                        referenceRect = GetReferenceRect(screenshot);
                                    }
                                    if (state.IsMatch(screenshot, referenceRect))
                                    {
                                        success = true;
                                        break;
                                    }
                                    Thread.Sleep(attemptFailWait);
                                    if (stopwatch.Elapsed >= timeout)
                                        break;
                                }
                                if (success)
                                {
                                    if (state.Type == AutomationType.RelativeClick)
                                    {
                                        Click(state, referenceRect);
                                    }
                                }
                                break;

                            case AutomationType.MoveMouseImage:
                            case AutomationType.MoveMouseImageOffset:
                            case AutomationType.WaitImage:
                            case AutomationType.ClickableImage:
                            case AutomationType.ClickableImageOffset:
                                DisposeBitmap(image);
                                image = state.LoadImage();
                                if(image != null)
                                {
                                    Rectangle rect = Rectangle.Empty;
                                    if (UseImageCaching && lastImagePath == state.Path && lastImageRect != Rectangle.Empty)
                                    {
                                        rect = lastImageRect;
                                    }
                                    else
                                    {
                                        for (int j = 0; j < attempts; j++)
                                        {
                                            screenshot = UpdateScreenshot(screenshot);
                                            rect = state.Find(screenshot, image);
                                            if (rect != Rectangle.Empty)
                                            {
                                                break;
                                            }
                                            Thread.Sleep(attemptFailWait);
                                            if (stopwatch.Elapsed >= timeout)
                                                break;
                                        }
                                    }
                                    success = rect != Rectangle.Empty;
                                    if (success)
                                    {
                                        lastImageRect = rect;
                                        lastImagePath = state.Path;

                                        int clickCount = state.Value <= 0 ? 1 : state.Value;
                                        for (int j = 0; j < clickCount; j++)
                                        {
                                            if (state.Type == AutomationType.ClickableImage)
                                            {
                                                Click(rect.Center());
                                            }
                                            else if (state.Type == AutomationType.ClickableImageOffset)
                                            {
                                                Click(rect.X + state.X, rect.Y + state.Y);
                                            }
                                            else if (state.Type == AutomationType.MoveMouseImage)
                                            {
                                                MoveMouse(rect.Center());
                                            }
                                            else if (state.Type == AutomationType.MoveMouseImageOffset)
                                            {
                                                MoveMouse(rect.X + state.X, rect.Y + state.Y);
                                            }
                                        }                                        
                                    }
                                }
                                break;                            
                        }
                        if (!success)
                        {
                            return false;
                        }
                    }
                }

            }
            finally
            {
                DisposeBitmap(screenshot);
                DisposeBitmap(image);
            }

            return true;
        }

        private Bitmap UpdateScreenshot(Bitmap screenshot)
        {
            DisposeBitmap(screenshot);
            return SnippingTool.CreateScreenshot();
        }

        public bool Run()
        {
            return Run(false);
        }

        private void DisposeBitmap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                try
                {
                    bitmap.Dispose();
                }
                catch { }
            }
        }

        private void Click(AutomationState state, Rectangle referenceRect)
        {
            Click(state.X + referenceRect.X, state.Y + referenceRect.Y);
        }

        private void Click(Point pos)
        {
            Click(pos.X, pos.Y);
        }

        private void Click(int x, int y)
        {
            Input.PInvoke.Click(x, y);
        }

        private void MoveMouse(Point pos)
        {
            MoveMouse(pos.X, pos.Y);
        }

        private void MoveMouse(int x, int y)
        {
            Input.PInvoke.SetCursorPos(x, y);
        }

        private void SendOpenImage()
        {
            SendKeys("ocr_image.png");
            SendKey(Keys.Enter);
        }

        private void SendSelectAll()
        {
            Input.PInvoke.KeyDown(Keys.ControlKey);
            Input.PInvoke.KeyDown(Keys.A);
            Thread.Sleep(100);
            Input.PInvoke.KeyUp(Keys.A);
            Input.PInvoke.KeyUp(Keys.ControlKey);
        }

        private void SendCopy()
        {
            Input.PInvoke.KeyDown(Keys.ControlKey);
            Input.PInvoke.KeyDown(Keys.C);
            Input.PInvoke.KeyUp(Keys.C);
            Input.PInvoke.KeyUp(Keys.ControlKey);
        }

        private void SendKeys(string text)
        {
            foreach(char character in text)
            {
                Input.PInvoke.SendCharUnicode(character);
            }
        }

        public void SendKey(Keys key)
        {
            Input.PInvoke.KeyDown(key);
            Input.PInvoke.KeyUp(key);
        }
    }

    class AutomationState
    {        
        private List<AutomationElement> elements = new List<AutomationElement>();

        public AutomationType Type { get; set; }
        public string Path { get; set; }
        public int Value { get; set; }
        public Keys Key { get; set; }
        public byte Variance { get; set; }

        public int X
        {
            get { return elements.Count > 0 ? elements[0].X : 0; }
        }
        public int Y
        {
            get { return elements.Count > 0 ? elements[0].Y : 0; }
        }

        public AutomationState(AutomationType type)
        {
            Type = type;
        }

        public AutomationState(AutomationType type, string path)
        {
            Type = type;
            Path = path;
        }

        public AutomationState AddElement(int x, int y, string color)
        {
            return AddElement(x, y, string.IsNullOrEmpty(color) ? default(Color) :
                System.Drawing.ColorTranslator.FromHtml(color));
        }

        public AutomationState AddElement(int x, int y, Color color)
        {
            elements.Add(new AutomationElement(x, y, color));
            return this;
        }

        public AutomationState SetVariance(byte variance)
        {
            Variance = variance;
            return this;
        }

        public Rectangle Find(Bitmap screenshot, Bitmap image)
        {
            if (Variance > 0)
            {
                return ImageFinder.Find(screenshot, image, Variance / 255.0);
            }
            else
            {
                return ImageFinder.Find(screenshot, image);
            }
        }

        public bool IsMatch(Bitmap bitmap, Rectangle referenceRect)
        {
            if (elements.Count == 0)
                return false;            

            foreach (AutomationElement element in elements)
            {
                int x = referenceRect.X + element.X;
                int y = referenceRect.Y + element.Y;
                Color color;

                if (x < 0 || y < 0 || x >= bitmap.Width || y >= bitmap.Height ||
                    ((color = bitmap.GetPixel(x, y)) != element.Color) && element.Color != default(Color))
                {
                    return false;
                }
            }
            return true;
        }

        public Bitmap LoadImage()
        {
            try
            {
                return (Bitmap)Bitmap.FromFile(Path);
            }
            catch { }
            return null;
        }
    }

    class AutomationElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public AutomationElement(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }

    enum AutomationType
    {
        Wait,//Wait X milliseconds

        ReferenceImage,//Reference image to use with relative clicks
        StartImage,//Start image to be clicked if nothing else is found

        WaitImage,//Wait until this image is found
        WaitRelativeImage,//Wait until this image is found relative to the reference image

        ClickableImage,//Find the image and click it
        ClickableImageOffset,//Find the image and click it (with an offset)

        AbsoluteClick,//Click the screen at this position
        RelativeClick,//Click the screen relative to the reference image

        MoveMouseImage,//Find the image and move the mouse to it
        MoveMouseImageOffset,//Find the image and move mouse to it (with an offset)

        OpenImage,//Open image file
        
        SelectAll,//Send Ctrl+A
        Copy,//Send Ctrl+C
        
        Text,//Send text
        TextReturn,//Send text and press the return key

        Key,//Send a key
    }
}