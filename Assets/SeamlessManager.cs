using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessManager : MonoBehaviour {

    public GameObject playerCamera;
    public GameObject player;
    public BoxCollider2D startWorld;
    public BoxCollider2D endWorld;

    private float m_startX;
    private float m_endX;
    private float m_worldSize;
    private Camera m_playerCamera;
    private GameObject m_secondaryCamera;
    private float m_halfFurstrumSize;

    private void Start()
    {
        m_startX = startWorld.transform.position.x - startWorld.size.x * startWorld.transform.localScale.x / 2;
        m_endX = endWorld.transform.position.x + endWorld.size.x * endWorld.transform.localScale.x / 2;
        m_worldSize = m_endX - m_startX;
        m_secondaryCamera = Instantiate(playerCamera, playerCamera.transform);
        m_secondaryCamera.GetComponent<FollowPlayer>().enabled = false;
        m_secondaryCamera.transform.localPosition = Vector3.right * m_worldSize;
        m_playerCamera = playerCamera.GetComponent<Camera>();
        m_halfFurstrumSize = Vector3.Distance(m_playerCamera.transform.position, m_playerCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)));
    }

    void Update () {
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

        float leftLimitXPos = playerCamera.transform.position.x - m_halfFurstrumSize;
        float rightLimitXPos = playerCamera.transform.position.x + m_halfFurstrumSize;

        if (leftLimitXPos > m_startX && rightLimitXPos < m_endX)
        {
            m_secondaryCamera.SetActive(false);
        }
        else
        {
            if (leftLimitXPos < m_startX)
                m_secondaryCamera.transform.localPosition = Vector3.right * m_worldSize;
            else if (rightLimitXPos > m_endX)
                m_secondaryCamera.transform.localPosition = Vector3.left * m_worldSize;

            m_secondaryCamera.SetActive(true);
        }

    }

    private void OnDrawGizmos()
    {
        m_startX = startWorld.transform.position.x - startWorld.size.x * startWorld.transform.localScale.x * 0.5f;
        m_endX = endWorld.transform.position.x + endWorld.size.x * endWorld.transform.localScale.x * 0.5f; ;

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
