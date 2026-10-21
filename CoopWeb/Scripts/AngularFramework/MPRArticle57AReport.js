var MasterMod = angular.module('MPRArticle57AReportMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("MPRArticle57AReportCnt", function ($scope, $rootScope, $http) {
     
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
        $scope.InYearPendingCaseRs = 0;
        $scope.LastMonthCaseReceived = 0;
        $scope.LastMonthCaseReceivedRs = 0;
        $scope.InMonthCaseReceived = 0;
        $scope.InMonthCaseReceivedRs = 0;
        $scope.TotalReceived = 0;
        $scope.TotalReceivedRs = 0;
        $scope.LastMonthCaseDisposed = 0;
        $scope.LastMonthCaseDisposedRs = 0;
        $scope.InMonthCaseDisposed = 0;
        $scope.InMonthCaseDisposedRs = 0;
        $scope.TotalDisposed = 0;
        $scope.TotalDisposedRs = 0;
        $scope.TotalPending = 0;
        $scope.TotalPendingRs = 0;
        $scope.No1YearPending = 0;
        $scope.No1to3YearPending = 0;
        $scope.No3to5YearPending = 0;
        $scope.No5YearPending = 0;
    }

    $scope.GetMPRArticle57AReport = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle57AReport").success(function (data) {
            //console.log(data, "MPRArticle57AReportList");
            $scope.clean();
            $scope.MPRArticle57AReportList = JSON.parse(data.Result.Data.MPRArticle57AReportList);
            $scope.MPRArticle57AReportList.forEach(function (item, indx) {
                $scope.InYearPendingCase += parseInt(item.InYearPendingCase);
                //$scope.InYearPendingCaseRs += parseInt(item.InYearPendingCaseRs);
                $scope.InYearPendingCaseRs = (parseFloat($scope.InYearPendingCaseRs || 0) + parseFloat(item.InYearPendingCaseRs || 0)).toFixed(2);
                $scope.LastMonthCaseReceived += parseInt(item.LastMonthCaseReceived);
                $scope.LastMonthCaseReceivedRs = (parseFloat($scope.LastMonthCaseReceivedRs || 0) + parseFloat(item.LastMonthCaseReceivedRs || 0)).toFixed(2);
                $scope.InMonthCaseReceived += parseInt(item.InMonthCaseReceived);
                $scope.InMonthCaseReceivedRs = (parseFloat($scope.InMonthCaseReceivedRs || 0) + parseFloat(item.InMonthCaseReceivedRs || 0)).toFixed(2);
                $scope.TotalReceived += parseInt(item.TotalReceived);
                $scope.TotalReceivedRs = (parseFloat($scope.TotalReceivedRs || 0) + parseFloat(item.TotalReceivedRs || 0)).toFixed(2);
                $scope.LastMonthCaseDisposed += parseInt(item.LastMonthCaseDisposed);
                $scope.LastMonthCaseDisposedRs = (parseFloat($scope.LastMonthCaseDisposedRs || 0) + parseFloat(item.LastMonthCaseDisposedRs || 0)).toFixed(2);
                $scope.InMonthCaseDisposed += parseInt(item.InMonthCaseDisposed);
                $scope.InMonthCaseDisposedRs = (parseFloat($scope.InMonthCaseDisposedRs || 0) + parseFloat(item.InMonthCaseDisposedRs || 0)).toFixed(2);
                $scope.TotalDisposed += parseInt(item.TotalDisposed);
                $scope.TotalDisposedRs = (parseFloat($scope.TotalDisposedRs || 0) + parseFloat(item.TotalDisposedRs || 0)).toFixed(2);
                $scope.TotalPending += parseInt(item.TotalPending);
                $scope.TotalPendingRs = (parseFloat($scope.TotalPendingRs || 0) + parseFloat(item.TotalPendingRs || 0)).toFixed(2);
                $scope.No1YearPending += parseInt(item.No1YearPending);
                $scope.No1to3YearPending += parseInt(item.No1to3YearPending);
                $scope.No3to5YearPending += parseInt(item.No3to5YearPending);
                $scope.No5YearPending += parseInt(item.No5YearPending);
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
        $scope.GetMPRArticle57AReport();
    };

    $scope.OnPageLoad(); 
});
  
