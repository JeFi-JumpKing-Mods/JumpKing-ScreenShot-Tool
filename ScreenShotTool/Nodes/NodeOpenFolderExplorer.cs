using BehaviorTree;
using JumpKing;
using System.Diagnostics;
using System.IO;

namespace ScreenShotTool.Nodes;
public class NodeOpenFolderExplorer : IBTnode
{
    private string folderPath = "";
    public  NodeOpenFolderExplorer(string p_folderPath) {
        folderPath = p_folderPath;
    }
    protected override BTresult MyRun(TickData p_data)
    {
        if (Directory.Exists(folderPath)) {
            Process.Start("explorer.exe", folderPath);
        }
        Game1.instance.contentManager.audio.menu.Select.Play();
        return BTresult.Success;
    }
}
