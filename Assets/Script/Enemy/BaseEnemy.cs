using UniRx;
using UnityEngine;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour, Eats.IEdible
    {
        private readonly Eats.Edible edible;

        public BaseEnemy()
        {
            edible = new Eats.Edible();
        }

        public IReadOnlyReactiveProperty<bool> IsEat
            => edible.IsEat;

        public bool TryEat()
            => edible.TryEat();

        public void Vomit()
            => edible.Vomit();
    }
}
