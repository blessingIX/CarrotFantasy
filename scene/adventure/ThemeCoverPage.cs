using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class ThemeCoverPage : Sprite2D
    {
        [Export(PropertyHint.File, "*.tscn")]
        private string themePackedScenePath = "res://scene/adventure/SelectLevelScene.tscn";

        [Node("../..")]
        private Themes themes;

        [Parent]
        private Node2D theme;

        private bool accessible;

        private readonly Vector2 accessiblePosition = new Vector2(384f, 272f);

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            accessible = theme.GlobalPosition == Vector2.Zero;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("click"))
            {
                Rect2 rect2 = GetRect();
                Vector2 mPosition = GetLocalMousePosition();
                if (accessible
                    && mPosition.X >= rect2.Position.X
                    && mPosition.Y >= rect2.Position.Y
                    && mPosition.X <= rect2.Position.X + rect2.Size.X
                    && mPosition.Y <= rect2.Position.Y + rect2.Size.Y)
                {
                    SceneManager.Instance.ChangeScene(themePackedScenePath);
                }
            }
        }
    }
}