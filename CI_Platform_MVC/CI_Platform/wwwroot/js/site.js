// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// // Listen for the click event on the dropdown items
// $(".dropdown-menu a").click(function() {
//     // Get the text of the selected item
//     var selectedItem = $(this).text();

//     // Add the selected item to the selected filters list
//     var selectedFilters = $("#selectedFilters").html();
//     $("#selectedFilters").html(selectedFilters + "<span class='badge badge-secondary ml-1'>" + selectedItem + "<button type='button' class='close ml-1' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span>");
//   });

//   // Listen for the click event on the close buttons within the selected filters
//   $("#selectedFilters").on("click", "button.close", function() {
//     // Remove the selected filter from the selected filters list
//     $(this).closest("span").remove();
//   });

//   // Listen for the click event on the clear filters button
//   $("#clearFiltersButton").click(function() {
//     // Remove all the selected filters from the selected filters list
//     $("#selectedFilters").empty();
//   });
// const toggleViewButton = document.querySelector('.toggle-view-btn');
// const productItems = document.querySelectorAll('.product-item');

// toggleViewButton.addEventListener('click', () => {
//   if (toggleViewButton.dataset.view === 'grid') {
//     toggleViewButton.innerHTML = '<i class="bi bi-list"></i> List View';
//     toggleViewButton.dataset.view = 'list';
//     productItems.forEach(item => {
//       item.classList.remove('col-md-4');
//       item.classList.remove('col-sm-6');
//       item.classList.add('col-12');
//     });
//   } else {
//     toggleViewButton.innerHTML = '<i class="bi bi-grid-3x3"></i> Grid View';
//     toggleViewButton.dataset.view = 'grid';
//     productItems.forEach(item => {
//       item.classList.add('col-md-4');
//       item.classList.add('col-sm-6');
//       item.classList.remove('col-12');
//     });
//   }
// });
// const addFavoriteButtons = document.querySelectorAll('.add-favorite');
// addFavoriteButtons.forEach(button => {
//   button.addEventListener('click', () => {
//     button.classList.toggle('active');
//   });
// });


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

// filters js 
$(document).ready(function () {
    // handle country dropdown
    $('#country').change(function () {
        var value = $(this).val();
        if (value != '') {
            $('#selected-filters').append('<div class="col-md-2 filter-item">' + value + '<button class="btn btn-sm btn-danger remove-filter" data-filter="country">&times;</button></div>');
        }
    });

    // handle city dropdown
    $('#city').change(function () {
        var value = $(this).val();
        if (value != '') {
            $('#selected-filters').append('<div class="col-md-2 filter-item">' + value + '<button class="btn btn-sm btn-danger remove-filter" data-filter="city">&times;</button></div>');
        }
    });
    // handle theme dropdown
    $('#theme').change(function () {
        var value = $(this).val();
        if (value != '') {
            $('#selected-filters').append('<div class="col-md-2 filter-item">' + value + '<button class="btn btn-sm btn-danger remove-filter" data-filter="theme">×</button></div>');
        }
    });
    // handle skill dropdown
    $('#skill').change(function () {
        var value = $(this).val();
        if (value != '') {
            $('#selected-filters').append('<div class="col-md-2 filter-item">' + value + '<button class="btn btn-sm btn-danger remove-filter" data-filter="skill">×</button></div>');
        }
    });
    // handle remove filter button click
    $(document).on('click', '.remove-filter', function () {
        var filter = $(this).data('filter');
        $('#' + filter).val('');
        $(this).parent().remove();
    });

    // handle clear filters button click
    $('#clear-filters').click(function () {
        $('#country, #city, #theme, #skill').val('');
        $('#selected-filters').empty();
    });
});