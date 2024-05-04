using UnityEngine;
using UnityEngine.InputSystem;

namespace InputControllers
{
    public class KeyboardInput : BaseInputController
    {
         public override void CheckInput()
          {
              horz = Input.GetAxis("Horizontal");
              vert = Input.GetAxis("Vertical");
              _Up = (vert > 0);
              _Down = (vert < 0);
              _Left = (horz < 0);
              _Right = (horz > 0);
              _Attack = Input.GetButtonDown("Fire1");
              _Jump = Input.GetButtonDown("Jump");
          }
    }
}
