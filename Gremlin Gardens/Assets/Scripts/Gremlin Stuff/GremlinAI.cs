using UnityEngine.AI;
using UnityEngine;

public class GremlinAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet; 
    public float walkPointRange; 

    private void Update(){
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
            walkPointSet = true;
        }

    }


    private void Patrolling(){
        if (!walkPointSet){
            SearchWalkPoint();
        }
        if (walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // WALKPOINT REACHED 
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }
}
