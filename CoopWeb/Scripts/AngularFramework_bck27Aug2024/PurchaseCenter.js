var PurchaseCenterMod = angular.module('PurchaseCenterModule', ['angularUtils.directives.dirPagination']);
PurchaseCenterMod.controller("PurchaseCenterCont", function ($scope, $http, $rootScope) {



    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
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

  
    $scope.ClearSearch = function () {
        $scope.PurchaseCenter.SearchText = "";
        $scope.PurchaseCenter.DISTRICT_ID = '0';
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetPurchaseCenterList();
    }
    /*------------End Paging and Filter-----------------*/



    $scope.OnPageLoad = function () {
        $scope._language = _language;

        if (_language === 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    // END : Page load Function

    $scope.divShowdata = true;
    $scope.AddNew = function () {
        $scope.divInsert = true;
        $scope.divShowdata = false;
        $scope.PurchaseCenter.PurchaseCenterNameEn = '';
        $scope.PurchaseCenter.PurchaseCenterNameEn = '';
        $scope.PurchaseCenter.DISTRICT_ID = '0';
        $('#IsActive').prop("checked", false);
    };
    $scope.PurchaseCenter = {};

    $scope.PurchaseCenter.Flag = 1;
    $scope.EditRecord = function (item) {
        
        $scope.divInsert = true;
        $scope.divShowdata = false;
        
        $scope.PurchaseCenter.PurchaseCenterId = item.PurchaseCenterId;
        $scope.PurchaseCenter.DISTRICT_ID = item.DistrictId.toString();
        $scope.PurchaseCenter.PurchaseCenterNameEn = item.PurchaseCenterNameEn;
        $scope.PurchaseCenter.PurchaseCenterNameHi = item.PurchaseCenterNameHi;
        $scope.PurchaseCenter.IsActive = item.IsActive;
        $scope.PurchaseCenter.Flag = 2;

    }
  
    $scope.ChangeStatus = function (item,flag) {
        $scope.PurchaseCenter.PurchaseCenterId = item.PurchaseCenterId;
        $scope.PurchaseCenter.IsActive = item.IsActive;
        $scope.PurchaseCenter.Flag = flag;
        $http.post(SiteUrl + "/Stock/InsertPurchaseCenter", { "model": $scope.PurchaseCenter }).
            success(function (data, status) {

                $(".blockUI").fadeOut("slow");
                if ($scope.PurchaseCenter.Flag == 3) {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Purchase Center inactive successfully." : "क्रय केंद्र सफलतापूर्वक निष्क्रिय किया गया", function () {
                        $scope.GetPurchaseCenterList();
                        $scope.divInsert = false;
                        $scope.divShowdata = true;
                    });
                }
                else if ($scope.PurchaseCenter.Flag == 4) {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Purchase Center successfully deleted." : "क्रय केंद्र सफलतापूर्वक हटाया गया", function () {
                        $scope.GetPurchaseCenterList();
                        $scope.divInsert = false;
                        $scope.divShowdata = true;
                    });
                }
                else {
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                }
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
            });

    }


    $scope.SavePurchaseCenter = function () {
      
        if ($scope.ValidateForm()) {
            $http.post(SiteUrl + "/Stock/InsertPurchaseCenter", { "model": $scope.PurchaseCenter }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if ($scope.PurchaseCenter.Flag==1) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Purchase Center successfully submited." : "क्रय केंद्र सफलतापूर्वक सबमिट किया गया", function () {
                            $scope.GetPurchaseCenterList();
                            $scope.divInsert = false;
                            $scope.divShowdata = true;
                        });
                    }
                    else if (true) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Purchase Center successfully updated." : "क्रय केंद्र सफलतापूर्वक अपडेटेड किया गया", function () {
                            $scope.GetPurchaseCenterList();
                            $scope.divInsert = false;
                            $scope.divShowdata = true;
                        });
                    }
                    else {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                   
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    
    }



    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.PurchaseCenter.DISTRICT_ID === '0' || $scope.PurchaseCenter.DISTRICT_ID === undefined ) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District ' : '\n <br /> कृपया जिला चुने |';
        }
        if ($scope.PurchaseCenter.PurchaseCenterNameEn === undefined || $scope.PurchaseCenter.PurchaseCenterNameEn === "") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Purchase Center Name (in Eng)' : '\n <br /> कृपया क्रय केंद्र नाम (इंग्लिश) दर्ज करें  |';
        }
        if ($scope.PurchaseCenter.PurchaseCenterNameHi === undefined || $scope.PurchaseCenter.PurchaseCenterNameHi === "") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Purchase Center Name (in Hindi)' : '\n <br /> कृपया क्रय केंद्र नाम (हिंदी) दर्ज करें  |';
        }

        if (message.length > 5) {
            ShowModelAlert($scope.translation.PurchaseCenter, message);
            return false;
        }
        return true;
    }


  
    $scope.PurchaseCenterList = [];
    $scope.GetPurchaseCenterList = function () {
       
        $(".blockUI").fadeIn("slow");
        $scope.PriceList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetPurchaseCenter", JSON.stringify({ Details: jsonParameters, DISTRICT_ID:0})).
            success(function (data, status) {
                
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.PurchaseCenterList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.PurchaseCenterList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    }

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.OnPageLoad();
    $scope.GetDistrictList();
    $scope.GetPurchaseCenterList();
});