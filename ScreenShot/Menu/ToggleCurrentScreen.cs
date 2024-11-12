using JumpKing.PauseMenu.BT.Actions;

namespace ScreenShot.Menu
{
    public class ToggleCurrentScreen : ITextToggle
    {
        public ToggleCurrentScreen() : base(ScreenShot.Preferences.DrawRayManWall)
        {
        }

        protected override string GetName() => "Only current screen";

        protected override void OnToggle()
        {
            ScreenShot.Preferences.isCurrentScreen = toggle;
        }
    }
}
