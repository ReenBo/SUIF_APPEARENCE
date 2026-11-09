mergeInto(LibraryManager.library, {
  SUIF_EmitBenchmarkComplete: function (jsonStrPtr) {
    var jsonStr = UTF8ToString(jsonStrPtr);
    if (window.onSUIFBenchmarkComplete) {
      window.onSUIFBenchmarkComplete(jsonStr);
    }
    window.suifBenchmarkResult = JSON.parse(jsonStr);
    console.log("[SUIF WebGL Benchmark Result]", window.suifBenchmarkResult);
  },

  SUIF_GetUrlSearchParam: function (paramNamePtr) {
    var paramName = UTF8ToString(paramNamePtr);
    var urlParams = new URLSearchParams(window.location.search);
    var val = urlParams.get(paramName);
    if (!val) return 0;
    var bufferSize = lengthBytesUTF8(val) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(val, buffer, bufferSize);
    return buffer;
  }
});

