using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class Point : Sprite2D
    {
        [Node("Highlight")]
        private Sprite2D highlight;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public void Active()
        {
            highlight?.Show();
        }

        public void Restrain()
        {
            highlight?.Hide();
        }
    }
}
