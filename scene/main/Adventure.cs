using GodotUtilities;
using CarrotFantasy.autoload;
using CarrotFantasy.common;

namespace CarrotFantasy.scene.main
{
    public partial class Adventure : ButtonWithAudio
    {
        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public override void _Pressed()
        {
            base._Pressed();
            SceneManager.Instance.ChangeScene("res://scene/adventure/AdventureScene.tscn");
        }
    }
}