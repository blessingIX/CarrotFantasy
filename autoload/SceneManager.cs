using System;
using CarrotFantasy.scene;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.autoload
{
    public partial class SceneManager : Node
    {
        public static readonly Object[] DefaultSceneData = new Object[] { };

        [Node("TransitionScene")]
        private TransitionScene transitionScene;

        private Object[] lastSceneData = DefaultSceneData;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public void ChangeScene(string path)
        {
            ChangeScene(path, DefaultSceneData);
        }

        public void ChangeScene(string path, params Object[] variant)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            ChangeScene(GD.Load(path) as PackedScene, variant);
        }

        public void ChangeScene(PackedScene packedScene)
        {
            ChangeScene(packedScene, DefaultSceneData);
        }

        public void ChangeScene(PackedScene packedScene, params Object[] variant)
        {
            if (packedScene == null)
            {
                return;
            }

            // 使用Tween来实现切换场景前间隔0.1秒，不然过渡场景不会显示
            Tween tween = CreateTween();
            tween.TweenCallback(Callable.From(() => transitionScene.Show()));
            tween.TweenInterval(0.1);
            tween.TweenCallback(Callable.From(() =>
            {
                ChangeSceneToPacked(packedScene, variant);
                transitionScene.Hide();
            }));
        }

        private void ChangeSceneToPacked(PackedScene packedScene, params Object[] variants)
        {
            try
            {
                lastSceneData = variants;
                GetTree().ChangeSceneToPacked(packedScene);
            }
            catch (Exception)
            {
                lastSceneData = DefaultSceneData;
            }
        }

        public Object[] Data()
        {
            return lastSceneData;
        }

        public T Data<T>(int index)
        {
            if (index < 0 || index >= lastSceneData.Length)
            {
                return default;
            }
            Object obj = lastSceneData[index];
            if (obj is T t)
            {
                return t;
            }
            return default;
        }
    }
}