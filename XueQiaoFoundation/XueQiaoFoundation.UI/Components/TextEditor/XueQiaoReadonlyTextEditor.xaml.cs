using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XueQiaoFoundation.UI.Components.TextEditor
{
    /// <summary>
    /// XueQiaoReadonlyTextEditor.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XueQiaoReadonlyTextEditor
    {
        public XueQiaoReadonlyTextEditor()
        {
            InitializeComponent();
            Setup();
        }
        
        public string EditorText
        {
            get { return (string)GetValue(EditorTextProperty); }
            set { SetValue(EditorTextProperty, value); }
        }

        /// <summary>
        /// 通过依赖属性'EditorText'。可以通过绑定来设置编辑器文本
        /// </summary>
        public static readonly DependencyProperty EditorTextProperty =
            DependencyProperty.Register("EditorText", typeof(string), typeof(XueQiaoReadonlyTextEditor), new PropertyMetadata(EditorText_Changed));

        private static void EditorText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var xqEditor = d as XueQiaoReadonlyTextEditor;
            if (xqEditor == null) return;

            var rtbEditor = xqEditor.rtbEditor;
            if (rtbEditor == null) return;

            var newText = e.NewValue as string;
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Text = newText;
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Text = text;
        }

        private void Setup()
        {
            var systemFontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            CmbFontFamily.ItemsSource = systemFontFamilies;
            CmbFontFamily.SelectedItem = systemFontFamilies.FirstOrDefault(i=>i.Equals(rtbEditor.FontFamily));

            CmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            CmbFontSize.Text = $"{rtbEditor.FontSize}";
            
            UpdateZoom(1);
        }

        private void CmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rtbEditor.Document.FontFamily = CmbFontFamily.SelectedItem as FontFamily;
            //var allRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            //allRange.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void CmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var fontSize = Convert.ToDouble(CmbFontSize.Text);
                rtbEditor.Document.FontSize = fontSize;
            }
            catch (Exception) { }
            //rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void CmbFontSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
        
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        private void OpenInNotepadButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            var text = range.Text;

            Process.Start("notepad.exe");

            var loopIdx = 0;
            Process[] notepads = null;
            while (notepads == null || notepads.Length == 0)
            {
                loopIdx++;
                if (loopIdx > 5) break;
                notepads = Process.GetProcessesByName("notepad");
                Thread.Sleep(500);
            }

            if (notepads.Length == 0) return;
            if (notepads[0] != null)
            {
                Clipboard.SetText(text);
                SendMessage(FindWindowEx(notepads[0].MainWindowHandle, new IntPtr(0), "Edit", null), 0x000C, 0, text);
            }
        }
        
        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(Math.Floor(Math.Round((this.zoom + 0.1) * 10, 3)) / 10);
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(Math.Ceiling(Math.Round((this.zoom - 0.1) * 10, 3)) / 10);
        }

        private const double minZoom = 0.2;
        private const double maxZoom = 16;

        private void UpdateZoom(double _zoom)
        {
            _zoom = Math.Max(_zoom, minZoom);
            _zoom = Math.Min(_zoom, maxZoom);
            this.zoom = _zoom;
            rtbEditor.LayoutTransform = new ScaleTransform(scaleX: zoom, scaleY: zoom);
            ZoomTextBlock.Text = string.Format("{0:P0}", zoom);
        }

        private double zoom;
        
    }
}
