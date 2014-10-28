using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViperEngine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ViperEngine
{
    public class CollisionDetection
    {
        Rectangle playerRect, bodyRect, _enemyBodyRect;
        Level.Level map;
        int playerIndex, enemyPlayerIndex;

        private const int gameObjToCenter = 8;

        public CollisionDetection(int playerIndex, Level.Level map)
        {
            this.playerIndex = playerIndex;
            this.map = map;
            enemyPlayerIndex = -1;
        }

        public CollisionDetection(int playerIndex, int enemyPlayerIndex, Level.Level map)
        {
            this.playerIndex = playerIndex;
            this.enemyPlayerIndex = enemyPlayerIndex;
            this.map = map;
        }

        public void Update(ref ScreenManager.Screens.ScreenManager manager)
        {
            playerRect = new Rectangle((int)Engine.Singleton.Players[playerIndex].Position.X + gameObjToCenter, (int)Engine.Singleton.Players[playerIndex].Position.Y + gameObjToCenter,
                Engine.Singleton.Players[playerIndex].head.Width,
                Engine.Singleton.Players[playerIndex].head.Height);

            if (map.collisionRects.Any(c => c.Intersects(playerRect)))
            {
                Engine.Singleton.Players[playerIndex].Alive = false;
                Engine.Singleton.Players[playerIndex].Kill();
                manager.GameState = ScreenManager.Screens.GameState.FREEZE;
            }

            GameObject gObj = null;

            gObj = Engine.Singleton.GameObjects.Find(g => map.collisionRects.Any(c =>
                new Rectangle((int)g.Position.X + gameObjToCenter, (int)g.Position.Y + gameObjToCenter, g.SheetItem.Width, g.SheetItem.Height).Intersects(
                new Rectangle((int)c.X, (int)c.Y, c.Width, c.Height))));

            if (gObj != null)
            {
                gObj.Reinitialize();
            }

            gObj = Engine.Singleton.GameObjects.Find(g =>
                playerRect.Intersects(new Rectangle((int)g.Position.X + gameObjToCenter, (int)g.Position.Y + gameObjToCenter, g.SheetItem.Width, g.SheetItem.Height)) && !g.Evil);

            if (gObj != null)
            {
                gObj.EatObject(Engine.Singleton.Players[playerIndex]);
                Engine.Singleton.GameObjects.Remove(gObj);
            }

            for (int i = 1; i < Engine.Singleton.Players[playerIndex].BodyList.Count; i++)
            {
                if (playerRect.Intersects(new Rectangle((int)Engine.Singleton.Players[playerIndex].BodyPositions[i].X + Engine.Singleton.Players[playerIndex].BodyList[i].Width / 2,
                    (int)Engine.Singleton.Players[playerIndex].BodyPositions[i].Y + Engine.Singleton.Players[playerIndex].BodyList[i].Height / 2,
                    Engine.Singleton.Players[playerIndex].BodyList[i].Width, Engine.Singleton.Players[playerIndex].BodyList[i].Height)))
                {
                    Engine.Singleton.Players[playerIndex].Kill();
                }
            }

            if (enemyPlayerIndex >= 0)
            {

                foreach (Vector2 item in Engine.Singleton.Players[enemyPlayerIndex].BodyPositions)
                {
                    _enemyBodyRect = new Rectangle((int)item.X, (int)item.Y, Engine.Singleton.Players[enemyPlayerIndex].body.Width, 
                        Engine.Singleton.Players[enemyPlayerIndex].body.Height);

                    if (playerRect.Intersects(_enemyBodyRect))
                    {
                        Engine.Singleton.Players[playerIndex].Kill();
                    }
                }

            }
        }
    }
}
