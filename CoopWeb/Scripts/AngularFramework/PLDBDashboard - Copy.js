
var MasterMod = angular.module('SocietyIndexmod', ['zingchart-angularjs', 'angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyIndexCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.ElectionProgress = [];
    $scope.showOrderDetails = false;
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
    $scope.RegistrationModel = [];
    $scope.LoanApplicationModel = [];
    $scope.ShowOrderDetails = function (id) {
        $(".blockUI").fadeIn("slow");
        // alert(id);
        $http.get(SiteUrl + "/Election/GetElectionSocietyDetails?ElectionMasterId=" + id).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                
                try {
                    $scope.ElectionSocietyDetails = [];
                    $scope.ElectionSocietyDetails = data;
                    $scope.showOrderDetails = true;
                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }
            }).
            error(function (data1, status) {
                $(".blockUI").fadeOut("slow");
            });


    }
    $scope.GetGraph = function (chartType, title, Responsedata, divId, innerWidth) {

        function drawChart() {
            var data = google.visualization.arrayToDataTable(Responsedata);
            console.log(data);

            // Optional; add a title and set the width and height of the chart
            if (chartType == 'pie') {
                var options = { 'title': title, 'width': innerWidth, 'height': 265 };
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

    $scope.GetAlldeshdata = function () {
        // alert(SiteUrl + "/Election/GetElectionDashboard");
        return $http.get(SiteUrl + "/PldbLoan/GetPLDBDashboard").
            success(function (data, status) {
                
               
                   // $scope.ElectionProgress = [];
                   // $scope.ElectionProgress = data[0];
                    $scope.RegistrationModel = [];
                    $scope.LoanApplicationModel = [];
                    $scope.RegistrationModel = data.filter(x => x.Type === "1");
                    $scope.LoanApplicationModel = data.filter(x => x.Type === "2");

                    var Responsedata1 = [];
                    var Responsedata2 = [];
                    var RegistrationModel = $scope.RegistrationModel.filter(x => x.Order === "2");
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    RegistrationModel.forEach(function (itm, index) {
                        Responsedata1.push([_language == 'en-us' ? itm.Status_En : itm.Status_Hi, parseInt(itm.TotalCount, 0), parseInt(itm.TotalCount, 0)]);
                    });   


                    var LoanApplicationModel = [];
                    var LoanApplicationModel = $scope.LoanApplicationModel.filter(x => x.Order === "4");
                    Responsedata2.push(['Task', '', { role: 'annotation' }]);
                    LoanApplicationModel.forEach(function (itm, index) {
                        Responsedata2.push([_language == 'en-us' ? itm.Status_En : itm.Status_Hi, parseInt(itm.TotalCount, 0), parseInt(itm.TotalCount, 0)]);
                    }); 

                try {
                    var innerWidth1 = $("#dvRegistrationgraph").width();
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Registration Graph' : 'पंजीकरण का ग्राफ', Responsedata1, 'divChart1', innerWidth1);  
                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }


                try {                   
                    var innerWidth2 = $("#dvLoanApplication").width();
                    $scope.GetGraph('pie', _language == 'en-us' ? 'Loan Application Graph' : 'ऋण आवेदन का ग्राफ', Responsedata2, 'divChart2', innerWidth2);
                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }



                return true;
            }).
            error(function (data1, status) {

                return true;

            });
        // return true;
    }

    $scope.GetElectionOrders = function () {

        return $http.get(SiteUrl + "/Election/GetElectionOrdersForDashboard").
            success(function (data, status) {

                
                try {
                    $scope.ElectionOrders = [];
                    $scope.ElectionOrders = data;

                }
                catch (err) {
                    //alert("Error occurred in DeshboardData. " + err.message);
                }
                return true;
            }).
            error(function (data1, status) {
                return true;
            });

    }


    $scope.OnPageLoad = function () {
        $(".blockUI").fadeIn("slow");
        $scope.GetAlldeshdata().then(function () {
            $scope.GetElectionOrders().then(function () {
                $(".blockUI").fadeOut("slow");
            });
        });

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

        //$scope.Department = [];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

        //    success(function (data, status) {

        //        $scope.Department = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Department = [];
        //    });

        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
        //    success(function (data, status) {
        //        //alert(JSON.stringify(data));
        //        $scope.FinancialYearList = data.filter(x => x.FIN_YEAR_END != data[0].FIN_YEAR_END);
        //    }).
        //    error(function (data, status) {
        //        $scope.FinancialYearList = [];
        //    });


    };

    $scope.OnPageLoad();

});






