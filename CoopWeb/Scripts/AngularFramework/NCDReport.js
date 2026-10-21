
var MasterMod = angular.module('ncdReportMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdReportCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.functionStatusList = [];
    $scope.districtWiseSociety = [];
    $scope.SectorWiseDistrictList = [];
    $scope.NCDDistrictDetails = [];
    $scope.activeTab = "sectorTab";
    $scope.NCDSectorDetails = [];

    $scope.search = {
        statecode: 8,
        districtcode: "0",
        isNoRecord: false,
        districtId: $("#hdnDistrictId").val(),
        functionalstatus: "0",
        colspan: 4,
        fromDate: null,
        toDate: null
    }

    //if ($("#hdnDistrictId").val() != null && $("#hdnDistrictId").val() != "0") {
    //    $scope.search.districtId = $("#hdnDistrictId").val();
    //}

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get("/Translations/Translation_HI.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }


        if ($scope.search.districtId == 0) {
            $scope.GetSectorWiseData();
            //$scope.GetDistrictWiseSocietyData();
            //$scope.GetSectorWiseSocietyDetail();
        }

        $scope.GetSocietyFunctionalStatus();
    };

    $scope.setActiveTab = function (tabName) {
        if (tabName === "sectorTab") {
            $scope.search.districtcode = ($scope.search.districtcode == "0") ? "0" : $scope.search.districtcode;
            $scope.search.functionalstatus = "0";
            $scope.search.fromDate = null;
            $scope.search.toDate = null;
            $scope.activeTab = tabName;
            $scope.GetSectorWiseData();
        } else if (tabName === "districtTab") {
            $scope.search.districtcode = ($scope.search.districtcode == "0") ? "0" : $scope.search.districtcode;
            $scope.search.functionalstatus = "0";
            $scope.search.fromDate = null;
            $scope.search.toDate = null;
            $scope.activeTab = tabName;
            $scope.GetDistrictWiseSocietyData();
        } else if (tabName === "sectorMembersTab") {
            $scope.search.districtcode = ($scope.search.districtcode == "0") ? "0" : $scope.search.districtcode;
            $scope.search.functionalstatus = "0";
            $scope.search.fromDate = null;
            $scope.search.toDate = null;
            $scope.activeTab = tabName;
            $scope.GetSectorWiseSocietyDetail();
        }
    };

    $scope.searchFilter = function () {
        if ($scope.activeTab === "sectorTab") {
            $scope.GetSectorWiseData();
        } else if ($scope.activeTab === "districtTab") {
            $scope.GetDistrictWiseSocietyData();
        } else if ($scope.activeTab === "sectorMembersTab") {
            $scope.GetSectorWiseSocietyDetail();
        }
    };

    // Clear Filters
    $scope.clearSearch = function () {
        $scope.sectorWiseDataList = [];
        $scope.districtWiseSociety = [];
        $scope.SectorWiseDistrictList = [];
    };

    $scope.GetDistrictByStatecode = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                    $scope.NCDDistrictDetails = JSON.parse(data.Result.Data.NCDDistrictDetail);
                    if ($("#hdnDistrictId").val() != "0") {
                        $scope.search.districtcode = $scope.NCDDistrictDetails.filter(x => x.DistrictId == $("#hdnDistrictId").val())[0].district_code;
                        $("#districtcode").attr("disabled", "disabled");
                        $scope.GetSectorWiseData();
                        $scope.GetDistrictWiseSocietyData();
                        $scope.GetSectorWiseSocietyDetail();
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetSocietyFunctionalStatus = function () {
        $scope.functionStatusList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSocietyFunctionalStatus")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.SocietyFunctionalStatusDetail) {
                        $scope.functionStatusList = JSON.parse(response.data.Result.Data.SocietyFunctionalStatusDetail);
                    } else { $scope.functionStatusList = []; }
                } else {
                    console.error("Error fetching functional status:", response.data && response.data.Result ?
                        response.data.Result.Message : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching functional status:", error);
            });
    };
    $scope.GetSectorWiseData = function () {
        $http.get(SiteUrl + "/NcdData/GetSectorDashboard?fromDate=" + $scope.convertDateFormat($scope.search.fromDate) + "&toDate=" + $scope.convertDateFormat($scope.search.toDate) + "&districtId=" + ($scope.search.districtcode ?? 0) + "&functionalStatusId=" + ($scope.search.functionalstatus ?? 0)).success(function (response) {
            if (response.Result.Status == "200") {
                if (response.Result.Data.SectorDashboard != null) {
                    $scope.sectorWiseDataList = JSON.parse(response.Result.Data.SectorDashboard);
                    if ($scope.sectorWiseDataList.length == 0) {
                        $scope.search.isNoRecord = true;
                        $scope.search.colspan = 4;
                    } else { $scope.search.isNoRecord = false; }
                }

            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };

    $scope.GetDistrictWiseSocietyData = function () {
        $http.get(SiteUrl + "/NcdData/GetDistrictDashboard?fromDate=" + $scope.convertDateFormat($scope.search.fromDate) + "&toDate=" + $scope.convertDateFormat($scope.search.toDate) + "&districtId=" + ($scope.search.districtcode ?? 0) + "&functionalStatusId=" + ($scope.search.functionalstatus ?? 0)).success(function (response) {
            if (response.Result.Status == "200" && response.Result.Data.DistrictDashboard != null) {
                $scope.districtWiseSociety = JSON.parse(response.Result.Data.DistrictDashboard);
                if ($scope.districtWiseSociety.length == 0) {
                    $scope.search.isNoRecord = true;
                    $scope.search.colspan = 4;
                } else { $scope.search.isNoRecord = false; }
            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };

    $scope.GetSectorWiseSocietyDetail = function () {
        $http.get(SiteUrl + "/NcdData/GetSocietyMemberDashboard?fromDate=" + $scope.convertDateFormat($scope.search.fromDate) + "&toDate=" + $scope.convertDateFormat($scope.search.toDate) + "&districtId=" + ($scope.search.districtcode ?? 0) + "&functionalStatusId=" + ($scope.search.functionalstatus ?? 0)).success(function (response) {
            if (response.Result && response.Result.Status === "200") {
                $scope.SectorWiseDistrictList = JSON.parse(response.Result.Data.SectorWiseDistrictDashboard);
                if ($scope.SectorWiseDistrictList.length > 0) {
                    $scope.TableColumns = Object.keys($scope.SectorWiseDistrictList[0]);
                }
            } else {
                ShowOkMessage("Error", response.data?.Result?.Message || "No records found.");
            }
        }).catch(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };

    $scope.resetSearch = function () {
        $scope.search.fromDate = null;
        $scope.search.toDate = null;
        $scope.search.districtcode = "0";
        $scope.search.functionalstatus = "0";
        $scope.GetSectorWiseData();
        $scope.GetSectorWiseSocietyDetail();
        $scope.GetDistrictWiseSocietyData();
    };

    $scope.convertDateFormat = function (date) {
        if (!date) {
            return "";
        }
        var d = new Date(date);
        var year = d.getFullYear();
        var month = String(d.getMonth() + 1).padStart(2, '0');
        var day = String(d.getDate()).padStart(2, '0');
        return d.toISOString();
        // return year + '-' + month + '-' + day;
    };

    $scope.GetDistrictByStatecode();
    $scope.OnPageLoad();
});