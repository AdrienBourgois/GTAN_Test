using System.Collections.Generic;
using System.IO;
using GTANetworkServer;

public class RadioManager : Script
{
    private bool running = true;
    public Dictionary<string, Radio> Radios { get; private set; }

    public RadioManager()
    {
        API.onResourceStart += OnResourceStart;
        API.onResourceStop += OnResourceStop;
        API.onClientEventTrigger += OnClientEventTrigger;
        Radios = new Dictionary<string, Radio>();
    }

    public void OnResourceStart()
    {
        DirectoryInfo musicsFolder = new DirectoryInfo("resources\\Radio\\musics\\");
        foreach (DirectoryInfo folder in musicsFolder.GetDirectories())
        {
            Radio currentRadio = new Radio(folder.Name);
            if (!currentRadio.IsValid) continue;
            Radios.Add(folder.Name, currentRadio);
            API.consoleOutput("Radio " + currentRadio.Name + " : " + string.Join(",", currentRadio.TrackList));
            API.consoleOutput("ON AIR Radio " + Radios[folder.Name].Name + " -> " + Radios[folder.Name].CurrentTrack.FileName);
        }

        API.startThread(Update);
    }

    public void OnResourceStop()
    {
        running = false;
    }

    private void Update()
    {
        while (running)
        {
            foreach (Radio radio in Radios.Values)
            {
                radio.Update();
                if (!radio.MustTriggerClients) continue;
                API.triggerClientEventForAll("NextTrack", radio.Name, radio.CurrentTrack.FileName);
                radio.ClientsTriggered();
            }
        }
    }

    private void OnClientEventTrigger(Client player, string eventName, params object[] arguments)
    {
        switch (eventName)
        {
            case "GetRadioList":
                API.triggerClientEvent(player, "SetRadioList", string.Join(",", Radios.Keys));
                break;
            case "GetRadioInfos":
                if (Radios.ContainsKey((string) arguments[0]))
                {
                    Track track = Radios[(string) arguments[0]].CurrentTrack;
                    API.triggerClientEvent(player, "SetRadioInfos", track.FileName, track.PlayTime.TotalSeconds);
                }
                else
                    API.triggerClientEvent(player, "ErrorRadioName");
                break;
            default:
                API.consoleOutput("Unknow Event !");
                break;
        }
    }

    [Command("radio", GreedyArg = true)]
    public void LogCurrentTrack(Client player, string radioName)
    {
        if (Radios.ContainsKey(radioName))
        {
            Radio radio = Radios[radioName];
            API.sendChatMessageToPlayer(player, "ON AIR Radio " + radio.Name + " -> " + radio.CurrentTrack.FileName + " - " + radio.CurrentTrack.PlayTime + "/" + radio.CurrentTrack.Length);
        }
        else
        {
            API.sendChatMessageToPlayer(player, "Error in radio name !");
        }
    }
}