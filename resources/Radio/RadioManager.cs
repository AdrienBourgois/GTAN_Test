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
            Radios.Add(folder.Name, currentRadio);
            API.consoleOutput("Radio " + currentRadio.Name + " : " + string.Join(",", currentRadio.TrackList));
        }

        API.consoleOutput("ON AIR Radio " + Radios["Electro"].Name + " -> " + Radios["Electro"].CurrentTrack.FileName);

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
                Track track = Radios[(string) arguments[0]].CurrentTrack;
                API.triggerClientEvent(player, "SetRadioInfos", track.FileName, track.PlayTime.TotalSeconds);
                break;
            default:
                API.consoleOutput("Unknow Event !");
                break;
        }
    }

    [Command("radio", GreedyArg = true)]
    public void LogCurrentTrack(Client player, string radio)
    {
        API.consoleOutput("ON AIR Radio " + Radios[radio].Name + " -> " + Radios[radio].CurrentTrack.FileName + " - " + Radios[radio].CurrentTrack.PlayTime + "/" + Radios[radio].CurrentTrack.Length);
    }
}