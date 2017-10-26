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

    var schedulerEngine = function() {
        var $startEngine = $("#startEngine");
        var $pauseEngine = $("#pauseEngine");
        var $shutdownEngine = $("#shutdownEngine");

        var schedulerHub = $.connection.schedulerHub;

        schedulerHub.client.jobUnscheduled = function(jobDetails) {
            schedulerObjects.removeJob(jobDetails.Group, jobDetails.Name);
        }

        schedulerHub.client.changeState = function(state) {
            setEngineDetails({ state: state });
            schedulerHub.server.getJobsSummary().done(displayJobs);
        }

        schedulerHub.client.jobScheduled = function(jobDetails) {
            schedulerObjects.addJob(jobDetails);
        }

        schedulerHub.client.jobUpdate = function (jobDetails) {
            schedulerObjects.updateJob(jobDetails);
        }

        var schedulerObjects = function () {
            var groupElementIdentifier = function (group) { return "[id='group_" + group + "']" };
            var jobElementIdentifier = function (group, job) { return "[id='job_" + group + "_" + job + "']" };
            var jobStateElementIdentifier = function (group, job) { return "[id='jobState_" + group + "_" + job + "']" };
            var jobTriggerElementIdentifier = function (group, job) { return "[id='jobTrigger_" + group + "_" + job + "']" };

            var $jobsObjects = $("#engine-jobs-details");

            function getJobs(groupName) {
                if (groupName) {
                    var jobs = [];

                    if (doesGroupExist(groupName)) {
                        $(groupElementIdentifier(groupName)).find("tr").each(function () {
                            jobs.push({
                                group: groupName,
                                name: $(this).find(".jobName").text(),
                                description: $(this).find(".jobGroup").text(),
                                schedule: $(this).find(".jobSchedule").text(),
                                prevFireTime: $(this).find(".jobPrevFireTime").text(),
                                nextFireTime: $(this).find(".jobNextFireTime").text(),
                                state: $(this).find(".jobState").text()
                            });
                        });
                    }

                    return jobs;
                }
            }

            function getJob(groupName, jobName) {
                var job = $(jobElementIdentifier(groupName, jobName));

                if (job) {
                    return {
                        group: groupName,
                        name: jobName,
                        description: job.find(".jobGroup").text(),
                        schedule: job.find(".jobSchedule").text(),
                        prevFireTime: job.find(".jobPrevFireTime").text(),
                        nextFireTime: job.find(".jobNextFireTime").text(),
                        state: job.find(".jobState").text()
                    };
                }
            }

            function getGroup(groupName) {
                if (doesGroupExist(groupName)) {
                    return {
                        name: groupName,
                        jobs: getJobs(groupName)
                    };
                }
            }

            function doesGroupExist(groupName) {
                return ($(groupElementIdentifier(groupName)).length > 0);
            }

            function doesJobExist(groupName, jobName) {
                return ($(jobElementIdentifier(groupName, jobName)).length > 0);
            }

            function createGroup(groupName) {
                if (!getGroup(groupName)) {
                    $jobsObjects.append(templates.jobGroup({ Group: groupName }));
                }
            }

            function addJob(job) {
                if (job) {
                    if (doesGroupExist(job.Group) === false) {
                        createGroup(job.Group);
                    }

                    if (job.ActionState === "Executing") {
                        job.IsExecuting = true;
                    }

                    $(groupElementIdentifier(job.Group)).find("table.table tbody").prepend(templates.jobDetails(job));
                    attachJobStateChangeEvent(job.Group, job.Name, job.State);
                    attachJobTriggerEvent(job.Group, job.Name);
                }
            }

            function updateJob(job) {
                if (!doesJobExist(job.Group, job.Name)) {
                    addJob(job);
                    return;
                }

                var jobElement = $(jobElementIdentifier(job.Group, job.Name));

                if (job.Schedule) {
                    jobElement.find(".jobSchedule").text(job.Schedule);
                }

                if (job.State) {
                    toggleState(job.Group, job.Name, job.State);
                }

                if (job.ActionState) {
                    setJobActionState(job.Group, job.Name, job.ActionState);
                }

                if (job.State && !job.ActionState) {
                    jobElement.find(".jobState").text(job.State);
                }

                if (job.PreviousFireTime) {
                    jobElement.find(".jobPrevFireTime").text(job.PreviousFireTime);
                }

                if (job.NextFireTime) {
                    jobElement.find(".jobNextFireTime").text(job.NextFireTime);
                }
            }

            function removeJob(groupName, jobName) {
                if (doesGroupExist(groupName) === true) {
                    var job = $(jobElementIdentifier(groupName, jobName));
                    if (job) {
                        job.remove();
                    }
                }
            }

            function getGroupElement(groupName) {
                return $jobsObjects.find(groupElementIdentifier(groupName))[0];
            }

            function getAllGroups() {
                return $jobsObjects.find(jobObjectIdentifier);
            }

            function cleanUp() {
                $jobsObjects.empty();
            }

            function toggleState(jobGroup, jobName, jobState) {
                if (jobState === "Paused") {
                    $(jobStateElementIdentifier(jobGroup, jobName)).bootstrapToggle("off");
                } else {
                    $(jobStateElementIdentifier(jobGroup, jobName)).bootstrapToggle("on");
                }
            }

            function setJobActionState(jobGroup, jobName, jobState) {
                if (jobState === "Succeeded" || jobState === "Failed" || jobState === "Skipped") {
                    if (jobState === "Skipped") {
                        $(jobTriggerElementIdentifier(jobGroup, jobName)).addClass("disabled");
                    }

                    highlightElement(jobElementIdentifier(jobGroup, jobName), jobState, 4500);
                    $(jobElementIdentifier(jobGroup, jobName)).find(".jobState").text(jobState);
                    setTimeout(function () {
                        $(jobElementIdentifier(jobGroup, jobName)).find(".jobState").text(getToggleState(jobGroup, jobName));
                        $(jobTriggerElementIdentifier(jobGroup, jobName)).removeClass("disabled");
                    }, 3500);
                } else if (jobState === "Executing") {
                    $(jobElementIdentifier(jobGroup, jobName)).find(".jobState").text(jobState);
                    $(jobTriggerElementIdentifier(jobGroup, jobName)).addClass("disabled");
                } else {
                    $(jobElementIdentifier(jobGroup, jobName)).find(".jobState").text(jobState);
                    $(jobTriggerElementIdentifier(jobGroup, jobName)).removeClass("disabled");
                }
            }

            function highlightElement(elementName, level, speed) {
                switch (level) {
                    case "Succeeded":
                        $(elementName).effect("highlight", { color: "#c9ffbf" }, speed);
                        return;
                    case "Failed":
                        $(elementName).effect("highlight", { color: "#ffbfbf" }, speed);
                        return;
                    default:
                        $(elementName).effect("highlight", { color: "#ffeebf" }, speed);
                        return;
                }
            }

            function getToggleState(jobGroup, jobName) {
                return ($(jobStateElementIdentifier(jobGroup, jobName)).prop('checked') === true) ? "Normal" : "Paused";
            }

            function attachJobStateChangeEvent(groupName, jobName, state) {
                var elementName = jobStateElementIdentifier(groupName, jobName);
                var elementState = (state === "Paused") ? "off" : "on";
                $(elementName).bootstrapToggle(elementState);
                $(elementName).parent().click(function () {
                    if (!$(elementName).prop('disabled')) {
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
                var elementName = jobTriggerElementIdentifier(groupName, jobName);
                $(elementName).click(function () {
                    if (!$(elementName).hasClass("disabled")) {
                        schedulerHub.server.triggerJob(jobName, groupName);
                    }
                });
            }

            return {
                removeJob: function (group, job) { removeJob(group, job); },
                addJob: function (data) { addJob(data) },
                updateJob: function (data) { updateJob(data) },
                groups: {
                    contains: function (groupName) { return (!getGroupElement(groupName)) },
                    count: function () { return getAllGroups().length },
                    getAll: function () { return getAllGroups() }
                },
                reset: function () { cleanUp() }
            };
        }();

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

        function cleanUpJobDetails() {
            schedulerObjects.reset();
            jobCounters.reset();
            schedulerInfo.reset();
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