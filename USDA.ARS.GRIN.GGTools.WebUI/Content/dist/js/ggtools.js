/*! ggtools.js
* ================
* Main JS application file for GGTools. This file
* should be included in all layout pages. 
* 
* @author Benjamin Haag
*/

/* ========================================================================================
 * General Utilities
  ======================================================================================== */

//$(document).keypress(function (event) {
//    var keycode = (event.keyCode ? event.keyCode : event.which);
//    if (keycode == '13') {
//        $("#btnSearch").click();
//    }
//});

function AddRecord() {
    var addNewRecordUrl = $("#hfAddNewRecordLink").val();
    window.location.href = addNewRecordUrl;
}

/*
 * Datatable utilities
 */
function InitDataTable(tableName) {
    tableName = "#" + tableName;
    $(document).ready(function () {
        table = $(tableName).DataTable({
            dom: 'Blfrtip',
            paging: true,
            "pageLength": 10,
            //initComplete: function () {
            //    SetControlVisibility(tableName);
            //},
            responsive: true,
            buttons: [
                'selectAll',
                'selectNone',
                'csv',
                'excel',
                'pdf'
                //{
                //    text: '+ Add',
                //    action: function (e, dt, node, config) {
                //        AddRecord();
                //    }
                //}
            ],
            select: true,
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });

        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}

function InitDataTableSingleSelect(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            paging: true,
            "pageLength": 10,
            responsive: true,
            scrollY: '300px',
            select: true,
            searching: true,
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
    });
}

function InitDataTableLookupFormat(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            paging: false,
            stateSave: true,
            "bLengthChange": false,
            scrollY: '300px',
            scrollCollapse: true,
            paging: false,
            responsive: true,
            select: {
                style: 'single'
            },
            searching: true,
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
        /*table.row(':eq(0)', { page: 'current' }).select();*/
    });
}

//function InitDataTableLookupFormat(tableName) {
//    $(document).ready(function () {
//        tableName = "#" + tableName;
//        table = $(tableName).DataTable({
//            paging: false,
//            stateSave: true,
//            "bLengthChange": false,
//            scrollY: '300px',
//            scrollCollapse: true,
//            paging: false,
//            responsive: true,
//            select: {
//                style: 'single'
//            },
//            searching: true,
//            columnDefs: [
//                { targets: [0], visible: false }
//            ]
//        });
//        /*table.row(':eq(0)', { page: 'current' }).select();*/
//    });
//}

function InitDataTableLight(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            paging: false,
            stateSave: true,
            "bLengthChange": false,
            scrollY: '300px',
            scrollCollapse: true,
            paging: false,
            responsive: true,
            select: true,
            searching: false,
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
        /*table.row(':eq(0)', { page: 'current' }).select();*/
    });
}

function InitDataTableWithAssembledName(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            "bLengthChange": false,
            scrollY: '300px',
            scrollCollapse: true,
            paging: true,
            responsive: true,
            select: {
                style: 'multi'
            },
            searching: true,
            columnDefs: [
                {
                    target: 0,
                    visible: false,
                    searchable: false,
                },
                {
                    target: 1,
                    visible: false,
                },
            ]
        });
        table.row(':eq(0)', { page: 'current' }).select();
    });
}

function InitDataTableByClass() {
    $(document).ready(function () {
        table = $("table.ggtools").DataTable({
            dom: 'Blfrtip',
            paging: true,
            "pageLength": 10,
            //initComplete: function () {
            //    SetControlVisibility(tableName);
            //},
            responsive: true,
            buttons: [
                'selectAll',
                'selectNone',
                'csv',
                'excel',
                'pdf'
                //{
                //    text: '+ Add',
                //    action: function (e, dt, node, config) {
                //        AddRecord();
                //    }
                //}
            ],
            select: true,
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });

        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}

function InitDataTableLightMultiSelect(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            paging: false,
            responsive: true,
            select: {
                style: 'multi'
            },
            searching: false,
            columnDefs: [
                {
                    target: 0,
                    visible: false,
                    searchable: false,
                },
                {
                    target: 1,
                    visible: false,
                },
            ]
        });
    });
}

function SetControlVisibility(tableName) {
    //TODO Logic to disable any controls "linked" to this table that depend on its data.
    //alert("TABLE " + tableName + "LOADED");
    var table = $('#' + tableName).DataTable();
    var rowCount = table.data().count();

    if (rowCount == 0) {
        $("#btnOpenFolderModal").addClass("disabled");
    }
    else {
        $("#btnOpenFolderModal").removeClass("disabled");
    }
        
}

function GetSelectedEntityIDs(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[0]
    });
    console.log(ids)
    return ids;
}

function GetSelectedEntityStringIDs(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return "'" + item[0] + "'"
    });
    console.log(ids)
    return ids;
}

function GetSelectedEntityLabels(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[1]
    });
    console.log(ids)
    return ids;
}

function GetSelectedEntityQuotedStrings(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return "'" + item[1] + "'";
    });
    console.log(ids)
    return ids;
}

function GetSelectedEntityText(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[1]
    });
    console.log(ids)
    return ids;
}

/* ========================================================================================
 * Edit logic
  ======================================================================================== */

/* 
 Name           : SetReadOnly()
 Description    : Iterates through all input controls within a designated div, and clears the
                  contents of each.
 Notes          : Requires that all input fields be within a div named "section-input-fields."
 */
function SetReadOnly() {
    $("#section-input-fields select").attr('readonly', true);
    $("#section-input-fields select").prop('disabled', true);
    $("#section-input-fields input").attr('readonly', true);
    $("#section-file-input-fields input").attr('readonly', true);
    $("#section-file-input-fields input").prop('disabled', true);
    $("#section-edit-controls").hide();
    $(".edit-controls").hide();
}

/* ========================================================================================
 * Search logic
  ======================================================================================== */
$(document).on("click", "[id='btnReset']", function () {
    Reset();
});

/* If the user resets the search, 1) Remove all search criteria, and 2) Clear the search-results
 * Datatable.*/
function Reset() {
    $(this).closest('form').find("input[type=text], textarea").val("");
    $("#section-search-criteria select").val("");
    $("#section-search-criteria input[type=text]").val("");
    $("textarea").val("");
    $("#ItemIDList").val("");
    $("#EventValue").val("");
    $('input:checkbox').removeAttr('checked');

    // NOTE: With the addition of saved-search list on each page, the main data table will be
    // the second one on the page.
    //var table = $('#' + $.fn.dataTable.tables()[1].id).DataTable();

    // Operates on all tables on the page.
    $.fn.dataTable
        .tables({ visible: true, api: true })
        .clear()
        .draw();
}

$(document).on("click", "[id='btnShowHideExtendedFields']", function () {
    var eventValue = $("#EventValue").val();
    if (eventValue == "EXTENDED") {
        /*alert("DEBUG HIDE EXTENDED");*/
        $("#EventValue").val("");
    }
    else {
        /*alert("DEBUG SHOW EXTENDED");*/
        $("#EventValue").val("EXTENDED");
    }
});

function SetExtendedFields() {
    var eventValue = $("#EventValue").val();
    if (eventValue == "EXTENDED") {
        $("#section-extended-search-fields").removeClass("collapsed-card");
        $("#section-extended-search-fields-body").show();
        $("#btnShowHideExtendedFieldsIcon").removeClass("fas fa-plus");
        $("#btnShowHideExtendedFieldsIcon").addClass("fas fa-minus");
        
    }
}

/* ========================================================================================
 * Note Logic
  ======================================================================================== */
function SearchNotes(link) {

    var tableName = $("#TableName").val();
    var note = $("#txtLookupNote").val();

    var formData = new FormData();
    formData.append("TableName", tableName);
    formData.append("Note", note);

    $.ajax({
        url: link,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $("#section-lookup-note-search-results").html(response);
        },
        error: function () {
            alert("Error");
        }
    });
}

function GetSelectedNote(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[1]
    });
    return ids;
}

/* 
 ========================================================================================
 Family Logic
 ======================================================================================== 
 */
//function SearchFamily(link) {
//    var lookupFamilyName = $("#txtLookupFamilyName").val();
//    var formData = new FormData();

//    formData.append("LookupFamilyName", lookupFamilyName);

//    $.ajax({
//        url: link,
//        type: 'POST',
//        cache: false,
//        contentType: false,
//        processData: false,
//        data: formData,
//        success: function (response) {
//            $("#section-family-lookup-search-results").html(response);
//        }
//    });
//}

//function SaveFamily() {
//    var selectedItemIdList = GetSelectedEntityIDs("data-table-family-lookup");
//    var selectedItemNameList = GetSelectedEntityLabels("data-table-family-lookup");

//    $("#Entity_FamilyID").val(selectedItemIdList);
//    $("#Entity_FamilyName").val(selectedItemNameList);
//}


/* 
 ========================================================================================
 Genus Logic
 ======================================================================================== 
 */
//function SearchGenus(link) {
//    var lookupGenusName = $("#txtLookupGenusName").val();
//    var formData = new FormData();

//    formData.append("LookupGenusName", lookupGenusName);

//    $.ajax({
//        url: link,
//        type: 'POST',
//        cache: false,
//        contentType: false,
//        processData: false,
//        data: formData,
//        success: function (response) {
//            $("#section-genus-lookup-search-results").html(response);
//        }
//    });
//}

//function SaveGenus() {
//    var selectedItemIdList = GetSelectedEntityIDs("data-table-genus-lookup");
//    var selectedGenusNameList = GetSelectedEntityLabels("data-table-genus-lookup");

//    $("#Entity_GenusID").val(selectedItemIdList);
//    $("#Entity_GenusName").val(selectedGenusNameList);
//}

/* 
========================================================================================
Species Logic
======================================================================================== 
 */
//function SearchSpecies(link) {
//    var lookupSpeciesName = $("#txtLookupSpeciesName").val();
//    var formData = new FormData();

//    formData.append("SpeciesName", lookupSpeciesName);

//    $.ajax({
//        url: link,
//        type: 'POST',
//        cache: false,
//        contentType: false,
//        processData: false,
//        data: formData,
//        success: function (response) {
//            $("#section-species-lookup-search-results").html(response);
//        }
//    });
//}

//function SaveSpecies() {
//    var selectedItemIdList = GetSelectedEntityIDs("data-table-species-lookup");
//    var selectedSpeciesNameList = GetSelectedEntityLabels("data-table-species-lookup");
//    $("#Entity_SpeciesID").val(selectedItemIdList);
//    $("#Entity_SpeciesName").val(selectedSpeciesNameList);
//}

/* 
========================================================================================
Accepted-Name Control Logic
======================================================================================== 
*/
$(document).on("click", "[id*='btnSetAccepted']", function () {
    var id = $(this).attr("id");
    var status = id.includes("On");
    ToggleAcceptedNameControls(status);
});

function SetControlVisibility() {
    var recordIsAccepted = $("#Entity_IsAcceptedName").val();
    if (recordIsAccepted == "Y") {
        $("#btnSetAcceptedOn").hide();
        $("#btnSetAcceptedOff").show();
        $(".accepted").hide();
    }
    else {
        $("#btnSetAcceptedOn").show();
        $("#btnSetAcceptedOff").hide();
        $(".accepted").show();
    }
}

function ToggleAcceptedNameControls(status) {
    var entityId = $("#Entity_ID").val();

    // If status is set to "ACCEPTED", set accepted (current) ID equal to the species ID.
    // Otherwise, use the specified accepted-name ID.
    if (status == true) {
        $("#Entity_AcceptedID").val(entityId);
        $("#Entity_AcceptedName").val("");

        $("#Entity_IsAcceptedName").val("Y");
        $("#btnSetAcceptedOn").hide();
        $("#btnSetAcceptedOff").show();
        $(".accepted").hide();
    }
    else {
        $("#Entity_IsAcceptedName").val("N");
        $("#btnSetAcceptedOn").show();
        $("#btnSetAcceptedOff").hide();
        $(".accepted").show();
    }

}

function SetAcceptedNameControls(selectorControlId) {
    var entityId = $("#Entity_ID").val();
    $(".accepted").hide();
    if ($('#' + selectorControlId).prop('checked') == true) {
        $("#Entity_IsAcceptedName").val("Y");
        $(".accepted").hide();
        $("#Entity_AcceptedID").val(entityID);
    }
    else {
        $("#Entity_IsAcceptedName").val("N ");
        $(".accepted").show();
    }
}

/*
 * Search utilities
 */
