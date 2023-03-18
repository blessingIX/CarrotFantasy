using Godot;
using GodotUtilities;
using System;
using CarrotFantasy.autoload;

namespace CarrotFantasy.scene.main
{
    public partial class MainScene : Scene
    {
        [Node("Carrot")]
        private Carrot carrot;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            carrot.Grow();
            this._<SoundManager>()?.Play("res://resource/main/BGMusic.tres");
        }
    }
}
