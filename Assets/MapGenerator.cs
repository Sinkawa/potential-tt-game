using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathIntLib;
using AdditionalClassesInt;

[System.Serializable]
public class HeightLimits {
    public int Minimal;
    public int Maximal;
}

[System.Serializable]
public class GeneralParameters {
    public HeightLimits HeightLimits;
    public Vector2Int MapLimits;
}

class IntHeightMapHandler 
{
    private int [,] map;
    private int minimalEdge;
    private Vector2Int limits;
    public void Clear()
    {
        if (map != null)
        {
            map = null;
        }
    }

    public ref int[,] Map()
    {
        return ref this.map;
    }

    public List<int> GetVerticlesValues(RectangleInt rectangle)
    {
        List<int> result = new List<int>();
        foreach (Vector2Int verticlePosition in rectangle.GetVerticlesCoordinates())
        {
            result.Add(this.map[verticlePosition.x, verticlePosition.y]);
        }
        return result;
    }

    public ref int[,] Initialize(Vector2Int size) 
    {
        this.map = new int[size.x, size.y];
        this.limits = new Vector2Int(size.x, size.y);
        Nullize();
        return ref this.map;
    }

    private void Nullize()
    {
        for (int x = 0; x < this.limits.x; x++)
        {
            for (int y = 0; y < this.limits.y; y++) 
            {
                this.map[x, y] = 0;
            }
        }
    }

    public ref int[,] RandomizeCorners(HeightLimits heightLimits) 
    {
        for (int x = 0; x < limits.x; x = x + (limits.x - 1))
        {
            for (int y = 0; y < limits.y; y = y + (limits.y - 1))
            {
                int roundedHeight = RandomInt.Range(heightLimits.Minimal, heightLimits.Maximal);
                this.map[x, y] = roundedHeight;
            }
        }

        return ref this.map;
    }

    public ref int[,] Square(RectangleInt rectangle, HeightLimits heightLimits)
    {
        var verticleValues = this.GetVerticlesValues(rectangle);

        int average = MathInt.Average(verticleValues);
        int displacement = this.Displace(average, heightLimits);

        var centerCoordinates = rectangle.GetCenterCoordinates();
        this.map[centerCoordinates.x, centerCoordinates.y] = average + displacement;

        return ref this.map;    
    }

    int Displace(int height, HeightLimits heightLimits)
    {
        int lLimit = heightLimits.Minimal - height;
        int rLimit = heightLimits.Maximal - height;

        return RandomInt.Range(lLimit, rLimit);
    }
}

public class MapGenerator : MonoBehaviour
{

    public GeneralParameters parameters;
    
    IntHeightMapHandler HeightMap;

    public void Generate() 
    {
        HeightMap.Clear();
        HeightMap.Initialize(parameters.MapLimits);
        HeightMap.RandomizeCorners(parameters.HeightLimits);
    }

  
}
