using UniRx;

namespace Eats
{
    public class Edible
    {
        public Edible()
        {
            IsEat = new BoolReactiveProperty(false);
        }

        public bool TryEat()
        {
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (!IsEat.Value)
                IsEat.Value = true;
            else
                IsEat.Value = false;
            return IsEat.Value;
        }

        public void Vomit()
            => IsEat.Value = false;
        
        // ReSharper disable once MemberCanBePrivate.Global
        public BoolReactiveProperty IsEat { private set; get; }
    }
}
