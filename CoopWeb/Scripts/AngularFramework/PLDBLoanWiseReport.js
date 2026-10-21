
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

    const urlParams = new URLSearchParams(window.location.search);


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
        //$scope.PLDBListData();
        $scope.GetPldbDetailList(false);
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

    //$scope.GetDistrictWithoutDivisionList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
    //        success(function (data, status) {
    //            $scope.DistrictList = data;

    //            if ($("#hdnDistrictId").val() != "0") {
    //                $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
    //                $scope.search.PLDB_ID = "1";
    //                $scope.search.PLDB_Branch_ID = "1";                    
    //                $("#DISTRICT_ID").attr("disabled", "disabled");
    //                $scope.PLDBListData();                            
    //            }
                
    //            if ($scope.Islinkcalled == false) { $scope.GetPldbBankWiseSummary(); }           
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictList = [];
    //        });
    //};

    //$scope.BlockListByOffAdd = function (id) {
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.Blocksdata = [];
    //    $scope.GramPanchayats = [];
    //    $scope.Villages = [];
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.search.DISTRICT_ID).
    //        success(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.Blocksdata = data;
    //            $scope.GetPldbDistrictWiseSummary();
    //        }).
    //        error(function (data, status) {
    //            $scope.Blocksdata = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
   


    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;

                //if ($("#hdnPldbId").val() != "0") {
                //    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                //    $scope.PLDBBranchList();
                //    $("#PLDB_ID").attr("disabled", "disabled");
                //}
                //if ($("#hdnPldbBranchId").val() == "0" && $scope.Islinkcalled== false) { $scope.GetPldbDetailList(); }
               
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };

    $scope.PLDBBranches = [];
    //$scope.PLDBBranchList = function (branch_ID) {
    //    $scope.search.PLDB_Branch_ID = "0";
    //    $(".se-pre-con").fadeIn("slow");
    //    return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
    //        success(function (data, status) {
    //            $scope.PLDBBranches = data;
    //            //if ($("#hdnPldbBranchId").val() != "0") {
    //            //    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
    //            //    $("#PLDB_Branch_ID").attr("disabled", "disabled");
    //            //    if($scope.Islinkcalled == false) { $scope.GetPldbDetailList(); }
    //            //}
    //            // $(".se-pre-con").fadeOut("slow");                
    //        }).
    //        error(function (data, status) {
    //            $scope.PLDBBranches = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetPldbBankWiseSummary();
    }
    $scope.exportExcel = function () {
        $scope.GetPldbDetailList(true);
    }
    $scope.GetPldbSummary = [];
    $scope.TotalGetPldbSummary = [];
    $scope.GetPldbDetailList = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
      
        var Case = "";
        var Type = "";
        //var Action = "PldbLoaneeSummary";
        $scope.search.PLDB_ID = urlParams.get('PldbId') ?? 0;
        $scope.search.PLDB_Branch_ID = urlParams.get('BranchId') ?? 0;
        $scope.FinancialYearId = urlParams.get('FyId') ?? 0;
        $scope.ReportType = urlParams.get('ReportType') ?? 0;
        $scope.search.DISTRICT_ID = urlParams.get('DistId') ?? 0;
        Type = urlParams.get('Type') ?? 0;
        
        var Action = "";

        if ($scope.ReportType == '1') { Action = 'PldbDistrictWiseDetail'; }
        else if ($scope.ReportType == '2') { Action = 'PldbBankWiseDetail'; }
        else if ($scope.ReportType == '3') { Action = 'PldbBankWiseTaamilDetail'; }
        else if ($scope.ReportType == '4') { Action = 'PldbBranchWiseTaamilDetail'; }
        else if ($scope.ReportType == '5') { Action = 'TamilReport'; }
        else if ($scope.ReportType == '6') { Action = 'SelfieReport'; }
        else if ($scope.ReportType == '7') { Action = 'AllTamilReport'; }
        else if ($scope.ReportType == '8') { Action = 'AllSelfieReport'; }
        

        if (Type == '1') { Case = 'Normal' }
        else if (Type == '2') { Case = '103' }
        else if (Type == '3') { Case = 'Both' }


        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        $http.post(SiteUrl + "/MIS/GetPldbCallSummary/?financialYearId=" + ($scope.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action + "&StatusType=" + Case + "&isExport=" + isExport, JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (isExport) {
                    $scope.downloadFile(data.filename);
                    return;
                }
                else {
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


   

    $scope.PldbNameChange = function (item)
    {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        //$scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;  
        $scope.GetPldbBankWiseSummary();
    }
    $scope.ReportTypeChange = function () {

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = "0"; }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = "0"; }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = "0"; }

        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0"; 
        $scope.GetPldbBankWiseSummary();
    }
    $scope.PldbBranchNameChange = function (item) {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        //$scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;
       // $scope.PLDBBranchList();
        $scope.search.PLDB_Branch_ID = item.BranchId;
        
        $scope.GetPldbBankWiseSummary();

        
    }
    
    $scope.BackToPage = function () {
        window.location.reload();

    }
    $scope.downloadFile = function (fileUrl) {
       // const fileName = fileUrl.split('/').pop();
        const fileName = "OtsReport.xlsx";
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
