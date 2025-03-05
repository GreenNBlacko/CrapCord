using System.Text.RegularExpressions;
using CrapCord_Services;
using ImGuiNET;
using static CrapCord_Client.GUI.Renderer;
using Color = System.Drawing.Color;

namespace CrapCord_Client.GUI.Menus;

public class Register(ContextManager ctx) : Menu(ctx) {
    override public int priority => (int)e_Menus.Register;

    private string Username = string.Empty;
    private string Email = string.Empty;
    private string Password = string.Empty;
    private string Passphrase = string.Empty;

    public override void Render() {
        ImGui.SeparatorText("Register");
        
        ImGui.InputText("Username", ref Username, 32);
        ImGui.InputText("Email", ref Email, 48);
        ImGui.InputText("Password", ref Password, 32, ImGuiInputTextFlags.Password);
        ImGui.SeparatorText("Encryption key generation");
        ImGui.TextWrapped("Enter the passphrase to act as your encryption key seed. Don't forget to keep it safe with you, as there is no other way to restore the keys without the seed or even transfer them between devices");
        GUI.SpaceY(5);
        ImGui.InputText("Passphrase", ref Passphrase, 64, ImGuiInputTextFlags.Password);

        if (Username == string.Empty) {
            ImGui.TextColored(Color.Red.ToVector(), "Username cannot be empty.");
            return;
        }
        
        if (Email == string.Empty) {
            ImGui.TextColored(Color.Red.ToVector(), "Email cannot be empty.");
            return;
        }
        
        if(!Regex.IsMatch(Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")) {
            ImGui.TextColored(Color.Red.ToVector(), "Please enter a valid email address.");
            return;
        }
        
        if (Password == string.Empty) {
            ImGui.TextColored(Color.Red.ToVector(), "Password cannot be empty.");
            return;
        }
        
        if(!Regex.IsMatch(Password, @"^([a-zA-Z]|[0-9]|[\!\@\#\$\%\^\&\*\(\)\\\\[\]\=\+\-_\']){8,32}$")) {
            ImGui.TextColored(Color.Red.ToVector(), "Password needs to be at least 8 characters long(max 32 characters).");
            return;
        }
        
        if(!Regex.IsMatch(Password, @"[a-zA-Z]+")) {
            ImGui.TextColored(Color.Red.ToVector(), "Password needs to contain at least one letter.");
            return;
        }
        
        if(!Regex.IsMatch(Password, @"[0-9]+")) {
            ImGui.TextColored(Color.Red.ToVector(), "Password needs to contain at least one number");
            return;
        }
        
        if(!Regex.IsMatch(Password, @"[\!\@\#\$\%\^\&\*\(\)\\\\[\]\=\+\-_\']+")) {
            ImGui.TextColored(Color.Red.ToVector(), "Password needs to contain at least one special character.");
            return;
        }

        if (Passphrase == string.Empty) {
            ImGui.TextColored(Color.Red.ToVector(), "Passphrase cannot be empty.");
            return;
        }

        if (Passphrase == Password) {
            ImGui.TextColored(Color.Red.ToVector(), "Passphrase cannot match password.");
            return;
        }
        
        ImGui.Separator();

        var selection = GUI.ButtonList(["Back", "Register"]) ?? -1;

        if (selection == 1) { // Register
            ctx.rsa.GenerateRSAKeypair(Passphrase);
            Console.WriteLine(ctx.hardwareInfo.CpuId+ " " + ctx.hardwareInfo.MotherboardSerial);
            
            // Handle database entry
            // ctx.client.
        }
        
        if (selection == 0)
            ctx.renderer.LoadMenu(e_Menus.Start);
    }

    public override float GetMenuHeight() {
        return 285;
    }
}