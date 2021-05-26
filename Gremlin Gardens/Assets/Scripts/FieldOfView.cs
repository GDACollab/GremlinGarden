using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructMask;
    private RaycastHit hit;

    public bool seeFruit;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FieldView();
    }

    // fruits will require colliders
    // coroutine?
    private void FieldView()
    {
        Collider[] collides = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (collides.Length != 0)
        {
            target = collides[0].transform;
            Vector3 directionTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionTarget) < angle / 2)
            {
                float distanceTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionTarget, distanceTarget, obstructMask))
                {
                    //Debug.Log(target);
                    seeFruit = true;
                    //Debug.Log("Can see stuff");
                }
                else
                {
                    seeFruit = false;
                }
            }
            else
            {
                seeFruit = false;
            }
        }
        else if (seeFruit)
        {
            seeFruit = false;
        }
    }
}
