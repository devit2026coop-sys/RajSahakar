var WomanSavingCreditSocietyModule = angular.module("WomanSavingCreditSocietyRPTMod", ['angularUtils.directives.dirPagination']);
WomanSavingCreditSocietyModule.controller("WomanSavingCreditSocietyRPTCont", function ($rootScope, $scope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 100,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }   
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.SearchData();
    }
    /*------------End Paging and Filter-----------------*/
  
    $scope.OnPageLoad = function () {

        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
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
        };
    }
  
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
          }
 
    $scope.ClearSearch = function () {
    $scope.Filter.SerachText = "";
    }

    $scope.GetWomanSavingCreditSociety = function () {     
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "MIS/GetWomanSavingCreditSociety?DistrictId=" + $("#ddrDistrictId").val() + "&Month=" + $("#txtFromMonth").val(), JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log(data.DataList);
                if (data.DataList.length > 2) {
                    $scope.WomanSavingCreditSociety = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                }
                else {
                    $scope.WomanSavingCreditSociety = [];
                    alert("No Data Available");                   
                    $rootScope.DataPagingDetails.TotalRecords = 0;                    
                }
                
            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            });
    };
    $scope.ExportData = function () {
        var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	
        excel.set({ sheet: 0, value: "Data" });
        // We want ROW 3 to be EXTRA TALL
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { IsAllRecords:true, "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "MIS/GetWomanSavingCreditSociety?DistrictId=" + $("#ddrDistrictId").val() + "&Month=" + $("#txtFromMonth").val(), JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                var WomanSavingCreditSocietyData = JSON.parse(data.DataList);
                var curr = 1
                var formatHColumn = excel.addStyle({ 															// Format for headers
                    //border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 15 #324148"
                });
                excel.set(0, 0, curr, $scope.translation.WomenSavingCreditSocietyReport, formatHColumn);
                if ($('#txtMonth').val() != '' || $('#txtMonth').val() == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
                    excel.set(0, 1, curr, $('#txtMonth').val(), formatHColumn);
                }

                if ($scope.DistrictId != '' && $scope.DistrictId != '0') {
                    ++curr;
                    excel.set(0, 0, curr, $scope.translation.DistrictHeading, formatHColumn);
                    excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $scope.DistrictId })[0].DISTRICT_MANGAL, formatHColumn);
                }

                // This array holds the HEADERS text
                var formatHeader = excel.addStyle({ 															// Format for headers
                    border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 12 #fff B",
                    background: "#64c0c5",
                }); 														// 		Font for headers
                curr = curr + 1;                
                excel.set(0, 0, curr + 1, $scope.translation.SRNO, formatHeader);
                excel.set(0, 1, curr + 1, $scope.translation.NameOfSociety, formatHeader);
                excel.set(0, 2, curr + 1, $scope.translation.SocietyAddressPhoneNoEmail, formatHeader);
                excel.set(0, 3, curr + 1, $scope.translation.RegistrationNo, formatHeader);
                excel.set(0, 4, curr + 1, $scope.translation.RegistrationDate, formatHeader);
                excel.set(0, 5, curr + 1, $scope.translation.PresidentNameAddressPhoneno, formatHeader);
                excel.set(0, 6, curr + 1, $scope.translation.NumberofMembers, formatHeader);
                excel.set(0, 7, curr + 1, $scope.translation.SocietyShareAmount, formatHeader);
                excel.set(0, 8, curr + 1, $scope.translation.SecurityDeposit, formatHeader);
                excel.set(0, 9, curr + 1, $scope.translation.WorkingCapital, formatHeader);
                excel.set(0, 10, curr + 1, $scope.translation.LoanAmountAndSource, formatHeader);
                excel.set(0, 11, curr + 1, $scope.translation.BusinessOfTheSociety, formatHeader);
                excel.set(0, 12, curr + 1, $scope.translation.Specifications + "(" + $scope.translation.Active + "/" + $scope.translation.InActive+")", formatHeader);
                // 		in color green
                var formatColumn = excel.addStyle({ 															// Format for headers
                    border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 10 #324148"
                });          

                curr = curr + 2;
                WomanSavingCreditSocietyData.forEach(function (item, index) {
                    excel.set(0, 0, index + curr, (index + 1), formatColumn);
                    excel.set(0, 1, index + curr, item.SocietyName, formatColumn);
                    excel.set(0, 2, index + curr, item.SocietyAddress, formatColumn);
                    excel.set(0, 3, index + curr, item.RegistrationNo, formatColumn);
                    excel.set(0, 4, index + curr, item.RegistrationDate, formatColumn);
                    excel.set(0, 5, index + curr, item.CEOName + ", " + item.CEOAddress + ", " + item.CEOContactNo , formatColumn);
                    excel.set(0, 6, index + curr, item.TotalMember, formatColumn);
                    excel.set(0, 7, index + curr, item.TotalShareAmount, formatColumn);
                    excel.set(0, 8, index + curr, item.SecurityDeposit, formatColumn);
                    excel.set(0, 9, index + curr, item.WorkingCapital, formatColumn);
                    excel.set(0, 10, index + curr, item.TotalLoan + "/" + item.LoanSourse, formatColumn);
                    excel.set(0, 11, index + curr, item.BusinessNameHi, formatColumn);
                    excel.set(0, 12, index + curr, item.IsActive ? $scope.translation.Active : $scope.translation.InActive, formatColumn);
                });
                excel.generate($scope.translation.MPR2001MembersShareCapital + ".xlsx");
            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            });
    }
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "") {
                    $("#ddrDistrictId").attr("disabled", "disabled");
                    $scope.DistrictId = $("#hdnDistrictId").val()
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.GetDistrictList();
    $scope.OnPageLoad();
    $scope.searchRequest($scope.GetWomanSavingCreditSociety);
});


