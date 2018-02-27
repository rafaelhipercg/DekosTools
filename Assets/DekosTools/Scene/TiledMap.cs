/*
MIT License

Copyright (c) 2017 Rafael cosentino garcia

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//Tiled Map System V0.0.1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DekosTools.TiledMapSystem
{
    public class TiledMap<T>
    {
        MapObject<T>[,] map;

        /// <summary>
        /// Return true if is outside of the map
        /// </summary>
        /// <param name="X">Position X</param>
        /// <param name="Y">Position Y</param>
        /// <returns></returns>
        public bool IsOutside(int X, int Y)
        {
            if ((X >= 0) &&
            (Y >= 0) &&
            (X < Width) &&
            (Y < Height))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Width
        {
            get
            {
                return map.GetLength(0);
            }
        }
        public int Height
        {
            get
            {
                return map.GetLength(1);
            }
        }

        public TiledMap(int width, int height)
        {
            map = new MapObject<T>[width + 1, height + 1];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    map[x, y] = new MapObject<T>();
                }
            }
        }

        public MapObject<T>[,] GetMap
        {
            get
            {
                return map;
            }
        }

        /// <summary>
        /// Move object in the position [From] to [To] and set [From] to default. 
        /// If this is not possible or is blocked, this will return false.
        /// </summary>
        /// <param name="From">From position</param>
        /// <param name="To">To position</param>
        /// <returns></returns> 
        public bool MoveObject(int FromX, int FromY, int ToX, int ToY)
        {
            if (!IsOutside(FromX, FromY) && !IsOutside(ToX, ToY))
            {
                if (map[ToX, ToY] == null || !map[ToX, ToY].Blocked)
                {
                    map[ToX, ToY] = map[FromX, FromY];
                    map[FromX, FromY] = new MapObject<T>();
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Move object in the position [From] to [To] and set [From] to default. 
        /// If this is not possible, this will return false.
        /// </summary>
        /// <param name="From">From position</param>
        /// <param name="To">To position</param>
        /// <returns></returns>
        public bool Move(int FromX, int FromY, int ToX, int ToY)
        {
            if (!IsOutside(FromX, FromY) && !IsOutside(ToX, ToY))
            {
                map[ToX, ToY] = map[FromX, FromY];
                map[FromX, FromY] = new MapObject<T>();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Swap object in the position [From] to [To]. 
        /// If this is not possible, this will return false.
        /// </summary>
        /// <param name="From">From position</param>
        /// <param name="To">To position</param>
        /// <returns></returns>
        public bool Swap(int FromX, int FromY, int ToX, int ToY)
        {
            if (!IsOutside(FromX, FromY) && !IsOutside(ToX, ToY))
            {
                MapObject<T> temp = map[ToX, ToY];
                map[ToX, ToY] = map[FromX, FromY];
                map[FromX, FromY] = temp;
                return true;
            }
            return false;
        }
    }

    public class AStar<T>
    {
        List<AStarPos> Open;
        List<AStarPos> Close;
        TiledMap<T> Map;
        Vector2 from;
        Vector2 to;

        public AStar(TiledMap<T> _Map, Vector2 From, Vector2 To)
        {
            Open = new List<AStarPos>();
            Close = new List<AStarPos>();
            Map = _Map;
            from = From;
            to = To;
        }

        public Vector2[] Pathfind()
        {
            Vector2[] O;
            Vector2[] C;
            return Pathfind(out O, out C);
        }

        public Vector2[] Pathfind(out Vector2[] _Open, out Vector2[] _Close)
        {
            _Open = new Vector2[0];
            _Close = new Vector2[0];
            Open.Clear();
            Close.Clear();
            Open.Add(new AStarPos(Map.GetMap[Mathf.FloorToInt(from.x), Mathf.FloorToInt(from.y)], from, from, to, null));
            AStarPos Arrival = null;
            while (Open.Count > 0)
            {
                AStarPos Current = null;
                foreach (AStarPos ASP in Open.ToArray())
                {
                    if (Current == null)
                    {
                        Current = ASP;
                    }
                    else
                    {
                        if (ASP.F < Current.F)
                        {
                            Current = ASP;
                        }
                        else if (ASP.F == Current.F)
                        {
                            if (ASP.H < Current.H)
                            {
                                Current = ASP;
                            }
                        }

                    }
                }
                Open.Remove(Current);
                Close.Add(Current);
                List<AStarPos> successor = new List<AStarPos>();
                if (!Map.IsOutside(Current.X + 1, Current.Y))
                {
                    successor.Add(
                        new AStarPos(Map.GetMap[Mathf.FloorToInt(Current.X + 1), Mathf.FloorToInt(Current.Y)]
                        , new Vector2(Current.X + 1, Current.Y)
                        , new Vector2(Current.X, Current.Y)
                        , to
                        , Current
                        ));
                }
                if (!Map.IsOutside(Current.X - 1, Current.Y))
                {
                    successor.Add(
                    new AStarPos(Map.GetMap[Mathf.FloorToInt(Current.X - 1), Mathf.FloorToInt(Current.Y)]
                    , new Vector2(Current.X - 1, Current.Y)
                    , new Vector2(Current.X, Current.Y)
                    , to
                    , Current
                    ));
                }
                if (!Map.IsOutside(Current.X, Current.Y + 1))
                {
                    successor.Add(
                    new AStarPos(Map.GetMap[Mathf.FloorToInt(Current.X), Mathf.FloorToInt(Current.Y + 1)]
                    , new Vector2(Current.X, Current.Y + 1)
                    , new Vector2(Current.X, Current.Y)
                    , to
                    , Current
                    ));
                }
                if (!Map.IsOutside(Current.X, Current.Y - 1))
                {
                    successor.Add(
                    new AStarPos(Map.GetMap[Mathf.FloorToInt(Current.X), Mathf.FloorToInt(Current.Y - 1)]
                    , new Vector2(Current.X, Current.Y - 1)
                    , new Vector2(Current.X, Current.Y)
                    , to
                    , Current
                    ));
                }

                foreach (AStarPos ASP in successor.ToArray())
                {
                    bool Skip = false;
                    if (ASP.Object.Blocked)
                    {
                        Skip = true;
                    }
                    foreach (AStarPos OASP in Open.ToArray())
                    {
                        if (ASP.X == OASP.X && ASP.Y == OASP.Y)
                        {
                            Skip = true;
                            break;
                        }
                    }
                    foreach (AStarPos CASP in Close.ToArray())
                    {
                        if (ASP.X == CASP.X && ASP.Y == CASP.Y)
                        {
                            Skip = true;
                            break;
                        }
                    }
                    if (!Skip)
                    {
                        Open.Add(ASP);
                    }
                    if (ASP.X == Mathf.FloorToInt(to.x) && ASP.Y == Mathf.FloorToInt(to.y))
                    {
                        Arrival = ASP;
                        break;
                    }
                }
                if (Arrival != null || Open.Count > (Map.Width * Map.Height))
                {
                    break;
                }
            }

            List<Vector2> Ope = new List<Vector2>();
            List<Vector2> Clo = new List<Vector2>();
            foreach (AStarPos OASP in Open.ToArray())
            {
                Ope.Add(new Vector2(OASP.X, OASP.Y));
            }
            foreach (AStarPos CASP in Close.ToArray())
            {
                Clo.Add(new Vector2(CASP.X, CASP.Y));
            }
            _Open = Ope.ToArray();
            _Close = Clo.ToArray();

            AStarPos CurrentPath = Arrival;
            List<Vector2> ToReturn = new List<Vector2>();
            while (CurrentPath != null)
            {
                ToReturn.Insert(0, new Vector2(CurrentPath.X, CurrentPath.Y));
                CurrentPath = CurrentPath.Parents;
            }

            return ToReturn.ToArray();

        }

        class AStarPos
        {
            public AStarPos Parents;
            public int X;
            public int Y;
            public float G;
            public float H;
            public float F { get { return G + H; } }
            public MapObject<T> Object;
            public AStarPos(MapObject<T> _Object, Vector2 _Pos, Vector2 _From, Vector2 _To, AStarPos _Parent)
            {
                Object = _Object;
                Parents = _Parent;
                X = Mathf.FloorToInt(_Pos.x);
                Y = Mathf.FloorToInt(_Pos.y);
                G = Mathf.Abs(Mathf.FloorToInt(_Pos.x) - Mathf.FloorToInt(_From.x))
                  + Mathf.Abs(Mathf.FloorToInt(_Pos.y) - Mathf.FloorToInt(_From.y));
                H = Mathf.Abs(Mathf.FloorToInt(_Pos.x) - Mathf.FloorToInt(_To.x))
                  + Mathf.Abs(Mathf.FloorToInt(_Pos.y) - Mathf.FloorToInt(_To.y));
            }
        }
    }

    public class MapObject<T>
    {
        T tObject;
        public T TObject
        {
            get
            {
                return tObject;
            }

            set
            {
                tObject = value;
            }
        }

        bool blocked = false;
        public bool Blocked
        {
            get
            {
                return blocked;
            }

            set
            {
                blocked = value;
            }
        }

        public MapObject(T _TObject = default(T),
                         bool _Blocked = false,
                         float _Cost = 1)
        {
            tObject = _TObject;
            blocked = _Blocked;
        }
    }
}
