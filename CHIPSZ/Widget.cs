using StereoKit;
using System;
using Windows.UI.Xaml.Controls;

namespace CHIPSZ
{
    class Widget
    {
        private String windowName;
        private Pose position;
        private float slider;
        Boolean showHeader;

        public Widget()
        {
            windowName = "Window";
            position = Pose.Identity;
            slider = 0;
            showHeader = true;
        }

        public void setWindowName(String windowName)
        { this.windowName = windowName; }

        public void setPosition(Pose position) 
        { this.position = position; }

        public void setSlider(float slider)
        { this.slider = slider; }

        public void setShowHeader(Boolean showHeader) 
        { this.showHeader = showHeader; }

        public void draw()
        {
            UI.WindowBegin(windowName, ref position, new Vec2(20, 0) * U.cm, showHeader ? UIWin.Normal : UIWin.Body);
            UI.Toggle("Show Header", ref showHeader);
            UI.Label("Slide");
            UI.SameLine();
            UI.HSlider("slider", ref slider, 0, 1, 0.2f, 72 * U.mm);
            UI.WindowEnd();
        }
    }

}

