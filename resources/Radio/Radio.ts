var RadioList = Array();
var CurrentRadioName = "Electro";
var CurrentTrack = "";

API.onResourceStart.connect(() => {
    API.triggerServerEvent("GetRadioList");
});

API.onServerEventTrigger.connect((eventName, args) => {
    switch (eventName) {

        case "SetRadioList":
            const radioListString = args[0].toString();
            RadioList = radioListString.split(",");
            API.sendChatMessage(RadioList.join(","));
            break;
        case "SetRadioInfos":
            PlayNextTrack(args[0].toString(), args[1]);
            break;
        case "NextTrack":
            if(args[0].toString() === CurrentRadioName)
                PlayNextTrack(args[1].toString(), 0);
            break;
    }
});

function PlayNextTrack(track, time) {
    API.stopAudio();
    CurrentTrack = track;
    API.sendChatMessage(CurrentRadioName + " - " + track);
    const path = "musics\\" + CurrentRadioName + "\\" + track;
    API.sendChatMessage(path);
    API.startAudio(path, false);
    if(time !== 0)
        API.setAudioTime(time);
}

function StopTrack() {
    CurrentTrack = "";
    API.stopAudio();
}

API.onPlayerEnterVehicle.connect(veh => {
    API.triggerServerEvent("GetRadioInfos", CurrentRadioName);
});

API.onPlayerExitVehicle.connect(veh => {
    StopTrack();
});