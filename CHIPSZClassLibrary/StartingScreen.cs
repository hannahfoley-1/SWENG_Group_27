using CHIPSZClassLibrary;
using StereoKit;
using System.Collections;
using System.Collections.Generic;

namespace CHIPSZ
{
    public class StartingScreen
    {
        private Pose windowPose;
        private Pose windowPose2;
        private Vec3 winVec; 
        private Vec3 winVec2;
        private bool ifCloseStartGame;
        private bool ifCloseStartDemo;
        private bool endDemo;
        bool firstStepDone;
        bool secondStepDone;
        private Pose statisticsPose;
        private bool reset = false;
        private bool statistics = false;
        private bool exit = false;


        public StartingScreen()
        {
            this.winVec = new Vec3(Input.Head.position.x, Input.Head.position.y, -0.25f);
            this.winVec2 = new Vec3(Input.Head.position.x-0.15f, Input.Head.position.y, -0.25f);
            //this.winVec2 = new Vec3(Input.Head.position.x + (-0.2f), Input.Head.position.y + (0.2f), -0.25f);
            this.windowPose = new Pose(winVec, Quat.LookDir(0, 0, 1));
            this.windowPose2 = new Pose(winVec2, Quat.LookDir(1, 0, 1));
            this.ifCloseStartGame = true;
            this.ifCloseStartDemo = true;
            this.endDemo = false;
            this.firstStepDone = false;
            this.secondStepDone = false;
            this.statisticsPose = new Pose(new Vec3(0, .2f, -.3f), Quat.LookDir(0, 1, 1));

        }

        public bool GetIfStartGame()
        {
            return ifCloseStartGame;
        }

        public bool GetIfStartDemo()
        {
            return ifCloseStartDemo;
        }

        public bool GetIfEndDemo()
        {
            return endDemo;
        }

        public void SetIfStartGame(bool set)
        {
            ifCloseStartGame = set;
        }

        public void SetIfStartDemo(bool set)
        {
            ifCloseStartDemo = set;
        }


        public void Draw()
        {
            if (ifCloseStartGame != false && ifCloseStartDemo != false)
            {
                UI.WindowBegin("  GAME MENU   ", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                UI.SetThemeColor(UIColor.Primary, Color.Hex(0xF37E1100));
                UI.SetThemeColor(UIColor.Background, Color.Hex(0x23700700));
                UI.SetThemeColor(UIColor.Common, Color.Hex(0x09A0D000));
                if (UI.Button("   START  GAME   -->   "))
                {
                    ifCloseStartGame = false; reset = true;
                }
                if (UI.Button("   START  DEMO   -->   ")){ ifCloseStartDemo = false; reset = true; }
                //if (UI.Button("Try again")) reset = true;
                if (UI.Button("   HIGH  SCORES   -->   ")) statistics = true;
                if (UI.Button("   LEAVE  GAME   -->   ")) exit = true;
                UI.WindowEnd();
            }
        }

        public bool PlayDemo1()
        {
            if (!endDemo && !firstStepDone)
            {
                UI.WindowBegin("  Make a palm towards your face to display menu   ", ref windowPose2, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   NEXT    -->   "))
                {
                    firstStepDone = true;
                }
                UI.WindowEnd();
            }
            return firstStepDone;
        }

        public bool PlayDemo2()
        {
            if (!endDemo && firstStepDone && !secondStepDone)
            {
                UI.WindowBegin("  Select element   ", ref windowPose2, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   NEXT    -->   "))
                {
                    secondStepDone = true;
                }
                UI.WindowEnd();
            }
            return secondStepDone;
        }

        public void PlayDemo3()
        {
            if (!endDemo && firstStepDone && secondStepDone)
            {
                UI.WindowBegin("  Throw by moving your palm outwards away from you   ", ref windowPose2, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   PLAY    GAME    -->   "))
                {
                    endDemo = true;
                }
                UI.WindowEnd();
            }
        }

        public void Update(List<int> scores)
        {
            if (!OptionSelected()) Draw();
            else if (statistics) StatisticsScreen(scores);
        }

        private void StatisticsScreen(List<int> scores)
        {
            UI.WindowBegin("Your Performance", ref windowPose, new Vec2(35, 0) * U.cm, UIWin.Normal);
            for (int i = 0; i < scores.Count; i++)
                UI.Text("Player: " + scores[i], TextAlign.Center);

            if (UI.Button("BACK")) Back();
            UI.WindowEnd();
        }

        private void Back() => statistics = false;
        public bool OptionSelected() => reset || statistics || exit;
        public bool IsReset() => reset;
        public bool IsExit() => exit;
    }
}