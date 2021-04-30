using UnityEngine.AI;
using UnityEngine;

public class GremlinAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Rigidbody rb;
    public Vector3 walkPoint;
    public bool walkPointSet; 
    public float walkPointRange; 
    public float startTime;
    public Vector3 startPos;

    void Awake(){
        agent = this.GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        
        player = GameObject.Find("Player").transform;
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update(){
        if(this.GetComponent<GremlinInteraction>().beingCarried ||  this.GetComponent<GremlinInteraction>().beingPet)
            walkPointSet = false;
        else
            Patrolling();
    }



    private void SearchWalkPoint(){
        //CALCULATE RANDOM POINT IN RANGE
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        print("THIS IS walkPoint");
        print(walkPoint);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            startTime = Time.time;
            startPos = transform.position;
            walkPointSet = true;
        }

    }


    private void Patrolling(){
        if (!walkPointSet){
            SearchWalkPoint();
        }
        if (walkPointSet){
            transform.position = Vector3.Lerp(startPos, walkPoint, (Time.time - startTime) * 0.5f);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // WALKPOINT REACHED 
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }

    void OnCollisionEnter(Collision collisionInfo){
        if (collisionInfo.gameObject.name == "Shop Building" || collisionInfo.gameObject.name == "House"){
            SearchWalkPoint();
        }
    }



}
