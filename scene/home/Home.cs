using Godot;
using GodotUtilities;
using System;

namespace CarrotFantasy.scene.home
{
    public partial class Home : Sprite2D
    {
        [Node("Carrot")]
        private Carrot carrot;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            carrot.Grow();
        }
    }
}
