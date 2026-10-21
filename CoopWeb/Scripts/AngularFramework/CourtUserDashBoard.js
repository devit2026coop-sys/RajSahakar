
var MasterMod = angular.module('CourtDmod', ['zingchart-angularjs']);
MasterMod.controller("CourtDCont", function ($scope, $http) {

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
        drawVisualization();
        drawVisualizationConfed();
        Getlwadata();
    }

    function Getlwadata() {
        $http.post(SiteUrl + "/User/Getlwadata").
            success(function (data, status) {
                $scope.Avg_Loan_Waiver_Amount = data.Avg_Loan_Waiver_Amount;
                $scope.OD_TOTAL_InCrores = data.OD_TOTAL_InCrores;
                $scope.NOD_TOTAL_InCrores = data.NOD_TOTAL_InCrores;
                $scope.New_Beneficiaries_2019 = data.New_Beneficiaries_2019;
                $scope.Total_Not_Intrested_Cases = data.Total_Not_Intrested_Cases;
                $scope.Total_Not_Eligible_Cases = data.Total_Not_Eligible_Cases;
                $scope.Total_GRA_Cases_Pending = data.Total_GRA_Cases_Pending;
                $scope.Total_Rechecking_Cases = data.Total_Rechecking_Cases;
                $scope.Total_Cert_Distributed = data.Total_Cert_Distributed;
                $scope.Total_Cert_Issued = data.Total_Cert_Issued;
                $scope.Total_UID_Authentication = data.Total_UID_Authentication;
                $scope.Total_Applications = data.Total_Applications;
                $scope.Total_PACS = data.Total_PACS + '/' + data.TotalActive_PACS;
                $scope.TotalActive_PACS = data.TotalActive_PACS;

                $scope.Total_Branch_Office = data.Total_Branch_Office + '/' + data.TotalActive_Branch_Office;
                $scope.TotalActive_Branch_Office = data.TotalActive_Branch_Office;
                $scope.Total_Head_Office = data.Total_Head_Office + '/' + data.TotalActive_Head_Office
                $scope.TotalActive_Head_Office = data.TotalActive_Head_Office + '/' + data.Total_Head_Office;

            }).
            error(function (data1, status) {

            });
    }

    function drawVisualization() {
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['Bhandar', 'Controlled Business Target', 'Reached', 'Uncontrolled Business Target', 'Reached']);
        else
            Arry.push(['भंडार', 'नियंत्रित व्यापार लक्ष्य', 'प्राप्त लक्ष्य', 'अनियंत्रित व्यवसाय लक्ष्य', 'प्राप्त लक्ष्य']);

        $http.post(SiteUrl + "/EntryForms/BhandarMprDataForDashboard").
            success(function (data, status) {
                angular.forEach(data, function (item) {
                    Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.ControlledBusinessTarget), parseInt(item.Controlledfulfillment), parseInt(item.UNControlledBusinessTarget), parseInt(item.UnControlledfulfillment)]);
                });

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
    function drawVisualizationConfed() {
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['Confed', 'Sale Amount']);
        else
            Arry.push(['भंडार', 'नियंत्रित व्यापार लक्ष्य']);



        $http.get(SiteUrl + "/User/GetConfedData").
            success(function (data, status) {
                angular.forEach(data, function (item) {
                    Arry.push([(item.District), parseInt(item.Amount)]);
                });

                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language == 'en-us' ? 'Rajasthan State Cooperative Consumers Association (Stores)' : 'राजस्थान राज्य सहकारी उपभोक्ता संघ (भंडार )'),
                    vAxis: { title: '' },
                    hAxis: { title: '' },
                    seriesType: 'bars',
                    series: { 3: { type: 'line' } }
                };
                var chart = new google.visualization.ColumnChart(document.getElementById('chartContainerForConfed'));

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
                    DD.APPROVED = ($scope.Deshboard.filter(x => x.StatusId == 26 || x.StatusId == 17)).length;
                    
                    DD.PAYMENTREQUEST = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'PAYMENT REQUEST/APPROVE BY  REGISTRAR')).length;
                    DD.REJECTED = ($scope.Deshboard.filter(x => x.ProceesStatusTextEn == 'REJECTED BY CLERK' || x.ProceesStatusTextEn == 'REJECTED BY REGISTRAR' || x.ProceesStatusTextEn == 'REJECTED (DUE TO PAYMENT)' || x.ProceesStatusTextEn == 'REJECTED DUE TO OBJECTION IS NOT CLEAR BY CITIZEN' || x.ProceesStatusTextEn == 'REJECTED BY INSPECTOR')).length;
                    DD.TOTAL = (DD.PARTIALSUBMITTED + DD.APPROVED + DD.CERTIFICATEISSUED + DD.PAYMENTREQUEST + DD.REJECTED);
                    DD.UNDERPROCESS = ($scope.Deshboard).length;
                    DD.UNDERPROCESS = (DD.UNDERPROCESS - DD.TOTAL)
                    DD.TOTAL = DD.TOTAL + DD.UNDERPROCESS;
                    DD.ACT1958 = ($scope.Deshboard.filter(x => x.ACTID == '1')).length;
                    DD.ACT2001 = ($scope.Deshboard.filter(x => x.ACTID == '3')).length;
                    DD.ACT2005 = ($scope.Deshboard.filter(x => x.ACTID == '7')).length;
                    DashboardDataList.push(DD);


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

                    $scope.TOTAL = DashboardDataList[0].TOTAL;
                    $scope.SUBMITTED = DashboardDataList[0].SUBMITTED;
                    $scope.APPROVED = DashboardDataList[0].APPROVED;
                    $scope.RegisteredSociety = DashboardDataList[0].CERTIFICATEISSUED;
                    $scope.PartialSubmit = DashboardDataList[0].PARTIALSUBMITTED;
                    $scope.PandingforPayment = DashboardDataList[0].PAYMENTREQUEST;
                    $scope.REJECTED = DashboardDataList[0].REJECTED;
                    $scope.UNDERPROCESS = DashboardDataList[0].UNDERPROCESS;

                    $scope.ACT1958 = DashboardDataList[0].ACT1958;
                    $scope.ACT2001 = DashboardDataList[0].ACT2001;
                    $scope.ACT2005 = DashboardDataList[0].ACT2005;
                    $scope.TOTAL = ($scope.ACT1958 + $scope.ACT2001 + $scope.ACT2005);

                    // alert('12');
                    //  console.log(data);
                    $scope.DeshboardBelow = data.Table2;
                    var OnlineTotalAppli = ($scope.DeshboardBelow.filter(x => x.Id == 1));
                    var OnlineRegiAppli = ($scope.DeshboardBelow.filter(x => x.Id == 2));
                    var OfflineTotalAppli = ($scope.DeshboardBelow.filter(x => x.Id == 3));
                    var OfflineRegiAppli = ($scope.DeshboardBelow.filter(x => x.Id == 4));
                    $scope.OnlineTotalAppli = OnlineTotalAppli[0].Data;
                    $scope.OnlineRegiAppli = OnlineRegiAppli[0].Data;
                    $scope.OfflineTotalAppli = OfflineTotalAppli[0].Data;
                    $scope.OfflineRegiAppli = OfflineRegiAppli[0].Data;
                    //  alert($scope.OnlineTotalAppli + "   " + $scope.OnlineRegiAppli + "   " + $scope.OfflineTotalAppli + "   " + $scope.OfflineRegiAppli + "   " )

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
                                text: "TOtalCourtCase"

                            }, {
                                values: [DashboardDataList[0].CERTIFICATEISSUED],
                                text: "TOtalRejCase"

                            }, {
                                values: [DashboardDataList[0].PAYMENTREQUEST],
                                text: "TOtalAppCase"
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
                    Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt(DashboardDataList[0].REJECTED, 10), parseInt(DashboardDataList[0].REJECTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt(DashboardDataList[0].UNDERPROCESS, 10), parseInt(DashboardDataList[0].UNDERPROCESS, 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Case Status Graph' : 'ग्राफ', Responsedata1, 'divChart1');
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

        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.OnPageLoad();

});






