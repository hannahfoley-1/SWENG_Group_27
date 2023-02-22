using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StereoKit;
using StereoKit.Framework;
using Windows.Devices.PointOfService;
using System;

namespace CHIPSZ
{
    internal class starting_screen
    {
        private Pose windowPose;
        Vec3 winVec = new Vec3(-0.1f, 0.2f, -0.3f);
        bool ifClose = true;

        public starting_screen()
        {
            this.windowPose = new Pose(winVec, Quat.LookDir(1, 0, 1));
        }

        public void Draw()
        {
            if (ifClose != false)
            {
                UI.WindowBegin("  Welcome to this Game!", ref windowPose, new Vec2(20, 10) * U.cm, ifClose ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   START  GAME   -->   "))
                {
                    ifClose = false;
                }

                UI.WindowEnd();
            }

        }
    }
}