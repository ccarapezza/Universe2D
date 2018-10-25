using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public GameObject background;
    public float dump;

	void Update () {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z) , Time.deltaTime * dump);
        background.transform.position = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);
    }
}