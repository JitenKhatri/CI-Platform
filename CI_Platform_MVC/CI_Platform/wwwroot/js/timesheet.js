var mission
var mission_id
var actions
var date
var message
var goal_object
var goal_achieved
var time_mission
var time_date
var hours
var mins
var time_message
const goaldata = () => {
    var type = "goal"
    validate(type)
    if ($(`#mission_goal`).is(":disabled")) {
        if (mission != 0 && actions.toString() != "NaN" && actions > 0 &&  actions < goal_achieved && date != "" && message.trim().length > 20 &&
            !(dateObj.getTime() < startDateObj.getTime() || dateObj.getTime() > endDateObj.getTime())) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: {
                    Mission_id: mission_id,
                    Date: date.toString(),
                    Actions: actions,
                    Message: message,
                    Type: "goal-edit",
                    Timesheet_id: parseInt(document.getElementById("timesheet-id").value)
                },
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
        if (mission != 0 && actions.toString() != "NaN" && actions > 0 && actions < goal_achieved && date != "" && message.trim().length > 20 &&
            !(dateObj.getTime() < startDateObj.getTime() || dateObj.getTime() > endDateObj.getTime())) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: {
                    Mission_id: mission_id,
                    Date: date.toString(),
                    Actions: actions,
                    Message: message,
                    Type: "goal"

                },
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
        if (time_mission != 0 && time_date.length > 0 && hours <= 23 && hours > 0 &&
            hours.length != 0 && mins <= 59 && mins > 0 && mins.length != 0 && time_message.trim().length > 20 && !(dateObj.getTime() < startDateObj.getTime() || dateObj.getTime() > endDateObj.getTime())) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: {
                    Mission_id: time_mission,
                    Date: time_date.toString(),
                    Hours: hours,
                    Minutes: mins,
                    Message: time_message,
                    Type: "time-edit",
                    Timesheet_id: parseInt(document.getElementById("timesheet-id").value)
                },
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
        if (time_mission != 0 && time_date.length > 0 && hours <= 23 && hours > 0 &&
            hours.length != 0 && mins <= 59 && mins > 0 && mins.length != 0 && time_message.trim().length > 20 && !(dateObj.getTime() < startDateObj.getTime() || dateObj.getTime() > endDateObj.getTime())) {
            $.ajax({
                url: '/Mission/Volunteering_Timesheet',
                type: 'POST',
                data: {
                    Mission_id: time_mission,
                    Date: time_date.toString(),
                    Hours: hours,
                    Minutes: mins,
                    Message: time_message,
                    Type: "time"
                },
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
        var startdate = $('#mission-' + mission_id + '-startdate').val();
        var enddate = $('#mission-' + mission_id + '-enddate').val();
        goal_achieved = $('#mission-' + mission_id + '-goalachieved').val();
        endDateObj = new Date(enddate);
        startDateObj = new Date(startdate);
        dateObj = new Date(date);
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
        else if (actions > goal_achieved || actions < 0) {
            $(".action-notvalid").text("Invalid Action value cannot be greater than " + goal_achieved + " or less than 0")
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
        if (dateObj.getTime() < startDateObj.getTime() || dateObj.getTime() > endDateObj.getTime()) {
            $(".goal-date-invalid").text("Invalid Date input (must be between mission's " + startdate + " " + "and " + enddate)
            $(".goal-date-invalid").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".goal-date-invalid").addClass("d-none").removeClass("d-block")
        }
    }
    else {
        date = $('#time').find('#dateOfTimeStory').val();
        time_mission = parseInt(document.getElementsByClassName("time-mission")[0].value)
        var startdate = $('#mission-' + time_mission + '-startdate').val();
        var enddate = $('#mission-' + time_mission + '-enddate').val();
        endDateObj = new Date(enddate);
        startDateObj = new Date(startdate);
        dateObj = new Date(date);
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
        if (dateObj.getTime() < startDateObj.getTime() || dateObj.getTime() > endDateObj.getTime()) {
            $(".time-date-invalid").text("Invalid Date input (must be between mission's "+ startdate + " " + "and " + enddate )
            $(".time-date-invalid").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".time-date-invalid").addClass("d-none").removeClass("d-block")
        }
        if (time_date.length > 0) {
            $(".time-date-empty").addClass("d-none").removeClass("d-block")
        }
        else {
            $(".time-date-empty").addClass("d-block").removeClass("d-none")
        }
        if (hours > 23 || hours <= 0 || hours.length == 0) {
            $(".time-hours-valid").addClass("d-block").removeClass("d-none")
        }
        else {
            $(".time-hours-valid").addClass("d-none").removeClass("d-block")
        }
        if (mins > 59 || mins <= 0 || mins.length == 0) {
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
        $('.time-hours')[0].value = ""
        $('.time-min')[0].value = ""
        $('.time-message')[0].value = ""
        $('.time-date')[0].value = ""
        $(".time-mission-empty").addClass("d-none").removeClass("d-block")
        $(".time-date-invalid").addClass("d-none").removeClass("d-block")
        $(".time-date-empty").addClass("d-none").removeClass("d-block")
        $(".time-hours-valid").addClass("d-none").removeClass("d-block")
        $(".time-min-valid").addClass("d-none").removeClass("d-block")
        $(".time-message-empty").addClass("d-none").removeClass("d-block")
    }
    else {
        $(`#mission_goal`).removeAttr("disabled", "disabled")
        $('#action').value = ""
        $('#goal-message').value = ""
        $('#goal-date').value = ""
        $(".goal-mission").addClass("d-none").removeClass("d-block")
        $(".action-empty").addClass("d-none").removeClass("d-block")
        $(".action-notvalid").addClass("d-none").removeClass("d-block")
        $(".date-empty").addClass("d-none").removeClass("d-block")
        $(".message-empty").addClass("d-none").removeClass("d-block")
        $(".goal-date-invalid").addClass("d-none").removeClass("d-block")
    }
}


const edittimesheet = (id, mission, hours, minutes, message, type, action,date) => {
    if (type == "time") {
        $(`.time-mission option[value=${mission}]`).attr("selected", "selected")
        $(`.time-mission`).attr("disabled", "disabled")
        document.getElementsByClassName('time-hours')[0].value = parseInt(hours)
        document.getElementsByClassName('time-min')[0].value = parseInt(minutes.slice(0, 2))
        document.getElementsByClassName('time-message')[0].value = message
        document.getElementById("timesheet-id").value = id
        document.getElementById("dateOfTimeStory").value = date
    }
    else {
        $(`#mission-${mission}`).attr("selected", "selected")
        $(`#mission_goal`).attr("disabled", "disabled")
        document.getElementById('action').value = parseInt(action)
        document.getElementById('goal-message').value = message
        document.getElementById("timesheet-id").value = id
        document.getElementById("goal-date").value = date
    }
}

const deletetimesheet = (id) => {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this timesheet?</p>
</div>
<div class="modal-footer border-0 d-flex align-item-center justify-content-center">
<button type="button" class="btn btn-secondary me-2" data-bs-dismiss="modal">Cancel</button>
<button type="button" class="btn btn-danger" id="confirmDeleteButton">Yes, Delete</button>
</div>
</div>
</div>
</div>
`;
    $(document.body).append(html);
    $('#confirmDeleteModal').modal('show');
    $('#confirmDeleteButton').on('click', () => {
        $(`#timesheet-${id}`).remove()
        $.ajax({
            url: '/Mission/Volunteering_Timesheet',
            type: 'POST',
            /* data: { timesheet_id: parseInt(id), type: "time-delete" },*/
            data: {
                Timesheet_id: parseInt(id),
                Type: "time-delete"
            },
            success: function (result) {
            },
            error: function () {
                console.log("Error updating variable");
            }
        });
        $('#confirmDeleteModal').modal('hide');
    });
    $('#confirmDeleteModal').on('hidden.bs.modal', () => {
        $('#confirmDeleteModal').remove();
    });
}
$(document).ready(function () {

    var timetstable = $('#TimeTimesheet').DataTable({
        lengthChange: false,
        paging: false,
        searching: false,
        columns: [
            { "orderable": false }, // column 1
            { "orderData": [1], "orderSequence": ["desc", "asc"], "title": "Date" }, // column 2 (date)
            { "orderData": [2], "orderSequence": ["desc", "asc"], "title": "Hours" }, // column 3 (hours)
            { "orderable": false }, // column 4
            { "orderable": false },
        ],
        order: [[1, "desc"], [2, "asc"]], // default ordering for columns 2 and 3
        language: { // remove "Showing 1 to 3 of 3 entries" message
            "info": ""
        },
    });
    var goaltstable = $('#GoalTimesheet').DataTable({

        lengthChange: false,
        paging: false,
        searching: false,
        order: [], // remove default ordering
        info: false, // remove info message
        columns: [
            { "orderable": false }, // column 1
            { "orderData": [1] }, // column 2 (date)
            { "orderData": [2] }, // column 3 (hours)
            { "orderable": false }, // column 4  
        ],
        order: [[1,"desc"],[2,"asc"]] // default ordering for columns 2 and 3
    });
});