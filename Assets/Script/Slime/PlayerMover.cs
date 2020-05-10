using System;
using LineTrace;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Slime
{
    public class PlayerMover : MonoBehaviour
    {
        public Vector3 PlayerDirection { get; private set; }
        [SerializeField]private DirectionController2d Controller = default;
        [Header("ジャンプ力")][SerializeField] private float jumpForce = 8.0f;
        [Header("移動スピード")][SerializeField] private float speed = 6.0f;
        [SerializeField] private LayerMask platformLayerMask = default;
        private BoolReactiveProperty isOnGround = new BoolReactiveProperty(); 
        private Rigidbody playerRb;
        
        // Start is called before the first frame update
        private void Start()
        {
            PlayerDirection = transform.position;
            playerRb = GetComponent<Rigidbody>();
            var boxCollider = GetComponent<BoxCollider>();
            var input = GetComponent<IInputEventProvider>();
            // 左
            input.MoveHorizontal
                .Where(x => x < 0)
                .Subscribe(x =>
                {
                    Controller.direction = Direction.back;
                    PlayerDirection = Controller.forward;
                    transform.position += PlayerDirection * (speed * Mathf.Abs(x) * Time.deltaTime);
                })
                .AddTo(this);
            // 右
            input.MoveHorizontal
                .Where(x => x > 0)
                .Subscribe(x =>
                {
                    Controller.direction = Direction.front;
                    PlayerDirection = Controller.forward;
                    transform.position += PlayerDirection * (speed * Mathf.Abs(x) * Time.deltaTime);
                })
                .AddTo(this);
            // ジャンプ
            input.JumpButton
                .Where(x => x && isOnGround.Value)
                .Subscribe(_ => playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse))
                .AddTo(this);
            // 接地判定、壁判定
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    // 当たり判定前方
                    
                }).AddTo(this);
            
            boxCollider.OnCollisionStayAsObservable()
                .Subscribe(x => isOnGround.Value = x != null && (((1 << x.gameObject.layer) & platformLayerMask) != 0))
                .AddTo(this);
            boxCollider.OnCollisionExitAsObservable()
                .Subscribe(_ => isOnGround.Value = false)
                .AddTo(this);
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.DrawWireCube(position + (transform.forward * .05f), Vector3.one * transform.lossyScale.x * 0.5f * 2);
            Gizmos.DrawWireCube(position + (Vector3.down * .05f), Vector3.one * transform.lossyScale.x * 0.5f * 2);
        }
    }
}