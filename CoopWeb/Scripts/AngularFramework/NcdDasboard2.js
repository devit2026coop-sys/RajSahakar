
var MasterMod = angular.module('ncdMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.ncd = {};
    $scope.SectorList = [];
    $scope.NCDStateDetails = [];
    $scope.NCDDistrictDetail = [];
    $scope.SocietyFunctionalStatusList = [];
    $scope.NCD_Statecode = '';
    $scope.totalCooperatives = 0;
    $scope.totalMembers = 0;
    $scope.DistrictList = [];
    $scope.SectorWiseDistrictList = [];
    $scope.NCDDistrictDetails = [];
    $scope.activeTab = "sectorTab";
    $scope.ncd.district_code = "0";
    $scope.DISTRICT_ID = null;
    if ($("#hdnDistrictId").val() != null && $("#hdnDistrictId").val() != "0") {
        $scope.DISTRICT_ID = $("#hdnDistrictId").val();
    }
    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }



        //Added by jaya
        if ($scope.DISTRICT_ID ==null) {
            $scope.loadSectorList();
            $scope.loadDistrictList();
            $scope.loadSectorWise();
        }

        $scope.GetSocietyFunctionalStatus();
    };

    $scope.BankTypes = [];
    $scope.BankTypes = [
        { value: 0, label: 'Cooperative Bank' },
        { value: 1, label: 'Commercial Bank' }
    ];


    $scope.NCDSectorDetails = [];

    $scope.GetDistrictByStatecode = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                    $scope.NCDDistrictDetails = JSON.parse(data.Result.Data.NCDDistrictDetail);
                    if ($("#hdnDistrictId").val() != "0") {
                        $scope.ncd.district_code = $scope.NCDDistrictDetails.filter(x => x.DistrictId == $("#hdnDistrictId").val())[0].district_code;
                        $scope.loadSectorList();
                        $scope.loadDistrictList();
                        $scope.loadSectorWise();
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SocietyFunctionalStatusList = [];

    $scope.GetSocietyFunctionalStatus = function () {
        $scope.SocietyFunctionalStatusList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSocietyFunctionalStatus")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.SocietyFunctionalStatusDetail) {
                        $scope.SocietyFunctionalStatusList = JSON.parse(response.data.Result.Data.SocietyFunctionalStatusDetail);
                    }
                } else {
                    console.error("Error fetching functional status:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching functional status:", error);
            });
    };


    //Added by jaya

    $scope.loadSectorList = function () {


        // Build filter query params
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : null,
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : null
        };

        $http({
            method: "GET",
            url: SiteUrl + "/NcdData/GetSectorDashboard",
            params: params
        }).success(function (response) {
            if (response.Result.Status == "200") {
                // Parse JSON into array
                $scope.SectorList = JSON.parse(response.Result.Data.SectorDashboard);

                // Calculate totals
                $scope.totalSocieties = 0;
                $scope.totalMembers = 0;
                angular.forEach($scope.SectorList, function (item) {

                    $scope.totalSocieties += parseInt(item.NumberOfSocieties) || 0;
                    $scope.totalMembers += parseInt(item.NumberOfMembers) || 0;
                });
                // Draw chart after data received Bhavesh 
                google.charts.load('current', { packages: ['corechart'] });
                google.charts.setOnLoadCallback(function () {
                    debugger;
                    $scope.drawVisualizationSectorWise();
                    $scope.drawDistrictPieCharts();
                });
               
            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };



    $scope.loadDistrictList = function () {

        // Build filter query params
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : null,
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : null
        };

        $http({
            method: "GET",
            url: SiteUrl + "/NcdData/GetDistrictDashboard",
            params: params
        }).success(function (response) {
            if (response.Result.Status == "200") {
                // Parse JSON into array
                $scope.DistrictList = JSON.parse(response.Result.Data.DistrictDashboard);
                // Calculate totals
                $scope.totalCooperatives = 0;
                $scope.totalMembers = 0;
                $scope.DistrictNames = [];
                angular.forEach($scope.DistrictList, function (item) {

                    $scope.totalCooperatives += parseInt(item.NumberOfCooperatives) || 0;
                    $scope.totalMembers += parseInt(item.NumberOfMembers) || 0;
                    
                    $scope.DistrictNames.push(item.DistrictName);
                    
                });
                // Load chart after data
                
                google.charts.setOnLoadCallback(function () {
                    $scope.drawDistrictPieCharts();
                });

            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };


    $scope.loadSectorWise = function () {

        // Build filter query params
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : null,
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : null
        };

        $http({
            method: "GET",
            url: SiteUrl + "/NcdData/GetSocietyMemberDashboard",
            params: params
        }).then(function (response) {
            if (response.data && response.data.Result && response.data.Result.Status === "200") {
                $scope.SectorWiseDistrictList = JSON.parse(response.data.Result.Data.SectorWiseDistrictDashboard);
                if ($scope.SectorWiseDistrictList.length > 0) {
                    $scope.TableColumns = Object.keys($scope.SectorWiseDistrictList[0]);
                }
            } else {
                ShowOkMessage("Error", response.data?.Result?.Message || "No records found.");
            }
        }).catch(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };

    $scope.resetSearch = function () {
        $scope.ncd.from_date = null;
        $scope.ncd.to_date = null;
        $scope.ncd.district_code = null;
        $scope.ncd.functional_status = null;
        $scope.loadSectorList();
        $scope.loadSectorWise();
        $scope.loadDistrictList();
    };

    $scope.setActiveTab = function (tabName) {
        $scope.activeTab = tabName;
    };

    $scope.searchDashboard = function () {
        if ($scope.activeTab === "sectorTab") {
            $scope.loadSectorList();
        } else if ($scope.activeTab === "districtTab") {
            $scope.loadDistrictList();
        } else if ($scope.activeTab === "sectorMembersTab") {
            $scope.loadSectorWise();
        }
    };

    // Clear Filters
    $scope.clearSearch = function () {
        $scope.ncd = {};
        $scope.SectorList = [];
        $scope.DistrictList = [];
        $scope.SectorWiseDistrictList = [];
    };

    // Added By Bhavesh 
    
    $scope.ExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        $scope.drawVisualizationSectorWise();
    };

    
    $scope.drawVisualizationSectorWise = function () {

        var Arr = [];
        Arr.push(['Sector', 'No. of Societies', 'No. of Members']);

        angular.forEach($scope.SectorList, function (item) {
            Arr.push([
                item.SectorName,
                parseInt(item.NumberOfSocieties) || 0,
                parseInt(item.NumberOfMembers) || 0,
                /*parse(item.DistrictName)*/
            ]);
        });

        var data = google.visualization.arrayToDataTable(Arr);

        var options = {
            title: 'ALL INDIA SECTOR-WISE COOPERATIVE SOCIETIES',

            vAxis: {
                title:'No. of Societies',
                logScale: true,
                ticks: [10, 100, 1000, 10000, 100000, 1000000],
                format: "short"
            },

            hAxis: {
                title: 'Sectors',
                slantedText: true,
                slantedTextAngle: 90,   
                textStyle: { fontSize: 10 }
            },

            legend: { position: 'RIGHT' },
            bar: { groupWidth: '80%' },
            seriesType: 'bars',
            series: {
                0: { color: 'rgb(65, 131, 78)' },   // Number of Societies (green bars)
                1: { color: 'rgb(217, 237, 221)' }    // Number of Members (light green line)
            }
        };

        var chart = new google.visualization.ColumnChart(
            document.getElementById('SectorChart')
        );
        chart.draw(data, options);
    };


    
    $scope.drawDistrictPieCharts = function () {
        debugger;

        var Arr = [];
        Arr.push(['DistrictName', 'Members']);
      /*  'Societies',, 'Size']*/
        angular.forEach($scope.DistrictList, function (item) {
            debugger
            Arr.push([
                //item.SectorName,
                item.DistrictName,
                //parseInt(item.DistrictName) ,
                //parseInt(item.NumberOfSocieties) || 0,
                parseInt(item.NumberOfMembers) || 0,
                /*parseInt(item.NumberOfSocieties) || 0,*/
                //parseInt(item.NumberOfSocieties) || 0  // Circle Size
            ]);
        });

        var data = google.visualization.arrayToDataTable(Arr);


        var options = {
            title: 'DISTRICT-SECTOR-WISE PIE CHART',
            pieHole: 0.3,
            legend: { position: 'right' },
            chartArea: { width: '85%', height: '80%' },
            is3D: true,
            //tooltip: { trigger: 'focus' },
            tooltip: {
                trigger: 'focus',
                isHtml: true
            },
            slices: {},
            animation: {
                duration: 300,
                easing: 'out'
            },
            allowPointSelect: true,
            cursor: 'pointer',
           /* depth: 5,*/
            //colorAxis: { colors: ['#A3D977', '#2E8B57'] }, // light → dark green
            //colorAxis: { colors: ['#E4F8D8', '#CFF3CF'] },
            colorAxis: {
                colors: ['#E4F8D8', '#DFFFD6', '#CFF3CF', '#D0F0E4']
                //colors: ['#E4F8D8', '#D6F5D6', '#CFF0CF', '#E6FFE6', '#F1FBEE']
            },


            legend: { position: 'right' },
        };

        var chart = new google.visualization.PieChart(
            document.getElementById('DistrictPieChart')
        );
        google.visualization.events.addListener(chart, 'onmouseover', function (e) {
            // Reset all slices
            options.slices = {};

            // Pop-out the hovered slice
            options.slices[e.row] = { offset: 0.15 };

            chart.draw(data, options);
        });

        google.visualization.events.addListener(chart, 'onmouseout', function (e) {
            // Reset ALL slices on mouse out
            options.slices = {};

            chart.draw(data, options);
        });

        chart.draw(data, options);
    }


    $scope.GetDistrictByStatecode();
    $scope.OnPageLoad();
});