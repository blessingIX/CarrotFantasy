using CarrotFantasy.autoload;
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

        [Node("HUD/MenuBar/Menu")]
        private Button menu;

        [Node("HUD/GameMenuRect")]
        private TextureRect gameMenuRect;

        [Node("HUD/GameMenuRect/GameMenuPanel/Continue")]
        private Button continueButton;

        [Node("HUD/GameMenuRect/GameMenuPanel/Quit")]
        private Button quit;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            menu.Pressed += OnMenuPressed;
            continueButton.Pressed += OnContinuePressed;
            quit.Pressed += OnQuitPressed;
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
            this._<SceneManager>().ChangeScene($"res://scene/adventure/{themeCode.ToLower()}/{themeCode}Scene.tscn", Variant.From<int>(levelIndex));
        }
    }
}
