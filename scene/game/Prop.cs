using CarrotFantasy.common;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    [Tool]
    public partial class Prop : Node2D
    {
        [Node("Appearance")]
        private Sprite2D appearance;

        [Node("Interaction")]
        private Button interaction;

        [Export]
        public Texture2D Texture
        {
            get
            {
                if (appearance == null)
                {
                    appearance = GetNodeOrNull<Sprite2D>("Appearance");
                }
                if (appearance == null)
                {
                    GD.Print("appearance is null.");
                    return null;
                }
                return appearance.Texture;
            }
            set
            {
                if (appearance == null)
                {
                    appearance = GetNodeOrNull<Sprite2D>("Appearance");
                }
                if (appearance == null)
                {
                    GD.Print("appearance is null.");
                    return;
                }
                appearance.Texture = value;
            }
        }

        private Vector2 propSize;
        [Export]
        public Vector2 PropSize
        {
            get => propSize;
            set
            {
                if (interaction == null)
                {
                    interaction = GetNodeOrNull<Button>("Interaction");
                }
                if (interaction == null)
                {
                    GD.Print("interaction is null.");
                    return;
                }
                if (value.X <= 0 && value.Y <= 0)
                {
                    GD.Print($"propSize{propSize} is invalid.");
                    propSize = Vector2.Zero;
                }
                else
                {
                    propSize = value;
                }
                interaction.Size = GameConstant.Global.CellSize * propSize;
                interaction.Position = -interaction.Size / 2;
            }
        }

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            interaction.Pressed += OnInteractionPressed;
        }

        private void OnInteractionPressed()
        {
            GD.Print($"OnInteractionPressed {Name} {Position}");
        }
    }
}
