using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int m_width, m_height;
    [SerializeField] private GridTile m_tilePrefab;
    [SerializeField] private Transform m_camera;
    private void Start()
    {
        GenerateGrid();
    }
    
    
    private void GenerateGrid()
    {
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                var spawnedTile = Instantiate(m_tilePrefab, new Vector3(x, y), Quaternion.identity, transform);
                spawnedTile.name = $"Tile {x} {y}";
                var isOffset = x % 2 == 0 && y % 2 != 0 || x % 2 != 0 && y % 2 == 0;
                spawnedTile.Init(isOffset, x,y);
            }
        }
        // TODO remplacer référence par un évènement et un truc qui fonctionne mieux
        m_camera.transform.position = new Vector3(m_width / 2f - 0.5f, m_height / 2f - 0.5f, -10f);
    }

}
