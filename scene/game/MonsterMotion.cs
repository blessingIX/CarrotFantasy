using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class MonsterMotion : PathFollow2D
    {
        [Node("RemoteTransform2D")]
        private RemoteTransform2D remoteTransform2D;

        [Export]
        private int velocity = 25;

        private NodePath remotePath;
        [Export]
        public NodePath RemotePath { set; get; }

        // 上一帧的ProgressRatio
        private float lastProgressRatio = -1f;

        [Signal]
        public delegate void ArrivedEventHandler(MonsterMotion motion);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            remoteTransform2D.RemotePath = $"../{RemotePath}";
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            Progress += (float)(delta * velocity);

            // 如果当前帧的ProgressRatio等于1并且上一帧的ProgressRatio不等于当前帧的ProgressRatio视为到达一次终点
            if (ProgressRatio == 1f && lastProgressRatio != ProgressRatio)
            {
                EmitSignal(SignalName.Arrived, this);
            }
            lastProgressRatio = ProgressRatio;
        }

        public void OnVelocityChanged(int velocity)
        {
            this.velocity = velocity;
        }
    }
}
