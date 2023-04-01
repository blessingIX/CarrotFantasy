using System.Collections.Generic;
using CarrotFantasy.autoload;
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

        private Queue<float> wave = new();

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

        public void Spawn(float[] intervals)
        {
            if (intervals == null || intervals.Length == 0)
            {
                return;
            }
            wave.Clear();
            foreach (float interval in intervals)
            {
                wave.Enqueue(interval);
            }
            StartSpawnTimer();
        }

        private void StartSpawnTimer()
        {
            if (wave.Count == 0)
            {
                timer.Stop();
                return;
            }

            timer.WaitTime = wave.Dequeue();
            timer.Start();
        }

        private void Spawn()
        {
            if (monsterPool == null || monsterMotionPool == null)
            {
                return;
            }

            PackedScene monsterScene = GD.Load<PackedScene>("res://scene/game/monster/YellowFlying.tscn");
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
            this._<SoundManager>().PlayFleeting(MonsterSpawnSound);
            if (monsterSpawnEffect != null)
            {
                monsterSpawnEffect.Position = monster.GetCenter();
                monster.AddChild(monsterSpawnEffect);
            }

            StartSpawnTimer();
        }
    }
}
