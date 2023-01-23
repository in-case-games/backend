using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.WPF.Services
{
    public class ResizeMainWindowService
    {
        private WindowMeasurements? WindowMeasurements { get; set; }

        public event Action<WindowMeasurements>? OnResizeMainWindow;

        public void ChangeSizeMainWindow(WindowMeasurements? windowMeasurements)
        {
            WindowMeasurements = windowMeasurements;
            OnResizeMainWindow?.Invoke(WindowMeasurements ?? new(8000, 8000, 450, 450));
        }
    }

    public class WindowMeasurements
    {
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MinHeight { get; set; }
        public int MinWidth { get; set; }

        public WindowMeasurements(
            int maxHeight,
            int maxWidth,
            int minHeight,
            int minWidth) 
        { 
            MaxHeight = maxHeight;
            MaxWidth = maxWidth;
            MinHeight = minHeight;
            MinWidth = minWidth;
        }
    }
}
