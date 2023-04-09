using CarrotFantasy.common;
using Godot;
using Godot.Collections;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class CellContainer : CanvasLayer
    {
        [Signal]
        public delegate void CellPressedEventHandler(Vector2 center, bool enable);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            ConnectCellSignal();
        }

        private void ConnectCellSignal()
        {
            Array<Node> nodes = GetChildren();
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }
            foreach (var node in nodes)
            {
                if (node is Cell cell)
                {
                    cell.CellPressed += OnCellCellPressed;
                }
            }
        }

        private void OnCellCellPressed(Vector2 center, bool enable)
        {
            EmitSignal(SignalName.CellPressed, center, enable);
        }
    }
}