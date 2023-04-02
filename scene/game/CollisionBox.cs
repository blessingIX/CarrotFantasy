using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class CollisionBox : Area2D
    {
        [Node("CollisionShape2D")]
        protected CollisionShape2D collisionShape2D;

        [Export]
        public NodePath OwnerPath = "..";

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        protected virtual void OnArea2DAreaEntered(Area2D area2D)
        {

        }

        public Node GetOwner()
        {
            return GetNode(OwnerPath);
        }
    }
}
