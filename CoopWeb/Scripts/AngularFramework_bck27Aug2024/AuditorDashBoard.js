var MasterMod = angular.module('Auditormod', ['zingchart-angularjs']);
MasterMod.controller("AuditorCont", function ($scope, $http) {
    
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.IsGraphSmall = 0;
    $scope.IsPMGraphSmall = 0;
    $scope.IsTGGraphSmall = 0;
    $scope.TilamSangQty = true;
    $scope.TilamSangPrice = false;
    DashboardDataList = new Array();

    function DashboardData() {
        this.TOTAL = 0,
            this.COMPLETED = 0,
            this.REAMINING = 0
    }



    $scope.GetGraph = function (chartType, title, Responsedata, divId) {

        
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);



    }

    //START : Act Wise Society data
    $scope.drawVisualizationDistrictwiseSociety = function (actId) {
        $(".se-pre-con").fadeIn("slow");
      
        google.charts.load('current', { 'packages': ['corechart'] });
        var Count = 1;
        var Arry = [];
        if (_language === 'en-us')
            Arry.push(['District', 'Societies']);
        else
            Arry.push(['जिला', 'सोसायटी']);

        $http.post(SiteUrl + "/Inspection/GetAuditorDashBoardData", { "ActionId": 2, "ActId": actId }).
            success(function (data1, status) {
                angular.forEach(data1.Table3, function (j, k) {

                    Arry.push([_language === 'en-us' ? j.DISTRICT_ENG : j.DISTRICT_MANGAL, parseInt(j.SocietyCount)]);
                });



                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language === 'en-us' ? 'District Wise  Societies' : 'जिलेवार सोसायटीज'),
                    vAxis: { title: _language === 'en-us' ? '(Count)' : '(संख्या )' },
                    hAxis: { title: _language === 'en-us' ? 'District' : 'जिला' }, textStyle: {
                        fontSize: 10 // or the number you want
                    },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('chartContainer'));

                chart.draw(data, options);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                alert("Error occur");
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    }
    //END : Act Wise Society Data




   

    //START : Expand Graph
    $scope.ExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        if (Small === 1) {
            //scope.drawVisualizationDistrictwiseSociety($scope.ActId);
        }
    };
    //END : Expand Graph

    function drawChart() {
        var data = google.visualization.arrayToDataTable(Responsedata);
        // console.log(data);

        // Optional; add a title and set the width and height of the chart
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



    $scope.GetAlldeshdata = function (FIN_YEAR_ID) {
        $http.post(SiteUrl + "/Inspection/GetAuditorDashBoardData", { "ActionId": 1 }).
            success(function (data, status) {

                console.log(data);

                // Society Data
                var _SocietyCount = 0;
                angular.forEach(data.Table, function (j, k) {
                    _SocietyCount += j.SocietyCount;
                });

                $scope.TotalSocieties = _SocietyCount;
                $scope.ActWiseSocieties = data.Table;


                //Auditable Societies Data
                var _TotalAuditableSocieties = 0;
                angular.forEach(data.Table1, function (j, k) {
                    _TotalAuditableSocieties += j.NoOfSocieties;
                });

                $scope.TotalAuditableSocieties = _TotalAuditableSocieties;
                $scope.AuditableSocietyStatusWise = data.Table1;

                // Auditors data
                var _AuditorCount = 0;
                angular.forEach(data.Table2, function (j, k) {
                    _AuditorCount += j.AuditorCount;
                });
                $scope.TotalAuditors = _AuditorCount;
                $scope.TypeWiseAuditor = data.Table2;

                // Housing Board Data
                $scope.TotalHousingSocieties = data.Table4.length > 0 ? data.Table4.Count : 0;
                $scope.FinancialYearWiseHousingSocieties = data.Table3;


                $scope.SocietyAuditUploadHousing = data.Table6;
                $scope.SocietyAuditUploadNonHousing = data.Table7;


                if (data.Table5.length > 0) {
                    $scope.TotalAudutableSocieties = data.Table5[0].TotalSocieties;
                    $scope.AuditUpToLastMonth = data.Table5[0].UptoLastMonth;
                    $scope.AuditInThisMonth = data.Table5[0].InThisMonth;
                    $scope.AuditInProgress = data.Table5[0].Running;
                    $scope.Remaining = data.Table5[0].TotalSocieties - (data.Table5[0].UptoLastMonth + data.Table5[0].InThisMonth + data.Table5[0].Running);
                }
                else {
                    $scope.TotalAudutableSocieties = 0;
                    $scope.AuditUpToLastMonth = 0;
                    $scope.AuditInThisMonth = 0;
                    $scope.TotalAudit = 0;
                    $scope.Remaining = 0;
                }



                $scope.Table3 = data.Table3;
               
                $scope.drawVisualizationDistrictwiseSociety(0);

                //try {
                    //$scope.Deshboard = data;
                    //var DD = new DashboardData();
                    //DD.TOTAL = ($scope.Deshboard.filter(x => x.TotalCount == 'A')).length;
                    //DD.REAMINING = ($scope.Deshboard.filter(x => x.IsPermanentSave == 'False')).length;
                    //DD.COMPLETED = ($scope.Deshboard.filter(x => x.IsPermanentSave == 'True')).length;                    
                    //DashboardDataList.push(DD);
                    //$scope.TOTAL = DD.TOTAL;
                    //$scope.REAMINING = DD.REAMINING;
                    //$scope.COMPLETED = DD.COMPLETED;
                    //$scope.myJson = {
                    //    type: "pie",
                    //    title: {
                    //        textAlign: 'center',
                    //        text: ""
                    //    },
                    //    plot: {
                    //        slice: 50 //to make a donut
                    //    },
                    //    series: [
                    //        {
                    //            values: [DashboardDataList[0].TOTAL],
                    //            text: "TOTAL"

                    //        }, {
                    //            values: [DashboardDataList[0].REAMINING],
                    //            text: "REAMINING"

                    //        }, {
                    //            values: [DashboardDataList[0].COMPLETED],
                    //            text: "COMPLETED"
                    //        }]
                    //};


                    //var Responsedata1 = [];
                    //// Graph 1 data
                    //Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    //Responsedata1.push([_language == 'en-us' ? 'TOTAL' : 'कुल', parseInt(DashboardDataList[0].TOTAL, 10), parseInt(DashboardDataList[0].TOTAL, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'COMPLETED' : 'पूर्ण', parseInt(DashboardDataList[0].COMPLETED, 10), parseInt(DashboardDataList[0].COMPLETED, 10)]);
                    //Responsedata1.push([_language == 'en-us' ? 'REAMINING' : 'शेष', parseInt(DashboardDataList[0].REAMINING, 10), parseInt(DashboardDataList[0].REAMINING, 10)]);

                    //$scope.GetGraph('pie', _language == 'en-us' ? 'Audit Status Graph' : 'ग्राफ', Responsedata1, 'StatusChart');
                //}
                //catch (err) {
                //    alert("Error occurred in DeshboardData. " + err.message);
                //}



            }).
            error(function (data1, status) {

            });
    }


    $scope.GetDivisionReport = function () {
        $http.get(SiteUrl + "/User/GetDivisionReport?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId).
            success(function (data, status) {
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
            }).
            error(function (data1, status) {

            });
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
    function GetDivisionReport_1() {
        $.ajax({
            url: SiteUrl + "/User/GetDivisionReport?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId,
            async: false,
            dataType: 'json',
            success: function (data) {

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

                        $scope.AuditableSocietiesTitle = _language == 'en-us' ? j.AuditedSocietiesTillLastEn : j.AuditedSocietiesTillLastHi;
                        $scope.AuditedSocietiesTillLastMonthTitle = (_language == 'en-us' ? j.AuditedSocietiesTillLastEn : j.AuditedSocietiesTillLastHi);
                        $scope.AuditedSocietiesCurrentMonthTitle = _language == 'en-us' ? j.AuditedSocietiesEn : j.AuditedSocietiesHi;
                        $scope.AuditedSocietiesTillCurrentMonthTitle = _language == 'en-us' ? j.AuditedSocietiesTillCurrentEn : j.AuditedSocietiesTillCurrentHi;



                        _Remaining_From_Audit += parseInt(j.Remaining_From_Audit);
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
        });
    }

    GetDivisionReport_1();


    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetAlldeshdata(48);

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

            $scope.ClearUnitForm = function () {
                $scope.deshboaddata = null;
            };
        }

        $scope.FinancialYearList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data.filter(x => x.FIN_YEAR_END != data[0].FIN_YEAR_END);
                //$scope.AuditorInspection.FinancialYear = data[0].FIN_YEAR_MASTR_ID;
                //$scope.drawInspectionVisualization(data[0].FIN_YEAR_MASTR_ID);
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });

        //$scope.drawVisualizationDistrictwiseSociety(0);
        $(".se-pre-con").fadeOut("slow");

    };
    $scope.OnPageLoad();

});