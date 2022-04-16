using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathIntLib;

namespace AdditionalClassesInt
{

    public class RectangleInt 
    {
        private Vector2Int TL, TR, BL, BR;

        public RectangleInt(int x, int y, int width, int height)
        {
            this.BL = new Vector2Int(x, y);
            this.TL = new Vector2Int(x, y + height);
            this.BR = new Vector2Int(x + width, y);
            this.TR = new Vector2Int(x + width, y + height);
        }

        public List<Vector2Int> GetVerticlesCoordinates()
        {
            List<Vector2Int> verticles = new List<Vector2Int>();
            verticles.Add(BL);
            verticles.Add(TL);
            verticles.Add(BR);
            verticles.Add(TR);
            return verticles;
        }

        public Vector2Int GetCenterCoordinates()
        {
            int xCoordinate = MathInt.Average(this.BL.x+this.TR.x, 2);
            int yCoordinate = MathInt.Average(this.BL.y+this.TR.y, 2);
            return new Vector2Int(xCoordinate, yCoordinate);
        }

    }

    public class SquareInt : RectangleInt
    {
        public SquareInt(int x, int y, int size) : base(x, y, size, size) {}
    }

}
