
var MasterMod = angular.module('RicemDmod', ['zingchart-angularjs']);
MasterMod.controller("RicemDCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    DashboardDataList = new Array();
    function DashboardData() {
        this.PARTIALSUBMITTED = 0,
            this.SUBMITTED = 0,
            this.PARTIALSUBMITTED = 0,
            this.CERTIFICATEISSUED = 0,
            this.PAYMENTREQUEST = 0,
            this.REJECTED = 0,
            this.UNDERPROCESS = 0,
            this.ACT1958 = 0,
            this.ACT2001 = 0,
            this.ACT2005 = 0
        this.TOTAL = 0
    }

    $scope.GetGraph = function (chartType, title, Responsedata, divId) {
        function drawChart() {
            var data = google.visualization.arrayToDataTable(Responsedata);          
            if (chartType == 'pie') {
                var options = { 'title': title, 'width': 360, 'height': 275 };
                var chart = new google.visualization.PieChart(document.getElementById(divId));
                chart.draw(data, options);
            }
            else if (chartType == 'bar') {
                var options = {
                    title: title,
                    width: 600,
                    height: 400,
                    bar: { groupWidth: "40%" },
                    hAxis: { title: "", direction: -1, slantedText: true, slantedTextAngle: 30, slantedTextWrap: true },
                    legend: { position: "none" },
                    annotations: {
                        alwaysOutside: true,
                        textStyle: {
                            bold: true,
                            color: '#000000',
                        },
                    },
                };
                var chart = new google.visualization.ColumnChart(document.getElementById(divId));
                chart.draw(data, options);
            }

        }
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
    }
    

    $scope.GetAlldeshdata = function () {
        $http.post(SiteUrl + "/Trainning/GetTrainningDashBoardData").
            success(function (data, status) {
                try {
                  //  alert("TotTraining=" + data[0].TotTraining + "  TotRequestPending=" + data[0].TotRequestPending + "   TotPendingAtDepart=" + data[0].TotPendingAtDepart + "  TotApproved=" + data[0].TotApproved + "    TotRejected=" + data[0].TotRejected + "    TotCompleted=" + data[0].TotCompleted + "    TotUpcoming=" + data[0].TotUpcoming       );

                    $scope.TotTraining = data[0].TotTraining;
                    $scope.TotRequestPending = data[0].TotRequestPending;
                    $scope.TotPendingAtDepart = data[0].TotPendingAtDepart;
                    $scope.TotApproved = data[0].TotApproved;
                    $scope.TotRejected = data[0].TotRejected;
                    $scope.TotCompleted = data[0].TotCompleted;
                    $scope.TotUpcoming = data[0].TotUpcoming;

                    $scope.myJson = {
                        type: "pie",
                        title: {
                            textAlign: 'center',
                            text: ""
                        },
                        plot: {
                            slice: 50 //to make a donut
                        },
                        series: [
                            {
                                values: [data[0].TotTraining],
                                text: "TotTraining"

                            }, {
                                values: [data[0].TotRequestPending],
                                text: "TotRequestPending"

                            }, {
                                values: [data[0].TotPendingAtDepart],
                                text: "TotPendingAtDepart"
                            }, {
                                values: [data[0].TotApproved],
                                text: "TotApproved"

                            }, {
                                values: [data[0].TotCompleted],
                                text: "TotCompleted"

                            }
                            , {
                                values: [data[0].TotUpcoming],
                                text: "TotUpcoming"

                            }]
                    };

                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    //Responsedata1.push([_language == 'en-us' ? 'TotTraining' : 'अनुमोदित', parseInt(data[0].TotTraining, 10), parseInt(data[0].TotTraining, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'TotRequestPending' : 'अस्वीकृत', parseInt(data[0].TotRequestPending, 10), parseInt(data[0].TotRequestPending, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'Completed Training' : 'पूर्ण प्रशिक्षण', parseInt(data[0].TotCompleted, 10), parseInt(data[0].TotCompleted, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'Upcoming Training' : 'आगामी प्रशिक्षण', parseInt(data[0].TotUpcoming, 10), parseInt(data[0].TotUpcoming, 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Training Graph' : 'ग्राफ', Responsedata1, 'divChart1');
                }
                catch (err) {
                    alert("Error occurred in DeshboardData. " + err.message);
                }



            }).
            error(function (data1, status) {

            });
    }
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetAlldeshdata();
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

   
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.OnPageLoad();

});






