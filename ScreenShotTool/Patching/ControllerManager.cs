using HarmonyLib;
using JK = JumpKing.Controller;
using System.Reflection;
using ScreenShotTool.Models;
using System;

namespace ScreenShotTool.Patching;
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
        Type type = typeof(JK.ControllerManager);
        MethodInfo Update = type.GetMethod(nameof(JK.ControllerManager.Update));
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