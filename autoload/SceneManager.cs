using System;
using CarrotFantasy.scene;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.autoload
{
    public partial class SceneManager : Node
    {
        [Node("TransitionScene")]
        private TransitionScene transitionScene;

        private Variant lastSceneData;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public void ChangeScene(string path)
        {
            ChangeScene(path, default);
        }

        public void ChangeScene(string path, Variant variant)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            ChangeScene(GD.Load(path) as PackedScene, variant);
        }

        public void ChangeScene(PackedScene packedScene)
        {
            ChangeScene(packedScene, default);
        }

        public void ChangeScene(PackedScene packedScene, Variant variant)
        {
            if (packedScene == null)
            {
                return;
            }

            // 使用Tween来实现切换场景前后各间隔0.1秒，不然过渡场景不会显示
            Tween tween = CreateTween();
            tween.TweenCallback(Callable.From(() => transitionScene.Show()));
            tween.TweenInterval(0.1);
            tween.TweenCallback(Callable.From(() => ChangeSceneToPacked(packedScene, variant)));
            tween.TweenInterval(0.1);
            tween.TweenCallback(Callable.From(() => transitionScene.Hide()));
        }

        private void ChangeSceneToPacked(PackedScene packedScene, Variant variant)
        {
            try
            {
                lastSceneData = variant;
                GetTree().ChangeSceneToPacked(packedScene);
            }
            catch (Exception)
            {
                lastSceneData = default;
            }
        }

        public T Data<[MustBeVariant] T>()
        {
            return lastSceneData.As<T>();
        }
    }
}