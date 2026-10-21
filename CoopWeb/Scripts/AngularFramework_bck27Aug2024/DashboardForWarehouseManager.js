var WarehouseManagerDashboardController = angular.module("WarehouseManagerDashboardCnt", ['zingchart-angularjs']);
WarehouseManagerDashboardController.controller("WarehouseManagerDashboardMod", function ($scope, $http) {

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
        $http.post(SiteUrl + "/MIS/GetWarehouseManagerDashboardData").
            success(function (data, status) {
                $scope.DashboardData = data.Table;
                console.log(data);
                var count = 0;

                $scope.DashboardData.forEach(function (i, j) {
                    count += parseInt(i.Quantity, 10);
                });
                
                $scope.TotalWRCount = count;
                var Arry = [];

                if (_language === 'en-us') {
                    Arry.push(['WR Count', 'Commodity']);
                }
                else {
                    Arry.push(['WR संख्या', 'कमोडिटी']);
                }

                if ($scope.DashboardData.length > 0) {
                    angular.forEach($scope.DashboardData, function (item) {
                        Arry.push([_language === 'en-us' ? item.CommodityNameEn : item.CommodityNameHi, parseInt(item.Quantity)]);
                    });
                }
                setTimeout(function () {
                    $scope.drawChartCommodityWiseWRCount(Arry);
                }, 1000);

            }).
            error(function (data1, status) {
                $scope.DashboardData = [];
            });
    };

    $scope.drawChartCommodityWiseWRCount = function (Arry) {
        if (Arry.length > 0) {
            var data = google.visualization.arrayToDataTable(Arry);
            var options = {
                title: _language === 'en-us' ? 'Commodity Wise WR Count' : 'कमोडिटी वार WR संख्या',
                vAxis: { title: _language === 'en-us' ? 'Commodity Wise WR Count' : 'कमोडिटी वार WR संख्या' },
                hAxis: { title: _language === 'en-us' ? '(Commodity)' : '(कमोडिटी)', textStyle: { fontSize: 10 } },
                seriesType: 'bars',
                series: { 5: { type: 'line' } }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('divChart1'));
            chart.draw(data, options);
        }
    };

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.DashBoardData();
    // END : On Page Load
});