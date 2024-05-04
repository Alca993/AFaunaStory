using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    public CharacterController controller;
    private Animator playerAn;
    private AudioSource playerAudio;
    public AudioClip pickupHeart;
    public Transform cam;
    public float speed = 4.5f;
    private float turnSmoothVelocity=0f;
    private float turnSmoothTime=0.1f;
    private float gravityValue = -9.81f;
    public float jumpHeight;
    public float downForce;
    private Vector3 moveDir;
    private bool groundedPlayer;
    public float gravityModifier = 1.0f;
    public bool hitObstacle;
    public bool gameOver = false;
    public bool isOnGround;
    public Image lifeFill;
    private float life = 1;
    public float damageTimeout = 1.0f;
    private bool canTakeDamage=true;
    public bool inWater = false;
    public bool inHollow = false;
    public Transform spawnPoint;
    public Transform spawnPointHollow;
    
    void Start()
    {
       playerAn = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
      float correctHeight = controller.center.y + controller.skinWidth;
      controller.center = new Vector3(0, correctHeight,0);
    }
    
    void Update()
    {
        if (!gameOver && !inWater && !inHollow)
        {

            PlayerMovement();
            PlayerJump();
            CheckObstacleCollision();
            CheckGround();
            CheckWater();
            CheckHollow();
        }else if(gameOver==true){
          //  FindObjectOfType<PlayerAttack>().enabled = false;
           // FindObjectOfType<GameManager>().EndGame();
        }
    }
   
    void PlayerMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && moveDir.y < 0)
        {
            moveDir.y = 0;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            playerAn.SetBool("isRunning", true);
        }
        else
        {
            playerAn.SetBool("isRunning", false);
        }
    }
    
    void PlayerJump() {
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            moveDir.y += Mathf.Sqrt(gravityValue * jumpHeight * -3.0f);
            playerAn.SetTrigger("Fox_Jump");
        }else if(!groundedPlayer)
        {
            moveDir.y -= downForce * Time.deltaTime;
        }
        moveDir.y += gravityValue * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }
    
    void CheckObstacleCollision()
        {
             RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.3f))
            {
                if (hit.collider.gameObject.tag == "obstacle")
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                    Debug.Log("Did Hit");
                    hitObstacle = true;
                    playerAn.SetBool("isRunning", false);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.green);
                Debug.Log("Did not Hit");
                hitObstacle = false;
            }
        }
    
    void CheckGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.5f))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
                   Debug.Log("Did Hit Ground");
                    isOnGround = true;
                  //  playerAn.SetBool("isRunning", true);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.green);
               Debug.Log("Did not Hit Ground");
                isOnGround = false;
                playerAn.SetBool("isRunning", false);
            }
        }
    
    void CheckWater()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.1f))
        {
            if (hit.collider.gameObject.tag == "Water")
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
                inWater = true;
                removeLife();
                if (life > 0)
                {
                    StartCoroutine(Respawn());
                    gameObject.transform.position = spawnPoint.transform.position;
                }
                else { removeLife(); }
            }}else{
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.green);
            inWater = false;
        }
    }
    
    void CheckHollow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.1f))
        {
            if (hit.collider.gameObject.tag == "Hollow")
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
                inHollow = true;
                removeLife();
                if (life > 0)
                {
                    StartCoroutine(RespawnHollow());
                    gameObject.transform.position = spawnPointHollow.transform.position;
                }else { removeLife(); }
            }
        }else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.green);
            inHollow = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
        {
             if(other.CompareTag("heart") && life!=1)
                {
                addLife();
                Destroy(other.gameObject);
                playerAudio.PlayOneShot(pickupHeart);
                }
        }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && canTakeDamage)
        {
            enemyAttack();
            if (life > 0)
            {
                StartCoroutine(damageTimer());
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && canTakeDamage)
        {
            enemyAttack();
            if (life > 0)
            {
                StartCoroutine(damageTimer());
            }
        }
    }
    
    private IEnumerator Respawn()
    {
        GetComponent<Animator>().enabled = false;
        inWater = true;
        yield return new WaitForSeconds(0.2f);
        inWater = false;
        GetComponent<Animator>().enabled = true;
    }
    
    private IEnumerator RespawnHollow()
    {
        GetComponent<Animator>().enabled = false;
        inHollow = true;
        yield return new WaitForSeconds(0.2f);
        inHollow = false;
        GetComponent<Animator>().enabled = true;
    }
    
    private IEnumerator damageTimer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageTimeout);
        canTakeDamage = true;
    }
    
    void addLife()
    {
        if(life<1)
        {
            life += 0.25f;
            lifeFill.fillAmount = life;
        }
    }
    
    void enemyAttack()
    {
        if (life > 0)
        {
            life -= 0.25f;
            lifeFill.fillAmount = life;
        }
        else if (life<=0){
            gameOver = true;
            playerAn.SetBool("Death", true);
            controller.enabled = false;
        }
    }
    
    void removeLife()
    {
        if (life > 0)
        {
            life -= 0.5f;
            lifeFill.fillAmount = life;
        }
        else if (life <= 0)
        {
            gameOver = true;
            playerAn.SetBool("Death", true);
            controller.enabled = false;
        }
    }
} 

