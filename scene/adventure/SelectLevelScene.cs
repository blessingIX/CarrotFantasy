using CarrotFantasy.autoload;
using CarrotFantasy.po;
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

        private ThemePO theme;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            coverPagePackedScene = GD.Load<PackedScene>("res://scene/adventure/Level.tscn");

            back.Pressed += Back;
            theme = LoadThemeData();
            LoadTheme();
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

        protected virtual void Back()
        {
            this._<SceneManager>().ChangeScene(backScene, Variant.From(GetThemeCode()));
        }

        protected virtual ThemePO LoadThemeData()
        {
            ThemePO themePO = null;

            if (FileAccess.FileExists(themeData))
            {
                FileAccess fileAccess = FileAccess.Open(themeData, FileAccess.ModeFlags.Read);
                try
                {
                    string json = fileAccess.GetAsText();
                    themePO = JsonConvert.DeserializeObject<ThemePO>(json);

                    if (themePO != null && themePO.Levels != null)
                    {
                        foreach (var level in themePO.Levels)
                        {
                            if (level == null)
                            {
                                continue;
                            }
                            level.ThemeCode = themePO.Code;
                        }
                    }
                }
                finally
                {
                    fileAccess = null;
                }
            }

            return themePO;
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

            if (currentLevelIndex >= theme.Levels.Count)
            {
                currentLevelIndex = theme.Levels.Count;
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
            if (theme == null)
            {
                return;
            }

            if (theme.Levels == null || theme.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= theme.Levels.Count)
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
            foreach (var levelPO in theme.Levels)
            {
                if (levelPO == null)
                {
                    continue;
                }
                Level level = coverPagePackedScene.Instantiate<Level>();
                level.LevelData = levelPO;
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
            if (theme == null)
            {
                return;
            }

            if (theme.Levels == null || theme.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= theme.Levels.Count)
            {
                return;
            }

            if (totalWaves == null)
            {
                return;
            }

            LevelPO levelPO = theme.Levels[currentLevelIndex - 1];
            if (levelPO == null)
            {
                return;
            }

            totalWaves.Texture = GD.Load<Texture2D>($"res://resource/adventure/totalWaves/TotalWaves{levelPO.TotalWaves}.tres");
        }

        protected virtual void LoadAvailableTowers()
        {
            if (theme == null)
            {
                return;
            }

            if (theme.Levels == null || theme.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= theme.Levels.Count)
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

            LevelPO levelPO = theme.Levels[currentLevelIndex - 1];
            if (levelPO == null || levelPO.AvailableTowers == null || levelPO.AvailableTowers.Count == 0)
            {
                return;
            }
            int count = 0;
            float offset = -(float)(levelPO.AvailableTowers.Count + 1) / 2f + 1;
            foreach (var availableTower in levelPO.AvailableTowers)
            {
                Sprite2D towerSprite = new();
                Texture2D towerTexture = null;
                if (!string.IsNullOrEmpty(availableTower))
                {
                    towerTexture = GD.Load<Texture2D>($"res://resource/adventure/availableTowers/{availableTower}.tres");
                    towerSprite.Name = availableTower;
                }
                towerSprite.Texture = towerTexture;
                towerSprite.Position = new Vector2(48f * (offset + (float)count), 0f);

                availableTowers.AddChild(towerSprite);

                count++;
            }
        }

        protected virtual void LoadStyle()
        {
            if (theme == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(theme.Code))
            {
                return;
            }

            if (style == null)
            {
                return;
            }

            style.Texture = GD.Load<Texture2D>($"res://resource/adventure/{theme.Code}/Style.tres");
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
            if (theme == null)
            {
                return;
            }

            if (theme.Levels == null || theme.Levels.Count == 0)
            {
                return;
            }

            if (currentLevelIndex < 1 && currentLevelIndex >= theme.Levels.Count)
            {
                return;
            }

            LevelPO level = theme.Levels[currentLevelIndex - 1];
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
            if (theme == null)
            {
                return string.Empty;
            }
            return theme.Code;
        }
    }
}
