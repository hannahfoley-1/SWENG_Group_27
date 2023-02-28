using StereoKit;

namespace CHIPSZ
{
    internal class StartingScreen
    {
        private Pose windowPose;
        private Vec3 winVec = new Vec3(-0.1f, 0.2f, -0.3f);
        private bool ifClose = true;

        public StartingScreen()
        {
            this.windowPose = new Pose(winVec, Quat.LookDir(1, 0, 1));
        }

        public bool GetIfClose()
        {
            return ifClose;
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