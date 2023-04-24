////let cities = []
////let countries = []
////let themes = []
////let skills = []

//$('.form-check-input').on('change', function () {
//    const dropdown = $(this).closest('.dropdown');
//    const dropdownTitle = dropdown.find('.dropdown-toggle').text();
//    const value = $(this).data('value');
//    const selectedItemsRow = $('#selected-items-row');
//    const itemId = $(this).attr('id');

//    // Create a badge element to display the selected item
//    const badge = $('<span class="badge text-bg-light ml-2 "></span>').text(value);
//    const cross = $('<span class="badge text-bg-light ml-2 d-inline" id="cross-btn"></span>').html('&times;');

//    if ($(this).is(':checked')) {
//          cross.addClass('d-inline');
//        badge.append(cross);
//        selectedItemsRow.append(badge);
//        // Cross button click event listener
//        cross.on('click', function () {
//            //    $(this).closest('.badge').remove();
//            dropdown.find(`input[data-value="${value}"]`).prop('checked', false);
//            badge.remove();
//            remove_badges(dropdown.find(`input[id="${itemId}"]`));

//        });
//        // Clear all button click event listener
//        $('#clear-all-btn').on('click', function () {
//            $('.form-check-input').prop('checked', false);
//            selectedItemsRow.empty();
//            clear_all();
//        });
//    } else {
//        // Checkbox is unchecked, remove its corresponding badge
//        // If checkbox is unchecked, remove its corresponding badge
//        const badge = selectedItemsRow.find(`span:contains(${value})`);
//        badge.remove();
//    }
//});
function Cityfilter(name) {
    $('.Stories').empty()
    if (document.getElementById(name).checked) {
        if (!cities.includes(name)) {
            cities.push(name)
        }
    }
    else {
        if (cities.includes(name)) {
            cities.splice(cities.indexOf(name), 1)
        }
    }
    $.ajax({
        url: '/Story/Story',
        type: 'POST',
        data: { Countries: countries, Cities: cities, Themes: themes, Skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
            $('.Stories').html(result);
            $('.pagination-link').css('display', 'none');
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}

function Countryfilter(name, dataid) {
    $('.Stories').empty()
    const country = document.getElementById(name)
    if (country.checked) {
        if (!countries.includes(name)) {
            countries.push(name)

        }
    }
    else {
        if (countries.includes(name)) {
            countries.splice(countries.indexOf(name), 1)


        }
    }
    $.ajax({
        url: '/Story/CityCascade',
        type: 'Post',
        data: { countryid: dataid },
        success: function (result) {
            console.log(result);
            if (result.cities.length > 0) {
                Loadcities(result.cities);
            }
            else {
                Loadcities(result.cities)

            }
            $.ajax({
                url: '/Story/Story',
                type: 'POST',
                data: { Countries: countries, Cities: cities, Themes: themes, Skills: skills },
                success: function (result) {
                    if (result == "") {
                        $('.page-not-found').css('display', 'block');
                    }
                    else {
                        $('.page-not-found').css('display', 'none');
                    }
                    $('.Stories').html(result);
                    $('.pagination-link').css('display', 'none');
                },
                error: function () {
                    console.log("Error updating variable");
                }
            });
        },
        error: function () {
            console.log("Error updating variable");
        }

    });

}
const Loadcities = (cities) => {
    $('.city').empty()
    $.each(cities, function (i, item) {
        var data = "<span>" +
            "<input type='checkbox' id='' data-value='' class='form-check-input'> " +
            " <label for=''> " + item.name + "</label>" + "</span>" + "<br>";
        $('.city').append(data)
        $('.city span').each(function () {
            $('.city span').eq(i).find('input').attr('id', item.name)
            $('.city span').eq(i).find('label').attr('for', item.name)
            $('.city span').eq(i).find('input').attr('data-value', item.name)
            $('.city span').eq(i).find('input').attr('onchange', `Cityfilter('${item.name}')`)


        })
    })
}

const Themefilter = (name) => {
    $('.Stories').empty()
    if (document.getElementById(name).checked) {
        if (!themes.includes(name)) {
            themes.push(name)
        }
    }
    else {
        if (themes.includes(name)) {
            themes.splice(themes.indexOf(name), 1)
        }
    }
    $.ajax({
        url: '/Story/Story',
        type: 'POST',
        data: { Countries: countries, Cities: cities, Themes: themes, Skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
            $('.Stories').html(result);
            $('.pagination-link').css('display', 'none');
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

const Skillfilter = (name) => {
    $('.Stories').empty()
    if (document.getElementById(name).checked) {
        if (!skills.includes(name)) {
            skills.push(name)
        }
    }
    else {
        if (skills.includes(name)) {
            skills.splice(skills.indexOf(name), 1)
        }
    }
    $.ajax({
        url: '/Story/Story',
        type: 'POST',
        data: { Countries: countries, Cities: cities, Themes: themes, Skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
            $('.Stories').html(result);
            $('.pagination-link').css('display', 'none');
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

function search() {
    var input = document.getElementById("search-input").value.toLowerCase();
    $('.Stories').empty()
    $.ajax(
        {
            url: '/Story/Story',
            type: 'POST',
            data: { SearchText :input, Cities: cities, Themes: themes, Skills: skills },
            success: function (result) {
                if (result == "") {
                    $('.page-not-found').css('display', 'block');
                }
                else {
                    $('.page-not-found').css('display', 'none');
                }
                $('.Stories').html(result);
                $('.pagination-link').css('display', 'none');
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
}
function clear_all() {

    countries = []
    cities = []
    themes = []
    skills = []
    $('.Stories').empty()
    $.ajax({
        url: '/Story/Story',
        type: 'POST',
        data: { Countries: countries, Cities: cities, Themes: themes, Skills: skills },
        success: function (result) {
            $('.Stories').html(result);
            $('.page-not-found').css('display', 'none');
            $('.pagination-link').css('display', 'block');
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

function remove_badges(input) {
    const id = input.attr('id');
    console.log(id)
    if (cities.includes(id)) {
        cities.splice(cities.indexOf(id), 1)
    }
    if (countries.includes(id)) {
        countries.splice(countries.indexOf(id), 1)
    }
    if (themes.includes(id)) {
        themes.splice(themes.indexOf(id), 1)
    }
    if (skills.includes(id)) {
        skills.splice(skills.indexOf(id), 1)

    }
    $('.missions').empty()
    $.ajax({
        url: '/Story/Story',
        type: 'POST',
        data: { Countries: countries, Cities: cities, Themes: themes, Skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
                $('.pagination-link').css('display', 'none');
            }
            else {
                $('.page-not-found').css('display', 'none');
                $('.pagination-link').css('display', 'block');
            }
            $('.Stories').html(result);
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
const editor = (StoryId) => {
    CKEDITOR.replace(`editor-${StoryId}`, {
        maxLength: 40000,
        removeButtons: ['About', 'Cut', 'Copy', 'Paste', 'Link', 'Unlink', 'Anchor', 'Indent', 'Outdent', 'NumberedList', 'BulletedList']
    });
}
//Dropzone.autoDiscover = false;
//const DRopzone = (StoryId) => {
//    var myDropzone = new Dropzone(`#myDropzone-${StoryId}`, {
//        url: "/Story/ShareStory",
//        maxFiles: 20,
//        maxFilesize: 4,
//        acceptedFiles: ".jpeg,.jpg,.png",
//        addRemoveLinks: false,
//        uploadMultiple: true,
//        parallelUploads: 20,
//        dictRemoveFile: "Remove",
//        autoProcessQueue: false,
//        dictDefaultMessage: "Drop files here or click to upload",
//        dictInvalidFileType: "Invalid file type. Only JPEG, JPG and PNG are allowed.",
//        dictFileTooBig: "File size is too big. Maximum file size allowed is 4MB.",
//        dictMaxFilesExceeded: "You can only upload a maximum of 20 files.",
//        previewTemplate: $("#imagePreview").html(),
//        init: function () {
//            this.on("addedfile", function (file) {
//                // Show the remove button when a file is added
//                file.previewElement.querySelector(".btn-remove").style.display = "block";
//                file.previewElement.querySelector(".btn-remove").addEventListener("click", function () {
//                    // Remove the file from Dropzone
//                    myDropzone.removeFile(file);
//                });
//            });
//            this.on("maxfilesexceeded", function (file) {
//                this.removeFile(file);
//                showAlert("You can only upload a maximum of 20 files.");
//            });

//            let myDropzone = this;
//            var imageurls = $(".imageurl").get();
//            imageurls.forEach(i => {
//                //let mockFile = { name: "Filename 2", size: 12345 };
//                //myDropzone.displayExistingFile(mockFile, i.value);
//                console.log(i.value);
//                fetch(i.value)
//                    .then(response => response.blob())
//                    .then(blob => {
//                        var file = new File([blob], i.value, { type: blob.type });
//                        var files = myDropzone.getAcceptedFiles();
//                        files.push(file);
//                        myDropzone.addFile(file); // add the file to the upload queue
//                    })
//                    .catch(error => console.error(error));
//            });// manually trigger processing of the preselected file
//            myDropzone.on("addedfile", function (file) {
//                console.log("Added file:", file);
//                file.previewElement.querySelector(".btn-remove").addEventListener("click", function () {
//                    // Remove the file from Dropzone
//                    myDropzone.removeFile(file);
//                });
//                myDropzone.on("removedfile", function (file) {
//                    console.log("Removed file:", file)

//                });
        
//        }
//    });
//}
Dropzone.autoDiscover = false;

const DRopzone = (StoryId) => {
    var myDropzone = new Dropzone(`#myDropzone-${StoryId}`, {
        url: "/Story/ShareStory",
        maxFiles: 20,
        maxFilesize: 4,
        acceptedFiles: ".jpeg,.jpg,.png",
        addRemoveLinks: false,
        uploadMultiple: true,
        parallelUploads: 20,
        dictRemoveFile: "Remove",
        autoProcessQueue: false,
        dictDefaultMessage: "Drop files here or click to upload",
        dictInvalidFileType: "Invalid file type. Only JPEG, JPG and PNG are allowed.",
        dictFileTooBig: "File size is too big. Maximum file size allowed is 4MB.",
        dictMaxFilesExceeded: "You can only upload a maximum of 20 files.",
        previewTemplate: $("#imagePreview").html(),
        init: function () {
            this.on("addedfile", function (file) {
                // Show the remove button when a file is added
                file.previewElement.querySelector(".btn-remove").style.display = "block";
                file.previewElement.querySelector(".btn-remove").addEventListener("click", function () {
                    // Remove the file from Dropzone
                    myDropzone.removeFile(file);
                });
            });
            this.on("maxfilesexceeded", function (file) {
                this.removeFile(file);
                showAlert("You can only upload a maximum of 20 files.");
            });

            let myDropzone = this;
            var imageurls = $(".imageurl").get();
            imageurls.forEach(i => {
                //let mockFile = { name: "Filename 2", size: 12345 };
                //myDropzone.displayExistingFile(mockFile, i.value);
                console.log(i.value);
                if (i.dataset.storyId == StoryId) {
                    fetch(i.value)
                        .then(response => response.blob())
                        .then(blob => {
                            var file = new File([blob], i.value, { type: blob.type });
                            var files = myDropzone.getAcceptedFiles();
                            files.push(file);
                            myDropzone.addFile(file); // add the file to the upload queue
                        })
                        .catch(error => console.error(error));
                }});
        // manually trigger processing of the preselected file
            myDropzone.on("addedfile", function (file) {
                console.log("Added file:", file);
                file.previewElement.querySelector(".btn-remove").addEventListener("click", function () {
                    // Remove the file from Dropzone
                    myDropzone.removeFile(file);
                });
            });
            myDropzone.on("removedfile", function (file) {
                console.log("Removed file:", file);
            });
        }
    });
}

function validateYouTubeUrls(urls) {
    const regex = /^https?:\/\/(?:www\.|m\.)?youtube\.com\/watch\?v=([a-zA-Z0-9_-]{11})$/;
  

    for (const url of urls) {
        if (!regex.test(url)) {
            return false;
        }
    }
    if (urls.length > 20) {
        return false;
    }
    else {
        return true;
    }
}
var edit_mission
var edit_title
var edit_date
var Current_date
var Comparedate
var edit_mystory
var Video_url

function editstory(type,storyId,missionId) {
    validateEdit(storyId);

    if (edit_title.trim().length > 50 && edit_title.trim().length < 80 && edit_date.length != 0
        && Date.parse(Current_date) >= Date.parse(Comparedate) && edit_mystory.trim().length > 70 && edit_mystory.trim().length < 40000 && validateYouTubeUrls(Video_url)) {

        var formData = new FormData();

        var myDropzone = Dropzone.getElement(`#myDropzone-${storyId}`).dropzone;
  
        myDropzone.on("sendingmultiple", function (files, xhr, formData) {
            for (var i = 0; i < files.length; i++) {
                formData.append('media', files[i]);
            }
            formData.append('MissionId', missionId);
            formData.append('Title', edit_title);
            formData.append('StoryId', storyId);
            formData.append('PublishedDate', edit_date);
            formData.append('StoryDescription', edit_mystory);
            formData.append('Type', type);
            for (var j = 0; j < Video_url.length; j++) {
                formData.append('VideoUrls', Video_url[j])
            }
        });
        myDropzone.processQueue();
        // Send the AJAX request with the FormData object after the files have been uploaded
        myDropzone.on("success", function (file, response) {
            showAlert("Story edited successfully")
            $('.preview-link').css('display', 'block');
        });

        // Manually process queue to upload files
       
    }
}
function validateEdit(storyId) {
/*    edit_mission = parseInt($('.form-select').find(':selected').val())*/
    edit_title = $(`#edit-${storyId}`).find('.story_title').val()
    edit_date = $(`#edit-${storyId}`).find('#publish_date').val()
    Current_date = new Date()
    Comparedate = new Date($(`#edit-${storyId}`).find('#publish_date').val())
    edit_mystory = CKEDITOR.instances[`editor-${storyId}`].getData();
    const urlString = $(`#edit-${storyId}`).find('.video-url').val();
    const urls = urlString.trim().split(/\r?\n/);
    Video_url = urls;
/*    Video_url = $(`#edit-${storyId}`).find('.video-url').val().split("\n");*/
    // Define validation conditions
    const conditions = [
        { message: "Title should be at least 50 characters long", test: edit_title.trim().length < 50 },
        { message: "Title can have maximum 80 characters", test: edit_title.trim().length > 80 },
        { message: "Please enter a valid date", test: edit_date.length == 0 || Date.parse(Current_date) <= Date.parse(Comparedate) },
        { message: "Story description should be at least have 70 character", test: edit_mystory.trim().length < 70 },
        { message: "Story description is too big", test: edit_mystory.trim().length > 40000 },
        { message: "Please enter youtube video urls only, enter different urls in new line and maximum 20 urls are allowed", test: !validateYouTubeUrls(Video_url)}
    ];

    // Loop through validation conditions and display alert if test fails
    conditions.forEach(condition => {
        if (condition.test) {
            showAlert(condition.message);
        }
    });
}
if (window.location.href === 'https://localhost:7064/Story/ShareStory') {
    CKEDITOR.replace('editor1', {
        height: 200,
        removeButtons: ['About', 'Cut', 'Copy', 'Paste', 'Link', 'Unlink', 'Anchor', 'Indent', 'Outdent', 'NumberedList', 'BulletedList']
    }
    );
}

var mission
var title
var date
var current_date
var comparedate
var mystory
var video_url
var media = []
var count = 0
$(function () {
    $("#datepicker").datepicker();
});
if (window.location.href === 'https://localhost:7064/Story/ShareStory') {
    Dropzone.autoDiscover = false;
    $(function () {
        var myDropzone = new Dropzone("#myDropzone", {
            url: "/Story/ShareStory",
            maxFiles: 20,
            maxFilesize: 4,
            uploadMultiple: true,
            acceptedFiles: ".jpeg,.jpg,.png",
            addRemoveLinks: false,
            dictRemoveFile: "Remove",
            autoProcessQueue: false,
            dictDefaultMessage: "Drop files here or click to upload",
            dictInvalidFileType: "Invalid file type. Only JPEG, JPG and PNG are allowed.",
            dictFileTooBig: "File size is too big. Maximum file size allowed is 4MB.",
            dictMaxFilesExceeded: "You can only upload a maximum of 20 files.",
            previewTemplate: $("#imagePreview").html(),
            init: function () {
                this.on("addedfile", function (file) {
                    // Show the remove button when a file is added
                    file.previewElement.querySelector(".btn-remove").style.display = "block";
                    file.previewElement.querySelector(".btn-remove").addEventListener("click", function () {
                        // Remove the file from Dropzone
                        myDropzone.removeFile(file);
                    });
                });
                this.on("maxfilesexceeded", function (file) {
                    this.removeFile(file);
                    showAlert("You can only upload a maximum of 20 files.");
                });
             
            }
        });

    });;
}

function sharestory(type) {
    validate();

    if (mission != 0 && title.trim().length > 50 && title.trim().length < 80 && $('#datepicker').datepicker().val().length != 0
        && Date.parse(current_date) >= Date.parse(comparedate) && mystory.trim().length > 70 && mystory.trim().length < 40000 && validateYouTubeUrls(video_url)){

        var formData = new FormData();

        var myDropzone = Dropzone.getElement("#myDropzone").dropzone;

        // Append the file(s) to the formData object
        myDropzone.on("sendingmultiple", function (files, xhr, formData) {
            for (var i = 0; i < files.length; i++) {
                formData.append('media', files[i]);
            }
            formData.append('MissionId', mission);
            formData.append('Title', title);
            formData.append('PublishedDate', date.toString());
            formData.append('StoryDescription', mystory);
            formData.append('Type', type);
            for (var j = 0; j < video_url.length; j++) {
                formData.append('VideoUrls', video_url[j])
            }
        });
        // Manually process queue to upload files
        myDropzone.processQueue();
        // Send the AJAX request with the FormData object after the files have been uploaded
        myDropzone.on("success", function (file, response) {
            showAlert("Story added !!")
            setTimeout(function () {
                window.location.href = "https://localhost:7064/Story/Story";
            }, 5000);
        });

        
    }
}
function validate() {
    mission = parseInt($('.form-select').find(':selected').val())
    title = $('.story_title').val()
    date = convertDate($('#datepicker').datepicker().val())
    current_date = new Date()
    comparedate = new Date($('#datepicker').datepicker().val())
    mystory = CKEDITOR.instances.editor1.getData();
    const urlString = $('.videourl').val();
    const urls = urlString.trim().split(/\r?\n/);
    video_url = urls;
    // Define validation conditions
    const conditions = [
        { message: "Please select a mission", test: mission === 0 },
        { message: "Title should be at least 50 characters long", test: title.trim().length < 50 },
        { message: "Title can have maximum 80 characters", test: title.trim().length > 80 },
        { message: "Please enter a valid date", test: $('#datepicker').datepicker().val().length == 0 || Date.parse(current_date) <= Date.parse(comparedate) },
        { message: "Story description should be at least have 70 character", test: mystory.trim().length < 70 },
        { message: "Story description is too big", test: mystory.trim().length > 40000 },
        { message: "Please enter youtube video urls only, enter different urls in new line and maximum 20 urls are allowed", test: !validateYouTubeUrls(video_url) }
    ];

    // Loop through validation conditions and display alert if test fails
    conditions.forEach(condition => {
        if (condition.test) {
            showAlert(condition.message);
        }
    });
}
function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/')
}

function Recommend(story_id, email, to_user_id) {
        $.ajax({
            url: `/Story/StoryDetail/${story_id}`,
            type: 'POST',
            data: { story_id: story_id, email: email,to_user_id : to_user_id},
            success: function (result) {
                console.log(result)

                var recommend = $('#recommend-' + to_user_id)

                recommend.prop('disabled', true)

               showAlert("Invitation sent successfully!")
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    
}

function searchcoworker() {
    var searchValue = $('#search-input').val().toLowerCase();
    $('#volunteers-list li').filter(function () {
        var fullName = $(this).find('span').text().toLowerCase();
        $(this).toggle(fullName.indexOf(searchValue) > -1);
    });
}
