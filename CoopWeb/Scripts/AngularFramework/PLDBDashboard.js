var MasterMod = angular.module('pldbMod', ['zingchart-angularjs', 'angularUtils.directives.dirPagination']);
MasterMod.controller("pldbCtrl", function ($scope, $http) {

    $scope._language = 'en-us';

    $scope.RegistrationModel = [];
    $scope.AccountModel = [];
    $scope.RecoveryPaymentStatistics = [];
    $scope.search = 0;
    $scope.isNoRecord = false;
    $scope.isColSpan = 0;

    $scope.GetAlldeshdata = function () {
        // alert(SiteUrl + "/Election/GetElectionDashboard");
        return $http.get(SiteUrl + "/pldb/GetPLDBDashboard").
            success(function (data1, status) {
                var data = JSON.parse(data1.DataList);
                $scope.RegistrationModel = [];
                $scope.RegistrationModel = data.Table.filter(x => x.Type === "1");
                $scope.AccountModel = data.Table.filter(x => x.Type === "2");
                $scope.PendingLevelList = data.Table1;

                return true;
            }).
            error(function (data1, status) {

                return true;

            });
        // return true;
    }

    //Added By Narpat Kumawat on 30-05-2025 GetRecoveryPaymentStatistics
    $scope.GetRecoveryPaymentStatistics = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.RecoveryPaymentStatistics = [];
        $http.get(SiteUrl + "/pldb/GetPldbRecoveryPaymentStatistics").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.PaymentRecoveryData != null) {
                    $scope.RecoveryPaymentStatistics = JSON.parse(data.Result.Data.PaymentRecoveryData);

                    if ($scope.RecoveryPaymentStatistics != null && $scope.RecoveryPaymentStatistics.length > 0) {
                        $scope.isNoRecord = false;
                        $scope.search = $scope.RecoveryPaymentStatistics[0].PLDBBranch ?? null;
                        if ($scope.search != null) {
                            $scope.PldbWiseParam = 'branch';
                            $scope.drawVisualizationPldbWise('branch');
                        }
                        else {
                            $scope.PldbWiseParam = 'pldb';
                            $scope.drawVisualizationPldbWise('pldb');
                        }
                    } else {
                        $scope.isNoRecord = true;
                        $scope.isColSpan = 7;
                    }
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.OnPageLoad = function () {
        $(".blockUI").fadeIn("slow");
        google.load("visualization", "1", { packages: ["corechart"] });
        $scope.GetAlldeshdata();
        $scope.GetRecoveryPaymentStatistics();
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

            $scope.ClearUnitForm = function () {
                $scope.deshboaddata = null;
            };
        }
        $(".blockUI").fadeOut("slow");
    };

    $scope.OnPageLoad();

    $scope.ExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        $scope.drawVisualizationPldbWise($scope.PldbWiseParam);
    };

    $scope.drawVisualizationPldbWise = function (TypeAX) {
        var Count = 1;
        var ArryAX = [];
        var title = '';
        $scope.DistrictwiseApexParam = TypeAX;
        var TypeLocalAX = ''
        if (TypeAX == 'pldb') {
            TypeLocalAX = '';
            ArryAX.push(['PLDB', 'Amount Recovered']);
            title = (_language == 'en-us' ? 'Amount Recovered' : 'वसूल की गई राशि');
        } else if (TypeAX == 'branch') {
            TypeLocalAX = '';
            ArryAX.push(['Branch', 'Amount Recovered']);
            title = (_language == 'en-us' ? 'Amount Recovered' : 'वसूल की गई राशि');
        }

        if (TypeAX == 'pldb') {
            angular.forEach($scope.RecoveryPaymentStatistics, function (item) {
                if (item.PLDBName != 'Total')
                    ArryAX.push([item.PLDBName, parseFloat(item.TotalAmount)]);
            });
        }
        else if (TypeAX == 'branch') {
            angular.forEach($scope.RecoveryPaymentStatistics, function (item) {
                if (item.PLDBName != 'Total')
                    ArryAX.push([item.PLDBBranch, parseFloat(item.TotalAmount)]);
            });
        }
        //});
        var data = google.visualization.arrayToDataTable(ArryAX);
        var options = {
            title: (TypeAX == 'pldb' ? 'PLDB Wise Recovery Statistics' : 'PLDB Branch Wise Recovery Statistics'),
            vAxis: { title: title },
            hAxis: {
                title: (TypeAX == 'pldb' ? '(PLDB)' : '(PLDB Branches)'), textStyle: {
                    fontSize: 10
                }
            },
            seriesType: 'bars',
            series: { 5: { type: 'line' } }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById('PldbChart'));
        chart.draw(data, options);
    }

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






