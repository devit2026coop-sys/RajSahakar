var MasterMod = angular.module('ApplicationFlowPurposeLoanWiseSectorModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationPurposeLoanWiseSectorFlowCont", function ($scope, $rootScope, $http, $window) {
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
    $scope.GridData = []; 
    $scope.HeaderSectorList = []; 
    $scope.VisibleSectorHeaders = []; 
    $scope.PurposeMap = {};

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
        $scope.SelectedSectorId = "0";
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
        $scope.PLDBLoanApplicationPurposeWiseSectorList();
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
                $scope.PLDB.BranchId = branch_ID ? branch_ID.toString() : '0';
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

            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    }

    $scope.LoadMaster();

    $scope.LoanSectorList = [];
    $scope.LoadSector = function (scheme_id, index) {

        return $http.post(SiteUrl + "/pldb/GetSectorMaster", { "WhereCase": "  and se.is_active=1 " }).
            success(function (data, status) {
                $scope.LoanSectorList = [];
                $scope.LoanSectorList = data;
                if ($scope.LoanSectorList && $scope.LoanSectorList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.Sector != "" && $scope.IsEditable
                    && ($scope.PLDB_OTSSchemeData.SectorId == undefined || $scope.PLDB_OTSSchemeData.SectorId == 0 || $scope.PLDB_OTSSchemeData.SectorId == "0")
                ) {
                    var matchedVillage = $scope.LoanSectorList.find(function (x) {
                        return x.sector_name_en.toLowerCase() == $scope.PLDB_OTSSchemeData.Sector.toLowerCase();
                    });

                    if (matchedVillage) {
                        $scope.PLDB.SectorId = matchedVillage.SectorId;
                        $scope.PLDB.Sector = matchedVillage.Sector;
                    }
                }
                else {
                    //$scope.PLDB.SectorId = $scope.PLDB_OTSSchemeData ? $scope.PLDB_OTSSchemeData.SectorId.toString() : "0";
                   
                }

            }).
            error(function (data, status) {
                $scope.LoanSectorList = [];
                $(".blockUI").fadeOut("slow");
            });
    };

    $scope.PLDBLoanApplicationPurposeWiseSectorList = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationPurposeWiseSector",JSON.stringify({ Details: jsonParameters }))
            .then(function (response) {
                var data = response.data;
                $scope.HeaderList = response.data.headers; 
                $scope.pldbLoanApplicationPurposeWiseSectorList = response.data;      

                if (!data.status) {
                    alert(data.message);
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }

                $scope.pldbLoanApplicationPurposeWiseSectorList = data.data;
                $(".se-pre-con").fadeOut("slow");
            })
            .catch(function (error) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.exportExcel = function () {
        var searchParams = angular.copy($scope.DataPaging.searchParameters) || [];

        if ($scope.SelectedSectorId && $scope.SelectedSectorId != 0) {
            searchParams.push({
                Key: "SECTOR_ID",
                Value: $scope.SelectedSectorId
            });
        }

        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = {
            TotalRecords: 0,
            PageNumber: 1,
            PageSize: 100000,
            SortingColumn: $scope.DataPaging.sortExpression,
            SortingOrder: $scope.DataPaging.sortDirection,
            SearchParameter: searchParams
        };

        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationPurposeWiseSector",  JSON.stringify({ Details: jsonParameters })).then(function (response) {
            var headersMeta = response.data.headers;
            var rawData = response.data.data;

            if (!rawData || !rawData.length) {
                alert("No data to export");
                return;
            }

            var grid = buildGrid(rawData);
            grid = grid.filter(x => x.PLDBName !== 'TOTAL');
            grid = addTotalRow(grid, $scope.HeaderList);

            var csv = '';

            var headers = ['PLDB Name'];
            angular.forEach($scope.HeaderList, function (h) {
                headers.push(h.Text + ' No');
                headers.push(h.Text + ' Amt');
            });
            csv += headers.join(',') + '\n';            
            angular.forEach(grid, function (row) {
                var line = [];
                line.push(row.PLDBName || '');

                angular.forEach($scope.HeaderList, function (h) {
                    line.push(row[h.Key + '_Count'] || 0);
                    line.push(row[h.Key + '_Amount'] || 0);
                });

                csv += line.join(',') + '\n';
            });
            var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "PLDB_Purpose_Wise_Sector_Report.csv";
            link.click();

        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    function buildGrid(data) {
        let grid = {};

        angular.forEach(data, function (row) {

            if (!grid[row.PLDBName]) {
                grid[row.PLDBName] = {
                    PLDBName: row.PLDBName,
                    PldbId: row.PldbId,
                    SectorId: row.SectorId,
                    PurposeId: row.PurposeId
                };
            }

            let headerKey = row.PurposeName.replace(/\s+/g, '_');       
   
            $scope.PurposeMap[headerKey] = row.PurposeId;

            grid[row.PLDBName][headerKey + "_Count"] = row.TotalCount || 0;
            grid[row.PLDBName][headerKey + "_Amount"] = row.TotalAmount || 0;

        });

        return Object.values(grid);
    }

    //Total Count and Total Amount
    function addTotalRow(gridData, headerList) {
        if (!gridData || !gridData.length) return gridData;

        let totalRow = { PLDBName: 'TOTAL' };

        angular.forEach(headerList, function (h) {

            let countKey = h.Key + '_Count';
            let amountKey = h.Key + '_Amount';

            let totalCount = 0;
            let totalAmount = 0;

            angular.forEach(gridData, function (row) {
                totalCount += (row[countKey] || 0);
                totalAmount += (row[amountKey] || 0);
            });

            totalRow[countKey] = totalCount;
            totalRow[amountKey] = totalAmount;
        });

        gridData.push(totalRow);
        return gridData;
    }

    // When Sector change
    $scope.SectorOnChange = function () { 

        $scope.HeaderList = [];
        $scope.HeaderSectorList = '';
        $scope.GridData = [];

        let searchParams = [];

        if ($scope.SelectedSectorId && $scope.SelectedSectorId != 0) {
            searchParams.push({ Key: "SECTOR_ID",Value: $scope.SelectedSectorId });
        }

        let jsonParameters = { SearchParameter: searchParams };

        $(".se-pre-con").fadeIn("slow");

        $http.post( SiteUrl + "/MIS/GetPLBDLoanApplicationPurposeWiseSector", JSON.stringify({ Details: jsonParameters })).then(function (response) {
            let result = response.data;           
            let purposes = response.data.headers[0].Purposes;           
            angular.forEach(purposes, function (p) {
                if (!p) return;
                $scope.HeaderList.push({ Text: p, Key: p.replace(/\s+/g, '_') });
            });

            $scope.HeaderPurposesList = result.headers[0].Purposes;
            $scope.HeaderSectorList = result.headers[0].SectorName;
            let grid = buildGrid(result.data);   
            grid = grid.filter(x => x.PLDBName !== 'TOTAL');           
            $scope.GridData = addTotalRow(grid, $scope.HeaderList);         
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.openCountDetails = function (row, header) {
        var params = {
            PldbId: row.PldbId,
            PldbName: row.PLDBName,
            HeaderKey: header.Key,
            SectorId: row.SectorId,
            PurposeId: $scope.PurposeMap[header.Key]
        };
        sessionStorage.setItem("LoanDetailParams", JSON.stringify(params));
        $window.location.href = "/MIS/PLBDLoanApplicationCountDetailList";
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
        $scope.PLDBLoanApplicationPurposeWiseSectorList(); 
        $scope.LoadSector();
    };

    $scope.OnPageLoad();

});
