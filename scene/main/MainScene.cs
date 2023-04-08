using Godot;
using GodotUtilities;

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
		}
	}
}
