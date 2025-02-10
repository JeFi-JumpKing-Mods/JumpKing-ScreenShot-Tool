using HarmonyLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using JumpKing;
using JumpKing.Mods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ScreenShot.Models;
using ScreenShot.Nodes;
using ScreenShot.Menu;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;

namespace ScreenShot;
[JumpKingMod(IDENTIFIER)]
public static class ScreenShot
{
    const string IDENTIFIER = "JeFi.ScreenShot";
    const string HARMONY_IDENTIFIER = "JeFi.ScreenShot.Harmony";
    const string SETTINGS_FILE = "JeFi.ScreenShot.Preferences.xml";
    private const string SCREENSHOT_DIR = "ScreenShot";

    public static string AssemblyPath { get; set; }
    public static Preferences Preferences { get; private set; }
    public static bool isDrawRayManWall = true;

    public static int OffsetX {get; private set; }
    public static int OffsetY {get; private set; }

    [BeforeLevelLoad]
    public static void BeforeLevelLoad()
    {
        AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
// #if DEBUG
//             Debugger.Launch();
//             Harmony.DEBUG = true;
//             Environment.SetEnvironmentVariable("HARMONY_LOG_FILE", $@"{AssemblyPath}\harmony.log.txt");
// #endif
        try
        {
            Preferences = XmlSerializerHelper.Deserialize<Preferences>($@"{AssemblyPath}\{SETTINGS_FILE}");
        }
        catch (Exception e)
        {
            Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
            Preferences = new Preferences();
        }
        Preferences.PropertyChanged += SaveSettingsOnFile;
        Preferences.StartIndex = Preferences.StartIndex;

        if (!Directory.Exists(Path.Combine(AssemblyPath, SCREENSHOT_DIR)))
        {
            Directory.CreateDirectory(Path.Combine(AssemblyPath, SCREENSHOT_DIR));
        }

        Harmony harmony = new Harmony(HARMONY_IDENTIFIER);

        new Patching.JumpGame(harmony);
        new Patching.ControllerManager(harmony);
        new Patching.RaymanWallEntity(harmony);

        SpriteFont spriteFont = Game1.instance.contentManager.font.MenuFont;

        Point start = spriteFont.MeasureString("Start").ToPoint();
        Point end = spriteFont.MeasureString("End").ToPoint();
        OffsetX = Math.Max(start.X, end.X);
        OffsetY = start.Y;

#if DEBUG
        Environment.SetEnvironmentVariable("HARMONY_LOG_FILE", null);
#endif
    }

    [OnLevelStart]
    public static void OnLevelStart()
    {
        string dir = Path.Combine(AssemblyPath, SCREENSHOT_DIR, Sanitize(GetDirName()));
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        Printer.SaveDir = dir;
    }

    [PauseMenuItemSetting]
    public static ToggleCurrentScreen ToggleCurrentScreen(object factory, GuiFormat format)
    {
        return new ToggleCurrentScreen();
    }

    [PauseMenuItemSetting]
    public static ToggleUpscale ToggleUpscale(object factory, GuiFormat format)
    {
        return new ToggleUpscale();
    }

    [PauseMenuItemSetting]
    public static ToggleSRayManWall ToggleSRayManWall(object factory, GuiFormat format)
    {
        return new ToggleSRayManWall();
    }


    [PauseMenuItemSetting]
    public static TextButton ButtonResetIndex(object factory, GuiFormat format)
    {
        return new TextButton("Reset Index", new ResetIndexNode());
    }

    [PauseMenuItemSetting]
    public static SliderStartIndex SliderStartIndex(object factory, GuiFormat format)
    {
        return new SliderStartIndex();
    }

    [PauseMenuItemSetting]
    public static SliderEndIndex SliderEndIndex(object factory, GuiFormat format)
    {
        return new SliderEndIndex();
    }

    [MainMenuItemSetting]
    [PauseMenuItemSetting]
    public static ExplorerTextButton OpenFolderExplorer(object factory, GuiFormat format)
    {
        return new ExplorerTextButton("Open Input Files Folder", new NodeOpenFolderExplorer(AssemblyPath), Color.Lime);
    }

    private static string GetDirName() {
        JKContentManager contentManager = Game1.instance.contentManager;
        if (contentManager == null)
        {
            return "Debug";
        }
        if (contentManager.level != null)
        {
            return contentManager.level.Name;
        }
        return "Nexile Maps";
    }
    private static string Sanitize(string name)
    {
        name = name.Trim();
        if (name == string.Empty)
        {
            name = "-";
        }
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '#');
        }
        foreach (char c in Path.GetInvalidPathChars())
        {
            name = name.Replace(c, '#');
        }
        name = Regex.Replace(name, "^\\.\\.$", "-", RegexOptions.IgnoreCase);
        name = Regex.Replace(name, "^(con|prn|aux|nul|com\\d|lpt\\d)$", $"-", RegexOptions.IgnoreCase);

        return name;
    }
    private static void SaveSettingsOnFile(object sender, System.ComponentModel.PropertyChangedEventArgs args)
    {
        try
        {
            XmlSerializerHelper.Serialize($@"{AssemblyPath}\{SETTINGS_FILE}", Preferences);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
        }
    }
}
