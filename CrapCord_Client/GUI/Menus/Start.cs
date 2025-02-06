using ImGuiNET;
using static CrapCord_Client.GUI.Renderer;
using Environment = System.Environment;

namespace CrapCord_Client.GUI.Menus;

public class Start(ContextManager ctx) : Menu(ctx) {
    override public int priority => (int)e_Menus.Start;

    public override void Render() {
        ImGui.SeparatorText("CrapCord");
        
        if (GUI.FullWidthButton("Register"))
            return;
        
        if (GUI.FullWidthButton("Log in"))
            return;
        
        if (GUI.FullWidthButton("Quit"))
            Environment.Exit(0);
    }

    public override float GetMenuHeight() {
        return 125;
    }
}