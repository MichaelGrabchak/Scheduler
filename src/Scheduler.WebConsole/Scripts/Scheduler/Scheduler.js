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
        var $instanceId = $("#scheduler-instanceId");

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

        function setInstance(instanceId) {
            $instanceId.text(instanceId);
        }

        function getInstance() {
            return ($instanceId) ? $instanceId.text() : null;
        }

        function cleanUp() {
            setVersion('');
            setStartDate('');
            setInstance('');
        }

        return {
            version: getVersion(),
            runningSince: getStartDate(),
            instanceId: getInstance(),

            setVersion: function (ver) { setVersion(ver) },
            setStartDate: function (date) { setStartDate(date) },
            setInstance: function (instance) { setInstance(instance) },

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

        function refresh() {
            var jobs = schedulerEngine.data.jobs.getAll();
            var enabled = jobs.filter(function (element) { return element.toggleState === true }).length;
            var disabled = jobs.filter(function (element) { return element.toggleState === false }).length;

            setValue($paused, disabled);
            setValue($running, enabled);
            setValue($total, jobs.length);
        }

        return {
            total: getValue($total),
            running: getValue($running),
            paused: getValue($paused),

            setTotal: function (count) { setValue($total, count) },
            setRunning: function (count) { setValue($running, count) },
            setPaused: function (count) { setValue($paused, count) },

            refresh: function () { refresh() },

            reset: function () { cleanUp() }
        };
    }();

    var schedulerEngine = function() {
        var $startEngine = $("#startEngine");
        var $pauseEngine = $("#pauseEngine");
        var $shutdownEngine = $("#shutdownEngine");

        var schedulerHub = $.connection.schedulerHub;

        schedulerHub.client.jobUnscheduled = function(jobDetails) {
            data.jobs.remove(jobDetails.Group, jobDetails.Name);
            jobCounters.refresh();
        }

        schedulerHub.client.changeState = function(state) {
            setEngineDetails({ state: state });
            schedulerHub.server.getJobsSummary().done(displayJobs);
            jobCounters.refresh();
        }

        schedulerHub.client.jobScheduled = function(jobDetails) {
            data.jobs.add(jobDetails);
            jobCounters.refresh();
        }

        schedulerHub.client.jobUpdate = function (jobDetails) {
            data.jobs.update(jobDetails);
            jobCounters.refresh();
        }

        var data = function () {
            var groupElementIdentifier = function (group) { return ("[id='group_" + group + "']").trimAll() };
            var jobElementIdentifier = function (group, job) { return ("[id='job_" + group + "_" + job + "']").trimAll() };
            var jobStateElementIdentifier = function (group, job) { return ("[id='jobState_" + group + "_" + job + "']").trimAll() };
            var jobTriggerElementIdentifier = function (group, job) { return ("[id='jobTrigger_" + group + "_" + job + "']").trimAll() };

            var $jobsObjects = $("#engine-jobs-details");

            function getJobs(groupName) {
                if (groupName) {
                    var items = [];

                    if (groupExists(groupName)) {
                        $(groupElementIdentifier(groupName)).find("tr").each(function () {
                            items.push({
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

                    return items;
                }
            }

            function getJob(groupName, jobName) {
                var job = $(jobElementIdentifier(groupName, jobName));

                if (job) {
                    return {
                        group: groupName,
                        name: jobName,
                        description: job.find(".jobDescription").text(),
                        schedule: job.find(".jobSchedule").text(),
                        prevFireTime: job.find(".jobPrevFireTime").text(),
                        nextFireTime: job.find(".jobNextFireTime").text(),
                        state: job.find(".jobState").text()
                    };
                }
            }

            function getAllJobs() {
                var jobsElements = $jobsObjects.find("tbody > tr");

                var items = [];

                if (jobsElements) {
                    jobsElements.each(function () {
                        items.push({
                            job: $(this).find(".jobName").text(),
                            description: $(this).find(".jobDescription").text(),
                            schedule: $(this).find(".jobSchedule").text(),
                            prevFireTime: $(this).find(".jobPrevFireTime").text(),
                            nextFireTime: $(this).find(".jobNextFireTime").text(),
                            state: $(this).find(".jobState").text(),
                            toggleState: ($(this).find(".toggle.btn-success").length > 0)
                        });
                    });
                }

                return items;
            }

            function getGroup(groupName) {
                if (groupExists(groupName)) {
                    return {
                        name: groupName,
                        jobs: getJobs(groupName)
                    };
                }
            }

            function groupExists(groupName) {
                return ($(groupElementIdentifier(groupName)).length > 0);
            }

            function jobExists(groupName, jobName) {
                return ($(jobElementIdentifier(groupName, jobName)).length > 0);
            }

            function createGroup(groupName) {
                if (!getGroup(groupName)) {
                    $jobsObjects.append(templates.jobGroup({ id: groupName.trimAll(), group: groupName }));
                }
            }

            function addJob(job) {
                if (job) {
                    if (groupExists(job.Group) === false) {
                        createGroup(job.Group);
                    }

                    $(groupElementIdentifier(job.Group)).find("table.table tbody").prepend(templates.jobDetails({
                        id: (job.Group + "_" + job.Name).trimAll(),
                        name: job.Name,
                        description: job.Description,
                        schedule: job.Schedule,
                        prevFireTime: job.PreviousFireTime,
                        nextFireTime: job.NextFireTime,
                        state: job.State,
                        actionState: job.ActionState,
                        isExecuting: (job.ActionState === "Executing")
                    }));

                    attachJobStateChangeEvent(job.Group, job.Name, job.State);
                    attachJobTriggerEvent(job.Group, job.Name);
                }
            }

            function updateJob(job) {
                if (!jobExists(job.Group, job.Name)) {
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
                if (groupExists(groupName) === true) {
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
                jobs: {
                    remove: function (group, job) { removeJob(group, job); },
                    add: function (data) { addJob(data) },
                    update: function (data) { updateJob(data) },
                    getAll: function () { return getAllJobs() },
                    count: function () { return getAllJobs().length }
                },
                groups: {
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

        function displayJobs(jobsData, rescheduleJobs) {
            data.reset();

            if (jobsData) {
                jobsData.Jobs.forEach(function (item) {
                    schedulerHub.client.jobScheduled(item);
                });

                setJobsSummary(jobsData);
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

                if (engineData.InstanceId) {
                    schedulerInfo.setInstance(engineData.InstanceId);
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
            data.reset();
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

            data: data,

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
        return this.replace(/\s/g, "");
    }
})();