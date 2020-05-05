using UniRx;

namespace Eats
{
	public interface IEdible
	{
		// 食べているか
		IReadOnlyReactiveProperty<bool> IsEat { get; }
		// 食べる
		bool TryEat();
		// 吐き出す
		void Vomit();
	}
}