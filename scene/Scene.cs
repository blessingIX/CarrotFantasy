using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene
{
    public partial class Scene : Sprite2D
    {
        [Export(PropertyHint.File, "*.ogg,*.mp3,*.wav")]
        protected string BGM;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            PlayBGM();
        }

        protected void PlayBGM()
        {
            if (string.IsNullOrEmpty(BGM))
            {
                this._<SoundManager>().Stop();
                return;
            }

            this._<SoundManager>().Play(BGM);
        }
    }
}
