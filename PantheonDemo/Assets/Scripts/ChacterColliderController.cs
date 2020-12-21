using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChacterColliderController : MonoBehaviour
{
    //TO DO:
    //Sonradan bu variable'a atama yap. AI zamanında. Simdilik statik
    public GameObject characterCheckPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            this.ResetCharacterToCheckPoint();
        }
    }

    private void ResetCharacterToCheckPoint()
    {
        transform.position = characterCheckPoint.transform.position;
    }
}
