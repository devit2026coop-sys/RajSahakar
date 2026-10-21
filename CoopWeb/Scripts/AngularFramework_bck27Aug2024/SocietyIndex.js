
var MasterMod = angular.module('SocietyIndexmod', ['zingchart-angularjs']);
MasterMod.controller("SocietyIndexCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    DashboardDataList = new Array();
    $scope.AuditorInspection = [];
    $scope.GetActionIdCCSComplaintReport = [];

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
            console.log(data);

            // Optional; add a title and set the width and height of the chart
            if (chartType == 'pie') {
                var options = { 'title': title, 'width': 360, 'height': 265 };
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

        $scope.AuditorInspection["FinancialYear"] = $scope.FinancialYearList[0].FIN_YEAR_MASTR_ID;
        $scope.drawInspectionVisualization($scope.FinancialYearList[0].FIN_YEAR_MASTR_ID);
    }
    $scope.DashboardCountSocietyAuditprogrss = [];
    $scope.GetDashboardCountSocietyAuditprogrss = function () {
        $http.get(SiteUrl + "/User/GetDashboardCountSocietyAuditprogrss?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId).
            success(function (data, status) {
                $scope.DashboardCountSocietyAuditprogrss = data;
            }).
            error(function (data1, status) {

            });
    }
    $scope.GetDashboardCountSocietyAuditprogrss();


    $scope.drawInspectionVisualization = function (FIN_YEAR_ID) {
        var Count = 1;
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['Bhandar', 'Number of Societies', 'Inspection done']); //, 'Number of Societies', 'Inspection done'
        else
            Arry.push(['भंडार', 'सोसाइटी की संख्या', 'निरीक्षण पूर्ण']); //, 'सोसाइटी की संख्या', 'निरीक्षण पूर्ण'

        var DistrictData = [];
        var ActData = [];
        $http.post(SiteUrl + "/Inspection/InspectionDataForDashboard", { "FinancialYearId": FIN_YEAR_ID }).
            success(function (data, status) {

                var StatusDataInspection = data.data2;
                data = data.data1;
               
                angular.forEach(data, function (item) {

                    if (DistrictData.length == 0) {
                        DistrictData.push({ DISTRICT_ENG: item.DISTRICT_ENG, DISTRICT_MANGAL: item.DISTRICT_MANGAL });
                    }
                    else {
                        if (DistrictData.filter(X => X.DISTRICT_ENG == item.DISTRICT_ENG).length == 0) {
                            DistrictData.push({ DISTRICT_ENG: item.DISTRICT_ENG, DISTRICT_MANGAL: item.DISTRICT_MANGAL });
                        }
                    }


                    if (ActData.length == 0) {
                        ActData.push({ ActNameEn: item.ActNameEn, ActNameHi: item.ActNameHi });
                    }
                    else {
                        if (ActData.filter(X => X.ActNameEn == item.ActNameEn).length == 0) {
                            ActData.push({ ActNameEn: item.ActNameEn, ActNameHi: item.ActNameHi });
                        }
                    }

                });

                angular.forEach(DistrictData, function (item) {

                    var SocietyCountData = data.filter(P => P.DISTRICT_ENG == item.DISTRICT_ENG);
                    var _SUM = 0;

                    angular.forEach(SocietyCountData, function (j) {
                        _SUM += parseInt(j.SocietyCount);
                    });

                    var InspectionDone = 0;
                    InspectionDone = StatusDataInspection.filter(V => V.DISTRICT_ENG == item.DISTRICT_ENG)[0].AuditCompleted;
                    Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), _SUM, parseInt(InspectionDone, 10)]);
                });

                var ActWiseData = [];
                var _SUM1 = 0;

                angular.forEach(data, function (j) {
                    _SUM1 += parseInt(j.SocietyCount);
                });

                ActWiseData.push({ ActName: (_language == 'en-us' ? 'Total' : 'कुल'), Count: _SUM1 });

                angular.forEach(ActData, function (item) {

                    var ActCountData = data.filter(P => P.ActNameEn == item.ActNameEn);
                    var _SUM = 0;

                    angular.forEach(ActCountData, function (j) {
                        _SUM += parseInt(j.SocietyCount);
                    });

                    ActWiseData.push({ ActName: (_language == 'en-us' ? item.ActNameEn : item.ActNameHi), Count: _SUM });
                });

                var _NoOfAuditorAppointed = 0;
                var _AuditCompleted = 0;
                var _Remaining = 0;

                angular.forEach(StatusDataInspection, function (j, i) {
                    _NoOfAuditorAppointed += parseInt(j.NoOfAuditorAppointed, 10);
                    _AuditCompleted += parseInt(j.AuditCompleted, 10);
                    _Remaining = parseInt(j.Remaining, 10);
                });

                ActWiseData.push({ ActName: (_language == 'en-us' ? 'Auditor Appointed' : 'लेखा परीक्षक नियुक्त'), Count: _NoOfAuditorAppointed });
                ActWiseData.push({ ActName: (_language == 'en-us' ? 'Audit Completed' : 'ऑडिट पूर्ण '), Count: _AuditCompleted });
                ActWiseData.push({ ActName: (_language == 'en-us' ? 'Audit Remaining' : 'ऑडिट अपूर्ण '), Count: _Remaining });
                ActWiseData.push({ ActName: (_language == 'en-us' ? 'Auditor Not Assigned' : 'लेखा परीक्षक नियुक्ति नहीं'), Count: (ActWiseData[0].Count - (_NoOfAuditorAppointed + _AuditCompleted)) });


                $scope.ActWiseCount = ActWiseData;




                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language == 'en-us' ? 'Inspection Report Zone Wise' : 'निरीक्षण रिपोर्ट क्षेत्रवार'),
                    vAxis: { title: (_language == 'en-us' ? 'No. of Count' : 'गिनती की संख्या') },
                    hAxis: { title: (_language == 'en-us' ? 'District Name' : 'जिला का नाम') },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('InspectionChart'));

                chart.draw(data, options);
            }).
            error(function (data1, status) {

            });
    }
    
    $scope.GetAlldeshdata_Old = function () {
        $http.get(SiteUrl + "/Citizen/GetDeshboardData").
            success(function (data, status) {
                //$scope.CaseRegDetailsData = '';
                //$scope.CaseRegDetailsData = data.Table;
                //$scope.CaseRegDetailsData = $scope.CaseRegDetailsData[0];
                //$scope.CaseRegApplntDetailsData = data.Table1;
                try {
                    $scope.Deshboard = data.Table;
                    var DD = new DashboardData();
                    DD.PARTIALSUBMITTED = ($scope.Deshboard.filter(x => x.StatusId == 1)).length;
                    DD.SUBMITTED = ($scope.Deshboard.filter(x => x.StatusId == 2)).length;
                    DD.CERTIFICATEISSUED = 0;
                    DD.APPROVED = ($scope.Deshboard.filter(x => x.StatusId == 26 || x.StatusId == 17)).length;

                    DD.PAYMENTREQUEST = ($scope.Deshboard.filter(x => x.StatusId == 33)).length;
                    DD.REJECTED = ($scope.Deshboard.filter(x => x.StatusId == 6 || x.StatusId == 13 || x.StatusId == 16 || x.StatusId == 18 || x.StatusId == 21 || x.StatusId == 29 || x.StatusId == 34 || x.StatusId == 1044)).length;
                    DD.TOTAL = (DD.PARTIALSUBMITTED + DD.APPROVED + DD.CERTIFICATEISSUED + DD.PAYMENTREQUEST + DD.REJECTED);
                    DD.UNDERPROCESS = ($scope.Deshboard).length;
                    DD.UNDERPROCESS = (DD.UNDERPROCESS - DD.TOTAL);
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

                    ////////// alert('12');
                    //////////  console.log(data);
                    ////////$scope.DeshboardBelow = data.Table2;
                    ////////var OnlineTotalAppli = ($scope.DeshboardBelow.filter(x => x.Id == 1));
                    ////////var OnlineRegiAppli = ($scope.DeshboardBelow.filter(x => x.Id == 2));
                    ////////var OfflineTotalAppli = ($scope.DeshboardBelow.filter(x => x.Id == 3));
                    ////////var OfflineRegiAppli = ($scope.DeshboardBelow.filter(x => x.Id == 4));
                    ////////$scope.OnlineTotalAppli = OnlineTotalAppli[0].length > 0 ? OnlineTotalAppli[0].Data : 0;
                    ////////$scope.OnlineRegiAppli = OnlineRegiAppli[0].Data.length > 0 ? OnlineRegiAppli[0].Data : 0;
                    ////////$scope.OfflineTotalAppli = OfflineTotalAppli[0].Data.length > 0 ? OfflineTotalAppli[0].Data : 0;
                    ////////$scope.OfflineRegiAppli = OfflineRegiAppli[0].Data.length > 0 ? OfflineRegiAppli[0].Data : 0;
                    //////////  alert($scope.OnlineTotalAppli + "   " + $scope.OnlineRegiAppli + "   " + $scope.OfflineTotalAppli + "   " + $scope.OfflineRegiAppli + "   " )

                    $scope.DeshboardBelow = data.Table2;
                    var OnlineTotalAppli = ($scope.DeshboardBelow.filter(x => x.Value == 'OnlineTotalAppli'));
                    var OnlineRegiAppli = ($scope.DeshboardBelow.filter(x => x.Value == 'OnlineRegiAppli'));
                    var OnlineRejeAppli = ($scope.DeshboardBelow.filter(x => x.Value == 'OnlineRejeAppli'));
                    var OfflineTotalAppli = ($scope.DeshboardBelow.filter(x => x.Value == 'OfflineTotalAppli'));
                    var OfflineRegiAppli = ($scope.DeshboardBelow.filter(x => x.Value == 'OfflineRegiAppli'));
                    var OfflineRejeAppli = ($scope.DeshboardBelow.filter(x => x.Value == 'OfflineRejeAppli'));
                    var OfflineSubmitted = ($scope.DeshboardBelow.filter(x => x.Value == 'OfflineSubmitted'));



                    $scope.OnlineTotalAppli = 0;
                    $scope.OfflineTotalAppli = 0;
                    $scope.OnlineRegiAppli = 0;
                    $scope.OfflineRegiAppli = 0;
                    $scope.OnlineRejeAppli = 0;
                    $scope.OfflineRejeAppli = 0;
                    $scope.OfflineSubmitted = 0;

                    $scope.ONLINETOTALACT1958 = 0;
                    $scope.ONLINETOTALACT2001 = 0;
                    $scope.ONLINETOTALACT2005 = 0;



                    angular.forEach(OnlineTotalAppli, function (item) {
                        $scope.OnlineTotalAppli += item.Data
                        if (item.ActId == 1) {
                            $scope.ONLINETOTALACT1958 += item.Data;
                        }
                        if (item.ActId == 3) {
                            $scope.ONLINETOTALACT2001 += item.Data;
                        }
                        if (item.ActId == 7) {
                            $scope.ONLINETOTALACT2005 += item.Data;
                        }
                    });

                    $scope.OFFLINETOTALACT1958 = 0;
                    $scope.OFFLINETOTALACT2001 = 0;
                    $scope.OFFLINETOTALACT2005 = 0;

                    angular.forEach(OfflineTotalAppli, function (item) {
                        $scope.OfflineTotalAppli += item.Data
                        if (item.ActId == 1) {
                            $scope.OFFLINETOTALACT1958 += item.Data;
                        }
                        if (item.ActId == 3) {
                            $scope.OFFLINETOTALACT2001 += item.Data;
                        }
                        if (item.ActId == 7) {
                            $scope.OFFLINETOTALACT2005 += item.Data;
                        }
                    });

                    $scope.TORSACT1958 = 0;
                    $scope.TORSACT2001 = 0;
                    $scope.TORSACT2005 = 0;

                    angular.forEach(OnlineRegiAppli, function (item) {
                        $scope.OnlineRegiAppli += item.Data;
                        if (item.ActId == 1) {
                            $scope.TORSACT1958 += item.Data;
                        }
                        if (item.ActId == 3) {
                            $scope.TORSACT2001 += item.Data;
                        }
                        if (item.ActId == 7) {
                            $scope.TORSACT2005 += item.Data;
                        }
                    });

                    angular.forEach(OnlineRejeAppli, function (item) {
                        $scope.OnlineRejeAppli += item.Data;
                    });

                    $scope.TOFFRSACT1958 = 0;
                    $scope.TOFFRSACT2001 = 0;
                    $scope.TOFFRSACT2005 = 0;

                    angular.forEach(OfflineRegiAppli, function (item) {
                        $scope.OfflineRegiAppli += item.Data;
                        if (item.ActId == 1) {
                            $scope.TOFFRSACT1958 += item.Data;
                        }
                        if (item.ActId == 3) {
                            $scope.TOFFRSACT2001 += item.Data;
                        }
                        if (item.ActId == 7) {
                            $scope.TOFFRSACT2005 += item.Data;
                        }
                    });

                    angular.forEach(OfflineRejeAppli, function (item) {
                        $scope.OfflineRejeAppli += item.Data;
                    });

                    var Responsedata1 = [];
                    
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt(DashboardDataList[0].SUBMITTED, 10), parseInt(DashboardDataList[0].SUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt(DashboardDataList[0].APPROVED, 10), parseInt(DashboardDataList[0].APPROVED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10), parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt(DashboardDataList[0].PAYMENTREQUEST, 10), parseInt(DashboardDataList[0].PAYMENTREQUEST, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt(DashboardDataList[0].REJECTED, 10), parseInt(DashboardDataList[0].REJECTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt(DashboardDataList[0].UNDERPROCESS, 10), parseInt(DashboardDataList[0].UNDERPROCESS, 10)]);
                    console.log(Responsedata1);
                   

                    $scope.GetGraph('pie', _language == 'en-us' ? 'Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart1');
                }
                catch (err) {                    
                    //alert("Error occurred in DeshboardData. " + err.message);
                }
            }).
            error(function (data1, status) {

            });

        $http.get(SiteUrl + "/Citizen/GetActsOfUser").
            success(function (data, status) {
                try {
                    $scope.ActsOfUser = data;
                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }
            }).
            error(function (data1, status) {

            });
    }
    $scope.GetAlldeshdata = function () {
        $http.get(SiteUrl + "/Citizen/GetDeshboardData_scheduler").
            success(function (data, status) {
                try {
                    $scope.Deshboard = data.Table;

                    $scope.TOTAL = 0;
                    $scope.PARTIALSUBMITTED = 0;
                    $scope.SUBMITTED = 0;
                    $scope.APPROVED = 0;
                    $scope.PAYMENTREQUEST = 0;
                    $scope.REJECTED = 0;
                    $scope.UNDERPROCESS = 0;
                    $scope.ACT1958 = 0;
                    $scope.ACT2001 = 0;
                    $scope.ACT2005 = 0;

                    $scope.SocietyInspectionsAppr = 0;
                    $scope.SocietyAuditAppr = 0;
                    $scope.SocietyElectionAppr = 0;

                    $scope.OfflineTotalAppli = 0;
                    $scope.OfflineRegiAppli = 0;
                    $scope.OfflineRejeAppli = 0;
                    $scope.OfflinePartialAppli = 0;
                    $scope.OfflineUnderProAppli = 0;
                    $scope.OnlineTotalAppli = 0;
                    $scope.OnlineRegiAppli = 0;
                    $scope.OnlineRejeAppli = 0;
                    $scope.OnlinePartialAppli = 0;
                    $scope.OnlineUnderProAppli = 0;
                    $scope.ACT1958 = 0;
                    $scope.ACT2001 = 0;
                    $scope.ACT2005 = 0;


                    angular.forEach($scope.Deshboard, function (item) {
                        $scope.TOTAL += item.TotSociety;
                        $scope.PARTIALSUBMITTED += item.TotPartialSave;
                        $scope.SUBMITTED += item.TotPermanentlSave;
                        $scope.APPROVED += item.TotApproved;
                        $scope.PAYMENTREQUEST += item.TotPaymentRequest;
                        $scope.REJECTED += item.TotRejected;
                        if (item.ActId == 1) {
                            $scope.ACT1958 += item.TotSociety;
                        }
                        if (item.ActId == 3) {
                            $scope.ACT2001 += item.TotSociety;
                        }
                        if (item.ActId == 7) {
                            $scope.ACT2005 += item.TotSociety;
                        }
                        $scope.SocietyInspectionsAppr += item.SocietyInspectionsAppr;
                        $scope.SocietyAuditAppr += item.SocietyAuditAppr;
                        $scope.SocietyElectionAppr += item.SocietyElectionAppr;

                        $scope.OfflineTotalAppli += item.OfflineTotalAppli;
                        $scope.OfflineRegiAppli += item.OfflineRegiAppli;
                        $scope.OfflineRejeAppli += item.OfflineRejeAppli;
                        $scope.OnlineTotalAppli += item.OnlineTotalAppli;
                        $scope.OnlineRegiAppli += item.OnlineRegiAppli;
                        $scope.OnlineRejeAppli += item.OnlineRejeAppli;
                        $scope.OnlinePartialAppli += item.OnlinePartialAppli;
                        $scope.OfflinePartialAppli += item.OfflinePartialAppli;
                    });
                    $scope.UNDERPROCESS = ($scope.TOTAL - ($scope.PARTIALSUBMITTED + $scope.SUBMITTED + $scope.APPROVED + $scope.PAYMENTREQUEST + $scope.REJECTED))
                    $scope.Society = $scope.TOTAL;
                    $scope.SocietyNotApprove = $scope.TOTAL - $scope.APPROVED;
                    $scope.SocietyApprove = $scope.APPROVED;

                    $scope.SocietyInspectionsNotAppr = $scope.APPROVED - $scope.SocietyInspectionsAppr;
                    $scope.SocietyAuditNotAppr = $scope.APPROVED - $scope.SocietyAuditAppr;
                    $scope.SocietyElectionNotAppr = $scope.APPROVED - $scope.SocietyElectionAppr;

                    $scope.OfflineUnderProAppli = $scope.OfflineTotalAppli - ($scope.OfflineRegiAppli + $scope.OfflineRejeAppli + $scope.OfflinePartialAppli);
                    $scope.OnlineUnderProAppli = $scope.OnlineTotalAppli - ($scope.OnlineRegiAppli + $scope.OnlineRejeAppli + $scope.OnlinePartialAppli);

                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt($scope.SUBMITTED, 10), parseInt($scope.SUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt($scope.APPROVED, 10), parseInt($scope.APPROVED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt($scope.PARTIALSUBMITTED, 10), parseInt($scope.PARTIALSUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt($scope.PAYMENTREQUEST, 10), parseInt($scope.PAYMENTREQUEST, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt($scope.REJECTED, 10), parseInt($scope.REJECTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt($scope.UNDERPROCESS, 10), parseInt($scope.UNDERPROCESS, 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart1');
                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }
            }).
            error(function (data1, status) {

            });

        $http.get(SiteUrl + "/Citizen/GetActsOfUser").
            success(function (data, status) {
                try {
                    $scope.ActsOfUser = data;
                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }
            }).
            error(function (data1, status) {

            });
    }

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetAlldeshdata();
        $scope.GetComplaintReportCounts();
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data.filter(x => x.FIN_YEAR_END != data[0].FIN_YEAR_END);

                if ($scope.FinancialYearList.length > 0) {
                    var FYDefaultValue = $scope.FinancialYearList[0].FIN_YEAR_MASTR_ID;


                    $scope.AuditorInspection["FinancialYear"] = FYDefaultValue;
                    $scope.drawInspectionVisualization(FYDefaultValue);
                }
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
        $scope.GetCCSCountdata();
        $(".se-pre-con").fadeOut("slow");
    };

    //CCS Complaint Report Count
    $scope.GetComplaintReportCounts = function () {

        var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;
        $(".se-pre-con").fadeIn("slow");
        $scope.GetActionIdCCSComplaintReport = [];
        $scope.GetCCSCompalainteReportData = [];
        $http.post(SiteUrl + "/User/GetActionIdCCSComplaintReport?ActionId=0&DistrictId=" + districtId + "&IsRowCount=true").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetActionIdCCSComplaintReport = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetActionIdCCSComplaintReport = [];
            });
    }


    $scope.GetCCSCountdata = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Citizen/GetCCSCount").
            success(function (data, status) {
                $scope.CCSCountdata = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
                //var chart = new google.visualization.ColumnChart(document.getElementById('chartMSPRajFedQty'));
                //chart.draw(data, options);
            }).
            error(function (data1, status) {
            });
        $(".se-pre-con").fadeOut("slow");
    }


    $scope.OnPageLoad();

});






