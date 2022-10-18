using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileTest : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    string[] tempData;
    public InputField AxisX;
    public InputField AxisY;
    public InputField CanMove;
    public List<Vector3> tileWorldLocations;
    public Tilemap tilemap;
    List<string[]> data = new List<string[]>();
    string delimiter = ",";
    public string fileName = "TilemapAxis.csv";

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    public static string GetPath(string file)
    {
        string path = GetPath();
        return Path.Combine(GetPath(), file);
    }

    public static string GetPath()
    {
        string path = null;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Application.persistentDataPath, "Resources/");
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Assets", "Resources/");
            case RuntimePlatform.WindowsEditor:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Assets", "Resources/");
            default:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Resources/");
        }
    }


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
               // this.tilemap.SetTileFlags(v3Int, TileFlags.None);

                //타일 색 바꾸기
              //  this.tilemap.SetColor(v3Int, (Color.red));

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

    private void ScanTileInLayer()
    {
        Tilemap TilemapInmap;
        TilemapInmap = GetComponent<Tilemap>();
        fileName = "TilemapAxis.csv";


        data.Clear();
        tempData = new string[4];
        tempData[0] = "AxisX";
        tempData[1] = "AxisY";
        tempData[2] = "AxisZ";
        tempData[3] = "CanMove";
        data.Add(tempData);


        StringBuilder sb = new StringBuilder();

        BoundsInt bounds = TilemapInmap.cellBounds;
        TileBase[] allTiles = TilemapInmap.GetTilesBlock(bounds);
        string[] TileInfoString = new string[bounds.size.y * bounds.size.x];

        tileWorldLocations = new List<Vector3>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = TilemapInmap.CellToWorld(localPlace);
            if (TilemapInmap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }

        for (int i = 0; i < tileWorldLocations.Count; i++)
        {
            string[] tmp = new string[4];
            tmp[0] = tileWorldLocations[i].x.ToString();
            tmp[1] = tileWorldLocations[i].y.ToString();
            tmp[2] = tileWorldLocations[i].z.ToString();
            tmp[3] = "true";
            data.Add(tmp);
        }

        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        string filepath = GetPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        StreamWriter outStream = System.IO.File.CreateText(filepath + fileName);
        outStream.Write(sb);
        outStream.Close();
    }
    // Start is called before the first frame update
    void Start()
    {
        ScanTileInLayer();
    }

    // Update is called once per frame
    void Update()
    {
        //onMouseOver();
    }
}
