﻿using System;
using Damages;
using Eats;
using LineTrace;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Slime
{
	public class Bullet : MonoBehaviour
	{
		[Header("弾の攻撃力")][SerializeField] private float power = 20.0f;
		[Header("弾のスピード")][SerializeField] private float bulletSpeed = 5.0f;
		[SerializeField] private LayerMask platformLayerMask = default;
		[SerializeField] private DirectionController2d controller = default;
		
		private void Start()
		{
			var yRotate = transform.rotation.normalized.eulerAngles.y;

			if (yRotate >= 0 && yRotate <= 180)
				controller.direction = Direction.front;
			else
				controller.direction = Direction.back;
			
			this.UpdateAsObservable()
				.Subscribe(_ =>
				{
					transform.position += controller.forward * (bulletSpeed * Time.deltaTime);
				});
			
			this.OnCollisionEnterAsObservable()
				.Subscribe(x =>
				{
					
					if (x != null && (((1 << x.gameObject.layer) & platformLayerMask) != 0))
					{
						Destroy(gameObject);
						return;
						// TODO:弾破壊のエフェクト、音
					}
					
					// ReSharper disable once PossibleNullReferenceException
					var damageable = x.collider.GetComponent<IDamageable>();
					damageable.AddDamage(power);
					Destroy(gameObject);
					// TODO:弾破壊のエフェクト、音
				});
		}
	}
}