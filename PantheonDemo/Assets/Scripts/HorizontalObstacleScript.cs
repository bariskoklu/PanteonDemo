using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObstacleScript : MonoBehaviour
{
    enum Directions
    {
        North,
        South,
        East,
        West
    };

    public GameObject platform;

    [SerializeField]private Directions direction; //Gidilecek ilk noktanın yönü. 
    [SerializeField]private float speed;
    private float platformWidthX;
    private float platformWidthZ;

    private void Start()
    {
        platformWidthX = platform.GetComponent<MeshRenderer>().bounds.size.x;
        platformWidthZ = platform.GetComponent<MeshRenderer>().bounds.size.z;
    }


    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case Directions.East:
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                if (transform.position.x > platform.transform.position.x + platformWidthX / 2)
                {
                    direction = Directions.West;
                }
                break;
            case Directions.West:
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
                if (transform.position.x < platform.transform.position.x  - platformWidthX / 2)
                {
                    direction = Directions.East;
                }
                break;
            case Directions.North:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);
                if (transform.position.z > platform.transform.position.z + platformWidthZ / 2)
                {
                    direction = Directions.South;
                }
                break;
            case Directions.South:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
                if (transform.position.z < platform.transform.position.z - platformWidthZ / 2)
                {
                    direction = Directions.North;
                }
                break;
            default:
                Debug.Log("No Direction is selected for HorizontalObstacle");
            break;
        }
        if (true)
        {

        }
    }
}
