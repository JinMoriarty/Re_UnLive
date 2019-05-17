using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{

    public float movePower = 4f;
    public float jumpPower = 4f;

    Rigidbody2D rigid;
    Animator animator;
    new Renderer renderer;

    Vector3 movement;
    bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0) //왼쪽
        {
            moveVelocity = Vector3.left;
            animator.SetInteger("Direction", -1);
            animator.SetBool("isMoving", true);

            transform.localScale = new Vector3(-6, 6, 6);
        }
        else if(Input.GetAxisRaw("Horizontal") > 0) //오른쪽
        {
            moveVelocity = Vector3.right;
            animator.SetInteger("Direction", 1);
            animator.SetBool("isMoving", true);

            transform.localScale = new Vector3(6, 6, 6);
        }
        else if(Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
            animator.SetTrigger("doJumping");
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Jump();
    }

    void Jump()
    {
        if (!isJumping)
            return;

        rigid.velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attach : " + other.gameObject.layer);

        if (other.gameObject.layer == 8 && rigid.velocity.y < 0)
            animator.SetBool("isJumping", false);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Detach : " + other.gameObject.layer);
    }
}
