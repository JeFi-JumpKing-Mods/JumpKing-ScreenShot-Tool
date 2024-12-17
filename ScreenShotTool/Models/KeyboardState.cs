using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ScreenShot.Models;
public static class KeyboardState
{
    private static List<Keys> LastState = new List<Keys>();
    private static List<Keys> CurrentState = new List<Keys>();
    static KeyboardState() {
    }

    public static void Update() {
        LastState = CurrentState;
        CurrentState = new List<Keys>(Keyboard.GetState().GetPressedKeys());
    }
    public static bool isPressed(Keys key) {
        return CurrentState.Contains(key) && !LastState.Contains(key);
    }
}
