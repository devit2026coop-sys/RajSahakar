
var KPIMasterMod = angular.module('KPIMasterMod', ['angularUtils.directives.dirPagination']);
KPIMasterMod.controller("KPIMasterCtrl", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divClass = false;
    $scope.KPIDataList = [];
    $scope.KPIDetailsList = [];
    $scope.ValidateDateList = [];
    $scope.ValidateTimeList = [];

    $scope.search = {
        Id: "0",
        KPIRequireFor: "",
        ValidateEntryDate: "",
        ValidateEntryTime: ""
    };
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).error(function (data, status) {
                    alert("Error while translation");
                });

        }
        $scope.GetKPIMasterDetails();
        $scope.GetKPIData();
        // Call Both Functions
        $scope.GetValidateDateList();
        $scope.GetValidateTimeList();

        $(".se-pre-con").fadeOut("slow");

    };

    $scope.GetKPIData = function () {
        $scope.KPIDataList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MasterSettings/GetKpiMasterNameBy?actionName=GetKPIName").success(function (data) {
            if (data.Result.Data && data.Result.Data.kpiData) {
                $scope.KPIDataList = JSON.parse(data.Result.Data.kpiData);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
        });

    };


    $scope.GetKPIMasterDetails = function () {
        $scope.KPIDetailsList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MasterSettings/GetKpiMasterDetails?actionName=GetData").success(function (data) {
            if (data.Result.Data && data.Result.Data.kpiDetails) {
                $scope.KPIDetailsList = JSON.parse(data.Result.Data.kpiDetails);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
            $scope.KPIDetailsList = [];
        });
    };

    $scope.SaveKPIMasterData = function () {
        $http.post(SiteUrl + "/KPI/InsertUpdateKPIValidate", $scope.search).success(function (response) {
            if (response.Result != null) {
                var resultData = JSON.parse(response.Result.Data);
                if (resultData != null && resultData.length > 0) {
                    alert(resultData[0].Message);
                }
                $scope.GetKPIMasterDetails();
                $('#myModal').hide();
                $scope.ClearKPIData();
                location.reload();
            }
            else {
                alert(response.Result != null ? response.Result.Message : "Unable to save KPI details.");
            }
        }).error(function (error) {
            console.error("Error:", error);
            alert("Server error occurred while saving KPI details.");
        });
    };
    $scope.GetValidateDateList = function () {
        $scope.ValidateDateList = [];
        for (var day = 1; day <= 31; day++) {
            $scope.ValidateDateList.push({ Id: day, text: day });
        }
    };
    $scope.GetValidateTimeList = function () {
        $scope.ValidateTimeList = [];
        for (var hour = 0; hour < 24; hour++) {
            $scope.ValidateTimeList.push({
                Id: hour,
                text: (hour < 10 ? "0" : "") + hour + ":00"
            });
        }
    };
    $scope.ClearKPIData = function () {
        $scope.search.Id = "0";
        $scope.search.ValidateEntryDate = "";;
        $scope.search.ValidateEntryTime = "";
    };
    $scope.EditKPI = function (item) {
        item.OriginalData = {
            ValidateEntryDate: item.ValidateEntryDate,
            ValidateEntryTime: item.ValidateEntryTime
        };
        item.ValidateEntryDate = String(item.ValidateEntryDate);
        item.ValidateEntryTime = String(item.ValidateEntryTime);
        item.IsEdit = true;
    };
    $scope.CancelEditKPI = function (item) {
        if (item.OriginalData) {
            item.ValidateEntryDate = item.OriginalData.ValidateEntryDate;
            item.ValidateEntryTime = item.OriginalData.ValidateEntryTime;
        }

        item.IsEdit = false;
    };

    $scope.UpdateKPI = function (item) {
        var modal = {
            Id: item.Id,
            KPIRequireFor: item.KPIName,
            ValidateEntryDate: parseInt(item.ValidateEntryDate, 10),
            ValidateEntryTime: parseInt(item.ValidateEntryTime, 10)
        };

        $http.post(SiteUrl + "/KPI/InsertUpdateKPIValidate", modal).success(function (response) {
            if (response.Result != null && response.Result.Status == 200) {
                var resultData = JSON.parse(response.Result.Data);
                if (resultData != null && resultData.length > 0) {
                    alert(resultData[0].Message);
                }

                item.IsEdit = false;
                delete item.OriginalData;

                $scope.GetKPIMasterDetails();
            }
            else {
                alert(response.Result != null ? response.Result.Message : "Unable to save KPI details.");
            }
        }).error(function (error) {
            console.error("Error:", error);
            alert("Server error occurred while saving KPI details.");
        });
    };

    $scope.OnPageLoad();
});



