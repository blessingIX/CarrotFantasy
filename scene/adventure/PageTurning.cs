using System.Collections.Generic;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class PageTurning : Control
    {
        [Node("Prev")]
        private Button prev;

        [Node("Next")]
        private Button next;

        [Node("PointContainer")]
        private Control pointContainer;

        [Signal]
        public delegate void PrevPressedEventHandler();

        [Signal]
        public delegate void NextPressedEventHandler();

        private string pointPackedScenePath = "res://scene/adventure/Point.tscn";

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            prev.Pressed += OnPrevPressed;
            next.Pressed += OnNextPressed;
        }

        private void OnPrevPressed()
        {
            EmitSignal(SignalName.PrevPressed);
        }

        private void OnNextPressed()
        {
            EmitSignal(SignalName.NextPressed);
        }

        public void RefreshPointContainer(int pageCount, int pageIndex)
        {
            if (pointContainer == null)
            {
                return;
            }

            if (pageCount == 0)
            {
                return;
            }

            List<Point> list = pointContainer.GetChildren<Point>();

            if (list != null && list.Count > 0 && list.Count != pageCount)
            {
                foreach (var item in list)
                {
                    item.QueueFree();
                }
                list.Clear();
            }

            if (list == null || list.Count == 0)
            {
                list = new();
                PackedScene packedScene = GD.Load<PackedScene>(pointPackedScenePath);
                for (int i = 0; i < pageCount; i++)
                {
                    Point point = packedScene.InstantiateOrNull<Point>();
                    if (point == null)
                    {
                        continue;
                    }

                    list.Add(point);
                    pointContainer.AddChild(point);
                }
            }

            float offset = -(float)(list.Count + 1) / 2f + 1;
            for (int i = 0; i < list.Count; i++) 
            {
                Point point = list[i];

                point.Position = new Vector2(32f * (offset + (float)i), 0f);
                if (i == pageIndex)
                {
                    point.Active();
                }
                else
                {
                    point.Restrain();
                }
            }
        }
    }
}
