using HarmonyLib;
using System.IO;
using System.Reflection;
using System;
using Drawing = System.Drawing;

using JumpKing;
using JumpKing.Level;
using JumpKing.Util.Tags;
using JumpKing.MiscSystems.Achievements;
using EntityComponent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenShot.Models
{
    public static class Printer
    {
        public const int WIDTH = Game1.WIDTH;
        public const int HEIGHT = Game1.HEIGHT;
        private static readonly RenderTarget2D OriginalTarget;
        private static readonly RenderTarget2D PrintingTarget;
        public static string SaveDir {get; set;}
        private static MethodInfo GetCurrentStats;
        private static object AchievementManagerInstance;
        private static Color[] Data = new Color[WIDTH*HEIGHT];

        static Printer() {
            Type achievementManagerType = AccessTools.TypeByName("JumpKing.MiscSystems.Achievements.AchievementManager");
            AchievementManagerInstance = AccessTools.Field(achievementManagerType, "instance").GetValue(null);
            GetCurrentStats = AccessTools.Method(achievementManagerType, "GetCurrentStats");

            OriginalTarget = Traverse.Create(Game1.instance).Field("m_render_target").GetValue<RenderTarget2D>();
            PrintingTarget = new RenderTarget2D(Game1.instance.GraphicsDevice, WIDTH, HEIGHT);
        }

        public static void SaveAllScreens() {
            int ticks = ((PlayerStats)GetCurrentStats.Invoke(AchievementManagerInstance, null))._ticks;

            int origIndex = Camera.CurrentScreen;
            Game1.instance.EndBatch();
            Game1.instance.GraphicsDevice.SetRenderTarget(PrintingTarget);

            var _current_screen = Traverse.Create(typeof(Camera)).Field("_current_screen");
            int start = Math.Max(0, ScreenShot.Preferences.StartIndex);
            int end = Math.Min(ScreenShot.Preferences.EndIndex+1, LevelManager.TotalScreens);
            for (int i=start; i<end; i++) {
            // for (int i=0; i<10; i++) {
                _current_screen.SetValue(i);
                string filePath = GetFullPath(i, ticks);
                SaveScreen(filePath);
            }

            _current_screen.SetValue(origIndex);
            Game1.instance.StartBatch();
            Game1.instance.GraphicsDevice.SetRenderTarget(OriginalTarget);
        }
        private static void SaveScreen(string filePath) {
            // Debug.WriteLine(filePath);
            Game1.instance.GraphicsDevice.Clear(Color.Black);

            Game1.instance.StartBatch();
            LevelManager.CurrentScreen.Draw();
            foreach (Entity entity in EntityManager.instance.Entities)
            {
                if (entity is JumpKing.Player.PlayerEntity || entity.GetType() == Type.GetType("JumpKing.MiscEntities.RavenEntity, JumpKing"))
                {
                    continue;
                }
                entity.Draw();
            }
            LevelManager.CurrentScreen.DrawForeground();
            foreach (Entity entity in EntityManager.instance.Entities)
            {
                if (entity is IForeground)
                {
                    (entity as IForeground).ForegroundDraw();
                }
            }
            Game1.instance.EndBatch();

            PrintingTarget.GetData<Color>(Data);
            using (Drawing.Bitmap bitmap = new Drawing.Bitmap(WIDTH, HEIGHT))
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        Color color = Data[x + y * WIDTH];
                        bitmap.SetPixel(x, y, Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
                    }
                }

                bitmap.Save(filePath, Drawing.Imaging.ImageFormat.Png);
            }
            // Don't use SaveAsPng in monogame, it will drop the data sometimes.
            // using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 1, useAsync: false))
            // {
            //     PrintingTarget.SaveAsPng(stream, WIDTH, HEIGHT);
            // }
        }
        private static string GetFullPath(int index, int tick) {
            string fileName = string.Format("screen{0:D3}_tick{1:D8}{2}.png", index+1, tick, (ScreenShot.isDrawRayManWall) ? "" : "_nR");
            return Path.Combine(SaveDir, fileName);
        }
    }
}
