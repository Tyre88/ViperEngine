using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using ScreenManager.Screens;

namespace ViperEngine
{
    public class Controls
    {
        KeyboardState keyState;

        public event EventHandler MakeMouseVisible;

        public Controls()
        {
        }

        public void Update(ref LastKeyState lastkeystate, Keys up, Keys down, Keys right, Keys left, ref ScreenManager.Screens.ScreenManager manager)
        {
            keyState = Keyboard.GetState();

            if (manager.GameState == GameState.PLAY)
            {
                if (keyState.IsKeyDown(up) && lastkeystate != LastKeyState.DOWN)
                {
                    lastkeystate = LastKeyState.UP;
                }
                else if (keyState.IsKeyDown(down) && lastkeystate != LastKeyState.UP)
                {
                    lastkeystate = LastKeyState.DOWN;
                }
                else if (keyState.IsKeyDown(right) && lastkeystate != LastKeyState.LEFT)
                {
                    lastkeystate = LastKeyState.RIGHT;
                }
                else if (keyState.IsKeyDown(left) && lastkeystate != LastKeyState.RIGHT)
                {
                    lastkeystate = LastKeyState.LEFT;
                }

            }

            if (keyState.IsKeyDown(Keys.Escape))
            {
                if (manager.GameState != GameState.QUIT)
                {
                    foreach (Screen item in manager.Screens)
                    {
                        item.Active = false;
                    }

                    manager.Screens.Find(s => s.Name == "Pause menu").Active = true;

                    manager.GameState = GameState.PAUSE;

                    MakeMouseVisible(this, null);
                }
            }
            else if (keyState.IsKeyDown(Keys.Space))
            {
                if (manager.GameState != GameState.QUIT)
                {
                    manager.GameState = GameState.PLAY;
                }
            }

            //if (keyState.IsKeyDown(Keys.Z))
            //{
            //    Game1.volume = 0.0f;
            //}
            //else if (keyState.IsKeyDown(Keys.X))
            //{
            //    Game1.volume = 1.0f;
            //}
        }
    }
}
