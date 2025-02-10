using BehaviorTree;
using ScreenShotTool.Menu;

namespace ScreenShotTool.Nodes;
public class ResetRangeNode : IBTnode
{
    protected override BTresult MyRun(TickData p_data) {
        SliderStartIndex.SetIndex(0);
        SliderEndIndex.SetIndex(168);
        return BTresult.Success;
    }
}
