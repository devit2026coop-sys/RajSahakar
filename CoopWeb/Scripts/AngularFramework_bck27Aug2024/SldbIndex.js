
var MasterMod = angular.module('Indexmod', ['zingchart-angularjs']);
MasterMod.controller("IndexCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.IsGraphSmall = 0;
    $scope.IsGraphFertilizerSmall = 0;
    $scope.IsPMGraphSmall = 0;
    $scope.IsTGGraphSmall = 0;
    $scope.TilamSangQty = true;
    $scope.TilamSangPrice = false;
    $scope.AuditYearWise = [];
    $scope.UserLevelWiseCount = [];

    var myVar;

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
    $scope.MSPTS = 2;
    $scope.QtyPrice = function (QtyPrice) {
        if ($scope.IsTGGraphSmall == 1) {
            drawVisualizationTilamSangQty(35, QtyPrice);
        }
        else {
            drawVisualizationTilamSangQty(4, QtyPrice);
        }
    }

    function GetGraph(chartType, title, Responsedata, divId) {

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
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        //Getlwadata();
        $scope.GetUserCountForDashBoard();
    }




    function myTimer() {
        $scope.DistrictwisePMKishanParam1 = 'fr';
        $scope.DistrictwisePMKishanParam2 = '3';

        $scope.drawVisualizationDistrictwisePMKishan('fr', '3');
        clearInterval(myVar);
    }



    $scope.OK = function () {
        $scope.drawVisualizationDistrictwiseSociety('to', 'ap');
        if ($scope.AuditYearWise.length == 0)
            $scope.HousingSocietyAuditsStatusForDasboard();

        if ($scope.UserLevelWiseCount.length == 0)
            $scope.UserLevelWiseCountFunction();

        //GetAlldeshdata_1();
    }

    $scope.ExpandGraph = function (Small) {
        if ($scope.IsGraphSmall == 0) {
            if (Small == 1) {
                drawVisualization(35);
                $scope.IsGraphSmall = 1;
            }
            else
                drawVisualization(4);
        }
    };

    $scope.TilamSangExpandGraph = function (Small) {
        if ($scope.IsTGGraphSmall == 0) {
            drawVisualizationTilamSangQty(35, $scope.MSPTS);
            $scope.IsTGGraphSmall = 1;
        }
        else {
            drawVisualizationTilamSangQty(4, $scope.MSPTS);
            $scope.IsTGGraphSmall = 0;
        }
    };

    $scope.drawVisualizationFertiChart = function (Flag) {
        var Count = 1;
        var Arry1 = [];
        var Title = '';
        if (_language == 'en-us')
            Arry1.push(['Fertilizer', 'Fertilizer']);
        else
            Arry1.push(['उर्वरक', 'उर्वरक']);


        if ($scope.FertilizerData != undefined && $scope.FertilizerData != null && $scope.FertilizerData.length > 0) {
            angular.forEach($scope.FertilizerData, function (item) {
                if (Flag == 'Sale') {
                    Arry1.push([(_language == 'en-us' ? item.Lovdescription : item.LovCode), parseInt(item.Sale)]);
                    Title = (_language == 'en-us' ? 'Fertilizer Retailer Sale (In Thousand MTs.)' : 'उर्वरक खुदरा बिक्री(हजार MTs. में)');
                }
                else if (Flag == 'Purchase') {
                    Arry1.push([(_language == 'en-us' ? item.Lovdescription : item.LovCode), parseInt(item.Purchase)]);
                    Title = (_language == 'en-us' ? 'Retailer Purchase (In Thousand MTs.)' : 'खुदरा विक्रेता खरीद(हज़ार MTs. में)');
                }
                else if (Flag == 'Target') {
                    Arry1.push([(_language == 'en-us' ? item.Lovdescription : item.LovCode), parseInt(item.TargetInMt)]);
                    Title = (_language == 'en-us' ? 'Fertilizer Wise Target (In Thousand MTs.)' : 'उर्वरक टारगेट(हज़ार MTs.)');
                }
            });

        }
        //// , 'DAP', 'Compost', 'SSP', 'NPKS', 'MOP', 'Urea'
        //// , 'डी ए पी', 'खाद', 'एस एस पी', 'एन पी के एस', 'एम ओ पी', 'यूरिया'
        //Arry1.push([(_language == 'en-us' ? 'DAP' : 'डी ए पी'), parseInt(23.38)]);
        //Arry1.push([(_language == 'en-us' ? 'Compost' : 'खाद'), parseInt(0.99)]);
        //Arry1.push([(_language == 'en-us' ? 'SSP' : 'एस एस पी'), parseInt(44.04)]);
        //Arry1.push([(_language == 'en-us' ? 'NPKS' : 'एन पी के एस'), parseInt(2.83)]);
        //Arry1.push([(_language == 'en-us' ? 'MOP' : 'एम ओ पी'), parseInt(5.25)]);
        //Arry1.push([(_language == 'en-us' ? 'Urea' : 'यूरिया'), parseInt(47.72)]);

        var data = google.visualization.arrayToDataTable(Arry1);
        var options = {
            title: Title,
            vAxis: { title: 'Qty(in thousand MTs.)' },
            hAxis: { title: '(Fertilizer)' },
            seriesType: 'bars',
            series: { 2: { type: 'line' } }
        };


        var chart = new google.visualization.ColumnChart(document.getElementById('FertiChart'));

        chart.draw(data, options);

    }

    function drawVisualizationConfed() {
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['Confed', 'Sale Amount']);
        else
            Arry.push(['भंडार', 'नियंत्रित व्यापार लक्ष्य']);

        $http.get(SiteUrl + "/User/GetConfedData/?YearWiseOrAll=" + $scope.EbazaarData).
            success(function (data, status) {
                angular.forEach(data, function (item) {
                    Arry.push([(item.District), parseInt(item.Amount)]);
                });

                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language == 'en-us' ? 'Rajasthan State Cooperative Consumers Association (Stores)' : 'राजस्थान राज्य सहकारी उपभोक्ता संघ (भंडार )'),
                    vAxis: { title: '(Rupees ₹)' },
                    hAxis: { title: '(Stores)' },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };
                var chart = new google.visualization.ColumnChart(document.getElementById('chartContainerForConfed'));

                chart.draw(data, options);
            }).
            error(function (data1, status) {

            });

    }

    $scope.GetUserCountForDashBoard = function () {
        $http.get(SiteUrl + "/User/GetUserCountForDashBoard").
            success(function (data, status) {

                $scope.TotalUser = data[0].totaluser;
                $scope.RequestReceive = data[0].requestuser;
                $scope.TotalActiveUser = data[0].activeuser;
            }).
            error(function (data, status) {
                $scope.UserCount = [];
            });
    };

    function current_progress_Status(current_progress) {
        $("#dynamic")
            .css("width", current_progress + "%")
            .attr("aria-valuenow", current_progress)
            .text(current_progress + "% Complete");
        if (current_progress == "100")
            $('#dynamic').hide();
    };

    //COMBO
    function FertilizerOpeningStockReport() {
        current_progress_Status(15);
        $.ajax({
            //url: SiteUrl + "/User/CommonFunctionForDashBoard",
            url: SiteUrl + "/PLDB/GetPLDBDashboard",
            async: false,
            type: 'GET',
            dataType: 'json',
            success: function (data) {

                var ArryPM = [];
                var Responsedata1 = [];
                $scope.TotalMemberArr = [];
                $scope.LoanApplicationModel = [];
                var data = JSON.parse(data.DataList);

                $scope.RegistrationModel = data.filter(x => x.Type == "1");

                $scope.TotalPLDB = data.filter(x => x.Type == "3" && x.Order == "1");
                if ($scope.TotalPLDB.length > 0)
                    $scope.TotalPLDBB = $scope.TotalPLDB[0].TotalCount;

                $scope.TotalBranch = data.filter(x => x.Type == "3" && x.Order == "2");
                if ($scope.TotalBranch.length > 0)
                    $scope.BTotalBranch = $scope.TotalBranch[0].TotalCount;

                $scope.TotalMemberArr = data.filter(x => x.Type == "4" && x.Order == "7");

                $scope.LoanApplicationModel = data.filter(x => x.Type == "2");

                $scope.ArrTotalLoanRequest = data.filter(x => x.Type == "2" && x.Order == "3")
                $scope.VTotalLoanRequest = $scope.ArrTotalLoanRequest[0].TotalCount;

                $scope.ArrTotalLoanAmountDisbursed = data.filter(x => x.Type == "2" && x.Order == "6");
                $scope.VTotalLoanAmountDisbursed = $scope.ArrTotalLoanAmountDisbursed[0].TotalCount;

                $scope.ArrTotalLoanAmountApproved = data.filter(x => x.Type == "2" && x.Order == "5");
                $scope.VTotalLoanAmountApproved = $scope.ArrTotalLoanAmountApproved[0].TotalCount;

                //START Fertilizer Progress
                $scope.FertilizerOpeningStockData = $scope.TotalMemberArr;
                var TotalStock = 0;
                var Urea = 0;
                var Compost = 0;
                var DAP = 0;
                var MOP = 0;
                var NPKS = 0;
                var SSP = 0;
                $scope.StockOn = '';

                $scope.drawVisualizationFertilizerOpeningStock('Urea');

                //END Fertilizer Progress


                //STATIC

                $scope.DistrictwiseSocietyParam1 = 'to';
                $scope.DistrictwiseSocietyParam2 = 'ap';
                //END STATIC
                current_progress_Status(30);



                //$scope.drawInspectionVisualization(48);

                //START STLOAN
                //$scope.FunGetStLoanDataForDashBoard();
                $scope.ApplicationReceived = 0;





                //$scope.Loan_Disbursement_Data = DataSet.Table2;
                //$scope.DistrictwiseApexParam = 'di';
                $scope.drawVisualizationDistrictwiseApex('di');

                //END STLOAN
                current_progress_Status(50);

                $scope.drawVisualizationDistrictwisePMKishan('fr', '3');
                return;
                //$scope.drawVisualizationPacsGraph('TotalPacs');

                //PM-Kisan APPLICATION STATUS
                $scope.PMCasesVerificationParam = 'Patwari_Esign';




                $scope.PM_Kisan_Application_Status_Data = DataSet.Table9;

                //END
                current_progress_Status(75);
                $scope.FertilizerOpeningStockParam = 'Urea';



                $scope.FertilizerTransactionData = DataSet.Table6;
                var TotalTransaction = 0;
                $scope.MonthName = '';
                angular.forEach(DataSet.Table6, function (item) {
                    TotalTransaction = TotalTransaction + parseFloat(item.QuantitySold);
                    $scope.MonthName = '(' + (_language == 'en-us' ? item.Month_En : item.Month_Hi) + ')';
                });

                $scope.TotalQuantitySold = TotalTransaction.toString().split('.')[0] + '.' + (TotalTransaction.toString().split('.')[1]).substr(0, 2);



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


            }
        });
    }

    //END

    function GetAlldeshdata_2() {

        $.ajax({
            url: SiteUrl + "/Citizen/GetDeshboardData",
            async: false,
            dataType: 'json',
            success: function (data) {
                try {
                    $scope.Deshboard = data.Table;
                    var DD = new DashboardData();
                    DD.PARTIALSUBMITTED = ($scope.Deshboard.filter(function (x) { return x.StatusId == 1 })).length;
                    DD.SUBMITTED = ($scope.Deshboard.filter(function (x) { return x.StatusId == 2 })).length;
                    DD.CERTIFICATEISSUED = 0;
                    DD.APPROVED = ($scope.Deshboard.filter(function (x) { return x.StatusId == 26 || x.StatusId == 17 })).length;

                    DD.PAYMENTREQUEST = ($scope.Deshboard.filter(function (x) { return x.StatusId == 33 })).length;
                    DD.REJECTED = ($scope.Deshboard.filter(function (x) { return x.StatusId == 6 || x.StatusId == 13 || x.StatusId == 16 || x.StatusId == 18 || x.StatusId == 21 || x.StatusId == 29 || x.StatusId == 34 || x.StatusId == 1044 })).length;
                    DD.TOTAL = (DD.PARTIALSUBMITTED + DD.APPROVED + DD.CERTIFICATEISSUED + DD.PAYMENTREQUEST + DD.REJECTED);
                    DD.UNDERPROCESS = ($scope.Deshboard).length;
                    DD.UNDERPROCESS = (DD.UNDERPROCESS - DD.TOTAL);
                    DD.TOTAL = DD.TOTAL + DD.UNDERPROCESS;
                    DD.ACT1958 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '1' })).length;
                    DD.ACT2001 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '3' })).length;
                    DD.ACT2005 = ($scope.Deshboard.filter(function (x) { return x.ACTID == '7' })).length;
                    DashboardDataList.push(DD);
                    $scope.HousingSocietyCount = ($scope.Deshboard.filter(function (x) { return x.ClassId == '6' })).length;


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
                    var OnlineTotalAppli = ($scope.DeshboardBelow.filter(function (x) { return x.Value == 'OnlineTotalAppli' }));
                    var OnlineRegiAppli = ($scope.DeshboardBelow.filter(function (x) { return x.Value == 'OnlineRegiAppli' }));
                    var OnlineRejeAppli = ($scope.DeshboardBelow.filter(function (x) { return x.Value == 'OnlineRejeAppli' }));
                    var OfflineTotalAppli = ($scope.DeshboardBelow.filter(function (x) { return x.Value == 'OfflineTotalAppli' }));
                    var OfflineRegiAppli = ($scope.DeshboardBelow.filter(function (x) { return x.Value == 'OfflineRegiAppli' }));
                    var OfflineRejeAppli = ($scope.DeshboardBelow.filter(function (x) { return x.Value == 'OfflineRejeAppli' }));

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

                    //$scope.OnlineTotalAppli = OnlineTotalAppli[0].Data;
                    //$scope.OnlineRegiAppli = OnlineRegiAppli[0].Data;
                    //$scope.OfflineTotalAppli = OfflineTotalAppli[0].Data;
                    //$scope.OfflineRegiAppli = OfflineRegiAppli[0].Data;
                    //  alert($scope.OnlineTotalAppli + "   " + $scope.OnlineRegiAppli + "   " + $scope.OfflineTotalAppli + "   " + $scope.OfflineRegiAppli + "   " )

                    var Responsedata1 = [];
                    // Graph 1 data
                    //alert("SUBMITTED "+DashboardDataList[0].SUBMITTED + "    APPROVED " + DashboardDataList[0].APPROVED + "     PARTIALSUBMITTED " + DashboardDataList[0].PARTIALSUBMITTED + "      PAYMENTREQUEST " + DashboardDataList[0].PAYMENTREQUEST + "    REJECTED " + DashboardDataList[0].REJECTED + "   UNDERPROCESS " + DashboardDataList[0].UNDERPROCESS);
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt(DashboardDataList[0].SUBMITTED, 10), parseInt(DashboardDataList[0].SUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt(DashboardDataList[0].APPROVED, 10), parseInt(DashboardDataList[0].APPROVED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10), parseInt(DashboardDataList[0].PARTIALSUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt(DashboardDataList[0].PAYMENTREQUEST, 10), parseInt(DashboardDataList[0].PAYMENTREQUEST, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt(DashboardDataList[0].REJECTED, 10), parseInt(DashboardDataList[0].REJECTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt(DashboardDataList[0].UNDERPROCESS, 10), parseInt(DashboardDataList[0].UNDERPROCESS, 10)]);
                    GetGraph('pie', _language == 'en-us' ? 'Society Status Graph' : 'ग्राफ', Responsedata1, 'StatusChart');

                }
                catch (err) {
                    alert("Error occurred in DeshboardData. " + err.message);
                }
            }
        });
    }
    function GetAlldeshdata_1() {

        $.ajax({
            url: SiteUrl + "/Citizen/GetDeshboardData_scheduler",
            async: false,
            dataType: 'json',
            success: function (data) {
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
                    $scope.OnlineTotalAppli = 0;
                    $scope.OnlineRegiAppli = 0;
                    $scope.OnlineRejeAppli = 0;
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
                    });
                    $scope.UNDERPROCESS = ($scope.TOTAL - ($scope.PARTIALSUBMITTED + $scope.SUBMITTED + $scope.APPROVED + $scope.PAYMENTREQUEST + $scope.REJECTED))
                    $scope.Society = $scope.TOTAL;
                    $scope.SocietyNotApprove = $scope.TOTAL - $scope.APPROVED;
                    $scope.SocietyApprove = $scope.APPROVED;

                    $scope.SocietyInspectionsNotAppr = $scope.APPROVED - $scope.SocietyInspectionsAppr;
                    $scope.SocietyAuditNotAppr = $scope.APPROVED - $scope.SocietyAuditAppr;
                    $scope.SocietyElectionNotAppr = $scope.APPROVED - $scope.SocietyElectionAppr;

                    var Responsedata1 = [];
                    // Graph 1 data
                    Responsedata1.push(['Task', '', { role: 'annotation' }]);
                    Responsedata1.push([_language == 'en-us' ? 'SUBMITTED' : 'अनुमोदित', parseInt($scope.SUBMITTED, 10), parseInt($scope.SUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'CERTIFICATEISSUED' : 'प्रमाणपत्र जारी किया गया', parseInt($scope.APPROVED, 10), parseInt($scope.APPROVED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PARTIALSUBMITTED' : 'आंशिक रूप से अनुमोदित', parseInt($scope.PARTIALSUBMITTED, 10), parseInt($scope.PARTIALSUBMITTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'PAYMENTREQUEST' : 'भुगतान अनुरोध', parseInt($scope.PAYMENTREQUEST, 10), parseInt($scope.PAYMENTREQUEST, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'REJECTED' : 'अस्वीकृत', parseInt($scope.REJECTED, 10), parseInt($scope.REJECTED, 10)]);
                    Responsedata1.push([_language == 'en-us' ? 'UNDERPROCESS' : 'प्रक्रियाधीन', parseInt($scope.UNDERPROCESS, 10), parseInt($scope.UNDERPROCESS, 10)]);
                    GetGraph('pie', _language == 'en-us' ? 'Society Status Graph' : 'ग्राफ', Responsedata1, 'StatusChart');

                }
                catch (err) {
                    alert("Error occurred in DeshboardData. " + err.message);
                }
            }
        });
    }

    $scope.GetDistrictData = function (DivisionId) {
        $http.get(SiteUrl + "/User/GetDistrictReport?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId + "&DivisionId=" + DivisionId).
            success(function (data, status) {
                try {
                    $scope.DistrictReport = data;
                }
                catch (err) {
                    $scope.DistrictReport = [];
                    alert("Error occurred in GetDistrictReport. " + err.message);
                }
            }).
            error(function (data1, status) {

            });
    }


    $scope.GetSocietyTypeWiseAuditProgressList = [];
    $scope.GetSocietyTypeWiseAuditProgress = function () {
        $http.get(SiteUrl + "/User/GetSocietyTypeWiseAuditProgress?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId).
            success(function (data, status) {
                $scope.GetSocietyTypeWiseAuditProgressList = data;
            }).
            error(function (data1, status) {

            });
    }



    $scope.drawVisualizationDistrictwiseSociety = function (Type, Status) {

        $scope.DistrictwiseSocietyParam1 = Type;
        $scope.DistrictwiseSocietyParam2 = Status;

        var Count = 1;
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['District', 'Societies']);
        else
            Arry.push(['District', 'Societies']);
        var TypeLocal = ''
        if (Type == 'to') {
            TypeLocal = 'Total';
        }
        else if (Type == 'on') {
            TypeLocal = 'Online';
        }
        else if (Type == 'of') {
            TypeLocal = 'Offline';
        }

        var StatusLocal = ''
        if (Status == 'ap') {
            StatusLocal = 'Approve';
        }
        else if (Status == 're') {
            StatusLocal = 'Reject';
        }
        else if (Status == 'pe') {
            StatusLocal = 'Pending';
        }

        $http.post(SiteUrl + "/User/GetDistrictwiseSocietyByTypeStatus", { "SocietyType": Type, "SocietyStatus": Status }).
            success(function (data, status) {
                angular.forEach(data, function (item) {
                    // if (Count < IsPageload) {
                    Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_ENG), parseInt(item.Data)]);
                    // }
                    // Count = Count + 1;
                });
                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language == 'en-us' ? 'District Wise ' + TypeLocal + ' ' + StatusLocal + ' Societies' : 'District Wise ' + TypeLocal + ' ' + StatusLocal + ' Societies'),
                    vAxis: { title: '(Count)' },
                    hAxis: { title: 'District' }, textStyle: {
                        fontSize: 10 // or the number you want
                    },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('chartContainer'));

                chart.draw(data, options);
            }).
            error(function (data1, status) {
                alert("Error occur");
            });
    }

    $scope.drawVisualizationDistrictwisePMKishan = function (TypePM, StatusPM) {
        var Count = 1;
        var ArryPM = [];
        var Title = '';

        $scope.DistrictwisePMKishanParam1 = TypePM;
        $scope.DistrictwisePMKishanParam2 = StatusPM;
        (['District', 'Societies']);
        var TypeLocalPM = ''
        if (TypePM == 'fr') {
            TypeLocalPM = 'Farmer Benefited';
            ArryPM.push(['District', 'Status']);
        }

        var StatusLocalPM = ''


        StatusLocalPM = 'Phase 1';
        Title = (_language == 'en-us' ? '(Count)' : '(संख्या)');

        $scope.TotAppRec = 0;
        $scope.TotalMembership = 0;
        $scope.TotalObjection = 0;

        if ($scope.RegistrationModel != null && $scope.RegistrationModel != undefined && $scope.RegistrationModel.length > 0) {
            angular.forEach($scope.RegistrationModel, function (item) {
                ArryPM.push([(_language == 'en-us' ? item.Status_En : item.Status_Hi), parseInt(item.TotalCount)]);
                if (item.Status_En == "APPLICATION SUBMITTED BY CITIZEN" || item.Status_En == "APPLICATION SUBMITTED BY CITIZEN THROUGH KIOSK" || item.Status_En == "APPLICATION SUBMITTED FOR CITIZEN THROUGH BANK ASSISTANT") {
                    $scope.TotAppRec += parseInt(item.TotalCount);
                }
                if (item.Status_En == "APPROVE AND GENERATE MEMBERSHIP") {
                    $scope.TotalMembership += parseInt(item.TotalCount);
                }
                if (item.Status_En == "APPROVE AND GENERATE MEMBERSHIP") {
                    $scope.TotalMembership += parseInt(item.TotalCount);
                }
                if (item.Status_En.indexOf("OBJECTION") > 0) {
                    $scope.TotalObjection += parseInt(item.TotalCount);
                }

            });
        }






        //});

        var data = google.visualization.arrayToDataTable(ArryPM);
        var options = {
            title: (_language == 'en-us' ? 'District Wise ' + TypeLocalPM + ' ' + StatusLocalPM + ' ' : 'District Wise ' + TypeLocalPM + ' ' + StatusLocalPM + ' '),
            vAxis: { title: Title },
            hAxis: {
                title: (_language == 'en-us' ? '(District)' : '(जिला)'), textStyle: {
                    fontSize: 10 // or the number you want
                }
            },
            seriesType: 'bars',
            series: { 5: { type: 'line' } }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById('divChart1'));
        chart.draw(data, options);
        ////}).
        ////error(function (data1, status) {
        ////    alert("Error occur");
        ////});
    }
    $scope.drawVisualizationFertilizerOpeningStock = function (Fertilizer) {
        var Count = 1;
        var ArryPM = [];
        var Title = '';


        ArryPM.push([_language == 'en-us' ? 'District' : 'जिले', _language == 'en-us' ? 'Members Count' : 'ओपनिंग स्टॉक']);

        if ($scope.FertilizerOpeningStockData != null && $scope.FertilizerOpeningStockData != undefined && $scope.FertilizerOpeningStockData.length > 0) {

            $scope.TotalMember = 0;

            $scope.FertilizerOpeningStockData.forEach(function (Member, index) {
                ArryPM.push([(_language == 'en-us' ? Member.Status_En : Member.Status_Hi), parseFloat(Member.TotalCount)]);
                $scope.TotalMember += parseFloat(Member.TotalCount);
            });

            var data = google.visualization.arrayToDataTable(ArryPM);
            if ($('#EUserLevel').val() != '3') {
                var options = {
                    title: _language == 'en-us' ? ('PLDB Wise Members Count') : ('प्राथमिक भूमि विकास बैंक वार सदस्य संख्या'),
                    vAxis: { title: _language == 'en-us' ? '(Members Count)' : '(सदस्य संख्या)' },
                    hAxis: {
                        title: (_language == 'en-us' ? '(Primary Land Development Bank)' : '(प्राथमिक भूमि विकास बैंक)'), textStyle: {
                            fontSize: 10 // or the number you want
                        }
                    },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };
            }
            else {
                var options = {
                    title: _language == 'en-us' ? ('Branch Wise Members Count') : ('प्राथमिक भूमि विकास बैंक वार सदस्य संख्या'),
                    vAxis: { title: _language == 'en-us' ? '(Members Count)' : '(सदस्य संख्या)' },
                    hAxis: {
                        title: (_language == 'en-us' ? '(PLDB Branch)' : '(प्राथमिक भूमि विकास बैंक)'), textStyle: {
                            fontSize: 10 // or the number you want
                        }
                    },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };
            }
            var chart = new google.visualization.ColumnChart(document.getElementById('FertiChart'));
            chart.draw(data, options);
        }


        //});


    }

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


        TypeLocalAX = (_language == 'en-us' ? 'Disbursement' : 'संवितरण');
        ArryAX.push(['District', 'Disbursement']);
        title = (_language == 'en-us' ? 'Disbursement(in Cr. ₹)' : 'संवितरण (करोड़ ₹ में)');


        var StatusLocalAX = ''

        if ($scope.LoanApplicationModel.length > 0) {
            angular.forEach($scope.LoanApplicationModel, function (item) {
                ArryAX.push([(_language == 'en-us' ? item.Status_En : item.Status_Hi), parseInt(item.TotalCount)]);
            });

        }

        //});
        var data = google.visualization.arrayToDataTable(ArryAX);
        var options = {
            title: (_language == 'en-us' ? 'Loan Disbursement Progress ' : 'ऋण संवितरण प्रगति'),
            vAxis: { title: title },
            hAxis: {
                title: (_language == 'en-us' ? 'Status' : 'स्थति'), textStyle: {
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

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").
            success(function (data, status) {
                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });

        $scope.FinancialYearList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data.filter(function (x) { return x.FIN_YEAR_END != data[0].FIN_YEAR_END });
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];

            });

        $scope.MonthList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
                $scope.MonthId = new Date().getMonth();
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });

        $(".se-pre-con").fadeOut("slow");

    };

    GetAlldeshdata_1();
    current_progress_Status(10);

    google.setOnLoadCallback(FertilizerOpeningStockReport);

    current_progress_Status(25);

    $scope.OnPageLoad();

});






