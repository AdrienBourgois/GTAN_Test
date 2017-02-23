API.onServerEventTrigger.connect((eventName, args) => {
    switch (eventName) {

        case "SetRadioTimer":
            API.sendChatMessage("Start music !");
            API.startAudio("Axero.mp3", false);
            API.setAudioTime(args[0]);
            break;
    }
});

API.onPlayerEnterVehicle.connect(veh => {
    API.triggerServerEvent("GetRadioTimer");
});

API.onPlayerExitVehicle.connect(veh => {
    API.stopAudio();
});