using JumpKing.PauseMenu.BT.Actions;

namespace ScreenShotTool.Menu;
public class ToggleSRayManWall : ITextToggle
{
    public ToggleSRayManWall() : base(ScreenShotTool.Preferences.DrawRayManWall)
    {
    }

    protected override string GetName() => "Draw RayManWall";

    protected override void OnToggle()
    {
        ScreenShotTool.Preferences.DrawRayManWall = toggle;
    }
}
