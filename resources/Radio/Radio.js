API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "SetRadioTimer":
            API.sendChatMessage("Start music !");
            API.startAudio("Axero.mp3", false);
            API.setAudioTime(args[0]);
            break;
    }
});
API.onPlayerEnterVehicle.connect(function (veh) {
    API.triggerServerEvent("GetRadioTimer");
});
API.onPlayerExitVehicle.connect(function (veh) {
    API.stopAudio();
});
//# sourceMappingURL=radio.js.map