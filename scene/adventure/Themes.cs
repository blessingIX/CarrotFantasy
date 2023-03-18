using Godot;
using Godot.Collections;
using GodotUtilities;
using System;

namespace CarrotFantasy.scene.adventure
{
    public partial class Themes : Node2D
    {
        [Node("../PageTurning")]
        private PageTurning pageTurning;

        private Array<Node> nodes = new();

        private int pageIndex = 0;

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            nodes = GetChildren();
            pageTurning.PrevPressed += OnPageTurningPrevPressed;
            pageTurning.NextPressed += OnPageTurningNextPressed;
            pageTurning.RefreshPointContainer(PageCount(), pageIndex);
        }

        private void OnPageTurningPrevPressed()
        {
            int past = pageIndex;
            int future = Math.Max(past - 1, 0);
            pageIndex = future;
            if (future != past)
            {
                SwitchTheme(pageIndex);
                pageTurning.RefreshPointContainer(PageCount(), pageIndex);
            }
        }

        private void OnPageTurningNextPressed()
        {
            int pageCount = PageCount();
            int past = pageIndex;
            int future = Math.Min(pageIndex + 1, pageCount - 1);
            pageIndex = future;
            if (future != past)
            {
                SwitchTheme(pageIndex);
                pageTurning.RefreshPointContainer(PageCount(), pageIndex);
            }
        }

        private int PageCount()
        {
            int nodesCount = nodes.Count;
            return nodes == null ? 0 : nodesCount;
        }

        private void SwitchTheme(int pageIndex)
        {
            Tween tween = CreateTween();
            tween.SetTrans(Tween.TransitionType.Linear);
            tween.TweenProperty(this, (string)PropertyName.Position, new Vector2(768f * -pageIndex, Position.Y), 0.3);
        }
    }
}
