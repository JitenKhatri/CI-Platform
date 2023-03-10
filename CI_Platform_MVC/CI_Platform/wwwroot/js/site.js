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
        });
        // Clear all button click event listener
        $('#clear-all-btn').on('click', function () {
            selectedItemsRow.empty();
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
function sortby(order) {
    $.ajax({
        url: '/Mission',
        type: 'Post',
        data: { sortOrder: order},
        success: function (result) {
            loadmissions(result.missions)
        },
        error: function () {
            console.log("Error getting missions")
        }
    });
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

const addcountries = (name) => {
    
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
        url: '/Mission',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills},
        success: function (result) {
            if (result.missions.length > 0) {
                loadcities(result.missions[0].cities);
                loadmissions(result.missions)
            }
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
            "<input type='checkbox'  class='form-check-input'> " + 
            " <label for= item.name> " + item.name + "</label>" + "</span>" +"<br>";
        $('.city').append(data)
        $('.city span').each(function () {
            $('.city span').eq(i).find('input').attr('id', item.name)
            $('.city span').eq(i).find('input').attr('onchange', `addcities('${item.name}')`)
        })
    })
}

const addthemes = (name) => {
    /*var selected = $('#sort').find(':selected').text();*/
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
            loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

const addskills = (name) => {
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
            loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
const loadmissions = (missions) => {
    console.log(missions)
    $('.missions').empty()
    console.log(missions)
    $.each(missions, function (i, item) {
        var data = "<div class='col-12 col-md-6 col-lg-4 mission-card'>" +       /* mission-card-div*/
            "<div class='card'>" +  /*card-div*/
                "<div class='img-event'>" + /* img-event-div */
                    "<img class='card-img-top' src='' alt='Card image cap'>" +
                    "<div class='location-img'>" + /*location-img-div*/
                        "<img class='text-light' src='images/pin.png' alt=''>" +
                            "<span class='text-light'>" + item.missions.city.name + "</span>" +
                    "</div>" + /*location-img-div-ends*/
                        "<button class='like-img border-0'>" +
                            "<img class='text-light' src='images/heart.png' alt=''> " +
                    "</button>" +
                         "<button class='stop-img border-0'>" +
                       "<img class='text-light' src='images/add1.png' alt=''>" +
                   "</button>" +
                            " <button class='mission-theme border-0'>" +
                             "<span class='p-2'>" + item.missions.theme.title +  "</span>" +
                               "</button>" +
                                 "</div>" + /*img-event-div-ends*/
                              "<div class='card-body'>" +  /*card-body-div*/
            "<h5 class='card-title'>" + item.missions.title + "</h5>" +
                                "<p class='card-text'>" + "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore..." + "</p>" +
                               " <div class='d-flex justify-content-between'> " +
            " <div> " + "<p class='slug'>" + item.missions.organizationName + "</p>" + "</div>" + 
                                   " <div class='d-flex flex-row'> "+
                                        "<img class='star' src='images/selected-star.png' alt=''> " +
                                           " <img class='star' src='images/selected-star.png' alt=''> " +
                                                "<img class='star' src='images/selected-star.png' alt=''> " +
                                                    "<img class='star' src='images/star-empty.png' alt=''>" +
                                                        "<img class='star' src='images/star-empty.png' alt=''> "+
                   " </div>" +
                                            " </div>" +
                                                   " <div class='duration-seats-info mt-4'> " +
                                                        "<div class='duration'> " +
                                                           

                                                       " </div> " + 
                                            "<div class='d-flex flex-row mission-deadline'>" +
                                            "<div class='d-flex flex-row deadline'>" + 
                                                          
            "<div class='ms-2 p-bar'>  " +
            "</div>" +
                       " </div>" + 
                      "</div> " + 
                      
                    "</div> " +
            "<div class='d-flex justify-content-center mt-4'> " +
            " <button class='apply-btn'> " +
            "Apply" + "<span>" + "<img src='images/right-arrow.png' alt=''> " + "</span>" +
            "</button>" +
            "</div> " +
                  "</div> " + 
                  
                                "</div> " +
                                "</div> " +
            "</div> "

        // some operations.
        $('.missions').append(data)
        $('.img-event').eq(i).find('img').eq(0).attr('src',item.image.mediaPath)
        if (item.missions.startDate != null && item.missions.endDate != null)
        {
          var a =  "<p id='duration-txt' style='margin-bottom: 0;'>" +

              "From" + " " + item.missions.startDate.slice(0, 10) + " until" + " " + item.missions.endDate.slice(0, 10) +
                "</p>"
            $('.duration').eq(i).append(a)
          }
        if (item.missions.startDate = null)
         {
            var b =  "<p id='duration-txt' style='margin-bottom: 0;'> " +
                item.missions.GoalMotto +
                "</p>"
            $('.duration').eq(i).append(b)
        }

        
            if(item.missions?.seatsLeft != null)
            {
                var c = "<div class='d-flex flex-row deadline'>" + 
                    "<div> " + "<img src='images/Seats-left.png' alt=''>" + "</div>" +
                        "<div> " + "<span> " + item.missions.seatsLeft + "<br>" + "Seats left" + "</span>" + "</div>"+
                        "</div> "
                    $('.mission-deadline').eq(i).append(c)
            }


                        
                            if(item.missions?.missionType == "TIME")
                            {
                                var d = "<div>" + "<img src='images/deadline.png' alt=''>" + "</div>"
                                 $('.deadline').eq(i).append(d)
                             }

                           
                           if(item.missions?.deadline != null)
                            {
                               var e = "<span>" + item.missions?.deadline.slice(0, 10) + " <br>" + " Deadline " + "</span>"
                               $('.p-bar').eq(i).append(e)
                               
                             }
                          else if(item.missions?.missionType == "GO")
                           {
                             var f = "<div class='d-flex flex-column'>" + 
                                       "<div class='' d-flex flex-row'>"+
                                       "<img class='align-self-center ms-3' src='images/mission.png' alt='' style='height:25px; width:25px;'>" + 
                                                            "<div class='progress align-self-center ms-3'>" +
                                               " <div class='progress-bar w-75' role='progressbar' aria-label='Basic example' aria-valuenow='75' aria-valuemin='0' aria-valuemax='100'> " +
                                               "</div>" + 
                                                            "</div>" +
                                                          "</div> " +
                                                           " <div> " + "<p>"+ "8000 achieved" + "</p>" + "</div>" +
                                   "</div>"
                               $('.p-bar').eq(i).append(f)
                            }
                           else if(item.missions?.missionType == "TIME")
                             {
                               var g = "<span> " + item.missions?.goalMotto + "</span>"
                               $('.p-bar').eq(i).append(g)
                             }
                                                   
        

    })
    if (missions.length === 0) {
        var pageNotFound = "<div class='page-not-found text-center'> " +
            
                "<h4 class='pt-2'>" + "No mission found" + "</h4>" + 
                
                
                "<div class='d-flex justify-content-center mt-4'>" +
                    "<button class='applyButton btn'>"+  "Go to Homepage"+ "<img src='images/right-arrow.png' alt=''>" +
            "</button>" + 
        "</div>" + 
            "</div>"
        $('.missions').append(pageNotFound)
    }
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



