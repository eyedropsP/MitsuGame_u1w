using System;
using System.Linq;
using System.Net.Sockets;
using LineTrace;
using LineTrace.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slime
{
    public class PlayerMover : MonoBehaviour
    {
        public Vector3 PlayerDirection { get; private set; }
        [SerializeField]private DirectionController2d Controller = default;
       
        [Header("移動スピード")][SerializeField] private float speed = 6.0f;
        [Header("速度")] [SerializeField] private Vector3 velocity = default;
        [Header("ジャンプ高さ")] [SerializeField] private float jumpHeight = 4.0f;
        [SerializeField] private LayerMask platformLayerMask = default;
        [SerializeField] private GameObject bodyObject = default;
        [SerializeField] private Collider groundCollider = default;
        private readonly float playerWidth = 0.15f;
        private float gravity;
        private float jumpVelocity;
        private float timeToJumpApex = .4f;
        private float accelerationTimeAirborne = .2f;
        private float accelerationTimeGrounded = .1f;
        private Vector3 inputHorizontal;
        private BoolReactiveProperty isOnGround = new BoolReactiveProperty();
        private LayerMask ignoreLayer = ~(1 << 9 | 1 << 11);
        // Start is called before the first frame update
        private void Start()
        {
            PlayerDirection = transform.forward;
            var input = GetComponent<IInputEventProvider>();
            
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            // 左
            input.MoveHorizontal
                .Where(x => x < 0)
                .Subscribe(x =>
                {
                    Controller.direction = Direction.back;
                    PlayerDirection = Controller.forward;
                    inputHorizontal = Controller.forward * Mathf.Abs(x);
                })
                .AddTo(this);
            // 右
            input.MoveHorizontal
                .Where(x => x > 0)
                .Subscribe(x =>
                {
                    Controller.direction = Direction.front;
                    PlayerDirection = Controller.forward;
                    inputHorizontal = Controller.forward * Mathf.Abs(x);
                })
                .AddTo(this);
            // ジャンプ
            input.JumpButton
                .Where(x => x && isOnGround.Value)
                .Subscribe(_ =>
                {
                    isOnGround.Value = false;
                    velocity.y = jumpVelocity;
                })
                .AddTo(this);
            
            // 移動
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    var horizontalMoveSpeed = inputHorizontal * speed;
                    velocity.x = horizontalMoveSpeed.x;
                    velocity.z = horizontalMoveSpeed.z;
                    if (isOnGround.Value)
                        velocity.y = 0;
                    velocity.y += gravity * Time.deltaTime;
                    Move();
                }).AddTo(this);

            groundCollider.OnTriggerStayAsObservable()
                .Where(x => x != null)
                .Subscribe(_ => isOnGround.Value = true)
                .AddTo(this);

            groundCollider.OnTriggerExitAsObservable()
                .Where(x => x != null)
                .Subscribe(_ => isOnGround.Value = false)
                .AddTo(this);
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            var tmpVelocity = velocity * Time.deltaTime;
            transform.position += tmpVelocity;
        }
    }
}