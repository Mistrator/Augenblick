using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Augenblick
{
    public delegate void KeyboardEventHandler(Keys k);

    public static class KeyboardHandler
    {
        public static List<Keys> ListenedKeys = new List<Keys>();
        private static List<Keys> KeysDown = new List<Keys>();

        public static event KeyboardEventHandler KeyPressed;

        public static void Update(GameTime time)
        {
            KeyboardState state = Keyboard.GetState();

            for (int i = 0; i < ListenedKeys.Count; i++)
            {
                if (state.IsKeyDown(ListenedKeys[i]))
                    if (KeyPressed != null)
                        if (!KeysDown.Contains(ListenedKeys[i]))
                        {
                            KeyPressed(ListenedKeys[i]);
                            KeysDown.Add(ListenedKeys[i]);
                        }
            }

            List<Keys> rem = new List<Keys>();

            foreach (Keys k in KeysDown)
            {
                if (!state.IsKeyDown(k))
                    rem.Add(k);
            }

            foreach (Keys k in rem) // foreachin aikana ei voi muokata collectionia
            {
                KeysDown.Remove(k);
            }
        }
    }
}