using System;
using UniRx;
using UnityEngine;

namespace Slime
{
	public class PlayerShooter : MonoBehaviour
	{
		[SerializeField] private PlayerCore playerCore = default;
		[SerializeField] private GameObject bullet = default;
		
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
			Instantiate(bullet, transform.position, transform.rotation);
		}
	}
}