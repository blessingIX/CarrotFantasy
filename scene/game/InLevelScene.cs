using System.Collections.Generic;
using CarrotFantasy.autoload;
using CarrotFantasy.def;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class InLevelScene : Scene
    {
        [Export(PropertyHint.Enum, "Skyline,Jungle,Desert,Deepsea")]
        private string themeCode = "Skyline";

        [Export(PropertyHint.Range, "1,2147483647")]
        private int levelIndex = 1;

        [Node("HUD")]
        private CanvasLayer hud;

        [Node("HUD/MenuBar/Menu")]
        private Button menu;

        [Node("HUD/GameMenuRect")]
        private TextureRect gameMenuRect;

        [Node("HUD/GameMenuRect/GameMenuPanel/Continue")]
        private Button continueButton;

        [Node("HUD/GameMenuRect/GameMenuPanel/Restart")]
        private Button restartButton;

        [Node("HUD/GameMenuRect/GameMenuPanel/Quit")]
        private Button quit;

        [Node("CellContainer")]
        private CellContainer cellContainer;

        [Node("InLevelMap/Monsters")]
        private Node2D monsterSet;  // 开启YSort

        [Node("InLevelMap/Fleeting")]
        private Node2D fleeting;

        [Node("InLevelMap/MonsterSpawner")]
        private MonsterSpawner monsterSpawner;

        private List<WaveDef> waveDefs;

        private int waveIndex = -1;

        private bool spawned = false;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            bool skipCountDown = this._<SceneManager>().Data<bool>(0);
            this.waveDefs = this._<SceneManager>().Data<List<WaveDef>>(1);

            menu.Pressed += OnMenuPressed;
            continueButton.Pressed += OnContinuePressed;
            quit.Pressed += OnQuitPressed;
            restartButton.Pressed += OnRestartPressed;
            cellContainer.CellPressed += OnCellContainerCellPressed;
            monsterSpawner.SpwanFinished += () => spawned = true;

            StartCountDown(skipCountDown);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (spawned)
            {
                List<Monster> monsters = monsterSet.GetChildren<Monster>();
                if (monsters == null || monsters.Count == 0)
                {
                    spawned = false;
                    SpawnMonsters();
                }
            }
        }

        private void OnMenuPressed()
        {
            if (gameMenuRect == null)
            {
                return;
            }
            gameMenuRect.Show();
        }

        private void OnContinuePressed()
        {
            if (gameMenuRect == null)
            {
                return;
            }
            gameMenuRect.Hide();
        }

        private void OnQuitPressed()
        {
            this._<SceneManager>().ChangeScene($"res://scene/adventure/{themeCode.ToLower()}/{themeCode}Scene.tscn", levelIndex);
        }

        private void OnRestartPressed()
        {
            this._<SceneManager>().ChangeScene($"res://scene/game/{themeCode}/Level{levelIndex}.tscn", true, this.waveDefs);
        }

        private void OnCellContainerCellPressed(Vector2 center)
        {
            PackedScene packedScene = GD.Load<PackedScene>("res://scene/game/SelectFault.tscn");
            SelectFault selectFault = packedScene.InstantiateOrNull<SelectFault>();
            if (selectFault == null)
            {
                return;
            }
            selectFault.Position = center;
            fleeting.AddChild(selectFault);
        }

        private void StartCountDown(bool skipCountDown = false)
        {
            PackedScene packedScene = GD.Load<PackedScene>("res://scene/game/CountDown.tscn");
            CountDown countDown = packedScene.InstantiateOrNull<CountDown>();
            if (countDown == null)
            {
                return;
            }
            countDown.Finished += OnCountDownFinished;
            if (skipCountDown)
            {
                countDown.SkipCountDown();
            }
            hud.AddChild(countDown);
        }

        protected void OnCountDownFinished()
        {
            SpawnMonsters();
        }

        private void SpawnMonsters()
        {
            if (waveDefs == null || waveDefs.Count == 0)
            {
                return;
            }
            ++waveIndex;
            if (waveIndex < 0 || waveIndex >= waveDefs.Count)
            {
                return;
            }
            monsterSpawner.Spawn(waveDefs[waveIndex]);
        }
    }
}
