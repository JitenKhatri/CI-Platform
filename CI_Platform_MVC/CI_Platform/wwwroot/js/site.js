


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

/*recent volunteers*/


function NextPage() {

    var x = document.getElementById("recent-volunteer-page-1");
    var y = document.getElementById("recent-volunteer-page-2");
    var z = document.getElementById("recent-volunteer-page-3");
    var p = document.getElementById("recent-volunteer-footer-txt");

    if (y.style.display == "block") {
        x.style.display = "none";
        y.style.display = "none";
        z.style.display = "block";
        p.innerHTML = "19-25  of 25 Recent Volunteer";


    }

    else if (z.style.display == "block") {
        x.style.display = "block";
        y.style.display = "none";
        z.style.display = "none";
        p.innerHTML = "1-9  of 25 Recent Volunteer";

    }
    else {
        x.style.display = "none";
        y.style.display = "block";
        z.style.display = "none";
        p.innerHTML = "10-18  of 25 Recent Volunteer";

    }
};