using HarmonyLib;
using System.Collections.Generic;
using System.Text;

namespace MinecraftFormatting.Patches
{
    [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.AddTextToChatOnServer))]
    public class SendChat
    {
        private static readonly Dictionary<char, string> colors = new()
        {
            { '0', "<color=#000000>" },
            { '1', "<color=#0000AA>" },
            { '2', "<color=#00AA00>" },
            { '3', "<color=#00AAAA>" },
            { '4', "<color=#AA0000>" },
            { '5', "<color=#AA00AA>" },
            { '6', "<color=#FFAA00>" },
            { '7', "<color=#AAAAAA>" },
            { '8', "<color=#555555>" },
            { '9', "<color=#5555FF>" },
            { 'a', "<color=#55FF55>" },
            { 'b', "<color=#55FFFF>" },
            { 'c', "<color=#FF5555>" },
            { 'd', "<color=#FF55FF>" },
            { 'e', "<color=#FFFF55>" },
            { 'f', "<color=#FFFFFF>" }
        };

        private static readonly Dictionary<char, string> formats = new()
        {
            { 'l', "<b>" },
            { 'm', "<s>" },
            { 'n', "<u>" },
            { 'o', "<i>" },
            { 'r', "<color=#FFFF00>" }
        };

        private static void Prefix(ref string chatMessage, int playerId)
        {
            // TODO: add system message support
            if (playerId != -1)
                chatMessage = FormatText(chatMessage, playerId);
        }

        private static string FormatText(string chatMessage, int playerId)
        {
            List<char> usedFormats = new();
            StringBuilder sb = new();
            sb.Append(playerId == -1 ? "<color=#7069ff>" : "<color=#FFFF00>");
            for (int i = 0; i < chatMessage.Length; i++)
            {
                bool hasFormatting = false;
                if (chatMessage[i] == '&')
                {
                    if (i + 1 == chatMessage.Length)
                        break;
                    // reset has highest priority
                    if (chatMessage[i + 1] == 'r')
                    {
                        sb.Append("</color>");
                        sb.Append(formats['r']);
                        foreach (char c in usedFormats)
                        {
                            sb.Append("</");
                            sb.Append(c);
                            sb.Append('>');
                        }
                        usedFormats.Clear();
                        i += 2;
                        hasFormatting = true;
                    }
                    // formats have second highest
                    else if (formats.ContainsKey(chatMessage[i + 1])) {
                        usedFormats.Add(formats[chatMessage[i + 1]][1]);
                        sb.Append(formats[chatMessage[i + 1]]);
                        i += 2;
                        hasFormatting = true;
                    }
                    // colors have least priority
                    else if (colors.ContainsKey(chatMessage[i + 1])) {
                        sb.Append("</color>");
                        sb.Append(colors[chatMessage[i + 1]]);
                        i += 2;
                        hasFormatting = true;
                    }
                }
                if (i >= chatMessage.Length)
                    break;
                // if there are multiple formatters together, move backward and parse it
                // TODO: spamming a bunch of & symbols will remove the last one. is this even worth fixing?
                if (chatMessage[i] == '&' && hasFormatting)
                    i--;
                else
                    sb.Append(chatMessage[i]);
            }
            foreach (char c in usedFormats)
            {
                sb.Append("</");
                sb.Append(c);
                sb.Append('>');
            }
            sb.Append("</color>");
            return sb.ToString();
        }
    }
}
