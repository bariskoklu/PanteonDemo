using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    private Rigidbody rb;
    private float screenWidth;
    [SerializeField]private float swerveSpeed;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (moveSpeed * Time.deltaTime));

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            float inputPositionX = Input.GetTouch(0).position.x;
            if (inputPositionX > screenWidth / 2)
            {
                //move right
                MoveCharacter(1.0f * (inputPositionX - screenWidth / 2));
            }
            if (inputPositionX < screenWidth / 2)
            {
                //move left
                MoveCharacter(-1.0f * (screenWidth / 2 - inputPositionX));
            }
        }
#else
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float inputPositionX = Input.mousePosition.x;
            if (inputPositionX > screenWidth / 2)
            {
                //move right
                MoveCharacter(1.0f * (inputPositionX - screenWidth / 2));
            }
            if (inputPositionX < screenWidth / 2)
            {
                //move left
                MoveCharacter(-1.0f * (screenWidth / 2 - inputPositionX));
            }
        }
#endif
    }

    private void MoveCharacter(float horizontalInput)
    {
        rb.AddForce(new Vector3(horizontalInput * swerveSpeed * Time.deltaTime, 0));
    }
}
