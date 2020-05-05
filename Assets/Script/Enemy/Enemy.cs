using System;
using System.Runtime.CompilerServices;
using Damages;
using Eats;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using UniRx.Async.Triggers;

namespace Enemy
{
    public class Enemy : BaseEnemy
    {
        private void Start()
        {
            this.OnCollisionEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<IEdible>())
                .Where(x => x != null)
                .Subscribe();
            
            this.OnCollisionEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<IDamageable>())
                .Where(x => x != null)
                .Subscribe();
        }
    }
}