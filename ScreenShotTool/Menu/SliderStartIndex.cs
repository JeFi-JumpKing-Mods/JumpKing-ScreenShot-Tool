using System.Reflection;
using ErikMaths;
using HarmonyLib;
using JumpKing;
using JumpKing.PauseMenu.BT.Actions;
using Microsoft.Xna.Framework;

namespace ScreenShotTool.Menu;
public class SliderStartIndex : ISlider
{
    public readonly static SliderStartIndex Instance;
    const int steps = 168;
    static SliderStartIndex() {
        Instance = new SliderStartIndex();
    }
    public SliderStartIndex() : base(ScreenShotTool.Preferences.StartIndex / (float)steps)
    {
        FieldInfo STEPS = AccessTools.Field(typeof(ISlider), "STEPS");
        STEPS.SetValue(this, steps/2);
    }

    public static void SetIndex(int index) {
        float value =  ErikMath.Clamp(index/(float)steps, 0f, 1f);
        Instance.SetValue(value);
        Instance.OnSliderChange(value);
    }

    protected override void IconDraw(float p_value, int x, int y, out int new_x)
    {
        Game1.spriteBatch.DrawString(
            Game1.instance.contentManager.font.MenuFont,
            "Start",
            new Vector2(x, y - ScreenShotTool.OffsetY / 4),
            Color.White);
        new_x = x + ScreenShotTool.OffsetX + 5;
        Game1.spriteBatch.DrawString(
            Game1.instance.contentManager.font.MenuFont,
            ((int)(168 * p_value)+1).ToString(),
            new Vector2(new_x + 65, y - ScreenShotTool.OffsetY / 4),
            Color.White);
    }

    protected override void OnSliderChange(float p_value)
    {
        ScreenShotTool.Preferences.StartIndex = (int)(steps * p_value);
    }
}