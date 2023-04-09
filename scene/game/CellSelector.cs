using CarrotFantasy.autoload;
using Godot;
using GodotUtilities;

namespace CarrotFantasy.scene.game
{
    public partial class CellSelector : Interaction2D
    {
        [Export(PropertyHint.File, "*.ogg")]
        private string selectSoundPath;

        [Export(PropertyHint.File, "*.ogg")]
        private string deselectSoundPath;

        [Node("AnimatedSprite2D")]
        private AnimatedSprite2D animatedSprite2D;

        /// <summary>
        /// 覆盖CanvasItem的Visible来增加播放动画与声音的逻辑
        /// </summary>
        public new bool Visible
        {
            set
            {
                base.Visible = value;
                if (base.Visible)
                {
                    animatedSprite2D.Play();
                    SoundManager.Instance.PlayFleeting(selectSoundPath);
                }
                else
                {
                    animatedSprite2D.Stop();
                    SoundManager.Instance.PlayFleeting(deselectSoundPath);
                }
            }
            get => base.Visible;
        }

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
        }

        protected override void OnInteractionPressed()
        {
            base.OnInteractionPressed();
            Visible = false;
        }
    }
}
