using Cyotek.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.UI
{
    //https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox
    //http://www.cyotek.com/blog/adding-drag-handles-to-an-imagebox-to-allow-resizing-of-selection-regions

    class OcrImagePanel : ImageBoxEx, ISupportInitialize
    {
        private RectangleF lastSelection;
        private bool isUpdating = false;

        public Image SourceImage { get; set; }
        public Image SelectionImage { get; set; }
        public Rectangle Selection { get; set; }

        private bool editable;
        public bool Editable
        {
            get { return editable; }
            set
            {
                editable = value;
                if (editable)
                {
                    base.SelectionMode = ImageBoxSelectionMode.Rectangle;
                }
                else
                {
                    base.SelectionMode = ImageBoxSelectionMode.None;
                }
                lastSelection = RectangleF.Empty;
                base.SelectionRegion = new RectangleF(Selection.X, Selection.Y, Selection.Width, Selection.Height);
                UpdateSelectionImage(true);                
            }
        }

        public OcrImagePanel()
        {            
            base.AutoPan = true;
            //base.GridDisplayMode = ImageBoxGridDisplayMode.None;
            this.ImageBorderStyle = ImageBoxBorderStyle.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;            
            base.CenterDragHandles = true;
            base.ZoomRequiresCtrl = true;
            base.ShowPixelGrid = true;

            this.KeyUp += OcrImagePanel_KeyUp;            
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Form form = FindForm();
            if (form != null)
            {
                this.FindForm().Activated += OcrImagePanel_Activated;
                this.FindForm().Deactivate += OcrImagePanel_Deactivate;
            }
        }

        void OcrImagePanel_Deactivate(object sender, EventArgs e)
        {
            if (lastSelection != base.SelectionRegion)
            {
                UpdateSelectionImage(false);
            }
        }

        void OcrImagePanel_Activated(object sender, EventArgs e)
        {
            if (lastSelection != base.SelectionRegion)
            {
                UpdateSelectionImage(false);
            }
        }

        void OcrImagePanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.C | Keys.Control) && SelectionImage != null)
            {
                Clipboard.SetImage(SelectionImage);
            }
            if (e.KeyData == (Keys.V | Keys.Control))
            {
                Image image = null;
                try
                {
                    image = Clipboard.GetImage();
                }
                catch
                {
                }
                if (image != null)
                {
                    SetImage(image);
                }
            }
        }

        private Image GetSelection(Image image, Rectangle selection)
        {
            return SnippingTool.GetSelection(image, selection);
        }

        public void SetImage(Image image)
        {
            SetImage(image, Rectangle.Empty);
        }

        public void SetImage(Image image, Rectangle selection)
        {
            if (SourceImage != null)
            {
                SourceImage.Dispose();
                SourceImage = null;
            }            
            Selection = selection;
            SourceImage = image;

            if (Editable)
            {
                base.SelectionRegion = new RectangleF(Selection.X, Selection.Y, Selection.Width, Selection.Height);
            }

            UpdateSelectionImage(true);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {            
            base.OnMouseUp(e);
            if (lastSelection != base.SelectionRegion)
            {
                UpdateSelectionImage(false);
            }
        }

        private void UpdateSelectionImage(bool center)
        {
            if (!editable && base.SelectionMode == ImageBoxSelectionMode.Rectangle)
            {
                base.SelectionMode = ImageBoxSelectionMode.None;
            }

            if (SourceImage == null)
                return;

            //isUpdating = true;

            if (SelectionImage != null)
            {
                SelectionImage.Dispose();
                SelectionImage = null;
            }

            if (Editable)
            {
                Selection = new Rectangle((int)SelectionRegion.X, (int)SelectionRegion.Y, Math.Abs((int)SelectionRegion.Width), Math.Abs((int)SelectionRegion.Height));
            }
            if(Selection.Width == 0 || Selection.Height == 0)
                SelectionImage = new Bitmap(SourceImage.Width, SourceImage.Height);
            else
                SelectionImage = new Bitmap(Selection.Width, Selection.Height);
            using (Graphics graphics = Graphics.FromImage(SelectionImage))
            {
                graphics.DrawImage(SourceImage, new Rectangle(0, 0, SelectionImage.Width, SelectionImage.Height), Selection, GraphicsUnit.Pixel);
            }            

            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }

            if (Editable)
            {
                Image = QuickImageCopy(SourceImage);
                base.SelectionRegion = new RectangleF(Selection.X, Selection.Y, Selection.Width, Selection.Height);
                if (center)
                {
                    base.CenterAt(Selection.Center());
                }                
            }
            else
            {
                Image = QuickImageCopy(SelectionImage);
                base.SelectionRegion = RectangleF.Empty;
            }

            lastSelection = base.SelectionRegion;

            //isUpdating = false;
            //Invalidate();
        }

        private Image QuickImageCopy(Image image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(image, Point.Empty);
            }
            return result;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (isUpdating)
            {
                this.DrawGrid(pe.Graphics);

                // draw the grid
                if (this.ShowPixelGrid && !this.VirtualMode)
                {
                    this.DrawPixelGrid(pe.Graphics);
                }
                return;
            }
            base.OnPaint(pe);
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
        }
    }
}
