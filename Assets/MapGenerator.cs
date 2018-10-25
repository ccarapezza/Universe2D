using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public TileEntity tilePrefab;

    // Use this for initialization
    void Start () {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                print(Mathf.PerlinNoise(i, j));
            }
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
