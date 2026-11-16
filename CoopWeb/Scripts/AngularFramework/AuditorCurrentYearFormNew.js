var CooperativeSocietyFormModule = angular.module("ApplicationPLDBKPIModule", ['angularUtils.directives.dirPagination']);

CooperativeSocietyFormModule.controller("CooperativePLDBKPIFormCont", function ($rootScope, $scope, $http) {

    var PldbId = $('#hdnPldbId').val();


    $scope.PLDB = {
        PldbId: PldbId || 0
    };


    $scope.OnPageLoad = function () {
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
        };

        $scope.UserRegistration = {};
        $scope.UserRegistration = {
            OfficeId: 0
        };
        $scope.IsUpdate = false;
        $scope.IsPLDBAdminLocked = false;

        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });


        $scope.SetCurrentFY();
        $scope.GetUserDetailsBySSO();
        $scope.SetPreviousMonth();
        $scope.DistrictList();
        $scope.ShowTable = true
    }

    $scope.PLDB = {
        PldbId: 0
    };

    $scope.CalculateBalanceAllocation = function (item, field) {

        var total = Number(item.TotalCommittees) || 0;
        var department = Number(item.TotalAllotmentDepartment) || 0;
        var ca = Number(item.TotalAllotmentCA) || 0;

        // Negative values not allowed
        if (total < 0 || department < 0 || ca < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.CalculateBalanceAllocation(item);
            return;
        }

        // Allotment cannot exceed total committees
        if ((department + ca) > total) {

            alert("विभाग + चार्टर्ड अकाउंटेंट का योग कुल समितियों से अधिक नहीं हो सकता।");

            if (field === "TotalAllotmentDepartment") {
                item.TotalAllotmentDepartment = 0;
            }
            else if (field === "TotalAllotmentCA") {
                item.TotalAllotmentCA = 0;
            }
            else if (field === "TotalCommittees") {
                item.TotalCommittees = department + ca;
            }

            total = Number(item.TotalCommittees) || 0;
            department = Number(item.TotalAllotmentDepartment) || 0;
            ca = Number(item.TotalAllotmentCA) || 0;
        }

        // Calculate remaining allocation
        item.balanceAllocation = total - (department + ca);
    };

    $scope.GetFinancialYear = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data) {
                if (data != null) {
                    $scope.FinancialYearList = data
                        .map(function (item) {
                            return {
                                FinYearId: item.FIN_YEAR_MASTR_ID,
                                FinYearName: item.FIN_YEAR_START + '-' + item.FIN_YEAR_END
                            };
                        })
                        .filter(function (item) {
                            return item.FinYearId == '1086';
                        });

                    if ($scope.FinancialYearList.length > 0) {
                        $scope.FinYear = '1086';
                    }
                }
                else {
                    $scope.FinancialYearList = [];
                }

            }).error(function () {
                $scope.FinancialYearList = [];
            });
    };

    $scope.GetAuditorCurrentYearFormNew = function () {

        if (!$scope.SelectedDate) {
            alert("Please select date first.");
            $scope.KPIData = [];
            $scope.ShowTable = false;
            return;
        }

        var date = new Date($scope.SelectedDate);

        var selectedDate = date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');

        var districtId = $scope.UserDetails.DISTRICT_ID;
        var financialYear = $scope.FinancialYearName;

        $http.get(SiteUrl + "/AuditorNew/GetAuditorCurrentYearFormNew", {
            params: {
                selectedDate: selectedDate,
                financialYear: financialYear,
                districtId: districtId
            }
        }).success(function (data) {
            if (!angular.isArray(data)) {
                data = data ? [data] : [];
            }
            $scope.KPIData = data;
            $scope.ShowTable = true;
            $(".se-pre-con").fadeOut("slow");
        }).error(function (error) {
            console.log(error);
        });
    };

    $scope.GetKPIResult = function () {
        debugger;

        if (!$scope.SelectedDate) {
            alert("Please select date first.");
            $scope.KPIData = [];
            $scope.ShowTable = false;
            return;
        }

        var date = new Date($scope.SelectedDate);

        var year = date.getFullYear();
        var month = date.getMonth() + 1;

        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        var previousFYStartYear = currentFYStartYear - 1;

        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        var date = new Date($scope.SelectedDate);

        var selectedDate = date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');

        var districtId = $scope.UserDetails.DISTRICT_ID;
        var financialYear = previousFinancialYear;

        $http.get(SiteUrl + "AuditorNew/GetAuditCommiteeDetails", {
            params: { selectedDate: selectedDate, financialYear: financialYear, districtId: districtId }

        }).then(function (response) {
            var data = response.data;
            $scope.KPIData = response.data;
            //console.log("API Response:", data);

            if (!angular.isArray(data)) {
                data = data ? [data] : [];
            }

            // Insert / Update button
            if (data.length > 0) {
                if (data[0].TotalCommittees > 0) {
                    $scope.IsUpdate = true;
                    $scope.isBtn = 'Update';
                }
                else {
                    $scope.IsUpdate = false;
                    $scope.isBtn = 'Insert';
                }
            } else {
                $scope.IsUpdate = false;
                $scope.isBtn = 'Insert';
            }

            // Bind API data to form
            angular.forEach(data, function (saved) {

                angular.forEach($scope.KPIData, function (item) {

                    var masterId = item.CommitteeTypeId || item.CommiteeTypeId;
                    var savedId = saved.CommitteeTypeId || saved.CommiteeTypeId;

                    if (Number(masterId) === Number(savedId)) {

                        item.Id = Number(saved.Id) || 0;

                        item.FinancialYear = saved.FinancialYear;
                        item.Month = Number(saved.Month) || 0;
                        item.OfficeId = Number(saved.OfficeId) || 0;
                        item.DistrictId = Number(saved.DistrictId) || 0;

                        // Preserve InitialEnable
                        item.InitialEnable = Number(saved.InitialEnable) || 0;
                        item.TotalCommittees = Number(saved.TotalCommittees) || 0;
                        item.TotalAllotmentDepartment = Number(saved.TotalAllotmentDepartment) || 0;
                        item.TotalAllotmentCA = Number(saved.TotalAllotmentCA) || 0;
                        item.AllotmentDepartmentLastMonth = Number(saved.AllotmentDepartmentLastMonth) || 0;
                        item.AllotmentDepartmentInMonth = Number(saved.AllotmentDepartmentInMonth) || 0;
                        item.AllotmentCALastMonth = Number(saved.AllotmentCALastMonth) || 0;
                        item.AllotmentCAInMonth = Number(saved.AllotmentCAInMonth) || 0;
                        item.balanceAllocation = Number(saved.balanceAllocation) || 0;
                    }
                });
            });

            $(".se-pre-con").fadeOut("slow");
        }, function (error) {

            console.error("GetAuditAllDetails Error:", error);
            alert("Unable to load audit details.");
            $(".se-pre-con").fadeOut("slow");
        });

    };

    $scope.GetSelectedMonth = function () {

        if (!$scope.SelectedDate) {
            return 0;
        }

        var date = new Date($scope.SelectedDate);
        console.log('date', date);

        return date.getMonth() + 1;
    };

    $scope.Calculate = function (item) {
        var target = parseFloat(item.Target) || 0;
        var achievement = parseFloat(item.Achievement) || 0;
        var weightedMarks = parseFloat(item.WeightedMarks) || 0;

        if (achievement > target) {
            alert("Achievement cannot be greater than Target.");
            item.Achievement = "";
            item.Progress = "0.00";
            item.MarksObtained = "0.00";
            return;
        }

        if ((item.KPIIndicatorsId == "105" || item.KPIIndicatorsId == "104") && target == 0) {
            item.Progress = "100.00";
            item.MarksObtained = weightedMarks.toFixed(2);
            return;
        }

        if (target == 0) {
            item.Progress = "0.00";
            item.MarksObtained = "0.00"
            return;
        }

        var progress = (achievement / target) * 100;
        progress = Math.min(progress, 100);
        item.Progress = progress.toFixed(2);
        item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
    };

    $scope.SetPreviousMonth = function () {

        var months = [
            { Id: 1, Name: "January" },
            { Id: 2, Name: "February" },
            { Id: 3, Name: "March" },
            { Id: 4, Name: "April" },
            { Id: 5, Name: "May" },
            { Id: 6, Name: "June" },
            { Id: 7, Name: "July" },
            { Id: 8, Name: "August" },
            { Id: 9, Name: "September" },
            { Id: 10, Name: "October" },
            { Id: 11, Name: "November" },
            { Id: 12, Name: "December" }
        ];

        var currentMonth = new Date().getMonth(); // 0-11

        if (currentMonth == 0) {
            $scope.PreviousMonthId = 12;
            $scope.PreviousMonth = "December";
        }
        else {
            $scope.PreviousMonthId = months[currentMonth - 1].Id;
            $scope.PreviousMonth = months[currentMonth - 1].Name;
        }
    };

    $scope.CheckPLDBAdminLock = function () {

        $http.get(SiteUrl + "/KPI/CheckPLDBAdminLock", {
            params: { pldbId: $scope.PLDB.PldbId, month: $scope.PreviousMonthId, kpiFor: "PLDB" }
        }).success(function (data) {
            if (data && data.length > 0) {
                $scope.IsPLDBAdminLocked = String(data[0].IsLocked).toLowerCase() === "true";
                //console.log('$scope.IsPLDBAdminLocked', $scope.IsPLDBAdminLocked);
            }
            else {
                $scope.IsPLDBAdminLocked = false;
            }

        }).error(function () {
            $scope.IsPLDBAdminLocked = false;
        });
    };

    $scope.SetCurrentFY = function () {
        $http.get(SiteUrl + "/Society/GetCurrentFinancialYear")
            .success(function (data) {
                $scope.FY = data[0].CurrentFY;
                console.log('$scope.FY', $scope.FY);
                $scope.GetFinancialYear();
            })
            .error(function () {
                alert("Unable to load Current Financial Year.");
            });
    };

    $scope.ValidateDepartmentAudit = function (item, field) {
        var totalDepartment = Number(item.TotalAllotmentDepartment) || 0;
        var previous = Number(item.AllotmentDepartmentLastMonth) || 0;
        var current = Number(item.AllotmentDepartmentInMonth) || 0;
        var totalAudited = previous + current;
        if (totalAudited > totalDepartment) {
            alert("गत माह तक ऑडिटेड विभाग और माह में ऑडिटेड विभाग का योग कुल आवंटित विभाग से अधिक नहीं हो सकता।");
            if (field === 'previous') {
                item.AllotmentDepartmentLastMonth = 0;
            }
            else if (field === 'current') {
                item.AllotmentDepartmentInMonth = 0;
            }
            return false;
        }

        return true;
    };

    $scope.ValidateCAAudit = function (item, field) {
        var totalAllotmentCA = Number(item.TotalAllotmentCA) || 0;
        var previousLastMonth = Number(item.AllotmentCALastMonth) || 0;
        var currentMonth = Number(item.AllotmentCAInMonth) || 0;
        var totalAudited = previousLastMonth + currentMonth;
        if (totalAudited > totalAllotmentCA) {
            alert("गत माह तक ऑडिटेड विभाग और माह में ऑडिटेड विभाग का योग कुल आवंटित विभाग से अधिक नहीं हो सकता।");
            if (field === 'previous') {
                item.AllotmentCALastMonth = 0;
            }
            else if (field === 'current') {
                item.AllotmentCAInMonth = 0;
            }
            return false;
        }

        return true;
    };

    $scope.SaveAuditResult = function () {
        $scope.FY = '2025-26';
        var SaveData = [];

        if (!$scope.FY) {
            alert("Please select Financial Year.");
            return;
        }

        var selectedMonth = $scope.GetSelectedMonth();

        if (!selectedMonth) {
            alert("Invalid selected date.");
            return;
        }

        if (!$scope.KPIData || $scope.KPIData.length === 0) {
            alert("No audit data available.");
            return;
        }

        for (var i = 0; i < $scope.KPIData.length; i++) {

            var area = $scope.KPIData[i];

            if (!$scope.ValidateDepartmentAudit(area)) {
                return;
            }
        }

        angular.forEach($scope.KPIData, function (area) {

            SaveData.push({
                FY: $scope.FY,
                Month: selectedMonth,
                OfficeId: Number(area.OfficeId) || 0,
                CommitteeTypeId: Number(area.CommitteeTypeId || area.CommiteeTypeId) || 0,
                TotalCommittees: Number(area.TotalCommittees) || 0,
                TotalAllotmentDepartment: Number(area.TotalAllotmentDepartment) || 0,
                TotalAllotmentCA: Number(area.TotalAllotmentCA) || 0,
                balanceAllocation: Number(area.balanceAllocation) || 0,
                AllotmentDepartmentLastMonth: Number(area.AllotmentDepartmentLastMonth) || 0,
                AllotmentCALastMonth: Number(area.AllotmentCALastMonth) || 0,
                AllotmentDepartmentInMonth: Number(area.AllotmentDepartmentInMonth) || 0,
                AllotmentCAInMonth: Number(area.AllotmentCAInMonth) || 0
            });

        });

        if (SaveData.length === 0) {
            alert("No records found.");
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/AuditorNew/SaveAuditResult", SaveData)
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    alert("Record Saved Successfully.");
                    //$scope.GetKPIResult();
                    location.reload();
                }
                else {
                    alert("Unable to save.");
                }
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while saving.");
            });
    };

    $scope.UpdateAuditResult = function () {
        var SaveData = [];
        if (!$scope.PreviousMonthId) {
            alert("Please select Month.");
            return;
        }

        if (!$scope.KPIData || $scope.KPIData.length === 0) {
            alert("No audit data available.");
            return;
        }
        angular.forEach($scope.KPIData, function (area) {

            SaveData.push({
                Id: Number(area.Id) || 0,
                FinancialYear: area.FinancialYear || $scope.FY,
                FY: area.FinancialYear || $scope.FY,
                Month: $scope.GetSelectedMonth(),
                DistrictId: Number(area.DistrictId || $('#hdnDistrictId').val()) || 0,
                OfficeId: Number(area.OfficeId) || 0,
                // Committee Type
                CommitteeTypeId: Number(area.CommitteeTypeId || area.CommiteeTypeId) || 0,
                // Total Committees
                TotalCommittees: Number(area.TotalCommittees) || 0,
                // Total Allotment Department
                TotalAllotmentDepartment: Number(area.TotalAllotmentDepartment) || 0,
                // Total Allotment CA
                TotalAllotmentCA: Number(area.TotalAllotmentCA) || 0,
                // Department Last Month
                AllotmentDepartmentLastMonth: Number(area.AllotmentDepartmentLastMonth) || 0,
                // Department In Month
                AllotmentDepartmentInMonth: Number(area.AllotmentDepartmentInMonth) || 0,
                // CA Last Month
                AllotmentCALastMonth: Number(area.AllotmentCALastMonth) || 0,
                // CA In Month
                AllotmentCAInMonth: Number(area.AllotmentCAInMonth) || 0,
                // Balance Allocation
                balanceAllocation: Number(area.balanceAllocation) || 0
            });
        });

        console.log("Update Audit Data:", SaveData);

        // 3. Validate Prepared Data
        if (SaveData.length === 0) {
            alert("No records found for update.");
            return;
        }

        if (SaveData.some(function (item) {
            return item.CommitteeTypeId === 0;
        })) {
            alert("Invalid Committee Type.");
            return;
        }
        if (!confirm("Are you sure you want to update audit records?")) {
            return;
        }
        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/AuditorNew/SaveAuditResult", SaveData)
            .then(function (data) {
                $(".se-pre-con").fadeOut("slow");
                var Result = data;
                console.log('data', Result);
                if (Result.data > 0) {
                    alert("Record Updated Successfully.");
                    $scope.GetKPIResult();

                } else {
                    alert(data.Message || "Unable to Update.");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("Update Audit Error:", error);
                alert("Error while updating audit records.");

            });

    };

    $scope.GetKPIResultPLDBWise = function () {
        $http.get(SiteUrl + "/KPI/GetKPIPLDBWiseDetails", {
            params: { pldbId: $scope.PLDB.PldbId, month: $scope.PreviousMonthId, kpiFor: "PLDB" }
        }).success(function (data) {
            angular.forEach($scope.KPIAreas, function (area) {
                angular.forEach(area.Indicators, function (indicator) {
                    indicator.Target = "";
                    indicator.Achievement = "";
                    indicator.Progress = "";
                    indicator.MarksObtained = "";
                    indicator.SelectedOption = "";
                });
            });

            if (data && data.length > 0) {
                $scope.IsUpdate = true;
                $scope.isBtn = 'Insert';
            }
            else {
                $scope.IsUpdate = false;
                $scope.isBtn = 'Insert';
            }

            angular.forEach(data, function (saved) {
                angular.forEach($scope.KPIAreas, function (area) {
                    angular.forEach(area.Indicators, function (item) {

                        if (item.KPIIndicatorsId == saved.KPIIndicatorsId) {
                            item.Target = Number(saved.KPIIndicatorsTarget);
                            item.Achievement = Number(saved.KPIIndicatorsAchivement);
                            item.Progress = Number(saved.ProgressPercentage);
                            item.MarksObtained = Number(saved.MarksObtained);
                            item.SelectedOption = saved.Packs_KPI_MarksId;
                        }

                    });
                });
            });

        });
    };

    $scope.IsTargetAchievementDisabled = function (item) {

        if ($scope.UserDetails && ($scope.UserDetails.RoleId == 1 || $scope.UserDetails.RoleId == 10235)) { //PLDB Admin
            return false;
        }

        if ($scope.UserDetails && $scope.UserDetails.RoleId == 10188 && $scope.IsPLDBAdminLocked) {
            return true;
        }

        if ($scope.IsPLDBAdminLocked && (item.KPIIndicatorsId == 97 || item.KPIIndicatorsId == 98 || item.KPIIndicatorsId == 111 || item.KPIIndicatorsId == 103)) {
            return true;
        }

        if ($scope.UserDetails && $scope.UserDetails.RoleId == 10188 && (item.KPIIndicatorsId == 97 || item.KPIIndicatorsId == 98 || item.KPIIndicatorsId == 111 || item.KPIIndicatorsId == 103)) { //PLDB Secratary
            return true;
        }

        return false;
    };

    $scope.LoadMaster = function () {

        return $http.get(SiteUrl + "/pldb/LoadMaster")
            .then(function (response) {
                $scope.PLDBList = JSON.parse(response.data.DataList).PLDB || [];

                var districtid = $('#hdnDistrictId').val();
                var LoginPldbId = $('#hdnPldbId').val();

                if (LoginPldbId && LoginPldbId !== '0') {

                    $scope.$evalAsync(function () {
                        $scope.PLDB = $scope.PLDB || {};
                        $scope.PLDB.PldbId = LoginPldbId.toString();
                        $scope.CheckPLDBAdminLock();
                    });
                }
            }, function () {
                $scope.PLDBList = [];
            });
    };

    $scope.LoadMaster();

    $scope.ShowTable = false;

    $scope.GetUserDetailsBySSO = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/AuditorNew/GetUserDetailsAuditBySSO")
            .success(function (data) {
                $scope.UserDetails = data[0];
                //$scope.OfficeId = data[0].OfficeId;
                $scope.ShowTable = true;
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    // START : Get List of Districts
    $scope.DistrictList = function () {
        $scope.Districts = [{}];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [{}];
            });
    };
    $scope.IsKPIExpired = false;
    $scope.IsExpiryAlertShown = false;

    $scope.GetFinancialYear = function () {
        debugger;

        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data, status) {
                $scope.FinancialYearList = data;
                var selectedFY = $scope.FinancialYearList.filter(function (x) {
                    return x.FIN_YEAR_MASTR_ID == $scope.FY;
                });

                if (selectedFY.length > 0) {
                    $scope.FinancialYearName = selectedFY[0].FIN_YEAR_NAME;
                }

                $(".se-pre-con").fadeOut("slow");

            })
            .error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.OnPageLoad();

});