using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Globalization;

namespace Character
{
    public class CharacterStatus : MonoBehaviour
    {
        protected GameControl gameManagerRef;
        protected bool isMoving;
        protected bool isRotating;
        protected bool isAttacking;
        protected bool isJumping;
        protected bool isGrounded;
        protected bool isWater;
        protected bool isTrap;
        protected bool isHitObstacle;
        protected bool isGameOver;
        protected bool isLevelCompleted;
        protected bool isDeath;
        protected bool isPowerUp;

        protected float movement;
        protected float rotation;
        
        private float minimumDistance;
        private float checkDistance;
        private float checkObstacleDistance;
       
        public bool IsPowerUp
        {
            get { return isPowerUp;  }
        }
        public bool IsDeath
        {
            get { return isDeath; }
        }
        public bool IsLevelCompleted
        {
            get { return isLevelCompleted; }
        }
        public bool IsGameOver
        {
            get
            {
                return isGameOver;
            }
        }
        public bool IsJumping
        {
            get { return isJumping; }
        }
        public bool IsMoving
        {
            get { return isMoving; }
        }
        public bool IsRotating
        {
            get { return isRotating; }
        }

        public bool IsAttacking
        {
            get { return isAttacking; }
        }
        public bool IsGrounded
        {
            get { return isGrounded; }
        }
        public bool IsHitObstacle
        {
            get { return isHitObstacle; }
        }
        public bool IsWater
        {
            get { return isWater; }
        }
        public bool IsTrap
        {
            get { return isTrap; }
        }
        public float Movement
        {
            get { return movement; }
        }
        public float Rotation
        {
            get { return rotation; }
        }
        private void Awake()
        {
            gameManagerRef = GameControl.GetInstance();
            Messenger.AddListener(GameEvent.HANDLE_POWERUP_ON, OnPowerUpOn);
            Messenger.AddListener(GameEvent.HANDLE_POWERUP_OFF, OnPowerUpOff);

            TextAsset data = Resources.Load<TextAsset>("File/characterState");
            string[] lines = data.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] token = line.Split('=');

                switch (token[0])
                {
                    case "minimumDistance":
                        minimumDistance = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "checkDistance":
                        checkDistance = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "checkObstacleDistance":
                        checkObstacleDistance = float.Parse(token[1], CultureInfo.InvariantCulture);
                        Debug.Log("damageTimeout: " + checkObstacleDistance);
                        break;
                    default:

                        break;
                }
            }
        }
        private void OnDestroy()
        {
            Messenger.RemoveListener( GameEvent.HANDLE_POWERUP_ON, OnPowerUpOn);
            Messenger.RemoveListener( GameEvent.HANDLE_POWERUP_OFF, OnPowerUpOff);
        }
        private void Update()
        {
            checkGrounded();
            CheckObstacleCollision();
        }
        private void OnPowerUpOn()
        {
            isPowerUp = true;
        }
        private void OnPowerUpOff()
        {
            isPowerUp = false;
        }
        protected void checkGrounded()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down), out hit, minimumDistance)&& hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        protected void  CheckObstacleCollision()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, checkObstacleDistance))
            {
                if (hit.collider.gameObject.tag == "obstacle")
                {
                    isHitObstacle = true;
                }
            }else
            {
                isHitObstacle = false;
            }
        }
        public void OnRunning(InputAction.CallbackContext context)
        {
            movement = context.ReadValue<Vector2>().y;
            rotation = context.ReadValue<Vector2>().x;
            if (movement != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            if (rotation != 0)
            {
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            float jump = context.ReadValue<float>();
            isJumping = isGrounded && jump > 0;
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            float attack = context.ReadValue<float>();
            isAttacking = attack > 0;
        }
    }
}