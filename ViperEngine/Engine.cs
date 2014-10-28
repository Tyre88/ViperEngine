using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViperEngine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ViperEngine
{
    public class Engine
    {
        public enum NumberOfPlayers
        {
            ONE,
            TWO
        }

        public readonly bool Debug = false;

        public SpriteFont GameObjectFont;

        public int MaxGoodObjects { get; set; }
        public int MaxEvilObjects { get; set; }

        public List<GameObject> GameObjects;
        public List<Player> Players;
        public List<Camera.Camera> Cameras;
        public List<CollisionDetection> Detections;

        public Texture2D objectsTextureSheet;
        public Rectangle objectApple, objectCarrot, objectChili, objectBanana;
        public Rectangle objectBrokenBottle;

        public List<Texture2D> TransitionTextures;

        public ScreenManager.Screens.ScreenManager manager;

        public NumberOfPlayers numberOfPlayers;

        public Level.Level currentMap;

        float appleTimer, timeToAddApple, carrotTimer, timeToAddCarrot, chiliTimer, timeToAddChili,
            bananaTimer, timeToAddBanana;
        float brokenBottleTimer, timeToAddBrokenBottle, toxicTubeTimer, timeToAddToxicTube;
        Texture2D toxicTubeTexture;

        public Texture2D ToxicTubeTexture
        {
            get { return toxicTubeTexture; }
            set { toxicTubeTexture = value; }
        }

        private static Engine instance;

        private Engine()
        {
            GameObjects = new List<GameObject>();
            Players = new List<Player>();
            Cameras = new List<Camera.Camera>();
            Detections = new List<CollisionDetection>();
            TransitionTextures = new List<Texture2D>();

            MaxEvilObjects = 5;
            MaxGoodObjects = 15;

            timeToAddApple = 2500;
            timeToAddCarrot = 13000;
            timeToAddChili = 90000;
            timeToAddBanana = 55000;
            timeToAddBrokenBottle = 6000;
            timeToAddToxicTube = 120000;
        }

        public static Engine Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new Engine();
                }
                return instance;
            }
        }

        public void AddGameObjects(Texture2D texture, int amount, bool evil, Level.Level map, Rectangle sheetItem)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = new GameObject(texture, evil, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight), sheetItem);
                GameObjects.Add(obj);
            }
        }

        #region Good
        public void AddApple(Texture2D texture, Level.Level map)
        {
            GameObject apple = new Apple(texture, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight));
            GameObjects.Add(apple);
        }

        private void AddCarrot(Texture2D texture, Level.Level map)
        {
            GameObject carrot = new Carrot(texture, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight));
            GameObjects.Add(carrot);
        }

        private void AddChili(Texture2D texture, Level.Level map)
        {
            GameObject chili = new Chili(texture, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight));
            chili.Eat += new GameObject.GOEventHandler(chili_Eat);
            GameObjects.Add(chili);
        }

        private void AddBanana(Texture2D texture, Level.Level map)
        {
            GameObject banana = new Banana(texture, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight));
            banana.Eat += new GameObject.GOEventHandler(banana_Eat);
            GameObjects.Add(banana);
        }
        #endregion

        #region Evil
        private void AddBrokenBottle(Texture2D texture, Level.Level map)
        {
            GameObject brokenBottle = new BrokenBottle(texture, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight));
            brokenBottle.Eat += new GameObject.GOEventHandler(brokenBottle_Eat);
            GameObjects.Add(brokenBottle);
        }

        private void AddToxictube(Texture2D texture, Level.Level map)
        {
            GameObject toxicTube = new Toxictube(ToxicTubeTexture, (map.MapWidth * map.TileWidth), (map.MapHeight * map.TileHeight));
            GameObjects.Add(toxicTube);
        }
        #endregion

        #region Events
        #region Good
        void banana_Eat(Player player)
        {
            if (player.Timer <= 0)
            {
                player.Timer = 3;
                player.Speed = 50;

                player.UpdateEvents += new Player.PUpdateEventHandler(player_UpdateBanana);
                player.DrawEvents += new Player.PDrawEventHandler(player_DrawBanana);
            }
        }

        void player_UpdateBanana(Player player, GameTime gameTime)
        {
            if (player.Timer <= 0)
            {
                player.Speed = 0;
                player.UpdateEvents -= player_UpdateBanana;
                player.DrawEvents -= player_DrawBanana;
            }
            else
            {
                player.Timer -= 0.1f;
            }
        }

        void player_DrawBanana(Player player, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameObjectFont, string.Format("{0}", (int)player.Timer), new Vector2(player.Position.X, player.Position.Y - 25), Color.Red);
        }

        void chili_Eat(Player player)
        {
            if (numberOfPlayers == NumberOfPlayers.TWO)
            {
                Player enemyPlayer = Players.Find(p => p != player);

                enemyPlayer.PlayerEffect.CurrentTechnique = enemyPlayer.PlayerEffect.Techniques["PlayerChili"];

                if (enemyPlayer.Percentage <= 0)
                {
                    enemyPlayer.Percentage = 1.5f;

                    enemyPlayer.Up = Microsoft.Xna.Framework.Input.Keys.Down;
                    enemyPlayer.Down = Microsoft.Xna.Framework.Input.Keys.Up;
                    enemyPlayer.Left = Microsoft.Xna.Framework.Input.Keys.Right;
                    enemyPlayer.Right = Microsoft.Xna.Framework.Input.Keys.Left;
                }

                enemyPlayer.PlayerEffect.Parameters["Percentage"].SetValue(enemyPlayer.Percentage);
            }
            else
            {
                player.PlayerEffect.CurrentTechnique = player.PlayerEffect.Techniques["PlayerChili"];

                if (player.Percentage <= 0)
                {
                    player.Percentage = 1.5f;
                }

                player.PlayerEffect.Parameters["Percentage"].SetValue(player.Percentage);
            }

            player.UpdateEvents += new Player.PUpdateEventHandler(player_EatChili);
            //player.Kill();
        }

        void player_EatChili(Player player, GameTime gameTime)
        {
            if (numberOfPlayers == NumberOfPlayers.TWO)
            {
                Player enemyPlayer = Players.Find(p => p != player);

                if (enemyPlayer.Percentage > 0)
                {
                    enemyPlayer.Percentage -= 0.01f;
                    enemyPlayer.PlayerEffect.Parameters["Percentage"].SetValue(enemyPlayer.Percentage);
                }
                else
                {
                    player.UpdateEvents -= new Player.PUpdateEventHandler(player_EatChili);
                    enemyPlayer.PlayerEffect.CurrentTechnique = enemyPlayer.PlayerEffect.Techniques["Player"];

                    enemyPlayer.Up = Microsoft.Xna.Framework.Input.Keys.Up;
                    enemyPlayer.Down = Microsoft.Xna.Framework.Input.Keys.Down;
                    enemyPlayer.Left = Microsoft.Xna.Framework.Input.Keys.Left;
                    enemyPlayer.Right = Microsoft.Xna.Framework.Input.Keys.Right;
                }
            }
            else
            {
                if (player.Percentage > 0)
                {
                    player.Percentage -= 0.01f;
                    player.PlayerEffect.Parameters["Percentage"].SetValue(player.Percentage);
                }
                else
                {
                    player.UpdateEvents -= new Player.PUpdateEventHandler(player_EatChili);
                    player.PlayerEffect.CurrentTechnique = player.PlayerEffect.Techniques["Player"];
                }
            }
        }
        #endregion

        #region Evil
        void brokenBottle_Eat(Player player)
        {
            player.BodyList.RemoveAt(player.BodyList.Count - 1);
            player.BodyList.RemoveAt(player.BodyList.Count - 1);
        }
        #endregion
        #endregion

        public void ClearGameObjects()
        {
            GameObjects.Clear();
        }

        public void Update(GameTime gameTime)
        {
            appleTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
            carrotTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
            chiliTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
            bananaTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
            brokenBottleTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
            toxicTubeTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;

            #region Good
            if (appleTimer >= timeToAddApple)
            {
                if (GameObjects.FindAll(g => !g.Evil).Count < MaxGoodObjects)
                {
                    AddApple(objectsTextureSheet, currentMap);
                    appleTimer = 0;
                }
            }

            if (carrotTimer >= timeToAddCarrot)
            {
                if (GameObjects.FindAll(g => !g.Evil).Count < MaxGoodObjects)
                {
                    AddCarrot(objectsTextureSheet, currentMap);
                    carrotTimer = 0;
                }
            }

            if (chiliTimer >= timeToAddChili)
            {
                if (GameObjects.FindAll(g => !g.Evil).Count < MaxGoodObjects)
                {
                    AddChili(objectsTextureSheet, currentMap);
                    chiliTimer = 0;
                }
            }

            if (bananaTimer >= timeToAddBanana)
            {
                if (GameObjects.FindAll(g => !g.Evil).Count < MaxGoodObjects)
                {
                    AddBanana(objectsTextureSheet, currentMap);
                    bananaTimer = 0;
                }
            }
            #endregion

            #region Evil
            if (brokenBottleTimer >= timeToAddBrokenBottle)
            {
                if (GameObjects.FindAll(g => g.Evil).Count < MaxEvilObjects)
                {
                    AddBrokenBottle(objectsTextureSheet, currentMap);
                    brokenBottleTimer = 0;
                }
            }

            if (toxicTubeTimer >= timeToAddToxicTube)
            {
                if (GameObjects.FindAll(g => g.Evil).Count < MaxEvilObjects)
                {
                    AddToxictube(ToxicTubeTexture, currentMap);
                    toxicTubeTimer = 0;
                }
            }
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
