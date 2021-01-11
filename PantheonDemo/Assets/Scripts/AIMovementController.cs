using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementController : CharacterClass
{

    [SerializeField] private Rigidbody rb;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float maxDistanceToCalculateAvoidingObject;
    private bool isMovingSideWays = false;
    private float horizontalCalculatedSpeed;

    [SerializeField] private BoolType isGameOver;

    [SerializeField] private GameObject obstaclesParent;
    private List<GameObject> allObstacles = new List<GameObject>();

    private GameObject collidedObject;
    private float collidingObjectsWidth;

    public GameObject startingPlatform;

    [SerializeField] private float timeToFinishDrawingMax;
    [SerializeField] private float timeToFinishDrawingMin;
    private float timeLeftToFinishDrawing;
    // Start is called before the first frame update
    void Start()
    {
        collidedObject = startingPlatform;
        collidingObjectsWidth = collidedObject.GetComponent<MeshRenderer>().bounds.size.x;
        foreach (Transform child in obstaclesParent.transform)
            allObstacles.Add(child.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGameOver();
        

        switch (currentPlayerPhase)
        {
            case PlayerPhase.platforming:
                isMovingSideWays = false;
                MoveForward();
                MoveHorizontally();
                break;
            case PlayerPhase.drawing:
                FakeDraw();
                break;
            default:
                break;
        }
    }

    //Faking drawing with randomness. Calculates random number between range and counts to that number to finish drawing and potantially win
    private void FakeDraw()
    {
        if (timeLeftToFinishDrawing < 0)
        {
            isGameOver.value = true;
        }
        else
        {
            timeLeftToFinishDrawing -= Time.deltaTime;
        }
    }

    private void CheckIfGameOver()
    {
        if (isGameOver.value)
        {
            currentPlayerPhase = PlayerPhase.gameOver;
        }
    }

    private void MoveForward()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (moveSpeed * Time.deltaTime / (isMovingSideWays ? 2: 3)) );
    }

    private void MoveHorizontally()
    {
        horizontalCalculatedSpeed = 0.0f;
        if (transform.position.x > collidedObject.transform.position.x + collidingObjectsWidth / 2)
        {
            AddForceWithRelativeDistanceToHorizontal(false, 2f);
        }
        else if (transform.position.x < collidedObject.transform.position.x - collidingObjectsWidth / 2)
        {
            AddForceWithRelativeDistanceToHorizontal(true, 2f);
        }
        for (int i = 0; i < allObstacles.Count; i++)
        {
            Vector3 playerRelative = allObstacles[i].transform.InverseTransformPoint(transform.position);
            if (playerRelative.z <= 0)// Obstacle is front of the player.
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
        Debug.Log(horizontalCalculatedSpeed);
        if (horizontalCalculatedSpeed < 0.002 && horizontalCalculatedSpeed > -0.002)
        {
            isMovingSideWays = true;
        }
        transform.position = new Vector3(transform.position.x + horizontalCalculatedSpeed, transform.position.y, transform.position.z);
    }

    private void AddForceWithRelativeDistanceToHorizontal(bool isToRight, float distance)
    {
        int horizontalInput = isToRight ? 1 : -1;
        if (distance < maxDistanceToCalculateAvoidingObject)
        {
            horizontalCalculatedSpeed += UnityEngine.Random.Range(1.0f,1.4f) * horizontalInput * horizontalSpeed * Time.deltaTime / Mathf.Pow(distance,2);
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        switch (col.collider.tag)
        {
            case "Platform":
                collidedObject = col.gameObject;
                collidingObjectsWidth = collidedObject.GetComponent<MeshRenderer>().bounds.size.x;
                break;
            case "RotatingPlatform":
                collidedObject = col.gameObject;
                collidingObjectsWidth = collidedObject.GetComponent<MeshRenderer>().bounds.size.x;
                break;
            default:
                break;
        }
    }

    public override void HandleFinishLine()
    {
        FreezeCharacterRigidbody();
        currentPlayerPhase = PlayerPhase.drawing;

        timeLeftToFinishDrawing = UnityEngine.Random.Range(timeToFinishDrawingMin, timeToFinishDrawingMax);
    }
}
