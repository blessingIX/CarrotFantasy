using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.common
{
    public partial class ButtonWithAudio : Button
    {
        [Export(PropertyHint.File, "*.ogg,*.mp3,*.wav")]
        protected string audioPath;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            this.Pressed += () => this._<SoundManager>().PlayFleeting(audioPath);
        }
    }
}
