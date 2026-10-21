var MasterMod = angular.module('MPRArticle30ReportMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("MPRArticle30ReportCnt", function ($scope, $rootScope, $http) {
     
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                //if ($("#hdnDistrictId").val() != "0") {
                //    $scope.DISTRICT_ID = $("#hdnDistrictId").val()
                //    $("#ddrDistrictId").attr("disabled", "disabled");
                //} 
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();

    $scope.clean = function () {
        $scope.InYearPendingCase = 0;
        $scope.LastMonthCaseReceived = 0;
        $scope.InMonthCaseReceived = 0;
        $scope.TotalReceived = 0;
        $scope.LastMonthCaseDisposed = 0;
        $scope.InMonthCaseDisposed = 0;
        $scope.TotalDisposed = 0;
        $scope.TotalPending = 0;
    }

    $scope.GetMPRArticle30Report = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle30Report").success(function (data) {
            //console.log(data, "MPRArticle30ReportList");
            $scope.clean();
            $scope.MPRArticle30ReportList = JSON.parse(data.Result.Data.MPRArticle30ReportList);
            $scope.MPRArticle30ReportList.forEach(function (item, indx) {
                $scope.InYearPendingCase += parseInt(item.InYearPendingCase);
                $scope.LastMonthCaseReceived += parseInt(item.LastMonthCaseReceived);
                $scope.InMonthCaseReceived += parseInt(item.InMonthCaseReceived);
                $scope.TotalReceived += parseInt(item.TotalReceived);
                $scope.LastMonthCaseDisposed += parseInt(item.LastMonthCaseDisposed);
                $scope.InMonthCaseDisposed += parseInt(item.InMonthCaseDisposed);
                $scope.TotalDisposed += parseInt(item.TotalDisposed);
                $scope.TotalPending += parseInt(item.TotalPending);
            })
        }).error(function () {
            alert("Error");
        });
    };

    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");
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

            //$scope.ClearUnitForm = function () {
            //    $scope.UnitMaster = null;
            //};
        }
        $scope.GetMPRArticle30Report();
    };

    $scope.OnPageLoad(); 
});
  
