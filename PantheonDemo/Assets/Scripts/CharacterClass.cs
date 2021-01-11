using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    public enum PlayerPhase
    {
        platforming,
        drawing,
        gameOver
    }

    [SerializeField] public PlayerPhase currentPlayerPhase = PlayerPhase.platforming;
    
    public virtual void HandleFinishLine()
    {
        Debug.Log("HandleFinishLine must be overridden");
    }

    public void FreezeCharacterRigidbody()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                                                           RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
