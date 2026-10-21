
var MasterMod = angular.module('ncdAnaMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdAnaCtrl", function ($scope, $http) {
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

        //Added by Bhavesh
        if ($scope.DISTRICT_ID ==null) {
            $scope.GetSocietyFunctionalStatus();
            $scope.loadSectorList();
            /*$scope.loadDistrictList();*/
            /*$scope.loadSectorWise();*/
            /*$scope.loadSectorWise();*/
            /*  Bhavesh*/
            $scope.prepareSectorData();  
            $scope.loadBlockCoverage();
            $scope.loadRegisteredSocietiesTrend();
            $scope.loadFunctionalStatusTrend();
            $scope.loadFunctionalStatusTrendDairy();
            $scope.loadFunctionalStatusTrendFishery();
        }

    };

    $scope.BankTypes = [];
    $scope.BankTypes = [
        { value: 0, label: 'Cooperative Bank' },
        { value: 1, label: 'Commercial Bank' }
    ];


    $scope.NCDSectorDetails = [];
    $scope.SocietyFunctionalStatusList = [];

    $scope.GetSocietyFunctionalStatus = function () {

        $scope.SocietyFunctionalStatusList = [];
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/NcdDataAnalytical/GetSocietyFunctionalStatus")
            .success(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response && response.Result && response.Result.Status === "200") {
                    debugger
                    var detail = response.Result.Data &&
                        response.Result.Data.SocietyFunctionalStatusDetail;

                    if (detail) {
                        $scope.SocietyFunctionalStatusList =
                            angular.isString(detail) ? JSON.parse(detail) : detail;
                    }

                } else {
                    console.log(
                        "Error fetching functional status:",
                       ( response && response.Result)
                            ? response.Result.Message
                            : "Invalid response format"
                    );
                }
            })
            .error(function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.log("HTTP error while fetching functional status:", error);
            });
    };


    $scope.loadSectorList = function () {
        
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : null,
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : null
        };

        $http({
            method: "GET",
            
            url: SiteUrl + "/NcdDataAnalytical/GetSectorDashboard",
            params: params
        }).success(function (response) {
            debugger
            if (response.Result.Status == "200") {
                
                $scope.SectorList = JSON.parse(response.Result.Data.SectorDashboard);
                google.charts.load('current', { packages: ['corechart'] });
                google.charts.setOnLoadCallback(function () {
                    debugger
                       $scope.prepareSectorData();
                });

            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };


    $scope.loadBlockCoverage = function () {
        $http.get(SiteUrl + "/NcdDataAnalytical/GetBlockCoverageDashboard", {
            params: {
                districtId: $scope.ncd.district_code || null
            }
        }).success(function (response) {
            if (response.Result.Status === "200") {
                $scope.coverageCounts = {
                    Fully: response.Result.Data.Fully,
                    Partial: response.Result.Data.Partial,
                    Not: response.Result.Data.Not,
                    TotalGP: response.Result.Data.TotalGP
                };

                google.charts.setOnLoadCallback($scope.drawBlockCoveragePieChart);
            }
        });
    };

    $scope.loadRegisteredSocietiesTrend = function () {
        $http.get(SiteUrl + "/NcdDataAnalytical/GetRegisteredSocietiesTrend")
            .success(function (response) {
                if (response.Result.Status === "200") {
                    $scope.societyTrendData =
                     JSON.parse(response.Result.Data.TrendData);
                    google.charts.setOnLoadCallback($scope.drawRegisteredSocietiesLineChart);
                }
            });
    };

    $scope.loadFunctionalStatusTrend = function () {
        $http.get(SiteUrl + "/NcdDataAnalytical/GetFunctionalStatusTrend")
            .success(function (response) {
                if (response.Result.Status === "200") {
                    $scope.functionalStatusData =
                        JSON.parse(response.Result.Data.TrendData);
                    google.charts.setOnLoadCallback($scope.drawFunctionalStatusChart);
                }
            });
    };

    $scope.loadFunctionalStatusTrendDairy = function () {
        
        $http.get(
            SiteUrl + "/NcdDataAnalytical/GetFunctionalStatusTrendDairy"
        )
            .success(function (response) {
                
                if (response.Result.Status === "200") {

                    $scope.functionalStatusDairyData =
                        JSON.parse(response.Result.Data.TrendData);

                    google.charts.setOnLoadCallback(
                        $scope.drawFunctionalStatusChartDairy
                    );
                }
            });
    };

    $scope.loadFunctionalStatusTrendFishery = function () {
      
        $http.get(SiteUrl + "/NcdDataAnalytical/GetFunctionalFisheryStatusTrend")
            .success(function (response) {
  
                if (response.Result.Status === "200") {

                    $scope.functionalStatusFisheryData =
                        JSON.parse(response.Result.Data.TrendData);

                    google.charts.setOnLoadCallback(
                        $scope.drawFunctionalStatusChartFishery
                    );
                }
            });
    };



    $scope.resetSearch = function () {
        $scope.ncd.from_date = null;
        $scope.ncd.to_date = null;
        $scope.ncd.district_code = null;
        $scope.ncd.functional_status = null;
        /*$scope.loadSectorList();*/
        $scope.loadSectorWise();
        /*$scope.loadDistrictList();*/
    };

    $scope.setActiveTab = function (tabName) {
        $scope.activeTab = tabName;
    };

    $scope.searchDashboard = function () {
        if ($scope.activeTab === "sectorTab") {
            //$scope.loadSectorList();
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
   
    // Start code fro Analytical Chart

    $scope.FilteredSectors = [];
    FilterBetween2000_22000 = [];
    FilterLess2000 = [];
   
    $scope.prepareSectorData = function () {
   
        $scope.FilteredSectors = $scope.SectorList.filter(x =>
            parseInt(x.NumberOfSocieties) > 3000
        );

        $scope.FilterBetween2000_22000 = $scope.SectorList.filter(x => {
            let count = parseInt(x.NumberOfSocieties);
            return count >= 2000 && count < 22000;
        });

        $scope.FilterLess2000 = $scope.SectorList.filter(x =>
            parseInt(x.NumberOfSocieties) < 2000
        );

        
        if ($scope.FilteredSectors.length > 0) {
            setTimeout(function () {
                $scope.drawVisualizationAnayticalSectorWise();
            }, 200);
        }

        if ($scope.FilterBetween2000_22000.length > 0) {
            setTimeout(function () {
                $scope.drawVisualizationAnayticalSectorWise();
            }, 200);
        }

        if ($scope.FilterLess2000.length > 0) {
            setTimeout(function () {
                $scope.drawVisualizationAnayticalSectorWise();
            }, 200);
        }
    };

    $scope.drawVisualizationAnayticalSectorWise = function () {

    function drawChart(dataArr, title, containerId) {

        if (dataArr.length <= 1) return;

        var data = google.visualization.arrayToDataTable(dataArr);

        var options = {
            title: title,

            vAxes: {
                0: { title: 'No. of Cooperatives', format: "short" },
                1: { title: 'Member Count', format: "short" }
            },

            series: {
                0: { type: 'bars', targetAxisIndex: 0 },
                1: { type: 'line', targetAxisIndex: 1 }
            },

            hAxis: {
                slantedText: true,
                slantedTextAngle: 90,
                textStyle: { fontSize: 10 }
            },

            legend: { position: 'bottom' },
            chartArea: { width: "80%", height: "70%" }
        };

        var container = document.getElementById(containerId);
        if (!container) return;

        var chart = new google.visualization.ComboChart(container);
        chart.draw(data, options);
    }

    /* --------- > 22000 --------- */
    var arrMore22000 = [['Sector', 'Societies Count', 'Member Count']];
    angular.forEach($scope.FilteredSectors, function (item) {
        arrMore22000.push([
            item.SectorName,
            +item.NumberOfSocieties || 0,
            +item.NumberOfMembers || 0
        ]);
    });
    drawChart(arrMore22000, 'SOCIETIES COUNT MORE THAN 22,000', 'container');

    /* --------- 2000 - 22000 --------- */
    var arrBetween = [['Sector', 'Societies Count', 'Member Count']];
    angular.forEach($scope.FilterBetween2000_22000, function (item) {
        arrBetween.push([
            item.SectorName,
            +item.NumberOfSocieties || 0,
            +item.NumberOfMembers || 0
        ]);
    });
    drawChart(arrBetween, 'SOCIETIES COUNT BETWEEN 2,000 AND 22,000', 'containerr');

    /* --------- < 2000 --------- */
    var arrLess2000 = [['Sector', 'Societies Count', 'Member Count']];
    angular.forEach($scope.FilterLess2000, function (item) {
        arrLess2000.push([
            item.SectorName,
            +item.NumberOfSocieties || 0,
            +item.NumberOfMembers || 0
        ]);
    });
    drawChart(arrLess2000, 'SOCIETIES COUNT LESS THAN 2,000', 'containerrr');
};

    $scope.drawBlockCoveragePieChart = function () {

        var fully = $scope.coverageCounts.Fully || 0;
        var partial = $scope.coverageCounts.Partial || 0;
        var notCovered = $scope.coverageCounts.Not || 0;

        var total = fully + partial + notCovered;

        if (total === 0) {
            document.getElementById('CoveragePieChart').innerHTML =
                '<div style="text-align:center;padding:40px;font-size:16px;color:#666;">No coverage data available</div>';
            return;
        }

        function percent(val) {
            return ((val * 100) / total).toFixed(1) + '%';
        }

        var data = google.visualization.arrayToDataTable([
            
            ['Coverage', 'Count', { role: 'tooltip', type: 'string' }],
            ['Fully Covered: ' + fully + ' (' + percent(fully) + ')', fully,
            'Fully Covered\nCount: ' + fully + '\n' + percent(fully)],
            ['Partial Covered: ' + partial + ' (' + percent(partial) + ')', partial,
            'Partial Covered\nCount: ' + partial + '\n' + percent(partial)],
            ['Not Covered: ' + notCovered + ' (' + percent(notCovered) + ')', notCovered,
            'Not Covered\nCount: ' + notCovered + '\n' + percent(notCovered)]
        ]);

        var options = {
            title:
                'COVERAGE OF GRAM PANCHAYAT – PACS/LAMPS/FSS\n' +
                'Total Gram Panchayat in Country: ' + total + '\n' +
                'Total PACS/LAMPS/FSS Count: ' + ($scope.coverageCounts.Fully || 0),

            is3D: true,

            pieSliceText: 'none',   
            sliceVisibilityThreshold: 0,

            slices: {
                0: { color: '#1E88E5' },   // Fully
                1: { color: '#90EE90' },   // Partial
                2: { color: '#F4A261' }    // Not
            },

            legend: 'none',       

            chartArea: {
                width: '90%',
                height: '75%'
            }
        };

        var chart = new google.visualization.PieChart(
            document.getElementById('CoveragePieChart')
        );

        chart.draw(data, options);
    };


    $scope.drawRegisteredSocietiesLineChart = function () {
     
        var arr = [];
        arr.push([
            /* 'Year',*/
            { label: 'Index', type: 'number' },
            'PACS/LAMPS/FSS',
            'Dairy',
            'Fishery'
        ]);
        var ticks = [];
        angular.forEach($scope.societyTrendData, function (item, index) {
            var startYear = parseInt(item.YearRange.split('-')[0]);
            arr.push([
                startYear,
                Number(item.PACS),
                Number(item.Dairy),
                Number(item.Fishery)
            ]);
           
            ticks.push({
                v: startYear,
                f: item.YearRange
            });
        });

        var data = google.visualization.arrayToDataTable(arr);
        var options = {
            title: 'REGISTERED PRIMARY SOCIETIES (1900–AS ON 12/12/2025)',
            hAxis: {
                title: 'Year',
                ticks: ticks,
                gridlines: {
                    color: 'transparent'   
                },
                minorGridlines: {
                    color: 'transparent'   
                }
            },
            vAxis: {
                title: 'Total Registered Society',
                viewWindow: {min: 0 },
                format: 'short'
            },
            curveType: 'function',
            interpolateNulls: true,  
            legend: { position: 'right' },
            textStyle: {
                color: '#000',
                fontSize: 10
            },
            pointSize: 4,
            lineWidth: 3,
            chartArea: {
                width: '60%',
                height: '65%'
            }
        };

        var chart = new google.visualization.LineChart(
            document.getElementById('RegisteredSocietyLineChart')
        );

        chart.draw(data, options);
    };


    // Functinal Based Scoiety in Last
    $scope.drawFunctionalStatusChart = function () {
        
        var arr = [];
        arr.push(['Functional Status', 'Societies Count', { role: 'style' }, { role: 'annotation' }]);

        // 1️⃣ Calculate Grand Total
        var grandTotal = 0;
        angular.forEach($scope.functionalStatusData, function (item) {
            grandTotal += parseInt(item.total_count) || 0;
            
        });

        angular.forEach($scope.functionalStatusData, function (item) {
            var count = parseInt(item.total_count) || 0;
            var percent = grandTotal > 0
                ? ((count / grandTotal) * 100).toFixed(1)
                : 0;

            function getColor(status) {
                if (status === 'Functional')
                    return 'color: #4DA3FF';          // Blue
                else if (status === 'Non-functional')
                    return 'color: #7ED957';          // Green
                else if (status === 'Under Liquidation')
                    return 'color: #F4A261';          // Orange
                else
                    return 'color: #999999';
            }

            arr.push([
                item.FunctionalStatusEn,
                count,
                getColor(item.FunctionalStatusEn),
                count + " (" + percent + "%)"
            ]);
        });

        
        var options = {
            title: 'PACS/LAMPS/FSS',
            legend: { position: 'none' },

            is3D: true,
            colors: ['#3399FF'],
            annotations: {
                alwaysOutside: true,
                textStyle: {
                    fontSize: 12,
                    bold: true
                }
            },
            vAxis: {
                title: 'Total PACS / LAMPS / FSS Count',
                format: 'short',
                minValue: 0

            },
            hAxis: {
                textStyle: { fontSize: 11 }
            },
            chartArea: {
                width: '75%',
                height: '70%'
            }
            
        };

        var container = document.getElementById('FunctionalStatusChart');
        if (container) {
            var chart = new google.visualization.ColumnChart(container);
            chart.draw(
                google.visualization.arrayToDataTable(arr),
                options
            );
        }
    };


    $scope.drawFunctionalStatusChartDairy = function () {
        

        var dataArr = [];
        dataArr.push([
            'Functional Status',
            'Total Count',
            { role: 'style' }, 
            { role: 'annotation' }
        ]);

        // 1️⃣ Calculate total
        var grandTotal = 0;
        angular.forEach($scope.functionalStatusDairyData, function (item) {
            grandTotal += parseInt(item.total_count) || 0;
        });

        function getColor(status) {
            if (status === 'Functional')
                return 'color: #4DA3FF';          // Blue
            else if (status === 'Non-functional')
                return 'color: #7ED957';          // Green
            else if (status === 'Under Liquidation')
                return 'color: #F4A261';          // Orange
            else
                return 'color: #999999';
        }


        // 2️⃣ Build chart rows
        angular.forEach($scope.functionalStatusDairyData, function (item) {

            var count = parseInt(item.total_count) || 0;
            var percent = grandTotal > 0
                ? ((count / grandTotal) * 100).toFixed(1)
                : 0;

            dataArr.push([
                item.FunctionalStatusEn,
                count,
                  getColor(item.FunctionalStatusEn),
                count + " (" + percent + "%)"
            ]);
        });

        var data = google.visualization.arrayToDataTable(dataArr);

        // 3️⃣ Chart options (matched to image)
        var options = {
            title: 'DAIRY',
            legend: { position: 'none' },
            bar: { groupWidth: '45%' },
            colors: ['#4DA3FF', '#7ED957', '#F4A261'],
            annotations: {
                alwaysOutside: true,
                textStyle: {
                    fontSize: 12,
                    bold: true
                }
            },
            is3D: true,
            tooltip: {
                isHtml: true
            },
            vAxis: {
                title: 'Total Dairy Count',
                format: 'short',
                minValue: 0
            },
            hAxis: {
                textStyle: { fontSize: 11 }
            },
            chartArea: {
                width: '75%',
                height: '70%'
            }
        };

        var container = document.getElementById('FunctionalStatusChartDairy');
        if (container) {
            var chart = new google.visualization.ColumnChart(container);
            chart.draw(data, options);
        }
    };

    $scope.drawFunctionalStatusChartFishery = function () {
        
        var dataArr = [];
        dataArr.push([
            'Functional Status',
            'Total Count',
            { role: 'style' },
            { role: 'annotation' }
        ]);

        // Calculate total
        var grandTotal = 0;
        angular.forEach($scope.functionalStatusFisheryData, function (item) {
            grandTotal += parseInt(item.total_count) || 0;
        });

        // Color mapping (same as pic)
        function getColor(status) {
            if (status === 'Functional')
                return 'color: #4DA3FF';          // Blue
            else if (status === 'Non-functional')
                return 'color: #7ED957';          // Green
            else if (status === 'Under Liquidation')
                return 'color: #F4A261';          // Orange
            else
                return 'color: #999999';
        }

        angular.forEach($scope.functionalStatusFisheryData, function (item) {

            var count = parseInt(item.total_count) || 0;
            var percent = grandTotal > 0
                ? ((count / grandTotal) * 100).toFixed(1)
                : 0;

            dataArr.push([
                item.FunctionalStatusEn,
                count,
                getColor(item.FunctionalStatusEn),
                count + " (" + percent + "%)"
            ]);
        });

        var data = google.visualization.arrayToDataTable(dataArr);

        var options = {
            title: 'FISHERY',
            legend: { position: 'none' },
            bar: { groupWidth: '45%' },
            annotations: {
                alwaysOutside: true,
                textStyle: { bold: true }
            },
            vAxis: {
                title: 'Total Fishery Count',
                format: 'short',
                minValue: 0
            },
            chartArea: {
                width: '75%',
                height: '70%'
            }

        };

        var chart = new google.visualization.ColumnChart(
            document.getElementById('FunctionalStatusChartFishery')
        );
        chart.draw(data, options);
    };


    $scope.OnPageLoad();
});