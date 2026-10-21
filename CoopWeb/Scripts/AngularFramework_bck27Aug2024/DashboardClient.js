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

        $scope.GetSocietyDistrictList = function () {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegistration.DIVISION_ID).
                success(function (data, status) {
                    $scope.DistrictList = data;
                }).
                error(function (data, status) {
                    $scope.DistrictList = [];
                });
        };

        //END

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

                        $scope.PRIMARY = 0;
                        $scope.CENTRAL = 0;
                        $scope.APEX = 0;
                        $scope.DAIRY = 0;
                        $scope.OTHERS = 0;

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

                            if (item.SocietyTypeId == 7) {
                                $scope.PRIMARY += item.TotSociety;
                            }
                           else if (item.SocietyTypeId == 10) {
                                $scope.DAIRY += item.TotSociety;
                            }
                            else if (item.SocietyTypeId == 8) {
                                $scope.CENTRAL += item.TotSociety;
                            }
                            else if (item.SocietyTypeId == 9) {
                                $scope.APEX += item.TotSociety;
                            }
                            else {
                                $scope.OTHERS += item.TotSociety;
                            }



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
        $scope.GetCertificate = function () {
            if ($("#txtSearch").val() == '') {
                ShowOkMessage("", "Please Enter Application Number", function () { });

            }
            else {
                return $http.post("/VerifyCertificate/GetCertificate", JSON.stringify({ ApplicationNo: $("#txtSearch").val() })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        if (JSON.parse(data.DataList).length <= 0) {
                            $scope.CertificateList = [];
                            ShowOkMessage("", "No Record Found", function () { });
                        } else {
                            $scope.CertificateList = JSON.parse(data.DataList);
                        }
                    }).
                    error(function (data, status) {
                        $scope.CertificateList = [];

                    });
            }
        }

        $scope.CertificateList = [];
        $scope.GetCertificate = function () {
            if ($("#txtSearch").val() == '') {
                ShowOkMessage("", "Please Enter Application Number", function () { });
            } else {
                return $http.post("/VerifyCertificate/GetCertificate", JSON.stringify({ ApplicationNo: $("#txtSearch").val() })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        if (JSON.parse(data.DataList).length <= 0) {
                            $scope.CertificateList = [];
                            ShowOkMessage("", "No Record Found", function () { });
                        } else {
                            $scope.CertificateList = JSON.parse(data.DataList);
                        }
                    }).
                    error(function (data, status) {
                        $scope.CertificateList = [];
                        ;
                    });
            }
        }

        $scope.getTranslation();
        //$scope.ChartData();
        $scope.GetAlldeshdata();
        $scope.GetDivision();
    });