var MasterMod = angular.module('ApplicationFlowSummerLoanWiseModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationSummerLoanWiseFlowCont", function ($scope, $rootScope, $http) {
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
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.PendingLevelList = [];
    $scope.MLPendingLevelList = [];
    $scope.IsPldbFreezed = false;


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
        $scope.PLDB.PldbId = "0";
        $scope.PLDB.BranchId = "0";
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";
        $scope.StatusId = "";
        $scope.q = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.IsPldbFreezed = false;
        $scope.IsPldbBranchFreezed = false; 

        $scope.PLDBLoanApplicationSummerWiseList();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.PldbId).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                var districtid = $('#hdnDistrictId').val();
                var LoginPldbBranchId = $('#hdnPldbBranchId').val();

                if (LoginPldbBranchId && LoginPldbBranchId != '0' && districtid) {
                    $scope.PLDB.BranchId = LoginPldbBranchId.toString();
                    $scope.PLDB.PLDBBranchList = LoginPldbBranchId.toString();
                    $scope.IsPldbBranchFreezed = true;

                } else {
                    $scope.PLDB.BranchId = branch_ID ? branch_ID.toString() : '0';
                
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.PLDBList = [];
    $scope.LoadMaster = function () {
        return $http.get(SiteUrl + "/pldb/LoadMaster")
            .then(function (response) {
                debugger
                $scope.PLDBList = JSON.parse(response.data.DataList).PLDB || [];

                var districtid = $('#hdnDistrictId').val();
                var LoginPldbId = $('#hdnPldbId').val();

                if (LoginPldbId && LoginPldbId !== '0' && districtid) {

                    // wait till dropdown options are ready
                    $scope.$evalAsync(function () {

                        $scope.PLDB = $scope.PLDB || {};
                        $scope.PLDB.PldbId = LoginPldbId.toString();
                        $scope.IsPldbFreezed = true;

                        // load branches AFTER PLDB is set
                        $scope.PLDBBranchList('0');
                    });
                }
            }, function () {
                $scope.PLDBList = [];
            });
    };

    $scope.LoadMaster();

    //Bhavesh

    $scope.pldbSummaryMap = {};
    angular.forEach($scope.data, function (row) {

        if (!$scope.pldbSummaryMap[row.PLDBName]) {
            $scope.pldbSummaryMap[row.PLDBName] = {
                PLDBName: row.PLDBName,
                CROP_LOAN_COUNT: 0,
                CROP_LOAN_AMOUNT: 0,
                DIV_1_COUNT: 0,
                DIV_1_AMOUNT: 0,
                FMS_COUNT: 0,
                FMS_AMOUNT: 0,
                MORTGAGE_LOAN_COUNT: 0,
                MORTGAGE_LOAN_AMOUNT: 0
            };
        }

        let obj = $scope.pldbSummaryMap[row.PLDBName];

        /* ===== Crop Loan ===== */
        if (row.HeaderName === 'Crop Loan') {
            obj.CROP_LOAN_COUNT += row.NoCount;
            obj.CROP_LOAN_AMOUNT += row.TotalAmount;
        }

        /* ===== DIV 1 ===== */
        if (row.sector_name_en === 'DIV. 1') {
            obj.DIV_1_COUNT += row.NoCount;
            obj.DIV_1_AMOUNT += row.TotalAmount;
        }

        /* ===== FMS ===== */
        if (row.sector_name_en === 'F.M.S.') {
            obj.FMS_COUNT += row.NoCount;
            obj.FMS_AMOUNT += row.TotalAmount;
        }

        /* ===== Mortgage Loan ===== */
        if (row.HeaderName === 'Mortagage Loan') {
            obj.MORTGAGE_LOAN_COUNT += row.NoCount;
            obj.MORTGAGE_LOAN_AMOUNT += row.TotalAmount;
        }
    });

    /* Convert map → array for ng-repeat */
    $scope.pldbSummaryList = Object.values($scope.pldbSummaryMap);


           /* ----------- LOAD SUMMARY LIST ----------- */

    $scope.PLDBLoanApplicationSummerWiseList = function () {
        debugger
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = {
            TotalRecords: $scope.DataPaging.TotalRecords,
            PageNumber: $scope.DataPaging.currentPageID,
            PageSize: $scope.pageSize,
            SortingColumn: $scope.DataPaging.sortExpression,
            SortingOrder: $scope.DataPaging.sortDirection,
            SearchParameter: $scope.DataPaging.searchParameters
        };

        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationReportSUMMERYWISE", JSON.stringify({ Details: jsonParameters })).then(function (response)
        {
            debugger
            var data = response.data;
            if (!data.status) {
                alert(data.message);
                $(".se-pre-con").fadeOut("slow");
                return;
            }
            $scope.rawData = data.data || [];

            /* ---------------- HEADER LIST ---------------- */

            $scope.HeaderList = [];

            if ($scope.rawData.length > 0) {
                angular.forEach(Object.keys($scope.rawData[0]), function (key) {
                    if (key !== 'PLDBName' && key.toLowerCase().endsWith('_cnt')) {
                        $scope.HeaderList.push(key.replace(/_cnt/i, ''));
                    }
                });
            }

        buildPurposeLookup();
            $(".se-pre-con").fadeOut("slow");
            }).catch(function () {
            $(".se-pre-con").fadeOut("slow");
            });
    };


    /* ---------- PURPOSE LOOKUP MAP ------------ */

    $scope.PurposeLookup = {};

    function buildPurposeLookup() {
        $scope.PurposeLookup = {};
        if (!$scope.rawData || !$scope.rawData.length) return;
        $scope.rawData.forEach(function (row) {
            if (!row.PldbId || !row.PLDB_OTS_LoanPurposeID || !row.PurposeNameEN) return;
            var purposeKey = normalizeHeaderKey(row.PurposeNameEN);
            if (!purposeKey) return;
            var mapKey = row.PldbId + '|' + purposeKey;
            if (!$scope.PurposeLookup[mapKey]) {
                $scope.PurposeLookup[mapKey] = row.PLDB_OTS_LoanPurposeID;
            }
        });
    }

    function normalizeHeaderKey(name) {
        return (name || '').toLowerCase().trim().replace(/\s+/g, '_').replace(/[\/&]/g, '_');
    }


    /*---------------- CLICK HANDLER ---------------- */

    $scope.openPurposeDetails = function (item, header) {
        if (!item || !header) return;
        var purposeId = item[header + '_PurposeId'];
        if (!purposeId) return;
        var params = {
            PldbId: item.PldbId,
            PurposeId: purposeId
        };
        sessionStorage.setItem("LoanPurposeDetailParams",JSON.stringify(params)
        );

        window.location.href = SiteUrl + "MIS/PLBDLoanApplicationSummeryWiseDetailList";
    };

    $scope.exportExcel = function () {
        var oldPageSize = $scope.pageSize;
        var oldPageNumber = $scope.DataPaging.currentPageID;
        $scope.pageSize = 100000;
        $scope.DataPaging.currentPageID = 1;
        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = {
                                TotalRecords: 0,
                                PageNumber: 1,
                                PageSize: $scope.pageSize,
                                SortingColumn: $scope.DataPaging.sortExpression,
                                SortingOrder: $scope.DataPaging.sortDirection,
                                SearchParameter: $scope.DataPaging.searchParameters
                              };

        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationReportSUMMERYWISE",JSON.stringify({ Details: jsonParameters })).then(function (response){
                    var res = response.data;
                    if (!res.status || !res.data || !res.data.length) {
                    alert("No data to export");
                    return;
                    }

            var list = res.data;

            /* -------- DETECT BASE HEADERS -------- */

            var headerSet = {};
            angular.forEach(Object.keys(list[0]), function (key) {
                if (/(?:_cnt|_count|_amt|_amount)$/i.test(key)) {
                    var base = key.replace(/_(cnt|count|amt|amount)$/i, '');
                    headerSet[base] = true;
                }
            });

            var headersBase = Object.keys(headerSet);

            /* -------- Force GRAND TOTAL -------- */
            if (headersBase.indexOf('GRAND_TOTAL') === -1) {
                headersBase.push('GRAND_TOTAL');
            }

            /* -------- BUILD CSV HEADER -------- */

            var csv = '';
            var headers = ['PLDB Name'];

            angular.forEach(headersBase, function (h) {
                headers.push(h + ' Count');
                headers.push(h + ' Amount');
            });

            csv += headers.join(',') + '\n';

            /* -------- BUILD ROWS -------- */

            angular.forEach(list, function (row) {
                    var line = [];
                    line.push(row.PLDBName || '');
                    angular.forEach(headersBase, function (h) {
                    var countVal = row[h + '_Cnt'] ?? row[h + '_Count'] ?? 0;
                    var amountVal =row[h + '_Amt'] ?? row[h + '_Amount'] ?? 0;
                    line.push(countVal);
                    line.push(amountVal);
                });
                csv += line.join(',') + '\n';
            });

            /* -------- DOWNLOAD -------- */

            var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "PLDB_Loan_Summary_Wise_Report.csv";
            link.click();
        }).finally(function () {
            $scope.pageSize = oldPageSize;
            $scope.DataPaging.currentPageID = oldPageNumber;
            $(".se-pre-con").fadeOut("slow");
        });
    };


    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

        $scope.showLoader = false;
        $scope.IsDistrictFreezed = false;
        $scope.PLDBLoanApplicationSummerWiseList();
    };

    $scope.OnPageLoad();

});
