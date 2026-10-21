
var MasterMod = angular.module('BeneficiaryDetailsListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryDetailsListCnt", function ($rootScope, $scope, $location, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divClass = false;
    $scope.BeneficiaryDetailList = [];
    $scope.TotalBeneficiaryDetailList = [];

    const urlParams = new URLSearchParams(window.location.search);
    $scope.requestType = urlParams.get('type');
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }

    //$scope.DataPaging.PageSize = $scope.obj.pageSize ?? 50;

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.obj.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetBeneficiaryList(false);
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
                $scope.GetBeneficiaryList( false);
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
                callback(false);
            }
            ++indx;
        });
    }

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
        $scope.GetDistrictList();
    };
        
    $scope.exportExcel = function () {
        $scope.GetBeneficiaryList(true);
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
    $scope.GetDistrictList = function () {
        $scope.hdnDistId = $('#hdnDistrictId').val();
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                if ($scope.hdnDistId > 0) {
                    $scope.DistrictList = data.filter(x => x.DISTRICT_ID == $scope.hdnDistId);
                    $scope.selectedDistrictId = data.filter(x => x.DISTRICT_ID == $scope.hdnDistId)[0].DISTRICT_ID;
                    $scope.search.districtId = $scope.selectedDistrictId;
                    $scope.GetSubDistrictList();                   
                }
                else {
                    $scope.DistrictList = data;
                    $scope.search.subDistrictName = '';
                    $scope.search.villageName = '';
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
   
    $scope.GetSubDistrictList = function () {
        $scope.search.subDistrictName = '';
        $scope.search.VillageName = '';
        $scope.GetSetSearchParameter("SubDistrictName", "");
        $scope.GetSetSearchParameter("VillageName", "");
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CM_SUB_DISTRICT&MasterCode=" + $scope.search.districtId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.SubDistrictList = data;                
                
                $scope.searchRequest($scope.GetBeneficiaryList);
                $scope.$apply();
                //$scope.GetBeneficiaryList(false);
            }).
            error(function (data, status) {
                $scope.SubDistrictList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetVillageList = function () {
        $scope.search.VillageName = '';
        $scope.GetSetSearchParameter("VillageName", "");
        // $scope.GetBeneficiaryList();
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CM_VILLAGE&MasterCode=" + $scope.search.districtId + "&MasterCode2=" + $scope.search.subDistrictName).
            success(function (data, status) {
                $scope.VillageList = data;
                $scope.searchRequest($scope.GetBeneficiaryList);
                $scope.$apply();
                //$scope.GetBeneficiaryList(false);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.VillageList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetSetSearchParameter = function (key, keyValue)
    {
        var index = $scope.DataPaging.searchParameters.findIndex(function (p) { return p.Key === key; });

        if (index !== -1) {
            // If found, replace the value
            $scope.DataPaging.searchParameters[index].Value = keyValue;
        }
        else {
            var parameter = { Key: key, Value: keyValue }
            $scope.DataPaging.searchParameters.push(parameter);
        }

    }
    $scope.GetBeneficiaryList = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
       
        $scope.GetSetSearchParameter("Action", "CmKisanList");

        if (!isExport)
            $rootScope.DataPagingDetails = {};

        var Exportjson = "";
        if (isExport) {
            Exportjson = {
                "RemoveColumns": ["TotalRecords"],
                "RenameColumns": {
                    "FarmerName": "Farmer Name",
                    "FatherName": "Father Name",
                    "MobileNo": "Mobile No",
                    "VillageName": "Village Name",
                    "SubDistrictName": "Sub District Name",
                    "DistrictName": "District Name",
                },
                "ReportName": "Farmer Data"
            }
        }

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/MIS/GccLoanReports", JSON.stringify({ Details: jsonParameters, Exportjson: Exportjson, Action: "CMBeneficiaryList" })).
            success(function (responcedata, status) {
                if (isExport) {
                    $scope.downloadFile(responcedata.filename);
                }
                else {
                    if (responcedata.DataList != null && responcedata.DataList.length > 0) {

                        var data = JSON.parse(responcedata.DataList);
                        $scope.BeneficiaryDetailList = data; // JSON.parse(data);

                        $rootScope.DataPagingDetails = responcedata.DataPaging;
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
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (responcedata, status) {
                // console.log(data);
                $(".se-pre-con").fadeOut("slow");
                $scope.GccApplicationList = [];
                $scope.TotalGccApplicationList = []
            });
    };



    $scope.OnPageLoad();
    $scope.GetBeneficiaryList(urlParams.get('type'), false);

});

