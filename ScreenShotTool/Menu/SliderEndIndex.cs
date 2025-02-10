using System.Reflection;
using BehaviorTree;
using ErikMaths;
using HarmonyLib;
using JumpKing;
using JumpKing.Controller;
using JumpKing.PauseMenu.BT.Actions;
using Microsoft.Xna.Framework;

namespace ScreenShotTool.Menu;
public class SliderEndIndex : ISlider
{
    public readonly static SliderEndIndex Instance;
    private Utils.Timer timer;
    private const float RepeatThreshold = 0.5f;
    private const float RepeatInterval = 0.05f;

    const int steps = 168;
    static SliderEndIndex() {
        Instance = new SliderEndIndex();
    }
    public SliderEndIndex() : base(ScreenShotTool.Preferences.EndIndex / (float)steps)
    {
        FieldInfo STEPS = AccessTools.Field(typeof(ISlider), "STEPS");
        STEPS.SetValue(this, steps);
        timer = new Utils.Timer(RepeatThreshold, RepeatInterval);
        timer.Reset();
    }

    public static void SetIndex(int index) {
        float value =  ErikMath.Clamp(index/(float)steps, 0f, 1f);
        Instance.SetValue(value);
        Instance.OnSliderChange(value);
    }

    protected override BTresult MyRun(TickData p_data) {
        BTresult result = base.MyRun(p_data);

        PadState padState = ControllerManager.instance.GetPadState();
        if (padState.left!=padState.right) {
            if (timer.Update(p_data.delta_time)) {
                float value = Traverse.Create(this).Field<float>("m_value").Value;
                int STEPS = Traverse.Create(this).Field<int>("STEPS").Value;
                if (padState.left) {
                    value -= 1f / (float)STEPS;
                }
                else if (padState.right)
                {
                    value += 1f / (float)STEPS;
                }

                value = ErikMath.Clamp(value, 0f, 1f);
                SetValue(value);
                OnSliderChange(value);
                result = BTresult.Success;
            }
        }
        else {
            timer.Reset();
        }

        return result;
    }
    protected override void IconDraw(float p_value, int x, int y, out int new_x)
    {
        Game1.spriteBatch.DrawString(
            Game1.instance.contentManager.font.MenuFont,
            "End",
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
        ScreenShotTool.Preferences.EndIndex = (int)(steps * p_value);
    }
}