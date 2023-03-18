using CarrotFantasy.scene;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.autoload
{
    public partial class SceneManager : Node
    {
        [Node("TransitionScene")]
        private TransitionScene transitionScene;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }
 
        public void ChangeScene(string path)
        {
            ChangeScene(GD.Load(path) as PackedScene);
        }

        public void ChangeScene(PackedScene packedScene)
        {
            if (packedScene == null)
            {
                return;
            }

            // 使用Tween来实现切换场景前后各间隔0.1秒，不然过渡场景不会显示
            Tween tween = CreateTween();
            tween.TweenCallback(Callable.From(() => transitionScene.Show()));
            tween.TweenInterval(0.1);
            tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToPacked(packedScene)));
            tween.TweenInterval(0.1);
            tween.TweenCallback(Callable.From(() => transitionScene.Hide()));
        }
    }
}