using CarrotFantasy.common;
using Godot;
using Godot.Collections;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class CellContainer : CanvasLayer
    {
        [Signal]
        public delegate void CellPressedEventHandler(Vector2 center);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            BuildCells();
        }

        private void BuildCells()
        {
            string path = "res://scene/game/Cell.tscn";
            PackedScene packedScene = GD.Load<PackedScene>(path);
            if (packedScene == null)
            {
                GD.PrintErr($"[WARN] {path} load failed.");
                return;
            }

            Array<Node> nodes = GetChildren();
            if (nodes != null && nodes.Count > 0)
            {
                foreach (Node node in nodes)
                {
                    if (node == null)
                    {
                        continue;
                    }
                    node.QueueFree();
                }
                nodes.Clear();
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Cell cell = packedScene.Instantiate<Cell>();
                    cell.Position = new Vector2(j, i) * GameConstant.Global.CellSize;
                    cell.CellPressed += OnCellCellPressed;
                    AddChild(cell);
                }
            }
        }

        private void OnCellCellPressed(Vector2 center)
        {
            EmitSignal(SignalName.CellPressed, new Variant[] { center });
        }
    }
}