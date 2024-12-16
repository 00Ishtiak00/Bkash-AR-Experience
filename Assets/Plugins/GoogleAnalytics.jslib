mergeInto(LibraryManager.library, {
    SendGTagEvent: function(eventName, category, label, value) {
        if (typeof gtag !== "undefined") {
            gtag('event', UTF8ToString(eventName), {
                'event_category': UTF8ToString(category),
                'event_label': UTF8ToString(label),
                'value': value
            });
        } else {
            console.error("gtag is not defined. Make sure Google Analytics is correctly set up.");
        }
    }
});
