using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementController : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;

    [SerializeField]private float horizontalSpeed = 5000;
    [SerializeField] private float moveSpeed = 10;

    [SerializeField]private GameObject obstaclesParent;
    private List<GameObject> allObstacles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in obstaclesParent.transform)
            allObstacles.Add(child.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        MoveHorizontally();
        
    }

    private void MoveForward()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (moveSpeed * Time.deltaTime));
    }

    private void MoveHorizontally()
    {
        for (int i = 0; i < allObstacles.Count; i++)
        {
            Vector3 playerRelative = allObstacles[i].transform.InverseTransformPoint(transform.position);
            if (playerRelative.z < 0)// Obstacle is front of the player.
            {
                float distance = Vector3.Distance(transform.position, allObstacles[i].transform.position);
                if (playerRelative.x > 0)//Obstacle is rightside of the player
                {
                    AddForceWithRelativeDistanceToHorizontal(true, distance);
                }
                else
                {
                    AddForceWithRelativeDistanceToHorizontal(false, distance);
                }
            }
        }
    }

    private void AddForceWithRelativeDistanceToHorizontal(bool isToRight, float distance)
    {
        int horizontalInput = isToRight ? 1 : -1;
        rb.AddRelativeForce(new Vector3(horizontalInput * horizontalSpeed * Time.deltaTime / distance, 0));
    }
}
