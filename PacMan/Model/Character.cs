﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    abstract class Character
    {
        protected Point MapPosition = new Point(5, 2); // current position in map(has bounds)
        protected PointF GraphicPosition; // current position in graphic

        protected List<CharacterSprite> SpriteAct;

        protected enum CharacterState { Alive, Died }
        protected enum Direction { Left, Right, Up, Down }

        protected CharacterState State;
        protected Direction CurrDirection = Direction.Right;
        protected float Speed = 6f;

        /// <summary>
        /// Constructor
        /// </summary>
        public Character()
        {
            AddSprite();
            GraphicPosition = GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y);
        }

        public Character(Point posMap)
        {
            MapPosition = posMap;
            GraphicPosition = GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y);
        }

        
        protected int ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    {
                        if (CurrDirection != Direction.Left)
                        {
                            if (GameManager.MapDataWithBound[MapPosition.X][MapPosition.Y - 1] != Constant.WallChar)
                            {
                                AdjustPos();
                                CurrDirection = Direction.Left;
                            }
                        }

                        break;
                    }
                case Direction.Right:
                    {
                        if (CurrDirection != Direction.Right)
                        {
                            if (GameManager.MapDataWithBound[MapPosition.X][MapPosition.Y + 1] != Constant.WallChar)
                            {
                                AdjustPos();
                                CurrDirection = Direction.Right;
                            }
                        }
                        break;
                    }
                case Direction.Up:
                    {
                        if (CurrDirection != Direction.Up)
                        {
                            if (GameManager.MapDataWithBound[MapPosition.X - 1][MapPosition.Y] != Constant.WallChar)
                            {
                                AdjustPos();
                                CurrDirection = Direction.Up;
                            }
                        }
                        break;
                    }
                case Direction.Down:
                    {
                        if (CurrDirection != Direction.Down)
                        {
                            if (GameManager.MapDataWithBound[MapPosition.X + 1][MapPosition.Y] != Constant.WallChar)
                            {
                                AdjustPos();
                                CurrDirection = Direction.Down;
                            }
                        }
                        break;
                    }
            }
            return 0;
        }


        virtual protected void Move()
        {
            switch(CurrDirection)
            {
                case Direction.Left:
                    {
                        if (GameManager.MapDataWithBound[MapPosition.X][MapPosition.Y - 1] == Constant.RoadChar
                            || GameManager.GetDistance(GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y - 1)) >= GameMap.BlockSize + Speed)
                        {
                            //MapPosition.Y--;
                            GraphicPosition.X -= Speed;
                        }
                        break;
                    }
                case Direction.Right:
                    {
                        if (GameManager.MapDataWithBound[MapPosition.X][MapPosition.Y + 1] == Constant.RoadChar
                            || GameManager.GetDistance(GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y + 1)) >= GameMap.BlockSize + Speed)
                        {
                            //MapPosition.Y++;
                            GraphicPosition.X += Speed;
                        }
                        break;
                    }
                case Direction.Up:
                    {
                        if (GameManager.MapDataWithBound[MapPosition.X - 1][MapPosition.Y] == Constant.RoadChar
                            || GameManager.GetDistance(GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X - 1, MapPosition.Y)) >= GameMap.BlockSize + Speed)
                        {
                            //MapPosition.X--;
                            GraphicPosition.Y -= Speed;

                        }
                        break;
                    }
                case Direction.Down:
                    {
                        if (GameManager.MapDataWithBound[MapPosition.X + 1][MapPosition.Y] == Constant.RoadChar
                            || GameManager.GetDistance(GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X + 1, MapPosition.Y)) >= GameMap.BlockSize + Speed)
                        {
                            //MapPosition.X++;
                            GraphicPosition.Y += Speed;

                        }
                        break;
                    }
            }

            MapPosition = GameMap.ToMapPosition(GraphicPosition.X, GraphicPosition.Y);
        }

        /// <summary>
        /// Update current position and redraw
        /// </summary>
        virtual public void UpdatePos()
        {
            //PointF pos = GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y);
            //g.DrawLine(Pens.Red, GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X + 1, MapPosition.Y));
            //g.DrawLine(Pens.Red, GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X - 1, MapPosition.Y));
            //g.DrawLine(Pens.Red, GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y + 1));
            //g.DrawLine(Pens.Red, GraphicPosition, GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y - 1));

           
            Move();
            //Manager.DrawSolidSquare(g, Brushes.Yellow, CONST.MapBlockSize, GraphicPosition.X, GraphicPosition.Y);
        }

        private void AdjustPos()
        {
            GraphicPosition = GameMap.ToGraphicPosition(MapPosition.X, MapPosition.Y);
        }

        abstract protected int AddSprite();
        abstract public int Behave();
        virtual public int Animate(Graphics g)
        {
            PointF drawPoint = new PointF(GraphicPosition.X - CharacterSprite.Size / 2, GraphicPosition.Y - CharacterSprite.Size / 2);

            switch (CurrDirection)
            {
                case Direction.Left:
                    {
                        SpriteAct[0].draw(g, drawPoint);
                        break;
                    }
                case Direction.Right:
                    {
                        SpriteAct[1].draw(g, drawPoint);
                        break;
                    }
                case Direction.Up:
                    {
                        SpriteAct[2].draw(g, drawPoint);
                        break;
                    }
                case Direction.Down:
                    {
                        SpriteAct[3].draw(g, drawPoint);
                        break;
                    }
            }
            return 0;
        }
        
    }
    
}