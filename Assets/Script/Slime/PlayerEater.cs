using System;
using System.Linq;
using Boo.Lang;
using UniRx;
using UnityEngine;
using Eats;

namespace Slime
{
	public class PlayerEater : MonoBehaviour
	{
		public ReactiveProperty<IEdible> CurrentEat { private set; get; }

		[SerializeField] [Tooltip("Slime's Body")]
		private Transform body = default;
		private GameObject eatObject = default;
		[Header("密接可能な半径")][SerializeField] private float capsuleCastRadius = .5f;
		[Header("密接可能な最長距離")][SerializeField] private float capsuleCastDistance = 1.5f;
		[Header("密接可能な高さ")][SerializeField] private float capsuleCastHeight = 0.25f;
		[Header("密接可能最短距離")] [SerializeField] private float capsuleCastMinDistance = 0.7f;
		private int ignorePlayerLayer;
		
		private PlayerMover playerMover;
		
		private void Start()
		{
			ignorePlayerLayer = ~(1 << LayerMask.NameToLayer($"Player"));
			playerMover = GetComponent<PlayerMover>();
			CurrentEat = new ReactiveProperty<IEdible>(null);
			var input = GetComponent<IInputEventProvider>();
			
			input.EatButton
				.Where(x => x)
				.Subscribe(x =>
				{
					if (CurrentEat.Value != null)
					{
						CurrentEat.Value.Vomit();
						eatObject.transform.parent = null;
						eatObject.GetComponent<Collider>().enabled = true;
						var tmpRb = eatObject.GetComponent<Rigidbody>();
						if (tmpRb != null)
						{
							eatObject.GetComponent<Rigidbody>().useGravity = true;
						}

						CurrentEat.Value = null;
						// TODO:吐き出す音
						// TODO:吐き出すエフェクト
						// TODO:密着状態でEatButton(吐き出し)
						// TODO:吐き出した敵は死ぬ
					}
					else
					{
						var position = transform.position;
						var capsulePos1 = new Vector3(position.x + capsuleCastMinDistance, position.y + capsuleCastHeight, position.z);
						var capsulePos2 = new Vector3(position.x + capsuleCastMinDistance, position.y - capsuleCastHeight, position.z);
						// ReSharper disable once Unity.PreferNonAllocApi
						var casts = Physics.CapsuleCastAll(capsulePos1, capsulePos2,
							capsuleCastRadius, playerMover.PlayerDirection,
							capsuleCastDistance, ignorePlayerLayer);
						
						var castList = new List<RaycastHit>();
						castList.AddRange(casts);
						castList.Sort((a, b) =>
							(int) (Vector3.Distance(a.transform.position, position) -
							       Vector3.Distance(b.transform.position, position)));
						var firstEdibleRayCastHit = castList
							.FirstOrDefault(value => value.transform.GetComponent<IEdible>() != null);

						if (firstEdibleRayCastHit.collider != null)
						{
							var edible = firstEdibleRayCastHit.transform.GetComponent<IEdible>();
							var result = edible.TryEat();
							if (!result) return;	// TODO:食べようとして失敗するモーションやらほしい
							firstEdibleRayCastHit.collider.transform.parent = body;
							firstEdibleRayCastHit.collider.GetComponent<Collider>().enabled = false;
							var tmpRb = firstEdibleRayCastHit.collider.GetComponent<Rigidbody>();
							if (tmpRb != null)
							{
								tmpRb.velocity = Vector3.zero;
								tmpRb.useGravity = false;
							}

							firstEdibleRayCastHit.transform.position = body.position;
							CurrentEat.Value = edible;
							eatObject = firstEdibleRayCastHit.transform.gameObject;
							// TODO:密着の音
						}
					}
				})
				.AddTo(this);
		}
	}
}