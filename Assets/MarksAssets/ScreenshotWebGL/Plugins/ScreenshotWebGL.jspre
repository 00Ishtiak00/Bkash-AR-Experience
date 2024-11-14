(function() {
	var loadDynamically = true;
	
	var s = document.createElement('script');
	s.id = "html2canvas";
	document.head.appendChild(s);
	s.onload = function() {
		Module["ScreenshotWebGL"] = Module["ScreenshotWebGL"] || {};
		if (Module.ScreenshotWebGL.onloaded)//if onloaded already exists
			Module.dynCall_v(Module.ScreenshotWebGL.onloaded);
		else {//if not, it may never run, but if it does, call onloaded
			Object.defineProperty(Module.ScreenshotWebGL, "onloaded", {
				configurable: true,
				set: function(v){
					Object.defineProperty(Module.ScreenshotWebGL, "onloaded", {configurable: true, enumerable: true, writable: true, value: v });
					Module.dynCall_v(Module.ScreenshotWebGL.onloaded);
				}
			});
		}
	}
	
	if (loadDynamically) {
		var httpRequest = new XMLHttpRequest();
		httpRequest.onreadystatechange = function() {
			if (httpRequest.readyState === XMLHttpRequest.DONE) {
				s.setAttribute('src', JSON.parse(httpRequest.responseText).latest);
			}
		}
		httpRequest.open('GET', 'https://api.cdnjs.com/libraries/html2canvas', true);
		httpRequest.send();
	} else {
		s.setAttribute("src", window.location.href.replace(/\/+$/, '') + "/StreamingAssets/html2canvas.min.js");
	}
})();