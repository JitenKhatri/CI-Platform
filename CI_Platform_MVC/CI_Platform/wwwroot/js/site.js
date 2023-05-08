let cities = []
let countries = []
let themes = []
let skills = []
let country_count = 0
let city_count = 0;
let theme_count = 0;
let skill_count = 0;
var count = 1;
var co_workers = []
var sortorder;
var input = '';
var checkedIds = $('#notification-setting-menu input[type=checkbox]:checked').map(function () {
    return parseInt($(this).attr('id'));
}).get();

$(document).bind("ajaxSend", function () {
    $('.loader').addClass('d-block').removeClass('d-none');
}).bind("ajaxComplete", function () {
    $('.loader').addClass('d-none').removeClass('d-block');
});
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
$('.navbar-nav .form-check-input').on('change', function () {
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
function updateCheckedIds(checkbox) {
    // check if the checkbox is checked or unchecked
    if (checkbox.checked) {
        // add the checkbox ID to the checked IDs array if it is not already present
        if (!checkedIds.includes(parseInt(checkbox.id))) {
            checkedIds.push(parseInt(checkbox.id));
        }
    } else {
        // remove the checkbox ID from the checked IDs array
        checkedIds.splice(checkedIds.indexOf(parseInt(checkbox.id)), 1);
    }
}

function ShowSettings() {
    $('.notifications').addClass("d-none")
    $('.notification-dropdown-menu').removeClass("d-none").addClass("d-block");
}
function HideSettings() {
    $('.notifications').removeClass("d-none");
    $('.notification-dropdown-menu').removeClass("d-block").addClass("d-none");
}
function SaveNotificationSetting(userId) {
    $.ajax(
        {
            url: '/Mission/SaveUserNotificationSetting',
            type: 'POST',
            data: { CheckedIds: checkedIds, UserId: userId },
            success: function () {
                location.reload();
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
}
function search() {
    input = document.getElementById("search-input").value.toLowerCase();
    $('.missions').empty()
    $.ajax(
        {
            url: '/Mission',
            type: 'POST',
            data: { SearchText: input, Cities: cities, Themes: themes, Skills: skills, SortOrder : sortorder},
            success: function (result) {
                if (result == "") {
                    $('.page-not-found').css('display', 'block');
                }
                else {
                    $('.page-not-found').css('display', 'none');
                }
                $('.missions').html(result);
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
}

function sortby(order) {
    input = document.getElementById("search-input").value.toLowerCase();
    sortorder = order;
    $('.missions').empty()
    $.ajax({
        url: '/Mission',
        type: 'Post',
        data: { SearchText: input,Countries: countries, Cities: cities, Themes: themes, Skills: skills, SortOrder: order},
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
    $('#search-input').val= '';
    $('.missions').empty()
    $.ajax({
        url: '/Mission',
        type: 'POST',
        data: { SearchText:"",Countries: countries, Cities: cities, Themes: themes, Skills: skills},
        success: function (result) {
            $('.missions').html(result);
            $('.page-not-found').css('display', 'none');
            $('.pagination-link').css('display', 'block');
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

function remove_badges(input) {
    searchinput = document.getElementById("search-input").value.toLowerCase();
    const id = input.attr('id');
    const dataid = input.attr('data-id');
    console.log(id)
        if (cities.includes(id)) {
            cities.splice(cities.indexOf(id), 1)
        }
        if (countries.includes(id)) {
            countries.splice(countries.indexOf(id), 1)
            $.ajax({
                url: 'Mission/GetCitiesForCountry',
                type: 'Post',
                data: { countryid: 0 },
                success: function (result) {
                    console.log(result);
                    if (result.cities.length > 0) {
                        loadcities(result.cities);
                    }
                    else {
                        loadcities(result.cities)
                    }
                },
                error: function () {
                    console.log("Error updating variable");
                }

            });
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
        data: { SearchText: searchinput,Countries: countries, Cities: cities, Themes: themes, Skills: skills, SortOrder: sortorder},
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
                $('.pagination-link').css('display', 'none');
            }
            else {
                $('.page-not-found').css('display', 'none');
                $('.pagination-link').css('display', 'block');
            }
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
        data: { SearchText: input,Countries: countries, Cities: cities, Themes: themes, Skills: skills, SortOrder: sortorder },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
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
                data: { SearchText: input,Countries: countries, Cities: cities, Themes: themes, Skills: skills, SortOrder: sortorder},
                success: function (result) {
                    if (result == "") {
                        $('.page-not-found').css('display', 'block'); 
                    }
                    else {
                        $('.page-not-found').css('display', 'none');
                    }
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
        data: { SearchText: input,Countries: countries, Cities: cities, Themes: themes, Skills: skills, SortOrder: sortorder },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
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
        data: { SearchText: input,Countries: countries, Cities: cities, Themes: themes, Skills: skills, SortOrder: sortorder},
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
            $('.missions').html(result);
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
function Pagination(page, pageSize) {
    $('.missions').empty()
    $.ajax({
        url: '/Mission',
        type: 'POST',
        data: { SearchText: input,Countries: countries, Cities: cities, Themes: themes, Skills: skills, PageSize: pageSize, Page: page, SortOrder: sortorder },
        success: function (result) {
            if (result == "") {
                $('.page-not-found').css('display', 'block');
            }
            else {
                $('.page-not-found').css('display', 'none');
            }
            $('.missions').html(result);
        },
        error: function (error) {
            console.log(error)
            console.log("Error updating variable");
        }
    })
}

/*volunteering mission js starts*/
/*recent volunteers*/
function add_comments (user_id, mission_id) {
    var commentText = document.getElementById('comment-text').value
    console.log(commentText)
    if (commentText.length > 3) {
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { user_id: user_id, mission_id: mission_id, comment: commentText},
            success: function (result) {
                load_comments(result.comments.result)
                $("#comment-text").val("");
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
const load_comments = (comments) => {
    $('#comments').append(comments);
}

function add_to_favourite(user_id, mission_id) {
    var icon = $("img." + mission_id);
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { request_for: "add_to_favourite", mission_id: mission_id, user_id: user_id },
        success: function (result) {
            if (result.success) {
            /*    $('.heart-image').removeAttr('src').attr('src', '/images/red-heart-png.png')*/
               icon.removeAttr('src').attr('src', '/images/red-heart-png.png')
                $('.favorite-text').html('Added to favorite')
                Swal.fire({
                    title: 'Mission added to favorites!',
                    icon: 'success'
                });
            }
            else {
               /* $('.heart-image').removeAttr('src').attr('src', '/images/heart1.png')*/
                icon.removeAttr('src').attr('src', '/images/heart1.png')
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
function apply_for_mission (user_id, mission_id)  {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { user_id: user_id, mission_id: mission_id, request_for: "mission" },
        success: function (result) {
            if (result.success) {
                $('.applyButton').empty().append('<button class="apply-btn btn" disabled>Applied<img src="images/right-arrow.png" alt="">' + '</button >')
                Swal.fire({
                    title: 'Congratulations You have applied successfully',
                    icon: 'success'
                });
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
function prev_volunteers (user_id, mission_id)  {
    var one_page_volunteers = 1
    if (count >1) {
        count--;
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { count: count - 1, request_for: "next_volunteers", user_id: user_id, mission_id: mission_id },
            success: function (result) {
                $('.volunteers').empty().append(result.recent_volunteers.result)
                $('.current_volunteers').html(`${one_page_volunteers * (count - 1) == 0 ? 1 : one_page_volunteers * (count - 1)}-${one_page_volunteers * count} of recent ${result.total_volunteers} volunteers`)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
function next_volunteers (max_page, user_id, mission_id) {
    var one_page_volunteers = 1
    if (count < max_page) {
        count++;
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { count: count - 1, request_for: "next_volunteers", mission_id: mission_id, user_id: user_id },
            success: function (result) {
                $('.volunteers').empty().append(result.recent_volunteers.result)
                $('.current_volunteers').html(`${one_page_volunteers * (count - 1) + 1}-${one_page_volunteers * count >= result.total_volunteers ? result.total_volunteers : one_page_volunteers * count} of recent ${result.total_volunteers} volunteers`)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
const add_coworkers = (id) => {
    id = parseInt(id.slice(9))
    if (!co_workers.includes(id)) {
        co_workers.push(id)
    }
    else {
        co_workers.splice(co_workers.indexOf(id), 1)
    }
}
const recommend = (user_id, mission_id,email,to_user_id) => {
    if (co_workers.length > 0) {
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { co_workers: co_workers, user_id: user_id, mission_id: mission_id, request_for: "recommend", email: email },
            success: function (result) {
                console.log(result)
               
                    var recommend = $('#recommend-'+to_user_id)

                recommend.prop('disabled', true)

                Swal.fire({
                    title: 'Invitation sent!!',
                    icon: 'success'
                });
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
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
    }, 3000);
}
function contactUs() {
    var subject = $('#contact-subject').val();
    var message = $('#contact-message').val();
    $.ajax({
        url: '/UserAuthentication/ContactUs',
        type: 'POST',
        data: { Subject: subject, Message: message },
        success: function (result) {
            $("#contact-us-model").modal('hide')
            ClearModal();
            showAlert("Your concern has been saved!!")
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

function ReadNotification(NotificationId) {
    $.ajax({
        url: '/Mission/ReadNotification',
        type: 'POST',
        data: { NotificationId: NotificationId },
        success: function () {
            $('#notification-' + NotificationId).addClass('bi-check-circle');
            $('#notification-' + NotificationId).removeClass('bi-circle-fill');
            $('#notification-' + NotificationId).removeClass('text-warning');
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

function ClearNotifications(userId) {
    $.ajax({
        url: '/Mission/ReadNotification',
        type: 'POST',
        data: { UserId: userId },
        success: function () {
            location.reload();
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

