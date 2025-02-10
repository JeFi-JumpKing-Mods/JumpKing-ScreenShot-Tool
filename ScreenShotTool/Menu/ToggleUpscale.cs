using JumpKing.PauseMenu.BT.Actions;

namespace ScreenShotTool.Menu;
public class ToggleUpscale : ITextToggle
{
    public ToggleUpscale() : base(ScreenShotTool.Preferences.Upscale)
    {
    }

    protected override string GetName() => "Up-Scale to 1080p";

    protected override void OnToggle()
    {
        ScreenShotTool.Preferences.Upscale = toggle;
    }
}
