using Godot;
using System;

namespace CarrotFantasy.scene.home
{
    public partial class FlyingPathFollow2D : PathFollow2D
    {
        [Export]
        private int velocity = 6;
        public override void _Process(double delta)
        {
            base._Process(delta);
            // 沿着Path2D匀速运动
            Progress += (float)(delta * velocity);
        }
    }
}
