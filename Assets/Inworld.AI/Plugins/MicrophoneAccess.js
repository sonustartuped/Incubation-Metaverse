function RequestMicrophoneAccess(callbackName) {
    navigator.mediaDevices.getUserMedia({ audio: true })
        .then(function(stream) {
            window.Unity.call(callbackName, "granted");
        })
        .catch(function(error) {
            window.Unity.call(callbackName, "denied");
        });
}