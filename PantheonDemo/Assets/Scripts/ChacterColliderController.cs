using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChacterColliderController : MonoBehaviour
{
    //TO DO:
    //Sonradan bu variable'a atama yap. AI zamanında. Simdilik statik
    public GameObject characterCheckPoint;


    [SerializeField] private float positionYToFallOver = -10.0f;

    void Update()
    {
        CheckIfCharacterFallOff();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ResetCharacterToCheckPoint();
        }
        else if (other.CompareTag("FinishLine"))
        {
            HandleCharacterFinishLine();
        }
    }

    private void CheckIfCharacterFallOff()
    {
        //Since the platforms y position does not change, this works but if it changes in the future, we should be detecting which platform we are currently on 
        //and compare position with that
        if (transform.position.y < positionYToFallOver)
        {
            ResetCharacterToCheckPoint();
        }
    }

    private void HandleCharacterFinishLine()
    {
        gameObject.GetComponent<CharacterClass>().HandleFinishLine();
    }

    private void ResetCharacterToCheckPoint()
    {
        transform.position = characterCheckPoint.transform.position;
    }
}
