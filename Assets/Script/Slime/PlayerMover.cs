using LineTrace;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Slime
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private DirectionController2d Controller;
        [SerializeField] private float jumpForce = 8.0f;
	    [SerializeField] private float speed = 6.0f;
        
        private BoolReactiveProperty isOnGround = new BoolReactiveProperty(); 
        private Rigidbody playerRb;
        
        // Start is called before the first frame update
        void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            var input = GetComponent<IInputEventProvider>();
            // 左
            input.MoveHorizontal
                .Where(x => x < 0)
                .Subscribe(x =>
                {
                    Controller.direction = Direction.back;
                    transform.position += Controller.forward * (speed * Mathf.Abs(x) * Time.deltaTime);
                });
            // 右
            input.MoveHorizontal
                .Where(x => x > 0)
                .Subscribe(x =>
                {
                    Controller.direction = Direction.front;
                    transform.position += Controller.forward * (speed * Mathf.Abs(x) * Time.deltaTime);
                });
            // ジャンプ
            input.JumpButton
                .Where(x => x && isOnGround.Value)
                .Subscribe(_ =>
                {
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    isOnGround.Value = false;
                });
            // 着地
            this.OnCollisionEnterAsObservable()
                .Where(x => x.gameObject.CompareTag("Ground"))
                .Subscribe(_ => isOnGround.Value = true);
        }
    }
}