using System.Collections;
using System.Collections.Generic;
using AdditionalClassesInt;
using AdditionalDataStructures;
using MathIntLib;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class MapController : MonoBehaviour
{
    //Generator parameters
    // [SerializeField] private int Width;
    // [SerializeField] private int Length;
    // [SerializeField] private float Scale;
    // [SerializeField] private float Threshold;
    // [SerializeField] private float Threshold2;
    //Dev parameters
    public RuleTile prefabTile;
    public RuleTile prefabTreesTile;

    //Service parameters
    private Tilemap map;
    public Tilemap map2D;
    private float[,] heightMap;
    private Camera mainCamera;
    private MapGenerator mapGenerator;
    public RawImage image;

    void Start()
    {
        this.map = GetComponent<Tilemap>();
        this.mainCamera = Camera.main;
        this.mapGenerator = GetComponent<MapGenerator>();


        GenerateMap();
    }

    void GenerateMap()
    {
        mapGenerator.Generate();
        heightMap = mapGenerator.GetMap();
        outputMap(heightMap, 0);
        int lastX = heightMap.GetLength(1);
        
        outputMap(
            mapGenerator.GetMap(true, mapGenerator.parameters.PostProcessCoefficient), lastX + 3);
       
    }
    
    void outputMap(float[,] map, int offset = 0)
    {
        for (var x = 0; x < mapGenerator.parameters.MapLimits.x + 2 * mapGenerator.parameters.Offset; x++)
        {
            for (var y = 0; y < mapGenerator.parameters.MapLimits.y + 2 * mapGenerator.parameters.Offset; y++)
            {
                //image.sprite.texture.SetPixel(x, y, new Color(heightMap[x, y], heightMap[x, y], heightMap[x, y]));
                this.map2D.SetTile(new Vector3Int(x, y + offset, 0), this.prefabTile);
                this.map2D.SetTileFlags(new Vector3Int(x, y + offset, 0), TileFlags.None);
                var huyomoyo = GetColorX(MathInt.Round(map[x, y]),
                    mapGenerator.parameters.HeightLimits.Minimal,
                    mapGenerator.parameters.HeightLimits.Maximal);
                Debug.Log(huyomoyo);
                this.map2D.SetColor(new Vector3Int(x, y + offset, 0), huyomoyo);
            }
        }    
    }

    Color GetColorX(int value, int minValue, int maxValue)
    {
        float cG = 1f, cB = 0f;

        if (value <= mapGenerator.parameters.HeightLimits.SeaLevel)
        {
            cG = 0f;
            cB = 3f;
        }
        
        int cv = value - minValue;
        int dv = maxValue - minValue;
        float pc = 1.0f * cv / dv;
        pc *= pc;

        return new Color(0, pc * cG, pc * cB);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickCellPosition = map.WorldToCell(clickPosition);
            Debug.Log(clickCellPosition);
        }
    }

    
}
