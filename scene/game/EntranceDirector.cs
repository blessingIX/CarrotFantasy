using System;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class EntranceDirector : Node2D
    {
        [Node("Arrow1")]
        private Sprite2D arrow1;

        [Node("Arrow2")]
        private Sprite2D arrow2;

        [Node("Arrow3")]
        private Sprite2D arrow3;

        [Export(PropertyHint.Range, "1,2147483647")]
        public int TotalCount = 8;

        private int count = 0;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            ResetArrows();
        }

        public void Start()
        {
            ResetArrows();
            if (count >= TotalCount)
            {
                count = 0;
                return;
            }
            count++;
            ArrowFlashAnimation(arrow1, 0.1f);
            ArrowFlashAnimation(arrow2, 0.2f);
            ArrowFlashAnimation(arrow3, 0.3f, Start);
        }

        private void ArrowFlashAnimation(Sprite2D arrow, float delay, Action action = null)
        {
            Tween tween = CreateTween();
            if (delay > 0f)
            {
                tween.TweenInterval(delay);
            }
            tween.TweenProperty(arrow, "modulate:a", 1f, 0.1);
            tween.TweenInterval(0.3);
            tween.TweenProperty(arrow, "modulate:a", 0f, 0.1);
            if (action != null)
            {
                tween.TweenCallback(Callable.From(action));
            }
        }

        public void ResetArrows()
        {
            arrow1.Modulate = new Color(arrow1.Modulate.R, arrow1.Modulate.G, arrow1.Modulate.B, 0f);
            arrow2.Modulate = new Color(arrow2.Modulate.R, arrow2.Modulate.G, arrow2.Modulate.B, 0f);
            arrow3.Modulate = new Color(arrow3.Modulate.R, arrow3.Modulate.G, arrow3.Modulate.B, 0f);
        }
    }
}
