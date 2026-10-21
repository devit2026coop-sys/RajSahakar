
angular.module('myApp', ['googlechart'])
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
        $scope.GetDashboardCountSocietyAuditprogrss();


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


        $scope.GetComplaintReportCounts();

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
           
            
           return $http.post(SiteUrl + "/user/GetNewDashboardData?DivisionId=" + DivisionId + "&DistrictId=" + DistrictId +'&FY=0').
               success(function (data, status) {
                  
                    var DataResult = JSON.parse(data.DataList);
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

                    $scope.SocietyStatustaskpenalOfflineTotal = parseInt($scope.OfflineRegiAppli1) + parseInt($scope.OfflineUnderProAppli1) + parseInt($scope.OfflineRejeAppli1) + parseInt( $scope.OfflinePartialAppli1);
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
        $(".se-pre-con").fadeIn("slow");
        $scope.GetDeshBoard(0, 0).then(function () {
            $(".se-pre-con").fadeOut("slow");
        });

        setTimeout(function () { $scope.GetDeshBoard(-1, 0) }, 5000);

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

                            if (index == ($scope.ComplainUnderSection.length-1)) {
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
        $scope.GetDivision();
    });