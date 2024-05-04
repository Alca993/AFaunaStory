using UnityEngine;
namespace InputControllers
{
    public class BaseInputController
    {
        protected bool _Up;
        protected bool _Down;
        protected bool _Left;
        protected bool _Right;

        protected bool _Jump;
        protected bool _Attack;

        protected float vert;
        protected float horz;

        protected Vector3 TEMPVec3;
        protected Vector3 zeroVector = new Vector3(0, 0, 0);

        public bool Down
        {
            get { return _Down; }
        }
        public bool Up
        {
            get { return _Up; }
        }
        public bool Left
        {
            get { return _Left; }
        }
        public bool Right
        {
            get { return _Right; }
        }
        public virtual bool GetJump()
        {
            return _Jump; 
        }
        public virtual void CheckInput()
        {
            horz = Input.GetAxis("Horizontal");
            vert = Input.GetAxis("Vertical");
        }
        public virtual float GetHorizontal()
        {
            return horz;
        }
        public virtual float GetVertical()
        {
            return vert;
        }
        public virtual bool GetAttack()
        {
            return _Attack;
        }
        public virtual Vector3 GetMovementDirectionVector()
        {
            if(Left || Right)
            {
                TEMPVec3.x = horz;
            }
            if (Up || Down)
            {
                TEMPVec3.z = vert;
            }
            return TEMPVec3;
        }
    }
}
