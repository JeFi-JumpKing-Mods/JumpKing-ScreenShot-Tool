using JumpKing.PauseMenu.BT.Actions;

namespace ScreenShotTool.Menu;
public class ToggleCurrentScreen : ITextToggle
{
    public ToggleCurrentScreen() : base(ScreenShotTool.Preferences.isCurrentScreen)
    {
    }

    protected override string GetName() => "Only current screen";

    protected override void OnToggle()
    {
        ScreenShotTool.Preferences.isCurrentScreen = toggle;
    }
}