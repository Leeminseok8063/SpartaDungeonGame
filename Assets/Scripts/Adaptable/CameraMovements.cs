using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public GameObject mainPlayer;
    public float posX;
    public float posY;
    public float posZ;
    public float lerpScale;
    private Vector3 destPos;
    private Vector3 vel = Vector3.zero;

    private void Start()
    {
        mainPlayer = FindObjectOfType<MovementController>().transform.gameObject;
        destPos = new Vector3 (posX, posY, posZ);
        transform.position = destPos;
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, mainPlayer.transform.position + destPos, ref vel, lerpScale); 
        //transform.position = mainPlayer.transform.position + destPos;
    }
}