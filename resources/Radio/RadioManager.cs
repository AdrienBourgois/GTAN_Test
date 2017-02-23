using System;
using System.Globalization;
using GTANetworkServer;

public class RadioManager : Script
{
    private DateTime trackStartTime;
    private TimeSpan currentMusicTimer;
    private TimeSpan currentMusicLength;
    private bool running = true;

    public RadioManager()
    {
        API.onResourceStart += OnResourceStart;
        API.onResourceStop += OnResourceStop;
        API.onClientEventTrigger += OnClientEventTrigger;
    }

    public void OnResourceStart()
    {
        trackStartTime = DateTime.Now;
        API.consoleOutput("[RadioManager] RadioManager timers start at : " + trackStartTime.TimeOfDay);
        NextTrack("Axero.mp3");
        API.startThread(UpdateTimers);
    }

    public void OnResourceStop()
    {
        running = false;
    }

    private void OnClientEventTrigger(Client player, string eventName, params object[] arguments)
    {
        switch (eventName)
        {
            case "GetRadioTimer":
                TimeSpan curentMusicTime = DateTime.Now.Subtract(trackStartTime);
                API.consoleOutput("Start track at : " + curentMusicTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                API.triggerClientEvent(player, "SetRadioTimer", curentMusicTime.TotalSeconds);
                break;
            default:
                API.consoleOutput("Unknow Event !");
                break;
        }
    }

    private void NextTrack(string track)
    {
        currentMusicLength = GetMusicLength(track);
    }

    private TimeSpan GetMusicLength(string track)
    {
        TagLib.File f = TagLib.File.Create(API.getResourceFolder() + "/musics/" + track, TagLib.ReadStyle.Average);
        TimeSpan length = f.Properties.Duration;
        API.consoleOutput("Track " + track + " : " + length.Minutes + "." + length.Seconds + "." + length.Milliseconds);
        return f.Properties.Duration;
    }

    private void UpdateTimers()
    {
        while (running)
        {
            /*TimeSpan delta = DateTime.Now.Subtract(trackStartTime);
            currentMusicTimer = currentMusicTimer.Add(delta);
            trackStartTime = DateTime.Now;*/
            //API.consoleOutput("Left : " + currentMusicTimerLeft.Minutes + "." + currentMusicTimerLeft.Seconds + "." + currentMusicTimerLeft.Milliseconds);
        }
    }
}