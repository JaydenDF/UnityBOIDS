using UnityEngine;

public class Alignment3D : MonoBehaviour
{
    public float visionRadius = 2.5f;
    public float visionAngle = 120f;
    public float alignmentStrength = 1.0f;

    private BoidWrap boid;

    void Start()
    {
        boid = GetComponent<BoidWrap>();
    }

    void Update()
    {
        Vector3 averageDirection = Vector3.zero;
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
                    BoidWrap otherBoid = other.GetComponent<BoidWrap>();
                    if (otherBoid != null)
                    {
                        averageDirection += otherBoid.moveDirection;
                        neighborCount++;
                    }
                }
            }
        }

        if (neighborCount > 0)
        {
            averageDirection /= neighborCount;
            boid.moveDirection += averageDirection.normalized * alignmentStrength * Time.deltaTime;
            boid.moveDirection = boid.moveDirection.normalized;
        }
    }
}