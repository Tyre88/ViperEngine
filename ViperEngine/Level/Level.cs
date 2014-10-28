using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ViperEngine.Level
{
    public class Level
    {
        public int MapHeight { get; set; }
        public int MapWidth { get; set; }
        public int TileHeight { get; set; }
        public int TileWidth { get; set; }

        public Texture2D Tilesheet { get; set; }

        //Layers
        public Layer TileLayer1 { get; set; }
        public Layer TileLayer2 { get; set; }
        public Layer SolidLayer { get; set; }

        //Tile rectangle that holds the tile bounds
        public List<Rectangle> tileSet = new List<Rectangle>();

        //Rectangle list for collision
        public List<Rectangle> collisionRects = new List<Rectangle>();

        //variable that holds the temp tile bounds
        Rectangle bounds;

        //Map scroll values
        Vector2 drawOffSet = Vector2.Zero;

        Vector2 origin;

        public void LoadMap(string loadFileName, Texture2D tilesheet)
        {
            try
            {
                loadFileName = string.Format("Content/{0}", loadFileName);
                //Map reader
                StreamReader objReader = new StreamReader(@loadFileName);

                //Find the map height and width from file
                MapHeight = Convert.ToInt32(objReader.ReadLine());
                MapWidth = Convert.ToInt32(objReader.ReadLine());
                TileHeight = Convert.ToInt32(objReader.ReadLine());
                TileWidth = Convert.ToInt32(objReader.ReadLine());

                //Reinitialize the maplayers
                TileLayer1 = new Layer(MapWidth, MapHeight, TileWidth, TileHeight);
                TileLayer2 = new Layer(MapWidth, MapHeight, TileWidth, TileHeight);
                SolidLayer = new Layer(MapWidth, MapHeight, TileWidth, TileHeight);

                //Load the layers
                TileLayer1.LoadLayer(objReader);
                TileLayer2.LoadLayer(objReader);
                SolidLayer.LoadLayer(objReader);

                //Close and dispose the reader
                objReader.Close();
                objReader.Dispose();

                Tilesheet = tilesheet;

                origin = new Vector2(TileWidth / 2, TileHeight / 2);
            }
            catch
            {
                Console.Write("There was an error loading the map, is the file name correct?");
            }
        }

        public void LoadTileSet(Texture2D tileset)
        {
            //Get the tilesheet dimentions
            int noOfTilesX = (int)tileset.Width / TileWidth;
            int noOfTilesY = (int)tileset.Height / TileHeight;

            //Initialize the tileset list
            tileSet = new List<Rectangle>(noOfTilesX * noOfTilesY);

            //Take out the separate tiles
            for (int y = 0; y < noOfTilesY; y++)
            {
                for (int x = 0; x < noOfTilesX; x++)
                {
                    bounds = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                    tileSet.Add(bounds);
                }

            }
        }

        public void PopulateCollisionLayer()
        {
            //Redeclare the rect list for collision
            collisionRects = new List<Rectangle>();

            //Loop through the array
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    //There is an collision
                    if (SolidLayer.layer[x, y] == 1)
                    {
                        collisionRects.Add(new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight));
                    }
                }
            }


        }

        public void DrawMap(SpriteBatch spriteBatch, Vector2 camera)
        {
            //get the camera offset
            drawOffSet = camera;

            try
            {
                //For each tile position
                for (int x = 0; x < MapHeight; x++)
                {
                    for (int y = 0; y < MapWidth; y++)
                    {
                        //Tile layer 1
                        //If there is a visible tile in that position
                        if (TileLayer1.layer[y, x] != 0)
                        {
                            //get the tilesheet bounds so the correct tile is drawn
                            bounds = tileSet[TileLayer1.layer[y, x] - 1];

                            //Draw it in screen space
                            spriteBatch.Draw(Tilesheet, new Vector2(((y * TileWidth) + drawOffSet.X),
                                ((x * TileHeight) + drawOffSet.Y)), bounds, Color.White, 0, origin, 1, SpriteEffects.None, 0);
                        }
                    }
                }


                //For each tile position Tile layer 2
                for (int x = 0; x < MapHeight; x++)
                {
                    for (int y = 0; y < MapWidth; y++)
                    {
                        //Tile layer 2
                        //If there is a visible tile in that position
                        if (TileLayer2.layer[y, x] != 0)
                        {
                            //get the tilesheet bounds so the correct tile is drawn
                            bounds = tileSet[TileLayer2.layer[y, x] - 1];

                            //Draw it in screen space
                            spriteBatch.Draw(Tilesheet, new Vector2(((y - drawOffSet.X) * TileWidth),
                                ((x - drawOffSet.Y) * TileHeight)), bounds, Color.White, 0, origin, 1, SpriteEffects.None, 0);
                        }
                    }
                }

                if (Engine.Singleton.Debug)
                {
                    foreach (Rectangle colRect in collisionRects)
                    {
                        spriteBatch.Draw(Tilesheet, new Vector2(colRect.X, colRect.Y), new Rectangle(0, 0, 32, 32), Color.Red, 0, origin, 1, SpriteEffects.None, 0);
                    }
                }
            }
            catch
            {
                Console.Write("There was a problem drawing the map.");
            }
        }
    }
}
