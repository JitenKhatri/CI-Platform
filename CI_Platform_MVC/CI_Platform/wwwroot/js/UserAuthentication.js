function AddUser(form, e) {
    e.preventDefault();
    var formData = new FormData(form);
    $.ajax({
        type: 'POST',
        url: "/UserAuthentication/registration",
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
                window.location.href ='https://localhost:7064/UserAuthentication/login'
            }
        },
        error: function (error) {
            console.log(error)
            console.log("Error updating variable");
        }
    });      
}
