using LineTrace;
using UnityEngine;

namespace Slime
{
    public class Move : MonoBehaviour
    {
        public DirectionController2d controller;
        public float jumpForce = 8.0f;
        public float speed = 6.0f;
        private bool isOnGround = true;
        private Rigidbody playerRb;

        private void Start()
        {
            playerRb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                controller.direction = Direction.back;
                transform.position += controller.forward * (speed * Time.deltaTime);
            }
            else if(UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                controller.direction = Direction.front;
                transform.position += controller.forward * (speed * Time.deltaTime);
            }

            if (!UnityEngine.Input.GetButtonDown("Jump") || !isOnGround) return;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag($"Ground"))
                isOnGround = true;
        }
    }
}
