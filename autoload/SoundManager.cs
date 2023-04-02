using Godot;
using GodotUtilities;

namespace CarrotFantasy.autoload
{
    public partial class SoundManager : Node
    {
        [Node("BGMPlayer")]
        private AudioStreamPlayer BGMPlayer;

        [Node("Fleeting")]
        private Node fleeting;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            BGMPlayer.Finished += ReplayBGM;
        }

        public void PlayBGM(string path)
        {
            AudioStream currentStream = BGMPlayer.Stream;
            if (currentStream != null && currentStream.ResourcePath == path && BGMPlayer.Playing)
            {
                return;
            }

            AudioStream audioStream = GD.Load<AudioStream>(path);
            BGMPlayer.Stop();
            BGMPlayer.Stream = audioStream;
            BGMPlayer.Play();
        }

        public void ReplayBGM()
        {
            AudioStream currentStream = BGMPlayer.Stream;
            if (currentStream == null)
            {
                return;
            }
            BGMPlayer.Play();
        }

        public void StopBGM()
        {
            BGMPlayer.Stop();
        }

        public void PlayFleeting(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            AudioStream audioStream = GD.Load<AudioStream>(path);
            if (audioStream == null)
            {
                return;
            }
            AudioStreamPlayer fleetingPlayer = new AudioStreamPlayer();
            fleetingPlayer.Finished += () => fleetingPlayer.QueueFree();

            fleetingPlayer.Stream = audioStream;
            fleetingPlayer.Autoplay = true;
            fleeting.AddChildDeferred(fleetingPlayer);
        }
    }
}
