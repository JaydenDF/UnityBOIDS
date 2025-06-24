using UnityEngine;
using System.Collections.Generic;

public class Separation : MonoBehaviour
{
    public float visionRadius = 1.5f;
    public float visionAngle = 120f; // degrees
    public float maxForce = 10f;

    private BoidWrap boid;
    private static Separation selectedBoid = null;
    private List<Transform> repelledNeighbors = new List<Transform>();

    void Start()
    {
        boid = GetComponent<BoidWrap>();
    }

    void Update()
    {
        Vector3 totalForce = Vector3.zero;
        int neighborCount = 0;
        repelledNeighbors.Clear();

        foreach (GameObject other in GameObject.FindGameObjectsWithTag("Boid"))
        {
            if (other == gameObject) continue;

            Vector3 offset = other.transform.position - transform.position;
            float distance = offset.magnitude;

            if (distance < visionRadius)
            {
                float angleBetween = Vector3.Angle(boid.moveDirection, offset);
                if (angleBetween < visionAngle / 2f && distance > 0f)
                {
                    totalForce -= offset.normalized * maxForce;
                    neighborCount++;

                    if (this == selectedBoid)
                        repelledNeighbors.Add(other.transform);
                }
            }
        }

        if (neighborCount > 0)
        {
            totalForce /= neighborCount;
            boid.moveDirection += totalForce * Time.deltaTime;
            boid.moveDirection = boid.moveDirection.normalized;
        }
    }

    void OnMouseDown()
    {
        if (selectedBoid == this)
            selectedBoid = null;
        else
            selectedBoid = this;
    }

    void OnDrawGizmos()
    {
        if (selectedBoid != this) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);

        foreach (Transform neighbor in repelledNeighbors)
        {
            Gizmos.DrawLine(transform.position, neighbor.position);
        }
    }
}