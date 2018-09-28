using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ocr.UI.ContextMenus
{
    abstract class CompactContextMenu : ContextMenu
    {
        public WPFCompactForm Window { get; set; }

        public CompactContextMenu()
        {
            //Resources.Add(Popup.PopupAnimationProperty, PopupAnimation.None);
            IsVisibleChanged += CompactContextMenu_IsVisibleChanged;
        }

        public virtual void Build()
        {
        }

        void CompactContextMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            Popup myParentPopup = menu.Parent as Popup;
            myParentPopup.PopupAnimation = PopupAnimation.None;
        }
    }
}
