using System;
using Damages;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Slime
{
    public class PlayerCore : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private FloatReactiveProperty energy = new FloatReactiveProperty(10.0f);
        [SerializeField]
        private IntReactiveProperty hp = new IntReactiveProperty(6);
        
        public IReactiveProperty<float> Energy => energy;
        public IReadOnlyReactiveProperty<int> Hp => hp;
        public ReadOnlyReactiveProperty<bool> IsDead;

        private void Awake()
        {
            IsDead = hp.Select(x => x <= 0).ToReadOnlyReactiveProperty();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        /// <summary>
        /// 体力はハート制、1ずつ減る
        /// </summary>
        public void AddDamage()
        {
            hp
                .Take(1)
                .Where(x => x > 0)
                .Subscribe(_ =>
                {
                    hp.Value--;
                    Debug.Log(Hp.Value);
                    Debug.Log(IsDead.Value);
                });
        }
    }
}
