using System.Collections.Generic;
using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class CountDown : Control
    {
        [Node("Control/Effect")]
        private Sprite2D effect;

        [Node("Control/Characters")]
        private Sprite2D characters;

        [Export(PropertyHint.File, "*.ogg")]
        private string countDownSound;

        [Export(PropertyHint.File, "*.ogg")]
        private string goSound;

        [Signal]
        public delegate void FinishedEventHandler();

        private int value = 3;

        private Dictionary<string, Texture2D> textureDict = new();

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            for (int i = 0; i <= value; i++)
            {
                string key = i.ToString();
                textureDict.Add(i.ToString(), GD.Load<Texture2D>($"res://resource/game/CountDown{key}.tres"));
            }

            CountDownAnimation();
        }

        private void CountDownAnimation()
        {
            UpdateCharacters(value.ToString());
            if (value < 0)
            {
                effect.Hide();
                return;
            }

            if (value == 0)
            {
                effect.Hide();

                SoundManager.Instance.PlayFleeting(goSound);
                double fadeInInterval = 0.1;
                double interval = 0.2;
                double fadeOutInterval = 0.1;
                Tween tween = CreateTween();
                tween.SetParallel(true);
                tween.TweenProperty(characters, (string)Node2D.PropertyName.Scale, Vector2.One, fadeInInterval).From(Vector2.Zero);
                tween.TweenProperty(characters, (string)Node2D.PropertyName.RotationDegrees, 0f, fadeInInterval).From(-180f);
                tween.SetParallel(false);
                tween.TweenInterval(interval);
                tween.TweenProperty(characters, (string)Node2D.PropertyName.Scale, Vector2.Zero, fadeOutInterval).From(Vector2.One);
                tween.TweenCallback(Callable.From(FinishCountDown));
            }
            else
            {
                SoundManager.Instance.PlayFleeting(countDownSound);
                double effectInterval = 1.1;
                double charactersFadeInInterval = 0.1;
                double charactersFadeOutInterval = 0.1;
                double charactersInterval = effectInterval - charactersFadeInInterval - charactersFadeOutInterval;
                Tween effectTween = CreateTween();
                Tween charactersTween = CreateTween();
                effectTween.TweenProperty(effect, (string)Node2D.PropertyName.RotationDegrees, -90f, effectInterval).From(270f);
                charactersTween.TweenProperty(characters, (string)Node2D.PropertyName.Scale, Vector2.One, charactersFadeInInterval).From(Vector2.Zero);
                charactersTween.TweenInterval(charactersInterval);
                charactersTween.TweenProperty(characters, (string)Node2D.PropertyName.Scale, Vector2.Zero, charactersFadeOutInterval).From(Vector2.One);
                effectTween.TweenCallback(Callable.From(CountDownAnimation));
            }
            value--;
        }

        private void UpdateCharacters(string suffix)
        {
            Texture2D texture = textureDict[suffix];
            characters.Texture = texture;
        }

        private void FinishCountDown()
        {
            EmitSignal(SignalName.Finished, new Variant[] { });
            QueueFree();
        }

        public void SkipCountDown()
        {
            value = 0;
        }
    }
}
