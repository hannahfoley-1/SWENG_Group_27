using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZ
{
    internal class FinishScreen
    {
        private Pose finishPose;

        public FinishScreen()
        {
            finishPose = new Pose(new Vec3(0, .2f, -.3f), Quat.LookDir(0, 0, 1));
        }

        public void Update()
        {
            GameOverScreen();
        }

        // Initial Screen where the user will be prompt after the timer runs out
        public void GameOverScreen()
        {
            UI.WindowBegin("", ref finishPose, new Vec2(20, 10) * U.cm, UIWin.Body);
            UI.PopTextStyle();
            UI.Text("Game Over", TextAlign.Center);
            UI.HSeparator();
            UI.Button("Try again");
            UI.Button("See game statistics");
            UI.Button("Leave the game");
            UI.WindowEnd();
        }

    }
}

