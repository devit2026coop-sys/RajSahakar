
angular.module('myApp', ['googlechart', 'zingchart-angularjs'])
    .controller('myController', function ($scope, $http) {
        $scope.colsList = [];
        $scope.ParticipantsList = []
        $scope.rows = [];
        $scope.FinalDataList = [];
        $scope.OptionsList = [];
        $scope.Headers = [];
        $scope.Rows = [];
        var Option = '';
        var IdCollection = '';
        $scope.IsVisible = false;
        $scope.GetActionIdCCSComplaintReport = [];
        $scope.ComplainUnderSection = [];

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
                this.ACT2005 = 0,
                this.APPROVED = 0
            this.TOTAL = 0
        }

        if ($("#hdnDistrictId").val() == "0") {
            $(".dashFilter").show();
        }

        // START : Code for Language label setting
        $scope.getTranslation = function () {
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
            }
            $(".se-pre-con").fadeOut("slow");

        };
        $scope.GetGraph = function (chartType, title, Responsedata, divId) {
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(drawChart);

            function drawChart() {
                var data = google.visualization.arrayToDataTable(Responsedata);
                console.log(data);

                // Optional; add a title and set the width and height of the chart
                if (chartType == 'pie') {
                    var options = { 'title': title, 'width': 406, 'height': 275 };
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
        }
        //START : Get Division List
        $scope.GetDivision = function () {
            $scope.Divisions = [{}];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
                success(function (data, status) {
                    $scope.Divisions = data;
                }).
                error(function (data, status) {
                    $scope.Divisions = [];
                });
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
        //$scope.GetDashboardCountSocietyAuditprogrss();


        $scope.GetSocietyDistrictList = function () {
            $(".se-pre-con").fadeIn("slow");
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegistration.DIVISION_ID).
                success(function (data, status) {
                    $scope.DistrictList = data;
                    $scope.GetNewdashboard();
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.DistrictList = [];
                });
        };
        $scope.WarehouseDashboardData = [];
        $scope.TotalWRCount = 0;
        $scope.DashWarehouseBoardData = function () {
            //$scope.GetGraph();
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

                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    $scope.DashboardData.forEach(function (item, index) {
                        Responsedata1.push([_language == 'en-us' ? item.CommodityNameEn : item.CommodityNameHi, parseInt(item.Quantity, 10), parseInt(item.Quantity, 10)]);

                        if (index == ($scope.DashboardData.length - 1)) {
                            $scope.GetGraph('pie', _language == 'en-us' ? 'Warehouse Commodity Wise E-WR Graph' : 'ग्राफ', Responsedata1, 'divChart1');
                        }

                    })

                }).
                error(function (data1, status) {
                    $scope.DashboardData = [];
                });
        };
        //   $scope.DashWarehouseBoardData();

        $scope.ComplainUnderSection = [];
        $scope.TotalComplainUnderSection = 0;
        //CCS ComplainUnderSection Report Count
        $scope.GetComplainUnderSection = function () {

            var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;
            $(".se-pre-con").fadeIn("slow");
            $scope.ComplainUnderSection = [];
            $scope.GetCCSCompalainteReportData = [];
            $http.post(SiteUrl + "/User/GetComplainUnderSection?ActionId=0&DistrictId=" + districtId + "&IsRowCount=true").
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ComplainUnderSection = JSON.parse(data.DataList);

                    $scope.ComplainUnderSection.forEach(function (item, index) {
                        $scope.TotalComplainUnderSection += item.Total;
                    });
                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    $scope.ComplainUnderSection.forEach(function (item, index) {
                        Responsedata1.push([_language == 'en-us' ? item.StatusName : item.StatusNameHi, parseInt(item.Total, 10), parseInt(item.Total, 10)]);

                        if (index == ($scope.ComplainUnderSection.length - 1)) {
                            $scope.GetGraph('pie', _language == 'en-us' ? 'Complain Under Section Status Graph' : 'ग्राफ', Responsedata1, 'divChart5');
                        }

                    })
                    //setTimeout(function () {
                    //    $scope.GetGraph('pie', _language == 'en-us' ? 'Complain Under Section Status Graph' : 'ग्राफ', Responsedata1, 'divChart5');
                    //}, 5000);

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ComplainUnderSection = [];
                });
        }

        //$scope.GetComplainUnderSection();


        //END
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

                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([$scope.translation.Reject, parseInt($scope.GetActionIdCCSComplaintReport[0].Reject, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Reject, 10)]);
                    Responsedata1.push([$scope.translation.dispose, parseInt($scope.GetActionIdCCSComplaintReport[0].Dispose, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Dispose, 10)]);
                    Responsedata1.push([$scope.translation.Hold, parseInt($scope.GetActionIdCCSComplaintReport[0].Hold, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Hold, 10)]);
                    Responsedata1.push([$scope.translation.NextHearing, parseInt($scope.GetActionIdCCSComplaintReport[0].NextHearing, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].NextHearing, 10)]);

                    $scope.GetGraph('pie', _language == 'en-us' ? 'CCS Compaint Status Graph' : 'ग्राफ', Responsedata1, 'divChart4');




                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.GetActionIdCCSComplaintReport = [];
                });
        }


        //$scope.openreportReport = function (Status) {
        //    window.location = '../MIS/SocietyRegisterDistWise?ActName=' + Status;



        //}


       // $scope.GetComplaintReportCounts();

        $scope.GetAlldeshdata_OLD = function () {
            $http.get(SiteUrl + "/Citizen/GetDeshboardData").
                success(function (data, status) {
                    //$scope.CaseRegDetailsData = '';
                    //$scope.CaseRegDetailsData = data.Table;
                    //$scope.CaseRegDetailsData = $scope.CaseRegDetailsData[0];
                    //$scope.CaseRegApplntDetailsData = data.Table1;
                    try {
                        $scope.Deshboard = data.Table;
                        var DD = new DashboardData();
                        DD.PARTIALSUBMITTED = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' })).length;
                        DD.SUBMITTED = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'SUBMITTED' })).length;
                        DD.CERTIFICATEISSUED = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'CERTIFICATE ISSUED' })).length;
                        DD.PAYMENTREQUEST = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'REGISTRAR ISSUED BANK OPENING CERTIFICATE' || x.ProceesStatusTextEn == 'REQUEST FOR PAYMENT BY  REGISTRAR' })).length;
                        DD.REJECTED = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'REJECTED BY CLERK' || x.ProceesStatusTextEn == 'REJECTED BY REGISTRAR' || x.ProceesStatusTextEn == 'REJECTED (DUE TO PAYMENT)' || x.ProceesStatusTextEn == 'REJECTED DUE TO OBJECTION IS NOT CLEAR BY CITIZEN' || x.ProceesStatusTextEn == 'REJECTED BY INSPECTOR' })).length;
                        //DD.TOTAL = (DD.PARTIALSUBMITTED + DD.SUBMITTED + DD.CERTIFICATEISSUED + DD.PAYMENTREQUEST + DD.REJECTED);
                        DD.APPROVED = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'APPROVED' })).length;
                        var AutoClosur = ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'AUTO CLOSERE DUE TO TIME LIMIT EXPIRED (NON PAYMENT)' || x.ProceesStatusTextEn == 'AUTO CLOSERE DUE TO TIME LIMIT EXPIRED (NON CLARIFICATION OF OBJECTION)' || x.ProceesStatusTextEn == 'AUTO CLOSURE' || x.ProceesStatusTextEn == 'AUTO CLOSERE DUE TO BANK ACCOUNT DETAILS NOT UPLOAD' })).length;


                        DD.UNDERPROCESS = ($scope.Deshboard).length - (DD.APPROVED + DD.REJECTED + DD.CERTIFICATEISSUED + DD.PARTIALSUBMITTED + AutoClosur)

                        DD.TOTAL = (($scope.Deshboard).length - DD.PARTIALSUBMITTED);

                        DD.ACT1958 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '1' && x.ProceesStatusTextEn == 'APPROVED' })).length;
                        DD.ACT2001 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '3' && x.ProceesStatusTextEn == 'APPROVED' })).length;
                        DD.ACT2005 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '7' && x.ProceesStatusTextEn == 'APPROVED' })).length;







                        DashboardDataList.push(DD);

                        $scope.TACT1958 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '1' }).length - ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' && x.ACTID == '1' }).length));
                        $scope.TACT2001 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '3' }).length - ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' && x.ACTID == '3' }).length));
                        $scope.TACT2005 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '7' }).length - ($scope.Deshboard.filter(function (x) { return x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' && x.ACTID == '7' }).length));

                        $scope.PACT1958 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '1' && x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' })).length;
                        $scope.PACT2001 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '3' && x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' })).length;
                        $scope.PACT2005 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '7' && x.ProceesStatusTextEn == 'PARTIAL SUBMITTED' })).length;

                        $scope.DeshboardBelow = data.Table2;
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
                        //alert("Error occurred in DeshboardData. " + err.message);
                    }



                }).
                error(function (data1, status) {

                });
        }


        $scope.ACTWiseTotalAppli = 0;
        $scope.LevelWiseTotalAppli = 0;
        $scope.ClassWiseTotalAppli = 0;
        $scope.StatusWiseTotalAppli = 0;
        $scope.CategoryTotalAppli = 0;
        $scope.SocietyUnderProcessTotal = 0;
        $scope.NewSocietyRegistrationTotal = 0;
        $scope.SocietyStatustaskpenalOnlineTotal = 0;
        $scope.SocietyStatustaskpenalOfflineTotal = 0;
        //$scope.GetDeshBoard = function () {




        $scope.GetDeshBoard = function (DivisionId, DistrictId) {

            return $http.get(SiteUrl + "/user/GetDasboardData").
                success(function (data, status) {

                    var DataResult = data.DataList;
                    $scope.SocietyRegistrationList = [];// DataResult.filter(function (x) { return x.typeameEng === 'SocietyRegistration' });

                    $("#TotalSocieties").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocieties' })[0].totalCount);
                    $("#TotalRegistaredSocieties").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalRegistaredSocieties' })[0].totalCount);
                    $("#TotalSocietiesUnderProcess").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesUnderProcess' })[0].totalCount);
                    $("#TotalSocietiesAutoCoased").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAutoCoased' })[0].totalCount);
                    $("#TotalSocietiesRejected").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesRejected' })[0].totalCount);

                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    //Responsedata1.push(["Total Societies", parseInt($("#TotalSocieties").text(), 10), parseInt($("#TotalSocieties").text(), 10)]);
                    Responsedata1.push(["Registared", parseInt($("#TotalRegistaredSocieties").text(), 10), parseInt($("#TotalRegistaredSocieties").text(), 10)]);
                    Responsedata1.push(["Under Process", parseInt($("#TotalSocietiesUnderProcess").text(), 10), parseInt($("#TotalSocietiesUnderProcess").text(), 10)]);
                    Responsedata1.push(["Auto Closed", parseInt($("#TotalSocietiesAutoCoased").text(), 10), parseInt($("#TotalSocietiesAutoCoased").text(), 10)]);
                    Responsedata1.push(["Rejected", parseInt($("#TotalSocietiesRejected").text(), 10), parseInt($("#TotalSocietiesRejected").text(), 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Society Registration Graph' : 'ग्राफ', Responsedata1, 'divChart1');


                    $("#TotalSocietiesAct").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct' })[0].totalCount)
                    $("#TotalSocietiesAct2001").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct2001' })[0].totalCount)
                    $("#TotalSocietiesAct1958").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct1958' })[0].totalCount)
                    $("#TotalSocietiesAct2005").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct2005' })[0].totalCount)


                    Responsedata2 = [];                   
                    Responsedata2.push(['Task', '', { role: 'annotation' }]);
                    Responsedata2.push(["Co-operative Societies Act,2001", parseInt($("#TotalSocietiesAct2001").text(), 10), parseInt($("#TotalSocietiesAct2001").text(), 10)]);
                    Responsedata2.push(["Society Registration Act 1958 (NGO, School etc)", parseInt($("#TotalSocietiesAct1958").text(), 10), parseInt($("#TotalSocietiesAct1958").text(), 10)]);
                    Responsedata2.push(["Registration of Sports associations under the Rajasthan Sports Act ,2005", parseInt($("#TotalSocietiesAct2005").text(), 10), parseInt($("#TotalSocietiesAct2005").text(), 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Society Registration Act Graph' : 'ग्राफ', Responsedata2, 'divChart2');



                    $("#TotalSocietiesActWise2001").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesActWise2001' })[0].totalCount);
                    $("#TotalSocietiesAct2001Registared").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct2001Registared' })[0].totalCount);
                    $("#TotalSocietiesAct2001UnderProcess").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct2001UnderProcess' })[0].totalCount);
                    $("#TotalSocietiesAct2001").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct2001' })[0].totalCount);
                    $("#TotalSocietiesAct2001AutoClosed").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAct2001AutoClosed' })[0].totalCount);

                    Responsedata3 = [];
                    Responsedata3.push(['Task', '', { role: 'annotation' }]);
                    Responsedata3.push(["Registared", parseInt($("#TotalSocietiesAct2001Registared").text(), 10), parseInt($("#TotalSocietiesAct2001Registared").text(), 10)]);
                    Responsedata3.push(["Under Process", parseInt($("#TotalSocietiesAct2001UnderProcess").text(), 10), parseInt($("#TotalSocietiesAct2001UnderProcess").text(), 10)]);
                    Responsedata3.push(["Auto Closed", parseInt($("#TotalSocietiesAct2001").text(), 10), parseInt($("#TotalSocietiesAct2001").text(), 10)]);
                    Responsedata3.push(["Rejected", parseInt($("#TotalSocietiesAct2001AutoClosed").text(), 10), parseInt($("#TotalSocietiesAct2001AutoClosed").text(), 10)]);

                    $scope.GetGraph('pie', _language == 'en-us' ? 'Co-operative Societies Act,2001 Graph' : 'ग्राफ', Responsedata3, 'divChart3');



                    $("#TotalSocieties1958").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocieties1958' })[0].totalCount);
                    $("#TotalSocietiesRegistared1958").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesRegistared1958' })[0].totalCount);
                    $("#TotalSocietiesUnderProcess1958").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesUnderProcess1958' })[0].totalCount);
                    $("#TotalSocietiesAutoClosed1958").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAutoClosed1958' })[0].totalCount);
                    $("#TotalSocietiesRejected1958").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesRejected1958' })[0].totalCount);

                    Responsedata4 = [];
                    Responsedata4.push(['Task', '', { role: 'annotation' }]);
                    Responsedata4.push(["Registared", parseInt($("#TotalSocietiesRegistared1958").text(), 10), parseInt($("#TotalSocietiesRegistared1958").text(), 10)]);
                    Responsedata4.push(["Under Process", parseInt($("#TotalSocietiesUnderProcess1958").text(), 10), parseInt($("#TotalSocietiesUnderProcess1958").text(), 10)]);
                    Responsedata4.push(["Auto Closed", parseInt($("#TotalSocietiesAutoClosed1958").text(), 10), parseInt($("#TotalSocietiesAutoClosed1958").text(), 10)]);
                    Responsedata4.push(["Rejected", parseInt($("#TotalSocietiesRejected1958").text(), 10), parseInt($("#TotalSocietiesRejected1958").text(), 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Society Registration Act 1958 (NGO, School etc) Graph' : 'ग्राफ', Responsedata4, 'divChart4');


                    $("#TotalSocieties2005").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocieties2005' })[0].totalCount);
                    $("#TotalSocietiesRegistared2005").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesRegistared2005' })[0].totalCount);
                    $("#TotalSocietiesUnderProcess2005").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesUnderProcess2005' })[0].totalCount);
                    $("#TotalSocietiesAutoClosed2005").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesAutoClosed2005' })[0].totalCount);
                    $("#TotalSocietiesRejected2005").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSocietiesRejected2005' })[0].totalCount);


                    Responsedata5 = [];
                    Responsedata5.push(['Task', '', { role: 'annotation' }]);
                    Responsedata5.push(["Registared", parseInt($("#TotalSocietiesRegistared2005").text(), 10), parseInt($("#TotalSocietiesRegistared2005").text(), 10)]);
                    Responsedata5.push(["Under Process", parseInt($("#TotalSocietiesUnderProcess2005").text(), 10), parseInt($("#TotalSocietiesUnderProcess2005").text(), 10)]);
                    Responsedata5.push(["Auto Closed", parseInt($("#TotalSocietiesAutoClosed2005").text(), 10), parseInt($("#TotalSocietiesAutoClosed2005").text(), 10)]);
                    Responsedata5.push(["Rejected", parseInt($("#TotalSocietiesRejected2005").text(), 10), parseInt($("#TotalSocietiesRejected2005").text(), 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Registration of Sports associations under the Rajasthan Sports Act ,2005 Graph' : 'ग्राफ', Responsedata5, 'divChart5');

                    $("#TotalCENTRAL").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalCENTRAL' })[0].totalCount);
                    $("#TotalPRIMARY").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalPRIMARY' })[0].totalCount);
                    $("#TotalAPEX").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalAPEX' })[0].totalCount);
                    $("#TotalAROGYASAMITI").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalAROGYASAMITI' })[0].totalCount);
                    $("#TotalGAUSEVASAMITI").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalGAUSEVASAMITI' })[0].totalCount);
                    $("#TotalNEHRUYUVAMANDA").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalNEHRUYUVAMANDA' })[0].totalCount);
                    $("#TotalPRIMARYDAIRY").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalPRIMARYDAIRY' })[0].totalCount);
                    $("#TotalVETERINARYHOSPITAL").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalVETERINARYHOSPITAL' })[0].totalCount);
                    $("#TotalAPARTMENTOWNERSHIPACT").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalAPARTMENTOWNERSHIPACT' })[0].totalCount);
                    $("#TotalSCHOOLDEVELOPMENT").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalSCHOOLDEVELOPMENT' })[0].totalCount);
                    $("#TotalRAJIVGANDHIPEYAJAL").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalRAJIVGANDHIPEYAJAL' })[0].totalCount);
                    $("#TotalHOUSINGSOCIETY").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalHOUSINGSOCIETY' })[0].totalCount);



                    Responsedata6 = [];
                    Responsedata6.push(['Task', '', { role: 'annotation' }]);
                    Responsedata6.push(["CENTRAL", parseInt($("#TotalCENTRAL").text(), 10), parseInt($("#TotalCENTRAL").text(), 10)]);
                    Responsedata6.push(["PRIMARY", parseInt($("#TotalPRIMARY").text(), 10), parseInt($("#TotalPRIMARY").text(), 10)]);
                    Responsedata6.push(["FEDERATION/APEX", parseInt($("#TotalAPEX").text(), 10), parseInt($("#TotalAPEX").text(), 10)]);
                    Responsedata6.push(["AROGYA SAMITI(RAJKIYA)", parseInt($("#TotalAROGYASAMITI").text(), 10), parseInt($("#TotalAROGYASAMITI").text(), 10)]);
                    Responsedata6.push(["GAU SEVA SAMITI", parseInt($("#TotalGAUSEVASAMITI").text(), 10), parseInt($("#TotalGAUSEVASAMITI").text(), 10)]);
                    Responsedata6.push(["NEHRU YUVA MANDAL", parseInt($("#TotalNEHRUYUVAMANDA").text(), 10), parseInt($("#TotalNEHRUYUVAMANDA").text(), 10)]);
                    Responsedata6.push(["PRIMARY DAIRY COOPERATIVE SOCIETY LTD", parseInt($("#TotalPRIMARYDAIRY").text(), 10), parseInt($("#TotalPRIMARYDAIRY").text(), 10)]);
                    Responsedata6.push(["GOVERNMENT VETERINARY HOSPITAL RELIEF SOCIETY", parseInt($("#TotalVETERINARYHOSPITAL").text(), 10), parseInt($("#TotalVETERINARYHOSPITAL").text(), 10)]);
                    Responsedata6.push(["GOVERNMENT SCHOOL DEVELOPMENT AND MANAGEMENT COMMITTEE", parseInt($("#TotalSCHOOLDEVELOPMENT").text(), 10), parseInt($("#TotalSCHOOLDEVELOPMENT").text(), 10)]);
                    Responsedata6.push(["RAJASTHAN APARTMENT OWNERSHIP ACT", parseInt($("#TotalAPARTMENTOWNERSHIPACT").text(), 10), parseInt($("#TotalAPARTMENTOWNERSHIPACT").text(), 10)]);
                    Responsedata6.push(["SOCIETIES UNDER RAJIV GANDHI PEYAJAL MISSION", parseInt($("#TotalRAJIVGANDHIPEYAJAL").text(), 10), parseInt($("#TotalRAJIVGANDHIPEYAJAL").text(), 10)]);
                    Responsedata6.push(["HOUSING SOCIETY", parseInt($("#TotalHOUSINGSOCIETY").text(), 10), parseInt($("#TotalHOUSINGSOCIETY").text(), 10)]);


                    $scope.GetGraph('pie', _language == 'en-us' ? 'Societies Type Wise Graph' : 'ग्राफ', Responsedata6, 'divChart6');



                    $("#TotalCCSComplaints").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalCCSComplaints' })[0].totalCount);
                    $("#TotalNoticeisssued").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalNoticeisssued' })[0].totalCount);
                    $("#TotalInstgasaPrepared").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalInstgasaPrepared' })[0].totalCount);
                    $("#TotalNotRelatedtoUnregulatedDeposit").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalNotRelatedtoUnregulatedDeposit' })[0].totalCount);
                    $("#TotalDisposePerRequest").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalDisposePerRequest' })[0].totalCount);
                    $("#TotalDisposeByCourt").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalDisposeByCourt' })[0].totalCount);
                    $("#TotalIstgasaSubmitted").text(DataResult.filter(function (x) { return x.typeameEng === 'TotalIstgasaSubmitted' })[0].totalCount);
                    

                    Responsedata8 = [];
                    Responsedata8.push(['Task', '', { role: 'annotation' }]);
                    Responsedata8.push(["Notice isssued", parseInt($("#TotalNoticeisssued").text(), 10), parseInt($("#TotalNoticeisssued").text(), 10)]);
                    Responsedata8.push(["Instgasa Prepared and made available to PP", parseInt($("#TotalInstgasaPrepared").text(), 10), parseInt($("#TotalInstgasaPrepared").text(), 10)]);
                    Responsedata8.push(["Not Related to Unregulated Deposit Schemes  2019", parseInt($("#TotalNotRelatedtoUnregulatedDeposit").text(), 10), parseInt($("#TotalNotRelatedtoUnregulatedDeposit").text(), 10)]);
                    Responsedata8.push(["Dispose as per request from Complainant", parseInt($("#TotalDisposePerRequest").text(), 10), parseInt($("#TotalDisposePerRequest").text(), 10)]);
                    Responsedata8.push(["Dispose by Court", parseInt($("#TotalDisposeByCourt").text(), 10), parseInt($("#TotalDisposeByCourt").text(), 10)]);
                    Responsedata8.push(["Istgasa Submitted to Court", parseInt($("#TotalIstgasaSubmitted").text(), 10), parseInt($("#TotalIstgasaSubmitted").text(), 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Credit Cooperative Societies Complaints Graph' : 'ग्राफ', Responsedata8, 'divChart8');




                    $("#SocietyCategoryWiseList").val("");
                    DataResult.filter(function (x) { return x.typeameEng === 'SocietyCategory' }).forEach(function (item,index) {
                        $("#SocietyCategoryWiseList").append(' <a href="javascript:void(0)" class="collection-item">' + item.typeameEng + '<span class="new badge2" id="TotalSocieties2005">' + item.totalCount +'</span></a>');
                    });
                   
                    $scope.SocietyTypeList = DataResult.Table1.filter(function (x) { return x.Type === 'SocietyType' });
                    $scope.ClassTypeList = DataResult.Table1.filter(function (x) { return x.Type === 'ClassType' });
                    $scope.ProceesStatusList = DataResult.Table1.filter(function (x) { return x.Type === 'ProceesStatus' });
                    $scope.ActList = DataResult.Table1.filter(function (x) { return x.Type === 'Act' });
                    $scope.CategoryList = DataResult.Table1.filter(function (x) { return x.Type === 'Category' });
                    $scope.ComplainUnderSection = DataResult.Table1.filter(function (x) { return x.Type === 'ComplainStatus55' });
                    $scope.WarehouseDashboardData = DataResult.Table1.filter(function (x) { return x.Type === 'WarehouseData' });


                    $scope.GetDivisionReport(DataResult.Table);

                    $scope.OnlineRegiAppli = DataResult.Table1.filter(function (x) { return x.Type === 'OnlineTotalAppli' })[0].Total;//DataResult.Table1[0].Total;
                    $scope.OfflineUnderProAppli = DataResult.Table1.filter(function (x) { return x.Type === 'OfflineTotalAppli' })[0].Total;// DataResult.Table2[1].Total;



                    $scope.OnlineUnderProAppli = DataResult.Table1.filter(function (x) { return x.Type === 'OnlineUnderProcessTotalAppli' })[0].Total; //DataResult.Table2[0].Total;
                    $scope.OfflineRegiAppli = DataResult.Table1.filter(function (x) { return x.Type === 'OfflineUnderProcessTotalAppli' })[0].Total;// DataResult.Table1[1].Total;

                    $scope.TotalCCSCompaint = DataResult.Table1.filter(function (x) { return x.Type === 'TotalCSSComplaintRegister' })[0].Total;// DataResult.Table3[0].TotalCSSComplaintRegister;

                    $scope.TotalComplainUnderSection = DataResult.Table1.filter(function (x) { return x.Type === 'TotalComplainUnderSection' })[0].Total;// DataResult.Table4[0].TotalComplainUnderSection;

                    $scope.ACTWiseTotalAppli = 0;
                    $scope.LevelWiseTotalAppli = 0;
                    $scope.ClassWiseTotalAppli = 0;
                    $scope.StatusWiseTotalAppli = 0;
                    $scope.CategoryTotalAppli = 0;
                    $scope.SocietyTypeList.forEach(function (item, index) {
                        $scope.LevelWiseTotalAppli += parseInt(item.Total);
                    });

                    $scope.ClassTypeList.forEach(function (item, index) {
                        $scope.ClassWiseTotalAppli += parseInt(item.Total);
                    });
                    $scope.ProceesStatusList.forEach(function (item, index) {
                        $scope.StatusWiseTotalAppli += parseInt(item.Total);
                    });
                    $scope.ActList.forEach(function (item, index) {
                        $scope.ACTWiseTotalAppli += parseInt(item.Total);
                    });
                    $scope.CategoryList.forEach(function (item, index) {
                        $scope.CategoryTotalAppli += parseInt(item.Total);
                    });
                    var count = 0;

                    $scope.WarehouseDashboardData.forEach(function (i, j) {
                        count += parseInt(i.Total, 10);
                    });

                    $scope.TotalWRCount = count;

                    var electindata = DataResult.Table1.filter(function (x) { return x.Type === 'Election' });
                    var TotalOrderGenerated = electindata.filter(function (x) { return x.TypeameEng === 'TotalOrderGenerated' })[0].Total;
                    var UpcomingElection = electindata.filter(function (x) { return x.TypeameEng === 'UpcomingElection' })[0].Total;
                    var ElectionInProgress = electindata.filter(function (x) { return x.TypeameEng === 'ElectionInProgress' })[0].Total;
                    var TotalElectionCompleted = electindata.filter(function (x) { return x.TypeameEng === 'TotalElectionCompleted' })[0].Total;


                    $scope.TotalElectableSocieties = electindata.filter(function (x) { return x.TypeameEng === 'TotalSocieties' })[0].Total;
                    $scope.TotalElectionSocieties = electindata.filter(function (x) { return x.TypeameEng === 'TotalElectionSocieties' })[0].Total;
                    $scope.TotalOrderGenerated = TotalOrderGenerated;
                    $scope.UpcomingElection = UpcomingElection;
                    $scope.ElectionInProgress = ElectionInProgress;
                    $scope.TotalElectionCompleted = TotalElectionCompleted;




                    $scope.OfflineRegiAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Offline Societie' && x.TypeameEng === 'RegisteredSociety' })[0].Total;// DataResult.Table7[0].RegisteredSociety;
                    $scope.OfflineUnderProAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Offline Societie' && x.TypeameEng === 'UnderProcess' })[0].Total;// DataResult.Table7[0].UnderProcess;
                    $scope.OfflineRejeAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Offline Societie' && x.TypeameEng === 'RejectedSociety' })[0].Total;// DataResult.Table7[0].RejectedSociety;
                    $scope.OfflinePartialAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Offline Societie' && x.TypeameEng === 'PartialSubmit' })[0].Total;// DataResult.Table7[0].PartialSubmit;

                    $scope.OnlineRegiAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Online Societie' && x.TypeameEng === 'RegisteredSociety' })[0].Total;// DataResult.Table7[0].RegisteredSociety;
                    $scope.OnlineUnderProAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Online Societie' && x.TypeameEng === 'UnderProcess' })[0].Total;// DataResult.Table7[0].UnderProcess;
                    $scope.OnlineRejeAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Online Societie' && x.TypeameEng === 'RejectedSociety' })[0].Total;// DataResult.Table7[0].RejectedSociety;
                    $scope.OnlinePartialAppli1 = DataResult.Table1.filter(function (x) { return x.Type === 'Online Societie' && x.TypeameEng === 'PartialSubmit' })[0].Total;// DataResult.Table7[0].PartialSubmit;

                    $scope.SocietyStatustaskpenalOfflineTotal = parseInt($scope.OfflineRegiAppli1) + parseInt($scope.OfflineUnderProAppli1) + parseInt($scope.OfflineRejeAppli1) + parseInt($scope.OfflinePartialAppli1);
                    $scope.SocietyStatustaskpenalOnlineTotal = parseInt($scope.OnlineRegiAppli1) + parseInt($scope.OnlineUnderProAppli1) + parseInt($scope.OnlineRejeAppli1) + parseInt($scope.OnlinePartialAppli1);


                    //$scope.OnlineRegiAppli1 = DataResult.Table8[0].RegisteredSociety;
                    //$scope.OnlineUnderProAppli1 = DataResult.Table8[0].UnderProcess;
                    //$scope.OnlineRejeAppli1 = DataResult.Table8[0].RejectedSociety;
                    //$scope.OnlinePartialAppli1 = DataResult.Table8[0].PartialSubmit;

                    $scope.SocietyUnderProcessTotal = parseInt($scope.OnlineUnderProAppli) + parseInt($scope.OfflineUnderProAppli);
                    $scope.NewSocietyRegistrationTotal = parseInt($scope.OnlineRegiAppli) + parseInt($scope.OfflineRegiAppli);
                    var Responsedata1 = [];
                    // Graph 1 data
                    //Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    //Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt($scope.SUBMITTED, 10), parseInt($scope.SUBMITTED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt($scope.APPROVED, 10), parseInt($scope.APPROVED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt($scope.PARTIALSUBMITTED, 10), parseInt($scope.PARTIALSUBMITTED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt($scope.PAYMENTREQUEST, 10), parseInt($scope.PAYMENTREQUEST, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt($scope.REJECTED, 10), parseInt($scope.REJECTED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt($scope.UNDERPROCESS, 10), parseInt($scope.UNDERPROCESS, 10)]);
                    //$scope.GetGraph('pie', _language == 'en-us' ? 'Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart1');





                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([$scope.translation.RegisteredSociety, parseInt($scope.OfflineRegiAppli1, 10), parseInt($scope.OfflineRegiAppli1, 10)]);
                    Responsedata1.push([$scope.translation.UnderProcess, parseInt($scope.OfflineUnderProAppli1, 10), parseInt($scope.OfflineUnderProAppli1, 10)]);
                    Responsedata1.push([$scope.translation.RejectedSociety, parseInt($scope.OfflineRejeAppli1, 10), parseInt($scope.OfflineRejeAppli1, 10)]);
                    Responsedata1.push([$scope.translation.PartialSubmit, parseInt($scope.OfflinePartialAppli1, 10), parseInt($scope.OfflinePartialAppli1, 10)]);
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Offline Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart2');

                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([$scope.translation.RegisteredSociety, parseInt($scope.OnlineRegiAppli1, 10), parseInt($scope.OnlineRegiAppli1, 10)]);
                    Responsedata1.push([$scope.translation.UnderProcess, parseInt($scope.OnlineUnderProAppli1, 10), parseInt($scope.OnlineUnderProAppli1, 10)]);
                    Responsedata1.push([$scope.translation.RejectedSociety, parseInt($scope.OnlineRejeAppli1, 10), parseInt($scope.OnlineRejeAppli1, 10)]);
                    Responsedata1.push([$scope.translation.PartialSubmit, parseInt($scope.OnlinePartialAppli1, 10), parseInt($scope.OnlinePartialAppli1, 10)]);

                    $scope.GetGraph('pie', _language == 'en-us' ? 'Online  Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart3');

                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([$scope.translation.Reject, parseInt($scope.GetActionIdCCSComplaintReport[0].Reject, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Reject, 10)]);
                    Responsedata1.push([$scope.translation.dispose, parseInt($scope.GetActionIdCCSComplaintReport[0].Dispose, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Dispose, 10)]);
                    Responsedata1.push([$scope.translation.Hold, parseInt($scope.GetActionIdCCSComplaintReport[0].Hold, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Hold, 10)]);
                    Responsedata1.push([$scope.translation.NextHearing, parseInt($scope.GetActionIdCCSComplaintReport[0].NextHearing, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].NextHearing, 10)]);

                    $scope.GetGraph('pie', _language == 'en-us' ? 'CCS Compaint Status Graph' : 'ग्राफ', Responsedata1, 'divChart4');


                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push(['Total Order Generated', parseInt(TotalOrderGenerated, 10), parseInt(TotalOrderGenerated, 10)]);
                    Responsedata1.push(['Election Upcoming', parseInt(UpcomingElection, 10), parseInt(UpcomingElection, 10)]);
                    Responsedata1.push(['Election In Progress', parseInt(ElectionInProgress, 10), parseInt(ElectionInProgress, 10)]);
                    Responsedata1.push(['Election Completed', parseInt(TotalElectionCompleted, 10), parseInt(TotalElectionCompleted, 10)]);


                    $scope.GetGraph('pie', _language == 'en-us' ? 'Election Status (For Act 2001) Graph' : 'ग्राफ', Responsedata1, 'divChart6');



                    Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    $scope.WarehouseDashboardData.forEach(function (item, index) {
                        Responsedata1.push([_language == 'en-us' ? item.TypeameEng : item.TypeNameHin, parseInt(item.Total, 10), parseInt(item.Total, 10)]);

                        if (index == ($scope.WarehouseDashboardData.length - 1)) {
                            $scope.GetGraph('pie', _language == 'en-us' ? 'Warehouse Commodity Wise E-WR Graph' : 'ग्राफ', Responsedata1, 'divChart1');
                        }

                    })

                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    $scope.ComplainUnderSection.forEach(function (item, index) {
                        Responsedata1.push([_language == 'en-us' ? item.TypeameEng : item.TypeNameHin, parseInt(item.Total, 10), parseInt(item.Total, 10)]);

                        if (index == ($scope.ComplainUnderSection.length - 1)) {
                            $scope.GetGraph('pie', _language == 'en-us' ? 'Complain Under Section Status Graph' : 'ग्राफ', Responsedata1, 'divChart5');
                        }

                    })

                    //$(".se-pre-con").fadeOut("slow");
                });
        }
        $scope.GetDeshBoard(0, 0).then(function () {
            $(".se-pre-con").fadeOut("slow");
        });

        function FertilizerOpeningStockReport() {
            current_progress_Status(15);
            $.ajax({
                url: SiteUrl + "/User/CommonFunctionForDashBoard",
                async: false,
                type: 'GET',
                dataType: 'json',
                success: function (DataSet) {

                    //START Fertilizer Progress
                    $scope.FertilizerOpeningStockData = DataSet.Table5;
                    var TotalStock = 0;
                    var Urea = 0;
                    var Compost = 0;
                    var DAP = 0;
                    var MOP = 0;
                    var NPKS = 0;
                    var SSP = 0;
                    $scope.StockOn = '';
                    angular.forEach(DataSet.Table5, function (item) {
                        TotalStock = TotalStock + parseFloat(item.Total);
                        if (item.FertilizerName_En == 'Compost')
                            Compost = Compost + parseFloat(item.Total);
                        if (item.FertilizerName_En == 'DAP')
                            DAP = DAP + parseFloat(item.Total);
                        if (item.FertilizerName_En == 'MOP')
                            MOP = MOP + parseFloat(item.Total);
                        if (item.FertilizerName_En == 'NPKS')
                            NPKS = NPKS + parseFloat(item.Total);
                        if (item.FertilizerName_En == 'SSP')
                            SSP = SSP + parseFloat(item.Total);
                        if (item.FertilizerName_En == 'Urea')
                            Urea = Urea + parseFloat(item.Total);
                        $scope.StockOn = '(' + item.OnDate + ')';
                    });

                    $scope.Urea = Urea.toString().split('.')[0] + '.' + (Urea.toString().split('.')[1]).substr(0, 2);
                    $scope.Compost = Compost.toString().split('.')[0] + '.' + (Compost.toString().split('.')[1]).substr(0, 2);
                    $scope.DAP = DAP.toString().split('.')[0] + '.' + (DAP.toString().split('.')[1]).substr(0, 2);
                    $scope.MOP = MOP.toString().split('.')[0] + '.' + (MOP.toString().split('.')[1]).substr(0, 2);
                    $scope.NPKS = NPKS.toString().split('.')[0] + '.' + (NPKS.toString().split('.')[1]).substr(0, 2);
                    $scope.SSP = SSP.toString().split('.')[0] + '.' + (SSP.toString().split('.')[1]).substr(0, 2);

                    $scope.TotalStock = TotalStock.toString().split('.')[0] + '.' + (TotalStock.toString().split('.')[1]).substr(0, 2);
                    //END Fertilizer Progress

                    //START LOAN WAIVER REPORT 2019
                    $scope.Avg_Loan_Waiver_Amount = DataSet.Table[0].Avg_Loan_Waiver_Amount;
                    $scope.OD_TOTAL_InCrores = DataSet.Table[0].OD_TOTAL_InCrores;
                    $scope.NOD_TOTAL_InCrores = DataSet.Table[0].NOD_TOTAL_InCrores;
                    $scope.New_Beneficiaries_2019 = DataSet.Table[0].New_Beneficiaries_2019;
                    $scope.Total_Not_Intrested_Cases = DataSet.Table[0].Total_Not_Intrested_Cases;
                    $scope.Total_Not_Eligible_Cases = DataSet.Table[0].Total_Not_Eligible_Cases;
                    $scope.Total_GRA_Cases_Pending = DataSet.Table[0].Total_GRA_Cases_Pending;
                    $scope.Total_Rechecking_Cases = DataSet.Table[0].Total_Rechecking_Cases;
                    $scope.Total_Cert_Distributed = DataSet.Table[0].Total_Cert_Distributed;
                    $scope.Total_Cert_Issued = DataSet.Table[0].Total_Cert_Issued;
                    $scope.Total_UID_Authentication = DataSet.Table[0].Total_UID_Authentication;
                    $scope.Total_Applications = DataSet.Table[0].Total_Applications;
                    $scope.Total_PACS = DataSet.Table[0].Total_PACS + '/' + DataSet.Table[0].TotalActive_PACS;
                    $scope.TotalActive_PACS = DataSet.Table[0].TotalActive_PACS;

                    $scope.Total_Branch_Office = DataSet.Table[0].Total_Branch_Office + '/' + DataSet.Table[0].TotalActive_Branch_Office;
                    $scope.TotalActive_Branch_Office = DataSet.Table[0].TotalActive_Branch_Office;
                    $scope.Total_Head_Office = DataSet.Table[0].Total_Head_Office + '/' + DataSet.Table[0].TotalActive_Head_Office
                    $scope.TotalActive_Head_Office = DataSet.Table[0].TotalActive_Head_Office + '/' + DataSet.Table[0].Total_Head_Office;

                    //END LOAN WAIVER REPORT 2019




                    //START BHANDAR

                    //drawVisualization(4);

                    var Arry = [];
                    if (_language == 'en-us')
                        Arry.push(['Bhandar', 'Controlled Business Target', 'Uncontrolled Business Target']);
                    else
                        Arry.push(['भंडार', 'नियंत्रित व्यापार लक्ष्य', 'अनियंत्रित व्यवसाय लक्ष्य']);


                    angular.forEach(DataSet.Table7, function (item) {
                        Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.ControlledBusinessTarget), parseInt(item.UNControlledBusinessTarget)]);

                    });

                    var dataBhandar = google.visualization.arrayToDataTable(Arry);
                    var options = {
                        title: (_language == 'en-us' ? 'Rajasthan State Cooperative Consumers Association (Stores)' : 'राजस्थान राज्य सहकारी उपभोक्ता संघ (भंडार )'),
                        vAxis: { title: (_language == 'en-us' ? '(In Crore)' : '(करोड़ में)') },
                        hAxis: { title: (_language == 'en-us' ? '(Bhandar)' : '(भंडार)') },
                        seriesType: 'bars',
                        series: { 5: { type: 'line' } }
                    };

                    var chart = new google.visualization.ColumnChart(document.getElementById('BhandarChart'));
                    chart.draw(dataBhandar, options);
                    //END BHANDAR

                    //STATIC
                    drawVisualizationTilamSangQty(4, 2);
                    $scope.DistrictwiseSocietyParam1 = 'to';
                    $scope.DistrictwiseSocietyParam2 = 'ap';
                    //END STATIC
                    current_progress_Status(30);

                    //START SOCIETY
                    var ArrySocieties = [];
                    if (_language == 'en-us')
                        ArrySocieties.push(['District', 'Societies']);
                    else
                        ArrySocieties.push(['District', 'Societies']);

                    angular.forEach(DataSet.Table8, function (item) {
                        ArrySocieties.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_ENG), parseInt(item.Data)]);
                    });

                    var data2 = google.visualization.arrayToDataTable(ArrySocieties);
                    var options = {
                        title: (_language == 'en-us' ? 'District Wise Total Approved Societies' : 'जिले वार कुल स्वीकृत सोसायटी'),
                        vAxis: { title: '(Count)' },
                        hAxis: { title: 'District' }, textStyle: {
                            fontSize: 10 // or the number you want
                        },
                        seriesType: 'bars',
                        series: { 5: { type: 'line' } }
                    };


                    var chart = new google.visualization.ColumnChart(document.getElementById('chartContainer'));

                    chart.draw(data2, options);

                    //END

                    //$scope.drawInspectionVisualization(48);

                    //START STLOAN
                    //$scope.FunGetStLoanDataForDashBoard();
                    $scope.ApplicationReceived = 0;
                    $scope.STTargetK = 0;
                    $scope.STTarget = 0;
                    $scope.STTargetR = 0;
                    $scope.ApprovedByPacs = 0;
                    $scope.ApprovedByBranch = 0;
                    $scope.LoanDisbursement = 0;
                    $scope.DMR_Credited = 0;

                    $scope.Loan_Disbursement = 0;
                    angular.forEach(DataSet.Table2, function (item) {
                        $scope.ApplicationReceived = parseInt($scope.ApplicationReceived) + parseInt(item.Total_Application_Received);
                        $scope.STTarget = parseInt($scope.STTarget) + parseInt(item.TargetK) + parseInt(item.TargetR);
                        $scope.STTargetK = parseInt($scope.STTargetK) + parseInt(item.TargetK);
                        $scope.STTargetR = parseInt($scope.STTargetR) + parseInt(item.TargetR);
                        $scope.ApprovedByPacs = parseInt($scope.ApprovedByPacs) + parseInt(item.Approved_By_Pacs);
                        $scope.ApprovedByBranch = parseInt($scope.ApprovedByBranch) + parseInt(item.Approved_By_Branch);
                        $scope.DMR_Credited = parseInt($scope.DMR_Credited) + parseInt(item.DMR_Credited_And_Loan_Disbursement);
                        $scope.Loan_Disbursement = (parseFloat($scope.Loan_Disbursement) + parseFloat(item.SANCTIONAMOUNT)).toFixed(4);
                    });
                    $scope.ApplicationReceived = (formatNumber($scope.ApplicationReceived)).split('.')[0];
                    $scope.Loan_Disbursement_Data = DataSet.Table2;
                    $scope.DistrictwiseApexParam = 'di';
                    $scope.drawVisualizationDistrictwiseApex('di');

                    //END STLOAN
                    current_progress_Status(50);

                    //START RAJFED
                    //drawVisualizationMSPRajFedQty();
                    var Arry5 = [];
                    var ArrayForTipMSP = [];
                    var GrandTotalTipMSP = parseFloat(0);

                    var ArrayForTipMSPKharif = [];
                    var GrandTotalTipMSPKharif = parseFloat(0);

                    if ($scope.MSPRAJFED == undefined || $scope.MSPRAJFED == '2') {
                        $scope.MSPRAJFED = '2';
                        if (DataSet.Table1.length > 0) {
                            Arry5.push(['Commodity', (_language == 'en-us' ? 'Target purchase (metric tons)' : 'टारगेट खरीद (मीट्रिक टन)'), (_language == 'en-us' ? 'Quantity of purchasing genes (metric tons)' : 'क्रय जींस की मात्रा(मीट्रिक टन)')]);
                            $scope.KharifTitle = _language === 'en-us' ? 'Kharif - (' + DataSet.Table1[0].FIN_YEAR_NAME + ')' : 'खरीफ - (' + DataSet.Table1[0].FIN_YEAR_NAME + ')';
                            $scope.RabiTitle = _language === 'en-us' ? 'Rabi - (' + DataSet.Table1[0].FIN_YEAR_NAME + ')' : 'रबी - (' + DataSet.Table1[0].FIN_YEAR_NAME + ')';
                        }


                        angular.forEach(DataSet.Table1, function (item) {
                            //var Cl = parseFloat(item.PurchaseTarget);
                            //var Cl = Cl * 100000;
                            if (item.FY_Id > 50)
                                Arry5.push([(item.Crop), parseInt(item.PurchaseTarget), parseInt(item.PurchaseQty)]);

                        });
                    }
                    else {
                        if (DataSet.Table1.length > 0) {
                            Arry5.push(['Commodity', (_language == 'en-us' ? 'Target purchase (in crores)' : 'टारगेट खरीद (करोडो में)'), (_language == 'en-us' ? 'Price of purchase genes (in crores)' : 'क्रय जींस का मूल्य(करोडो में)')]);
                        }
                        angular.forEach(DataSet.Table1, function (item) {
                            Arry5.push([(item.Crop), parseInt(item.PurchaseTarget), parseInt(item.PurchaseAmount)]);
                        });
                    }

                    angular.forEach(DataSet.Table1, function (item) {
                        if ((item.IsRabi === 'True' || item.IsRabi === 'true' || item.IsRabi === true || item.IsRabi === '1' || item.IsRabi === 1) && item.FY_Id == 1049) {
                            ArrayForTipMSP.push({ "Crop": item.Crop, "Value": parseInt(item.PurchaseAmount) });
                            GrandTotalTipMSP += parseFloat(parseInt(item.PurchaseAmount));
                        }
                        else {
                            ArrayForTipMSPKharif.push({ "Crop": item.Crop, "Value": parseInt(item.PurchaseAmount) });
                            GrandTotalTipMSPKharif += parseFloat(parseInt(item.PurchaseAmount));
                        }

                    });
                    $scope.ArrayForTipMSP = ArrayForTipMSP;
                    $scope.GrandTotalTipMSP = GrandTotalTipMSP;


                    $scope.ArrayForTipMSPKharif = ArrayForTipMSPKharif;
                    $scope.GrandTotalTipMSPKharif = GrandTotalTipMSPKharif;

                    var dataMSPRAJFED = google.visualization.arrayToDataTable(Arry5);
                    var TxtEn = '';
                    var TxtHi = '';
                    var x = '';

                    if ($scope.MSPRAJFED == undefined || $scope.MSPRAJFED == '2') {
                        TxtEn = 'Minimun Support Price Plan Progress(metric tons)';
                        TxtHi = 'समर्थन मूल्य योजनान्तर्गत खरीद प्रगति(मीट्रिक टन)';
                        x = (_language == 'en-us' ? '(In Metric Tons)' : 'मीट्रिक टन में');
                    }
                    else {
                        TxtEn = 'Minimun Support Price Plan Progress(in crores ₹)';
                        TxtHi = 'समर्थन मूल्य योजनान्तर्गत खरीद प्रगति(करोडो में)';
                        x = (_language == 'en-us' ? '(In Crores)' : '(करोडो में ₹)');
                    }
                    var options = {
                        title: (_language == 'en-us' ? TxtEn : TxtHi),
                        vAxis: { title: x },
                        hAxis: {
                            title: (_language == 'en-us' ? '(Commodity)' : '(जिंस)'), textStyle: {
                                fontSize: 10 // or the number you want
                            }
                        },
                        seriesType: 'bars',
                        series: { 5: { type: 'line' } }
                    };
                    var chart = new google.visualization.ColumnChart(document.getElementById('chartMSPRajFedQty'));
                    chart.draw(dataMSPRAJFED, options);

                    drawVisualizationMSPRajFedQty();


                    //END RAJFED

                    //$scope.drawVisualizationPacsGraph('TotalPacs');

                    //PM-Kisan APPLICATION STATUS
                    $scope.PMCasesVerificationParam = 'Patwari_Esign';
                    $scope.PatwariEsign = 0;
                    $scope.PatwariReject = 0;
                    $scope.TehsildarEsign = 0;
                    $scope.TehsildarReject = 0;
                    $scope.Collectorsign = 0;
                    $scope.CollectorReject = 0;
                    $scope.TotAppRec = 0;
                    angular.forEach(DataSet.Table9, function (item) {
                        $scope.PatwariEsign = parseInt($scope.PatwariEsign) + parseInt(item.PAT_ESIGN_STATUS);
                        $scope.PatwariReject = parseInt($scope.PatwariReject) + parseInt(item.PATWARI_REJ);

                        $scope.TehsildarEsign = parseInt($scope.TehsildarEsign) + parseInt(item.TEH_ESIGN_STATUS);
                        $scope.TehsildarReject = parseInt($scope.TehsildarReject) + parseInt(item.TEHSILDAR_REJ);

                        $scope.Collectorsign = parseInt($scope.Collectorsign) + parseInt(item.COLLECTOR_ESIGN_STATUS);
                        $scope.CollectorReject = parseInt($scope.CollectorReject) + parseInt(item.COLLECTER_REJ);
                        $scope.TotAppRec = parseInt($scope.TotAppRec) + parseInt(item.TotalApplication);
                    });
                    $scope.PM_Kisan_Application_Status_Data = DataSet.Table9;
                    $scope.drawVisualizationPMCasesVerification('Patwari_Esign');
                    //END
                    current_progress_Status(75);
                    $scope.FertilizerOpeningStockParam = 'Urea';
                    $scope.drawVisualizationFertilizerOpeningStock('Urea');


                    $scope.FertilizerTransactionData = DataSet.Table6;
                    var TotalTransaction = 0;
                    $scope.MonthName = '';
                    angular.forEach(DataSet.Table6, function (item) {
                        TotalTransaction = TotalTransaction + parseFloat(item.QuantitySold);
                        $scope.MonthName = '(' + (_language == 'en-us' ? item.Month_En : item.Month_Hi) + ')';
                    });

                    $scope.TotalQuantitySold = TotalTransaction.toString().split('.')[0] + '.' + (TotalTransaction.toString().split('.')[1]).substr(0, 2);

                    $scope.drawVisualizationFertilizerRetailerTransaction();

                    //START PMKISSAN DATA
                    $scope.PMkisanDataForDashboard = DataSet.Table4;
                    var TotalFarmerBenefited = 0;
                    var TotalFundDistributed = 0;
                    var IstInstallment = 0;
                    var IIstInstallment = 0;
                    var IIIrdInstallment = 0;
                    var IVthInstallment = 0;
                    var IstInstallmentAmount = 0;
                    var IIstInstallmentAmount = 0;
                    var IIIrdInstallmentAmount = 0;
                    var IVthInstallmentAmount = 0;
                    angular.forEach(DataSet.Table4, function (item) {
                        TotalFarmerBenefited = TotalFarmerBenefited + parseInt(item.Beneficiaries);
                        TotalFundDistributed = TotalFundDistributed + (parseInt(item.IstInstallmentAmount) + parseInt(item.IIstInstallmentAmount) + parseInt(item.IIIrdInstallmentAmount) + parseInt(item.IVthInstallmentAmount));

                        IstInstallment = IstInstallment + parseInt(item.IstInstallment);
                        IIstInstallment = IIstInstallment + parseInt(item.IIstInstallment);
                        IIIrdInstallment = IIIrdInstallment + parseInt(item.IIIrdInstallment);
                        IVthInstallment = IVthInstallment + parseInt(item.IVthInstallment);

                        IstInstallmentAmount = IstInstallmentAmount + parseInt(item.IstInstallmentAmount);
                        IIstInstallmentAmount = IIstInstallmentAmount + parseInt(item.IIstInstallmentAmount);
                        IIIrdInstallmentAmount = IIIrdInstallmentAmount + parseInt(item.IIIrdInstallmentAmount);
                        IVthInstallmentAmount = IVthInstallmentAmount + parseInt(item.IVthInstallmentAmount);
                    });


                    $scope.TotalFundDistributed = (formatNumber(TotalFundDistributed)).split('.')[0];
                    $scope.TotalFarmerBenefited = (formatNumber(TotalFarmerBenefited)).split('.')[0];
                    $scope.IstInstallmentAmount = (formatNumber(IstInstallmentAmount)).split('.')[0];
                    $scope.IIstInstallmentAmount = (formatNumber(IIstInstallmentAmount)).split('.')[0];
                    $scope.IstInstallment = (formatNumber(IstInstallment)).split('.')[0];
                    $scope.IIstInstallment = (formatNumber(IIstInstallment)).split('.')[0];
                    $scope.IIIrdInstallment = (formatNumber(IIIrdInstallment)).split('.')[0];
                    $scope.IVthInstallment = (formatNumber(IVthInstallment)).split('.')[0];
                    $scope.IIIrdInstallmentAmount = (formatNumber(IIIrdInstallmentAmount)).split('.')[0];
                    $scope.IVthInstallmentAmount = (formatNumber(IVthInstallmentAmount)).split('.')[0];
                    $scope.drawVisualizationDistrictwisePMKishan('fr', '3');
                    //END  PMKISSAN DATA
                    current_progress_Status(100);

                    //PECS MEMBER 
                    try {
                        $scope.PacsPacsMemberCountReportData = DataSet.Table10;
                        var TOTAL_PACS = 0;
                        var TOTAL_MEMBER = 0;
                        var TOTAL_OD = 0;
                        var TOTAL_NOD = 0;
                        angular.forEach(DataSet.Table10, function (item) {
                            TOTAL_PACS = TOTAL_PACS + parseInt(item.TOTAL_PACS);
                            TOTAL_MEMBER = TOTAL_MEMBER + parseInt(item.MEMBER_Count);
                            TOTAL_OD = TOTAL_OD + parseInt(item.OD_COUNT);
                            TOTAL_NOD = TOTAL_NOD + parseInt(item.NOD_COUNT);
                        });

                        $scope.TOTAL_PACS = TOTAL_PACS;
                        $scope.TOTAL_MEMBER = TOTAL_MEMBER;
                        $scope.TOTAL_OD = TOTAL_OD;
                        $scope.TOTAL_NOD = TOTAL_NOD;
                        $scope.drawVisualizationPacsGraph('TotalPacs');

                    }
                    catch (err) {
                        alert("Error occurred in GetPacsMemberCountReport. " + err.message);
                    }

                    //END
                }
            });
        }

        function current_progress_Status(current_progress) {
            $("#dynamic")
                .css("width", current_progress + "%")
                .attr("aria-valuenow", current_progress)
                .text(current_progress + "% Complete");
            if (current_progress == "100")
                $('#dynamic').hide();
        };

        $scope.drawVisualizationDistrictwiseApex = function (TypeAX) {
            var Count = 1;
            var ArryAX = [];
            var title = '';
            $scope.DistrictwiseApexParam = TypeAX;
            //if (_language == 'en-us')
            //    ArryAX.push(['District', 'Societies']);
            //else
            //    ArryAX.push(['District', 'Societies']);
            var TypeLocalAX = ''
            if (TypeAX == 'st') {
                TypeLocalAX = 'ST Target';
                ArryAX.push(['District', 'ST Target']);
                title = '(In Crore ₹)'
            }
            else if (TypeAX == 'di') {
                TypeLocalAX = 'Target(Kharif)';
                ArryAX.push(['District', 'Target']);
                title = '(In Crore ₹)'
            }
            else if (TypeAX == 'APP') {
                TypeLocalAX = 'Application Received';
                ArryAX.push(['District', 'Application Received']);
                title = 'Application Count'
            }
            else if (TypeAX == 'Pacs') {
                TypeLocalAX = 'Approved By Pacs';
                ArryAX.push(['District', 'Approved By Pacs']);
                title = 'Application Count'
            }
            else if (TypeAX == 'Branch') {
                TypeLocalAX = 'Approved By Branch';
                ArryAX.push(['District', 'Approved By Branch']);
                title = 'Application Count'
            }
            else if (TypeAX == 'DMR_Credited') {
                TypeLocalAX = 'DMR Credited';
                ArryAX.push(['District', 'DMR Credited']);
                title = 'DMR Credited'
            }
            else if (TypeAX == 'Disbursement') {
                TypeLocalAX = (_language == 'en-us' ? 'Disbursement' : 'संवितरण');
                ArryAX.push(['District', 'Disbursement']);
                title = (_language == 'en-us' ? 'Disbursement(in Cr. ₹)' : 'संवितरण (करोड़ ₹ में)');
            }

            var StatusLocalAX = ''

            if (TypeAX == 'st') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseInt(item.TargetK)]);
                });
            }
            else if (TypeAX == 'di') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseInt(item.TargetK)]);
                });
            }
            else if (TypeAX == 'APP') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseInt(item.Total_Application_Received)]);
                });
            }
            else if (TypeAX == 'Pacs') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseInt(item.Approved_By_Pacs)]);
                });
            }
            else if (TypeAX == 'Branch') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseInt(item.Approved_By_Branch)]);
                });
            }
            else if (TypeAX == 'DMR_Credited') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseInt(item.DMR_Credited_And_Loan_Disbursement)]);
                });
            }
            else if (TypeAX == 'Disbursement') {
                angular.forEach($scope.Loan_Disbursement_Data, function (item) {
                    ArryAX.push([(_language == 'en-us' ? item.CCBNameEn : item.CCBNameHi), parseFloat(item.SANCTIONAMOUNT)]);
                });
            }
            //});
            var data = google.visualization.arrayToDataTable(ArryAX);
            var options = {
                title: (_language == 'en-us' ? 'DCCB Wise ' + TypeLocalAX + ' ' : 'DCCB Wise ' + TypeLocalAX + ' '),
                vAxis: { title: title },
                hAxis: {
                    title: '(DCCB)', textStyle: {
                        fontSize: 10 // or the number you want
                    }
                },
                seriesType: 'bars',
                series: { 5: { type: 'line' } }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('ApexChart'));
            chart.draw(data, options);
            ////}).
            ////error(function (data1, status) {
            ////    alert("Error occur");
            ////});
        }
        $scope.drawVisualizationPacsGraph = function (Status) {
            var Count = 1;
            var title = ''
            var Arry = [];
            if (_language == 'en-us')
                Arry.push(['District', 'PACS']);
            else
                Arry.push(['District', 'PACS']);
            if (Status == 'TotalPacs') {
                vAxis = 'PACS COUNT'
                title = 'DCCB WISE TOTAL PACS COUNT';
            }
            else if (Status == 'Profilingcompleted')
                title = 'DCCB WISE PACS PROFILING COMPLETD COUNT';
            else if (Status == 'ProfilingPending')
                title = 'DCCB WISE PACS PROFILING PENDING COUNT';
            else if (Status == 'TotalPacsMember') {
                title = 'DCCB WISE PACS MEMBER COUNT';
                vAxis = 'PACS MEMBER COUNT'
            }
            else if (Status == 'TotalODMember') {
                title = 'DCCB WISE PACS OD MEMBER COUNT';
                vAxis = 'PACS MEMBER COUNT(OD)'
            }
            else if (Status == 'TotalNODMember') {
                title = 'DCCB WISE PACS NOD MEMBER COUNT';
                vAxis = 'PACS MEMBER COUNT(NOD)'
            }



            StatusLocal = Status;

            if ($scope.PacsPacsMemberCountReportData.length > 0) {
                angular.forEach($scope.PacsPacsMemberCountReportData, function (item) {
                    if (Status == 'TotalPacs')
                        Arry.push([(_language == 'en-us' ? item.DCCB_NAME : item.DCCB_NAME), parseInt(item.TOTAL_PACS)]);
                    if (Status == 'TotalPacsMember')
                        Arry.push([(_language == 'en-us' ? item.DCCB_NAME : item.DCCB_NAME), parseInt(item.MEMBER_Count)]);
                    if (Status == 'TotalODMember')
                        Arry.push([(_language == 'en-us' ? item.DCCB_NAME : item.DCCB_NAME), parseInt(item.OD_COUNT)]);
                    if (Status == 'TotalNODMember')
                        Arry.push([(_language == 'en-us' ? item.DCCB_NAME : item.DCCB_NAME), parseInt(item.NOD_COUNT)]);
                });
                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: title,
                    vAxis: { title: vAxis },
                    hAxis: { title: 'DCCB NAME' }, textStyle: {
                        fontSize: 10 // or the number you want
                    },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('PacschartContainer'));

                chart.draw(data, options);
            }

        }
        FertilizerOpeningStockReport();
       // setTimeout(function () { $scope.GetDeshBoard(-1, 0) }, 5000);

        $scope.GetNewdashboard = function () {
            var DistrictId = $("#ddlDistrict").val();
            var DivisionId = $("#ddlDivision").val();
            $(".se-pre-con").fadeIn("slow");
            $scope.GetDeshBoard(DivisionId, DistrictId).then(function () {
                $(".se-pre-con").fadeOut("slow");
            });
        }

        $scope.GetDivisionReport = function (data) {

            try {
                var _Auditable_Societies = 0;
                var _Audited_Societies_Till_Last_Month = 0;
                var _Audited_Societies_Current_Month = 0;
                var _Audited_Societies_Till_Current_Month = 0;
                var _Remaining_From_Audit = 0;

                $scope.DivisionReport = data;
                angular.forEach(data, function (j) {
                    _Auditable_Societies += parseInt(j.Auditable_Societies);
                    _Audited_Societies_Till_Last_Month += parseInt(j.Audited_Societies_Till_Last_Month);
                    _Audited_Societies_Current_Month += parseInt(j.Audited_Societies_Current_Month);
                    _Audited_Societies_Till_Current_Month += parseInt(j.Audited_Societies_Till_Current_Month);
                    _Remaining_From_Audit += parseInt(j.Remaining_From_Audit);
                    $scope.AuditableSocietiesTitle = _language == 'en-us' ? j.AuditedSocietiesTillLastEn : j.AuditedSocietiesTillLastHi;
                    $scope.AuditedSocietiesTillLastMonthTitle = (_language == 'en-us' ? j.AuditedSocietiesTillLastEn : j.AuditedSocietiesTillLastHi);
                    $scope.AuditedSocietiesCurrentMonthTitle = _language == 'en-us' ? j.AuditedSocietiesTillCurrentEn : j.AuditedSocietiesTillCurrentHi;

                });
                $scope.AuditableSocieties = _Auditable_Societies;
                $scope.AuditedSocietiesTillLastMonth = _Audited_Societies_Till_Last_Month;
                $scope.AuditedSocietiesCurrentMonth = _Audited_Societies_Current_Month;
                $scope.AuditedSocietiesTillCurrentMonth = _Audited_Societies_Till_Current_Month;
                $scope.RemainingSocietiesForAudit = _Remaining_From_Audit;
                $scope.AuditProgrssDivisionWise('AuditableSocieties');
                $scope.AuditProgrssLable = 'Auditable Societies';
            }
            catch (err) {
                $scope.DivisionReport = [];
                alert("Error occurred in GetDivisionReport. " + err.message);
            }

        }
        $scope.AuditProgrssDivisionWise = function (Status) {


            if ($scope.DivisionReport.length > 0) {
                var DivData = [];
                angular.forEach($scope.DivisionReport, function (j) {
                    if (Status == 'AuditableSocieties') {
                        DivData.push(
                            {
                                "DivisionName": j.DIVISION_ENG,
                                "Result": j.Auditable_Societies
                            });
                        $scope.AuditProgrssLable = 'Auditable Societies';
                    }
                    else if (Status == 'AuditedSocietiesTillLastMonth') {
                        DivData.push(
                            {
                                "DivisionName": j.DIVISION_ENG,
                                "Result": j.Audited_Societies_Till_Last_Month
                            });

                        $scope.AuditProgrssLable = 'Audited Societies Till May';
                    }
                    else if (Status == 'AuditedSocietiesCurrentMonth') {
                        DivData.push(
                            {
                                "DivisionName": j.DIVISION_ENG,
                                "Result": j.Audited_Societies_Current_Month
                            });
                        $scope.AuditProgrssLable = 'Audited Societies June';
                    }
                    else if (Status == 'AuditedSocietiesTillCurrentMonth') {
                        DivData.push(
                            {
                                "DivisionName": j.DIVISION_ENG,
                                "Result": j.Audited_Societies_Till_Current_Month
                            });
                        $scope.AuditProgrssLable = 'Audited Societies Till June';
                    }
                    else if (Status == 'RemainingSocietiesForAudit') {
                        DivData.push(
                            {
                                "DivisionName": j.DIVISION_ENG,
                                "Result": j.Remaining_From_Audit
                            });
                        $scope.AuditProgrssLable = 'Remaining Societies For Audit';
                    }

                });
                $scope.DivisionReportDataCus = DivData;
            }

        }
        var year = (new Date()).getFullYear();
        var month = (new Date().getMonth() + 1);

        var currentYear = month < 4 ? (year - 1) : year;

        var currentY = currentYear + "-" + (year + 1).toString().substr(2, 3);

        var prevY = currentY.substr(0, 4) - 1 + "-" + currentY.substr(0, 4).toString().substr(2, 3);

        $scope.PreFY = prevY;
        $scope.FinancialYear = [{ Year: currentYear, Text: currentY }, { Year: currentY.substr(0, 4) - 1, Text: prevY }];


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

                            //$scope.OfflineTotalAppli += item.OfflineTotalAppli;
                            // $scope.OfflineRegiAppli += item.OfflineRegiAppli;
                            //$scope.OfflineRejeAppli += item.OfflineRejeAppli;
                            //$scope.OnlineTotalAppli += item.OnlineTotalAppli;
                            //$scope.OnlineRegiAppli += item.OnlineRegiAppli;
                            //$scope.OnlineRejeAppli += item.OnlineRejeAppli;
                            //$scope.OnlinePartialAppli += item.OnlinePartialAppli;
                            //$scope.OfflinePartialAppli += item.OfflinePartialAppli;
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





                        Responsedata1 = [];
                        // Graph 1 data
                        Responsedata1.push(['Task', '', { role: 'annotation' }]);
                        Responsedata1.push([$scope.translation.RegisteredSociety, parseInt($scope.OfflineRegiAppli, 10), parseInt($scope.OfflineRegiAppli, 10)]);
                        Responsedata1.push([$scope.translation.UnderProcess, parseInt($scope.OfflineUnderProAppli, 10), parseInt($scope.OfflineUnderProAppli, 10)]);
                        Responsedata1.push([$scope.translation.RejectedSociety, parseInt($scope.OfflineRejeAppli, 10), parseInt($scope.OfflineRejeAppli, 10)]);
                        Responsedata1.push([$scope.translation.PartialSubmit, parseInt($scope.OfflinePartialAppli, 10), parseInt($scope.OfflinePartialAppli, 10)]);

                        $scope.GetGraph('pie', _language == 'en-us' ? 'Offline Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart2');

                        Responsedata1 = [];
                        // Graph 1 data
                        Responsedata1.push(['Task', '', { role: 'annotation' }]);
                        Responsedata1.push([$scope.translation.RegisteredSociety, parseInt($scope.OnlineRegiAppli, 10), parseInt($scope.OnlineRegiAppli, 10)]);
                        Responsedata1.push([$scope.translation.UnderProcess, parseInt($scope.OnlineUnderProAppli, 10), parseInt($scope.OnlineUnderProAppli, 10)]);
                        Responsedata1.push([$scope.translation.RejectedSociety, parseInt($scope.OnlineRejeAppli, 10), parseInt($scope.OnlineRejeAppli, 10)]);
                        Responsedata1.push([$scope.translation.PartialSubmit, parseInt($scope.OnlinePartialAppli, 10), parseInt($scope.OnlinePartialAppli, 10)]);

                        $scope.GetGraph('pie', _language == 'en-us' ? 'Online  Society Status Graph' : 'ग्राफ', Responsedata1, 'divChart3');

                        Responsedata1 = [];
                        // Graph 1 data
                        Responsedata1.push(['Task', '', { role: 'annotation' }]);
                        Responsedata1.push([$scope.translation.Reject, parseInt($scope.GetActionIdCCSComplaintReport[0].Reject, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Reject, 10)]);
                        Responsedata1.push([$scope.translation.dispose, parseInt($scope.GetActionIdCCSComplaintReport[0].Dispose, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Dispose, 10)]);
                        Responsedata1.push([$scope.translation.Hold, parseInt($scope.GetActionIdCCSComplaintReport[0].Hold, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].Hold, 10)]);
                        Responsedata1.push([$scope.translation.NextHearing, parseInt($scope.GetActionIdCCSComplaintReport[0].NextHearing, 10), parseInt($scope.GetActionIdCCSComplaintReport[0].NextHearing, 10)]);

                        $scope.GetGraph('pie', _language == 'en-us' ? 'CCS Compaint Status Graph' : 'ग्राफ', Responsedata1, 'divChart4');


                        Responsedata1 = [];
                        // Graph 1 data
                        Responsedata1.push(['Task', '', { role: 'annotation' }]);
                        $scope.ComplainUnderSection.forEach(function (item, index) {
                            Responsedata1.push([_language == 'en-us' ? item.StatusName : item.StatusNameHi, parseInt(item.Total, 10), parseInt(item.Total, 10)]);

                            if (index == ($scope.ComplainUnderSection.length - 1)) {
                                $scope.GetGraph('pie', _language == 'en-us' ? 'Complain Under Section Status Graph' : 'ग्राफ', Responsedata1, 'divChart5');
                            }

                        })
                        setTimeout(function () {
                            $scope.GetGraph('pie', _language == 'en-us' ? 'Complain Under Section Status Graph' : 'ग्राफ', Responsedata1, 'divChart5');
                        }, 5000);




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

        // END : Code for Language label setting
        $scope.ChartData = function () {
            $http.get(SiteUrl + "/MIS/GetChartData").
                success(function (data, status) {
                    var P = [];
                    var Col = [];
                    var YDistinctTitle = [];
                    var XDistinctTitle = [];
                    angular.forEach(data, function (item) {
                        YDistinctTitle.push(item.TypeameEng)
                    });

                    angular.forEach(data, function (item) {
                        XDistinctTitle.push(item.TypeCount)
                    });


                    angular.forEach(XDistinctTitle, function (itemData) {
                        var c = [];
                        c.push({ "v": itemData });

                        angular.forEach(YDistinctTitle, function (Yitem) {
                            c.push({ "v": Yitem.TypeCount });
                        });
                        P.push({ "c": c });
                    });

                    Col.push({ "label": "", "type": "string" });

                    angular.forEach(YDistinctTitle, function (item, i) {

                        Col.push({ "label": item, "type": "number" });
                    });




                    var myChartObject = {};
                    myChartObject.type = "google.charts.Bar";
                    myChartObject.displayed = false;
                    myChartObject.data = {
                        "cols": Col,
                        "rows": P
                    };

                    myChartObject.options = {
                        'title': 'DashBoard',
                        "isStacked": "true",
                        "fill": 20,
                        "displayExactValues": true,
                        "vAxis": {
                            "title": "Sales unit",
                            "gridlines": {
                                "count": 10
                            }
                        },
                        "hAxis": {
                            "title": "Date"
                        }
                    };
                    $scope.myChart = myChartObject;



                    var MyData = [];
                    angular.forEach(YDistinctTitle, function (itemData) {
                        var obj = {};
                        obj.YTitle = itemData;
                        YData = [];

                        angular.forEach(XDistinctTitle, function (Yitem) {
                            YData.push(Yitem);
                        });
                        obj.YData = YData;
                        MyData.push(obj);
                    });


                    $scope.Headers = XDistinctTitle;
                    $scope.Rows = MyData;

                }).
                error(function (data, status) {
                    alert("Error while fetching Department list.");
                    $scope.showLoader = false;
                });
            $(".se-pre-con").fadeOut("slow");

        };
        $scope.CertificateList = [];

        $scope.CertificateList = [];
        $scope.GetCertificate = function () {
            if ($("#txtSearch").val() == '') {
                ShowOkMessage("", "Please Enter Application Number", function () { });
            } else {
                $(".blockUI").fadeIn("slow");
                return $http.post("/VerifyCertificate/GetCertificate", JSON.stringify({ ApplicationNo: $("#txtSearch").val() })).
                    success(function (data, status) {

                        $(".blockUI").fadeOut("slow");
                        if (JSON.parse(data.DataList).length <= 0) {
                            $scope.CertificateList = [];
                            ShowOkMessage("", "No Record Found", function () { });
                        } else {
                            $("#modelSocietyDetails").modal("show");
                            $scope.CertificateList = JSON.parse(data.DataList);
                        }
                    }).
                    error(function (data, status) {
                        $scope.CertificateList = [];
                        ;
                    });
            }
        }



        $scope.Societyapplicationview = function (SocietyIDInc) {
            if (SocietyIDInc != '') {
                window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + SocietyIDInc + "~_Citizen_SocietyProfile~undefined";
            }

        }
        //$scope.GetSocietyBasicDetailList = function () {
        //    $(".blockUI").fadeIn("slow");
        //    $http.post(SiteUrl + "/Society/GetCCSocietyList", { "SearchType": $scope.SearchType, "DistrictId": $scope.DISTRICT_ID }).
        //        success(function (data, status) {
        //            $scope.SocietyBasicDetailList = data;
        //            $(".blockUI").fadeOut("slow");
        //        }).
        //        error(function (data, status) {
        //            alert("Error while fetching list");
        //            $(".blockUI").fadeOut("slow");
        //        });
        //    $(".blockUI").fadeOut("slow");
        //};

        $scope.getTranslation();
        //$scope.ChartData();
        //$scope.GetAlldeshdata();
       // $scope.GetDivision();
    });