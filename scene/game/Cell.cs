using CarrotFantasy.common;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    [Tool]
    public partial class Cell : Button
    {
        [Export]
        public Theme EnalbelTheme;

        [Export]
        public Theme DisableTheme;

        private bool enable;
        [Export]
        public bool Enable
        {
            set
            {
                enable = value;
                if (enable && EnalbelTheme != null)
                {
                    Theme = EnalbelTheme;
                }
                if (!enable && DisableTheme != null)
                {
                    Theme = DisableTheme;
                }
            }
            get => enable;
        }

        [Signal]
        public delegate void CellPressedEventHandler(Vector2 center, bool enable);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public override void _Pressed()
        {
            base._Pressed();
            EmitSignal(SignalName.CellPressed, GetCenter(), enable);
        }

        public Vector2 GetCenter()
        {
            return Position + (GameConstant.Global.CellSize / 2);
        }
    }
}
