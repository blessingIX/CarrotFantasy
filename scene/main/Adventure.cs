using Godot;
using GodotUtilities;
using System;
using CarrotFantasy.autoload;

namespace CarrotFantasy.scene.main
{
    public partial class Adventure : Button
    {
        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public override void _Pressed()
        {
            base._Pressed();
            this._<SceneManager>().ChangeScene("res://scene/adventure/AdventureScene.tscn");
        }
    }
}