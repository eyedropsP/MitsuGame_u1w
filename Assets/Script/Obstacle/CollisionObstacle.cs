using System;
using System.Runtime.CompilerServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Obstacle
{
	[RequireComponent(typeof(Collider))]
	public class CollisionObstacle : MonoBehaviour
	{
		[SerializeField] private LayerMask bulletLayer;
		private void Start()
		{
			
		}
	}
}