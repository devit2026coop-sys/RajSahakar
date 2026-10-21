
var MasterMod = angular.module('BeneficiaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryListCnt", function ($scope, $http, $rootScope) {

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
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GetPldbSummary = [];
    $scope.TotalGetPldbSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;

    $scope.ReportType = 1;
    $scope.Islinkcalled = false;
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
        $scope.PLDBListData();


        if (!$("#hdnPldbId").val() || Number($("#hdnPldbId").val()) <= 0) {$scope.GetPldbCallSummary(false); }
        
        //$scope.LoanBankList();
        //var hdnDistId = $('#hdnDistrictId').val();
        //if (hdnDistId > 0) {
        //    $scope.isLBDisabled = true;
        //    $scope.search.LoanDistrictId = hdnDistId
        //} else
        //    $scope.isLBDisabled = false;
    };

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

                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.search.PLDB_ID = "0";
                    $scope.search.PLDB_Branch_ID = "0";                    
                    $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();                            
                }
                
                if ( $scope.Islinkcalled == false ) { $scope.GetPldbCallSummary(false); }           
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    //$scope.BlockListByOffAdd = function (id) {
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.Blocksdata = [];
    //    $scope.GramPanchayats = [];
    //    $scope.Villages = [];
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.search.DISTRICT_ID).
    //        success(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.Blocksdata = data;
    //            $scope.GetPldbCallSummary();
    //        }).
    //        error(function (data, status) {
    //            $scope.Blocksdata = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
   


    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                $(".se-pre-con").fadeOut("slow");

                if ($("#hdnPldbId").val() != "0") {
                    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }
               
               
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.search.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");                    
                }

                $(".se-pre-con").fadeOut("slow");
                $scope.GetPldbCallSummary(false);
                
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetPldbCallSummary(false);
    }
    $scope.exportExcel = function (branch_ID) {
        $scope.GetPldbCallSummary(true);
    }

    $scope.GetPldbCallSummary = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
       
        var Action = "";


        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }


        if ($scope.ReportType == '1') { Action = 'PldbCallCenterReportData'; }
        else if ($scope.ReportType == '2') { Action = 'PldbCallCenter103ReportData'; }
        else if ($scope.ReportType == '3') { Action = 'PldbCallCenterBothReportData'; }
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        
        $http.post(SiteUrl + "/MIS/GetPldbCallSummary/?financialYearId=" + ($scope.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action + "&isExport=" +isExport      , JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (isExport) {
                    $scope.downloadFile(data.filename);
                    return;
                }
                else
                {
                    $scope.GetPldbSummary = [];
                    $scope.TotalGetPldbSummary = [];
                    
                }
                if (data != null && data.DataList != null) {

                    $scope.GetPldbSummary = JSON.parse(data.DataList);

                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.DataPagingDetails.TotalRecords = $scope.GetPldbSummary[0].TotalRecords;
                    $rootScope.SetDataPaging();

                    $scope.colsort = "";
                    $scope.reverseSort = false;
                    $scope.sortData = function (column) {
                        $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                        $scope.colsort = column;
                    };

                    $scope.getSortClass = function (column) {
                        if ($scope.colsort == column) {
                            return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                        }
                        return '';
                    };

                }
                else {
                    $rootScope.DataPagingDetails.TotalRecords = 0;
                }
               
            }).
            error(function (data, status) {
                $scope.GetPldbSummary = [];
                $scope.TotalGetPldbSummary = [];
                $rootScope.DataPagingDetails.TotalRecords = 0;
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.IsOverall = false;
    $scope.GopalCreditCardModel = {};
    $scope.GopalCreditCardModel.IsPendancy = 0;
    $scope.GridShow = function (val) {
        debugger;
        if (val == 0) {
            $scope.IsOverall = false;
        }
        else {
            $scope.IsOverall = true;

        }
        $scope.GetPldbCallSummary(false);
    }

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetPldbCallSummary(false);
    }
    $scope.PldbNameChange = function (item)
    {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        $scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;  
        $scope.GetPldbCallSummary(false);
    }
    $scope.ReportTypeChange = function () {

        //if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = "0"; }
        //if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = "0"; }
        //if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = "0"; }

        //$scope.search.PLDB_ID = "0";
        //$scope.search.PLDB_Branch_ID = "0"; 
        $scope.GetPldbCallSummary(false);
    }
    $scope.PldbBranchNameChange = function (item) {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        $scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;
        $scope.PLDBBranchList();
        $scope.search.PLDB_Branch_ID = item.BranchId;
        
        $scope.GetPldbCallSummary(false);

        
    }
    
    $scope.BackToPage = function () {
        window.location.reload();

    }

    $scope.downloadFile = function (fileUrl) {
        const fileName = fileUrl.split('/').pop();
        $http({
            url: fileUrl,
            method: 'GET',
            responseType: 'blob'
        }).then(function (response) {
            var blob = new Blob([response.data], { type: response.headers('Content-Type') });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }).catch(function (error) {
            console.error('Error downloading file:', error);
        });
    };
    $scope.OnPageLoad();

});
MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
