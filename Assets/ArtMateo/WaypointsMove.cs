using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsMove : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float minimumDistance;
    private int currentIndex;
    private bool goBack;

    public List<Transform> waypoints;

    void Start()
    {
        
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        Vector3 deltaVector = waypoints[currentIndex].position - transform.position;
        Vector3 direction = deltaVector.normalized;

        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;

        float distance = deltaVector.magnitude;
        if (distance < minimumDistance)
        {
            currentIndex = goBack ? --currentIndex : ++currentIndex;
            if (currentIndex >= waypoints.Count)
            {
                currentIndex = 0;
            }
            else if (currentIndex <= 0)
            {
                goBack = false;
            }
        }
    }
}
