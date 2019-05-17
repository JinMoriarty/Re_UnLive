using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;
    public bool slide = false;
    public int maxHealth = 3;

    float horizontalMove = 0f;
    Rigidbody2D rigid;

    public float runSpeed = 80f;
    bool isDie = false;
    bool jump = false;
    bool crouch = false;
    bool attack = false;

    int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //움직임 구현
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        //공격 구현
        if (Input.GetMouseButtonDown(0))
        {
            attack = true;
            animator.SetBool("IsAttack", true);
        }else if (Input.GetMouseButtonUp(0))
        {           
            animator.SetBool("IsAttack", false); 
        }

        //슬라이드 구현
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slide = true;

            if(slide && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
            {
                animator.SetBool("IsRoll", true);
            }else if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("IsRoll"))
            {
                animator.SetBool("IsRoll", false);
            }
        }

        //플레이어 죽음 구현
        if(health == 0)
        {
            if (!isDie)
                Die();
            return;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        if (health == 0)
            return;
    }

    public void Die()
    {
        isDie = true;

        rigid.velocity = Vector2.zero;

        animator.Play("Player_Die");

        BoxCollider2D[] colls = gameObject.GetComponents<BoxCollider2D>();
        colls[0].enabled = false;
        colls[1].enabled = false;

        Vector2 dieVelocity = new Vector2(0, 10f);
        rigid.AddForce(dieVelocity, ForceMode2D.Impulse);

        //Invoke("RestartStage", 2f);

    }

    /*public static void RestartStage()
    {
        Time.timeScale = 0f;

        SceneManager.LoadScene(GameScene, LoadSceneMode.Single);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
