using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Augenblick
{
    public delegate void MouseEventHandler(Vector2 pos);

    public static class MouseHandler
    {
        public static Vector2 MousePosition
        {
            get { return Mouse.GetState().Position.ToVector2(); }
        }

        public static event MouseEventHandler LeftMouseClicked;
        public static event MouseEventHandler RightMouseClicked;

        public static void Update(GameTime time)
        {
            MouseState state = Mouse.GetState();
            Vector2 pos = state.Position.ToVector2();

            if (state.LeftButton == ButtonState.Pressed)
                if (LeftMouseClicked != null)
                    LeftMouseClicked(pos);

            if (state.RightButton == ButtonState.Pressed)
                if (RightMouseClicked != null)
                    RightMouseClicked(pos);
        }
    }
}
