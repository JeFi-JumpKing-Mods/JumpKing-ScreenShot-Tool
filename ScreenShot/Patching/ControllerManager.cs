using HarmonyLib;
using JK = JumpKing.Controller;
using System.Reflection;
using ScreenShot.Models;

namespace ScreenShot.Patching
{
    public class ControllerManager
    {
        // private static JK.IPad KeyboardPad;
        public ControllerManager (Harmony harmony)
        {
            // var padInstances = JK.ControllerManager.instance.GetConnectedPads();
            // foreach (var pad in padInstances) {
            //     if (pad.GetPad().GetSaveIdentifier() == "pc_keyboard_jump_king") {
            //         KeyboardPad = pad.GetPad();
            //     }
            // }

            MethodInfo Update = typeof(JK.ControllerManager).GetMethod(nameof(JK.ControllerManager.Update));
            harmony.Patch(
                Update,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(ControllerManager), nameof(postUpdate)))
            );
        }

        private static void postUpdate()
        {
            KeyboardState.Update();
        }
    }
}