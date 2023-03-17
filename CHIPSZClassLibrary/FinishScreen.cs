using StereoKit;

namespace CHIPSZClassLibrary
{
  public class FinishScreen
  {

    private Pose finishPose;

    public FinishScreen() {
      finishPose = new Pose(new Vec3(0, .2f, -.3f), Quat.LookDir(1, 0, 1));
    }

    public void Update() {
      GameOverScreen();
    }

    // Initial Screen where the user will be prompt after the timer runs out
    public void GameOverScreen() {
      UI.WindowBegin("\tGame Over", ref finishPose, new Vec2(20, 10) * U.cm, UIWin.Normal);
      UI.WindowEnd();
    }

  }
}



