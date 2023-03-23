using Godot;
using GodotUtilities;

namespace CarrotFantasy.common
{
    public partial class ButtonWithAudio : Button
    {
        [Node("AudioStreamPlayer")]
        protected AudioStreamPlayer audioStreamPlayer;

        [Export(PropertyHint.File, "*.ogg,*.mp3,*.wav")]
        protected string audioPath;

        protected AudioStream audioStream;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            audioStream = GD.Load<AudioStream>(audioPath);
            this.Pressed += Play;
        }

        protected virtual void Play()
        {
            if (audioStream == null)
            {
                return;
            }

            if (audioStreamPlayer == null)
            {
                return;
            }

            audioStreamPlayer.Stream = audioStream;
            audioStreamPlayer.Play();
        }
    }
}
