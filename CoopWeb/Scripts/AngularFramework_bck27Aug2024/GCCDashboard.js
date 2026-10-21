var GccDashboardController = angular.module("GccDashboardCnt", ['zingchart-angularjs']);
GccDashboardController.controller("GccDashboardMod", function ($scope, $http) {

    // START : Page load Function

    $scope.OnPageLoad = function () {
        $scope._language = _language;

        if (_language === 'en-us') {
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
    };
    // END : Page load Function

    // START : Get Data List
    $scope.DashBoardData = function () {
    //    $scope.GetGraph();
        $http.post(SiteUrl + "/GopalCreditCard/GetGCCDashboardData").
            success(function (data, status) {
                $scope.DashboardData = data.Table;
                console.log(data);
                var count = 0;
              

             
                setTimeout(function () {
                //    $scope.drawChartCommodityWiseWRCount(Arry);
                }, 1000);

            }).
            error(function (data1, status) {
                $scope.DashboardData = [];
            });
    };

    
    // START : On Page Load
    $scope.OnPageLoad();
    $scope.DashBoardData();
    // END : On Page Load
});