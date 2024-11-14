mergeInto(LibraryManager.library, {
	Screenshot_ScreenshotWebGL: function(screenshotCallback, element, options, type, encoderOptions) {
		var opts = undefined;
		var element = UTF8ToString(element);
		var type = UTF8ToString(type);
		
		if (options !== 0) {
			opts = {};
			Object.entries(JSON.parse(UTF8ToString(options))).forEach(function(entry) {
				opts[entry[0]] = entry[1];
			});
		}
		
		function captureVideos() {
			var videos = document.querySelectorAll("video:not([data-html2canvas-ignore], [width='1'], [height='1'], [style*='display: none'], [style*='display:none'],[style*='visibility: hidden'], [style*='visibility:hidden'])");
			if (!videos.length) return;
			
			var canvas = document.getElementById("CaptureVideoCanvas_ScreenshotWebGL");
			if (!canvas){
				canvas = document.createElement("canvas");
				canvas.setAttribute("id","CaptureVideoCanvas_ScreenshotWebGL");
				canvas.hidden = true;
				document.body.append(canvas);
			}
			
			var ctx = canvas.getContext("2d");
			var i, w, h;

			for (i = 0, len = videos.length; i < len; i++) {
				const v = videos[i];
				if (v.getAttribute("data-html2canvas-ignore"))
					continue;

				try {
					w = v.videoWidth;
					h = v.videoHeight;
					canvas.width = w;
					canvas.height = h;
					ctx.fillRect(0, 0, w, h);
					ctx.drawImage(v, 0, 0, w, h);
					const a = canvas.toDataURL();
					v.style.backgroundImage = "url(" + a + ")";
					ctx.clearRect(0, 0, w, h); // clean the canvas
					canvas.width = canvas.height = 0;
				} catch(e) {
					alert(e);
				}
			}
		}
			
		captureVideos();

		html2canvas(document.querySelector(element), opts)
		.then(function(canvas) {
			function dataURLtoFile(dataurl) {
				var bstr = atob(dataurl.split(',')[1]), n = bstr.length, u8arr = new Uint8Array(n);
				while(n--){
					u8arr[n] = bstr.charCodeAt(n);
				}
				return u8arr;
			}
						
			const byteArray = dataURLtoFile(canvas.toDataURL(type, encoderOptions));
			const buffer = Module._malloc(byteArray.length * byteArray.BYTES_PER_ELEMENT);
			Module.HEAPU8.set(byteArray, buffer);
			Module.dynCall_viiii(screenshotCallback, buffer, byteArray.length, canvas.width, canvas.height);
			Module._free(buffer);
		})
		.catch(function(e) {
			alert(e);
		})
		.finally(function() {
			var videos = document.querySelectorAll("video:not([data-html2canvas-ignore])");
			if (!videos.length) return;
			for (i = 0, len = videos.length; i < len; i++) {
				const v = videos[i];
				v.style.removeProperty("background-image");
			}
		});
		
	},
	ScreenshotOptimized_ScreenshotWebGL: function(screenshotCallback, element, options, type, encoderOptions) {
		var opts = undefined;
		var element = UTF8ToString(element);
		var type = UTF8ToString(type);
		
		if (options !== 0) {
			opts = {};
			Object.entries(JSON.parse(UTF8ToString(options))).forEach(function(entry) {
				opts[entry[0]] = entry[1];
			});
		}
		
		function captureVideos() {
			var videos = document.querySelectorAll("video:not([data-html2canvas-ignore])");
			if (!videos.length) return;
			
			var canvas = document.getElementById("CaptureVideoCanvas_ScreenshotWebGL");
			if (!canvas){
				canvas = document.createElement("canvas");
				canvas.setAttribute("id","CaptureVideoCanvas_ScreenshotWebGL");
				canvas.hidden = true;
				document.body.append(canvas);
			}
			
			var ctx = canvas.getContext("2d");
			var i, w, h;

			for (i = 0, len = videos.length; i < len; i++) {
				const v = videos[i];
				if (v.getAttribute("data-html2canvas-ignore"))
					continue;

				try {
					w = v.videoWidth;
					h = v.videoHeight;
					canvas.width = w;
					canvas.height = h;
					ctx.fillRect(0, 0, w, h);
					ctx.drawImage(v, 0, 0, w, h);
					const a = canvas.toDataURL();
					v.style.backgroundImage = "url(" + a + ")";
					ctx.clearRect(0, 0, w, h); // clean the canvas
					canvas.width = canvas.height = 0;
				} catch(e) {
					alert(e);
				}
			}
		}
			
		captureVideos();

		html2canvas(document.querySelector(element), opts)
		.then(function(canvas) {
			fetch(canvas.toDataURL(type, encoderOptions))
			.then(function(response) {
				return response.blob()
			})
			.then(function(blob) {
				Module["ScreenshotWebGL"].screenShotBlob = blob;
				Module.dynCall_v(screenshotCallback);
			})
			.catch(function(e) {
				alert(e);
			});
		})
		.catch(function(e) {
			alert(e);
		})
		.finally(function() {
			var videos = document.querySelectorAll("video:not([data-html2canvas-ignore])");
			if (!videos.length) return;
			for (i = 0, len = videos.length; i < len; i++) {
				const v = videos[i];
				v.style.removeProperty("background-image");
			}
		});
		
	},
	onLoaded_ScreenshotWebGL: function(onloadedCallback) {
		Module["ScreenshotWebGL"] = Module["ScreenshotWebGL"] || {};
		Module["ScreenshotWebGL"].onloaded = onloadedCallback;
	},
	didLoad_ScreenshotWebGL: function() {
		try {
			html2canvas;
		} catch(e) {
			return false;
		}
		return true;
	},
	getGamePositionX_ScreenshotWebGL: function() {
		return document.getElementsByTagName("canvas")[0].getBoundingClientRect().left + window.scrollX;
	},
	getGamePositionY_ScreenshotWebGL: function() {
		return document.getElementsByTagName("canvas")[0].getBoundingClientRect().top + window.scrollY;
	},
	getGameWidth_ScreenshotWebGL: function() {
		return document.getElementsByTagName("canvas")[0].getBoundingClientRect().width;
	},
	getGameHeight_ScreenshotWebGL: function() {
		return document.getElementsByTagName("canvas")[0].getBoundingClientRect().height;
	},
	getDOMProperty_ScreenshotWebGL: function(propertyPath) {
		propertyPath = UTF8ToString(propertyPath);
		var object = (function(root) {
			return root === "window" ? window : root === "document" ? document : undefined;
		})(propertyPath.split('.')[0]);
		if (!object) return -1.0;
		propertyPath = propertyPath.split('.').slice(1).join('.');
		var resolvePath = function resolvePath(object, path, defaultValue) {
		  return path.split('.').reduce(function (o, p) {
			return o ? o[p] : defaultValue;
		  }, object);
		};
		return resolvePath(object, propertyPath);
	},
	clearScreenshotBlob_ScreenshotWebGL: function() {
		if (!Module["ScreenshotWebGL"] || !("screenShotBlob" in Module["ScreenshotWebGL"])) return;
		Module["ScreenshotWebGL"].screenShotBlob = "";
	}
});