var RadioList = Array();
var CurrentRadioName = "";
var CurrentTrack = "";

var Browser = null;

API.onResourceStart.connect(() => {
    API.triggerServerEvent("GetRadioList");
    CreateBrowser();
});

API.onResourceStop.connect(() => {
    DestroyBrowser();
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
        case "ErrorRadioName":
        {
            API.sendChatMessage("Erreur, contacter un admin ! (Wrong Radio Name !)");
            break;
        }
    }
});

function CreateBrowser() {
    const res = API.getScreenResolution();
    Browser = API.createCefBrowser(res.Width, res.Height);
    API.waitUntilCefBrowserInit(Browser);
    API.setCefBrowserPosition(Browser, res.Width - 200, res.Height - 200);
    API.setCefBrowserHeadless(Browser, true);
    API.loadPageCefBrowser(Browser, "radio_list.html");
}

function DestroyBrowser() {
    API.sendChatMessage("Destroy");
    if (Browser)
        API.destroyCefBrowser(Browser);
}

function PlayRadio(radio) {
    StopRadio();
    CurrentRadioName = radio;
    API.triggerServerEvent("GetRadioInfos", CurrentRadioName);
}

function DisplayTrackList() {
    if (!API.getCefBrowserHeadless(Browser)) return;

    API.setCefBrowserHeadless(Browser, false);
    const res = API.getScreenResolution();
    API.setCefBrowserSize(Browser, res.Width, res.Height);
    API.setCefBrowserPosition(Browser, res.Width - 200, res.Height - 200);
    API.showCursor(true);
}

function HideTrackList() {
    if (API.getCefBrowserHeadless(Browser)) return;

    API.setCefBrowserHeadless(Browser, true);
    API.showCursor(false);
}

function PlayNextTrack(track, time) {
    CurrentTrack = track;
    API.sendChatMessage(CurrentRadioName + " - " + track);
    const path = `musics\\${CurrentRadioName}\\${track}`;
    API.sendChatMessage(path);
    API.startAudio(path, false);
    if(time !== 0)
        API.setAudioTime(time);
}

function StopRadio() {
    CurrentRadioName = "";
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