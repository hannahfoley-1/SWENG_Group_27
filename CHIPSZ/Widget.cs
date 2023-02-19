﻿using StereoKit;
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

        public void drawHighScores()
        {
            UI.WindowBegin("High Scores", ref position, new Vec2(10, 0) * U.cm, showHeader ? UIWin.Normal : UIWin.Body);
            //in the future this will be gotten using actual data
            UI.Text("Plaer 1 - 300");
            UI.Text("Player 2 - 130");
            UI.Text("Player 3 - 10");
            UI.WindowEnd();
        }
    }

}

