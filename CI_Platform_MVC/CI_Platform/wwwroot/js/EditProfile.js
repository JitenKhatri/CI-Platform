var OldPassword
var NewPassword
var ConfirmPassword
let alertId = 0;
var skills = [];
var selectedSkillsInput = document.getElementById('selected_skills');
if (selectedSkillsInput.value != "") {
    skills = selectedSkillsInput.value.split(',').map(id => parseInt(id));
}

var skills_name = []
var selectedskillnameinput = document.getElementById('selected_skill_names');
if (selectedskillnameinput.value != "") {
    skills_name = selectedskillnameinput.value.split(',');
}
   

function showAlert(message) {
    const alert = document.createElement("div");
    alert.classList.add("alert");
    alert.setAttribute("id", "alert-" + alertId);
    alert.innerText = message;
    document.body.appendChild(alert);

    // Add CSS to position the alert at the top right and give it a reddish background color
    alert.style.position = "fixed";
    alert.style.top = (10 + alertId * 50) + "px";
    alert.style.right = "10px";
    alert.style.backgroundColor = "#ffcccc";

    // Increment the alert ID for the next call to showAlert
    alertId++;

    // Remove the alert after 5 seconds
    setTimeout(() => {
        alert.remove();
        alertId = 0;
    }, 5000);
}
function changePassword() {
    validate()
    if (OldPassword.length >= 4 && NewPassword.length >= 4 && ConfirmPassword.length >= 4) {
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

const addskill = (skill_id, skill_name) => {
    var id = parseInt(skill_id.slice(6))
    if (!skills.includes(id)) {
        $(`#${skill_id}`).css("background-color", "#0000000D")
        $('.selected-skills').append(`<span class="mt-1" id=${id}>` + skill_name + '</span>')
        skills.push(id)
        skills_name.push(skill_name)
        document.getElementById('selected_skills').value += (skills.length > 1 ? ',' : '') + id;
    }
    else {
        $(`#${skill_id}`).css("background-color", "white")
        $('.selected-skills').find(`#${id}`).remove()
        skills.splice(skills.indexOf(id), 1)
        skills_name.splice(skills_name.indexOf(skill_name), 1)
        document.getElementById('selected_skills').value = skills.join(',');
    }
}

const saveskills = () => {
    $('.saved-skills').empty()
    skills_name.forEach((item, i) => {
        $('.saved-skills').append(`<span class="mt-1 ms-3">` + item + '</span>')
    })
}

const upload_profile_image = () => {
    var image = document.getElementById('profile-image').files[0]
    var fr = new FileReader()
    fr.onload = () => {
        $('#old-profile-image').attr('src', fr.result)
    }
    fr.readAsDataURL(image)
}