using UnityEngine;

public class GremlinAI : MonoBehaviour
{
    public float duration;    //the max time of a walking session (set to ten)
    [Range(0, 2)]
    public float speed = 1;
    private float elapsedTime = 0f; //time since started walk
    private float wait = 0f; //wait this much time
    private float waitTime = 0f; //waited this much time

    public Vector3 movementDirection;

    [HideInInspector] public bool move = true; //start moving

    void Start()
    {
        movementDirection = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        duration = Random.Range(1f, 5f);
    }

    void Update()
    {
        Wander();
    }

    void Wander()
    {
        if (elapsedTime < duration && move)
        {
            //move in given direction for duration
            transform.Translate(movementDirection * Time.deltaTime * speed, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * 10f);
            elapsedTime += Time.deltaTime;
        }
        else if (elapsedTime >= duration)
        {
            //stop moving and wait
            elapsedTime = 0f;
            move = false;
            wait = Random.Range(0f, 7f);
            waitTime = 0f;
        }

        if (waitTime < wait && !move)
            //wait timer
            waitTime += Time.deltaTime;
        else if (!move)
        {
            //done waiting. Move to these random directions
            move = true;
            duration = Random.Range(2f, 5f);
            movementDirection.x = Random.Range(-3f, 3f);
            movementDirection.z = Random.Range(-3f, 3f);
        }
    }

    void OnCollisionEnter(Collision other)
    {

        while (Physics.Raycast(transform.position, movementDirection, 2f))
        {
            movementDirection.x = Random.Range(-3f, 3f);
            movementDirection.z = Random.Range(-3f, 3f);
        }
    }
}
