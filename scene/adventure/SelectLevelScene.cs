using CarrotFantasy.autoload;
using CarrotFantasy.def;
using Godot;
using Godot.Collections;
using GodotUtilities;
using Newtonsoft.Json;

namespace CarrotFantasy.scene.adventure
{
    public partial class SelectLevelScene : Scene
    {
        [Node("MenuBar/Back")]
        private Button back;

        [Node("MenuBar/Help")]
        private Button help;

        [Node("Levels")]
        private Node2D levels;

        [Node("TotalWaves")]
        private Sprite2D totalWaves;

        [Node("AvailableTowers")]
        private Node2D availableTowers;

        [Node("Start")]
        private Button start;

        [Node("Style")]
        private Sprite2D style;

        [Export(PropertyHint.File, "*.tscn")]
        protected string backScene = "res://scene/adventure/AdventureScene.tscn";

        [Export(PropertyHint.File, "theme.json")]
        protected string themeData;

        [Export]
        protected int levelCoverPageInterval = 414;

        protected int currentLevelIndex = 1;

        public static readonly Vector2 LevelCoverPagePosition = new Vector2(384f, 232f);

        private PackedScene coverPagePackedScene;

        private ThemeDef themeDef;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            coverPagePackedScene = GD.Load<PackedScene>("res://scene/adventure/Level.tscn");

            back.Pressed += OnBackPressed;
            start.Pressed += OnStartPressed;
            themeDef = LoadThemeDef();
            LoadTheme();
            SwitchLevel(SceneManager.Instance.Data<int>(0));
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if (@event.IsActionPressed("ui_left"))
            {
                SwitchLevel(currentLevelIndex - 1);
            }
            if (@event.IsActionPressed("ui_right"))
            {
                SwitchLevel(currentLevelIndex + 1);
            }
        }

        protected virtual void OnBackPressed()
        {
            SceneManager.Instance.ChangeScene(backScene, GetThemeCode());
        }

        protected virtual ThemeDef LoadThemeDef()
        {
            ThemeDef themeDef = null;

            if (FileAccess.FileExists(themeData))
            {
                FileAccess fileAccess = FileAccess.Open(themeData, FileAccess.ModeFlags.Read);
                try
                {
                    string json = fileAccess.GetAsText();
                    themeDef = JsonConvert.DeserializeObject<ThemeDef>(json);

                    if (themeDef != null && themeDef.Levels != null)
                    {
                        foreach (var level in themeDef.Levels)
                        {
                            if (level == null)
                            {
                                continue;
                            }
                            level.ThemeCode = themeDef.Code;
                        }
                    }
                }
                finally
                {
                    fileAccess = null;
                }
            }

            return themeDef;
        }

        protected virtual void LoadTheme()
        {
            LoadLevels();
            SwitchLevelAnimation();
            LoadTotalWaves();
            LoadAvailableTowers();
            LoadStyle();
        }

        private void SwitchLevel(int index)
        {
            int old = currentLevelIndex;
            currentLevelIndex = index;

            if (currentLevelIndex < 1)
            {
                currentLevelIndex = 1;
            }

            if (currentLevelIndex >= themeDef.Levels.Count)
            {
                currentLevelIndex = themeDef.Levels.Count;
            }

            if (old == currentLevelIndex)
            {
                return;
            }

            SwitchLevelAnimation();
        }

        private void UpdateLevelDisplay()
        {
            LoadTotalWaves();
            LoadAvailableTowers();
            UpdateLevelCoverPageShadow();
            UpdateStartButtonStatus();
        }

        protected virtual void LoadLevels()
        {
            if (themeDef == null)
            {
                return;
            }

            if (themeDef.Levels == null || themeDef.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= themeDef.Levels.Count)
            {
                return;
            }

            if (levels == null)
            {
                return;
            }

            if (coverPagePackedScene == null)
            {
                return;
            }

            Array<Node> levelList = levels?.GetChildren();
            if (levelList != null && levelList.Count != 0)
            {
                foreach (var level in levelList)
                {
                    level.QueueFree();
                }
                levelList.Clear();
            }

            int count = 0;
            foreach (var levelDef in themeDef.Levels)
            {
                if (levelDef == null)
                {
                    continue;
                }
                Level level = coverPagePackedScene.Instantiate<Level>();
                level.LevelDef = levelDef;
                level.Position = LevelCoverPagePosition + new Vector2(levelCoverPageInterval * count, 0);
                level.GrayingOrLightUp(currentLevelIndex);
                levels.AddChild(level);
                if (level.LevelCoverPage != null)
                {
                    level.LevelCoverPage.BeSelected += SwitchLevel;
                }

                count++;
            }
        }

        protected void SwitchLevelAnimation()
        {
            Tween tween = CreateTween();
            tween.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Linear);
            tween.TweenProperty(levels, (string)Node2D.PropertyName.Position, new Vector2(-(currentLevelIndex - 1) * levelCoverPageInterval, 0), 0.2);
            tween.TweenCallback(Callable.From(UpdateLevelDisplay));
        }

        protected virtual void LoadTotalWaves()
        {
            if (themeDef == null)
            {
                return;
            }

            if (themeDef.Levels == null || themeDef.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= themeDef.Levels.Count)
            {
                return;
            }

            if (totalWaves == null)
            {
                return;
            }

            LevelDef levelPO = themeDef.Levels[currentLevelIndex - 1];
            if (levelPO == null)
            {
                return;
            }

            totalWaves.Texture = GD.Load<Texture2D>($"res://resource/adventure/totalWaves/TotalWaves{levelPO.TotalWaves}.tres");
        }

        protected virtual void LoadAvailableTowers()
        {
            if (themeDef == null)
            {
                return;
            }

            if (themeDef.Levels == null || themeDef.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= themeDef.Levels.Count)
            {
                return;
            }

            if (availableTowers == null)
            {
                return;
            }

            Array<Node> towers = availableTowers?.GetChildren();
            if (towers != null && towers.Count != 0)
            {
                foreach (var tower in towers)
                {
                    tower.QueueFree();
                }
                towers.Clear();
            }

            LevelDef levelDef = themeDef.Levels[currentLevelIndex - 1];
            if (levelDef == null || levelDef.AvailableTowers == null || levelDef.AvailableTowers.Count == 0)
            {
                return;
            }
            int count = 0;
            float offset = -(float)(levelDef.AvailableTowers.Count + 1) / 2f + 1;
            foreach (var availableTower in levelDef.AvailableTowers)
            {
                Sprite2D towerSprite = new();
                Texture2D towerTexture = null;
                if (!string.IsNullOrEmpty(availableTower.Code))
                {
                    towerTexture = GD.Load<Texture2D>($"res://resource/adventure/availableTowers/{availableTower.Code}.tres");
                    towerSprite.Name = availableTower.Code;
                }
                towerSprite.Texture = towerTexture;
                towerSprite.Position = new Vector2(48f * (offset + (float)count), 0f);

                availableTowers.AddChild(towerSprite);

                count++;
            }
        }

        protected virtual void LoadStyle()
        {
            if (themeDef == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(themeDef.Code))
            {
                return;
            }

            if (style == null)
            {
                return;
            }

            style.Texture = GD.Load<Texture2D>($"res://resource/adventure/{themeDef.Code}/Style.tres");
        }

        private void UpdateLevelCoverPageShadow()
        {
            int index = currentLevelIndex;
            foreach (var item in levels.GetChildren())
            {
                if (item is Level level)
                {
                    level.GrayingOrLightUp(index);
                }
            }
        }

        private void UpdateStartButtonStatus()
        {
            if (themeDef == null)
            {
                return;
            }

            if (themeDef.Levels == null || themeDef.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= themeDef.Levels.Count)
            {
                return;
            }

            LevelDef level = themeDef.Levels[currentLevelIndex - 1];
            if (level == null)
            {
                return;
            }

            if (start == null)
            {
                return;
            }

            start.Disabled = !level.IsUnlocked;
        }

        public string GetThemeCode()
        {
            if (themeDef == null)
            {
                return string.Empty;
            }
            return themeDef.Code;
        }

        private void OnStartPressed()
        {
            if (themeDef == null)
            {
                return;
            }
            string themeCode = themeDef.Code;
            SceneManager.Instance.ChangeScene(
                $"res://scene/game/{themeCode}/Level{currentLevelIndex}.tscn",
                false,
                themeDef.Levels[currentLevelIndex - 1].Waves,
                themeDef.Levels[currentLevelIndex - 1].AvailableTowers
            );
        }
    }
}
