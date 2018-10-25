using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessManager : MonoBehaviour {

    public Renderer backgroundRenderer;
    public GameObject playerCamera;
    public GameObject player;
    public float startWorld;
    public float endWorld;
    public float StartX { get{ return m_startX; } }
    public float EndX { get { return m_endX; } }
    public float horizontalScrollVelocity = 1;
    public float verticalScrollVelocity = 1;
    private float m_startX;
    private float m_endX;
    private float m_worldSize;
    private Camera m_playerCamera;
    private Camera m_secondaryCamera;
    private float m_halfFurstrumSize;

    private Vector3 m_oldPos;

    private void Start()
    {
        m_startX = startWorld - 0.5f;
        m_endX = endWorld + 0.5f;
        m_worldSize = m_endX - m_startX;
               
        m_secondaryCamera = Instantiate(playerCamera.GetComponent<Camera>(), playerCamera.transform);


        foreach (Transform child in m_secondaryCamera.transform)
            Destroy(child.gameObject);

        Destroy(m_secondaryCamera.GetComponent<FollowPlayer>());
        Destroy(m_secondaryCamera.GetComponent<AudioListener>());
        m_secondaryCamera.transform.localPosition = Vector3.right * m_worldSize;
        m_playerCamera = playerCamera.GetComponent<Camera>();
        m_halfFurstrumSize = Vector3.Distance(m_playerCamera.transform.position, m_playerCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)));

        m_oldPos = playerCamera.transform.position;
        backgroundRenderer.material.mainTextureOffset = playerCamera.transform.position;
    }

    void Update () {
        Vector2 diff = (m_oldPos - playerCamera.transform.position);
        diff = new Vector2(diff.x * horizontalScrollVelocity, diff.y * verticalScrollVelocity);
        backgroundRenderer.material.mainTextureOffset -= diff;

        if (player.transform.position.x > m_endX)
        {
            float oldX = player.transform.position.x;
            player.transform.position = new Vector3(m_startX + player.transform.position.x - m_endX, player.transform.position.y, player.transform.position.z);
            playerCamera.transform.position += Vector3.left * (oldX - player.transform.position.x);
        }

        if (player.transform.position.x < m_startX)
        {
            float oldX = player.transform.position.x;
            player.transform.position = new Vector3(m_endX - player.transform.position.x + m_startX, player.transform.position.y, player.transform.position.z);
            playerCamera.transform.position += Vector3.left * (oldX - player.transform.position.x);
        }

        m_oldPos = playerCamera.transform.position;

        float leftLimitXPos = playerCamera.transform.position.x - m_halfFurstrumSize;
        float rightLimitXPos = playerCamera.transform.position.x + m_halfFurstrumSize;

        if (leftLimitXPos > m_startX && rightLimitXPos < m_endX)
        {
            m_secondaryCamera.gameObject.SetActive(false);
        }
        else
        {
            if (leftLimitXPos < m_startX)
                m_secondaryCamera.transform.localPosition = Vector3.right * m_worldSize;
            else if (rightLimitXPos > m_endX)
                m_secondaryCamera.transform.localPosition = Vector3.left * m_worldSize;

            m_secondaryCamera.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        m_startX = startWorld - 0.5f;
        m_endX = endWorld + 0.5f; ;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(m_startX, 0, 0), 0.25f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(m_endX, 0, 0), 0.25f);

        var vertExtent = playerCamera.GetComponent<Camera>().orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(playerCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0.5f, 0)), 0.25f);
        Gizmos.DrawSphere(playerCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1, 0.5f, 0)), 0.25f);
    }
}
