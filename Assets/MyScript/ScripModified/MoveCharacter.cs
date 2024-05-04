using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

namespace Character
{
    public class MoveCharacter : MonoBehaviour
    {
        protected GameControl gameManagerRef;
        protected CharacterController _charController;
        protected CharacterStatus status;
        protected Animator charAn;
        protected AudioSource playerAudio;
        public AudioSource pickupChicken;
        public AudioSource pickupApple;
        private ObjectManager om;
        public ParticleSystem dirtParticle;

        private float speed;
        private float gravity;
        private float runBoost;
        private float jumpForce;
        private float damageTimeout;
        private float vertSpeed = 0.0f;
        protected float rotationSensitvity;
        protected GameObject spawnPoint;

        Vector2 input;
        Transform cam;
        float angle;
        Vector3 movement;
        Quaternion targetRotation;
        private void Awake()
        {
            gameManagerRef = GameControl.GetInstance();
        }
        private void Start()
        {
            cam = GameObject.Find("Main Camera").transform;
            _charController = GetComponent<CharacterController>();
            status = GetComponent<CharacterStatus>();
            charAn = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            om = ObjectManager.Instance();
            spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
            float correctHeight = _charController.center.y + _charController.skinWidth;
            _charController.center = new Vector3(0, correctHeight, 0);

            TextAsset data = Resources.Load<TextAsset>("File/characterFeatures");
            string[] lines = data.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] token = line.Split('=');

                switch (token[0])
                {
                    case "speed":
                        speed = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "gravity":
                        gravity = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "runBoost":
                        runBoost = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "jumpForce":
                        jumpForce = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "rotationSensitivity":
                        rotationSensitvity = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "damageTimeout":
                        damageTimeout = float.Parse(token[1], CultureInfo.InvariantCulture);
                        Debug.Log("damageTimeout: " + damageTimeout);
                        break;
                    default:

                        break;
                }
            }

        }
        private void Update()
        {
            _GameState gs = gameManagerRef.GetGameState();
            if (gs == _GameState.Over || gs == _GameState.Won) return;
            if (om.CurrentHealtBar > 0)
            {
                if (!status.IsGrounded)
                {
                    vertSpeed += gravity * Time.deltaTime;
                }
                else
                {
                    vertSpeed = 0;
                    if (status.IsJumping)
                    {
                        vertSpeed += jumpForce;
                        charAn.SetTrigger("Fox_Jump");
                    }
                }
                movement.y += vertSpeed * Time.deltaTime;
                _charController.Move(transform.TransformDirection(new Vector3(0, vertSpeed * Time.deltaTime, 0)));
                if (!status.IsMoving && !status.IsRotating)
                    {
                        charAn.SetBool("isRunning", false);
                    }
                    else if(!status.IsPowerUp)
                    {
                        GetInput();
                        CalculateDirection();
                        Move();
                        Rotate();
                    }else{
                        GetInput();
                        CalculateDirection();
                        Run();
                        Rotate();
                    } 
            }else { charAn.SetBool("Death", true);
              FindObjectOfType<PlayerAttack>().enabled = false; }
         }
        void GetInput()
        {
            input.x = status.Rotation;
            input.y = status.Movement;
        }

        void CalculateDirection()
        {
            angle = Mathf.Atan2(input.x, input.y);
            angle = Mathf.Rad2Deg * angle;
            angle += cam.eulerAngles.y;


        }

        void Rotate()
        {
            targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSensitvity * Time.deltaTime);
        }
        private void Move()
        {
                movement = transform.forward * speed * Time.deltaTime;
                _charController.Move(movement);
        }
        private void Run()
        {
                 movement = transform.forward * speed * runBoost * Time.deltaTime;
                _charController.Move(movement);
        }
        private void OnAnimatorMove()
        {
            if (status.IsGrounded && !status.IsHitObstacle)
            {
                charAn.SetBool("isRunning", true);
            }
            else if (!status.IsGrounded || status.IsHitObstacle)
            {
                charAn.SetBool("isRunning", false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Chicken")
            {
                if (other.gameObject != null)
                {
                    CollectableItemsController itemsC = other.gameObject.GetComponent<CollectableItemsController>();
                    Dictionary<string, int> scoreDic = itemsC.ScoreValueDic;
                    Messenger<int>.Broadcast(GameEvent.HANDLE_COLLECT_CHICKEN, scoreDic["scoreValue"]);
                    pickupChicken.Play();
                    Destroy(other.gameObject);
                }
            }
            else if (other.tag == "Apple")
            {
                    CollectableItemsController itemsC = other.gameObject.GetComponent<CollectableItemsController>();
                    Dictionary<string, int> scoreDic = itemsC.ScoreValueDic;
                    Messenger<int>.Broadcast(GameEvent.HANDLE_COLLECT_APPLE, scoreDic["scoreValue"]);
                    pickupApple.Play();
                    Destroy(other.gameObject);
            }
        }
    }
}
