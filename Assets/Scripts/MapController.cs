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
    
    //Dev parameters
    public Tile prefabTile;
    
    //Service parameters
    private Tilemap map;
    private Camera mainCamera;
    
    void Start()
    {
        this.map = GetComponent<Tilemap>();
        this.mainCamera = Camera.main;

        map.BoxFill(new Vector3Int(0, 0, 0), prefabTile, 0, 0, Length, Width);
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
