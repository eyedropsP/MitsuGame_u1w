using System;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slime
{
	public class PlayerShooter : MonoBehaviour
	{
		[SerializeField] private PlayerCore playerCore = default;
		[SerializeField] private GameObject bullet = default;
		[SerializeField] private GameObject ShotPos = default;
		
		private void Start()
		{
			var input = GetComponent<IInputEventProvider>();
			input.ShotButton
				.Where(x => x && playerCore.Energy.Value.Value >= 5.0f)
				.Subscribe(_ =>
				{
					playerCore.Energy.Value.Sub(5);
					Shot();
				})
				.AddTo(this);
		}

		void Shot()
		{
			// TODO:射撃
			// ReSharper disable once Unity.InefficientPropertyAccess
			Instantiate(bullet, ShotPos.transform.position, transform.rotation);
		}
	}
}