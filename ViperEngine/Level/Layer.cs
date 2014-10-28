using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ViperEngine.Level
{
    public class Layer
    {
        //Layer array
        public int[,] layer;

        //Maps and Tiles
        private int mapWidth, mapHeight, tileWidth, tileHeight;

        public Layer(int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.tileHeight = tileHeight;
            this.tileWidth = tileWidth;

            layer = new int[mapWidth, mapHeight];
        }

        public void LoadLayer(StreamReader objReader)
        {
            try
            {
                //Populate the layer array

                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        layer[x, y] = Convert.ToInt32(objReader.ReadLine());
                    }
                }
            }
            catch
            {
                Console.Write("There was an error loading the map");
            }
        }
    }
}
