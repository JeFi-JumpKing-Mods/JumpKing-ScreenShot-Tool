using JumpKing.PauseMenu.BT.Actions;

namespace ScreenShot.Menu;
public class ToggleSRayManWall : ITextToggle
{
    public ToggleSRayManWall() : base(ScreenShot.Preferences.DrawRayManWall)
    {
    }

    protected override string GetName() => "Draw RayManWall";

    protected override void OnToggle()
    {
        ScreenShot.Preferences.DrawRayManWall = toggle;
    }
}
