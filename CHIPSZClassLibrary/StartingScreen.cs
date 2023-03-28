using CHIPSZClassLibrary;
using StereoKit;

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
                UI.WindowBegin("  Welcome to this Game!", ref windowPose, new Vec2(20, 10) * U.cm, ifCloseStartGame ? UIWin.Normal : UIWin.Body);
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