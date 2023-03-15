let cities = []
let countries = []
let themes = []
let skills = []
let country_count = 0
let city_count = 0;
let theme_count = 0;
let skill_count = 0;

// for grid view list view
function showList(e) {
    var $gridCont = $('.grid-container');
    e.preventDefault();
    $gridCont.hasClass('list-view') ? $gridCont.removeClass('list-view') : $gridCont.addClass('list-view');
}
function gridList(e) {
    var $gridCont = $('.grid-container')
    e.preventDefault();
    $gridCont.removeClass('list-view');
}

$(document).on('click', '.btn-grid', gridList);
$(document).on('click', '.btn-list', showList);



// Dropdown item click event listener
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

function search() {
    var input = document.getElementById("search-input").value.toLowerCase();
    var missionCards = document.getElementsByClassName("mission-card");
    var cardTitles = document.getElementsByClassName("card-title");

    var found = false;
    for (var i = 0; i < cardTitles.length; i++) {
        var title = cardTitles[i].innerHTML.toLowerCase();
        var missionCard = missionCards[i];

        if (title.indexOf(input) !== -1) {
            missionCard.style.display = "block";
            found = true;
        } else {
            missionCard.style.display = "none";
        }
    }

    var pageNotFound = document.getElementsByClassName("page-not-found")[0];
    var pagination = document.getElementsByClassName("pagination-link")[0];
    if (!found) {
        pageNotFound.style.display = "block";
        pagination.style.display = "none";

    } else {
        var hiddenCount = 0;
        for (var i = 0; i < missionCards.length; i++) {
            if (missionCards[i].style.display === "none") {
                hiddenCount++;
            }
        }
        if (hiddenCount === missionCards.length) {
            pageNotFound.style.display = "block";
        } else {
            pageNotFound.style.display = "none";
        }
    }
}
function sortby(order) {
    $('.missions').empty()
    $.ajax({
        url: '/Mission',
        type: 'Post',
        data: { sortOrder: order},
        success: function (result) {
            $('.missions').html(result);
        },
        error: function () {
            console.log("Error getting missions")
        }
    });
}

function clear_all() {

    countries = []
    cities = []
    themes = []
    skills = []
    $('.missions').empty()
    $.ajax({
        url: '/Mission',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills},
        success: function (result) {
            $('.missions').html(result);
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
        url: '/Mission',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills},
        success: function (result) {
            $('.missions').html(result);
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
function addcities(name) {
    $('.missions').empty()
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
        url: '/Mission',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
        success: function (result) {
            $('.missions').html(result);
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}

function addcountries(name,dataid) {
    $('.missions').empty()
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
        url: 'Mission/GetCitiesForCountry',
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
                url: '/Mission',
                type: 'POST',
                data: { countries: countries, cities: cities, themes: themes, skills: skills},
                success: function (result) {
                    $('.missions').html(result);
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
            " <label for=''> " + item.name + "</label>" + "</span>" +"<br>";
        $('.city').append(data)
        $('.city span').each(function () {
            $('.city span').eq(i).find('input').attr('id', item.name)
            $('.city span').eq(i).find('label').attr('for', item.name)
            $('.city span').eq(i).find('input').attr('data-value', item.name)
            $('.city span').eq(i).find('input').attr('onchange', `addcities('${item.name}')`)
            
           
        })
    })
}

const addthemes = (name) => {
    $('.missions').empty()
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
        url: '/Mission',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills },
        success: function (result) {
            $('.missions').html(result);
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

const addskills = (name) => {
    $('.missions').empty()
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
        url: '/Mission',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills},
        success: function (result) {
            $('.missions').html(result);
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


/*volunteering mission js starts*/
/*recent volunteers*/
function add_comments (user_id, mission_id) {
    $('#comments').empty()
    var commentText = document.getElementById('comment-text').value
    console.log(commentText)
 /*   var length = $('.user-comments').find('.usercomment-image').length*/
    if (commentText.length > 3) {
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { user_id: user_id, mission_id: mission_id, comment: commentText},
            success: function (result) {
                $('#comments').html(result);
               /* load_comments(result.comments)*/
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}

function add_to_favourite (user_id, mission_id) {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { request_for: "add_to_favourite", mission_id: mission_id, user_id: user_id },
        success: function (result) {
            if (result.success) {
                $('.heart-image').removeAttr('src').attr('src', '/images/red-heart-png.png')
                $('.favorite-text').html('Added to favorite')
                Swal.fire({
                    title: 'Mission added to favorites!',
                    icon: 'success'
                });
            }
            else {
                $('.heart-image').removeAttr('src').attr('src', '/images/heart1.png')
                $('.favorite-text').html('Add to favorite')
                Swal.fire({
                    title: 'Mission removed from favorites!',
                    icon: 'success'
                });
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
const rating = (rating, user_id, mission_id) => {
    if (rating == 1) {
        $('.rating').find('img').each(function (i, item) {
            if (i == (rating - 1)) {
                if (item.id == `${i + 1}-star-empty`) {
                    item.src = '/images/selected-star.png'
                    item.id = `${i + 1}-star`
                    $.ajax({
                        url: `/volunteering_mission/${mission_id}`,
                        type: 'POST',
                        data: { request_for: "rating", mission_id: mission_id, user_id: user_id, rating: rating },
                        success: function (result) {
                        },
                        error: function () {
                            console.log("Error updating variable");
                        }
                    })
                }
                else {
                    item.src = '/images/star-empty.png'
                    item.id = `${i + 1}-star-empty`
                    $.ajax({
                        url: `/volunteering_mission/${mission_id}`,
                        type: 'POST',
                        data: { request_for: "rating", mission_id: mission_id, user_id: user_id, rating: 0 },
                        success: function (result) {
                        },
                        error: function () {
                            console.log("Error updating variable");
                        }
                    })
                }
            }
            else {
                item.src = '/images/star-empty.png'
                item.id = `${i + 1}-star-empty`
            }
        })
    }
    else {
        $('.rating').find('img').each(function (i, item) {
            if (i <= (rating - 1)) {
                if (item.id == `${i + 1}-star-empty` || i <= (rating - 1)) {
                    item.src = '/images/selected-star.png'
                    item.id = `${i + 1}-star`
                }
                else {
                    item.src = '/images/star-empty.png'
                    item.id = `${i + 1}-star-empty`
                }
            }
            else {
                item.src = '/images/star-empty.png'
                item.id = `${i + 1}-star-empty`
            }
        })
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { request_for: "rating", mission_id: mission_id, user_id: user_id, rating: rating },
            success: function (result) {
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }

}
//const load_comments = (comments) => {
//    $.each(comments, function (i, item) {
//        var comment =  "<div class='d-flex'>" + 
//        "<img id='img' class='rounded-circle img-fluid' src='/images/volunteer1.png' alt='' />" +
//        "<div>" + 
//            "<p style='font-size: 15px; margin-bottom:0;margin-left:15px;margin-top:15px;'> " + '${item.user.firstName} ${item.user.lastName}' + "</p> " +
//            "<p style='font-size: 15px;margin-left: 15px;'>" +
//            item.createdAt.slice(0, 10) + "</p>" +
//        "</div> " + 
     
//    "</div>" +
//            "<div class='comment-text' style='margin-top:10px;'>" +
//            item.commentText +
//            "</div>"
//         $('#comments').append(comment);
//    })
//}

function NextPage() {

    var page1 = document.getElementById("recent-volunteer-page-1");
    var page2 = document.getElementById("recent-volunteer-page-2");
    var page3 = document.getElementById("recent-volunteer-page-3");
    var footer_txt = document.getElementById("recent-volunteer-footer-txt");

    if (y.style.display == "block") {
        page1.style.display = "none";
        page2.style.display = "none";
        page3.style.display = "block";
        footer_txt.innerHTML = "19-25  of 25 Recent Volunteer";


    }

    else if (z.style.display == "block") {
        page1.style.display = "block";
        page2.style.display = "none";
        page3.style.display = "none";
        footer_txt.innerHTML = "1-9  of 25 Recent Volunteer";

    }
    else {
        page1.style.display = "none";
        page2.style.display = "block";
        page3.style.display = "none";
        footer_txt.innerHTML = "10-18  of 25 Recent Volunteer";

    }
};

function PreviousPage() {
    var page1 = document.getElementById("recent-volunteer-page-1");
    var page2 = document.getElementById("recent-volunteer-page-2");
    var page3 = document.getElementById("recent-volunteer-page-3");
    var footer_txt = document.getElementById("recent-volunteer-footer-txt");

    if (page2.style.display == "block") {
        page1.style.display = "block";
        page2.style.display = "none";
        page3.style.display = "none";
        footer_txt.innerHTML = "1 - 9  of recent 25 volunteers"
    }
    else if (page3.style.display == "flex") {
        page1.style.display = "none";
        page2.style.display = "block";
        page3.style.display = "none";
        footer_txt.innerHTML = "9 - 18  of recent 25 volunteers"
    }
    else {
        page1.style.display = "none";
        page2.style.display = "none";
        page3.style.display = "block";
        page4.innerHTML = "18 - 25  of recent 25 volunteers"
    }
};



