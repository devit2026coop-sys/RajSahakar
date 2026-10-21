
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
                drawVisualization(4);

                //drawVisualizationConfed();
                drawVisualizationTilamSangQty(4, 2);

                $scope.DistrictwiseSocietyParam1 = 'to';
                $scope.DistrictwiseSocietyParam2 = 'ap';

                $scope.drawVisualizationDistrictwiseSociety('to', 'ap');

                // $scope.AuditorInspection["FinancialYear"] = $scope.FinancialYearList[0].FIN_YEAR_MASTR_ID;
                $scope.drawInspectionVisualization(48);
                $scope.FunGetStLoanDataForDashBoard();

                drawVisualizationMSPRajFedQty();



                myVar = setInterval(myTimer, 3000);
                $scope.PMCasesVerificationParam = 'Patwari_Esign';
                $scope.drawVisualizationPMCasesVerification('Patwari_Esign');
                $scope.FertilizerOpeningStockParam = 'Urea';
                $scope.drawVisualizationFertilizerOpeningStock('Urea');
                $scope.drawVisualizationFertilizerRetailerTransaction();
            }).
            error(function (data1, status) {

            });
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
    $scope.PMKisanExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        if (Small == 1)
            $scope.drawVisualizationPMCasesVerification($scope.PMCasesVerificationParam);
        else if (Small == 2)
            $scope.drawVisualizationFertilizerOpeningStock($scope.FertilizerOpeningStockParam)
        else if (Small == 3)
            $scope.drawVisualizationFertilizerRetailerTransaction();
        else if (Small == 4)
            $scope.drawVisualizationDistrictwisePMKishan($scope.DistrictwisePMKishanParam1, $scope.DistrictwisePMKishanParam2);
        else if (Small == 5)
            $scope.drawVisualizationDistrictwiseApex($scope.DistrictwiseApexParam);
        else if (Small == 6)
            $scope.drawVisualizationMSPRajFedQty();
        else if (Small == 7)
            $scope.drawVisualizationDistrictwiseSociety($scope.DistrictwiseSocietyParam1, $scope.DistrictwiseSocietyParam2);
        else if (Small == 8)
            $scope.drawVisualizationPacsGraph('TotalPacs');
        else if (Small == 9) {
            if ($("#" + Id).hasClass('popupGraph')) {
                drawVisualization(35);
            } else {
                drawVisualization(4);
            }
        }
        //$scope.drawVisualizationPMCasesVerification('Patwari_Esign');       


    };
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

    function drawVisualization(IsPageload) {
        var Count = 1;
        var Arry = [];
        if (_language == 'en-us')
            Arry.push(['Bhandar', 'Controlled Business Target', 'Uncontrolled Business Target']);  //Arry.push(['Bhandar', 'Controlled Business Target', 'Reached', 'Uncontrolled Business Target', 'Reached']);
        else
            Arry.push(['भंडार', 'नियंत्रित व्यापार लक्ष्य', 'अनियंत्रित व्यवसाय लक्ष्य']);  // Arry.push(['भंडार', 'नियंत्रित व्यापार लक्ष्य', 'प्राप्त लक्ष्य', 'अनियंत्रित व्यवसाय लक्ष्य', 'प्राप्त लक्ष्य']);

        $http.post(SiteUrl + "/EntryForms/BhandarMprDataForDashboard").
            success(function (data, status) {

                angular.forEach(data, function (item) {

                    //Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.ControlledBusinessTarget), parseInt(item.Controlledfulfillment), parseInt(item.UNControlledBusinessTarget), parseInt(item.UnControlledfulfillment)]);
                    Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.ControlledBusinessTarget), parseInt(item.UNControlledBusinessTarget)]);


                });


                var data = google.visualization.arrayToDataTable(Arry);
                var options = {
                    title: (_language == 'en-us' ? 'Rajasthan State Cooperative Consumers Association (Stores)' : 'राजस्थान राज्य सहकारी उपभोक्ता संघ (भंडार )'),
                    vAxis: { title: (_language == 'en-us' ? '(In Crore)' : '(करोड़ में)') },
                    hAxis: { title: (_language == 'en-us' ? '(Bhandar)' : '(भंडार)') },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('BhandarChart'));

                chart.draw(data, options);

            }).
            error(function (data1, status) {

            });
    }




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
    $scope.Ebazaar = function () {
        //drawVisualizationConfed();
    };
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

    $scope.drawVisualizationPMCasesVerification = function (Flag) {


        var Arry = [];
        var Count = 1;
        var Tit = '';
        $scope.PMCasesVerificationParam = Flag;

        if (_language == 'en-us') {
            if (Flag == 'Patwari_Esign') {
                Arry.push(['PM-Kisan', 'Esign By Patwari']);
                Tit = 'Esign By Patwari';
            }

            if (Flag == 'Patwari_Reject') {
                Arry.push(['PM-Kisan', 'Reject By Patwari']);
                Tit = 'Reject By Patwari';
            }

            if (Flag == 'Tehsildar_Esign') {
                Arry.push(['PM-Kisan', 'Esign By Tehsildar']);
                Tit = 'Esign By Tehsildar';
            }

            if (Flag == 'Tehsildar_Reject') {
                Arry.push(['PM-Kisan', 'Reject By Tehsildar']);
                Tit = 'Reject By Tehsildar';
            }

            if (Flag == 'Collector_Esign') {
                Arry.push(['PM-Kisan', 'Esign By Collector']);
                Tit = 'Esign By Collector';
            }

            if (Flag == 'Collector_Reject') {
                Arry.push(['PM-Kisan', 'Reject By Collector']);
                Tit = 'Reject By Collector';
            }

        }
        else {
            if (Flag == 'Patwari_Esign') {
                Arry.push(['पी.एम -किसान', 'पटवारी द्वारा ई-हस्ताक्षर']);
                Tit = 'Reject By Collector';
            }

            if (Flag == 'Patwari_Reject') {
                Arry.push(['पी.एम -किसान', 'पटवारी द्वारा अस्वीकृत']);
                Tit = 'पटवारी द्वारा अस्वीकृत';
            }

            if (Flag == 'Tehsildar_Esign') {
                Arry.push(['पी.एम -किसान', 'तहसीलदार द्वारा ई-हस्ताक्षर']);
                Tit = 'तहसीलदार द्वारा ई-हस्ताक्षर';
            }

            if (Flag == 'Tehsildar_Reject') {
                Arry.push(['पी.एम -किसान', 'तहसीलदार द्वारा अस्वीकृत']);
                Tit = 'तहसीलदार द्वारा अस्वीकृत';
            }

            if (Flag == 'Collector_Esign') {
                Arry.push(['पी.एम -किसान', 'कलेक्टर द्वारा ई-हस्ताक्षर']);
                Tit = 'कलेक्टर द्वारा ई-हस्ताक्षर';
            }

            if (Flag == 'Collector_Reject') {
                Arry.push(['पी.एम -किसान', 'कलेक्टर द्वारा अस्वीकृत']);
                Tit = 'कलेक्टर द्वारा अस्वीकृत';
            }
        }


        if ($scope.PM_Kisan_Application_Status_Data.length > 0) {

            angular.forEach($scope.PM_Kisan_Application_Status_Data, function (item) {

                if (Flag == 'Patwari_Esign') {
                    Arry.push([(_language == 'en-us' ? item.DISTRICTNAME : item.DISTRICTNAME), parseInt(item.PAT_ESIGN_STATUS)]);
                }
                else if (Flag == 'Patwari_Reject') {
                    Arry.push([(_language == 'en-us' ? item.DISTRICTNAME : item.DISTRICTNAME), parseInt(item.PATWARI_REJ)]);
                }
                else if (Flag == 'Tehsildar_Esign') {
                    Arry.push([(_language == 'en-us' ? item.DISTRICTNAME : item.DISTRICTNAME), parseInt(item.TEH_ESIGN_STATUS)]);
                }
                else if (Flag == 'Tehsildar_Reject') {
                    Arry.push([(_language == 'en-us' ? item.DISTRICTNAME : item.DISTRICTNAME), parseInt(item.TEHSILDAR_REJ)]);
                }
                else if (Flag == 'Collector_Esign') {
                    Arry.push([(_language == 'en-us' ? item.DISTRICTNAME : item.DISTRICTNAME), parseInt(item.COLLECTOR_ESIGN_STATUS)]);
                }
                else if (Flag == 'Collector_Reject') {
                    Arry.push([(_language == 'en-us' ? item.DISTRICTNAME : item.DISTRICTNAME), parseInt(item.COLLECTER_REJ)]);
                }

            });

        }

        var data = google.visualization.arrayToDataTable(Arry);
        var options = {
            title: (_language == 'en-us' ? ('PM-Kisan (' + Tit + ')') : ('पीएम-किसान  (' + Tit + ')')),
            vAxis: { title: '(No. Of Cases)' },
            hAxis: {
                title: '(District)', textStyle: {
                    fontSize: 10 // or the number you want
                }
            },
            seriesType: 'bars',
            series: { 1: { type: 'line' } },
        };



        var chart = new google.visualization.ColumnChart(document.getElementById('chartPMKisanCasesVerification'));

        chart.draw(data, options);


        var data = google.visualization.arrayToDataTable(Arry);

    };

    function drawVisualizationTilamSangQty(Small, QtyPrice) {
        var Arry = [];
        if (QtyPrice == "1") {
            if (_language == 'en-us')
                Arry.push(['TilamSang (Purchase Rs.)', 'Sarson', 'Chana', 'Gehu']);
            else
                Arry.push(['तिलम संघ (खरीद रुपये)', 'सरसों', 'चना', 'गेहू']);

            if (Small == 35) {
                Arry.push([('नाहरगढ़'), 40737900, 11443740, 0]);
                Arry.push([('करवर'), 58254000, 29156820, 0]);
                Arry.push([('सुमेरगंज मंडी'), 61154100, 8752590, 0]);
                Arry.push([('हडोल'), 58818900, 28985880, 0]);
                Arry.push([('नाहरगढ़'), 40737900, 11443740, 0]);
                Arry.push([('करवर'), 58254000, 29156820, 0]);
                Arry.push([('सुमेरगंज मंडी'), 61154100, 8752590, 0]);
                Arry.push([('हडोल'), 58818900, 28985880, 0]);
            }
            else {
                Arry.push([('नाहरगढ़'), 40737900, 11443740, 0]);
                Arry.push([('करवर'), 58254000, 29156820, 0]);
                Arry.push([('सुमेरगंज मंडी'), 61154100, 8752590, 0]);
                Arry.push([('हडोल'), 58818900, 28985880, 0]);
            }

            var data = google.visualization.arrayToDataTable(Arry);
            var options = {
                title: (_language == 'en-us' ? 'MSP Purchase Report (Price)' : 'एमएसपी खरीद रिपोर्ट (मूल्य)'),
                vAxis: { title: '' },
                hAxis: { title: '' },
                seriesType: 'bars',
                series: { 5: { type: 'line' } }
            };
        }
        else {
            if (_language == 'en-us')
                Arry.push(['TilamSang (Qty)', 'Sarson', 'Chana', 'Gehu']);
            else
                Arry.push(['तिलम संघ (मात्रा)', 'सरसों', 'चना', 'गेहू']);


            if (Small == 35) {
                Arry.push([('नाहरगढ़'), 9699, 2477, 0]);
                Arry.push([('करवर'), 13870, 6311, 0]);
                Arry.push([('सुमेरगंज मंडी'), 14560, 1894, 0]);
                Arry.push([('हडोल'), 14004, 6274, 0]);
                Arry.push([('नाहरगढ़'), 9699, 2477, 0]);
                Arry.push([('करवर'), 13870, 6311, 0]);
                Arry.push([('सुमेरगंज मंडी'), 14560, 1894, 0]);
                Arry.push([('हडोल'), 14004, 6274, 0]);
            }
            else {
                Arry.push([('नाहरगढ़'), 9699, 2477, 0]);
                Arry.push([('करवर'), 13870, 6311, 0]);
                Arry.push([('सुमेरगंज मंडी'), 14560, 1894, 0]);
                Arry.push([('हडोल'), 14004, 6274, 0]);
            }

            var data = google.visualization.arrayToDataTable(Arry);
            var options = {
                title: (_language == 'en-us' ? 'MSP Purchase Report(Quantity)' : 'एमएसपी खरीद रिपोर्ट (मात्रा)'),
                vAxis: { title: '' },
                hAxis: { title: '' },
                seriesType: 'bars',
                series: { 5: { type: 'line' } }
            };
        }
        var chart = new google.visualization.ColumnChart(document.getElementById('chartTilamSangQty'));
        chart.draw(data, options);

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

    $scope.HousingSocietyAuditsStatusForDasboard = function () {
        $http.get(SiteUrl + "/User/HousingSocietyAuditsCountYearWise").
            success(function (data, status) {
                $scope.AuditYearWise = data;
            }).
            error(function (data, status) {
                $scope.AuditYearWise = [];
            });
    };

    $scope.UserLevelWiseCountFunction = function () {
        $http.get(SiteUrl + "/User/GetUserLevelWiseCount").
            success(function (data, status) {
                $scope.UserLevelWiseCount = data;
            }).
            error(function (data, status) {
                $scope.UserLevelWiseCount = [];
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

                //$scope.Urea = Urea.toString().split('.')[0] + '.' + (Urea.toString().split('.')[1]).substr(0, 2);
                //$scope.Compost = Compost.toString().split('.')[0] + '.' + (Compost.toString().split('.')[1]).substr(0, 2);
                //$scope.DAP = DAP.toString().split('.')[0] + '.' + (DAP.toString().split('.')[1]).substr(0, 2);
                //$scope.MOP = MOP.toString().split('.')[0] + '.' + (MOP.toString().split('.')[1]).substr(0, 2);
                //$scope.NPKS = NPKS.toString().split('.')[0] + '.' + (NPKS.toString().split('.')[1]).substr(0, 2);
                //$scope.SSP = SSP.toString().split('.')[0] + '.' + (SSP.toString().split('.')[1]).substr(0, 2);

                //  $scope.TotalStock = TotalStock.toString().split('.')[0] + '.' + (TotalStock.toString().split('.')[1]).substr(0, 2);
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

    function drawVisualizationMSPRajFedQty() {
        var Arry = [];
        var ArrayForTipMSP = [];
        var GrandTotalTipMSP = 0;
        $http.get(SiteUrl + "/User/GetRajFedData").
            success(function (data, status) {

                console.log('TEST #2');
                console.log(data);

                if ($scope.MSPRAJFED == undefined || $scope.MSPRAJFED == '2') {
                    $scope.MSPRAJFED = '2';
                    if (data.length > 0) {
                        Arry.push(['Commodity', (_language == 'en-us' ? 'Target purchase (metric tons)' : 'टारगेट खरीद (मीट्रिक टन)'), (_language == 'en-us' ? 'Quantity of purchasing genes (metric tons)' : 'क्रय जींस की मात्रा(मीट्रिक टन)')]);
                    }
                    angular.forEach(data, function (item) {
                        var Cl = parseFloat(item.PurchaseTarget);
                        var Cl = Cl * 100000;
                        Arry.push([(item.Crop), Cl, parseInt(item.PurchaseQty)]);

                    });
                }
                else {
                    if (data.length > 0) {
                        Arry.push(['Commodity', (_language == 'en-us' ? 'Target purchase (in crores)' : 'टारगेट खरीद (करोडो में)'), (_language == 'en-us' ? 'Price of purchase genes (in crores)' : 'क्रय जींस का मूल्य(करोडो में)')]);
                    }
                    angular.forEach(data, function (item) {
                        var Cl = parseFloat(item.PurchaseTarget);
                        Cl = (Cl * 1000000);
                        Cl = (Cl * parseFloat(item.Rate));
                        Cl = (Cl / 10000000);

                        var C2 = parseInt(item.PurchaseAmount);
                        //C2 = ((C2 * 10) * parseFloat(item.Rate));
                        //C2 = (C2 / 10000000);
                        Arry.push([(item.Crop), Cl, C2]);
                    });
                }

                angular.forEach(data, function (item) {

                    ArrayForTipMSP.push({ "Crop": item.Crop, "Value": parseInt(item.PurchaseAmount) });
                    GrandTotalTipMSP += parseFloat(parseInt(item.PurchaseAmount));
                });
                $scope.ArrayForTipMSP = ArrayForTipMSP;
                $scope.GrandTotalTipMSP = GrandTotalTipMSP;

                var data = google.visualization.arrayToDataTable(Arry);
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

                chart.draw(data, options);
            }).
            error(function (data1, status) {

            });

    };
    $scope.FunGetStLoanDataForDashBoard = function () {
        $http.get(SiteUrl + "/User/GetStLoanDataForDashBoard").
            success(function (data, status) {
                $scope.ApplicationReceived = 0;
                $scope.STTargetK = 0;
                $scope.STTarget = 0;
                $scope.STTargetR = 0;
                $scope.ApprovedByPacs = 0;
                $scope.ApprovedByBranch = 0;
                $scope.LoanDisbursement = 0;
                $scope.DMR_Credited = 0;

                $scope.Loan_Disbursement = 0;
                angular.forEach(data, function (item) {
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
                $scope.Loan_Disbursement_Data = data;
                $scope.DistrictwiseApexParam = 'di';
                $scope.drawVisualizationDistrictwiseApex('di');

            }).
            error(function (data, status) {
                $scope.Department = [];
            });
    };
    function FertilizerRetailerTransactionReport() {
        $.ajax({
            url: SiteUrl + "/CommonServices/Mobile_GetFertilizerRetailerTransactionReport",
            async: false,
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                $scope.FertilizerTransactionData = data;
                var TotalTransaction = 0;
                //var Urea = 0;
                //var Compost = 0;
                //var DAP = 0;
                //var MOP = 0;
                //var NPKS = 0;
                //var SSP = 0;
                $scope.MonthName = '';
                angular.forEach(data, function (item) {
                    TotalTransaction = TotalTransaction + parseFloat(item.QuantitySold);
                    //if (item.FertilizerName_En == 'Compost')
                    //    Compost = Compost + parseFloat(item.Total);
                    //if (item.FertilizerName_En == 'DAP')
                    //    DAP = DAP + parseFloat(item.Total);
                    //if (item.FertilizerName_En == 'MOP')
                    //    MOP = MOP + parseFloat(item.Total);
                    //if (item.FertilizerName_En == 'NPKS')
                    //    NPKS = NPKS + parseFloat(item.Total);
                    //if (item.FertilizerName_En == 'SSP')
                    //    SSP = SSP + parseFloat(item.Total);
                    //if (item.FertilizerName_En == 'Urea')
                    //    Urea = Urea + parseFloat(item.Total);
                    $scope.MonthName = '(' + (_language == 'en-us' ? item.Month_En : item.Month_Hi) + ')';
                });

                //$scope.Urea = Urea.toString().split('.')[0] + '.' + (Urea.toString().split('.')[1]).substr(0, 2);
                //$scope.Compost = Compost.toString().split('.')[0] + '.' + (Compost.toString().split('.')[1]).substr(0, 2);
                //$scope.DAP = DAP.toString().split('.')[0] + '.' + (DAP.toString().split('.')[1]).substr(0, 2);
                //$scope.MOP = MOP.toString().split('.')[0] + '.' + (MOP.toString().split('.')[1]).substr(0, 2);
                //$scope.NPKS = NPKS.toString().split('.')[0] + '.' + (NPKS.toString().split('.')[1]).substr(0, 2);
                //$scope.SSP = SSP.toString().split('.')[0] + '.' + (SSP.toString().split('.')[1]).substr(0, 2);

                $scope.TotalQuantitySold = TotalTransaction.toString().split('.')[0] + '.' + (TotalTransaction.toString().split('.')[1]).substr(0, 2);

                // alert(TotalFarmerBenefited);    

            }
        });
    }
    //END


    $scope.MSPRajFedQty = function () {
        drawVisualizationMSPRajFedQty();
    };

    $scope.GetPacsdeshdata = function () {
        $http.get(SiteUrl + "/User/GetPacsMemberCountReport").
            success(function (data, status) {
                try {
                    $scope.PacsPacsMemberCountReportData = data;
                    var TOTAL_PACS = 0;
                    var TOTAL_MEMBER = 0;
                    var TOTAL_OD = 0;
                    var TOTAL_NOD = 0;
                    angular.forEach(data, function (item) {
                        TOTAL_PACS = TOTAL_PACS + parseInt(item.TOTAL_PACS);
                        TOTAL_MEMBER = TOTAL_MEMBER + parseInt(item.MEMBER_Count);
                        TOTAL_OD = TOTAL_OD + parseInt(item.OD_COUNT);
                        TOTAL_NOD = TOTAL_NOD + parseInt(item.NOD_COUNT);
                    });

                    $scope.TOTAL_PACS = TOTAL_PACS;
                    $scope.TOTAL_MEMBER = TOTAL_MEMBER;
                    $scope.TOTAL_OD = TOTAL_OD;
                    $scope.TOTAL_NOD = TOTAL_NOD;

                }
                catch (err) {
                    alert("Error occurred in GetPacsMemberCountReport. " + err.message);

                }
            }).
            error(function (data1, status) {

            });
    };


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


    $scope.GetPacsStatusReport = function () {
        $http.get(SiteUrl + "/User/GetPacsStatusReport").
            success(function (data, status) {
                try {
                    var Profiling_Completed = 0;
                    var Profiling_Pending = 0;
                    angular.forEach(data, function (item) {
                        if (parseInt(item.SocietyProceesStatusId) == 26 || parseInt(item.SocietyProceesStatusId) == 17)
                            Profiling_Completed = Profiling_Completed + parseInt(item.TotalSociety);
                        else
                            Profiling_Pending = Profiling_Pending + parseInt(item.TotalSociety);
                    });
                    $scope.Profiling_Completed = Profiling_Completed;
                    $scope.Profiling_Pending = Profiling_Pending;

                }
                catch (err) {
                    alert("Error occurred in GetPacsStatusReport. " + err.message);
                }
            }).
            error(function (data1, status) {

            });
    }

    function GetPacsStatusReport_1() {
        $.ajax({
            url: SiteUrl + "/User/GetPacsStatusReport",
            async: false,
            dataType: 'json',
            success: function (data) {
                try {
                    var Profiling_Completed = 0;
                    var Profiling_Pending = 0;
                    angular.forEach(data, function (item) {
                        if (parseInt(item.SocietyProceesStatusId) == 26 || parseInt(item.SocietyProceesStatusId) == 17)
                            Profiling_Completed = Profiling_Completed + parseInt(item.TotalSociety);
                        else
                            Profiling_Pending = Profiling_Pending + parseInt(item.TotalSociety);
                    });
                    $scope.Profiling_Completed = Profiling_Completed;
                    $scope.Profiling_Pending = Profiling_Pending;

                }
                catch (err) {
                    alert("Error occurred in GetPacsStatusReport. " + err.message);
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
                    ////// $scope.AuditProgrssDivisionWise('AuditableSocieties');
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
                    /// $scope.AuditProgrssDivisionWise('AuditableSocieties');
                    $scope.AuditProgrssLable = 'Auditable Societies';
                }
                catch (err) {
                    $scope.DivisionReport = [];
                    alert("Error occurred in GetDivisionReport. " + err.message);
                }
            }
        });
    }

    $scope.GetSocietyStatusWiseData = [];
    $scope.GetSocietyStatusWiseDataList = function () {
        $http.get(SiteUrl + "/User/GetSocietyStatusWiseDataList?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId).
            success(function (data, status) {
                $scope.GetSocietyStatusWiseData = data;
            }).
            error(function (data1, status) {

            });
    }
    $scope.GetSocietyStatusWiseDataList();



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



    $scope.GetSocietyTypeWiseAuditProgressList = [];
    $scope.GetSocietyTypeWiseAuditProgress = function () {
        $http.get(SiteUrl + "/User/GetSocietyTypeWiseAuditProgress?FYearID=" + $scope.FYearId + "&MonthId=" + $scope.MonthId).
            success(function (data, status) {
                $scope.GetSocietyTypeWiseAuditProgressList = data;
            }).
            error(function (data1, status) {

            });
    }
    $scope.GetSocietyTypeWiseAuditProgress();
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
                        if (DistrictData.filter(function (X) { return X.DISTRICT_ENG == item.DISTRICT_ENG }).length == 0) {
                            DistrictData.push({ DISTRICT_ENG: item.DISTRICT_ENG, DISTRICT_MANGAL: item.DISTRICT_MANGAL });
                        }
                    }


                    if (ActData.length == 0) {
                        ActData.push({ ActNameEn: item.ActNameEn, ActNameHi: item.ActNameHi });
                    }
                    else {
                        if (ActData.filter(function (X) { return X.ActNameEn == item.ActNameEn }).length == 0) {
                            ActData.push({ ActNameEn: item.ActNameEn, ActNameHi: item.ActNameHi });
                        }
                    }

                });

                angular.forEach(DistrictData, function (item) {

                    var SocietyCountData = data.filter(function (P) { P.DISTRICT_ENG == item.DISTRICT_ENG });
                    var _SUM = 0;

                    angular.forEach(SocietyCountData, function (j) {
                        _SUM += parseInt(j.SocietyCount);
                    });

                    var InspectionDone = 0;
                    InspectionDone = StatusDataInspection.filter(function (V) { V.DISTRICT_ENG == item.DISTRICT_ENG })[0].AuditCompleted;
                    Arry.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), _SUM, parseInt(InspectionDone, 10)]);
                });

                var ActWiseData = [];
                var _SUM1 = 0;

                angular.forEach(data, function (j) {
                    _SUM1 += parseInt(j.SocietyCount);
                });

                ActWiseData.push({ ActName: (_language == 'en-us' ? 'Total' : 'कुल'), Count: _SUM1 });

                angular.forEach(ActData, function (item) {

                    var ActCountData = data.filter(function (P) { P.ActNameEn == item.ActNameEn });
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
                    vAxis: { title: (_language == 'en-us' ? 'No. of Count' : 'संख्या') },
                    hAxis: {
                        title: (_language == 'en-us' ? 'District Name' : 'जिला का नाम'), textStyle: {
                            fontSize: 10 // or the number you want
                        }
                    },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };


                var chart = new google.visualization.ColumnChart(document.getElementById('InspectionChart'));

                chart.draw(data, options);
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

        //if (_language == 'en-us')
        //    ArryPM.push(['District', 'Societies']);
        //else
        //    ArryPM.push(['District', 'Societies']);
        var TypeLocalPM = ''
        if (TypePM == 'fr') {
            TypeLocalPM = 'Farmer Benefited';
            ArryPM.push(['District', 'Farmer']);
        }
        else if (TypePM == 'fd') {
            TypeLocalPM = 'Fund Distributed';
            ArryPM.push(['District', 'Fund']);
        } else {
            TypeLocalPM = 'Farmer Benefited and Fund Distributed';
            ArryPM.push(['District', 'Farmer', 'Fund']);
        }
        var StatusLocalPM = ''

        if (StatusPM == '1') {
            StatusLocalPM = 'Phase 1';
            Title = (_language == 'en-us' ? '(In Crore ₹)' : '(करोड़ में ₹)');
            if (TypePM == 'fr') {

                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IstInstallment)]);
                    });
                }

            }
            else if (TypePM == 'fd') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IstInstallmentAmount)]);
                    });
                }
            }

        }
        else if (StatusPM == '2') {
            Title = (_language == 'en-us' ? '(In Crore ₹)' : '(करोड़ में ₹)');
            StatusLocalPM = 'Phase 2';
            if (TypePM == 'fr') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IIstInstallment)]);
                    });
                }
            }
            else if (TypePM == 'fd') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IIstInstallmentAmount)]);
                    });
                }
            }
        }
        else if (StatusPM == '3') {
            Title = (_language == 'en-us' ? '(In Crore ₹)' : '(करोड़ में ₹)');
            StatusLocalPM = 'Phase 3';
            if (TypePM == 'fr') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IIIrdInstallment)]);
                    });
                }
            }
            else if (TypePM == 'fd') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IIIrdInstallmentAmount)]);
                    });
                }
            }
        }
        else if (StatusPM == '4') {
            Title = (_language == 'en-us' ? '(In Crore ₹)' : '(करोड़ में ₹)');
            StatusLocalPM = 'Phase 4';
            if (TypePM == 'fr') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IVthInstallment)]);
                    });
                }
            }
            else if (TypePM == 'fd') {
                if ($scope.PMkisanDataForDashboard != null && $scope.PMkisanDataForDashboard != undefined && $scope.PMkisanDataForDashboard.length > 0) {
                    angular.forEach($scope.PMkisanDataForDashboard, function (item) {
                        ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseInt(item.IVthInstallmentAmount)]);
                    });
                }
            }
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
        $scope.FertilizerOpeningStockParam = Fertilizer;

        ArryPM.push([_language == 'en-us' ? 'District' : 'जिले', _language == 'en-us' ? 'Opening Stock' : 'ओपनिंग स्टॉक']);

        if ($scope.FertilizerOpeningStockData != null && $scope.FertilizerOpeningStockData != undefined && $scope.FertilizerOpeningStockData.length > 0) {

            var FertilizerOpeningStock = $scope.FertilizerOpeningStockData.filter(function (x) { return x.FertilizerName_En == Fertilizer });

            if (FertilizerOpeningStock.length > 0) {
                angular.forEach(FertilizerOpeningStock, function (item) {
                    ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseFloat(item.Total)]);
                });
            }

            var data = google.visualization.arrayToDataTable(ArryPM);
            var options = {
                title: _language == 'en-us' ? ('District Wise ' + Fertilizer + ' Opening Stock') : ('जिले वार' + Fertilizer + ' ओपनिंग स्टॉक'),
                vAxis: { title: _language == 'en-us' ? '(Thousand MT)' : '(हजार एमटी)' },
                hAxis: {
                    title: (_language == 'en-us' ? '(District)' : '(जिला)'), textStyle: {
                        fontSize: 10 // or the number you want
                    }
                },
                seriesType: 'bars',
                series: { 5: { type: 'line' } }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('FertiChart'));
            chart.draw(data, options);
        }


        //});


    }
    $scope.drawVisualizationFertilizerRetailerTransaction = function (District) {
        var Count = 1;
        var ArryPM = [];
        var Title = '';

        ArryPM.push([_language == 'en-us' ? 'District' : 'जिले', _language == 'en-us' ? 'Fertilizer Sold' : 'उर्वरक बिक्री']);

        if ($scope.FertilizerTransactionData != null && $scope.FertilizerTransactionData != undefined && $scope.FertilizerTransactionData.length > 0) {
            angular.forEach($scope.FertilizerTransactionData, function (item) {
                ArryPM.push([(_language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL), parseFloat(item.QuantitySold)]);
            });


            var data = google.visualization.arrayToDataTable(ArryPM);
            var options = {
                title: _language == 'en-us' ? ('District Wise Fertilizer Sold') : ('जिले वार उर्वरक बिक्री'),
                vAxis: { title: _language == 'en-us' ? '(Thousand MT)' : '(हजार एमटी)' },
                hAxis: {
                    title: (_language == 'en-us' ? '(District)' : '(जिला)'), textStyle: {
                        fontSize: 10 // or the number you want
                    }
                },
                seriesType: 'bars',
                series: { 5: { type: 'line' } }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('FertilizerDistrictChart'));
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
    //$scope.drawVisualizationDivReportGraph = function () {      
    //    var Arry = [];        
    //    if ($scope.DivisionReport.length > 0) {
    //        if (_language == 'en-us')
    //            Arry.push(['Division', 'Auditable Societies', 'Audited Societies Till Current Month']);
    //        else
    //            Arry.push(['संभाग', 'ऑडिट योग्य समितियाँ ', 'चालू माह तक लेखा परीक्षित सोसाइटी']);

    //        angular.forEach($scope.DivisionReport, function (item) {
    //            Arry.push([(_language == 'en-us' ? item.DIVISION_ENG : item.DIVISION_MANGAL), parseInt(item.Auditable_Societies), parseInt(item.Audited_Societies_Till_Current_Month)]);
    //        });            

    //        var data = google.visualization.arrayToDataTable(Arry);
    //        var options = {
    //            title: (_language == 'en-us' ? 'Division Wise Audit Progress' : 'डिवीजन वाइज ऑडिट प्रोग्रेस'),
    //            vAxis: { title: (_language == 'en-us' ? 'Society Count' : 'सोसाइटी काउंट') },
    //            hAxis: { title: (_language == 'en-us' ? 'Division' : 'डिवीजन') },
    //            seriesType: 'bars',
    //            series: { 5: { type: 'line' } }
    //        };
    //    }

    //    var chart = new google.visualization.ColumnChart(document.getElementById('ChartDivisionWiseAuditProgress'));
    //    chart.draw(data, options);
    //}


    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");

        //$scope.GetPacsStatusReport();
        //$scope.GetPacsdeshdata();

        //$scope.GetFertiliserDataForDashBoard();


        //$scope.GetPMkisanDataForDashboard();
        //GetFertiliserDataForDashBoard_1();
        //GetPMkisanDataForDashboard_1();
        //$scope.PM_Kisan_Application_Status();


        //$scope.GetDivisionReport();

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

    function PM_Kisan_Application_Status_1() {
        $.ajax({
            url: SiteUrl + "/CommonServices/PM_Kisan_Application_Status",
            async: false,
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                $scope.PatwariEsign = 0;
                $scope.PatwariReject = 0;
                $scope.TehsildarEsign = 0;
                $scope.TehsildarReject = 0;
                $scope.Collectorsign = 0;
                $scope.CollectorReject = 0;
                $scope.TotAppRec = 0;

                angular.forEach(data, function (item) {
                    $scope.PatwariEsign = parseInt($scope.PatwariEsign) + parseInt(item.PAT_ESIGN_STATUS);
                    $scope.PatwariReject = parseInt($scope.PatwariReject) + parseInt(item.PATWARI_REJ);

                    $scope.TehsildarEsign = parseInt($scope.TehsildarEsign) + parseInt(item.TEH_ESIGN_STATUS);
                    $scope.TehsildarReject = parseInt($scope.TehsildarReject) + parseInt(item.TEHSILDAR_REJ);

                    $scope.Collectorsign = parseInt($scope.Collectorsign) + parseInt(item.COLLECTOR_ESIGN_STATUS);
                    $scope.CollectorReject = parseInt($scope.CollectorReject) + parseInt(item.COLLECTER_REJ);
                    $scope.TotAppRec = parseInt($scope.TotAppRec) + parseInt(item.TotalApplication);


                });

                $scope.PM_Kisan_Application_Status_Data = data;
            }
        });
    }



    //Start EmitraPendancyServiceCallDaily
    function getCurrentDate() {
        const date = new Date();
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    var currentDate = getCurrentDate();    
    $scope.EmitraPendancyService = function () {
      

        $http.get(SiteUrl + "/User/IsEMITRATHIRDPARTYPENDENCYSERVICECall").
            success(function (data, status) {
                if (data > 0)
                    localStorage.setItem('Date', currentDate);
            }).
            error(function (data, status) {
                localStorage.clear();
            });
    }
    if (localStorage.length === 0 || localStorage.getItem('Date') != currentDate) {

        localStorage.clear();
        $scope.EmitraPendancyService();
    }
    //End EmitraPendancyServiceCallDaily


    GetAlldeshdata_1();
    current_progress_Status(10);
    //google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(FertilizerOpeningStockReport);
    // FertilizerOpeningStockReport();
    current_progress_Status(25);
    //GetPacsStatusReport_1();
    // GetPacsdeshdata_1();
    GetDivisionReport_1();


    $scope.OnPageLoad();




});
