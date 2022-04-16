using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using MathIntLib;
using Enumerations;

namespace AdditionalClassesInt
{

    public class RectangleInt 
    {
        private Vector2Int TL, TR, BL, BR, L, R, T, B;

        public RectangleInt(int x, int y, int width, int height)
        {
            this.Initialize(x, y, width, height);    
        }

        public RectangleInt(Vector2Int centerPosition, int width, int height)
        {
            int x = MathInt.Round(centerPosition.x - 1.0f * width / 2);
            int y = MathInt.Round(centerPosition.y - 1.0f * height / 2);
            this.Initialize(x, y, width, height);
        }

        private void Initialize(int x, int y, int width, int height)
        {
            this.BL = new Vector2Int(x, y);
            this.TL = new Vector2Int(x, y + height);
            this.BR = new Vector2Int(x + width, y);
            this.TR = new Vector2Int(x + width, y + height);
            
            this.B = new Vector2Int(x + MathInt.Round(width/2), y);
            this.L = new Vector2Int(x, y + MathInt.Round(height/2));
            this.T = new Vector2Int(this.B.x, y + height);
            this.R = new Vector2Int(x + width, this.L.y);    
        }

        public List<Vector2Int> GetVerticlesCoordinates()
        {
            List<Vector2Int> verticles = new List<Vector2Int>();
            verticles.Add(this.BL);
            verticles.Add(this.TL);
            verticles.Add(this.TR);
            verticles.Add(this.BR);
            return verticles;
        }

        public List<Vector2Int> GetDiamondCoordinates()
        {
            List<Vector2Int> verticles = new List<Vector2Int>();
            verticles.Add(this.B);
            verticles.Add(this.L);
            verticles.Add(this.R);
            verticles.Add(this.T);
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
        
        public SquareInt(Vector2Int centerPosition, int size) : base(centerPosition, size, size) {}
    }

}

namespace AdditionalDataStructures
{
    
    [System.Serializable]
    public class HeightLimits {
        public int Minimal = -3;
        public int SeaLevel = 0;
        public int Maximal = 3;
    }

    [System.Serializable]
    public class BorderTypes
    {
        public BorderType SouthWest = BorderType.Random;
        public BorderType NorthWest = BorderType.Random;
        public BorderType NorthEast = BorderType.Random;
        public BorderType SouthEast = BorderType.Random;
    }

    [System.Serializable]
    public class GeneralParameters {
        public HeightLimits HeightLimits;
        public Vector2Int MapLimits = new Vector2Int(128, 128);
        public BorderTypes BorderTypes;
        public float Coefficient = 1;
        public float PostProcessCoefficient = 1;
        public int Offset = 1;
        public bool EnablePostProcess = true;
    }
}