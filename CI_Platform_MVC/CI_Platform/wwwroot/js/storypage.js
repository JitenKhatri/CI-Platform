let cities = []
let countries = []
let themes = []
let skills = []

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
        url: '/Story',
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
        url: 'Story/GetCitiesForCountry',
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
                url: '/Story',
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
            $('.city span').eq(i).find('input').attr('onchange', `addcities('${item.name}')`)


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
        url: '/Story',
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
        url: '/Story',
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
