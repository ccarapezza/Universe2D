using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    DirtGrass,
    Dirt,
    Stone,
    Sand
}

public class TileEntity : MonoBehaviour {
    private SpriteRenderer m_spriteRenderer;
    private TileType? m_type = null;
    public TileType? Type {
        get
        {
            return m_type;
        }
        set
        {
            if (m_type == null)
                m_type = value;
        }
    }

    private void Start()
    {
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < WorldGenerator.TILE_SET.Count; i++)
        {
            if (WorldGenerator.TILE_SET[i].type == m_type)
                m_spriteRenderer.sprite = WorldGenerator.TILE_SET[i].sprite;
        }
        
    }
}
