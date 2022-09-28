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

/* Datatables */
function InitDataTable(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            dom: 'Blfrtip',
            paging: true,
            initComplete: function () {
                SetControlVisibility(tableName);
            }, 
            responsive: true,
            buttons: [
                'selectAll',
                'selectNone',
                'csv',
                'excel',
                'pdf'
            ],
            select: true,
            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
    });
}

function InitDataTableLight(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            paging: true,
            responsive: true,
            select: {
                style: 'single'
            },
            searching: false,
            columnDefs: [
                { targets: [0], visible: false }
            ]
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
                { targets: [0], visible: false }
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

function GetSelectedEntityLabels(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[2]
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
    $("#ItemIDList").val("");
    $("#EventValue").val("");

    // NOTE: With the addition of saved-search list on each page, the main data table will be
    // the second one on the page.
    var table = $('#' + $.fn.dataTable.tables()[1].id).DataTable();

    table
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
 * Citation Logic
  ======================================================================================== */

function LookupCitations(link) {
    var tableName = $("#TableName").val();
    var literatureTypeCode = $("#SearchEntity_LiteratureTypeCode").val();
    var standardAbbreviation = $("#SearchEntity_StandardAbbreviation").val();
    var abbreviation = $("#SearchEntity_Abbreviation").val();
    var citationTitle = $("#SearchEntity_CitationTitle").val();
    var editorAuthorName = $("#SearchEntity_EditorAuthorName").val();
    var citationYear = $("#SearchEntity_CitationYear").val();
    var formData = new FormData();

    formData.append("TableName", tableName);
    formData.append("LiteratureTypeCode", literatureTypeCode);
    formData.append("StandardAbbreviation", standardAbbreviation);
    formData.append("Abbreviation", abbreviation);
    formData.append("CitationTitle", citationTitle);
    formData.append("EditorAuthorName", editorAuthorName);
    formData.append("CitationYear", citationYear);

    try {
        $.ajax({
            url: link,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                $("#section-citation-lookup-search-results").html(response);
            }
        });
    }
    catch (ex) {
    }
}

/* ========================================================================================
 * Literature Logic
  ======================================================================================== */

function LookupLiterature(link) {
    var tableName = $("#TableName").val();
    var literatureTypeCode = $("#ddlType").val();
    var abbreviation = $("#txtAbbreviation").val();
    var referenceTitle = $("#txtReferenceTitle").val();
    var editorAuthorName = $("#txtEditorAuthorName").val();
    var publicationYear = $("#txtPublicationYear").val();
    var formData = new FormData();

    formData.append("TableName", tableName);
    formData.append("LiteratureTypeCode", literatureTypeCode);
    formData.append("Abbreviation", abbreviation);
    formData.append("ReferenceTitle", referenceTitle);
    formData.append("EditorAuthorName", editorAuthorName);
    formData.append("PublicationYear", publicationYear);

    try {
        $.ajax({
            url: link,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                $("#section-literature-lookup-search-results").html(response);
            }
        });
    }
    catch (ex) {
    }
}

function SaveLiterature(link, refreshLink) {
    var selectedItemIdList = GetSelectedEntityIDs("data-table-literature-lookup");
    var selectedItemNameList = GetSelectedEntityLabels("data-table-literature-lookup");
    $("#Entity_LiteratureID").val(selectedItemIdList);
    $("#Entity_Abbreviation").val(selectedItemNameList);
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
function SearchFamily(link) {
    var lookupFamilyName = $("#txtLookupFamilyName").val();
    var formData = new FormData();

    formData.append("LookupFamilyName", lookupFamilyName);

    $.ajax({
        url: link,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $("#section-family-lookup-search-results").html(response);
        }
    });
}

function SaveFamily() {
    var selectedItemIdList = GetSelectedEntityIDs("data-table-family-lookup");
    var selectedItemNameList = GetSelectedEntityLabels("data-table-family-lookup");

    $("#Entity_FamilyID").val(selectedItemIdList);
    $("#Entity_FamilyName").val(selectedItemNameList);
}


/* 
 ========================================================================================
 Genus Logic
 ======================================================================================== 
 */
function SearchGenus(link) {
    var lookupGenusName = $("#txtLookupGenusName").val();
    var formData = new FormData();

    formData.append("LookupGenusName", lookupGenusName);

    $.ajax({
        url: link,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $("#section-genus-lookup-search-results").html(response);
        }
    });
}

function SaveGenus() {
    var selectedItemIdList = GetSelectedEntityIDs("data-table-genus-lookup");
    var selectedGenusNameList = GetSelectedEntityLabels("data-table-genus-lookup");

    $("#Entity_GenusID").val(selectedItemIdList);
    $("#Entity_GenusName").val(selectedGenusNameList);
}

/* 
========================================================================================
Species Logic
======================================================================================== 
 */
function SearchSpecies(link) {
    var lookupSpeciesName = $("#txtLookupSpeciesName").val();
    var formData = new FormData();

    formData.append("SpeciesName", lookupSpeciesName);

    $.ajax({
        url: link,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $("#section-species-lookup-search-results").html(response);
        }
    });
}

function SaveSpecies() {
    var selectedItemIdList = GetSelectedEntityIDs("data-table-species-lookup");
    var selectedSpeciesNameList = GetSelectedEntityLabels("data-table-species-lookup");
    $("#Entity_SpeciesID").val(selectedItemIdList);
    $("#Entity_SpeciesName").val(selectedSpeciesNameList);
}

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
    if (status == true) {
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