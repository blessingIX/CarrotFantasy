using CarrotFantasy.po;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.adventure
{
    public partial class Level : Node2D
    {
        [Node("LevelCoverPage")]
        private Sprite2D levelCoverPage;

        [Node("LevelCoverPage/LevelLock")]
        private Sprite2D levelLock;

        private LevelPO levelData;
        public LevelPO LevelData
        {
            set => levelData = value;
            get => levelData;
        }

        private readonly Color lightUpColor = new Color("#ffffff");

        private readonly Color grayingColor = new Color("#525b6b");

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            if (levelData != null)
            {
                levelCoverPage.Texture = GD.Load<Texture2D>($"res://resource/adventure/{levelData.ThemeCode}/level{levelData.Index}/LevelCoverPage.tres");

                levelLock.Texture = GD.Load<Texture2D>("res://resource/adventure/LevelLock.tres");
            }

            if (levelLock != null && levelData != null)
            {
                levelLock.Visible = !levelData.IsUnlocked;
            }
        }

        public void GrayingOrLightUp(int index)
        {
            if (levelData == null || levelData.Index != index)
            {
                Modulate = grayingColor;
            }
            else
            {
                Modulate = lightUpColor;
            }
        }
    }
}
