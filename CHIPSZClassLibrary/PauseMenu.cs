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
        public static Widget pauseWidget = new Widget();
        
        public PauseMenu() 
        {
            pauseWidget.SetWindowName("PAUSE");
            pauseWidget.SetPosition(new Pose(0.4f, 0, -0.25f, Quat.LookDir(-1.5f, 0, 2)));  // default pause widget position
            pauseWidget.SetShowHeader(true);
            pauseWidget.AddButton("Pause Game");
        }

        public static void setPose(Pose pose)
        {
            pauseWidget.SetPosition(pose);
        }

        public static bool pausePressed(bool paused)
        {
            Console.WriteLine("Pause button pressed!");
            return UI.Toggle("Pause Game", ref paused);
        }

        public void Draw()
        {
            pauseWidget.Draw();
        }


    }
}
