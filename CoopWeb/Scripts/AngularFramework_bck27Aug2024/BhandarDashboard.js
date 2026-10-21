
var MasterMod = angular.module('BhandarDashboardMod', ['zingchart-angularjs']);
MasterMod.controller("BhandarDashboardCont", function ($scope, $http) {

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
                //var chart = new google.visualization.PieChart(document.getElementById(divId));
                //chart.draw(data, options);
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
              //  var chart = new google.visualization.ColumnChart(document.getElementById(divId));
              //  chart.draw(data, options);
            }

            drawVisualization();
        }
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
    }

    function drawVisualization() {       
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['Year', 'Controlled Business Target', 'Reached', 'Uncontrolled Business Target', 'Reached']);
        else
            Arry.push(['वर्ष', 'नियंत्रित व्यापार लक्ष्य', 'प्राप्त लक्ष्य', 'अनियंत्रित व्यवसाय लक्ष्य', 'प्राप्त लक्ष्य']);

        $http.post(SiteUrl + "/EntryForms/BhandarDashboardData").
            success(function (data, status) {
                angular.forEach(data, function (item) {
                 //   alert((item.FIN_YEAR_NAME) + "         TotControlledBusinessTarget-" + (item.TotControlledBusinessTarget) + "         TotControlledfulfillment-" + (item.TotControlledBusinessfulfillment) + "         TotUNControlledBusinessTarget-" + (item.TotUnControlledBusinessTarget) + "         TotUnControlledfulfillment-" + (item.TotUnControlledBusinessFulfillment))
                    Arry.push([(item.FIN_YEAR_NAME), parseInt(item.TotControlledBusinessTarget), parseInt(item.TotControlledBusinessfulfillment), parseInt(item.TotUnControlledBusinessTarget), parseInt(item.TotUnControlledBusinessFulfillment)]);
                });
               // alert(Arry);
               // alert(JSON.stringify(Arry))
                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language == 'en-us' ? 'Rajasthan State Cooperative Consumers Association (Stores)' : 'राजस्थान राज्य सहकारी उपभोक्ता संघ (भंडार )'),
                    vAxis: { title: '' },
                    hAxis: { title: '' },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('BhandarChart'));

                chart.draw(data, options);
            }).
            error(function (data1, status) {

            });
    }

    $scope.GetAlldeshdata = function () {
        $http.get(SiteUrl + "/Citizen/GetDeshboardData").
            success(function (data, status) {
                //$scope.CaseRegDetailsData = '';
                //$scope.CaseRegDetailsData = data.Table;
                //$scope.CaseRegDetailsData = $scope.CaseRegDetailsData[0];
                //$scope.CaseRegApplntDetailsData = data.Table1;
                try {
                    $scope.Deshboard = data.Table;
                    var DD = new DashboardData();
                    DD.PARTIALSUBMITTED = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'PARTIAL SUBMITTED')).length;
                    DD.SUBMITTED = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'SUBMITTED')).length;
                    DD.CERTIFICATEISSUED = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'CERTIFICATE ISSUED')).length;
                    DD.PAYMENTREQUEST = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'REGISTRAR ISSUED BANK OPENING CERTIFICATE' || x.ProceesStatusTextEn == 'REQUEST FOR PAYMENT BY  REGISTRAR')).length;
                    DD.REJECTED = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'REJECTED BY CLERK' || x.ProceesStatusTextEn == 'REJECTED BY REGISTRAR' || x.ProceesStatusTextEn == 'REJECTED (DUE TO PAYMENT)' || x.ProceesStatusTextEn == 'REJECTED DUE TO OBJECTION IS NOT CLEAR BY CITIZEN' || x.ProceesStatusTextEn == 'REJECTED BY INSPECTOR')).length;
                    //DD.TOTAL = (DD.PARTIALSUBMITTED + DD.SUBMITTED + DD.CERTIFICATEISSUED + DD.PAYMENTREQUEST + DD.REJECTED);
                    DD.APPROVED = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'APPROVED')).length;
                    var AutoClosur = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'AUTO CLOSERE DUE TO TIME LIMIT EXPIRED (NON PAYMENT)' || x.ProceesStatusTextEn == 'AUTO CLOSERE DUE TO TIME LIMIT EXPIRED (NON CLARIFICATION OF OBJECTION)' || x.ProceesStatusTextEn == 'AUTO CLOSURE' || x.ProceesStatusTextEn == 'AUTO CLOSERE DUE TO BANK ACCOUNT DETAILS NOT UPLOAD')).length;


                    DD.UNDERPROCESS = ($scope.Deshboard).length - (DD.APPROVED + DD.REJECTED + DD.CERTIFICATEISSUED + DD.PARTIALSUBMITTED + AutoClosur)

                    DD.TOTAL = (($scope.Deshboard).length - DD.PARTIALSUBMITTED);

                    DD.ACT1958 = ($scope.Deshboard.filter(x => x.ACTID == '1' && x.ProceesStatusTextEn == 'APPROVED')).length;
                    DD.ACT2001 = ($scope.Deshboard.filter(x => x.ACTID == '3' && x.ProceesStatusTextEn == 'APPROVED')).length;
                    DD.ACT2005 = ($scope.Deshboard.filter(x => x.ACTID == '7' && x.ProceesStatusTextEn == 'APPROVED')).length;

                    DashboardDataList.push(DD);

                    $scope.TACT1958 = ($scope.Deshboard.filter(x => x.ACTID == '1').length - ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' && x.ACTID == '1').length));
                    $scope.TACT2001 = ($scope.Deshboard.filter(x => x.ACTID == '3').length - ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' && x.ACTID == '3').length));
                    $scope.TACT2005 = ($scope.Deshboard.filter(x => x.ACTID == '7').length - ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' && x.ACTID == '7').length));

                    $scope.PACT1958 = ($scope.Deshboard.filter(x => x.ACTID == '1' && x.ProceesStatusTextEn == 'PARTIAL SUBMITTED')).length;
                    $scope.PACT2001 = ($scope.Deshboard.filter(x => x.ACTID == '3' && x.ProceesStatusTextEn == 'PARTIAL SUBMITTED')).length;
                    $scope.PACT2005 = ($scope.Deshboard.filter(x => x.ACTID == '7' && x.ProceesStatusTextEn == 'PARTIAL SUBMITTED')).length;

                    $scope.DeshboardBelow = data.Table2;
                    alert(JSON.stringify($scope.DeshboardBelow));
                    $scope.DeshboardLeft = data.Table1;
                    $scope.Society = $scope.DeshboardLeft[0].Society;
                    $scope.SocietyNotApprove = $scope.DeshboardLeft[0].SocietyNotApprove;
                    $scope.SocietyApprove = $scope.DeshboardLeft[0].SocietyApprove;
                    $scope.SocietyInspectionsAppr = $scope.DeshboardLeft[0].SocietyInspectionsAppr;
                    $scope.SocietyAuditAppr = $scope.DeshboardLeft[0].SocietyAuditAppr;
                    $scope.SocietyElectionAppr = $scope.DeshboardLeft[0].SocietyElectionAppr;

                    $scope.SocietyInspectionsNotAppr = parseInt($scope.DeshboardLeft[0].SocietyApprove) - parseInt($scope.DeshboardLeft[0].SocietyInspectionsAppr);
                    $scope.SocietyAuditNotAppr = parseInt($scope.DeshboardLeft[0].SocietyApprove) - parseInt($scope.DeshboardLeft[0].SocietyAuditAppr);
                    $scope.SocietyElectionNotAppr = parseInt($scope.DeshboardLeft[0].SocietyApprove) - parseInt($scope.DeshboardLeft[0].SocietyElectionAppr);

                    $scope.APPROVED = DashboardDataList[0].APPROVED;

                    $scope.TOTAL = DashboardDataList[0].TOTAL;
                    $scope.SUBMITTED = DashboardDataList[0].SUBMITTED;
                    $scope.RegisteredSociety = DashboardDataList[0].CERTIFICATEISSUED;

                    $scope.TotalNoSocieties = DashboardDataList[0].CERTIFICATEISSUED;



                    $scope.PartialSubmit = DashboardDataList[0].PARTIALSUBMITTED;
                    $scope.PandingforPayment = DashboardDataList[0].PAYMENTREQUEST;
                    $scope.REJECTED = DashboardDataList[0].REJECTED;
                    $scope.UNDERPROCESS = DashboardDataList[0].UNDERPROCESS;

                    $scope.ACT1958 = DashboardDataList[0].ACT1958;
                    $scope.ACT2001 = DashboardDataList[0].ACT2001;
                    $scope.ACT2005 = DashboardDataList[0].ACT2005;

                    $scope.TotalNoSocieties = (parseInt($scope.ACT1958) + parseInt($scope.ACT2001) + parseInt($scope.ACT2005));




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
                                values: [DashboardDataList[0].TOTAL],
                                text: "TOTAL"

                            }, {
                                values: [DashboardDataList[0].CERTIFICATEISSUED],
                                text: "REGISTERED SOCIETY"

                            }, {
                                values: [DashboardDataList[0].PAYMENTREQUEST],
                                text: "PANDING FOR PAYMENT"
                            }, {
                                values: [DashboardDataList[0].PARTIALSUBMITTED],
                                text: "PARTIAL SUBMITTED"

                            }, {
                                values: [DashboardDataList[0].REJECTED],
                                text: "REJECTED"

                            }
                            , {
                                values: [DashboardDataList[0].UNDERPROCESS],
                                text: "UNDERPROCESS"

                            }]
                    };

                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt(DashboardDataList[0].SUBMITTED, 10), parseInt(DashboardDataList[0].SUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt(DashboardDataList[0].CERTIFICATEISSUED, 10), parseInt(DashboardDataList[0].CERTIFICATEISSUED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10), parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt(DashboardDataList[0].PAYMENTREQUEST, 10), parseInt(DashboardDataList[0].PAYMENTREQUEST, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt(DashboardDataList[0].REJECTED, 10), parseInt(DashboardDataList[0].REJECTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt(DashboardDataList[0].UNDERPROCESS, 10), parseInt(DashboardDataList[0].UNDERPROCESS, 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart1');
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
       // $scope.GetAlldeshdata();
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






