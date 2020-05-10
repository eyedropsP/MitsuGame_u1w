using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Damages;
using Eats;
using LineTrace;
using LineTrace.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Slime
{
	public class Bullet : MonoBehaviour
	{
		[Header("弾の攻撃力")][SerializeField] private float power = 20.0f;
		[Header("弾のスピード")][SerializeField] private float bulletSpeed = 15.0f;
		[SerializeField] private DirectionController2d controller = default;
		private int ignoreBulletLayer;

		private void Start()
		{
			ignoreBulletLayer = ~(1 << LayerMask.NameToLayer($"Bullet"));

			// スライムの向きをもとに弾の初期方向を決定
			var yRotate = transform.rotation.normalized.eulerAngles.y;
			if (yRotate >= 0 && yRotate <= 180)
				controller.direction = Direction.front;
			else
				controller.direction = Direction.back;

			this.FixedUpdateAsObservable()
				.Subscribe(_ =>
				{
					// 移動
					// 高さ制御用で下方向にレイを打つ
					RaycastHit groundHit;
					if (Physics.Raycast(transform.position, Vector3.down,
						out groundHit, Mathf.Infinity, ignoreBulletLayer))
					{
						Vector3 newPos = transform.position;
						newPos.y = groundHit.point.y + groundHit.distance;
						transform.position = newPos;
						transform.position += controller.forward * (bulletSpeed * Time.deltaTime);
					}
					
					// 当たり判定、レイを少し前に飛ばす
					var radius = transform.lossyScale.x * 0.5f;
					var castMaxDistance = 0.2f;
					var hit = new RaycastHit();
					var isHit = Physics.SphereCast(transform.position, radius,
						controller.forward, out hit, castMaxDistance, ignoreBulletLayer);
					if (isHit)
					{
						var hitGameObject = hit.collider.gameObject;
						if (hitGameObject.GetComponent<IDamageable>() != null)
						{
							hitGameObject.GetComponent<IDamageable>().AddDamage(power);
						}

						Destroy(gameObject);
						return;
					}
					// 当たり判定、弾の内部判定
					var overLapColliders = Physics.OverlapBox(transform.position,
						transform.localScale, Quaternion.identity, ignoreBulletLayer);
					foreach (var collider in overLapColliders)
					{
						// IDamageableだったら、ダメージを与える
						if(collider.GetComponent<IDamageable>() != null)
							collider.GetComponent<IDamageable>().AddDamage(power);
						// 1度でもcolliderに衝突したら弾消滅
						Destroy(gameObject);
						return;
					}
				}).AddTo(this);

			// 一定時間衝突がない場合弾消滅
			Observable.Timer(TimeSpan.FromMilliseconds(5000))
				.Subscribe(_ => Destroy(gameObject)).AddTo(this);

			Disposable.Create(() =>
			{
				// TODO:弾消滅エフェクト、音
			}).AddTo(this);
		}
	}
}