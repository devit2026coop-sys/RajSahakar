var AppClasses = angular.module("TransportMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("TransportCnt", function ($scope, $http, $rootScope) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 2,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'created_dated' || $scope.Filter.FilterType == 'request_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetTransportList();
    }
    /*------------End Paging and Filter-----------------*/

    $scope._language = 'en-us';   
    $scope._language = _language;
    $scope.MasterModel = {
        Transportid: 0,
        TransportNameEn: "",
        TransportNameHi: "",
        AddressEn: "",
        AddressHi: "",
        MiningId: "1",
        FinancialYear: "",
        Email: ""
    }

    $scope.FinancialYear=[]
    $scope.TodayDate = "";
    $scope.CurrentFY = ""
    $scope.GetCurrentDate = function () {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        
        $scope.TodayDate = dd + '/' + mm + '/' + yyyy;
        $scope.MasterModel.FinancialYear = yyyy + '-' + (parseInt(yyyy.toString().substring(2, 4)) + 1);
        $scope.FinancialYear.push({ FinancialYear: $scope.MasterModel.FinancialYear, Id: yyyy});
        for (var i = 1; i <= 5; i++) {
            $scope.FinancialYear.push({ FinancialYear: (yyyy - i) + '-' + (parseInt((yyyy - i).toString().substring(2, 4)) + 1), Id: (yyyy - i) });
        }
    }
    $scope.GetCurrentDate();
    if (_language == 'en-us') {
        $http.get("../Translations/Translation_EN.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
    }
    else {
        $http.get("../Translations/Translation_HI.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }
    $scope.AddNew = function () {
        $scope.ResetModel();
        $("#addModal").modal("show");
    }
    $scope.SaveMasterData = function () {
        if ($scope.ValidateMaster()) {
            $(".blockUI").fadeIn("slow");
            var formData = $scope.MasterModel;
            $http.post(SiteUrl + "/FertilizerSupply/TransportMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Name Already Exist." : "नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Name successfully submited." : "नाम सफलतापूर्वक सबमिट किया गया", function () {
                            window.location.href = "/FertilizerSupply/TransportMaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }

    $scope.ValidateMaster = function () {
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.TransportNameEn.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter name in English.' : '\n <br />कृपया नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.TransportNameHi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter name in Hindi.' : '\n <br />कृपया नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }

        if ($scope.MasterModel.AddressEn.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Address in English.' : '\n <br />कृपया पता अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.AddressHi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Address in Hindi.' : '\n <br />कृपया पता हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }


        if (message.length > 29) {
            ShowModelAlert("Scheme Master", message);
            return false;
        }
        return true;
    }

    $scope.ResetModel = function () {
        $scope.MasterModel = {
            Transportid: 0,
            TransportNameEn: "",
            TransportNameHi: "",
            AddressEn: "",
            AddressHi: "",
            MiningId: "1",
            FinancialYear: $scope.MasterModel.FinancialYear,
            Email: ""
        }
    }
    $scope.TransportList = [];
    $scope.GetTransportList = function () {
        $(".blockUI").fadeIn("slow");
        $scope.TransportList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/FertilizerSupply/GetTransportMasterList", JSON.stringify({ Details: jsonParameters})).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.TransportList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");               

            }).
            error(function (data, status) {
                $scope.TransportList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetTransportList();

    $scope.EditMaster = function(item) {
        $scope.MasterModel = {
            Transportid: item.transport_id,
            TransportNameEn: item.transport_name_en,
            TransportNameHi: item.transport_name_hi,
            AddressEn: item.address_en,
            AddressHi: item.address_hi,
            MiningId: item.mining_id,
            FinancialYear: item.FinancialYear,
            Email: item.email
        }
        $("#addModal").modal("show");
    }
});