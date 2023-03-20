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
            this.pauseWidget.SetWindowName("PAUSE");
            this.pauseWidget.SetPosition(new Pose(0.4f, 0, -0.25f, Quat.LookDir(-1.5f, 0, 2)));  // default pause widget position
            this.pauseWidget.SetShowHeader(true);
            this.pauseWidget.AddButton("Pause Game");
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
            pauseWidget.Draw();
            if (UI.Button("Pause Game"))
            {
                bool set = !paused;
                SetPaused(set);
            }
        }


    }
}
