﻿let cities = []
let countries = []
let themes = []
let skills = []

$('.form-check-input').on('change', function () {
    const dropdown = $(this).closest('.dropdown');
    const dropdownTitle = dropdown.find('.dropdown-toggle').text();
    const value = $(this).data('value');
    const selectedItemsRow = $('#selected-items-row');
    const itemId = $(this).attr('id');

    // Create a badge element to display the selected item
    const badge = $('<span class="badge text-bg-light ml-2 "></span>').text(value);
    const cross = $('<span class="badge text-bg-light ml-2 d-inline" id="cross-btn"></span>').html('&times;');

    if ($(this).is(':checked')) {
          cross.addClass('d-inline');
        badge.append(cross);
        selectedItemsRow.append(badge);
        // Cross button click event listener
        cross.on('click', function () {
            //    $(this).closest('.badge').remove();
            dropdown.find(`input[data-value="${value}"]`).prop('checked', false);
            badge.remove();
            remove_badges(dropdown.find(`input[id="${itemId}"]`));

        });
        // Clear all button click event listener
        $('#clear-all-btn').on('click', function () {
            $('.form-check-input').prop('checked', false);
            selectedItemsRow.empty();
            clear_all();
        });
    } else {
        // Checkbox is unchecked, remove its corresponding badge
        // If checkbox is unchecked, remove its corresponding badge
        const badge = selectedItemsRow.find(`span:contains(${value})`);
        badge.remove();
    }
});
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
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
                loadcities(result.cities);
            }
            else {
                loadcities(result.cities)

            }
            $.ajax({
                url: '/Story/Story',
                type: 'POST',
                data: { countries: countries, cities: cities, themes: themes, skills: skills },
                success: function (result) {
                    if (result == "") {
                        $('.page-not-found').css('display', 'block');
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
const loadcities = (cities) => {
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
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
            data: { searchtext :input, cities: cities, themes: themes, skills: skills },
            success: function (result) {
                if (result == "") {
                    $('.page-not-found').css('display', 'block');
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
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
CKEDITOR.replace('editor1', {
    height: 200,
    removeButtons: ['About','Cut','Copy','Paste','Link','Unlink','Anchor','Indent','Outdent','NumberedList','BulletedList']
}
);
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
let alertId = 0;

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
Dropzone.autoDiscover = false;
$(function () {
    var myDropzone = new Dropzone("#myDropzone", {
        url: "/Story/ShareStory",
        maxFiles: 20,
        maxFilesize: 4,
        acceptedFiles: ".jpeg,.jpg,.png",
        addRemoveLinks: false,
        dictRemoveFile: "Remove",
        parallelupload: 20,
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
            });
        }
    });
    
});;


function sharestory(type) {
    validate();

    if (mission != 0 && title.trim().length > 20 && title.trim().length < 255 && $('#datepicker').datepicker().val().length != 0
        && Date.parse(current_date) >= Date.parse(comparedate) && mystory.trim().length > 20 && mystory.trim().length < 40000) {

        var formData = new FormData();

        var myDropzone = Dropzone.getElement("#myDropzone").dropzone;

        // Append the file(s) to the formData object
        myDropzone.on("sending", function (file, xhr, formData) {
            formData.append('media', file);
            formData.append('Mission_id', mission);
            formData.append('title', title);
            formData.append('published_date', date.toString());
            formData.append('story_description', mystory);
            formData.append('type', type);
        });

        // Send the AJAX request with the FormData object after the files have been uploaded
        myDropzone.on("success", function (file, response) {
            showAlert("Story added as a draft")
        });

        // Manually process queue to upload files
        myDropzone.processQueue();
    }
}
function validate() {
    mission = parseInt($('.form-select').find(':selected').val())
    title = $('.story_title').val()
    date = convertDate($('#datepicker').datepicker().val())
    current_date = new Date()
    comparedate = new Date($('#datepicker').datepicker().val())
    mystory = CKEDITOR.instances.editor1.getData();
    video_url = $('.videourl').val()
    if (video_url.trim().length > 3) {
        media.push(video_url)
    }
    // Define validation conditions
    const conditions = [
        { message: "Please select a mission", test: mission === 0 },
        { message: "Title should be at least 20 characters long", test: title.trim().length < 20 },
        { message: "Title can have maximum 255 characters", test: title.trim().length > 255 },
        { message: "Please enter a valid date", test: $('#datepicker').datepicker().val().length == 0 || Date.parse(current_date) <= Date.parse(comparedate) },
        { message: "Story description should be at least have 70 character", test: mystory.trim().length < 70 },
        { message: "Story description is too big", test: mystory.trim().length > 40000 }
        /*{ message: "Please upload at least one image", test: $('.gallary').find('.preview-image').length == 0 }*/
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