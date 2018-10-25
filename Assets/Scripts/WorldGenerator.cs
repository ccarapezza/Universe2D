using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using System;

[Serializable]
public class TileSprite
{
    public TileType type;
    public Sprite sprite;
}

public class WorldGenerator : MonoBehaviour {
    public Vector2 worldSize;
    public Vector2 offset;
    public TileEntity tilePrefab;
    public List<TileSprite> tileSet;
    public static List<TileSprite> TILE_SET;

    private SeamlessManager m_seamlessManager;

    private void Awake()
    {
        

        m_seamlessManager = GetComponent<SeamlessManager>();
        WorldGenerator.TILE_SET = tileSet;
        /*for (int i = 0; i > -worldSize.y; i--)
        {
            for (int j = 0; j < worldSize.x; j++)
            {
                TileType tileType = TileType.Dirt;
                if (i == 0)
                    tileType = TileType.DirtGrass;

                TileEntity tile = CreateTile(tileType, new Vector2(j, i));
                if (i == 0 && j == 0)
                    m_seamlessManager.startWorld = tile.GetComponentInChildren<BoxCollider2D>();

                if (i == 0 && j == worldSize.x - 1)
                    m_seamlessManager.endWorld = tile.GetComponentInChildren<BoxCollider2D>();
            }
        }*/

        m_seamlessManager.startWorld = offset.x;
        m_seamlessManager.endWorld = (int)worldSize.x-2 + offset.x;

        int[,] map = GenerateArray((int)worldSize.x, (int)worldSize.y, true);
        map = PerlinNoiseTst(map, UnityEngine.Random.Range(0f, 1f - Mathf.Epsilon));
        /*Perlin perlin = new Perlin();
        print(perlin.perlin(0.15, 0.20, 0));
        print(perlin.perlin(0.10, 0.15, 0));
        print(perlin.perlin(0.5, 0.32, 0));*/
        //map = PerlinNoise(map, UnityEngine.Random.Range(0, float.MaxValue
        RenderMap(map);
    }

    public static int[,] PerlinNoiseTst(int[,] map, float seed)
    {
        int newPoint;
        print("Seed: "+seed);
        //Create the Perlin
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            Perlin perlin = new Perlin();
            //print(Mathf.PerlinNoise(x * reduction, seed * reduction));
            var realX = (float)x / map.GetUpperBound(0);
            print("X: "+ realX + "Perlin:" + perlin.perlin(realX, seed, seed));

            newPoint = Mathf.FloorToInt((float)perlin.perlin(realX, seed, seed) * map.GetUpperBound(1));
            print("NewPoint:" + newPoint);
            //Make sure the noise starts near the halfway point of the height
            /*newPoint += (map.GetUpperBound(1) / 2);
            print("++NewPoint:" + newPoint);*/
            for (int y = newPoint; y >= 0; y--)
            {
                //print("x:" + x + "y:" + y);
                map[x, y] = 1;
            }
        }
        return map;
    }

    private TileEntity CreateTile(TileType type, Vector2 position)
    {
        TileEntity tile = Instantiate(tilePrefab);
        tile.Type = type;
        tile.transform.position = position;
        return tile;
    }

    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public void RenderMap(int[,] map)
    {
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    CreateTile(TileType.Dirt, new Vector2(x, y)+offset);
                }
            }
        }
    }



    public int[,] PerlinNoise(int[,] map, float seed)
    {
        int newPoint;
        //Used to reduced the position of the Perlin point
        float reduction = 0.5f;
        //Create the Perlin
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction) * map.GetUpperBound(1));
            print(newPoint);

            //Make sure the noise starts near the halfway point of the height
            newPoint += (map.GetUpperBound(1) / 2);
            for (int y = newPoint; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }
}