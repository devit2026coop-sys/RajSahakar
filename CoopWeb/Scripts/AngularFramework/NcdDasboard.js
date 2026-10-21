
var MasterMod = angular.module('ncdMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.ncd = {};
    $scope.SectorList = [];
    $scope.NCDStateDetails = [];
    $scope.SocietyFunctionalStatusList = [];
    $scope.NCD_Statecode = '';
    $scope.totalCooperatives = 0;
    $scope.totalMembers = 0;
    $scope.DistrictList = [];
    $scope.SectorWiseDistrictList = [];
    $scope.NCDDistrictDetails = [];
    $scope.activeTab = "sectorTab";
    $scope.ncd.district_code = "0";
    $scope.DISTRICT_ID = 0;
    $scope.NCDSectorDetails = [];
    $scope.isNoRecord = false;
    $scope.ncd.functional_status = "0"

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



        if ($scope.DISTRICT_ID == 0) {
            $scope.loadSectorList();
            $scope.loadDistrictList();
            $scope.loadSectorWise();
        }

        $scope.GetSocietyFunctionalStatus();
    };

    $scope.setActiveTab = function (tabName) {
        if (tabName === "sectorTab") {
            $scope.ncd.district_code = ($scope.ncd.district_code == "0") ? "0" : $scope.ncd.district_code;
            $scope.ncd.functional_status = "0";
            $scope.activeTab = tabName;
            $scope.loadSectorList();
        } else if (tabName === "districtTab") {
            $scope.ncd.district_code = ($scope.ncd.district_code == "0") ? "0" : $scope.ncd.district_code;
            $scope.ncd.functional_status = "0";
            $scope.activeTab = tabName;
            $scope.loadDistrictList();
        } else if (tabName === "sectorMembersTab") {
            $scope.ncd.district_code = ($scope.ncd.district_code == "0") ? "0" : $scope.ncd.district_code;
            $scope.ncd.functional_status = "0";
            $scope.activeTab = tabName;
            $scope.loadSectorWise();
        }
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

    $scope.clearSearch = function () {
        $scope.ncd = {};
        $scope.SectorList = [];
        $scope.DistrictList = [];
        $scope.SectorWiseDistrictList = [];
    };

    $scope.GetDistrictByStatecode = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                    $scope.NCDDistrictDetails = JSON.parse(data.Result.Data.NCDDistrictDetail);
                    if ($("#hdnDistrictId").val() != "0") {
                        $scope.ncd.district_code = $scope.NCDDistrictDetails.filter(x => x.DistrictId == $("#hdnDistrictId").val())[0].district_code;
                        $("#district").attr("disabled", "disabled");
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

    $scope.loadSectorList = function () {
        // Build filter query params
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : "0",
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : "0"
        };
        $http({ method: "GET", url: SiteUrl + "/NcdData/GetSectorDashboard", params: params }).success(function (response) {
            if (response.Result.Status == "200") {
                // Parse JSON into array
                $scope.SectorList = JSON.parse(response.Result.Data.SectorDashboard);
                $scope.isNoRecord = false;
                if ($scope.SectorList.length == 0) {
                    $scope.isNoRecord = true;
                    $scope.colspan = 4;
                }
                google.charts.load('current', { packages: ['corechart'] });
                google.charts.setOnLoadCallback(function () {
                    $scope.drawVisualizationSectorWise();
                });
            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };
    $scope.loadDistrictList = function () {
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : "0",
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : "0"
        };

        $http({
            method: "GET", url: SiteUrl + "/NcdData/GetDistrictDashboard", params: params
        }).success(function (response) {
            if (response.Result.Status == "200") {
                $scope.DistrictList = JSON.parse(response.Result.Data.DistrictDashboard);
                google.charts.load('current', { packages: ['corechart'] });
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
        var params = {
            fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
            toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : "0",
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : "0"
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
        $scope.ncd.district_code = "0";
        $scope.ncd.functional_status = "0";
        $scope.loadSectorList();
        $scope.loadSectorWise();
        $scope.loadDistrictList();
    };

    $scope.ExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        $scope.drawVisualizationSectorWise();
    };

    $scope.drawVisualizationSectorWise = function () {
        var Arr = [];
        Arr.push(['Sector', 'No. of Societies', 'No. of Members']);
        var filterData = $scope.SectorList.filter(x => x.SectorName !== "Total");
        angular.forEach(filterData, function (item) {
            Arr.push([item.SectorName, parseInt(item.NumberOfSocieties) || 0, parseInt(item.NumberOfMembers) || 0,]);
        });

        var data = google.visualization.arrayToDataTable(Arr);
        var options = {
            title: 'SECTOR WISE SOCIETIES',
            vAxis: {
                title: 'No. of Societies',
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
                0: { color: 'rgb(65, 131, 78)' },
                1: { color: 'rgb(217, 237, 221)' }
            }
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('SectorChart'));
        chart.draw(data, options);
    };

    $scope.drawDistrictPieCharts = function () {
        if (!$scope.DistrictList || $scope.DistrictList.length === 0) return;

        $scope.DistrictList = $scope.DistrictList.filter(x => x.DistrictName !== "Total");
        var totalSocieties = $scope.DistrictList.reduce(function (acc, item) {
            return acc + (parseInt(item.NumberOfCooperatives) || 0);
        }, 0);
        var dataArray = [['DistrictName', 'Societies', { role: 'tooltip', type: 'string', p: { html: true } }]];
        $scope.DistrictList.forEach(function (item) {
            var Societies = parseInt(item.NumberOfCooperatives) || 0;
            var Members = parseInt(item.NumberOfMembers) || 0;
            var percent = totalSocieties ? ((Societies / totalSocieties) * 100).toFixed(2) : '0.00';
            var tooltipHtml = '<div style="padding:5px; font-family: Arial, sans-serif; font-size: 11px;">' +
                '<strong>' + item.DistrictName + '</strong><br>' + 'Societies: ' + $scope.numberWithCommas(Societies) + '<br>' + 'Members: ' + $scope.numberWithCommas(Members) + '<br>' + 'Percentage: ' + percent + '%' +
                '</div>';
            dataArray.push([item.DistrictName, Societies, tooltipHtml]);

        });

        var data = google.visualization.arrayToDataTable(dataArray);
        var options = {
            title: 'District wise chart',
            pieHole: 0.3,
            is3D: true,
            pieSliceText: 'percentage',
            pieSliceTextStyle: { color: 'black', fontSize: 12 },
            allowOverlap: true,
            chartArea: { width: '85%', height: '80%' },
            legend: { position: 'right' },
            tooltip: { isHtml: true },
            animation: { duration: 300, easing: 'out' },
            allowPointSelect: true,
            cursor: 'pointer',
            colors: ['#0890C2', '#EF701F', '#C0CF8B', '#7FC8A9', '#A6C8FF',
                '#F4B6C2', '#FFD59E', '#B39DDB', '#80CBC4', '#FFAB91',
                '#F7D6A5', '#B2DFDB', '#C5E1A5', '#E1BEE7', '#FFCC80',
                '#90CAF9', '#FFE082', '#A5D6A7', '#FF8A65', '#DCE775',
                '#81D4FA', '#CE93D8', '#FFF59D', '#B0BEC5', '#FFAB91',
                '#F48FB1', '#AED581', '#64B5F6', '#FFD54F', '#4DB6AC',
                '#BA68C8', '#FFB74D', '#4FC3F7', '#F06292', '#AED581',
                '#7986CB', '#FF8A65', '#4DD0E1', '#E57373', '#FFB74D',
                '#4DB6AC']
        };

        var chart = new google.visualization.PieChart(document.getElementById('DistrictPieChart'));
        chart.draw(data, options);


    };

    $scope.numberWithCommas = function (x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    $scope.GetDistrictByStatecode();
    $scope.OnPageLoad();
});