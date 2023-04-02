using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class HurtBox : CollisionBox
    {
        [Signal]
        public delegate void HurtEventHandler(Node owner);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            this.AreaEntered += OnArea2DAreaEntered;
        }

        protected override void OnArea2DAreaEntered(Area2D area2D)
        {
            if (area2D is HitBox hitBox)
            {
                EmitSignal(SignalName.Hurt, hitBox.GetOwner());
            }
        }
    }
}
