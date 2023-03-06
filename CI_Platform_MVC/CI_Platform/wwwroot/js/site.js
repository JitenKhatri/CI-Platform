let cities = []
let countries = []
let themes = []
let skills = []

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
$('.dropdown-item').on('click', function () {
    const dropdown = $(this).closest('.dropdown');
    const dropdownTitle = dropdown.find('.dropdown-toggle').text();
    const value = $(this).data('value');
    const selectedItemsRow = $('#selected-items-row');

    // Create a badge element to display the selected item
    const badge = $('<span class="badge text-bg-light ml-2 "></span>').text(value);
    const cross = $('<span class="badge text-bg-light ml-2 d-inline" id="cross-btn"></span>').html('&times;');


    cross.addClass('d-inline');
    badge.append(cross);
    selectedItemsRow.append(badge);

    // Cross button click event listener
    cross.on('click', function () {
        //    $(this).closest('.badge').remove();
        badge.remove();
    });

    // Clear all button click event listener
    $('#clear-all-btn').on('click', function () {
        selectedItemsRow.empty();
    });
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
    if (!found) {
        pageNotFound.style.display = "block";
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

function addcities(name) {
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
            loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}

const loadmissions = (missions) => {
    $('.missions').empty()
    console.log(missions)
    $.each(missions, function (i, item) {
        var data = "<div class='col-12 col-md-6 col-lg-4 mission-card'>" +
            "<div class='card'>" +
                "<div class='img-event'>" +
                    "<img class='card-img-top' src= mission.image?.MediaPath alt='Card image cap'>" +
                    "<div class='location-img'>" +
                        + "<img class='text-light' src='~/images/pin.png' alt=''>" +
                            "<span class='text-light'>"+ item.missions.city.name + "</span>" +
                    "</div>" +
                        "<button class='like-img border-0'>" +
                            "<img class='text-light' src='~/images/heart.png' alt=''> " +
                    "</button>" +
                         "<button class='stop - img border - 0'>" +
                       "<img class='text - light' src='~/images/add1.png' alt=''>" +
                   "</button>" +
                            " <button class='mission - theme border - 0'>" +
                             "<span class='p - 2'>" + item.missions.theme.title +  "</span>" +
                               "</button>" +
                                 "</div>" +
                              "<div class='card - body'>" +
            "<h5 class='card-title'>" + item.missions.title + "</h5>" +
                                "<p class='card-text'>" + "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore..." + "</p>" +
                               " <div class='d-flex justify-content-between'> " +
            " <div> " + "<p class='slug'>" + item.missions.organizationName + "</p>" + "</div>" +
                                   " <div class='d-flex flex-row'> "
                                        "<img class='star' src='~/images/selected-star.png' alt=''> " +
                                           " <img class='star' src='~/images/selected-star.png' alt=''> " +
                                                "<img class='star' src='~/images/selected-star.png' alt=''> " +
                                                    "<img class='star' src='~/images/star-empty.png' alt=''>" +
                                                        "<img class='star' src='~/images/star-empty.png' alt=''> "+
                   " </div>" +
                                            " </div>" +
                                                   " <div class='duration-seats-info mt-4'> " +
                                                        "<div class='duration'> " +
                                                           

                                                       " </div> " + 
                                                        "<div class='d-flex flex-row mission-deadline'>" +
                                                          
                                                                    "<div class='ms-2'>  " + 
                       " </div>" + 
                      "</div> " + 
                      
                    "</div> " +
                    
                  "</div> " + 
                  "<div class='d-flex justify-content-center mt-4'> " + 
                   " <button class='apply-btn'> " +
                      "Apply" +  "<span>" + "<img src='~/images/right-arrow.png' alt=''> " + "</span>" +
                    "</button>" + 
                                "</div> " +
                                "</div> " +
                                "</div> " +
            "</div> "

        // some operations.
        $('.missions').append(data)
        $('.img-event').eq(i).find('img').eq(0).attr('src', item.image.mediaPath)
        if (item.missions.startDate != null && item.missions.endDate != null)
        {
          var a =  "<p id='duration-txt' style='margin-bottom: 0;'>" +

              "From" + item.missions.startDate.slice(0, 10) + " until" + item.missions.endDate.slice(0, 10) +
                "</p>"
            $('.duration').eq(i).append(a)
          }
        else {
            var b =  "<p id='duration-txt' style='margin-bottom: 0;'> " +
                item.missions.GoalMotto +
                "</p>"
            $('.duration').eq(i).append(b)
        }

        if (item.missions.SeatsLeft != null) {
            var c = "<div class='d-flex flex-row'> " +
                "<div> " + "<img src='~/images/Seats-left.png' alt=''>" + " </div>" +
                "<div>" + "<span>" + item.missions.seatsLeft + "<br>" + "Seats left" + "</span>" + "</div>" +
                "</div>"
            $('.mission-deadline').eq(i).append(c)
        }
           
        if (item.missions.missionType == "TIME")
        {
             var d = "<div> " + "<img src='~/images/deadline.png' alt=''>" + "</div >"
             $('.flex-row').eq(i).append(d)
       } 
        if (item.missions.deadline != null) {

            var e = "<span> " + item.missions.deadline.slice(0, 10) + "<br>" + " Deadline" + "</span>"
            $('.mission-deadline').eq(i).append(e)

        }
        else {
            if (item.missions.missionType == "GO") {
                var f = "<div class='d-flex flex-column'> " +
                    " <div class='d - flex flex - row'>" +
                    "<img class='align-self-center ms-3' src='~/images/mission.png' alt='' style='height:25px; width:25px;'> " +
                    " <div class='progress align-self-center ms-3'> " +
                    "<div class='progress-bar w-75' role='progressbar' aria-label='Basic example' aria-valuenow='75' aria-valuemin='0' aria-valuemax='100'>" + "</div>" +
                    " </div>" + "</div>" +
                    "<div>" + "<p>" + "8000 achieved" + "</p>" + "</div>" +
                    "</div>"
                $('.mission-deadline').eq(i).append(f)
            }
            else {
                var g =
                    "<span> " + item.missions.goalMotto + "</span>"
                $('.mission-deadline').eq(i).append(g)

            }
        }
            

           

    })
}





/*recent volunteers*/


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



