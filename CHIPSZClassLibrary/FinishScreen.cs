using StereoKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIPSZ
{          
    // TODO: Style header, body and buttons

    internal class FinishScreen
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
            else if (reset) StartScreen();
            else if (statistics) StatisticsScreen();
            else Exit();
            
        }

        // Initial Screen where the user will be prompt after the timer runs out
        public void GameOverScreen()
        {
            UI.WindowBegin("", ref finishPose, new Vec2(20, 10) * U.cm, UIWin.Body);
           // UI.PushTextStyle(HeaderStyle());
            UI.Text("Game Over", TextAlign.Center);
            //UI.PopTextStyle();
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
            UI.WindowBegin("Your Performance", ref finishPose, new Vec2(35, 0) * U.cm, UIWin.Normal); 

            UI.WindowEnd();
        }

        public void Exit()
        {

        }

       // public TextStyle HeaderStyle()
       // {
           // return Text.MakeStyle(Font.Default, 30, Color.Hex(0xDE0025));
        //}

       // public TextStyle BodyStyle()
       // {
       //     return new TextStyle();
       // }
       // public TextStyle ButtonStyle()
       // {
      //      return new TextStyle();
       // }

        public bool OptionSelected()
        {
            return reset || statistics || exit;
        }



        

    }
}
