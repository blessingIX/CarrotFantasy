using Godot;
using GodotUtilities;
using System;

namespace CarrotFantasy.autoload
{
    public partial class SoundManager : Node
    {
        [Node]
        private AudioStreamPlayer audioStreamPlayer;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        public void Play(string path)
        {
            AudioStream currentStream = audioStreamPlayer.Stream;
            if (currentStream != null && currentStream.ResourcePath == path)
            {
                return;
            }

            AudioStream audioStream = GD.Load<AudioStream>(path);
            audioStreamPlayer.Stop();
            audioStreamPlayer.Stream = audioStream;
            audioStreamPlayer.Play();
        }
    }
}