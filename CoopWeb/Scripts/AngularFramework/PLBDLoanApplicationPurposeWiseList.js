
var MasterMod = angular.module('ApplicationFlowPurposeLoanWiseModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationPurposeLoanWiseFlowCont", function ($scope, $rootScope, $http) {
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
    $scope.dynamicCols = [];

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
        $scope.PLDBLoanApplicationPurposeWiseList();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;


    //Pldb Branch
    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");

        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.PldbId).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                var levelId = $('#hdnLevelId').val();
                var LoginPldbId = $('#hdnPldbId').val();
                var LoginBranchId = $('#hdnPldbBranchId').val();  

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

    //Pldb Bank
    $scope.LoadMaster = function () {
        return $http.get(SiteUrl + "/pldb/LoadMaster").
            success(function (data, status) {
                $scope.PLDBList = JSON.parse(data.DataList).PLDB;  
                var levelId = $('#hdnLevelId').val();
                var LoginPldbId = $('#hdnPldbId').val();
                var LoginBranchId = $('#hdnPldbBranchId').val();               

                // Level 3 = PLDB Login 
                if (levelId == "3" && LoginPldbId && LoginPldbId != '0') {

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

                // Level 4 = Branch Login
                if (levelId == "4" && LoginPldbId && LoginPldbId !== '0') {
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

    $scope.PLDBLoanApplicationPurposeWiseList = function () {
        debugger;
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationPurposeWise",JSON.stringify({ Details: jsonParameters }))
            .then(function (response) {
                var data = response.data;         
                $scope.SectorHeaderList = data.headers;
                $scope.pldbLoanApplicationPurposeList = response.data; 
                console.log("$scope.pldbLoanApplicationPurposeList", $scope.pldbLoanApplicationPurposeList);                

                if ($scope.pldbLoanApplicationPurposeList.data.length > 0) {
                    $scope.dynamicCols = Object.keys(
                        $scope.pldbLoanApplicationPurposeList.data[0]).filter(function (k) {
                        return k !== 'PldbId'  && k !== 'PLDBName' && !k.startsWith('false ') && !k.startsWith('true ');
                    });            
                }                 

                $scope.dynamicColumns = [];
                $scope.ColumnKeyMap = {};

                angular.forEach($scope.SectorHeaderList, function (sector) {

                    angular.forEach(sector.Purposes, function (purpose) {             
                      
                        var sectorKey = $scope.normalizeKey(sector.SectorShortName);
                        var purposeKey = $scope.normalizeKey(purpose.PurposeName);

                        var key = sectorKey + '_' + purposeKey;
                     
                        $scope.dynamicColumns.push(key + '_No');
                        $scope.dynamicColumns.push(key + '_Amt');

                       
                        $scope.ColumnKeyMap[key + '_No'] = {
                            SectorId: sector.SectorId,
                            PurposeId: purpose.PurposeId
                        };

                        $scope.ColumnKeyMap[key + '_Amt'] = {
                            SectorId: sector.SectorId,
                            PurposeId: purpose.PurposeId
                        };
                    });
                });

                if (!data.status) {
                    alert(data.message);
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                $scope.pldbLoanApplicationSectorList = data.data;
                $(".se-pre-con").fadeOut("slow");
            })
            .catch(function (error) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.normalizeKey = function (name) {
        if (!name) return '';
        return String(name)
            .trim()
            .replace(/[.&\/()]/g, '')   
            .replace(/\s+/g, '_');  
    };


    $scope.getBaseColumnName = function (col) {
        if (!col) return '';      
        col = col.replace(/_(No|Amt)$/, '');    
        var parts = col.split('_');
        return parts[0];  
    };

    $scope.openCountDetails = function (row, col) {
        var normalizedCol = $scope.normalizeKey(col);
        var map = $scope.ColumnKeyMap[normalizedCol];

        if (!map) {
            console.warn('ColumnKeyMap not found for:', col, normalizedCol);
            return;
        }

        var params = {
            PldbId: row.PldbId,
            SectorId: map.SectorId,
            PurposeId: map.PurposeId
            , BranchId: ($scope.PLDB && $scope.PLDB.BranchId && $scope.PLDB.BranchId != '0') ? $scope.PLDB.BranchId  : 0
        };

        sessionStorage.setItem("LoanDetailParams", JSON.stringify(params));
        window.location.href = "/MIS/PLBDLoanApplicationCountDetailList";
    };



    $scope.exportCSV = function () {
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

        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationPurposeWise", JSON.stringify({ Details: jsonParameters })
        ).then(function (response) {
            var list = response.data.data;
            if (!list || list.length === 0) {
                alert("No data to export");
                return;
            }
            var cols = Object.keys(list[0]).filter(function (c) {
                return c !== 'PldbId'
                    && c !== 'SortOrder'
                    && c !== 'PLDBName';
            });

            var csv = '';            
            csv += ['PLDB Name'].concat(cols).join(',') + '\n';
            
            angular.forEach(list, function (row) {
                var line = [row.PLDBName];   
                cols.forEach(function (c) {
                    line.push(row[c] != null ? row[c] : 0);
                });
                csv += line.join(',') + '\n';
            });
            
            var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "PLDB_Purpose_Wise.csv";
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
        $scope.PLDBLoanApplicationPurposeWiseList();       
    };

    $scope.OnPageLoad();

});
