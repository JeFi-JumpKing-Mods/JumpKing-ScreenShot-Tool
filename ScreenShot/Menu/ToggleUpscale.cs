using JumpKing.PauseMenu.BT.Actions;

namespace ScreenShot.Menu
{
    public class ToggleUpscale : ITextToggle
    {
        public ToggleUpscale() : base(ScreenShot.Preferences.DrawRayManWall)
        {
        }

        protected override string GetName() => "Up-Scale to 1080p";

        protected override void OnToggle()
        {
            ScreenShot.Preferences.Upscale = toggle;
        }
    }
}
