using Godot;

namespace CarrotFantasy.scene
{
    public partial class TransitionScene : CanvasLayer
    {
        // TransitionScene从继承自Sprite2D修改为继承自CanvasLayer
        // 将layer属性设置较大值使其覆盖在游戏场景之上，并利用Control节点(ColorRect)的mouse_filter属性Stop的特性阻止鼠标事件往下传播
    }
}
