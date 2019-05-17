using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour{
    public float movePower = 1f;
    public int creatureType;
    bool isTracing = false;
    GameObject traceTarget;

    Animator animator;
    Vector3 movement;
    int movementFlag = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isTracing)
        {
            Vector3 PlayerPos = traceTarget.transform.position;

            if (PlayerPos.x < transform.position.x)
                dist = "Left";
            else if (PlayerPos.x > transform.position.x)
                dist = "Right";
        }
        else
        {
            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";
        }

        if(dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(7, 7, 7);
        }
        else if(dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-7, 7, 7);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (creatureType == 0)
            return;
        if (other.gameObject.tag == "Player") { 
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (creatureType == 0)
            return;
        if (other.gameObject.tag == "Player"){
            isTracing = true;
            animator.SetBool("isMoving", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (creatureType == 0)
            return;
        if (other.gameObject.tag == "Player"){
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }
}
