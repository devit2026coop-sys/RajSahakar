var MasterMod = angular.module('RajfedDashboardmod', ['zingchart-angularjs']);
MasterMod.controller("RajfedDashboardCont", function ($scope, $http) {

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
                //var options = {
                //    title: title,
                //    width: 350,
                //    height: 260,
                //    bar: { groupWidth: "30%" },
                //    hAxis: { title: "", direction: -1, slantedText: true, slantedTextAngle: 20, slantedTextWrap: true, textStyle: { fontSize: 10} },
                //    legend: { position: "none" },
                //    annotations: {
                //        alwaysOutside: true,
                //        textStyle: {
                //            bold: true,
                //            color: '#000000',
                //        },
                //    },
                //};
                var options = {
                    title: (_language == 'en-us' ? 'Society Graph View' : 'सोसायटी ग्राफ देखें'),
                    vAxis: { title: (_language == 'en-us' ? 'Total KVSS' : 'कुल KVSS') },
                    hAxis: { title: (_language == 'en-us' ? 'District' : 'जिला') },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };
                var chart = new google.visualization.ColumnChart(document.getElementById(divId));
                chart.draw(data, options);

            }

        }
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        $scope.AuditorInspection["FinancialYear"] = $scope.FinancialYearList[0].FIN_YEAR_MASTR_ID;
    };


    $scope.GetAlldeshdata = function () {
        $http.get(SiteUrl + "/Citizen/GetRajfedDashboardData").
            success(function (data, status) {
                //$scope.CaseRegDetailsData = '';
                //$scope.CaseRegDetailsData = data.Table;
                //$scope.CaseRegDetailsData = $scope.CaseRegDetailsData[0];
                //$scope.CaseRegApplntDetailsData = data.Table1;
                try {
                    //$scope.Deshboard = data.Table;
                    var TotSociety = data.Table;
                    // alert(JSON.stringify(TotSociety));
                    $scope.TotSocietyAll = 0;
                    $scope.Responsedata1 = [];
                    $scope.Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    angular.forEach(TotSociety, function (item) {
                        $scope.TotSocietyAll += item.TotSocietyDist;
                        $scope.Responsedata1.push([_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_ENG, parseInt(item.TotSocietyDist, 10), parseInt(item.TotSocietyDist, 10)]);
                    });
                    // alert($scope.TotSocietyAll);
                    //var DD = new DashboardData();
                    //DD.PARTIALSUBMITTED = ($scope.Deshboard.filter(x => x.StatusId == 1)).length;
                    //DD.SUBMITTED = ($scope.Deshboard.filter(x => x.StatusId == 2)).length;
                    //DD.CERTIFICATEISSUED = 0;
                    //DD.APPROVED = ($scope.Deshboard.filter(x => x.StatusId == 26 || x.StatusId == 17)).length;

                    //DD.PAYMENTREQUEST = ($scope.Deshboard.filter(x => x.StatusId == 33)).length;
                    //DD.REJECTED = ($scope.Deshboard.filter(x => x.StatusId == 6 || x.StatusId == 13 || x.StatusId == 16 || x.StatusId == 18 || x.StatusId == 21 || x.StatusId == 29 || x.StatusId == 34 || x.StatusId == 1044)).length;
                    //DD.TOTAL = (DD.PARTIALSUBMITTED + DD.APPROVED + DD.CERTIFICATEISSUED + DD.PAYMENTREQUEST + DD.REJECTED);
                    //DD.UNDERPROCESS = ($scope.Deshboard).length;
                    //DD.UNDERPROCESS = (DD.UNDERPROCESS - DD.TOTAL);
                    //DD.TOTAL = DD.TOTAL + DD.UNDERPROCESS;
                    //DD.ACT1958 = ($scope.Deshboard.filter(x => x.ACTID == '1')).length;
                    //DD.ACT2001 = ($scope.Deshboard.filter(x => x.ACTID == '3')).length;
                    //DD.ACT2005 = ($scope.Deshboard.filter(x => x.ACTID == '7')).length;
                    //DashboardDataList.push(DD);


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

                    //$scope.TOTAL = DashboardDataList[0].TOTAL;
                    //$scope.SUBMITTED = DashboardDataList[0].SUBMITTED;
                    //$scope.APPROVED = DashboardDataList[0].APPROVED;
                    //$scope.RegisteredSociety = DashboardDataList[0].CERTIFICATEISSUED;
                    //$scope.PartialSubmit = DashboardDataList[0].PARTIALSUBMITTED;
                    //$scope.PandingforPayment = DashboardDataList[0].PAYMENTREQUEST;
                    //$scope.REJECTED = DashboardDataList[0].REJECTED;
                    //$scope.UNDERPROCESS = DashboardDataList[0].UNDERPROCESS;

                    //$scope.ACT1958 = DashboardDataList[0].ACT1958;
                    //$scope.ACT2001 = DashboardDataList[0].ACT2001;
                    //$scope.ACT2005 = DashboardDataList[0].ACT2005;
                    //$scope.TOTAL = ($scope.ACT1958 + $scope.ACT2001 + $scope.ACT2005);

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

                    $scope.OnlineTotalAppli = 0;
                    $scope.OfflineTotalAppli = 0;
                    $scope.OnlineRegiAppli = 0;
                    $scope.OfflineRegiAppli = 0;
                    $scope.OnlineRejeAppli = 0;
                    $scope.OfflineRejeAppli = 0;

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

                    //alert(JSON.stringify(data.Table3));
                    if (data.Table3.length > 0) {
                        $scope.TotalAudutableSocieties = data.Table3[0].TotalSocieties;
                        $scope.AuditUpToLastMonth = data.Table3[0].UptoLastMonth;
                        $scope.AuditInThisMonth = data.Table3[0].InThisMonth;
                        $scope.AuditInProgress = data.Table3[0].Running;
                        $scope.Remaining = data.Table3[0].TotalSocieties - (data.Table3[0].UptoLastMonth + data.Table3[0].InThisMonth + data.Table3[0].Running);
                    }
                    else {
                        $scope.TotalAudutableSocieties = 0;
                        $scope.AuditUpToLastMonth = 0;
                        $scope.AuditInThisMonth = 0;
                        $scope.TotalAudit = 0;
                        $scope.Remaining = 0;
                    }

                    // Graph 1 data
                    //Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    //Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt(DashboardDataList[0].SUBMITTED, 10), parseInt(DashboardDataList[0].SUBMITTED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt(DashboardDataList[0].APPROVED, 10), parseInt(DashboardDataList[0].APPROVED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10), parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt(DashboardDataList[0].PAYMENTREQUEST, 10), parseInt(DashboardDataList[0].PAYMENTREQUEST, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt(DashboardDataList[0].REJECTED, 10), parseInt(DashboardDataList[0].REJECTED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt(DashboardDataList[0].UNDERPROCESS, 10), parseInt(DashboardDataList[0].UNDERPROCESS, 10)]);
                    //console.log(Responsedata1);


                    $scope.GetGraph('bar', _language == 'en-us' ? 'Society Graph' : 'ग्राफ', $scope.Responsedata1, 'divChart1');
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
    };

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

        $scope.DashboardData(0, 0, 0);

        $(".se-pre-con").fadeOut("slow");
    };

    $scope.PMKisanExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        if (Small == 1) {
            drawVisualizationApexData();
        }

    };


    function drawVisualizationApexData() {
        var data = google.visualization.arrayToDataTable($scope.Responsedata1);
        var options = {
            title: (_language == 'en-us' ? 'Society Graph View' : 'सोसायटी ग्राफ देखें'),
            vAxis: { title: (_language == 'en-us' ? 'Total KVSS' : 'कुल KVSS') },
            hAxis: {
                title: (_language == 'en-us' ? 'District' : 'जिला')
            },
            seriesType: 'bars',
            series: { 5: { type: 'line' } }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById('divChart1'));
        chart.draw(data, options);

    }
    $scope.TableData = [];
    $scope.DashboardData = function (ActId = 0, SocietyTypeId = 0, ClassId = 0) {
        $http.post(SiteUrl + "/Citizen/CitizenDashboardDataWithDepartmentUser", { "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId) }).
            success(function (data, status) {
                $scope.TableData = [];
                $scope.TableData = data;


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
            }).
            error(function (data, status) {
                $scope.TableData = [];
            });

    };

    $scope.ShowDetails = function (SocietyIdENC) {
        if (SocietyIdENC != '') {
            window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + SocietyIdENC + "~_Society_RajfedDashboard~undefined";
        }
    };
    $scope.OnPageLoad();

});






