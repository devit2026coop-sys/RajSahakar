
var MasterMod = angular.module('GccApplicationListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("GccApplicationListCnt", function ($rootScope, $scope, $location, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divClass = false;
    $scope.GccApplicationList = [];
    $scope.TotalGccApplicationList = [];

    const urlParams = new URLSearchParams(window.location.search);
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }
    $scope.search = {};
    $scope.search.LoanBankID = '0';
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
        $scope.GetGccApplicationList(false);
    }


    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,date,input:checkbox').length;
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
                $scope.GetGccApplicationList(false);
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
                callback($scope.SocietyCountId_ss, $scope.DistrictId_ss);
            }
            ++indx;
        });
    }

    $scope.levelId = 0;
    $scope.InspectorStatus = false;
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

        $scope.UserDistrictId = $('#hdnDistrictId').val() ?? 0;
     

        if ($scope.UserDistrictId != 0) {
            //  alert($scope.UserDistrictId);          

            $scope.DistrictListData().then(function () {
                $scope.DistrictId = $scope.UserDistrictId.toString(); $('#ddlDistrict').prop('disabled', true);
                $scope.getBlock();
                $scope.GetGccApplicationList(false);
            });


        }
        else { $scope.DistrictListData();  $scope.GetGccApplicationList(false); }

       
    };


    $scope.GetGccApplicationList = function (isExport) {
        $(".se-pre-con").fadeIn("slow");

        var key = "";
        var keyValue = "";
        if ($scope.UserDistrictId != 0) {

            var key = "";
            var keyValue = "";

            if ($scope.UserDistrictId != 0) {
                key = "DistrictId";
             
            }


            var index = $scope.DataPaging.searchParameters.findIndex(function (p) { return p.Key === key; });

            if (index !== -1) {
                // If found, replace the value
                $scope.DataPaging.searchParameters[index].Value = $scope.UserDistrictId;
            }
            else {
                var parameter = { Key: key, Value: $scope.UserDistrictId }
                $scope.DataPaging.searchParameters.push(parameter);
            }


            //$scope.GetGccApplicationList(false);
            //$scope.searchRequest($scope.GetGccApplicationList(false));
        }

        if (!isExport)
            $rootScope.DataPagingDetails = {};

        var Exportjson = "";
        if (isExport) {
            Exportjson = {
                "RemoveColumns": ["TotalRecords",  "InspactorTargetsId"],
                "RenameColumns": {
                    "CCBName": "CCB Name"
                },
                "ReportName": "InspectorList"
            }
        }

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/MIS/GccLoanReports", JSON.stringify({ Details: jsonParameters, Exportjson: Exportjson, Action: "MPacs_InspectorList" })).
            success(function (responcedata, status) {
                console.log(responcedata);
                if (isExport) {
                    $scope.downloadFile(responcedata.filename);
                }
                else {
                    if (responcedata.DataList != null && responcedata.DataList.length > 0) {

                        var data = JSON.parse(responcedata.DataList);
                        $scope.GccApplicationList = data; // JSON.parse(data);

                        $rootScope.DataPagingDetails = responcedata.DataPaging;
                        $rootScope.SetDataPaging();


                        //if ($scope.DataPaging.currentPageID == 1) {
                        //    $scope.TotalRecords = data[0].TotalRecords;
                        //}
                        //$rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                        //$rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                        //$rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                        //$rootScope.SetDataPaging();
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
    $scope.exportExcel = function () {
        $scope.GetGccApplicationList(true);
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


    //document.getElementById('BankID').setAttribute('disabled', 'disabled');

    $scope.DistrictListData = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictLst = data;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DistrictLst = [];
            });
    };
    $scope.getBlock = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.DistrictId).
            success(function (data, status) {
                $scope.BlockDataList = data;
                $scope.BlockId = '0';
                $(".se-pre-con").fadeOut("slow");
                // _callback(Addressgetinfo);
            }).
            error(function (data, status) {
                $scope.BlockDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.getGrampanchyat = function () {

        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.BlockId).success(function (data, status) {
            $scope.GramPancyatList = data;
            $scope.PanchayatId = '0';
            $(".se-pre-con").fadeOut("slow");
        }).
            error(function (data, status) {
                $scope.GramPancyatList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    }


    $scope.InspectorStatus = false;
    $scope.GetInspectorStatusList = function (InspactorTargetsId) {

        $http.post("/Society/GetInspectorStatusDetails", JSON.stringify({ InspactorTargetsId: InspactorTargetsId })).
            success(function (data, status) {

                $scope.GetInspectorStatus = JSON.parse(data.DataList);
                if ($scope.GetInspectorStatus.length > 0) {
                    $scope.InspectorStatus = true;
                }
                else {
                    $scope.InspectorStatus = false;
                }
            }).
            error(function (data, status) {
                $scope.GetInspectorStatus = [];
                alert('Error while Society Election Detail data.');
            });
    }
    

    $scope.backStaushistory = function () {
        $scope.InspectorStatus = false;
    }
    $scope.OnPageLoad();


});

