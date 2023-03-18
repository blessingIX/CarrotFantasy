using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class SelectLevelScene : Scene
    {
        [Node("MenuBar/Back")]
        private Button back;

        [Node("MenuBar/Help")]
        private Button help;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            back.Pressed += () => this._<SceneManager>().ChangeScene("res://scene/adventure/AdventureScene.tscn");
        }
    }
}
