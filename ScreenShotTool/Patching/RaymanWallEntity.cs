using HarmonyLib;
using ScreenShotTool.Models;
using System;
using System.Reflection;

namespace ScreenShotTool.Patching;
public class RaymanWallEntity
{
    public RaymanWallEntity (Harmony harmony)
    {
        Type OldManEntity = AccessTools.TypeByName("JumpKing.Props.RaymanWall.RaymanWallEntity");
        MethodInfo Draw = OldManEntity.GetMethod("Draw");
        harmony.Patch(
            Draw,
            prefix: new HarmonyMethod(AccessTools.Method(typeof(RaymanWallEntity), nameof(preDraw)))
        );
    }

    private static void preDraw(ref object __instance) {
        if (Renderer.isRendering && !Renderer.isDrawRayManWall) {
            var m_fade = Traverse.Create(__instance).Field("m_fade");
            var m_alpha = Traverse.Create(m_fade.GetValue()).Field("m_alpha");
            m_alpha.SetValue(0f); 
        }
    }
}