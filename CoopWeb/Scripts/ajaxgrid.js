var _currentPageID = 1;
var _pageSize = 10;
var _sortExpression = '';
var _sortDirection = 1;
var _accessUrl = '';
var _addUrl = '';
var _deleteUrl = '';
var _disableUrl = '';
var _gridDivId = 'DataList';
var _searchBoxDivId = 'search-filter';
var _searchParameters = [];
var _otherParameters = [];
var _maxDays = 10;

$(document).ready(function () {
    $('#selectPageSize').on("change", function () {
        _pageSize = parseInt(this.value);
        _currentPageID = 1;
        asynList();
    });
    $('#search-button').on("click", function () {
        searchRequest();
    });
    //$('.sort-icon').on("click", function () {
    //    sortRequest($(this).attr('sort-expression'));
    //});
    $('.icon-trash').on("click", function () {
        deleteRequest($(this).attr('row-id'));
    });
    $('.button-disable').on("click", function () {
        disableRequest($(this).attr('row-id'));
    });
});

function asynList() {

    //$.blockUI({
    //    css: {
    //        border: 'none',
    //        padding: '15px',
    //        backgroundColor: '#000',
    //        '-webkit-border-radius': '10px',
    //        '-moz-border-radius': '10px',
    //        opacity: .5,
    //        color: '#fff'
    //    },
    //    message: 'Please wait...'
    //});
    $(".blockUI").show();
    // $('#' + _gridDivId).hide();
    //alert(JSON.stringify(jsonParameters ));
    var jsonParameters = { "TotalRecords": $("#hdnTotalRecords").val(), "PageNumber": _currentPageID, "PageSize": _pageSize, "SortingColumn": _sortExpression, "SortingOrder": _sortDirection, "SearchParameter": _searchParameters };
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Details: jsonParameters }),
        url: _accessUrl,
        success: function (result) {

            $.ajax({
                type: 'POST',
                contentType: 'application/json',
                data: {},
                url: '/home/CheckSession',
                success: function (data) {
                    if (data == false) {
                        window.location = "/";
                        return false;
                    } else {
                        $('#' + _gridDivId).html(result);

                        $('.sort-icon').find("i").removeClass('fa-sort-asc');
                        $('.sort-icon').find("i").removeClass('fa-sort-desc');
                        var length = $('.sort-icon').length;
                        for (var i = 0; i < length; i++) {

                          
                            //sortRequest('Priority');
                            if ($('.sort-icon').eq(i).attr('onclick') == "sortRequest('" + _sortExpression + "');") {
                                // alert($('.sort-icon').eq(i).attr('onclick'));
                                if (_sortDirection == 1) {
                                    $('.sort-icon').eq(i).find("i").addClass("fa-sort-asc");
                                }
                                else {
                                    $('.sort-icon').eq(i).find("i").addClass('fa-sort-desc');
                                }
                            }

                        }
                        var body = angular.element(document.getElementById("mController"));
                        //$("#body").empty();
                        body.html($("#mController").html());
                        var mController = angular.element(document.getElementById("mController"));
                        mController.scope().activateView(body);
                    }
                    
                    if (_accessUrl == "/iwedds/_advance_search") {
                        $("#adback").show();
                        $("#search-filter").hide();
                    }$('body,html').animate({ scrollTop: 0 }, 500);
                    $(".blockUI").hide();

                }
            });

        }, complete: function (result) {
            $(".blockUI").hide();
        },
        error: function (request, status, error) {
            // look for status of 401 and redirect to login
            if (status == 403) {
                window.location = "/login/Step1";
            }
        }
    });
}

$(document).ready(function () {
    $('#selectPageSize').on("change", function () {
        _pageSize = parseInt(this.value);
        _currentPageID = 1;
        asynList();
    });
});

function pageRequest(pageid) {
    _currentPageID = parseInt(pageid);
    _searchParameters.splice(0, _searchParameters.length);
    var ln = $('#' + _searchBoxDivId).find('input:text,select,input:checkbox').length;
    var indx = 1;
    $('#' + _searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

        if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
            if ($(this).is(":checked")) {
                var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                _searchParameters.push(parameter);
            }
        } else {
            if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                _searchParameters.push(parameter);
            }
        }
        if (indx == ln) {
          //  _currentPageID = 1;
            asynList();
        }
        ++indx;
    });
    // alert(1)
    // alert(JSON.stringify(_searchParameters));
   // asynList();
}

function searchRequest() {
    _searchParameters.splice(0, _searchParameters.length);
    //$('#' + _searchBoxDivId).find('input:text').each(function () {
    //    if ($(this).val() != '' && $(this).val() != null) {
    //        if ($(this).attr('name') != '' && $(this).attr('name') != null) {
    //            var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
    //            _searchParameters.push(parameter);
    //        }
    //    }
    //});
    //$('#' + _searchBoxDivId).find('input:checkbox').each(function (indx) {
    //    if ($(this).is(":checked")) {
    //        var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
    //        _searchParameters.push(parameter);
    //        //alert(JSON.stringify(parameter));
    //    }
    //});
    //$('#' + _searchBoxDivId).find('select').each(function () {
    //    if ($(this).val() != '' && $(this).val() != null) {
    //        var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
    //        _searchParameters.push(parameter);
    //    }
    //});
    //
    var ln = $('#' + _searchBoxDivId).find('input:text,select,input:checkbox').length;
    var indx = 1;   
    $('#' + _searchBoxDivId).find('input:text,select,input:checkbox').each(function () {
        
        if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
            if ($(this).is(":checked")) {            
                var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                _searchParameters.push(parameter);              
            }
        } else {
            if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {               
                var parameter = { Key: $(this).attr('name'), Value: $(this).val() }            
                _searchParameters.push(parameter);
            }
        }
        if (indx == ln) {
            _currentPageID = 1;
            if (_accessUrl == "/iwedds/_advance_search" && _searchParameters.length <= 0) {
                $("#AboutMessage").html("Please select at least one filter");
            } else {
                asynList();
            }
        }
        ++indx;
    });
}

function resetFilter() {
    _searchParameters.splice(0, _searchParameters.length);
    $('#' + _searchBoxDivId).find('input:text').each(function () {

        $(this).val("");

    });
    $('#' + _searchBoxDivId).find('input:checkbox').each(function (indx) {

        $(this).prop("checked", false);

    });
    $('#' + _searchBoxDivId).find('select').each(function () {
        $(this).val("");
    });
    $("#ageFrom").val("18");
    $("#ageTo").val("75");
    $("#heightFrom").val("3");
    $("#heightTo").val("7");
    _currentPageID = 1;
    asynList();

}

function sortRequest(sortColumn) {
    if (_sortExpression == sortColumn) {
        if (_sortDirection == 1) {
            _sortDirection = 2;

        }
        else {
            _sortDirection = 1;

        }
    }
    else {
        _sortDirection = 1;

    }
    _currentPageID = 1;
    _sortExpression = sortColumn;
    asynList();

}