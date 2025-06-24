using UnityEngine;

public class Cohesion : MonoBehaviour
{
    public float visionRadius = 3f;
    public float visionAngle = 120f;
    public float cohesionStrength = 1.0f;

    private BoidWrap boid;

    void Start()
    {
        boid = GetComponent<BoidWrap>();
    }

    void Update()
    {
        Vector3 averagePosition = Vector3.zero;
        int neighborCount = 0;

        foreach (GameObject other in GameObject.FindGameObjectsWithTag("Boid"))
        {
            if (other == gameObject) continue;

            Vector3 offset = other.transform.position - transform.position;
            float distance = offset.magnitude;

            if (distance < visionRadius)
            {
                float angleBetween = Vector3.Angle(boid.moveDirection, offset);
                if (angleBetween < visionAngle / 2f)
                {
                    averagePosition += other.transform.position;
                    neighborCount++;
                }
            }
        }

        if (neighborCount > 0)
        {
            averagePosition /= neighborCount;
            Vector3 directionToCenter = averagePosition - transform.position;

            boid.moveDirection += directionToCenter.normalized * cohesionStrength * Time.deltaTime;
            boid.moveDirection = boid.moveDirection.normalized;
        }
    }
}