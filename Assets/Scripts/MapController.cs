using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    //Generator parameters
    [SerializeField] private int Width;
    [SerializeField] private int Length;
    [SerializeField] private float Scale;
    [SerializeField] private float Threshold;
    [SerializeField] private float Threshold2;
    //Dev parameters
    public RuleTile prefabTile;
    public RuleTile prefabTreesTile;
    
    //Service parameters
    private Tilemap map;
    private Camera mainCamera;
    
    void Start()
    {
        this.map = GetComponent<Tilemap>();
        this.mainCamera = Camera.main;
        


        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < Length; x++) 
        {
            for (int y = 0; y < Width; y++) 
            {
                float BerlinNoise = Mathf.PerlinNoise(x*Scale, y*Scale);
                Debug.Log($"For coordinates {x}/{y} BerlinNoise equals {BerlinNoise}");
                if (BerlinNoise > Threshold) {
                    int positionZ = (int) Mathf.Round(BerlinNoise * 10);
                    this.map.SetTile(new Vector3Int(x, y, positionZ), this.prefabTile);
                    
                }
            }
        }
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
