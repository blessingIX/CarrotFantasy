using CarrotFantasy.scene.game;
using Godot;
using GodotUtilities;

public partial class Carrot : Node2D
{
    [Node("HurtBox")]
    private HurtBox hurtBox;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        hurtBox.Hurt += node => GD.Print(node);
    }
}
