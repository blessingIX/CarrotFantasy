using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class TowerItem : Interaction2D
    {
        private string towerCode;
        public string TowerCode
        {
            set
            {
                this.towerCode = value;
                ReloadTexture();
            }
            get => this.towerCode;
        }

        public int TowerPrice;

        [Node("Sprite2D")]
        private Sprite2D sprite2D;

        [Signal]
        public delegate void PlaceTowerEventHandler(string towerCode, int price);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            ReloadTexture();
        }

        protected override void OnInteractionPressed()
        {
            base.OnInteractionPressed();
            EmitSignal(SignalName.PlaceTower, TowerCode, TowerPrice);
        }

        public void FadeIn()
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this, "scale", new Vector2(1f, 1f), 0.2).From(new Vector2(0f, 0f));
        }

        public void FadeOut()
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this, "scale", new Vector2(0f, 0f), 0.2).FromCurrent();
        }

        private void ReloadTexture()
        {
            if (this.sprite2D != null)
            {
                this.sprite2D.Texture = GD.Load<Texture2D>($"res://assets/Theme/Towers/T{this.towerCode}-hd/Available.png");
            }
        }
    }
}
