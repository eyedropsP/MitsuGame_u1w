using UniRx;

namespace Slime
{
	/// <summary>
	/// エネルギークラス
	/// </summary>
	public class Energy
	{
		public float Value { private set; get; }

		public Energy(float value)
			=> Value = value;

		public void Add(float energy)
			=> Value += energy;

		public void Sub(float consumption)
		{
			if (Value <= 0)
			{
				Value = 0;
				return;
			}
			
			Value -= consumption;
		}
	}
}