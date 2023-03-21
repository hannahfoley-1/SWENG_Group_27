using StereoKit;

namespace CHIPSZ
{          
    // TODO: Style header, body and buttons

    public class FinishScreen
    {
        private Pose finishPose;
        private Pose statisticsPose;

        private bool reset = false;
        private bool statistics = false;
        private bool exit = false;
        

        public FinishScreen()
        {
            finishPose = new Pose(new Vec3(0, .2f, -.3f), Quat.LookDir(0, 0, 1));
            statisticsPose = new Pose(new Vec3(0, .2f, -.3f), Quat.LookDir(0, 1, 1));

        }

        public void Update()
        {
            if (!OptionSelected()) GameOverScreen();
            else if (statistics) StatisticsScreen();
            
        }

        // Initial Screen where the user will be prompt after the timer runs out
        public void GameOverScreen()
        {
            UI.WindowBegin("", ref finishPose, new Vec2(20, 10) * U.cm, UIWin.Body);
            UI.Text("Game Over", TextAlign.Center);
            UI.HSeparator();
            if (UI.Button("Try again")) reset = true;
            if (UI.Button("See game statistics")) statistics = true;
            if (UI.Button("Leave the game")) exit = true;
            UI.WindowEnd();
        }

        private void StatisticsScreen()
        {
            UI.WindowBegin("Your Performance", ref finishPose, new Vec2(35, 0) * U.cm, UIWin.Normal); 
            UI.WindowEnd();
        }

        public bool OptionSelected() =>  reset || statistics || exit;
        public bool IsReset() => reset;
        public bool IsExit() => exit;

    }
}
