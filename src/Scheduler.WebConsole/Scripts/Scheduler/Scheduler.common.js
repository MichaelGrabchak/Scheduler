var scheduler = window.scheduler || {};
scheduler.common = scheduler.common || {};

(function () {
    var self = scheduler.common;

    String.prototype.trimAll = function () {
        return this.replace(/\s/g, "");
    }
})();