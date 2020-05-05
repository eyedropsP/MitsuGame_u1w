using System;
using UnityEngine;

namespace Slime
{
	public class PlayerShot : MonoBehaviour
	{
		[SerializeField]
		private PlayerCore _playerCore;

		private void Start()
		{
			var input = GetComponent<IInputEventProvider>();
		}

		private bool TryShot()
		{
			return false;
		}
	}
}