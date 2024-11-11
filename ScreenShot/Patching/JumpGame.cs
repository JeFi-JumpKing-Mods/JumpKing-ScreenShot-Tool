using HarmonyLib;
using System.Reflection;

using JK = JumpKing;
using ScreenShot.Models;

namespace ScreenShot.Patching
{
    public class JumpGame
    {
        public JumpGame (Harmony harmony)
        {
            MethodInfo Draw = typeof(JK.JumpGame).GetMethod(nameof(JK.JumpGame.Draw));
            harmony.Patch(
                Draw,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(JumpGame), nameof(postDraw)))
            );
        }

        private static void postDraw() {
            if (JumpKing.GameManager.GameLoop.instance.IsRunning() && KeyboardState.isPressed(Microsoft.Xna.Framework.Input.Keys.F12)) {
                if (!ScreenShot.Preferences.DrawRayManWall) {
                    ScreenShot.isDrawRayManWall = false;
                }
                Printer.SaveAllScreens();
                ScreenShot.isDrawRayManWall = true;
            }
        }
    }
}