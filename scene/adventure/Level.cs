using CarrotFantasy.def;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class Level : Node2D
    {
        [Node("LevelCoverPage")]
        private LevelCoverPage levelCoverPage;
        public LevelCoverPage LevelCoverPage
        {
            get => levelCoverPage;
        }

        [Node("LevelCoverPage/LevelLock")]
        private Sprite2D levelLock;

        private LevelDef levelDef;
        public LevelDef LevelDef
        {
            set => levelDef = value;
            get => levelDef;
        }

        private readonly Color lightUpColor = new Color("#ffffff");

        private readonly Color grayingColor = new Color("#525b6b");

        private bool beSelected = false;
        public bool BeSelected
        {
            get { return beSelected; }
        }

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            if (levelDef != null)
            {
                levelCoverPage.Texture = GD.Load<Texture2D>($"res://resource/adventure/{levelDef.ThemeCode}/level{levelDef.Index}/LevelCoverPage.tres");

                levelLock.Texture = GD.Load<Texture2D>("res://resource/adventure/LevelLock.tres");
            }

            if (levelLock != null && levelDef != null)
            {
                levelLock.Visible = !levelDef.IsUnlocked;
            }
        }

        public bool Selected(int index)
        {
            return GetLevelIndex() == index;
        }

        public void GrayingOrLightUp(int index)
        {
            beSelected = Selected(index);
            Modulate = beSelected ? lightUpColor : grayingColor;
        }

        public int GetLevelIndex()
        {
            return levelDef != null ? levelDef.Index : 0;
        }
    }
}
