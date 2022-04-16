using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathIntLib;
using AdditionalClassesInt;
using Enumerations;
using AdditionalDataStructures;
using UnityEngine.TestTools;

class IntHeightMapHandler
{
    private int[,] map;

    private Vector2Int limits;

    public void Clear()
    {
        map = null;
    }

    public ref int[,] Map()
    {
        return ref this.map;
    }

    public List<int> GetVerticlesValues(List<Vector2Int> verticlesCoordinates)
    {
        var result = new List<int>();
        foreach (var verticleCoordinates in verticlesCoordinates)
        {
            result.Add(GetVerticleValue(verticleCoordinates));
        }

        return result;
    }

    public int GetVerticleValue(Vector2Int coordinates)
    {
        var xCoordinate = MathInt.Clamp(coordinates.x + 1, 0, this.limits.x + 1);
        var yCoordinate = MathInt.Clamp(coordinates.y + 1, 0, this.limits.y + 1);
        return this.map[xCoordinate, yCoordinate];
    }

    public void SetVerticleValue(Vector2Int coordinates, int value)
    {
        this.map[coordinates.x + 1, coordinates.y + 1] = value;
    }

    public ref int[,] Initialize(Vector2Int size, BorderTypes borderTypes, HeightLimits heightLimits)
    {
        this.map = new int[size.x + 2, size.y + 2];
        this.limits = new Vector2Int(size.x, size.y);
        Nullize(borderTypes, heightLimits);
        return ref this.map;
    }

    private void Nullize(BorderTypes borderTypes, HeightLimits heightLimits)
    {
        for (var x = 0; x < this.limits.x; x++)
        {
            for (var y = 0; y < this.limits.y; y++)
            {
                SetVerticleValue(new Vector2Int(x, y), 0);
            }
        }

        InitializeBorders(borderTypes, heightLimits);
    }

    public void InitializeBorders(BorderTypes borderTypes, HeightLimits heightLimits)
    {
        for (var x = 0; x < this.limits.x + 2; x++)
        {
            for (var y = 0; y < this.limits.y + 2; y++)
            {
                if (x == 0 || y == 0 || x == this.limits.x + 1 || y == this.limits.y + 1)
                    this.map[x, y] = this.GetBorderHeight(x, y, borderTypes, heightLimits);
            }
        }
    }

    private int GetBorderHeight(int x, int y, BorderTypes borderTypes, HeightLimits heightLimits)
    {
        var result = 0;
        var borderType = BorderType.NotBorder;
        if (x == 0)
            borderType = borderTypes.SouthWest;
        else if (y == this.limits.y + 1)
            borderType = borderTypes.NorthWest;
        else if (x == this.limits.x + 1)
            borderType = borderTypes.NorthEast;
        else if (y == 0)
            borderType = borderTypes.SouthEast;

        if (borderType == BorderType.NotBorder)
            result = 0;
        else if (borderType == BorderType.Land)
            result = heightLimits.Maximal;
        else if (borderType == BorderType.Sea)
            result = heightLimits.Minimal;
        
        return result;
    }
    
    public ref int[,] RandomizeCorners(BorderTypes borderTypes, HeightLimits heightLimits, float coefficient) 
    {
        for (var x = 0; x < limits.x + 1; x = x + (limits.x - 1))
        {
            for (var y = 0; y < limits.y + 1; y = y + (limits.y - 1))
            {
                var coordinates = new Vector2Int(x, y);
                SquareInt rectangle = new SquareInt(coordinates, 1);
                var verticleValues = this.GetVerticlesValues(rectangle.GetVerticlesCoordinates());

                var average = MathInt.Average(verticleValues);
                var displacement = Displace(average, heightLimits, coefficient);

                SetVerticleValue(coordinates, average + displacement);

                //var roundedHeight = RandomInt.Range(heightLimits.Minimal, heightLimits.Maximal);
                //this.map[x, y] = roundedHeight;
            }
        }

        return ref this.map;
    }
    
    
    public ref int[,] Square(RectangleInt rectangle, HeightLimits heightLimits, float coefficient)
    {
        var verticleValues = this.GetVerticlesValues(rectangle.GetVerticlesCoordinates());

        var average = MathInt.Average(verticleValues);
        var displacement = Displace(average, heightLimits, coefficient);

        var centerCoordinates = rectangle.GetCenterCoordinates();
        SetVerticleValue(centerCoordinates, average + displacement);

        return ref this.map;    
    }

    private static int Displace(int height, HeightLimits heightLimits, float coefficient)
    {
        var lLimit = -1 * coefficient;//heightLimits.Minimal - height;
        var rLimit = 1 * coefficient;//heightLimits.Maximal - height;

        return RandomInt.Range(lLimit, rLimit);
    }
    
    public ref int[,] Diamond(RectangleInt rectangle, HeightLimits heightLimits, float coefficient)
    {
        var verticleValues = this.GetVerticlesValues(rectangle.GetDiamondCoordinates());

        var average = MathInt.Average(verticleValues);
        var displacement = Displace(average, heightLimits, coefficient);

        var centerCoordinates = rectangle.GetCenterCoordinates();
        SetVerticleValue(centerCoordinates, average + displacement);
        return ref this.map;
    }

}

class floatHeightMapHandler
{
    private float[,] map;
    private bool[,] calculated;
    public int offset = 1;
    private Vector2Int limits;

    public void Clear()
    {
        map = null;
    }

    public ref float[,] Map()
    {
        return ref this.map;
    }
    
    public List<float> GetVerticlesValues(List<Vector2Int> verticlesCoordinates)
    {
        var result = new List<float>();
        foreach (var verticleCoordinates in verticlesCoordinates)
        {
            result.Add(GetVerticleValue(verticleCoordinates));
        }

        return result;
    }

    public float GetVerticleValue(Vector2Int coordinates)
    {
        var xCoordinate = MathInt.Clamp(coordinates.x + offset, 0, this.limits.x + offset);
        var yCoordinate = MathInt.Clamp(coordinates.y + offset, 0, this.limits.y + offset);
        return this.map[xCoordinate, yCoordinate];
    }

    public void SetVerticleValue(Vector2Int coordinates, float value, bool calculated = true)
    {
        this.calculated[coordinates.x + offset, coordinates.y + offset] = calculated;
        this.map[coordinates.x + offset, coordinates.y + offset] = value;
    }

    public ref float[,] Initialize(Vector2Int size, BorderTypes borderTypes, HeightLimits heightLimits)
    {
        this.map = new float[size.x + offset*2, size.y + offset*2];
        this.calculated = new bool[size.x + offset * 2, size.y + offset * 2];
        this.limits = new Vector2Int(size.x, size.y);
        Nullize(borderTypes, heightLimits);
        return ref this.map;
    }

    private void Nullize(BorderTypes borderTypes, HeightLimits heightLimits)
    {
        for (var x = 0; x < this.limits.x; x++)
        {
            for (var y = 0; y < this.limits.y; y++)
            {
                SetVerticleValue(new Vector2Int(x, y), 0f, false);
            }
        }

        InitializeBorders(borderTypes, heightLimits);
    }

    public void InitializeBorders(BorderTypes borderTypes, HeightLimits heightLimits)
    {
        for (var x = 0; x < this.limits.x + offset*2; x++)
        {
            for (var y = 0; y < this.limits.y + offset*2; y++)
            {
                if (x <= offset - 1 || y <= offset - 1 || x >= this.limits.x + offset || y >= this.limits.y + offset)
                    this.map[x, y] = this.GetBorderHeight(x, y, borderTypes, heightLimits);
            }
        }
    }

    public ref float[,] PostProcess(HeightLimits heightLimits, float coefficient)
    {
        for (var x = 0; x < limits.x; x++)
        {
            for (var y = 0; y < limits.y; y++)
            {
                var coordinates = new Vector2Int(x, y);
                SquareInt rectangle = new SquareInt(coordinates, 1);
                var verticleValues = this.GetVerticlesValues(rectangle.GetVerticlesCoordinates());
                var averageSquare = MathInt.Sum(verticleValues) / verticleValues.Count;
                
                verticleValues = this.GetVerticlesValues(rectangle.GetDiamondCoordinates());
                var averageDiamond = MathInt.Sum(verticleValues) / verticleValues.Count;

                var average = (averageSquare + averageDiamond) / 2;
                
                var displacement = Displace(coordinates, heightLimits, coefficient / 2);

                SetVerticleValue(coordinates, average + displacement);
            }
        }
        
        return ref this.map;
    }
    
    private float GetBorderHeight(int x, int y, BorderTypes borderTypes, HeightLimits heightLimits)
    {
        var result = 0;
        var borderType = BorderType.NotBorder;
        if (x <= offset - 1)
            borderType = borderTypes.SouthWest;
        else if (y >= this.limits.y + offset)
            borderType = borderTypes.NorthWest;
        else if (x >= this.limits.x + offset)
            borderType = borderTypes.NorthEast;
        else if (y <= offset - 1)
            borderType = borderTypes.SouthEast;

        if (borderType == BorderType.NotBorder)
            result = 0;
        else if (borderType == BorderType.Land)
            result = heightLimits.Maximal;
        else if (borderType == BorderType.Sea)
            result = heightLimits.Minimal;
        
        return result;
    }
    
    public ref float[,] RandomizeCorners(Vector2Int position, int Size, HeightLimits heightLimits, float coefficient) 
    {
        for (var x = position.x; x < position.x + Size; x += (Size - 1))
        {
            for (var y = position.y; y < position.y + Size; y += (Size - 1))
            {
                if (this.calculated[x + offset, y + offset])
                {
                    continue;
                }

                var coordinates = new Vector2Int(x, y);
                SquareInt rectangle = new SquareInt(coordinates, 1);
                var verticleValues = this.GetVerticlesValues(rectangle.GetVerticlesCoordinates());
                
                var average = MathInt.Sum(verticleValues) / verticleValues.Count;
                var displacement = Displace(coordinates, heightLimits, coefficient);

                SetVerticleValue(coordinates, average + displacement);
            }
        }
        
        return ref this.map;
    }

    public ref float[,] Square(RectangleInt rectangle, HeightLimits heightLimits, float coefficient)
    {
        var centerCoordinates = rectangle.GetCenterCoordinates();
        if (this.calculated[centerCoordinates.x + offset, centerCoordinates.y + offset])
        {
            return ref this.map;
        }
        
        var verticleValues = this.GetVerticlesValues(rectangle.GetVerticlesCoordinates());

        var average = MathInt.Sum(verticleValues) / verticleValues.Count;
        var displacement = Displace(centerCoordinates, heightLimits, coefficient);
        
        SetVerticleValue(centerCoordinates, average + displacement);

        return ref this.map;    
    }

    private static float deprecatedDisplace(float height, HeightLimits heightLimits, float coefficient)
    {
        
        var lLimit = -1 * coefficient;//heightLimits.Minimal - height;
        var rLimit = 1 * coefficient;//heightLimits.Maximal - height;

        return Random.Range(lLimit, rLimit);
    }
    
    public float Displace(Vector2Int coordinates, HeightLimits heightLimits, float coefficient)
    {
        bool isNegative = Random.Range(-1, 1) < 0;
        var Displace = Mathf.PerlinNoise(coordinates.x, coordinates.y) * coefficient;
        if (isNegative) 
            Displace = - Displace;

        return Displace;
    }
    
    public ref float[,] Diamond(RectangleInt rectangle, HeightLimits heightLimits, float coefficient)
    {
        var centerCoordinates = rectangle.GetCenterCoordinates();
        if (this.calculated[centerCoordinates.x + offset, centerCoordinates.y + offset])
        {
            return ref this.map;
        }
        
        var verticleValues = this.GetVerticlesValues(rectangle.GetDiamondCoordinates());

        var average = MathInt.Sum(verticleValues) / verticleValues.Count;
        var displacement = Displace(centerCoordinates, heightLimits, coefficient);
        
        SetVerticleValue(centerCoordinates, average + displacement);
        return ref this.map;
    }

}


public class MapGenerator : MonoBehaviour
{
    
    public GeneralParameters parameters;

    floatHeightMapHandler HeightMap;

    private void Start()
    {
        HeightMap = new floatHeightMapHandler();
        HeightMap.offset = parameters.Offset;
    }

    private void RandomizeBorderTypes()
    {
        if (parameters.BorderTypes.NorthEast == BorderType.Random)
            parameters.BorderTypes.NorthEast = (BorderType) RandomInt.Range(0, 1);
        if (parameters.BorderTypes.NorthWest == BorderType.Random)
            parameters.BorderTypes.NorthWest = (BorderType) RandomInt.Range(0, 1);
        if (parameters.BorderTypes.SouthEast == BorderType.Random)
            parameters.BorderTypes.SouthEast = (BorderType) RandomInt.Range(0, 1);
        if (parameters.BorderTypes.SouthWest == BorderType.Random)
            parameters.BorderTypes.SouthWest = (BorderType) RandomInt.Range(0, 1);
    }

    public void Generate()
    {
        HeightMap.Clear();
        HeightMap.Initialize(parameters.MapLimits, parameters.BorderTypes, parameters.HeightLimits);

        int SquareSize = MathInt.GCD(parameters.MapLimits.x, parameters.MapLimits.y);
        
        
        var mapLimits = parameters.MapLimits;
        var heightLimits = parameters.HeightLimits;
        var coefficient = parameters.Coefficient;
        var postProcessCoefficient = parameters.PostProcessCoefficient;
        var currentSize = parameters.MapLimits.x;

        for (var x = 0; x + SquareSize - 1 < parameters.MapLimits.x; x += SquareSize - 1)
        {
            for (var y = 0; y + SquareSize - 1 < parameters.MapLimits.y; y += SquareSize - 1)
            {
                var startPosition = new Vector2Int(x, y);
                HeightMap.RandomizeCorners(startPosition, SquareSize, parameters.HeightLimits,
                    parameters.Coefficient);
            }
        }
        
        for (var x = 0; x + SquareSize - 1  < parameters.MapLimits.x; x += SquareSize - 1)
        {
            for (var y = 0; y + SquareSize - 1 < parameters.MapLimits.y; y += SquareSize - 1)
            {
                this.SquareDiamond(new Vector2Int(x, y), 
                        SquareSize, new Vector2Int(x + SquareSize, y + SquareSize), 
                            heightLimits, coefficient);
            }
        }
        //if (parameters.EnablePostProcess)
        //    HeightMap.PostProcess(heightLimits, postProcessCoefficient);
    }

    public float[,] GetMap(bool PostProccess = false, float coeff = 0)
    {
        var map = HeightMap.Map();
        
        float[,] mapBackup = CopyMap(map);
        float[,] outputMap;
        if (PostProccess)
        {
            HeightMap.PostProcess(parameters.HeightLimits, coeff);
            HeightMap.PostProcess(parameters.HeightLimits, coeff);
            outputMap = CopyMap(map);
            HeightMap.Map() = mapBackup;
        }
        else
        {
            outputMap = mapBackup;
        }
        
        return outputMap;
    }

    public float[,] CopyMap(float[,] map)
    {
        float[,] mapCopy = new float[map.GetLength(0), map.GetLength(1)];
        for (var x = 0; x < parameters.MapLimits.x + 2 * parameters.Offset; x++)
        {
            for (var y = 0; y < parameters.MapLimits.y + 2 * parameters.Offset; y++)
            {
                mapCopy[x, y] = map[x, y];
            }
        }

        return mapCopy;
    }
    
    private void SquareDiamond(Vector2Int startPosition, int currentSize, Vector2Int currentLimits, HeightLimits heightLimits, float coefficient)
    {
        //List<Vector2Int> pathCoordinates = getPath();
        while (currentSize > 1)
        {
            for (var currentX = startPosition.x; currentX+currentSize < currentLimits.x; currentX += currentSize)
            {
                for (var currentY = startPosition.y; currentY + currentSize < currentLimits.y; currentY += currentSize)
                {
                    SquareInt square = new SquareInt(currentX, currentY, currentSize);

                    HeightMap.Square(square, heightLimits, parameters.Coefficient);
                }
            }
           
            for (var currentX = startPosition.x; currentX+currentSize < currentLimits.x; currentX += currentSize)
            {
                for (var currentY = startPosition.y; currentY+currentSize < currentLimits.y; currentY += currentSize)
                {
                    
                    SquareInt square = new SquareInt(currentX, currentY, currentSize);
                    foreach (var verticleCoordinates in square.GetDiamondCoordinates())
                    {
                        SquareInt currentSquare = new SquareInt(verticleCoordinates, currentSize);
                        HeightMap.Diamond(currentSquare, heightLimits, coefficient);
                    }
                }
            }
            
            currentSize /= 2;
            coefficient /= 2;
        }
    }
    
}
