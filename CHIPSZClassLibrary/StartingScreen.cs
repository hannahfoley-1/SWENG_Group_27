using CHIPSZClassLibrary;
using StereoKit;

namespace CHIPSZ
{
    public class StartingScreen
    {
        private Pose windowPose;
        private Pose windowPose2;
        private Vec3 winVec = new Vec3(0f, 0.2f, -0.3f); //(x,y,z)
        private Vec3 winVec2 = new Vec3(-0.2f, 0.2f, -0.3f); //(x,y,z)
        private bool ifCloseStartGame;
        private bool ifCloseStartDemo;
        private bool endDemo;
        bool firstStepDone;
        bool secondStepDone;


        public StartingScreen()
        {
            this.windowPose = new Pose(winVec, Quat.LookDir(0, 0, 1));
            this.windowPose2 = new Pose(winVec2, Quat.LookDir(1, 0, 1));
            this.ifCloseStartGame = true;
            this.ifCloseStartDemo = true;
            this.endDemo = false;
            this.firstStepDone = false;
            this.secondStepDone = false;


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
                    ifCloseStartGame = false;
                }
                else if (UI.Button("   START  DEMO   -->   "))
                {
                    ifCloseStartDemo = false;
                }
                UI.WindowEnd();
            }
        }

        public bool PlayDemo1()
        {
            if (!endDemo && !firstStepDone)
            {
                UI.WindowBegin("  Clench fist to spawn EARTH model   ", ref windowPose2, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
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
                UI.WindowBegin("  Make palm to spawn FIRE and WATER models   ", ref windowPose2, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
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
                UI.WindowBegin("  Throw ball at target to win points   ", ref windowPose2, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   PLAY    GAME    -->   "))
                {
                    endDemo = true;
                }
                UI.WindowEnd();
            }
        }
    }
}