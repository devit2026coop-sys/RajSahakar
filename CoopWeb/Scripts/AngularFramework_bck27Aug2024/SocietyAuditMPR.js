var CommityRegistrtionModule = angular.module('SocietyAuditMPRMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('SocietyAuditMPRCnt', function ($scope, $http, $rootScope) {

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
        if ($scope.Filter.FilterType == 'SubmissionDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            $scope.AddCalender();
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
        $scope.GetElectionSocietyListData();
    }
    /*------------End Paging and Filter-----------------*/
    $scope.FilterSearch = {};

    $scope.FilterSearch.Month = (new Date().getMonth() + 1).toString();
    $scope.FilterSearch.Year = (new Date().getFullYear()).toString();
    $("#ddrYear").val(new Date().getFullYear());
    $("#ddrMonth").val((new Date().getMonth() + 1));

    $scope.FinancialYearList = [];
    $scope.BindFY = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
                $scope.FilterSearch.Month = (new Date().getMonth() + 1).toString();
                $scope.FilterSearch.Year = (new Date().getFullYear()).toString();
                $scope.GetSocietyRegistrationTimeSheet();
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }

    $scope.Divisions = [{}];
    $scope.IsShowDivision = false;
    if ($("#hdnDivisionId").val() != "0" && $("#hdnDivisionId").val() != "0") {

        $("#ddlDivision").val($("#hdnDivisionId").val())
        $("#ddlDivision").attr("disabled", "disabled");
        $("#ddlDistrictId").val($("#hdnDistrictId").val())
        $("#ddlDistrictId").attr("disabled", "disabled");

        $scope.FilterSearch.DIVISION_ID = $("#hdnDivisionId").val();
        $scope.FilterSearch.DistrictId = $("#hdnDistrictId").val();
        $scope.IsShowDivision = true;
    } 

    if ($("#hdnDivisionId").val() != "0") {
        $("#ddlDivision").val($("#hdnDivisionId").val())
        $("#ddlDivision").attr("disabled", "disabled");
        $scope.FilterSearch.DIVISION_ID = $("#hdnDivisionId").val();
        //$scope.FilterSearch.DistrictId = $("#hdnDistrictId").val();
        $scope.IsShowDivision = true;

    }   

    if ($("#hdnDistrictId").val() != "0") {
        //alert($("#hdnDistrictId").val());
        $("#ddlDistrictId").val($("#hdnDistrictId").val())
        $("#ddlDistrictId").attr("disabled", "disabled");
        //$scope.FilterSearch.DIVISION_ID = $("#hdnDivisionId").val();
        $scope.FilterSearch.DistrictId = $("#hdnDistrictId").val();
        $scope.IsShowDivision = false;

    } else {
        $scope.IsShowDivision = true;

    }

    $scope.BindDivisions = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
    }
    $scope.BindDivisions();
    $scope.BindFY();
    

    $scope.MonthList = [
        { Id: "1", NameEn: "January", NameHi: "जनवरी" },
        { Id: "2", NameEn: "February", NameHi: "फ़रवरी" },
        { Id: "3", NameEn: "March", NameHi: "मार्च" },
        { Id: "4", NameEn: "April", NameHi: "अप्रैल" },
        { Id: "5", NameEn: "May", NameHi: "मई" },
        { Id: "6", NameEn: "June", NameHi: "जून" },
        { Id: "7", NameEn: "July", NameHi: "जुलाई" },
        { Id: "8", NameEn: "August", NameHi: "अगस्त" },
        { Id: "9", NameEn: "September", NameHi: "सितंबर" },
        { Id: "10", NameEn: "October", NameHi: "अक्टूबर" },
        { Id: "11", NameEn: "November", NameHi: "नवंबर" },
        { Id: "12", NameEn: "December", NameHi: "दिसंबर" }];

    $scope.TimeSheet = { Type: '1' };
    $scope.IsSubmiApplication = false;
    $scope.AuditMRPList = [];
    // $scope.LoanApplication.LOAN_ID = '0';

    $scope.selectedMlonth = (_language == 'en-us' ? $scope.MonthList.filter(function (x) { return x.Id === $scope.FilterSearch.Month })[0].NameEn: $scope.MonthList.filter(function (x) { return x.Id === $scope.FilterSearch.Month })[0].NameHi) + " " + $scope.FilterSearch.Year+" ";

    $scope.GetSocietyRegistrationTimeSheet = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/mis/GetAuditMPRMonthWise", JSON.stringify({ Details: jsonParameters, type: 1 })).
            success(function (data, status) {

                $scope.AuditMRPList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.AuditMRPList = [];
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    //$scope.GetSocietyRegistrationTimeSheet();
    $scope.GetLoanApplicationDetails = function (table, loanId) {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/pldb/GetLoanApplicationDetails", JSON.stringify({ Table: table, LoanId: loanId })).
            success(function (data, status) {
                $scope.ElectionSocietyListData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    $scope.dvApplicationForm = false;
    $scope.divLoanApplicationList = true;
    $scope.ShowApplicationForm = function () {
        $scope.dvApplicationForm = true;
        $scope.divLoanApplicationList = false;
        $scope.ShowAddNewApplication = false;
    }
    $scope.ShowAddNewApplication = true;
    $scope.divUserRegistration = true;
    $scope.currentPage = 1;
    
    // START : Code for Language label setting


    $scope.getTranslation = function () {
       // _language = 'hi-in';
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
    $scope.getTranslation();

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.GetDistrictList();

    // END : Code for Language label setting
});
