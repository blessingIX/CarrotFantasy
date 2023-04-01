using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class Monster : Node2D
    {
        [Node("AnimatedSprite2D")]
        private AnimatedSprite2D animatedSprite2D;

        private int velocity = 80;
        [Export]
        public int Velocity
        {
            get => velocity;
            set
            {
                velocity = value;
                EmitSignal(SignalName.VelocityChanged, value);
            }
        }

        [Signal]
        public delegate void VelocityChangedEventHandler(int velocity);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            // EmitSignal
            this.Velocity = this.Velocity;
        }

        public void OnMonsterMotionArrived(Node motion)
        {
            if (motion != null)
            {
                motion.QueueFree();
            }
            QueueFree();
        }

        public Vector2 GetCenter()
        {
            Vector2 center = Vector2.Zero;
            if (animatedSprite2D != null)
            {
                center = animatedSprite2D.Position;
            }
            return center;
        }
    }
}
