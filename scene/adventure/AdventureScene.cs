using Godot;
using GodotUtilities;
using CarrotFantasy.autoload;

namespace CarrotFantasy.scene.adventure
{
    public partial class AdventureScene : Scene
    {
        [Node("MenuBar/Back")]
        private Button back;

        [Node("MenuBar/Help")]
        private Button help;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            back.Pressed += () => this._<SceneManager>().ChangeScene("res://scene/main/MainScene.tscn");
        }
    }
}
