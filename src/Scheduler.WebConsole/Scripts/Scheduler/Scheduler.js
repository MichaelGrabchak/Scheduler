scheduler = window.scheduler || {};

(function() {
    var self = scheduler;

    var templates = function () {
        var $jobGroupTemplate = $("#jobGroup-template");
        var $jobDetailsTemplate = $("#jobDetails-template");

        function compileTemplate($template) {
            return Handlebars.compile($template.html());
        }

        return {
            jobGroup: function (params) { return compileTemplate($jobGroupTemplate)(params); },
            jobDetails: function (params) { return compileTemplate($jobDetailsTemplate)(params); }
        };
    }();

    var schedulerInfo = function () {
        var $startDate = $("#scheduler-startDate");
        var $version = $("#scheduler-version");

        function setStartDate(date) {
            $startDate.text(date);
        }

        function getStartDate() {
            return ($startDate) ? $startDate.text() : null;
        }

        function setVersion(version) {
            $version.text(version);
        }

        function getVersion() {
            return ($version) ? $version.text() : null;
        }

        function cleanUp() {
            setVersion('');
            setStartDate('');
        }

        return {
            version: getVersion(),
            runningSince: getStartDate(),

            setVersion: function (ver) { setVersion(ver) },
            setStartDate: function (date) { setStartDate(date) },

            reset: function () { cleanUp() }
        };
    }();

    var engineInfo = function () {
        var $type = $("#engine-type");
        var $state = $("#engine-state");

        function setEngineType(type) {
            $type.text(type);
        }

        function getEngineType() {
            return ($type) ? $type.text() : null;
        }

        function setEngineState(state) {
            $state.text(state);
        }

        function getEngineState() {
            return ($state) ? $state.text() : null;
        }

        function cleanUp() {
            setVersion('');
            setStartDate('');
        }

        return {
            type: getEngineType(),
            state: getEngineState(),

            setType: function (type) { setEngineType(type) },
            setState: function (state) { setEngineState(state) },

            reset: function () { cleanUp() }
        };
    }();

    var jobCounters = function () {
        var $total = $("#jobs-total-count");
        var $running = $("#jobs-total-running");
        var $paused = $("#jobs-total-paused");

        function setValue($element, value) {
            if ($element)
            {
                $element.text(value);
            }
        }

        function getValue($element) {
            return ($element) ? $element.text() : 0;
        }

        function cleanUp() {
            setValue($total, 0);
            setValue($running, 0);
            setValue($paused, 0);
        }

        return {
            total: getValue($total),
            running: getValue($running),
            paused: getValue($paused),

            setTotal: function (count) { setValue($total, count) },
            setRunning: function (count) { setValue($running, count) },
            setPaused: function (count) { setValue($paused, count) },

            reset: function () { cleanUp() }
        };
    }();

    var schedulerObjects = function () {
        var jobObjectIdentifier = "div.panel";
        var $jobsObjects = $("#engine-jobs-details");

        function getAllGroups() {
            return $jobsObjects.find(jobObjectIdentifier);
        }

        function cleanUp() {
            $jobsObjects.empty();
        }

        return {
            jobs: {

            },
            groups: {
                count: function () { return getAllGroups().length },
                getAll: function () { return getAllGroups() }
            },
            reset: function () { cleanUp() }
        };
    }();

    var schedulerEngine = function() {
        var $startEngine = $("#startEngine");
        var $pauseEngine = $("#pauseEngine");
        var $shutdownEngine = $("#shutdownEngine");

        var schedulerHub = $.connection.schedulerHub;

        schedulerHub.client.jobUnscheduled = function(jobDetails) {
            removeJob(jobDetails.Group, jobDetails.Name);
        }

        schedulerHub.client.changeState = function(state) {
            setEngineDetails({ state: state });
            schedulerHub.server.getJobsSummary().done(displayJobs);
        }

        schedulerHub.client.jobScheduled = function(jobDetails) {
            addJob(jobDetails);
        }

        schedulerHub.client.jobUpdate = function (jobDetails) {
            updateJob(jobDetails);
        }

        function init() {
            $.connection.hub.start().done(function () {
                schedulerHub.server.getJobsSummary().done(displayJobs);
                schedulerHub.server.getEngineInfo().done(setEngineInfo);
            });
        };

        function displayJobs(data, rescheduleJobs) {
            schedulerObjects.reset();

            if (data) {
                data.Jobs.forEach(function (item) {
                    schedulerHub.client.jobScheduled(item);
                });

                setJobsSummary(data);
            }
        }

        function setJobsSummary(jobsInfo) {
            if (jobsInfo) {
                if (!isNaN(jobsInfo.TotalCount)) {
                    jobCounters.setTotal(jobsInfo.TotalCount);
                }

                if (!isNaN(jobsInfo.TotalRunning)) {
                    jobCounters.setRunning(jobsInfo.TotalRunning);
                }

                if (!isNaN(jobsInfo.TotalPaused)) {
                    jobCounters.setPaused(jobsInfo.TotalPaused);
                }  
            }
        }

        function setEngineInfo(engineData) {
            if (engineData) {
                if (engineData.Engine) {
                    engineInfo.setType(engineData.Engine);
                }

                if (engineData.State) {
                    engineInfo.setState(engineData.State);
                }

                if (engineData.RunningSince) {
                    schedulerInfo.setStartDate(engineData.RunningSince);
                }

                if (engineData.Version) {
                    schedulerInfo.setVersion(engineData.Version);
                }
            }
        }

        function setEngineDetails(engineDetails) {
            if (engineDetails.engineType) {
                engineInfo.setType(engineDetails.engineType);
            }

            if (engineDetails.state) {
                engineInfo.setState(engineDetails.state);
            }
        }

        function jobGroupExists(groupName) {

            if (schedulerObjects.groups.count() > 0) {
                return (schedulerObjects.groups.getAll()
                    .find("div.panel-heading:contains('" + groupName + "')").length >
                    0);
            }

            return false;
        }

        function getGroup(groupName) {
            if (schedulerObjects.groups.count() > 0) {
                return $("[id='group_" + groupName + "']");
            }
        }

        function createGroup(groupName) {
            if (jobGroupExists(groupName) === false) {
                $("#engine-jobs-details").append(templates.jobGroup({ Group: groupName }));
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
                toggleState(job.Group, job.Name, job.State);
            }

            if (job.ActionState) {
                setJobActionState(job.Group, job.Name, job.ActionState);
            }

            if (job.State && !job.ActionState)
            {
                $(jobIdentifier).find(".jobState").text(job.State);
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
                    if (job.ActionState === "Executing")
                    {
                        job.IsExecuting = true;
                    }
                    $(group).find("table.table tbody").prepend(templates.jobDetails(job));
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
                if (!$(elementName).prop('disabled'))
                {
                    var isEnabled = $(elementName).prop("checked");
                    if (!isEnabled) {
                        schedulerHub.server.resumeJob(jobName, groupName);
                    } else {
                        schedulerHub.server.pauseJob(jobName, groupName);
                    }
                }
            });
        }

        function attachJobTriggerEvent(groupName, jobName) {
            var elementName = "[id='jobTrigger_" + groupName + "_" + jobName + "']";
            $(elementName).click(function () {
                if (!$(elementName).hasClass("disabled"))
                {
                    schedulerHub.server.triggerJob(jobName, groupName);
                }
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

            jobCounters.reset();
            schedulerInfo.reset();
        }

        function toggleState(jobGroup, jobName, jobState) {
            var elementName = "[id='jobState_" + jobGroup + "_" + jobName + "']";
            if (jobState === "Paused") {
                $(elementName).bootstrapToggle("off");
            } else {
                $(elementName).bootstrapToggle("on");
            }
        }

        function getToggleState(jobGroup, jobName) {
            var elementId = "[id='jobState_" + jobGroup + "_" + jobName + "']";
            return ($(elementId).prop('checked') === true) ? "Normal" : "Paused";
        }

        function setJobActionState(jobGroup, jobName, jobState) {
            var jobElementId = "[id='job_" + jobGroup + "_" + jobName + "']";
            var triggerButtonId = "[id='jobTrigger_" + jobGroup + "_" + jobName + "']";

            if (jobState === "Succeeded" || jobState === "Failed" || jobState === "Skipped") {
                if (jobState === "Skipped")
                {
                    $(triggerButtonId).addClass("disabled");
                }

                highlightElement(jobElementId, jobState, 4500);
                $(jobElementId).find(".jobState").text(jobState);
                setTimeout(function () {
                    $(jobElementId).find(".jobState").text(getToggleState(jobGroup, jobName));
                    $(triggerButtonId).removeClass("disabled");
                }, 3500);
            } else if (jobState === "Executing") {
                $(jobElementId).find(".jobState").text(jobState);
                $(triggerButtonId).addClass("disabled");
            } else {
                $(jobElementId).find(".jobState").text(jobState);
                $(triggerButtonId).removeClass("disabled");
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

            if (engineInfo.state !== "Paused") {
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

    String.prototype.trimAll = function () {
        return this.replace(/\s/g, '_');
    }
})();