using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Slime
{
	interface IInputEventProvider
	{
		IReadOnlyReactiveProperty<bool> JumpButton { get; }
		IReadOnlyReactiveProperty<bool> EatButton { get; }
		IReadOnlyReactiveProperty<bool> ShotButton { get; }
		IReadOnlyReactiveProperty<bool> AbsorbButton { get; }
		IReadOnlyReactiveProperty<float> MoveHorizontal { get; }
	}
}
