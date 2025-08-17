using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 
public class BasicEnemyAI : MonoBehaviour
{
 
    Animator animator;
 
    public float moveSpeed = 0.2f;
 
    Vector3 stopPosition;
 
    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    public bool isChasing;
    public float detectionRadius = 20f;
    public Transform player;
 
    int WalkDirection;
 
    public bool isWalking;

    public float checkInterval = 2f; // seconds between damage checks
    public float damageRange = 5f; // damages player within this range
 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckProximityRoutine());

        animator = GetComponent<Animator>();
 
        //So that all the prefabs don't move/stop at the same time
        walkTime = Random.Range(3,6);
        waitTime = Random.Range(5,7);
 
 
        waitCounter = waitTime;
        walkCounter = walkTime;
 
        ChooseDirection();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
 
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        isChasing = (dist <= detectionRadius);

        if (isChasing)
        {
            moveSpeed = 0.3f;
            animator.SetBool("isRunning", true);

            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            if (dir.sqrMagnitude > 0f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }
        }

        else if (isWalking)
        {
            moveSpeed = 0.2f;
 
            animator.SetBool("isRunning", true);
 
            walkCounter -= Time.deltaTime;
 
            switch (WalkDirection)
            {
                case  0:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case  1:
                    transform.localRotation = Quaternion.Euler(0f, 90, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case  2:
                    transform.localRotation = Quaternion.Euler(0f, -90, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case  3:
                    transform.localRotation = Quaternion.Euler(0f, 180, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }
 
            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;
                //stop movement
                transform.position = stopPosition;
                animator.SetBool("isRunning", false);
                //reset the waitCounter
                waitCounter = waitTime;
            }
 
 
        }
        else
        {
 
            waitCounter -= Time.deltaTime;
 
            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }
 
 
    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);
 
        isWalking = true;
        walkCounter = walkTime;
    }

    IEnumerator CheckProximityRoutine()
    {
        while (true)
        {
            CheckProximity();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void CheckProximity()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= damageRange)
        {
            PlayerState.Instance.currentHealth -= 20f;
        }
    }
}