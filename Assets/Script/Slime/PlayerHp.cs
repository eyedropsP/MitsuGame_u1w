namespace Slime
{
	public class PlayerHp
	{
		public PlayerHp(int value)
		{
			Value = value;
		}

		private const int maxHp = 6;
		public int Value { get; private set; }

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