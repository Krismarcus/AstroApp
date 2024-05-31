using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.Tools
{
    internal class SegmentSelectionManager
    {
        private static SegmentSelectionManager _instance;

        public static SegmentSelectionManager Instance => _instance ??= new SegmentSelectionManager();

        public Border SelectedSegmentBorder { get; set; }

        private SegmentSelectionManager() { }

        public void SelectSegment(Border newSelectedBorder)
        {
            // Remove shadow from the previously selected segment
            if (SelectedSegmentBorder != null && SelectedSegmentBorder != newSelectedBorder)
            {
                SelectedSegmentBorder.Shadow = new Shadow { Brush = Brush.Black, Opacity = 0, Offset = new Point(0, 0), Radius = 10 };
            }

            // Add shadow to the newly selected segment
            if (newSelectedBorder != null)
            {
                newSelectedBorder.Shadow = new Shadow
                {
                    Brush = new SolidColorBrush(ColorManager.GetResourceColor("PrimaryLightText", Colors.Transparent)),
                    Radius = 5,
                    Opacity = 1,
                    Offset = new Point(0, 5)
                };
            }

            SelectedSegmentBorder = newSelectedBorder;
        }


    }
}
