using UnityEngine;

namespace SpaceShooter.Player
{
    public class PlayerInput
    {
        public float MoveInput { get; private set; }
        public int RotateInput { get; private set; }
        public bool ShootBulletPressed { get; private set; }
        public bool ShootLazerPressed { get; private set; }

        public void UpdateInput()
        {
            MoveInput = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f);
            RotateInput = -(int)Input.GetAxisRaw("Horizontal");
            ShootBulletPressed = Input.GetMouseButtonDown(0);
            ShootLazerPressed = Input.GetMouseButtonDown(1);
        }
    }
}
