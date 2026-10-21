var FertilizerSupplierDashboardController = angular.module("FertilizerSupplierDashboardCnt", ['zingchart-angularjs']);
FertilizerSupplierDashboardController.controller("FertilizerSupplierDashboardMod", function ($scope, $http) {

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

    $scope.GetGraph = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
    };
    // START : Get Data List
    $scope.DashBoardData = function () {
        $scope.GetGraph();
        $http.post(SiteUrl + "/FertilizerSupply/GetFertilizerSupplierDashboardData").
            success(function (data, status) {
                $scope.DashboardData = data;
                $scope.WarehouseCount = $scope.DashboardData.Table[0].TotalWarehouse;
                $scope.PurchaseCenterCount = $scope.DashboardData.Table1[0].TotalPurchaseCenter;
                $scope.WRCCount = $scope.DashboardData.Table3[0].TotalWRCGenerated;

                var Arry = [];
                var Title = '';

                if (_language === 'en-us') {
                    Arry.push(['District', 'Purchase Center']);
                }
                else {
                    Arry.push(['जिला', 'खरीद केंद्र']);
                }

                if ($scope.DashboardData.Table2.length > 0) {
                    angular.forEach($scope.DashboardData.Table2, function (item) {
                        Arry.push([_language === 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL, parseInt(item.TotalPurchaseCenter)]);
                    });
                }
                setTimeout(function () {
                    $scope.drawChartDistrictWisePurchaseCenter(Arry);
                }, 1000);
                
            }).
            error(function (data1, status) {
                $scope.DashboardData = [];
            });
    };

    $scope.drawChartDistrictWisePurchaseCenter = function (Arry) {
        var data = google.visualization.arrayToDataTable(Arry);
        var options = {
            title: _language === 'en-us' ? 'District Wise Purchase Center' : 'जिला वार खरीद केंद्र',
            vAxis: { title: _language === 'en-us' ? 'District Wise Purchase Center' : 'जिला वार खरीद केंद्र' },
            hAxis: {title: _language === 'en-us' ? '(District)' : '(जिला)', textStyle: {fontSize: 10}},
            seriesType: 'bars',
            series: { 5: { type: 'line' } }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById('divChart1'));
        chart.draw(data, options);
    };

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.DashBoardData();
    // END : On Page Load
});