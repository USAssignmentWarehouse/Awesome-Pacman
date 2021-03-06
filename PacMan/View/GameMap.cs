﻿using PacMan.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    class GameMap
    {
        public struct Pos
        {
            public int X;
            public int Y;

            public Pos(int nx, int ny)
            {
                X = nx;
                Y = ny;
            }

            public static Pos operator +(Pos p1, Pos p2)
            {
                return new Pos(p1.X + p2.X, p1.Y + p2.Y);
            }

            public static Pos operator -(Pos p1, Pos p2)
            {
                return new Pos(p1.X - p2.X, p1.Y - p2.Y);
            }

            public static bool operator ==(Pos p1, Pos p2)
            {
                return ((p1.X == p2.X) && (p1.Y == p2.Y));
            }

            public static bool operator !=(Pos p1, Pos p2)
            {
                return ((p1.X != p2.X) || (p1.Y != p2.Y));
            }
        }

        public const int BlockSize = 15;
        public const float HalfBlockSize = BlockSize / 2 + 0.001f;
        public const float TopLeftX = 0;
        public const float TopLeftY = 50;

        /// <summary>
        /// Auxiliary 2d unitary values.
        /// </summary>
        public static Pos UnitX = new GameMap.Pos(1, 0);
        public static Pos UnitY = new GameMap.Pos(0, 1);

        public int height { get; set; }
        public int width { get; set; }

        public enum SpaceType
        {
            Empty,
            Wall
        };

        public SpaceType[,] spaces;

        public bool setSpaceType(SpaceType st, int X, int Y)
        {
            spaces[Y, X] = st;
            return true;
        }

        public SpaceType getSpaceType(Pos pos)
        {
            if ((pos.X > (width - 1)) || (pos.X < 0) || (pos.Y > (height - 1)) || (pos.Y < 0))
                return SpaceType.Empty;

            return spaces[pos.Y, pos.X];
        }

        private List<string> mapDataWithBound;
        
        public GameMap(List<string> mapDataWithBound)
        {
            if (mapDataWithBound != null && mapDataWithBound.Count() > 0)
            {
                this.mapDataWithBound = mapDataWithBound;
                this.height = mapDataWithBound.Count();
                this.width = mapDataWithBound[0].Length;

                spaces = new SpaceType[height, width];

                for(int y = 0; y < height; y++)
                    for(int x = 0; x < width; x++)
                    {
                        if (mapDataWithBound[y][x] == Constant.WallChar || mapDataWithBound[y][x] == Constant.BoundChar)
                            spaces[y, x] = SpaceType.Wall;
                        else 
                            spaces[y, x] = SpaceType.Empty;
                    }
            }
            else
                System.Windows.Forms.MessageBox.Show("Map Data is empty!", "Pacman", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
        
        public void DrawMap(Graphics g)
        {
            Pen mapPen = new Pen(Color.SpringGreen, 4f);
            Brush mapBrush = Brushes.Black;

            List<Bitmap> yellowDotBmp = new List<Bitmap>();
            yellowDotBmp.Add(new Bitmap(Properties.Resources.Yellow_dot, Sprite.Size/3, Sprite.Size/3));
            Item itemYellowDot = new Item(new Sprite(yellowDotBmp));

            List<Bitmap> fruitBmp = new List<Bitmap>();
            fruitBmp.Add(new Bitmap(Properties.Resources.Fruit__1_, Sprite.Size, Sprite.Size));
            Item itemFruit = new Item(new Sprite(fruitBmp));

            for (int i = 1; i < mapDataWithBound.Count - 1; i++)
            {
                for (int j = 1; j < mapDataWithBound[i].Length - 1; j++)
                {
                    if (mapDataWithBound[i][j] == Constant.WallChar)
                    {
                        float posX = j * BlockSize + HalfBlockSize + TopLeftX;
                        float posY = i * BlockSize + HalfBlockSize + TopLeftY;

                        if (mapDataWithBound[i][j - 1] == Constant.WallChar)//haft line to the left
                        {
                            g.DrawLine(mapPen, posX, posY, posX - HalfBlockSize, posY);
                        }
                        if (mapDataWithBound[i][j + 1] == Constant.WallChar)//haft line to the right
                        {
                            g.DrawLine(mapPen, posX, posY, posX + HalfBlockSize, posY);
                        }
                        if (mapDataWithBound[i - 1][j] == Constant.WallChar)//haft line upward
                        {
                            g.DrawLine(mapPen, posX, posY, posX, posY - HalfBlockSize);
                        }
                        if (mapDataWithBound[i + 1][j] == Constant.WallChar)//haft line downward
                        {
                            g.DrawLine(mapPen, posX, posY, posX, posY + HalfBlockSize);
                        }
                    }
                    if (mapDataWithBound[i][j] == Constant.DotChar)
                    {
                        itemYellowDot.MapPosition = new Point(i, j);
                        PointF pos = ToGraphicPosition(itemYellowDot.MapPosition.Y, itemYellowDot.MapPosition.X);
                        pos.X -= Sprite.Size / 6;
                        pos.Y -= Sprite.Size / 6;
                        itemYellowDot.ItemSprite.draw(g, pos);
                    }
                    if (mapDataWithBound[i][j] == Constant.FruitChar)
                    {
                        itemFruit.MapPosition = new Point(i, j);
                        PointF pos = ToGraphicPosition(itemFruit.MapPosition.Y, itemFruit.MapPosition.X);
                        pos.X -= Sprite.Size / 2;
                        pos.Y -= Sprite.Size / 2;
                        itemFruit.ItemSprite.draw(g, pos);
                    }
                }
            }
        }

        /// <summary>
        /// convert position in map(has bounds) to position in graphic
        /// </summary>
        /// <returns></returns>
        public static PointF ToGraphicPosition(int mapX, int mapY)
        {
            PointF posGraph = new PointF();
            posGraph.X = mapX * BlockSize + HalfBlockSize + TopLeftX;
            posGraph.Y = mapY * BlockSize + HalfBlockSize + TopLeftY;

            return posGraph;
        }

        public static GameMap.Pos ToMapPosition(float posX, float posY)
        {
            //GameMap.Pos closestPosMap = MapPosition;
            //double minDistance = Manager.GetDistance(MapPosition, new PointF(posX, posY));
            int x = (int)Math.Round((posX - HalfBlockSize - TopLeftX) / BlockSize);
            int y = (int)Math.Round((posY - HalfBlockSize - TopLeftY) / BlockSize);

            //TODO: Fix here
            return new GameMap.Pos(x, y);
        }
     

    }
}
