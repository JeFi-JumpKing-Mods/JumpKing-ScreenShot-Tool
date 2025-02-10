using BehaviorTree;

namespace ScreenShotTool.Nodes;
public class ResetIndexNode : IBTnode
{
    protected override BTresult MyRun(TickData p_data) {
        ScreenShotTool.Preferences.StartIndex = 0;
        ScreenShotTool.Preferences.EndIndex = 168;
        return BTresult.Success;
    }
}
