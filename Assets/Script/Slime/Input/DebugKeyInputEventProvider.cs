using UniRx; 
using UniRx.Triggers;
using UnityEngine;

namespace Slime.Input
{
    public class DebugKeyInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly  ReactiveProperty<bool> _jumpButtonPushed = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<bool> _eatButtonPushed = new ReactiveProperty<bool>();
        private readonly  ReactiveProperty<bool> _shotButtonPushed = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<float> _moveHorizontal = new ReactiveProperty<float>();
        
        public IReadOnlyReactiveProperty<bool> JumpButton => _jumpButtonPushed;
        public IReadOnlyReactiveProperty<bool> EatButton => _eatButtonPushed;
        public IReadOnlyReactiveProperty<bool> ShotButton => _shotButtonPushed;
        public IReadOnlyReactiveProperty<float> MoveHorizontal => _moveHorizontal;

        private void Start()
        {
            //　ジャンプボタン
            this.UpdateAsObservable()
                .Select(_ => UnityEngine.Input.GetButtonDown("Jump"))
                .DistinctUntilChanged()
                .Subscribe(x => _jumpButtonPushed.Value = x);
            
            // 食べるボタン
            this.UpdateAsObservable()
                .Select(_ => UnityEngine.Input.GetButtonDown("Eat"))
                .DistinctUntilChanged()
                .Subscribe(x => _eatButtonPushed.Value = x);

            // 射撃ボタン
            this.UpdateAsObservable()
                .Select(_ => UnityEngine.Input.GetButton("Shot"))
                .DistinctUntilChanged()
                .Subscribe(x => _shotButtonPushed.Value = x);
                
            // 左右
            this.UpdateAsObservable()
                .Select(_ => UnityEngine.Input.GetAxis("Horizontal"))
                .Subscribe(x => _moveHorizontal.SetValueAndForceNotify(x));

            // このコンポーネントが破棄された時点でObservableも破棄
            _jumpButtonPushed.AddTo(this);
            _eatButtonPushed.AddTo(this);
            _shotButtonPushed.AddTo(this);
            _moveHorizontal.AddTo(this);
        }
    }
}
