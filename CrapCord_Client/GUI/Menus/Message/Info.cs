﻿using ImGuiNET;

namespace CrapCord_Client.GUI.Menus.Message;

public class Info(string message) : Message(message) {
    public override void Render() {
        ImGui.Begin("Info", flags);

        GUI.ctx.window.Title = "Notice";

        ImGui.SetWindowFontScale(1.3f);

        GUI.CenteredWrappedText(_message, 5);

        GUI.SpaceY(5);

        ImGui.Separator();

        if (GUI.FullWidthButton("OK")) acknowledged = true;

        ImGui.End();
    }
}