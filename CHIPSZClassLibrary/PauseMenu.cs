using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZClassLibrary
{
    public class PauseMenu
    {
        private Widget pauseWidget = new Widget();
        public bool paused;

        public PauseMenu()
        {
            this.pauseWidget.SetWindowName("PAUSE MENU");
            this.pauseWidget.SetPosition(new Pose(0.4f, 0, -0.25f, Quat.LookDir(-1.5f, 0, 2)));  // default pause widget position
            this.pauseWidget.SetShowHeader(true);
            //this.pauseWidget.AddButton("Pause Game");
            this.paused = false;
        }

        public void SetPose(Pose pose)
        {
            this.pauseWidget.SetPosition(pose);
        }

        public bool GetPaused()
        {
            return this.paused;
        }

        public void SetPaused(bool set)
        {
            this.paused = set;
        }

        public void Draw()
        {
            //pauseWidget.Draw();
            UI.WindowBegin(pauseWidget.windowName, ref pauseWidget.position, new Vec2(20, 0) * U.cm);
            if (pauseWidget.buttonLabels.Count > 0)
            {
                for (int i = 0; i < pauseWidget.buttonLabels.Count; i++)
                {
                    UI.Button((string)pauseWidget.buttonLabels[i]);
                }
            }
            if (paused == false)
            {
                if (UI.Button("Pause"))
                {
                    bool set = !paused;
                    SetPaused(set);
                }
            }
            else
            {
                if (UI.Button("Resume"))
                {
                    bool set = !paused;
                    SetPaused(set);
                }
            }
            UI.WindowEnd();
        }


    }
}
