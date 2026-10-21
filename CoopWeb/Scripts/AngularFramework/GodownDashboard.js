var WarehouseManagerDashboardController = angular.module("DashboardCnt", ['zingchart-angularjs']);
WarehouseManagerDashboardController.controller("DashboardMod", function ($scope, $http) {

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
        $http.post(SiteUrl + "/MIS/GetGodownReportData").
            success(function (data, status) {
                $scope.DashboardData = JSON.parse(data);
                console.log(data);
                var count = 0;

                $scope.DashboardData.forEach(function (i, j) {
                    count += parseInt(i.total_godown, 10);
                });

                $scope.TotalGodowns = count;
                var Arry = [];

                if (_language === 'en-us') {
                    Arry.push(['Godown Count', 'District']);
                }
                else {
                    Arry.push(['गोदाम संख्या', 'ज़िला']);
                }

                if ($scope.DashboardData.length > 0) {
                    angular.forEach($scope.DashboardData, function (item) {
                        Arry.push([_language === 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL, parseInt(item.total_godown)]);
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
                title: _language === 'en-us' ? 'District Wise Godown Count' : 'जिलेवार गोदामों की गणना',
                vAxis: { title: _language === 'en-us' ? 'District Wise Godown Count' : 'जिलेवार गोदामों की गणना' },
                hAxis: { title: _language === 'en-us' ? '(Godown)' : '(गोदाम)', textStyle: { fontSize: 10 } },
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