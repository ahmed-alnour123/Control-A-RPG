using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_AnimalAIEnemy : MonoBehaviour {

    public enum AIState { Idle, Walking, Howl, Running, Attacking }
    public AIState currentState = AIState.Idle;
    public int awarenessArea = 15; //How far the deer should detect the enemy
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 7f;
    public float attackSpeed = 0.5f;
    [SerializeField] float attackDistance;
    [SerializeField] float playerAttckDistance = 1.25f;
    [SerializeField] float raycastHight;

    public Animator animator;

    //Trigger collider that represents the awareness area
    SphereCollider c;
    //NavMesh Agent
    NavMeshAgent agent;
    bool switchAction = false;
    float actionTimer = 0; //Timer duration till the next action
    public Collider enemy;

    //Detect NavMesh edges to detect whether the AI is stuck
    //How long the AI has been near the edge of NavMesh, if too long, send it to one of the random previousIdlePoints
    //Store previous idle points for reference
    List<Vector3> previousIdlePoints = new List<Vector3>();
    float stoppingDistance;
    [SerializeField] float attackPoints;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;

        c = gameObject.AddComponent<SphereCollider>();
        c.isTrigger = true;
        c.radius = awarenessArea;


        //Initialize the AI state
        currentState = AIState.Idle;
        actionTimer = Random.Range(0.1f, 2.0f);
        SwitchAnimationState(currentState);

    }

    // Update is called once per frame
    void Update() {
        Debug.DrawRay(transform.position + Vector3.up * raycastHight, transform.TransformDirection(Vector3.forward) * 20, Color.yellow);

        if (enemy) {
            if (enemy.CompareTag("Player")) {
                stoppingDistance = playerAttckDistance - 0.1f;

                agent.stoppingDistance = stoppingDistance;

            } else if (enemy.CompareTag("Companion")) {
                stoppingDistance = attackDistance - .1f;

                agent.stoppingDistance = stoppingDistance;

            }
        }
        if (currentState != AIState.Attacking && enemy) {
            agent.SetDestination(enemy.transform.position);
        }

        //Wait for the next course of action
        if (actionTimer > 0) {
            actionTimer -= Time.deltaTime;
        } else {
            switchAction = true;
        }

        if (currentState == AIState.Idle) {
            if (switchAction) {
                if (enemy) {
                    //Run Towards enemy
                    agent.SetDestination(enemy.transform.position);
                    currentState = AIState.Running;
                    SwitchAnimationState(currentState);
                    if (Vector3.Distance(transform.position, enemy.transform.position) < attackDistance) {
                        var direction = (Vector3.ProjectOnPlane(enemy.transform.position, Vector3.up) - Vector3.ProjectOnPlane(transform.position, Vector3.up)).normalized;

                        transform.forward = Vector3.RotateTowards(transform.forward, direction, .02f, 1);
                        if (Vector3.Distance(transform.forward, direction) < 0.2f)
                            StartCoroutine(Attack(direction));
                    }
                } else {
                    //No enemies nearby, start eating
                    actionTimer = Random.Range(24, 30);

                    currentState = AIState.Howl;
                    SwitchAnimationState(currentState);

                    //Keep last 5 Idle positions for future reference
                    previousIdlePoints.Add(transform.position);
                    if (previousIdlePoints.Count > 5) {
                        previousIdlePoints.RemoveAt(0);
                    }
                }
            }
        } else if (currentState == AIState.Walking) {
            //Set NavMesh Agent Speed
            agent.speed = walkingSpeed;

            // Check if we've reached the destination
            if (DoneReachingDestination()) {
                currentState = AIState.Idle;
            }
        } else if (currentState == AIState.Howl) {
            if (switchAction) {
                //Wait for current animation to finish playing
                if (!animator || animator.GetCurrentAnimatorStateInfo(0).normalizedTime - Mathf.Floor(animator.GetCurrentAnimatorStateInfo(0).normalizedTime) > 0.99f) {
                    //Walk to another random destination
                    agent.destination = RandomNavSphere(transform.position, Random.Range(7, 10));
                    currentState = AIState.Walking;
                    SwitchAnimationState(currentState);
                }
            }
        } else if (currentState == AIState.Running) {
            //Set NavMesh Agent Speed
            agent.speed = runningSpeed;

            if (Vector3.Distance(transform.position, enemy.transform.position) < attackDistance) {
                var direction = (Vector3.ProjectOnPlane(enemy.transform.position, Vector3.up) - Vector3.ProjectOnPlane(transform.position, Vector3.up)).normalized;

                transform.forward = Vector3.RotateTowards(transform.forward, direction, .04f, 1);
                if (Vector3.Distance(transform.forward, direction) < .05f)
                    StartCoroutine(Attack(direction));
            }

        }

        switchAction = false;
    }

    bool DoneReachingDestination() {
        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    //Done reaching the Destination
                    return true;
                }
            }
        }

        return false;
    }

    void SwitchAnimationState(AIState state) {
        animator.SetBool("Howl", state == AIState.Howl);
        animator.SetBool("Walking", state == AIState.Walking);
        animator.SetBool("Running", state == AIState.Running);
        animator.SetBool("Attacking", state == AIState.Attacking);


        //Animation control
        // if (animator != null)
        // {
        //     if (animator)
        //     {
        //         if (state == AIState.Howl)
        //         {
        //             animator.SetFloat("Speed", 0);
        //         }
        //         if (state == AIState.Idle)
        //         {
        //             animator.SetFloat("Speed", 0);
        //         }
        //         if (state == AIState.Running)
        //         {
        //             animator.SetFloat("Speed", 1f);
        //         }
        //         if (state == AIState.Walking)
        //         {
        //             animator.SetFloat("Speed", .5f);
        //         }

        //     }
        // }

    }

    Vector3 RandomNavSphere(Vector3 origin, float distance) {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

        return navHit.position;
    }

    IEnumerator Attack(Vector3 direction) {
        agent.SetDestination(transform.position);
        currentState = AIState.Attacking;
        SwitchAnimationState(AIState.Attacking);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 20, Color.yellow);
        if (Physics.Raycast(transform.position + Vector3.up * raycastHight, transform.TransformDirection(Vector3.forward), out hit, 200f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Companion")) {
                yield return new WaitForSeconds(attackSpeed / 2);
                // hit.collider.GetComponentInChildren<HealthSystem>().DamageHP(attackPoints);
            }
        }
        yield return new WaitForSeconds(attackSpeed / 2);


        currentState = AIState.Idle;
        SwitchAnimationState(AIState.Idle);
    }
    public void Die() {
        Destroy(this.gameObject);
    }
    public void TakeDamage() {
        StopCoroutine(DamageEffect());
    }
    IEnumerator DamageEffect() {
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;

        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.white;

    }
    void OnTriggerEnter(Collider other) {
        if (enemy) return;
        //Make sure the Player instance has a tag "Player"
        if ((other.CompareTag("Player") || other.CompareTag("Companion")) && !other.isTrigger) {
            enemy = other;

            actionTimer = Random.Range(0.24f, 0.8f);
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other == enemy)
            enemy = null;
    }
}
