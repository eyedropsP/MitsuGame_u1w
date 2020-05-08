using System;
using Damages;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Slime
{
    public class PlayerCore : MonoBehaviour, IDamageable
    {
        private ReactiveProperty<PlayerHp> hp = new ReactiveProperty<PlayerHp>(new PlayerHp(6));
        private ReactiveProperty<Energy> energy = new ReactiveProperty<Energy>(new Energy(40.0f));
        public IReadOnlyReactiveProperty<Energy> Energy => energy;
        public IReadOnlyReactiveProperty<PlayerHp> Hp => hp;
        public ReadOnlyReactiveProperty<bool> IsDead;

        private void Awake()
        {
            IsDead = Hp.Select(x => x.Value <= 0).ToReadOnlyReactiveProperty();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        /// <summary>
        /// 体力はハート制、1ずつ減る(たぶん)
        /// </summary>
        public void AddDamage(float damage)
        {
            Hp
                .Take(1)
                .Where(x => x.Value > 0)
                .Subscribe(_ =>
                {
                    Hp.Value.Sub((int)damage);
                });
        }
    }
}
