$(".toggle-sidebar,.overlay").click(function () {
	$("body").toggleClass("sidebar-open")
});
var MissionThemeTable
var MissionApplicationTable
$(document).ready(function () {

    var Usertable = $('#Users-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#search-input').on('keyup', function () {
        Usertable.search($(this).val()).draw();
    });

    var MissionTable = $('#Missions-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#mission-search-input').on('keyup', function () {
        MissionTable.search($(this).val()).draw();
    });

    MissionThemeTable = $('#Missiontheme-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        lengthMenu: [[5, 10, 15, -1], [5, "5+5", "5+5", "All"]],
        pageLength: 5,
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#theme-search-input').on('keyup', function () {
        MissionThemeTable.search($(this).val()).draw();
    });

    MissionApplicationTable = $('#Missionapplication-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        lengthMenu: [[5, 10, 15, -1], [5, "5+5", "5+5", "All"]],
        pageLength: 5,
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#mission-application-search-input').on('keyup', function () {
        MissionApplicationTable.search($(this).val()).draw();
    });
    var MissionSkillTable = $('#Missionskill-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#skill-search-input').on('keyup', function () {
        MissionSkillTable.search($(this).val()).draw();
    });

    var StoryTable = $('#Story-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#story-search-input').on('keyup', function () {
        StoryTable.search($(this).val()).draw();
    });
});

var ThemeName
var ThemeStatus
function ValidateAddTheme() {
    ThemeName = document.getElementsByClassName("theme-name")[0].value;
    ThemeStatus = parseInt(document.getElementsByClassName("theme-status")[0].value);
    if (ThemeStatus == 2) {
        $(".theme-status-empty").addClass("d-block").removeClass("d-none")
    }
    else {
        $(".theme-status-empty").addClass("d-none").removeClass("d-block")
    }
    if (ThemeName.length > 0) {
        $(".theme-name-empty").addClass("d-none").removeClass("d-block")
    }
    else {
        $(".theme-name-empty").addClass("d-block").removeClass("d-none")
    }
}
function AddTheme(action) {
    ValidateAddTheme()
        if (ThemeStatus != 2 && ThemeName.length > 4) {
            $.ajax({
                url: '/Admin/Home/MissionThemeCrud',
                type: 'POST',
                data: {
                    ThemeName: ThemeName,
                    Status: ThemeStatus
                },
                success: function (result) {
                    if (result.view) {
                        $(".missionThemes").append(result.view.result)
                        $("#AddTheme").modal('hide')
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        } 
}
function ValidateEditTheme() {
    var modal = $("#EditTheme");
    ThemeName = modal.find(".theme-name").val();
    ThemeStatus = parseInt(modal.find(".theme-status").val());
    if (ThemeStatus == 2) {
        modal.find(".theme-status-empty").addClass("d-block").removeClass("d-none")
    }
    else {
        modal.find(".theme-status-empty").addClass("d-none").removeClass("d-block")
    }
    if (ThemeName.length > 0) {
        modal.find(".theme-name-empty").addClass("d-none").removeClass("d-block")
    }
    else {
        modal.find(".theme-name-empty").addClass("d-block").removeClass("d-none")
    }
}
function Edittheme() {
    ValidateEditTheme();
    if (ThemeStatus != 2 && ThemeName.length > 4) {
        $.ajax({
            url: '/Admin/Home/MissionThemeCrud',
            type: 'POST',
            data: {
                ThemeName: ThemeName,
                Status: ThemeStatus,
                ThemeId: parseInt(document.getElementById("theme-id").value),
                Action: "Edit"
            },
            success: function (result) {
                if (result.view) {
                    $(`#theme-${parseInt(document.getElementById("theme-id").value)}`).replaceWith(result.view.result)
                    $("#EditTheme").modal('hide')
                }
            },
            error: function () {
                console.log("Error updating variable");
            }
        });
    }
}
const DeleteTheme = (id) => {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this theme?</p>
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
/*        $(`#theme-${id}`).remove()*/
        $.ajax({
            url: '/Admin/Home/MissionThemeCrud',
            type: 'POST',
            data: {
                ThemeId: id,
                Action: "Delete"
            },
            success: function (result) {
                var row = $(`#theme-${id}`);
                row.remove();
                $('#Missiontheme-table').DataTable().row(row).remove().draw();
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

function EditTheme(themeid, title, status) {
    // Find the modal with the id "editmodal"
    var modal = $("#EditTheme");
    // Find the input element with the class "theme-name" inside the modal and set its value
    modal.find(".theme-name").val(title);
    modal.find(`.theme-status option[value="${status}"]`).attr("selected", "selected");
    document.getElementById("theme-id").value = themeid;
}

function ClearThemeModal() {
    document.getElementsByClassName("theme-name")[0].value = "";
    // Select the disabled option that should remain selected
    var $selectedOption = $('.theme-status option:selected:disabled');

    // Remove the "selected" attribute from all non-disabled options
    $('.theme-status option:not(:disabled)').removeAttr('selected');

    // Set the "selected" attribute on the selected disabled option
    $selectedOption.attr('selected', 'selected');
}


//Skillpage js
var SkillName
var SkillStatus
function ValidateAddSkill() {
    SkillName = document.getElementsByClassName("skill-name")[0].value;
    SkillStatus = parseInt(document.getElementsByClassName("skill-status")[0].value);
    if (SkillStatus == 2) {
        $(".skill-status-empty").addClass("d-block").removeClass("d-none")
    }
    else {
        $(".skill-status-empty").addClass("d-none").removeClass("d-block")
    }
    if (SkillName.length > 0) {
        $(".skill-name-empty").addClass("d-none").removeClass("d-block")
    }
    else {
        $(".skill-name-empty").addClass("d-block").removeClass("d-none")
    }
}
function AddSkill(action) {
    ValidateAddSkill()
    if (action == "Add") {
        if (SkillStatus != 2 && SkillName.length > 4) {
            $.ajax({
                url: '/Admin/Home/MissionSkillCrud',
                type: 'POST',
                data: {
                    SkillName: SkillName,
                    Status: SkillStatus
                },
                success: function (result) {
                    if (result.view) {
                        $(".missionskills").append(result.view.result)
                        $("#AddSkill").modal('hide')
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        }
    }
}

function ValidateEditSkill() {
    var modal = $("#EditSkill");
    SkillName = modal.find(".skill-name").val();
    SkillStatus = parseInt(modal.find(".skill-status").val());
    if (SkillStatus == 2) {
        modal.find(".skill-status-empty").addClass("d-block").removeClass("d-none")
    }
    else {
        modal.find(".skill-status-empty").addClass("d-none").removeClass("d-block")
    }
    if (SkillName.length > 0) {
        modal.find(".skill-name-empty").addClass("d-none").removeClass("d-block")
    }
    else {
        modal.find(".skill-name-empty").addClass("d-block").removeClass("d-none")
    }
}
function Editskill() {
    ValidateEditSkill();
    if (SkillStatus != 2 && SkillName.length > 4) {
        $.ajax({
            url: '/Admin/Home/MissionSkillCrud',
            type: 'POST',
            data: {
                SkillName: SkillName,
                Status: SkillStatus,
                SkillId: parseInt(document.getElementById("skill-id").value),
                Action: "Edit"
            },
            success: function (result) {
                if (result.view) {
                    $(`#skill-${parseInt(document.getElementById("skill-id").value)}`).replaceWith(result.view.result)
                    $("#EditSkill").modal('hide')
                }
            },
            error: function () {
                console.log("Error updating variable");
            }
        });
    }
}
function EditSkill(skillid, name, status) {
    // Find the modal with the id "editmodal"
    var modal = $("#EditSkill");
    // Find the input element with the class "theme-name" inside the modal and set its value
    modal.find(".skill-name").val(name);
    modal.find(`.skill-status option[value="${status}"]`).attr("selected", "selected");
    document.getElementById("skill-id").value = skillid
}

const DeleteSkill = (id) => {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this Skill?</p>
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
        /*        $(`#theme-${id}`).remove()*/
        $.ajax({
            url: '/Admin/Home/MissionSkillCrud',
            type: 'POST',
            data: {
                SkillId: id,
                Action: "Delete"
            },
            success: function (result) {
                var row = $(`#skill-${id}`);
                row.remove();
                $('#Missionskill-table').DataTable().row(row).remove().draw();
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

function MissionApplication(id, Action) {
    $.ajax({
        url: '/Admin/Home/MissionApplications',
        type: 'POST',
        data: {
            MissionApplicationId: id,
            Action: Action
        },
        success: function (result) {
            if (Action == 1) {
                toastr.success('Application Approved Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $('.MA-' + id).addClass("bi-check-circle-fill");
                $('.MA-' + id).removeClass("bi-check-circle");
            }
            else {
                toastr.error('Application Declined Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $('.MA-' + id).addClass("bi-x-circle-fill");
                $('.MA-' + id).removeClass("bi-x-circle");
            }
          
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
    
}

function ChangeStoryStatus(id, Action) {
    $.ajax({
        url: '/Admin/Home/StoryCrud',
        type: 'POST',
        data: {
            StoryId: id,
            Action: Action
        },
        success: function (result) {
            if (Action == 1) {
                toastr.success('Story Approved Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $('.story-' + id).addClass("bi-check-circle-fill");
                $('.story-' + id).removeClass("bi-check-circle");
            }
            else {
                toastr.error('Stored Declined Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $('.story-' + id).addClass("bi-x-circle-fill");
                $('.story-' + id).removeClass("bi-x-circle");
            }

        },
        error: function () {
            console.log("Error updating variable");
        }
    });

}