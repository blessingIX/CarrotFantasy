using CarrotFantasy.common;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class Cell : Button
    {
        [Signal]
        public delegate void CellPressedEventHandler(Vector2 center);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public override void _Pressed()
        {
            base._Pressed();
            EmitSignal(SignalName.CellPressed, new Variant[] { GetCenter() });
        }

        public Vector2 GetCenter()
        {
            return Position + (GameConstant.CellSize / 2);
        }
    }
}
