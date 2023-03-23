let cities = []
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

CKEDITOR.replace('editor1', {
    height: 200,
    removeButtons: ['About','Cut','Copy','Paste','Link','Unlink','Anchor','Indent','Outdent','NumberedList','BulletedList']
}
    );

const imageUpload = document.getElementById('imageUpload');
const imagePreview = document.getElementById('imagePreview');

imageUpload.addEventListener('change', () => {
    // Clear the imagePreview div
    imagePreview.innerHTML = '';

    // Loop through each uploaded file, up to a maximum of 20 files
    for (let i = 0; i < Math.min(imageUpload.files.length, 20); i++) {
        const file = imageUpload.files[i];

        // Check if the file is a valid image
        if (file.type.startsWith('image/')) {
            // Create a new image element and set its attributes
            const img = document.createElement('img');
            img.classList.add('img-thumbnail');
            img.width = 100;
            img.height = 100;

            // Create a URL for the uploaded image
            const url = URL.createObjectURL(file);

            // Set the src attribute of the image element
            img.src = url;

            // Create a new button element for the close button
            const closeButton = document.createElement('button');
            closeButton.textContent = 'x';
            closeButton.addEventListener('click', () => {
                container.remove();
            });

            // Create a new container element and append the image and close button to it
            const container = document.createElement('div');
            container.classList.add('photo-container');
            container.appendChild(img);
            container.appendChild(closeButton);

            // Append the container element to the imagePreview div
            imagePreview.appendChild(container);
        }

        // Check if the loop has reached the limit of 20 files
        if (i >= 19) {
            break;
        }
    }
});