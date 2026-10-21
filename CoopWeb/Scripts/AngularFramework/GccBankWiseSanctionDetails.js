var MasterMod = angular.module('GccBankWiseListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("GccBankWiseListCnt", function ($rootScope, $scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GccSummary = [];
    $scope.TotalGccSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;

    /*------START------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    };

    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.isBankDisabled = false;
    $scope.SearchText = "";
    $scope.Filter = [];


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
    };

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
    };

    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };


    $scope.searchRequest = function (callback) {
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;

        var indx = 1;

        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio')
            .each(function () {
                // Checkbox
                if ($(this).attr("type") == "checkbox") {
                    if ($(this).is(":checked")) {
                        var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() };
                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }
                // Radio Button
                else if ($(this).attr("type") == "radio") {
                    if ($(this).is(":checked")) {
                        var parameter = { Key: $(this).attr('name'), Value: $(this).val() };
                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }
                // Textbox / Select
                else {
                    if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                        var parameter = { Key: $(this).attr('name'), Value: $(this).val() };
                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }
                if (indx == ln) {
                    $scope.DataPaging.currentPageID = 1;
                    callback();
                }
                ++indx;
            });
    };


    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
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
        //$scope.GetDistrictWithoutDivisionList();
        $scope.FinancialYearList();
        var hdnDistId = $('#hdnDistrictId').val();
        console.log('hdnDistId', hdnDistId);
        if (hdnDistId > 0) {
            $scope.isLBDisabled = true;
            $scope.search.LoanDistrictId = hdnDistId
        } else {
            $scope.isLBDisabled = false;
        }
        $scope.LoanBankList();

    };


    //GET Financial Year
    $scope.FinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }

    function getCurrentFinancialYear() {
        var today = new Date();
        var year = today.getFullYear();
        var month = today.getMonth() + 1; // Months are zero-based
        var startYear, endYear;
        if (month >= 4) { // April (4th month) starts the new financial year
            startYear = year;
            endYear = year + 1;
        } else {
            startYear = year - 1;
            endYear = year;
        }
        return startYear + '-' + (endYear.toString().slice(-2));
    }

    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                $scope.search.LoanBankID = '0';
                $scope.search.LoanBranchId = '0';
                $scope.search.BLOCK_ID = '0';
                $scope.GccBankWiseSanctionList();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.BlockListByOffAdd = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.search.DISTRICT_ID).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Blocksdata = data;
                $scope.GetGccSummary();
            }).
            error(function (data, status) {
                $scope.Blocksdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //$scope.LoanBankList = function () {
    //    $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.search.LoanDistrictId).
    //        success(function (data, status) {
    //            if ($scope.search.LoanDistrictId > 0) {
    //                $scope.BankMasterList = data.filter(x => x.DistrictId == $scope.search.LoanDistrictId);
    //                $scope.selectedCCBId = data.filter(x => x.DistrictId == $scope.search.LoanDistrictId)[0].CCBId;
    //                $scope.search.LoanBankID = $scope.selectedCCBId;
    //                $scope.Get_LoanBankBranch($scope.selectedCCBId);
    //            }
    //            else {
    //                $scope.BankMasterList = data;
    //                $scope.search.LoanBankID = '0';
    //                $scope.search.LoanBranchId = '0';
    //            }
    //        }).
    //        error(function (data, status) {
    //            $scope.BankMasterList = [];
    //        });
    //};

    $scope.LoanBankList = function () {
        var hdnDistId = $('#hdnDistrictId').val();

        console.log("LoanDistrictId:", $scope.search.LoanDistrictId);

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + hdnDistId)
            .success(function (data) {

                console.log("API Data:", data);

                $scope.BankMasterList = data;

                console.log("BankMasterList:", $scope.BankMasterList);

                if ($scope.BankMasterList.length == 1) {

                    $scope.search.LoanBankID = $scope.BankMasterList[0].CCBId.toString();
                    $scope.isBankDisabled = true;

                    console.log("Selected Bank:", $scope.search.LoanBankID);

                    $scope.Get_LoanBankBranch($scope.search.LoanBankID);
                }
            });
    };

    $scope.Get_LoanBankBranch = function (branch_ID) {
        var districtId = $('#hdnDistrictId').val();
        $scope.search.LoanDistrictId = districtId;

        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.search.LoanDistrictId + "&LoanBankId=" + $scope.search.LoanBankID).
            success(function (data, status) {
                $scope.BankBranches = data;
                $scope.search.LoanBranchId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GccBankWiseSanctionList();
    }


    $scope.Clear = function () {
        $scope.search.ReportType = '0';
        $scope.search.FinancialYearId = '0';
        $scope.search.LoanBranchId = '0';
        $scope.search.LoanBankID = '0';
        $scope.search.NODODStatus = '0';
        $('#txtFromDate').val('');
        $('#txtToDate').val('');
        $scope.GopalCreditCardModel.IsRenew = null;
        $scope.GopalCreditCardModel.Type = null;
        $scope.GccSummary = [];
        $scope.DataPagingDetails = {};
        $scope.pageSize = 50;
    };

    $scope.FromDate = "";
    $scope.ToDate = "";
    $scope.SearchData = function () {

        $scope.FromDate = $('#txtFromDate').val();
        $scope.ToDate = $('#txtToDate').val();
        // Report Type
        if (!$scope.search.ReportType || $scope.search.ReportType == '0') {
            alert("Please select the Report Type");
            return;
        }

        //// Financial Year
        //if ( $scope.search.ReportType != 'Count' && (!$scope.search.FinancialYearId || $scope.search.FinancialYearId == 0) ) {
        //    alert("Please select Financial Year");
        //    return;
        //}

        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();

        console.log('fromDate', fromDate);
        console.log('toDate', toDate);

        if ((!fromDate || !toDate)) {
            alert("Please select From Date and To Date");
            return;
        }

        // Bank
        if ($scope.search.ReportType != 'Count' && (!$scope.search.LoanBankID || $scope.search.LoanBankID == 0)) {
            alert("Please select Bank");
            return;
        }

        // Application Type
        if ($scope.search.ReportType != 'Count' && ($scope.GopalCreditCardModel.IsRenew === undefined || $scope.GopalCreditCardModel.IsRenew === null)) {
            alert("Please select Application Type (New / Renew)");
            return;
        }

        // Type
        if (($scope.GopalCreditCardModel.Type === undefined || $scope.GopalCreditCardModel.Type === null)) {
            alert("Please select Type (Disbursement / Sanction)");
            return;
        }

        var isRenew = $scope.GopalCreditCardModel.IsRenew;

        if (isRenew === undefined || isRenew === null) {
            isRenew = 0;
        }

        //var type = $scope.GopalCreditCardModel.Type;

        //if (type === undefined || type === null) {
        //    type = 0;
        //}

        // Type (Mandatory only for Details)
        if ($scope.search.ReportType != 'Count') {

            if (!$scope.GopalCreditCardModel.Type &&
                $scope.GopalCreditCardModel.Type != '0') {

                alert("Please select File Type");
                return;
            }
        }


        $scope.searchRequest($scope.GccBankWiseSanctionList);
    };

    //New
    $scope.GccBankWiseSanctionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GccSummary = [];
        var jsonParameters = { TotalRecords: $scope.DataPaging.TotalRecords, PageNumber: $scope.DataPaging.currentPageID, PageSize: $scope.pageSize, SortingColumn: $scope.DataPaging.sortExpression, SortingOrder: $scope.DataPaging.sortDirection, SearchParameter: $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/GetGccBankWiseSanctionList", JSON.stringify({ Details: jsonParameters })).success(function (data) {
            if (data && data.GccSummary && data.GccSummary && data.GccSummary) {
                $scope.GccSummary = JSON.parse(data.GccSummary);
                console.log('$scope.GccSummary', $scope.GccSummary);
                if (data.DataPaging) {
                    $scope.DataPagingDetails = data.DataPaging;
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $scope.DataPaging.currentPageID = data.DataPaging.PageNumber;
                    $rootScope.SetDataPaging();
                }
            }
            else {
                $scope.GccSummary = [];
            }

            $(".se-pre-con").fadeOut("slow");
        })
            .error(function (error) {
                console.log("Error", error);
                $scope.GccSummary = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.OnBankChange = function () {
        $scope.search.LoanBranchId = '0';
        if ($scope.search.LoanBankID != '0') {
            $scope.Get_LoanBankBranch($scope.search.LoanBankID);
        }
        else {
            $scope.BranchList = [];
        }
    };


    $scope.OnReportTypeChange = function () {

        //$scope.LoanBankList();

        // COUNT
        if ($scope.search.ReportType == 'Count') {

            $scope.searc.ReportType = 'Count';
            $scope.searc.FinancialYearId = '0';
            $scope.searc.LoanBranchId = '0';
            $scope.searc.IsOverall = '0';
            $scope.searc.Type = '';
            $scope.search.ODODStatus = '0';


            $scope.GopalCreditCardModel = {
                IsRenew: '',
                Type: ''
            };

            $('#txtFromDate').val('');
            $('#txtToDate').val('');

            $scope.GccSummary = [];
            //setTimeout(function () {
            //    $scope.$apply(function () {
            //        $scope.searchRequest($scope.GccBankWiseSanctionList);
            //    });
            //}, 0);
        }
        // DETAILS
        else {
            $scope.GccSummary = [];
        }
    };

    $scope.exportAllToExcel = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, PageSize: $scope.DataPaging.TotalRecords, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        $http.post(SiteUrl + "/MIS/GetGccBankWiseSanctionList", jsonParameters)
            .success(function (data) {
                var allData = JSON.parse(data.GccSummary);
                if (allData.length == 0) {
                    alert("No records found");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                exportJsonToExcel(allData, "GCC Bank Wise List");
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                alert("Error while exporting data");
                $(".se-pre-con").fadeOut("slow");
            });
    };

    function exportJsonToExcel(data, fileName) {
        var filteredData = data.map(function (item, index) {
            var newItem = {};
            newItem["S.No"] = index + 1;

            Object.keys(item).forEach(function (key) {
                if (key !== "TotalRecords") {
                    newItem[key] = item[key];
                }
            });
            return newItem;
        });

        var worksheet = XLSX.utils.json_to_sheet(filteredData);
        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
        XLSX.writeFile(workbook, fileName + ".xlsx");
    }


    $scope.IsOverall = false;
    $scope.GopalCreditCardModel = {};
    $scope.GopalCreditCardModel.IsPendancy = 0;
    $scope.GridShow = function (val) {
        if (val == 0) {
            $scope.IsOverall = false;
        }
        else {
            $scope.IsOverall = true;

        }
    }

    $scope.getReportTypeText = function () {

        switch ($scope.GopalCreditCardModel.Type) {
            case '0':
                return "Applications Disbursed";
            case '1':
                return "Applications Sanctioned";
            case '2':
                return "Applications Received";
            case '3':
                return "NOC Generated Applications";
            default:
                return "Applications";
        }
    };

    $scope.getReportTypeHindiText = function () {

        switch ($scope.GopalCreditCardModel.Type) {
            case '0':
                return "वितरित किए गए आवेदनों";
            case '1':
                return "स्वीकृत किए गए आवेदनों";
            case '2':
                return "प्राप्त आवेदनों";
            case '3':
                return "एनओसी जनरेट किए गए आवेदनों";
            default:
                return "आवेदनों";
        }
    };

    $scope.getTotal = function (columnName) {
        if (!$scope.GccSummary || $scope.GccSummary.length === 0) {
            return 0;
        }
        var total = 0;
        for (var i = 0; i < $scope.GccSummary.length; i++) {
            var val = parseFloat($scope.GccSummary[i][columnName]);
            if (!isNaN(val)) {
                total += val;
            }
        }
        return total;
    };


    $scope.OnPageLoad();

});
MasterMod.directive('datePicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            element.datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                yearRange: '1950:2050',

                onSelect: function (dateText) {
                    scope.$apply(function () {
                        ngModelCtrl.$setViewValue(dateText);
                    });
                }
            });

            // Destroy datepicker when Angular element is destroyed
            scope.$on('$destroy', function () {
                element.datepicker('destroy');
            });
        }
    };
});
