
var MasterMod = angular.module('ApplicationFlowSectorLoanWiseModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationSectorLoanWiseFlowCont", function ($scope, $rootScope, $http, $window) {
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
    $scope.IsPldbBranchFreezed = false;
    $scope.SectorMap = [];

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
        $scope.PLDBLoanApplicationSectorWiseList();
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
                var levelId = $('#hdnLevelId').val();
                var LoginPldbId = $('#hdnPldbId').val();
                var LoginBranchId = $('#hdnPldbBranchId').val();
                //PLDB Bank Branch
                if (levelId == "4" && LoginPldbId && LoginPldbId != '0' && LoginBranchId && LoginBranchId != '0') {
                    $scope.PLDB.PldbId = LoginPldbId.toString();
                    $scope.IsPldbFreezed = true;
                    $scope.PLDB.BranchId = LoginBranchId.toString();
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
        return $http.get(SiteUrl + "/pldb/LoadMaster").
            success(function (data, status) {
                $scope.PLDBList = JSON.parse(data.DataList).PLDB;       
                var LevelId = $('#hdnLevelId').val();
                var LoginPldbId = $('#hdnPldbId').val();
                var LoginBranchId = $('#hdnPldbBranchId').val();         

                //Pldb Bank
                if (LevelId == '3' && LoginPldbId && LoginPldbId != '0') {
                    var selectedPLDB = $scope.PLDBList.find(x => x.PldbId == LoginPldbId);
                    if (selectedPLDB) {
                        $scope.PLDB.PldbId = selectedPLDB.PldbId.toString();
                        $scope.IsPldbFreezed = true;                        
                        $scope.PLDBBranchList(LoginBranchId && LoginBranchId != '0'? LoginBranchId : '0' );                        
                        if (LoginBranchId && LoginBranchId != '0') {
                            $scope.IsPldbBranchFreezed = true;
                        }
                    }
                }
                //Pldb Branch
                if (LevelId == '4' && LoginPldbId && LoginPldbId != '0') {
                    $scope.PLDB.PldbId = LoginPldbId.toString();
                    $scope.IsPldbFreezed = true; 
                    $scope.PLDBBranchList(LoginPldbId);
                }
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    }

    $scope.LoadMaster();

    $scope.PLDBLoanApplicationSectorWiseList = function () {

        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = {
            TotalRecords: $scope.DataPaging.TotalRecords,
            PageNumber: $scope.DataPaging.currentPageID,
            PageSize: $scope.pageSize,
            SortingColumn: $scope.DataPaging.sortExpression,
            SortingOrder: $scope.DataPaging.sortDirection,
            SearchParameter: $scope.DataPaging.searchParameters
        };

        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationSectorWise",JSON.stringify({ Details: jsonParameters }) )
            .then(function (response) {
                var res = response.data;            
                $scope.HeaderList = res.headers || [];
                $scope.pldbLoanApplicationSectorList = res.data || [];     
                $scope.SectorMap = {};
                angular.forEach($scope.HeaderList, function (h) {
                    if (!h || !h.HeaderName || h.SectorId === null) return;
                    var key = h.HeaderName.trim().replace(/\./g, '').replace(/\s+/g, '_').toUpperCase();
                    $scope.SectorMap[key] = h.SectorId;
                });

                $(".se-pre-con").fadeOut("slow");
            })
            .catch(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //For display Name
    $scope.headerDisplayMap = {
        'ML': 'Mortagage Loan',
        'PL': 'Personal Loan',
        'DIV1': 'Diversified (DIV1)',
        'DIV2': 'Diversified (DIV2)',
        'FMS': 'Farm Mechanization Sector (FMS)',
        'MIS': 'Minor Irrigation (MIS)',
        'NFS': 'Non Farm Sector (NFS)',
        'RH': 'Rural Housing (RH)'
    };

    //Header Display Name
    $scope.getHeaderDisplayName = function (h) {
        if (!h || !h.HeaderName) return '';
        var key = h.HeaderName.toUpperCase().replace(/\./g, '').replace(/\s+/g, '');
        return $scope.headerDisplayMap[key] || h.HeaderName;
    };

    //Count Detail Page
    $scope.openSectorDetails = function (row, sectorKey) {
        if (row.PLDBName === 'TOTAL') return;
        var sectorId = $scope.SectorMap[sectorKey];
        if (!sectorId) {            
            return;
        }
        var params = {
            PldbId: row.PldbId,
            SectorId: sectorId,
            BranchId: ($scope.PLDB && $scope.PLDB.BranchId && $scope.PLDB.BranchId != "0") ? $scope.PLDB.BranchId : 0 
        };

        sessionStorage.setItem("LoanDetailParams", JSON.stringify(params));
        $window.location.href = "/MIS/PLBDLoanApplicationSectorDetailList";
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

        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationSectorWise",JSON.stringify({ Details: jsonParameters })
        ).then(function (response) {
            var res = response.data;
            var headerList = res.headers || [];
            var list = res.data || [];

            if (!list.length) {
                alert("No data to export");
                return;
            }
            var csv = '';

            var headers = ['PLDB Name'];

            angular.forEach(headerList, function (h) {
                headers.push(h.HeaderName + ' No');
                headers.push(h.HeaderName + ' Amt');
            });

            csv += headers.join(',') + '\n';
            angular.forEach(list, function (row) {

                var line = [];
                line.push(row.PLDBName || '');

                angular.forEach(headerList, function (h) {                    
                    var key = h.HeaderName.trim().replace(/\./g, '').replace(/\s+/g, '_');
                    var countKey = key + '_Count';
                    var amountKey = key + '_Amount';

                    line.push(row[countKey] != null ? row[countKey] : 0);
                    line.push(row[amountKey] != null ? row[amountKey] : 0);                    
                });

                csv += line.join(',') + '\n';
            });

            var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "PLDB_Sector_Wise_Report.csv";
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

        var LevelId = parseInt($("#hdnLevelId").val(), 10); 
        // Clear button visibility
        $scope.showClearButton = (LevelId === 1);

        $scope.showLoader = false;
        $scope.IsDistrictFreezed = false;     
        $scope.PLDBLoanApplicationSectorWiseList();       
    };

    $scope.OnPageLoad();

});
