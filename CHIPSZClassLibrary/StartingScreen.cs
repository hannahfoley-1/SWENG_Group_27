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
        public bool inStart { get; private set; }
        public bool inDemo { get; private set; }
        public bool inGame { get; private set; }
        bool firstStepDone;
        bool secondStepDone;


        public StartingScreen()
        {
            this.windowPose = new Pose(winVec, Quat.LookDir(0, 0, 1));
            this.windowPose2 = new Pose(winVec2, Quat.LookDir(1, 0, 1));
            inStart = true;
            inDemo = false;
            inGame = false;
            this.firstStepDone = false;
            this.secondStepDone = false;

        }

        public void Draw(GameState gameState)
        {
            if (inStart)
            {
                UI.WindowBegin("  Welcome to this Game!", ref windowPose, new Vec2(20, 10) * U.cm, inGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   START  GAME   -->   "))
                {
                    inStart = false;
                    inGame = true;
                }
                else if (UI.Button("   START  DEMO   -->   "))
                {
                    inStart = false;
                    inDemo = true;
                }
                UI.WindowEnd();
            }
        }

        public bool PlayDemo1()
        {
            if (inDemo && !firstStepDone)
            {
                UI.WindowBegin("  Clench fist to spawn EARTH model   ", ref windowPose2, new Vec2(20, 10) * U.cm, inGame ? UIWin.Normal : UIWin.Body);
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
            if (inDemo && firstStepDone && !secondStepDone)
            {
                UI.WindowBegin("  Make palm to spawn FIRE and WATER models   ", ref windowPose2, new Vec2(20, 10) * U.cm, inGame ? UIWin.Normal : UIWin.Body);
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
            if (inDemo && firstStepDone && secondStepDone)
            {
                UI.WindowBegin("  Throw ball at target to win points   ", ref windowPose2, new Vec2(20, 10) * U.cm, inGame ? UIWin.Normal : UIWin.Body);
                if (UI.Button("   PLAY    GAME    -->   "))
                {
                    inDemo = false;
                    inGame = true;
                }
                UI.WindowEnd();
            }
        }
    }
}