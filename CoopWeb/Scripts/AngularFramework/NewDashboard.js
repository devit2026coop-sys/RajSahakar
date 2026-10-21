var MasterMod = angular.module('DashboardMod', []);
MasterMod.controller("DashboardCtrl", function ($scope, $http, $timeout, $q) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.MonthList = [];
    $scope.FinancialYearList = [];
    $scope.Department = [];
    $scope.SocietyList = [];
    $scope.dashboardData = {}
    $scope.countDetails = [];
    $scope.ActList = [];
    $scope.selectedAct = null;
    $scope.isNoRecord = false;

    $scope.searchRequest = {
        MonthId: 0,
        CurrentFinYear: 0,
        pendencyFilter: "today",
        divisionId: parseInt($('#DivisionId').val()),
        actId: $('#hdnActId').val(),
        districtId: parseInt($('#hdnDistrictId').val()),
        userId: parseInt($('#hdnUserId').val()),
        roleId: parseInt($('#hdnRoleId').val()),
        actionType: ""
    };

    $scope.pendencyList = [{ value: "today", text: "Today" }];
    $scope.chartData = [];


    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });

        }

        $scope.GetFinancialYear()
            .then(function () {
                return $scope.GetDepartment();
            })
            .then(function () {
                return $scope.GetMonthList();
            })
            .then(function () {
                // return $scope.GetAuditStatusChartData();
            })
            .then(function () {
                return $scope.GetSocietyAct();
            })
            .catch(function (error) {
                console.error("Error in chain:", error);
            });

        $(".se-pre-con").fadeOut("slow");

    };

    $scope.GetDepartment = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").
            success(function (data, status) {
                if (data != null) {
                    $scope.Department = data;
                } else {
                    $scope.Department = [];
                }
            }).
            error(function (data, status) {
                $scope.Department = [];
            });


    };
    $scope.GetFinancialYear = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                if (data != null) {
                    var filteredData = data.filter(function (x) { return x.FIN_YEAR_END != data[0].FIN_YEAR_END });
                    if (filteredData != null && filteredData.length > 0) {
                        $scope.FinancialYearList = filteredData.map(function (item) {
                            return {
                                FinYearId: item.FIN_YEAR_MASTR_ID,
                                FinYearName: 'FY: ' + item.FIN_YEAR_START + '-' + item.FIN_YEAR_END
                            };
                        });
                    }
                    if ($scope.FinancialYearList.length > 0) {
                        $scope.searchRequest.CurrentFinYear = $scope.FinancialYearList[0].FinYearId;
                        $scope.GetDashboardData();
                    } else {
                        $scope.FinancialYearList = [];
                    }
                }
            }).error(function (data, status) {
                $scope.FinancialYearList = [];
            });

    };

    $scope.GetMonthList = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
                if ($scope.MonthList != null && $scope.MonthList.length > 0) {
                    $scope.searchRequest.MonthId = String(new Date().getMonth() + 1).padStart(2, '0');
                    var monthData = $scope.MonthList.map(function (item) {
                        let monthNumber = item.Month_Id.padStart(2, '0');
                        let shortName = item.Month_En.substring(0, 3);
                        return monthNumber + " " + shortName;
                    });
                }
            }).error(function (data, status) {
                $scope.MonthList = [];
            });
    };

    $scope.GetSocietyRegistrationChartData = function (data) {
        var completed = 65; var inProgress = 25; var pending = 10;
        var series = [completed, inProgress, pending];

        var options = {
            chart: { type: 'donut', height: 280 },
            series: series,
            labels: ['Completed', 'In Progress', 'Pending'],
            colors: ['#2CA58D', '#7FD1C8', '#D3DDDC'],
            plotOptions: {
                pie: {
                    donut: {
                        size: '75%', labels: {
                            show: true,
                            name: { show: false },
                            value: {
                                //show: true, fontSize: '18px', fontWeight: 700, color: '#000000', offsetY: -10,
                                //formatter: function (w) { var total = w.globals.seriesTotals.reduce((a, b) => a + b, 0); return total.toLocaleString(); }
                            },
                            total: {
                                show: true, fontSize: '14px', color: '#000000', formatter: function (w) {
                                    var series = w.globals.series;
                                    var total = series.reduce((a, b) => a + b, 0);
                                    var completed = series[0];
                                    var percent = total ? ((completed / total) * 100).toFixed(0) : 0;
                                    return percent + '% done';
                                }
                            }
                        }
                    }
                }
            },

            legend: {
                show: true,
                position: 'bottom',
                horizontalAlign: 'center'
            },

            stroke: {
                width: 6,
                colors: ['#fff']
            }
        };

        var chart = new ApexCharts(document.querySelector("#chart"), options);
        chart.render();

    }

    $scope.GetSocietyRegistrationChartDataDynamicWorking = function (data) {
        let labels = [];
        let series = [];
        Object.keys(data[0]).forEach(function (key) {
            if (key === "Total" || key.includes("Percent")) return;

            labels.push(key);
            series.push(Number(data[key]) || 0);
        });

        var options = {
            chart: { type: 'donut', height: 280 },
            series: series,
            labels: labels,
            colors: ['#2CA58D', '#7FD1C8', '#D3DDDC'],
            plotOptions: {
                pie: {
                    donut: {
                        size: '75%',
                        labels: {
                            show: true,
                            total: {
                                show: true,
                                formatter: function (w) {
                                    var total = w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                                    var completed = w.globals.series[0];
                                    var percent = total ? ((completed / total) * 100).toFixed(0) : 0;
                                    return percent + '% done';
                                }
                            }
                        }
                    }
                }
            },

            legend: {
                position: 'bottom'
            }
        };

        if ($scope.chart) {
            $scope.chart.destroy();
        }

        $scope.chart = new ApexCharts(document.querySelector("#chart"), options);
        $scope.chart.render();
    };

    $scope.GetAuditStatusChartData = function () {
        var completed = 61; var inProgress = 35; var pending = 18;
        var series = [completed, inProgress, pending];

        var options = {
            chart: { type: 'donut', height: 280 },
            series: series,
            labels: ['Completed', 'In Progress', 'Pending'],
            colors: ['#2CA58D', '#7FD1C8', '#D3DDDC'],
            plotOptions: {
                pie: {
                    donut: {
                        size: '75%', labels: {
                            show: true,
                            name: { show: false },
                            value: {
                                //show: true, fontSize: '18px', fontWeight: 700, color: '#000000', offsetY: -10,
                                //formatter: function (w) { var total = w.globals.seriesTotals.reduce((a, b) => a + b, 0); return total.toLocaleString(); }
                            },
                            total: {
                                show: true, fontSize: '14px', color: '#000000', formatter: function (w) {
                                    var series = w.globals.series;
                                    var total = series.reduce((a, b) => a + b, 0);
                                    var completed = series[0];
                                    var percent = total ? ((completed / total) * 100).toFixed(0) : 0;
                                    return percent + '% done';
                                }
                            }
                        }
                    }
                }
            },

            legend: {
                show: true,
                position: 'bottom',
                horizontalAlign: 'center'
            },

            stroke: {
                width: 6,
                colors: ['#fff']
            }
        };

        var chart = new ApexCharts(document.querySelector("#criclecrt"), options);
        chart.render();
    };
    $scope.GetDashboardData = function () {
        $scope.searchRequest.actionType = "GetDashboardDetails";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Dashboard/GetDashboardData?FinancialYear=" + $scope.searchRequest.CurrentFinYear + "&DivisionId=" + $scope.searchRequest.divisionId + "&DistrictId=" + $scope.searchRequest.districtId + "&ActionType=" + $scope.searchRequest.actionType).
            success(function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
                if (data && data.Status == "200") {
                    if (data.Data.countData != null) {
                        if (data.Data.societyDetails != null) {
                            $scope.SocietyList = JSON.parse(data.Data.societyDetails);
                        }
                        if ($scope.SocietyList != null && $scope.SocietyList.length > 0) {
                            $scope.isNoRecord = false;
                        } else {
                            $scope.SocietyList = [];
                            $scope.isNoRecord = true;
                        }

                        $scope.countDetails = JSON.parse(data.Data.countData);
                        if ($scope.countDetails != null && $scope.countDetails.length > 0) {
                            $scope.dashboardData = {
                                TotalSociety: $scope.countDetails[0].TotalSociety,
                                TotalAuditor: $scope.countDetails[0].TotalAuditor,
                                TotalCCSComplaints: $scope.countDetails[0].TotalCCSComplaints,
                                TotalMPacs: $scope.countDetails[0].TotalMPacs,
                            }
                        }
                    } else {
                        $scope.SocietyList = [];
                        $scope.countDetails = [];
                    }
                } else {
                    $scope.SocietyList = [];
                }
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.GetSocietyAct = function () {
        return $http.get(SiteUrl + "/Dashboard/GetSocietyActioList").success(function (data, status) {
            if (data && data && data.Status == "200") {
                if (data.Data.actList != null) {
                    $scope.ActList = JSON.parse(data.Data.actList);
                    if ($scope.ActList.length > 0) {
                        $scope.selectedAct = $scope.ActList[0].ActId;
                        $scope.searchRequest.actId = $scope.ActList[0].ActId;
                        $scope.GetDashboardData().then(function () {
                            // return $scope.SocietyRegistrationChart($scope.searchRequest.actId);
                        });
                    }
                }
            } else {
                $scope.ActList = [];
                $scope.selectedAct = null;
            }
        }).error(function (data, status) {
            $scope.ActList = [];
            $scope.selectedAct = null;
        });
    };

    $scope.setActiveAct = function (item) {
        $scope.selectedAct = item.ActId;
        $scope.searchRequest.actId = item.ActId;
        $scope.GetDashboardData().then(function () {
            //return $scope.SocietyRegistrationChart($scope.searchRequest.actId);
        });

    };

    $scope.OnChangeFilterData = function () {
        $scope.GetDashboardData();
    };

    $scope.SocietyDetails = function (type) {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyList = [];
        $scope.searchRequest.actionType = type;
        return $http.get(SiteUrl + "/Dashboard/GetPendencyDetails?FinancialYear=" + $scope.searchRequest.CurrentFinYear + "&DivisionId=" + $scope.searchRequest.divisionId + "&DistrictId=" + $scope.searchRequest.districtId + "&ActionType=" + $scope.searchRequest.actionType).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data && data.Status == "200") {
                    if (data.Data.pendencyData != null) {
                        $scope.SocietyList = JSON.parse(data.Data.pendencyData);
                        if ($scope.SocietyList != null && $scope.SocietyList.length > 0) {
                            $scope.isNoRecord = false;
                        } else {
                            $scope.SocietyList = [];
                            $scope.isNoRecord = true;
                        }
                    }
                } else {
                    $scope.SocietyList = [];
                }
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.SocietyList = [];
            });
    }

    $scope.SocietyRegistrationChart = function (type) {
        $scope.chartData = [];
        if (type == 1) { $scope.searchRequest.actionType = "Act1958"; }
        else if (type == 3) { $scope.searchRequest.actionType = "Act2001"; }
        else if (type == 7) { $scope.searchRequest.actionType = "Act2005"; }
        return $http.get(SiteUrl + "/Dashboard/GetSocietyRegAuditorData?FinancialYear=" + $scope.searchRequest.CurrentFinYear + "&DivisionId=" + $scope.searchRequest.divisionId + "&DistrictId=" + $scope.searchRequest.districtId + "&ActionType=" + $scope.searchRequest.actionType).
            success(function (data, status) {
                if (data != null) {
                    $scope.chartData = JSON.parse(data.Data.chartData);
                    //$scope.GetSocietyRegistrationChartData($scope.chartData);
                }
            }).error(function (data, status) {
                $scope.chartData = [];
            });
    }

    $scope.OnPageLoad();

});













