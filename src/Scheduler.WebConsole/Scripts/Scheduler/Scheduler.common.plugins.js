var scheduler = window.scheduler || {};
scheduler.common = scheduler.common || {};
scheduler.common.plugins = scheduler.common.plugins || {};

(function () {
    var self = scheduler.common.plugins;

    var loadingBox = function () {
        var loadingElement = $("<div>", {
            class: "loading",
        });

        function showLoadingOverlay() {
            $.LoadingOverlay("show", {
                color: "rgba(255, 255, 255, 1)",
                fade: [0, 300],
                image: "",
                custom: loadingElement,
                size: 0
            });
        }

        function hideLoadingOverlay() {
            $.LoadingOverlay("hide");
        }

        function showLoadingOverlayBox(element) {
            $(element).LoadingOverlay("show", {
                color: "rgba(255, 255, 255, 1)",
                fade: [0, 300],
                image: "",
                custom: loadingElement,
                size: 0
            });
        }

        function hideLoadingOverlayBox(element) {
            $(element).LoadingOverlay("hide");
        }

        return {
            showOverlay: function () { showLoadingOverlay(); },
            hideOverlay: function () { hideLoadingOverlay(); },

            showOverlayBox: function (el) { showLoadingBox(el); },
            hideOverlayBox: function (el) { hideLoadingBox(el); }
        };
    }();

    self.showLoadingOverlay = function () {
        loadingBox.showOverlay();
    };

    self.hideLoadingOverlay = function () {
        loadingBox.hideOverlay();
    };

    self.showLoadingOverlayBox = function (el) {
        loadingBox.showOverlayBox(el);
    };

    self.hideLoadingOverlayBox = function (el) {
        loadingBox.hideOverlayBox(el);
    };
})();