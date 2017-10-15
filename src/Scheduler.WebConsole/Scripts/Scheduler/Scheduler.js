scheduler = window.scheduler || {};

(function() {
    var self = scheduler;

    var schedulerEngine = function() {
        var $startEngine = $("#startEngine");
        var $pauseEngine = $("#pauseEngine");
        var $shutdownEngine = $("#shutdownEngine");

        var jobGroupTemplate = Handlebars.compile($("#jobGroup-template").html());
        var jobDetailsTemplate = Handlebars.compile($("#jobDetails-template").html());

        var schedulerHub = $.connection.schedulerHub;

        schedulerHub.client.jobUnscheduled = function(jobDetails) {
            removeJob(jobDetails.Group, jobDetails.Name);
        }

        schedulerHub.client.changeState = function(state) {
            setEngineDetails({ state: state });

            schedulerHub.server.getJobsSummary().done(function (result) { displayJobs(result, true); });
        }

        schedulerHub.client.jobScheduled = function(jobDetails) {
            addJob(jobDetails);
        }

        schedulerHub.client.jobUpdate = function (jobDetails) {
            updateJob(jobDetails);
        }

        function init() {
            $.connection.hub.start().done(function () {
                schedulerHub.server.getJobsSummary().done(function (result) { displayJobs(result, true); });
                schedulerHub.server.getEngineInfo().done(setEngineInfo);
            });
        };

        function displayJobs(data, rescheduleJobs) {
            if (rescheduleJobs) {
                $("#engine-jobs-details").empty();
            }

            if (data) {
                if (rescheduleJobs) {
                    data.Jobs.forEach(function (item) {
                        schedulerHub.client.jobScheduled(item);
                    });
                }

                setJobsSummary(data);
            }
        }

        function setJobsSummary(jobsInfo) {
            if (jobsInfo) {
                if (!isNaN(jobsInfo.TotalCount)) {
                    $("#jobs-total-count").text(jobsInfo.TotalCount);
                }

                if (!isNaN(jobsInfo.TotalRunning)) {
                    $("#jobs-total-running").text(jobsInfo.TotalRunning);
                }

                if (!isNaN(jobsInfo.TotalPaused)) {
                    $("#jobs-total-paused").text(jobsInfo.TotalPaused);
                }  
            }
        }

        function setEngineInfo(engineInfo) {
            if (engineInfo) {
                if (engineInfo.Engine) {
                    $("#engine-type").text(engineInfo.Engine);
                }

                if (engineInfo.State) {
                    $("#engine-state").text(engineInfo.State);
                }

                if (engineInfo.RunningSince) {
                    $("#scheduler-startDate").text(engineInfo.RunningSince);
                }

                if (engineInfo.Version) {
                    $("#scheduler-version").text(engineInfo.Version);
                }
            }
        }

        function setEngineDetails(engineInfo) {
            if (engineInfo.engineType) {
                $("#engine-type").text(engineInfo.engineType);
            }

            if (engineInfo.state) {
                $("#engine-state").text(engineInfo.state);    
            }
        }

        function jobGroupExists(groupName) {

            if ($("#engine-jobs-details").find("div.panel").length > 0) {
                return ($("#engine-jobs-details").find("div.panel")
                    .find("div.panel-heading:contains('" + groupName + "')").length >
                    0);
            }

            return false;
        }

        function getGroup(groupName) {
            if ($("#engine-jobs-details").find("div.panel").length > 0) {
                return $("[id='group_" + groupName + "']");
            }
        }

        function createGroup(groupName) {
            if (jobGroupExists(groupName) === false) {
                $("#engine-jobs-details").append(jobGroupTemplate({ Group: groupName }));
            }
        }

        function getJob(groupName, jobName) {
            if (jobGroupExists(groupName)) {
                return getGroup(groupName).filter(function(index, element) {
                    return $(element).find("[id='job_" + groupName + "_" + jobName + "']").length > 0;
                });
            }
        }

        function updateJob(job) {
            var jobObj = getJob(job.Group, job.Name);

            if (!jobObj) {
                addJob(job);
                return;
            }

            var jobIdentifier = "[id='job_" + job.Group + "_" + job.Name + "']";
       
            if (job.Schedule) {
                $(jobIdentifier).find(".jobSchedule").text(job.Schedule);
            }

            if (job.State) {
                setJobState(job.Group, job.Name, job.State);
            }

            if (job.PreviousFireTime) {
                $(jobIdentifier).find(".jobPrevFireTime").text(job.PreviousFireTime);
            }

            if (job.NextFireTime) {
                $(jobIdentifier).find(".jobNextFireTime").text(job.NextFireTime);
            }
        }

        function addJob(job) {
            if (job) {
                if (jobGroupExists(job.Group) === false) {
                    createGroup(job.Group);
                }

                var group = getGroup(job.Group);
                if (group) {
                    $(group).find("table.table tbody").prepend(jobDetailsTemplate(job));
                    attachJobStateChangeEvent(job.Group, job.Name, job.State);
                    attachJobTriggerEvent(job.Group, job.Name);
                }    
            }
        }

        function attachJobStateChangeEvent(groupName, jobName, state) {
            var elementName = "[id='jobState_" + groupName + "_" + jobName + "']";
            var elementState = (state === "Paused") ? "off" : "on";
            $(elementName).bootstrapToggle(elementState);
            $(elementName).parent().click(function () {
                var isEnabled = $(elementName).prop("checked");
                if (!isEnabled) {
                    schedulerHub.server.resumeJob(jobName, groupName);
                } else {
                    schedulerHub.server.pauseJob(jobName, groupName);
                }
                schedulerHub.server.getJobsSummary().done(displayJobs);
            });
        }

        function attachJobTriggerEvent(groupName, jobName) {
            var elementName = "[id='jobTrigger_" + groupName + "_" + jobName + "']";
            $(elementName).click(function() {
                schedulerHub.server.triggerJob(jobName, groupName);
            });
        }

        function removeJob(groupName, jobName) {
            if (jobGroupExists(groupName) === true) {
                var job = getJob(groupName, jobName);
                if (job) {
                    job.remove();
                }
            }
        }

        function cleanUpJobDetails() {
            $("#engine-jobs-details").empty();

            $("#jobs-total-count").text(0);
            $("#jobs-total-running").text(0);
            $("#jobs-total-paused").text(0);
            $("#jobs-total-executed").text(0);

            $("#scheduler-version").empty();
            $("#scheduler-startDate").empty();
        }

        function toggleState(jobGroup, jobName, jobState) {
            var elementName = "[id='jobState_" + jobGroup + "_" + jobName + "']";
            if (jobState === "Paused") {
                $(elementName).bootstrapToggle("off");
            } else {
                $(elementName).bootstrapToggle("on");
            }

            var jobElementId = "[id='job_" + jobGroup + "_" + jobName + "']";
            $(jobElementId).find(".jobState").text(jobState);
        }

        function getToggleState(jobGroup, jobName) {
            var elementId = "[id='jobState_" + jobGroup + "_" + jobName + "']";
            return ($(elementId).prop('checked') === true) ? "Normal" : "Paused";
        }

        function setJobState(jobGroup, jobName, jobState) {
            var jobElementId = "[id='job_" + jobGroup + "_" + jobName + "']";

            if (jobState === "Paused" || jobState === "Normal") {
                toggleState(jobGroup, jobName, jobState);
            } else if (jobState === "Succeeded" || jobState === "Failed" || jobState === "Skipped") {
                highlightElement(jobElementId, jobState, 5000);
                $(jobElementId).find(".jobState").text(jobState);
                setTimeout(function () {
                    var state = $(jobElementId).find(".jobState").text();
                    if (jobState == state)
                    {
                        $(jobElementId).find(".jobState").text(getToggleState(jobGroup, jobName));
                    }
                }, 4000);
            } else {
                $(jobElementId).find(".jobState").text(jobState);
            }
        }

        function highlightElement(elementName, level, speed) {
            switch (level) {
                case "Succeeded" :
                    $(elementName).effect("highlight", { color: "#c9ffbf" }, speed);
                    return;
                case "Failed" :
                    $(elementName).effect("highlight", { color: "#ffbfbf" }, speed);
                    return;
                default: 
                    $(elementName).effect("highlight", { color: "#ffeebf" }, speed);
                    return;
            }            
        }

        function onStartEngineClick() {
            schedulerHub.server.start();

            if ($("#engine-state").text() != "Paused") {
                schedulerHub.server.getEngineInfo().done(setEngineInfo);
            }

            $startEngine.prop("disabled", true);
            $pauseEngine.prop("disabled", false);
            $shutdownEngine.prop("disabled", false);
        }

        function onPauseEngineClick() {
            schedulerHub.server.pause();

            $startEngine.prop("disabled", false);
            $pauseEngine.prop("disabled", true);
            $shutdownEngine.prop("disabled", false);
        }

        function onTurnOffEngineClick() {
            schedulerHub.server.shutdown();

            cleanUpJobDetails();

            $startEngine.prop("disabled", false);
            $pauseEngine.prop("disabled", true);
            $shutdownEngine.prop("disabled", true);
        }

        return {
            // properties
            startEngine: $startEngine,
            pauseEngine: $pauseEngine,
            shutdownEngine: $shutdownEngine,
            hub: schedulerHub,

            // methods
            init: function() { init() },

            // event handlers
            onStartClick: function() { onStartEngineClick() },
            onPauseClick: function () { onPauseEngineClick() },
            onTurnOffClick: function () { onTurnOffEngineClick() }
    };
    }();

    self.init = function() {
        schedulerEngine.init();
    };

    self.initEvents = function() {
        $(schedulerEngine.startEngine).click(function() {
            schedulerEngine.onStartClick();
        });

        $(schedulerEngine.pauseEngine).click(function() {
            schedulerEngine.onPauseClick();
        });

        $(schedulerEngine.shutdownEngine).click(function() {
            schedulerEngine.onTurnOffClick();
        });
    };
})();