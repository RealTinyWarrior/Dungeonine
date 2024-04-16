using UnityEngine;

public class LaserCast : MonoBehaviour
{
    public string target;
    public string obsticle;
    public Transform laser;
    public Transform rayPoint;
    public float rayPointOffset;
    public float raycastDistance;
    public Vector2 origin;
    public Vector2 destination;
    public Vector2 direction;
    bool isX;
    Vector2 gizmos;

    void Start()
    {
        if (direction.x == 0) isX = false;
        else isX = true;
    }

    void Update()
    {
        RaycastHit2D[] ray = Physics2D.RaycastAll(origin, direction, raycastDistance);
        bool hasHitObsticle = false;
        float obsticleLength = 0f;
        bool hasHit = false;

        foreach (RaycastHit2D rayHit in ray)
        {
            if (hasHitObsticle) break;

            if (rayHit.collider.CompareTag(target) || rayHit.collider.CompareTag(obsticle))
            {
                Vector2 center = (origin + rayHit.point) / 2;
                gizmos = rayHit.point;

                if (isX)
                {
                    float lengthX = Mathf.Abs(rayHit.point.x - origin.x);
                    laser.localScale = new Vector2(lengthX, laser.localScale.y);
                    rayPoint.position = new Vector2(rayHit.point.x + rayPointOffset, rayPoint.position.y);

                    laser.position = center;
                }

                else
                {
                    float lengthY = Mathf.Abs(rayHit.point.y - origin.y);
                    laser.localScale = new Vector2(laser.localScale.x, lengthY);
                    rayPoint.position = new Vector2(rayPoint.position.x, rayHit.point.y + rayPointOffset);

                    laser.position = center;
                }

                hasHit = true;
            }

            if (rayHit.collider.CompareTag(obsticle))
            {
                hasHitObsticle = true;
                obsticleLength = rayHit.distance;
            }
        }

        foreach (RaycastHit2D rayHit in ray)
        {
            if (!hasHitObsticle) return;
            if (rayHit.collider.CompareTag(target))
            {
                if (rayHit.distance < obsticleLength)
                {
                    Vector2 center = (origin + rayHit.point) / 2;
                    gizmos = rayHit.point;

                    if (isX)
                    {
                        float lengthX = Mathf.Abs(rayHit.point.x - origin.x);
                        laser.localScale = new Vector2(lengthX, laser.localScale.y);
                        rayPoint.position = new Vector2(rayHit.point.x + rayPointOffset, rayPoint.position.y);

                        laser.position = center;
                    }

                    else
                    {
                        float lengthY = Mathf.Abs(rayHit.point.y - origin.y);
                        laser.localScale = new Vector2(laser.localScale.x, lengthY);
                        rayPoint.position = new Vector2(rayPoint.position.x, rayHit.point.y + rayPointOffset);

                        laser.position = center;
                    }

                    hasHit = true;
                }
            }
        }

        if (!hasHit)
        {
            Vector2 center = (origin + destination) / 2;

            if (isX)
            {
                float lengthX = Mathf.Abs(destination.x - origin.x);
                laser.localScale = new Vector2(lengthX, laser.localScale.y);
                rayPoint.position = new Vector2(destination.x + rayPointOffset, rayPoint.position.y);

                laser.position = center;
            }

            else
            {
                float lengthY = Mathf.Abs(destination.y - origin.y);
                laser.localScale = new Vector2(laser.localScale.x, lengthY);
                rayPoint.position = new Vector2(rayPoint.position.x, destination.y + rayPointOffset);

                laser.position = center;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, gizmos);
    }
}
