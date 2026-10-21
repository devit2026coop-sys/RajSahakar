
var MasterMod = angular.module('financialMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("financialCtrl", function ($rootScope, $scope, $location, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.districtDataList = [];
    $scope.districtData = '';
    $scope.TrunoverDataList = [];
    $scope.isNoRecord = false;
    $scope.colspan = 0;
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
        }
        $scope.getDistrictDetailByState();
        $scope.getFinacialTrunOverDetail(false);
    };
    $scope.getDistrictDetailByState = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                    $scope.districtDataList = JSON.parse(data.Result.Data.NCDDistrictDetail);
                    //if ($("#hdnDistrictId").val() != "0") {
                    //    $scope.search.districtcode = $scope.districtDataList.filter(x => x.district_code == $("#hdnDistrictId").val())[0].district_code;
                    //}
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.getFinacialTrunOverDetail = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
        if ($scope.districtDataList && $scope.districtDataList.length > 0) {
            var districtObj = $scope.districtDataList.find(x => x.district_code == $scope.search.districtcode);
            if (districtObj && districtObj.district_name) {
                $scope.districtData = ', District - ' + districtObj.district_name;
            } else {
                $scope.districtData = '';
            }
        }
        return $http.post(SiteUrl + "/NcdData/GetTrunoverProfitLoseDetails", JSON.stringify({ district_code: ($scope.search.districtcode ?? 0), actionType: "GetFinancialReport", isExport: isExport })).success(function (data, status) {
            if (isExport) {
                $scope.downloadFile(data);
            }
            else {
                if (data != null && data.length > 0) {
                    $scope.TrunoverDataList = data;
                    if ($scope.TrunoverDataList != null && $scope.TrunoverDataList.length > 0) {
                        $scope.isNoRecord = false;
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
                } else {
                    $scope.TrunoverDataList = [];
                    $scope.colspan = 5;
                    $scope.isNoRecord = true;
                }
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TrunoverDataList = [];
        });
    };

    $scope.ClearFilter = function () {
        $scope.search.districtcode = "0";
        $scope.getFinacialTrunOverDetail(false);
    }

    $scope.exportExcel = function () {
        $scope.getFinacialTrunOverDetail(true);
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

