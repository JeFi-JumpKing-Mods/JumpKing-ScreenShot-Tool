using System.Reflection;
using HarmonyLib;
using JumpKing;
using JumpKing.PauseMenu.BT.Actions;
using Microsoft.Xna.Framework;

namespace ScreenShot.Menu;
public class SliderEndIndex : ISlider
{
    const int steps = 168;
    public SliderEndIndex() : base(ScreenShot.Preferences.EndIndex / (float)steps)
    {
        FieldInfo STEPS = AccessTools.Field(typeof(ISlider), "STEPS");
        STEPS.SetValue(this, steps/2);
    }

    protected override void IconDraw(float p_value, int x, int y, out int new_x)
    {
        Game1.spriteBatch.DrawString(
            Game1.instance.contentManager.font.MenuFont,
            "End",
            new Vector2(x, y - ScreenShot.OffsetY / 4),
            Color.White);
        new_x = x + ScreenShot.OffsetX + 5;
        Game1.spriteBatch.DrawString(
            Game1.instance.contentManager.font.MenuFont,
            ((int)(168 * p_value)+1).ToString(),
            new Vector2(new_x + 65, y - ScreenShot.OffsetY / 4),
            Color.White);
    }

    protected override void OnSliderChange(float p_value)
    {
        ScreenShot.Preferences.EndIndex = (int)(steps * p_value);
    }
}