$(".toggle-sidebar,.overlay").click(function () {
	$("body").toggleClass("sidebar-open")
});
var currentPath = window.location.pathname;
$(".sidebar ul li.nav-item").each(function () {
    var menuItem = $(this);
    var url = menuItem.find("a.nav-link").attr("href");

    if (url === currentPath) {
        menuItem.addClass("active");
    } else {
        menuItem.removeClass("active");
    }
});
$(document).bind("ajaxSend", function () {
    $('.loader').addClass('d-block').removeClass('d-none');
}).bind("ajaxComplete", function () {
    $('.loader').addClass('d-none').removeClass('d-block');
});

$(window).on('popstate', function (event) {
    window.location.reload();
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

    var CMSTable = $('#CMS-table').DataTable({
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
    $('#cms-search-input').on('keyup', function () {
        CMSTable.search($(this).val()).draw();
    });

    var BannerTable = $('#Banners-table').DataTable({
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
    $('#banner-search-input').on('keyup', function () {
        BannerTable.search($(this).val()).draw();
    });
});

var ThemeName
var ThemeStatus
function ValidateAddTheme() {
    ThemeName = document.getElementsByClassName("theme-name")[0].value;
    ThemeStatus = parseInt(document.querySelector('input[name="status"]:checked').value);
    /*ThemeStatus = parseInt(document.getElementsByClassName("theme-status")[0].value);*/
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
                url: '/Admin/MissionThemeCrud',
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
    ThemeStatus = parseInt(document.querySelector('input[name="theme-edit-status"]:checked').value);
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
            url: '/Admin/MissionThemeCrud',
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
            url: '/Admin/MissionThemeCrud',
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
    modal.find(`input[name="theme-edit-status"][value="${status}"]`).prop("checked", true);
    /*modal.find(`.theme-status option[value="${status}"]`).attr("selected", "selected");*/
    document.getElementById("theme-id").value = themeid;
}

function ClearThemeModal() {
    document.querySelector('.theme-name').value = "";
    // Uncheck all radio buttons with the name "status"
    document.querySelectorAll('input[name="status"]').forEach(function(radio) {
        radio.checked = false;
    });
    // Check the first radio button with the name "status"
    document.querySelector('input[name="status"]:first-of-type').checked = true;
}


//Skillpage js
var SkillName
var SkillStatus
function ValidateAddSkill() {
    SkillName = document.getElementsByClassName("skill-name")[0].value;
    SkillStatus = parseInt(document.querySelector('input[name="skill-status"]:checked').value);
    /*SkillStatus = parseInt(document.getElementsByClassName("skill-status")[0].value);*/
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
                url: '/Admin/MissionSkillCrud',
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
    SkillStatus = parseInt(document.querySelector('input[name="skill-edit-status"]:checked').value);
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
function Editskill(action) {
    ValidateEditSkill();
    if (SkillStatus != 2 && SkillName.length > 4) {
        $.ajax({
            url: '/Admin/MissionSkillCrud',
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
    modal.find(`input[name="skill-edit-status"][value="${status}"]`).prop("checked", true);
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
            url: '/Admin/MissionSkillCrud',
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
        url: '/Admin/MissionApplications',
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
                $('.MA-approve-' + id).addClass("bi-check-circle-fill");
                $('.MA-approve-' + id).removeClass("bi-check-circle");
                $('.MA-decline-' + id).removeClass("bi-x-circle-fill");
                $('.MA-decline-' + id).addClass("bi-x-circle");
            }
            else {
                toastr.error('Application Declined Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $('.MA-decline-' + id).addClass("bi-x-circle-fill");
                $('.MA-decline-' + id).removeClass("bi-x-circle");
                $('.MA-approve-' + id).removeClass("bi-check-circle-fill");
                $('.MA-approve-' + id).addClass("bi-check-circle");
            }
          
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
    
}

function ChangeStoryStatus(id, Action) {
    $.ajax({
        url: '/Admin/StoryCrud',
        type: 'POST',
        data: {
            StoryId: id,
            Action: Action
        },
        success: function (result) {
            if (window.location.href == 'https://localhost:7064/Admin/StoryCrud') {
                if (Action == 1) {
                    toastr.success('Story Approved Successfully!', {
                        "positionClass": "toast-top-center",
                        progressBar: true,
                        timeOut: 3000,
                        closeButton: true,
                    });
                    $('.story-publish-' + id).addClass("bi-check-circle-fill");
                    $('.story-publish-' + id).removeClass("bi-check-circle");
                    $('.story-decline-' + id).removeClass("bi-x-circle-fill");
                    $('.story-decline-' + id).addClass("bi-x-circle");
                }
                else {
                    toastr.error('Stored Declined Successfully!', {
                        "positionClass": "toast-top-center",
                        progressBar: true,
                        timeOut: 3000,
                        closeButton: true,
                    });
                    $('.story-decline-' + id).addClass("bi-x-circle-fill");
                    $('.story-decline-' + id).removeClass("bi-x-circle");
                    $('.story-publish-' + id).removeClass("bi-check-circle-fill");
                    $('.story-publish-' + id).addClass("bi-check-circle");
                }
            }
            else {
                if (Action == 1) {
                    toastr.success('Story Approved Successfully!', {
                        "positionClass": "toast-top-center",
                        progressBar: true,
                        timeOut: 3000,
                        closeButton: true,
                    });
                    $('.publish-btn').prop('disabled', true).text('Published...');
                    $('.decline-btn').removeClass('disabled').prop('disabled', false).html('Decline<i class="bi bi-x-circle text-danger fs-5 story-@item.StoryId ms-2"></i>');
                   
                }
                else {
                    toastr.error('Stored Declined Successfully!', {
                        "positionClass": "toast-top-center",
                        progressBar: true,
                        timeOut: 3000,
                        closeButton: true,
                    });
                    $('.decline-btn').prop('disabled', true).text('Declined...');
                    $('.publish-btn').removeClass('disabled').prop('disabled', false).html('Publish<i class="bi bi-check-circle text-success fs-5 story-@item.StoryId ms-2"></i>');
                }
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    });

}

function DeleteStory(StoryId,Action) {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this Story?</p>
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
        $.ajax({
            url: '/Admin/StoryCrud',
            type: 'POST',
            data: {
                StoryId: StoryId,
                Action: Action
            },
            success: function (result) {
                if (window.location.href === 'https://localhost:7064/Admin/StoryCrud') {
                    var row = $(`#story-${StoryId}`);
                    row.remove();
                    $('#Story-table').DataTable().row(row).remove().draw();
                }
                else {
                    toastr.error('Stored Deleted Successfully!', {
                        "positionClass": "toast-top-center",
                        progressBar: true,
                        timeOut: 3000,
                        closeButton: true,
                    });
                    $('.delete-btn').prop('disabled', true).text('Deleted...');
                    window.location.href = 'https://localhost:7064/Admin/StoryCrud';
                }
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


function AddUserModal(UserId) {
    $.ajax({
        url: '/Admin/AddUserPartial',
        type: 'POST',
        data: {
            UserId: UserId
        },
        success: function (result) {
            $('.crud-container').empty();
            $('.crud-container').append(result);
            history.pushState({ page: 'add-user' }, '', '/Admin');
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
    
}

function AddUser(form, e) {
    e.preventDefault();
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: "/Admin/Index",
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.message == "Email exists") {
                if ($("#Email").val().length > 0) {
                    $('#valid-email-error').removeClass('d-none').addClass('d-block');
                    $("#Email").on('input', function () {
                        if ($("#Email").val().lenght != 0) {
                            $("#valid-email-error").removeClass('d-block').addClass('d-none');
                            flag = true;
                        }
                    });
                }
            }
            else {
                toastr.success('User Saved Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $("#AddUserForm")[0].reset();
                if (formData.UserId != 0) {
                        window.location.reload()
                }
            }
            
        },
        error: function (error) {
            console.log("Error updating variable");
        }
    });
}

function CascadeCity() {
    var country = $('.country').find(":selected").val()
    if (parseInt(country) != 0) {
        $.ajax({
            url: '/Admin/AddUserPartial',
            type: 'POST',
            data: { CountryId: country },
            success: function (result) {
                $('.city').empty().append(result.cities.result)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}

function upload_profile_image() {
    var image = document.getElementById('profile-image').files[0]
    if (image.type.startsWith('image/')) {
        var fr = new FileReader()
        fr.onload = () => {
            $('#old-profile-image').attr('src', fr.result)
        }
        fr.readAsDataURL(image)
    }
    else {
        toastr.error('Please Upload an Image file only!', {
            "positionClass": "toast-top-center",
            progressBar: true,
            timeOut: 3000,
            closeButton: true,
        });
        var fileInput = document.getElementById('profile-image');
        fileInput.value = '';
    }
}

function DeleteUser(id){
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this User?</p>
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
        $.ajax({
            url: '/Admin/Index',
            type: 'POST',
            data: {
                UserId: id,
                Action: "Delete"
            },
            success: function (result) {
                var row = $(`#user-${id}`);
                row.remove();
                $('#Users-table').DataTable().row(row).remove().draw();
                toastr.error('User Deleted Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
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


function AddCMSModel(CMSPageId) {
    $.ajax({
        url: '/Admin/AddCMSPartial',
        data: {
            CMSPageId: CMSPageId
        },
        type: 'POST',
        success: function (result) {
            $('.crud-container').empty();
            $('.crud-container').append(result);
            history.pushState({ page: 'add-cms' }, '', '/Admin/CMSCrud');
        },
        error: function () {
            console.log("Error updating variable");
        }
    });

}

function AddCMS(form, e) {
    e.preventDefault();
    var formData = new FormData(form);
    var description = CKEDITOR.instances.cmseditorhtml.getData();
    if (description.length < 20) {
        $(".cms-description-required").addClass("d-block").removeClass("d-none")
    }
    else {
        $(".cms-description-required").addClass("d-none").removeClass("d-block")
        formData.set('Description', CKEDITOR.instances.cmseditorhtml.getData());
        $.ajax({
            type: 'POST',
            url: "/Admin/CMSCrud",
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                toastr.success('CMS Page Saved Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
                $("#cmsform")[0].reset();
                if (formData.CMSPageId != 0) {
                    window.location.reload();
                }
            },
            error: function (error) {
                console.log("Error updating variable");
            }
        });
    }
    
}

function DeleteCMS(id) {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this CMS Page?</p>
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
        $.ajax({
            url: '/Admin/CMSCrud',
            type: 'POST',
            data: {
                CMSPageId: id,
                Action: "Delete"
            },
            success: function (result) {
                var row = $(`#cms-${id}`);
                row.remove();
                $('#CMS-table').DataTable().row(row).remove().draw();
                toastr.error('CMS Page Deleted Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
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

function AddMissionModel(MissionId) {
    $.ajax({
        url: '/Admin/AddMissionPartial',
        data: {
            MissionId: MissionId
        },
        type: 'POST',
        success: function (result) {
            $('.crud-container').empty();
            $('.crud-container').append(result);
            history.pushState({ page: 'add-mission' }, '', '/Admin/MissionCrud');
        },
        error: function () {
            console.log("Error updating variable");
        }
    });

}

function DeleteMission(id) {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this Mission?</p>
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
        $.ajax({
            url: '/Admin/MissionCrud',
            type: 'POST',
            data: {
                MissionId: id,
                Action: "Delete"
            },
            success: function (result) {
                var row = $(`#mission-${id}`);
                row.remove();
                $('#Missions-table').DataTable().row(row).remove().draw();
                toastr.error('Mission Deleted Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
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

function AddBannerModel(BannerId) {
    $.ajax({
        url: '/Admin/AddBannerPartial',
        data: {
            BannerId: BannerId
        },
        type: 'POST',
        success: function (result) {
            $('.crud-container').empty();
            $('.crud-container').append(result);
            history.pushState({ page: 'add-banner' }, '', '/Admin/BannerCrud');
        },
        error: function () {
            console.log("Error updating variable");
        }
    });

}

function DeleteBanner(id) {
    const html = `
<div class="modal fade" id="confirmDeleteModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
<div class="modal-dialog modal-dialog-centered modal-delete" >
<div class="modal-content">
<div class="modal-header border-0 d-flex justify-content-center">
<h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Deletion</h5>
<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body text-center">
<p class="mb-0">Are you sure you want to delete this Banner?</p>
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
        $.ajax({
            url: '/Admin/BannerCrud',
            type: 'POST',
            data: {
                BannerId: id,
                Action: "Delete"
            },
            success: function (result) {
                var row = $(`#banner-${id}`);
                row.remove();
                $('#Banners-table').DataTable().row(row).remove().draw();
                toastr.error('Banner Deleted Successfully!', {
                    "positionClass": "toast-top-center",
                    progressBar: true,
                    timeOut: 3000,
                    closeButton: true,
                });
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

