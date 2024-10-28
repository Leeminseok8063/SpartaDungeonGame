using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField]float destroyTimerValue = 0f;
    float localTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        localTimer += Time.deltaTime;
        if (localTimer > destroyTimerValue)
            Destroy(this.gameObject);
    }
}
