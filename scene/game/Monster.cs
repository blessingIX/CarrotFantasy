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

        [Export(PropertyHint.Range, "1,2147483647")]
        public int Atk = 1;

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
            // 怪物吃掉萝卜/到达终点经常同时触发，如果其中一个方法中已经进行了QueueFree会抛出System.ObjectDisposedException
            // 这里判断一下当前对象是否有效再进行QueueFree
            if (IsInstanceValid(this))
            {
                QueueFree();
            }
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
                SoundManager.Instance.PlayFleeting("res://assets/Music/Items/Crash.ogg");
                QueueFree();
            }
        }

        public void OnHurtBoxHurt(Node hitter)
        {

        }
    }
}
