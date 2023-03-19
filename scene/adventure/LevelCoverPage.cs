using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class LevelCoverPage : Sprite2D
    {
        [Parent]
        private Level level;

        [Signal]
        public delegate void BeSelectedEventHandler(int index);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("click"))
            {
                Rect2 rect2 = GetRect();
                Vector2 mPosition = GetLocalMousePosition();
                if (mPosition.X >= rect2.Position.X
                    && mPosition.Y >= rect2.Position.Y
                    && mPosition.X <= rect2.Position.X + rect2.Size.X
                    && mPosition.Y <= rect2.Position.Y + rect2.Size.Y)
                {
                    if (level != null && !level.BeSelected)
                    {
                        int index = level.GetLevelIndex();
                        EmitSignal(SignalName.BeSelected, new Variant[] { Variant.From<int>(index) });
                    }
                }
            }
        }
    }
}
