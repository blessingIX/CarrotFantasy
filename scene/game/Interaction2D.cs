using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class Interaction2D : Node2D
    {
		[Node("Interaction")]
        protected Button Interaction;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            Interaction.Pressed += OnInteractionPressed;
        }

		protected virtual void OnInteractionPressed()
		{

		}
    }
}
