using System;

public class Track
{
    public string Name { get; private set; }
    public string Author { get; private set; }
    public string FileName { get; private set; }
    public TimeSpan Length { get; private set; }
    public TimeSpan PlayTime { get; private set; }
    public DateTime StartTime { get; private set; }
    public bool IsFinished { get; private set; }

    public Track(string radio, string file)
    {
        FileName = file;

        TagLib.File f = TagLib.File.Create("resources/Radio/musics/" + radio + "/" + FileName, TagLib.ReadStyle.Average);
        Name = f.Tag.Title;
        Author = f.Tag.FirstPerformer;
        Length = f.Properties.Duration;

        IsFinished = false;

        StartTime = DateTime.Now;
    }

    public void Update()
    {
        PlayTime = DateTime.Now.Subtract(StartTime);
        if (PlayTime > Length)
            IsFinished = true;
    }
}