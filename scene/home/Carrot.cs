using Godot;
using System;
using GodotUtilities;

namespace CarrotFantasy.scene.home
{
    public partial class Carrot : Sprite2D
    {

        [Node("LeafMiddle")]
        private Sprite2D leafMiddle;

        [Node("LeafRight")]
        private Sprite2D LeafRight;

        [Node("Timer")]
        private Timer timer;

        private Timer growDelayTimer = new();

        private Vector2 initPosition;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
            AddChild(growDelayTimer);

            initPosition = Position;
            timer.Timeout += () => GD.Print("timeout");
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
                tween.TweenCallback(Callable.From(() => GD.Print("TweenCallback")));
            };

            growDelayTimer.Start();
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if (@event.IsActionPressed("ui_up"))
            {
                Grow();
            }
        }
    }
}