var AssignAuditorModule = angular.module('UploadAuditByAuditorMod', ['angularUtils.directives.dirPagination']);
AssignAuditorModule.controller("UploadAuditByAuditorCont", function ($rootScope, $scope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 300,
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
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.FinancialYear = "";
        $scope.Filter.DistrictId = "";
        $scope.Filter.SocietyIdENC = "";
        $scope.Filter.ActId = "";
        $scope.Filter.RequestStatus = "";
        $scope.Filter.IsFirm = "";
        $scope.Filter.SerachText = "";
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetSocietyListAssignedToAuditor();
    }
    /*------------End Paging and Filter-----------------*/

    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
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
        //GET Financial Year

    };

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();
    $scope.getTranslation();
    $scope.AssignedAuditor = [];

    $scope.FinancialYearList = [];
    var backFy = ''
    if ((new Date().getMonth() + 1) < 4) {
        backFy = new Date().getFullYear() - 2;
    } else {
        backFy = new Date().getFullYear() - 1;
    }

    var isPageLoad = true;
    var isBackFy = false;

    $scope.BindFY = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
               
                isBackFy = getUrlVars()["year"] == '' || getUrlVars()["year"] == undefined ? false : true;
                if (isBackFy) {
                    $scope.Filter.FinancialYear = getUrlVars()["year"];// backFy + '-' + (parseInt(backFy) + 1).toString().substring(2, 4);
                }
                setTimeout($scope.searchRequest($scope.GetSocietyListAssignedToAuditor), 1000);
               
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }



    $scope.BindFY();
    $scope.GetSocietyListAssignedToAuditor = function () {
        $(".blockUI").fadeIn("slow");

        if (isPageLoad && isBackFy)
        {
            var parameter = { Key: 'FIN_YEAR_NAME', Value: $scope.Filter.FinancialYear }
            $scope.DataPaging.searchParameters.push(parameter);
        }

        
        isPageLoad = false;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetSocietyAuditDocumentUploadReport", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                //// $scope.SocietyList = data;
                $scope.AssignedAuditor = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");


            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };
  //  $scope.GetSocietyListAssignedToAuditor();

    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
        success(function (data, status) {
            $scope.ActList = data;
        }).
        error(function (data, status) {
        });
    $scope.SocietyList = [];
    $http.get(SiteUrl + "/MIS/GetSocietyListAuditDocumentUploadReport").
        success(function (data, status) {
            $scope.SocietyList = JSON.parse(data.DataList);
        }).
        error(function (data, status) {
            $scope.SocietyList = [];
        });



    // SCOPE
    //$scope._language = 'en-us';

    // Code for Language label setting

    // START :Get List of Societies

    // END :Get List of Societies

    // START :Get List of Districts
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.ViewDocuments = function (Id) {
        $(".se-pre-con").fadeIn("slow");
        $("#myModal").modal("show");
        $http.get(SiteUrl + "/Auditor/GetAuditUplodedDocument?Id=" + Id).
            success(function (data, status) {
                $scope.DocumentList = data;
            }).
            error(function (data, status) {
                $scope.DocumentList = [];
                alert('Error while Document fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }


});