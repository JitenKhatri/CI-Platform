$(".toggle-sidebar,.overlay").click(function () {
	$("body").toggleClass("sidebar-open")
});

$(document).ready(function () {

    var Usertable = $('#Users-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#search-input').on('keyup', function () {
        Usertable.search($(this).val()).draw();
    });

    var MissionTable = $('#Missions-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#mission-search-input').on('keyup', function () {
        MissionTable.search($(this).val()).draw();
    });

    var MissionThemeTable = $('#Missiontheme-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#theme-search-input').on('keyup', function () {
        MissionThemeTable.search($(this).val()).draw();
    });

    var MissionSkillTable = $('#Missionskill-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#skill-search-input').on('keyup', function () {
        MissionSkillTable.search($(this).val()).draw();
    });

    var StoryTable = $('#Story-table').DataTable({
        "pagingType": "full_numbers",
        lengthChange: false,
        searchDelay: 500,
        "ordering": false,
        dom: 'lrtip',
        initComplete: function () {
            // Hide default search bar
            $(this.api().table().container()).find('.dataTables_filter').hide();
        }
    });
    $('#story-search-input').on('keyup', function () {
        StoryTable.search($(this).val()).draw();
    });
});