using System;
using System.IO;
using System.Collections.Generic;

public class Radio
{
    public string Name { get; private set; }
    public Track CurrentTrack { get; private set; }
    public List<string> TrackList { get; private set; }
    public bool MustTriggerClients { get; private set; }

    public Radio(string name)
    {
        TrackList = new List<string>();

        Name = name;

        DirectoryInfo musicsFolder = new DirectoryInfo("resources\\Radio\\musics\\" + Name);
        foreach (FileInfo file in musicsFolder.GetFiles())
        {
            TrackList.Add(file.Name);
        }

        NextTrack();
    }

    private void NextTrack()
    {
        MustTriggerClients = true;

        Random rand = new Random();
        string nextTrack = "";

        /*if (TrackList.Count >= 2)
            while(nextTrack == CurrentTrack.FileName)
                nextTrack = TrackList[rand.Next(0, TrackList.Count)];
        else*/
            nextTrack = TrackList[rand.Next(0, TrackList.Count)];

        CurrentTrack = new Track(Name, nextTrack);
    }

    public void ClientsTriggered()
    {
        MustTriggerClients = false;
    }

    public void Update()
    {
        CurrentTrack.Update();
        if(CurrentTrack.IsFinished)
            NextTrack();
    }
}