using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBound : MonoBehaviour
{
    // Define the upper and lower bounds to check if the object is out of range
    public float topBound = 40.0f;  // The upper bound position
    public float lowerBound = -15.0f; // The lower bound position
    public GameObject onCollectEffect;  // Effect to instantiate when the object is collected (currently not used)

    // Start is a Unity lifecycle function, called once at the beginning
    void Start()
    {
        // Currently no action performed in this function
    }

    // Update is a Unity lifecycle function, called once per frame
    void Update()
    {
        // Check if the object is below the lower bound (Z position less than lowerBound)
        if (transform.position.z < lowerBound)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }

    // This function is called when another object enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Instantiate the collect effect if desired (currently not used)
            // Instantiate(onCollectEffect, transform.position, transform.rotation);

            // Destroy the object when it collides with the player
            Destroy(gameObject);
        }
    }
}

