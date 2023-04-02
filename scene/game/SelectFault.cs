using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class SelectFault : Sprite2D
    {
        [Export(PropertyHint.File, "*.ogg")]
        private string soundPath;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            this.FadeOut();
        }

        private void FadeOut()
        {
            this._<SoundManager>().PlayFleeting(soundPath);
            var tween = CreateTween();
            tween.TweenProperty(this, "modulate:a", 0f, 0.7);
            tween.TweenCallback(Callable.From(QueueFree));
        }
    }
}