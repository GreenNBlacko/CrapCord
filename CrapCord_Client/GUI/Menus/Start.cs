using ImGuiNET;
using static CrapCord_Client.GUI.Renderer;
using Environment = System.Environment;

namespace CrapCord_Client.GUI.Menus;

public class Start(ContextManager ctx) : Menu(ctx) {
    override public int priority => (int)e_Menus.Start;

    public override void Render() {
        ImGui.SeparatorText("CrapCord");
        
        if (GUI.FullWidthButton("Register"))
            ctx.renderer.LoadMenu(e_Menus.Register);
        
        if (GUI.FullWidthButton("Log in"))
            ctx.renderer.LoadMenu(e_Menus.Login);
        
        if (GUI.FullWidthButton("Quit"))
            ctx.renderer.Close();
    }

    public override float GetMenuHeight() {
        return 125;
    }
}