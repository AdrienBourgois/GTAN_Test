var RadioList = Array();
var CurrentRadioName = "";
var CurrentTrack = "";

var Browser;

API.onResourceStart.connect(() => {
    API.triggerServerEvent("GetRadioList");
});

API.onServerEventTrigger.connect((eventName, args) => {
    switch (eventName) {

        case "SetRadioList":
        {
            const radioListString = args[0].toString();
            RadioList = radioListString.split(",");
            API.sendChatMessage(RadioList.join(","));
            break;
        }
        case "SetRadioInfos":
        {
            PlayNextTrack(args[0].toString(), args[1]);
            break;
        }
        case "NextTrack":
        {
            if (args[0].toString() === CurrentRadioName)
                PlayNextTrack(args[1].toString(), 0);
            break;
        }
    }
});

function PlayRadio(radio) {
    StopRadio();
    CurrentRadioName = radio;
    API.triggerServerEvent("GetRadioInfos", CurrentRadioName);
}

function DisplayTrackList() {
    const res = API.getScreenResolution();
    Browser = API.createCefBrowser(res.Width, res.Height);
    API.waitUntilCefBrowserInit(Browser);
    API.setCefBrowserPosition(Browser, 50, 50);
    API.loadPageCefBrowser(Browser, "radio_list.html");
    API.showCursor(true);
}

function HideTrackList() {
    API.showCursor(false);
    API.destroyCefBrowser(Browser);
}

function PlayNextTrack(track, time) {
    CurrentTrack = track;
    API.sendChatMessage(CurrentRadioName + " - " + track);
    const path = "musics\\" + CurrentRadioName + "\\" + track;
    API.sendChatMessage(path);
    API.startAudio(path, false);
    if(time !== 0)
        API.setAudioTime(time);
}

function StopRadio() {
    CurrentTrack = "";
    API.stopAudio();
}

API.onKeyDown.connect((sender, e) => {
    if (e.KeyCode === Keys.Up) {
        DisplayTrackList();
    }
    if (e.KeyCode === Keys.Down) {
        HideTrackList();
    }
});