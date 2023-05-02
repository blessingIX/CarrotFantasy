using System.Collections.Generic;
using CarrotFantasy.autoload;
using CarrotFantasy.common;
using CarrotFantasy.def;
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

        [Node("TowerPlacers")]
        private Node2D towerPlacers;

        private List<TowerDef> availableTowers;
        public List<TowerDef> AvailableTowers
        {
            get => this.availableTowers;
            set
            {
                this.availableTowers = value;
                if (this.availableTowers == null || this.availableTowers.Count == 0)
                {
                    QueueFreeTowers();
                    return;
                }

                QueueFreeTowers();
                List<Node2D> towerNodes = new();
                Vector2 stepVector2 = GameConstant.Global.CellSize * new Vector2(1.3f, 0f);
                Vector2 v = stepVector2 * new Vector2(-(float)(this.availableTowers.Count - 1) / 2, 0);
                foreach (var t in this.availableTowers)
                {
                    PackedScene packedScene = GD.Load<PackedScene>("res://scene/game/TowerItem.tscn");
                    TowerItem towerItem = packedScene.InstantiateOrNull<TowerItem>();
                    towerItem.TowerCode = t.Code;
                    towerItem.TowerPrice = t.Price;
                    towerItem.PlaceTower += OnTowerPlacerPlaceTower;
                    towerPlacers.AddChild(towerItem);
                    towerItem.Position = v;
                    v += (stepVector2 * new Vector2(1f, 0f));
                }
                towerPlacers.Position = this.Position + (GameConstant.Global.CellSize * new Vector2(0, -1));
            }
        }

        /// <summary>
        /// 覆盖CanvasItem的Visible来增加播放动画与声音的逻辑
        /// </summary>
        public new bool Visible
        {
            set
            {
                this.Interaction.Visible = value;
                this.animatedSprite2D.Visible = value;
                if (this.Visible)
                {
                    animatedSprite2D.Play();
                    SoundManager.Instance.PlayFleeting(selectSoundPath);
                    TowerPlacersFadeIn();
                }
                else
                {
                    animatedSprite2D.Stop();
                    SoundManager.Instance.PlayFleeting(deselectSoundPath);
                    TowerPlacersFadeOut();
                }
            }
            get => this.Interaction.Visible && this.animatedSprite2D.Visible;
        }

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            this.Interaction.Visible = false;
            this.animatedSprite2D.Visible = false;
        }

        protected override void OnInteractionPressed()
        {
            base.OnInteractionPressed();
            Visible = false;
        }

        private void QueueFreeTowers()
        {
            if (this.towerPlacers == null || this.towerPlacers.GetChildCount() == 0)
            {
                return;
            }
            foreach (var i in this.towerPlacers.GetChildren<Node>())
            {
                i.QueueFree();
            }
        }

        private void OnTowerPlacerPlaceTower(string code, int price)
        {
            this.Visible = false;
        }

        private void TowerPlacersFadeIn()
        {
            if (this.towerPlacers == null || this.towerPlacers.GetChildCount() == 0)
            {
                return;
            }
            foreach (var towerPlacer in this.towerPlacers.GetChildren<TowerItem>())
            {
                towerPlacer.FadeIn();
            }
        }

        private void TowerPlacersFadeOut()
        {
            if (this.towerPlacers == null || this.towerPlacers.GetChildCount() == 0)
            {
                return;
            }
            foreach (var towerPlacer in this.towerPlacers.GetChildren<TowerItem>())
            {
                towerPlacer.FadeOut();
            }
        }
    }
}
