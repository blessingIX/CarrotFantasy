using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.main
{
    public partial class Carrot : Sprite2D
    {

        [Node("LeafMiddleContainer")]
        private Node2D leafMiddleContainer;

        [Node("LeafRightContainer")]
        private Node2D LeafRightContainer;

        [Node("LeafSwingTimer")]
        private Timer leafSwingTimer;

        private Timer growDelayTimer = new();

        private Vector2 initPosition;

        private int swingCycleCount = 0;

        private const int swingCycleTotalCount = 6;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
            AddChild(growDelayTimer);

            initPosition = Position;
            leafSwingTimer.Timeout += OnLeafSwingTimerTimeout;
        }

        public void Grow()
        {
            if (!growDelayTimer.IsStopped())
            {
                return;
            }

            double duration = 0.16;

            growDelayTimer.OneShot = true;
            growDelayTimer.WaitTime = 0.2;
            growDelayTimer.Timeout += () =>
            {

                Tween tween = CreateTween().SetParallel();
                tween.SetEase(Tween.EaseType.OutIn).SetTrans(Tween.TransitionType.Sine);
                tween.TweenProperty(this, (string)Node2D.PropertyName.Scale, Vector2.One, duration).From(Vector2.Zero);
                tween.SetEase(Tween.EaseType.In);
                tween.TweenProperty(this, (string)Node2D.PropertyName.Position, initPosition, duration).From(initPosition + new Vector2(-100f, 0f));
            };

            growDelayTimer.Start();
            ResetSwingCycle();
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if (@event.IsActionPressed("ui_up"))
            {
                Grow();
            }
        }

        private void OnLeafSwingTimerTimeout()
        {
            swingCycleCount = ++swingCycleCount % swingCycleTotalCount;
            switch (swingCycleCount)
            {
                case 1:
                    LeafSwingAnimation(leafMiddleContainer);
                    break;
                case 2:
                    LeafSwingAnimation(LeafRightContainer);
                    break;
                default:
                    break;
            }
        }

        private void LeafSwingAnimation(Node2D node2D)
        {
            if (node2D == null)
            {
                return;
            }

            Tween tween = CreateTween();
            tween.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Linear);
            tween.TweenProperty(node2D, (string)Node2D.PropertyName.RotationDegrees, 15f, 0.1);
            tween.TweenProperty(node2D, (string)Node2D.PropertyName.RotationDegrees, 0f, 0.1);
            tween.TweenProperty(node2D, (string)Node2D.PropertyName.RotationDegrees, 15f, 0.1);
            tween.TweenProperty(node2D, (string)Node2D.PropertyName.RotationDegrees, 0f, 0.1);
        }

        private void ResetSwingCycle()
        {
            swingCycleCount = 0;
            leafMiddleContainer.RotationDegrees = 0f;
            LeafRightContainer.RotationDegrees = 0f;

            if (!leafSwingTimer.IsStopped())
            {
                leafSwingTimer.Stop();
            }
            leafSwingTimer.Start();
        }
    }
}