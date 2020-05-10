using System;
using Cinemachine;
using UnityEngine;

namespace Managers
{
	public class SwitchCamera : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera dollyLookCam = default, followCam = default;

		private void OnTriggerEnter(Collider other)
		{
			dollyLookCam.Priority = 10;
			followCam.Priority = 11;
		}

		private void OnTriggerExit(Collider other)
		{
			dollyLookCam.Priority = 11;
			followCam.Priority = 10;
		}
	}
}