namespace Enemy
{
	public class EnemyHp
	{
		public EnemyHp(float value)
		{
			Value = value;
		}

		private const float maxHp = 10;
		public  float Value { get; private set; }

		public void Sub(int damage)
		{
			if (Value <= 0)
			{
				Value = 0;
				return;
			}

			Value -= damage;
		}

		public void Add(int recoveryAmount)
		{
			if (Value >= maxHp)
			{
				Value = maxHp;
				return;
			}

			Value += recoveryAmount;
		}
	}
}