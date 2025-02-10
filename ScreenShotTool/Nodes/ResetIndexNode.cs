using BehaviorTree;

namespace ScreenShot.Nodes;
public class ResetIndexNode : IBTnode
{
    protected override BTresult MyRun(TickData p_data) {
        ScreenShot.Preferences.StartIndex = 0;
        ScreenShot.Preferences.EndIndex = 168;
        return BTresult.Success;
    }
}
