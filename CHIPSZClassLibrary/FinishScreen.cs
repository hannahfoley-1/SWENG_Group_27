using StereoKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZ
{
    internal class FinishScreen
    {
        private Pose finishPose;
        private bool reset = false;
        private bool statistics = false;
        private bool exit = false;
        

        public FinishScreen()
        {
            finishPose = new Pose(new Vec3(0, .2f, -.3f), Quat.LookDir(0, 0, 1));
        }

        public void Update()
        {
            if (!OptionSelected()) GameOverScreen();
            else if (reset) StartScreen();
            else if (statistics) StatisticsScreen();
            else Exit();
            
        }

        // Initial Screen where the user will be prompt after the timer runs out
        public void GameOverScreen()
        {
            UI.WindowBegin("", ref finishPose, new Vec2(20, 10) * U.cm, UIWin.Body);
            UI.PopTextStyle();
            UI.Text("Game Over", TextAlign.Center);
            UI.HSeparator();
            if (UI.Button("Try again")) reset = true;
            if (UI.Button("See game statistics")) statistics = true;
            if (UI.Button("Leave the game")) exit = true;
            UI.WindowEnd();
        }

        public void StartScreen()
        {

        }

        public void StatisticsScreen()
        {

        }

        public void Exit()
        {

        }

        public bool OptionSelected()
        {
            return reset || statistics || exit;
        }



        

    }
}
