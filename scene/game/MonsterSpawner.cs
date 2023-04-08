using System.Collections.Generic;
using CarrotFantasy.autoload;
using CarrotFantasy.common;
using CarrotFantasy.def;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class MonsterSpawner : Node
    {
        [Export]
        public NodePath MonsterPoolPath;

        [Export]
        public NodePath MonsterMotionPoolPath;

        [Export(PropertyHint.File, "*.ogg")]
        public string MonsterSpawnSound;

        private Node monsterPool;

        private Node monsterMotionPool;

        [Node("Timer")]
        private Timer timer;

        private Queue<MonsterSpawnDef> wave = new();

        [Signal]
        public delegate void SpwanFinishedEventHandler();

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            if (!string.IsNullOrEmpty(MonsterPoolPath))
            {
                monsterPool = GetNodeOrNull<Node>(MonsterPoolPath);
            }
            if (!string.IsNullOrEmpty(MonsterMotionPoolPath))
            {
                monsterMotionPool = GetNodeOrNull<Node>(MonsterMotionPoolPath);
            }

            timer.Timeout += Spawn;
        }

        public void Spawn(WaveDef waveDef)
        {
            if (waveDef == null)
            {
                return;
            }
            List<MonsterBundleDef> monsterBundles = waveDef.MonsterBundles;
            if (monsterBundles == null || monsterBundles.Count == 0)
            {
                return;
            }
            wave.Clear();
            wave.Enqueue(MonsterSpawnDef.Empty(waveDef.Delay));
            foreach (MonsterBundleDef monsterBundleDef in monsterBundles)
            {
                if (monsterBundleDef == null || monsterBundleDef.Qty <= 0)
                {
                    continue;
                }
                for (int i = 0; i < monsterBundleDef.Qty; i++)
                {
                    wave.Enqueue(new MonsterSpawnDef()
                    {
                        Scene = monsterBundleDef.Scene,
                        Delay = monsterBundleDef.Delay
                    });
                }
            }
            StartSpawnTimer();
        }

        private void StartSpawnTimer()
        {
            if (wave.Count == 0)
            {
                timer.Stop();
                EmitSignal(SignalName.SpwanFinished);
                return;
            }

            timer.WaitTime = wave.Peek().Delay;
            timer.Start();
        }

        private void Spawn()
        {
            if (wave.Count == 0)
            {
                return;
            }

            MonsterSpawnDef monsterSpawnDef = wave.Dequeue();
            if (monsterSpawnDef == null || monsterSpawnDef.Scene == GameConstant.Monster.Empty)
            {
                StartSpawnTimer();
                return;
            }

            if (monsterPool == null || monsterMotionPool == null)
            {
                return;
            }

            PackedScene monsterScene = GD.Load<PackedScene>($"res://scene/game/monster/{monsterSpawnDef.Scene}.tscn");
            Monster monster = monsterScene.InstantiateOrNull<Monster>();
            if (monster == null)
            {
                return;
            }

            PackedScene monsterMotionScene = GD.Load<PackedScene>("res://scene/game/MonsterMotion.tscn");
            MonsterMotion monsterMotion = monsterMotionScene.InstantiateOrNull<MonsterMotion>();
            if (monsterMotion == null)
            {
                return;
            }

            PackedScene monsterSpawnEffectScene = GD.Load<PackedScene>("res://scene/game/MonsterSpawnEffect.tscn");
            MonsterSpawnEffect monsterSpawnEffect = monsterSpawnEffectScene.InstantiateOrNull<MonsterSpawnEffect>();


            monsterMotion.Arrived += monster.OnMonsterMotionArrived;
            monster.VelocityChanged += monsterMotion.OnVelocityChanged;

            monsterPool.AddChild(monster);
            monsterMotion.RemotePath = $"../../{MonsterPoolPath}/{monster.Name}";
            monsterMotionPool.AddChild(monsterMotion);
            SoundManager.Instance.PlayFleeting(MonsterSpawnSound);
            if (monsterSpawnEffect != null)
            {
                monsterSpawnEffect.Position = monster.GetCenter();
                monster.AddChild(monsterSpawnEffect);
            }

            StartSpawnTimer();
        }

        private class MonsterSpawnDef
        {
            public string Scene;

            public float Delay;

            public static MonsterSpawnDef Empty(float delay)
            {
                return new MonsterSpawnDef()
                {
                    Scene = GameConstant.Monster.Empty,
                    Delay = delay
                };
            }
        }
    }
}
