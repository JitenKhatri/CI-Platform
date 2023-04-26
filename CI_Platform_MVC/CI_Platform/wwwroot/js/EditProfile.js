var OldPassword
var NewPassword
var ConfirmPassword
var Skills = [];
var selectedSkillsInput = document.getElementById('selected_skills');
if (selectedSkillsInput.value != "") {
    Skills = selectedSkillsInput.value.split(',').map(id => parseInt(id));
}

var skills_name = []
var selectedskillnameinput = document.getElementById('selected_skill_names');
if (selectedskillnameinput.value != "") {
    skills_name = selectedskillnameinput.value.split(',');
}
function changePassword() {
    validate()
    if (OldPassword.length >= 4 && NewPassword.length >= 4 && ConfirmPassword.length >= 4 && NewPassword == ConfirmPassword) {
        $.ajax({
            url: '/UserAuthentication/ChangePassword',
            type: 'POST',
            data: {
                password: NewPassword
            },
            success: function (result) {
                showAlert("Password changed successfully");
                $("#change-password-modal").modal('hide')
                ClearModal();
            },
            error: function () {
                console.log("Error updating variable");
            }
        });
    }
}

function validate() {
    OldPassword = document.getElementById("old-password-input").value
    NewPassword = document.getElementById("new-password-input").value
    ConfirmPassword = document.getElementById("confirm-password-input").value

    if (OldPassword.length < 4) {
        $(".o-pass").addClass("d-block").removeClass("d-none")
    }
    else {
        $(".o-pass").addClass("d-none").removeClass("d-block")
    }
    if (NewPassword.length < 4) {
        $(".n-pass").addClass("d-block").removeClass("d-none")
    }
    else {
        $(".n-pass").addClass("d-none").removeClass("d-block")
    }
    if (ConfirmPassword.length < 4) {
        $(".c-pass").addClass("d-block").removeClass("d-none")
    }
    else {
        $(".c-pass").addClass("d-none").removeClass("d-block")
    }
    if (NewPassword != ConfirmPassword) {
        $(".m-pass").addClass("d-block").removeClass("d-none");
    }
    else {
        $(".m-pass").addClass("d-none").removeClass("d-block");
    }

}

function ClearModal() {
    document.getElementById("old-password-input").value = ""
    document.getElementById("new-password-input").value = ""
    document.getElementById("confirm-password-input").value = ""
}

function CascadeCity() {
    var country = $('.country').find(":selected").val()
    if (parseInt(country) != 0) {
        $.ajax({
            url: '/UserAuthentication/EditProfile',
            type: 'POST',
            data: { country: country },
            success: function (result) {
                $('.city').empty().append(result.cities.result)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}

function addskill (skill_id, skill_name) {
    var id = parseInt(skill_id.slice(6))
    if (!Skills.includes(id)) {
        $(`#${skill_id}`).css("background-color", "#0000000D")
        $('.selected-skills').append(`<span class="mt-1" id=${id}>` + skill_name + '</span>')
        Skills.push(id)
        skills_name.push(skill_name)
        document.getElementById('selected_skills').value += (Skills.length > 1 ? ',' : '') + id;
    }
    else {
        $(`#${skill_id}`).css("background-color", "white")
        $('.selected-skills').find(`#${id}`).remove()
        Skills.splice(Skills.indexOf(id), 1)
        skills_name.splice(skills_name.indexOf(skill_name), 1)
        document.getElementById('selected_skills').value = Skills.join(',');
    }
}

function saveskills() {
    $('.saved-skills').empty()
    skills_name.forEach((item, i) => {
        $('.saved-skills').append(`<span class="mt-1 ms-3">` + item + '</span>')
    })
}

function upload_profile_image() {
    var image = document.getElementById('profile-image').files[0]
    console.log('file type', image.type)
    if (image.type.startsWith('image/')) {
        var fr = new FileReader()
        fr.onload = () => {
            $('#old-profile-image').attr('src', fr.result)
        }
        fr.readAsDataURL(image)
    }
    else {
        showAlert("Please only upload an image");
    }
    
}
