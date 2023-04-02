using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class MonsterSpawnEffect : Sprite2D
    {
        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            MonsterSpawnEffectAnimation();
        }

        public void MonsterSpawnEffectAnimation()
        {
            Texture = GD.Load<Texture2D>("res://resource/game/MonsterSpawnEffectStage1.tres");
            Tween tween = CreateTween();
            // tween.SetSpeedScale(0.25f); // test
            double stage1Interval = 0.1;
            tween.TweenInterval(stage1Interval);
            tween.TweenCallback(Callable.From(() =>
            {
                Texture = GD.Load<Texture2D>("res://resource/game/MonsterSpawnEffectStage2.tres");
            }));
            double stage2Interval = 0.2;
            tween.TweenProperty(this, (string)PropertyName.Scale, Vector2.One * 1.5f, stage2Interval);
            tween.TweenCallback(Callable.From(() => {}));
            double stage3Interval = 0.2;
            tween.SetParallel(true);
            tween.TweenProperty(this, (string)PropertyName.RotationDegrees, 90f, stage3Interval);
            tween.TweenProperty(this, "modulate:a", 0f, stage3Interval);
            tween.SetParallel(false);
            tween.TweenCallback(Callable.From(QueueFree));
        }
    }
}