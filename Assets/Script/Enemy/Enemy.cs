using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Damages;
using Eats;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using UniRx.Async.Triggers;
using LineTrace;
using LineTrace.Extensions;
namespace Enemy
{
    public class Enemy : BaseEnemy
    {
        public enum EnemyBehaviourType
        {
            Stand,
            Patrol,
            Grasshopper
        }

        public EnemyBehaviourType behaviourType = EnemyBehaviourType.Patrol;

        [SerializeField] private DirectionController2d Controller = default;
        
        [Header("初期位置のWayPoint")][SerializeField] private GameObject startWayPoint;

        [Header("初期の向き")][SerializeField] private Direction startDirection = Direction.front;


        [Header("パトロール片道距離")][SerializeField] private float patrolDistance = 50.0f;
        [Header("パトロール距離カウントステップ値")][SerializeField] private float patrolCountStep = 0.01f;


        //指定の挙動の有効/無効
        private bool enableBehaviour = false;


        public Vector3 EnemyDirection { get; private set; }
        private Rigidbody EnemyRb;
        [Header("移動スピード")][SerializeField] private float speed = 6.0f;
        [Header("ジャンプ力")][SerializeField] private float jumpForce = 8.0f;

        private float movedDistance = 0f;

        private /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            transform.position = startWayPoint.transform.position;
            
        }

        private void Start()
        {
            Controller.direction = startDirection;
            this.OnCollisionEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<IEdible>())
                .Where(x => x != null)
                .Subscribe();
            
            this.OnCollisionEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<IDamageable>())
                .Where(x => x != null)
                .Subscribe();

            EnemyDirection = transform.position;
            this.FixedUpdateAsObservable()
                .Subscribe(_ => {
                    switch(behaviourType) {
                        case EnemyBehaviourType.Stand:
                        break;

                        case EnemyBehaviourType.Patrol:
                            if (movedDistance >= patrolDistance) InvertDirection();
                            EnemyDirection = Controller.forward;
                            var add = EnemyDirection * (speed * Mathf.Abs(1) * Time.deltaTime);
                            movedDistance += patrolCountStep;
                            // Debug.Log(movedDistance);
                            transform.position += add;
                        break;

                        case EnemyBehaviourType.Grasshopper:
                        break;

                        default:
                        break;
                    }
                });

        }


        private void InvertDirection() {
            Controller.direction = Controller.direction == Direction.front ? Direction.back : Direction.front;
            movedDistance = 0f;
        }

    }
}