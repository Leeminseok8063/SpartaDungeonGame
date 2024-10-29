using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public float posX;
    public float posY;
    public float posZ;
    public float lerpScale;
    
    private GameObject mainPlayer;
    private Vector3 destPos;
    private Vector3 vel = Vector3.zero;

    private void Start()
    {
        mainPlayer = GameManager.Instance.Player.gameObject;
        destPos = new Vector3 (posX, posY, posZ);
        transform.position = destPos;
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, mainPlayer.transform.position + destPos, ref vel, lerpScale); 
    }
}
