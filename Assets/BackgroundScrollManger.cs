using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollManger : MonoBehaviour {

    public GameObject reference;
    public SeamlessManager seamlessManager;

    public float verticalScrollVelocity = 1;
    public float horizontalScrollVelocity = 1;

    public Vector2 offset;

    private Renderer m_backgroundRenderer;
    private Vector3 m_oldPos;

    private void Awake()
    {
        m_backgroundRenderer = GetComponent<Renderer>();
        m_oldPos = reference.transform.position;
        m_backgroundRenderer.material.mainTextureOffset = reference.transform.position;
    }

    private void Update()
    {
        Vector2 diff = (m_oldPos - reference.transform.position);
        diff = new Vector2(diff.x*horizontalScrollVelocity, diff.y*verticalScrollVelocity);
        print("Diff: "+diff);
        m_backgroundRenderer.material.mainTextureOffset -= diff;
        m_oldPos = reference.transform.position;
    }
}
