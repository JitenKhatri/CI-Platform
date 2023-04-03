var mission
var mission_id
var actions
var date
var message
var goal_object
var time_mission
var time_date
var hours
var mins
var time_message
const goaldata = () => {
    var type = "goal"
    validate(type)
    if ($(`#mission_goal`).is(":disabled")) {
        if (mission != 0 && actions.toString() != "NaN" && actions > 0 && date != "" && message.trim().length > 20) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: { mission_id: mission_id, date: date.toString(), actions: actions, message: message, type: "goal-edit", timesheet_id: parseInt(document.getElementById("timesheet-id").value) },
                success: function (result) {
                    if (result.view) {
                        $(`#timesheet-${parseInt(document.getElementById("timesheet-id").value)}`).replaceWith(result.view.result)
                        $("#goal").modal('hide')
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        }
    }
    else {
        if (mission != 0 && actions.toString() != "NaN" && actions > 0 && date != "" && message.trim().length > 20) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: { mission_id: mission_id, date: date.toString(), actions: actions, message: message, type: "goal" },
                success: function (result) {
                    if (result.view) {
                        $(".goal-timesheets").append(result.view.result)
                        $("#goal").modal('hide')
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        }
    }

}
const timedata = () => {
    var type = "time"
    validate(type)
    if ($(`.time-mission`).is(":disabled")) {
        if (time_mission != 0 && time_date.length > 0 && hours <= 24 && hours > 0 &&
            hours.length != 0 && mins <= 60 && mins > 0 && mins.length != 0 && time_message.trim().length > 20) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: { mission_id: time_mission, date: time_date.toString(), hours: hours, minutes: mins, message: time_message, type: "time-edit", timesheet_id: parseInt(document.getElementById("timesheet-id").value) },
                success: function (result) {
                    if (result.view) {
                        $(`#timesheet-${parseInt(document.getElementById("timesheet-id").value)}`).replaceWith(result.view.result)
                        $("#time").modal('hide')
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        }
    }
    else {
        if (time_mission != 0 && time_date.length > 0 && hours <= 24 && hours > 0 &&
            hours.length != 0 && mins <= 60 && mins > 0 && mins.length != 0 && time_message.trim().length > 20) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: { mission_id: time_mission, date: time_date.toString(), hours: hours, minutes: mins, message: time_message, type: "time" },
                success: function (result) {
                    if (result.view) {
                        $(".time-timesheets").append(result.view.result)
                        $("#time").modal('hide')
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        }
    }
}
const validate = (type) => {
    if (type == "goal") {
        mission = document.getElementById("mission_goal").value
        mission_id = parseInt(mission.split("goal_object")[0].split("mission_id-")[1])
        actions = parseInt(document.getElementById("action").value)
        date = document.getElementById("goal-date").value
        message = document.getElementById("goal-message").value
        if (mission == 0) {
            $(".goal-mission").addClass("d-block").removeClass("d-none")
        }
        else {
            goal_object = mission.split("goal_object-")[1]
            $(".goal-mission").addClass("d-none").removeClass("d-block")
        }
        if (actions.toString() == "NaN") {
            $(".action-empty").addClass("d-block").removeClass("d-none")
        }
        else if (actions > goal_object || actions < 0) {
            $(".action-notvalid").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".action-empty").addClass("d-none").removeClass("d-block")
            $(".action-notvalid").addClass("d-none").removeClass("d-block")
        }
        if (date == "") {
            $(".date-empty").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".date-empty").addClass("d-none").removeClass("d-block")
        }
        if (message.trim().length < 20) {
            $(".message-empty").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".message-empty").addClass("d-none").removeClass("d-block")
        }
    }
    else {
        time_mission = parseInt(document.getElementsByClassName("time-mission")[0].value)
        time_date = document.getElementsByClassName("time-date")[0].value
        hours = document.getElementsByClassName("time-hours")[0].value
        mins = document.getElementsByClassName("time-min")[0].value
        time_message = document.getElementsByClassName("time-message")[0].value
        if (time_mission == 0) {
            $(".time-mission-empty").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".time-mission-empty").addClass("d-none").removeClass("d-block")
        }
        if (time_date.length > 0) {
            $(".time-date-empty").addClass("d-none").removeClass("d-block")
        }
        else {
            $(".time-date-empty").addClass("d-block").removeClass("d-none")
        }
        if (hours > 24 || hours <= 0 || hours.length == 0) {
            $(".time-hours-valid").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".time-hours-valid").addClass("d-none").removeClass("d-block")
        }
        if (mins > 60 || mins <= 0 || mins.length == 0) {
            $(".time-min-valid").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".time-min-valid").addClass("d-none").removeClass("d-block")
        }
        if (time_message.trim().length < 20) {
            $(".time-message-empty").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".time-message-empty").addClass("d-none").removeClass("d-block")
        }
    }
}

const clear_modal = (type) => {
    if (type == 'time') {
        $(`.time-mission`).removeAttr("disabled", "disabled")
        document.getElementsByClassName('time-hours')[0].value = ""
        document.getElementsByClassName('time-min')[0].value = ""
        document.getElementsByClassName('time-message')[0].value = ""
    }
    else {
        $(`#mission_goal`).removeAttr("disabled", "disabled")
        document.getElementById('action').value = ""
        document.getElementById('goal-message').value = ""
    }
}


const edittimesheet = (id, mission, hours, minutes, message, type, action) => {
    if (type == "time") {
        $(`.time-mission option[value=${mission}]`).attr("selected", "selected")
        $(`.time-mission`).attr("disabled", "disabled")
        document.getElementsByClassName('time-hours')[0].value = parseInt(hours)
        document.getElementsByClassName('time-min')[0].value = parseInt(minutes.slice(0, 2))
        document.getElementsByClassName('time-message')[0].value = message
        document.getElementById("timesheet-id").value = id
    }
    else {
        $(`#mission-${mission}`).attr("selected", "selected")
        $(`#mission_goal`).attr("disabled", "disabled")
        document.getElementById('action').value = parseInt(action)
        document.getElementById('goal-message').value = message
        document.getElementById("timesheet-id").value = id
    }
}

const deletetimesheet = (id) => {
    $(`#timesheet-${id}`).remove()
    $.ajax({
        url: '/Mission/Volunteering_Timesheet',
        type: 'POST',
        data: { timesheet_id: parseInt(id), type: "time-delete" },
        success: function (result) {
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}
