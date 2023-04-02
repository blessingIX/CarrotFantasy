using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class Monster : Node2D
    {
        [Node("AnimatedSprite2D")]
        private AnimatedSprite2D animatedSprite2D;

        [Node("HitBox")]
        private HitBox hitBox;

        [Node("HurtBox")]
        private HurtBox hurtBox;

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
            hitBox.Hit += OnHitBoxHit;
            hurtBox.Hurt += OnHurtBoxHurt;
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

        private void OnHitBoxHit(Node beHit)
        {
            if (beHit is Carrot carrot)
            {
                this._<SoundManager>().PlayFleeting("res://assets/Music/Items/Crash.ogg");
            }
        }

        public void OnHurtBoxHurt(Node hitter)
        {

        }
    }
}
