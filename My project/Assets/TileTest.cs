using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap tilemap;
    //마우스가 타일 위에 위치할 때만 작업할 것이기 때문에 onMouseOver를 사용했습니다.

    //가능하면 기즈모로 하는것도 좋을것 같네요.

    private void OnMouseEnter()
    {
        try
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);

           
            if (this.tilemap = hit.transform.GetComponent<Tilemap>())
            {
                this.tilemap.RefreshAllTiles();
                int x, y;
                x = this.tilemap.WorldToCell(ray.origin).x;
                y = this.tilemap.WorldToCell(ray.origin).y;

                Vector3Int v3Int = new Vector3Int(x, y, 0);

                //타일 색 바꿀 때 이게 있어야 하더군요
                this.tilemap.SetTileFlags(v3Int, TileFlags.None);

                //타일 색 바꾸기
                this.tilemap.SetColor(v3Int, (Color.red));

                Debug.Log("Current Cell's Loc = " + this.tilemap.CellToLocal(v3Int));
                
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("TilemapS is nullptr");
        }
    }
    private void OnMouseExit()
    {
        this.tilemap.RefreshAllTiles();

    }

    private async void ScanTileInLayer()
    {
        Tilemap TilemapInmap;
        TilemapInmap = GetComponent<Tilemap>();

        BoundsInt bounds = TilemapInmap.cellBounds;
        TileBase[] allTiles = TilemapInmap.GetTilesBlock(bounds);
        string[] TileInfoString = new string[bounds.size.y* bounds.size.x];
        string tmp;
        int stringIdx = 0;
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                    tmp = "x:" + x + " y:" + y + " tile:" + tile.name;
                else
                    tmp = "x:" + x + " y:" + y + " tile: (null)";
                TileInfoString.SetValue(tmp, stringIdx++);
            }
        }

        await File.WriteAllLinesAsync("WriteLines.txt", TileInfoString);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("TilemapS is Start");
        ScanTileInLayer();
    }

    // Update is called once per frame
    void Update()
    {
        //onMouseOver();
    }
}
