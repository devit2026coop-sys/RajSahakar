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



        //$scope.GetSAKPIRAODetails();
        $scope.GetUserDetailsBySSO();
        $scope.SetPreviousMonth();
        //$scope.GetKPIResult();
        $scope.DistrictList();
        $scope.ShowTable = true
    }


    //date change


    $scope.SetProgress = function (item) {

        if (item.SelectedOption == "113") {         //Yes
            item.Progress = 100;
            item.MarksObtained = item.WeightedMarks;
        }
        else if (item.SelectedOption == "114") {     // No
            item.Progress = 0;
            item.MarksObtained = 0;
        }
        else {
            item.Progress = 0;
            item.MarksObtained = 0;
        }
    };

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


    $scope.GetAuditorCurrentYearFormNew = function () {
        debugger;

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

        $http.get(SiteUrl + "/Auditor/GetAuditorCurrentYearFormNew", {
            params: {
                selectedDate: selectedDate,
                financialYear: financialYear,
                districtId: districtId
            }
        }).success(function (data) {
            // Ensure API response is an array
            if (!angular.isArray(data)) {
                data = data ? [data] : [];
            }
            $scope.KPIData = data;
            //$scope.GetKPIResult();
            console.log("FIRST ROW:", $scope.KPIData[0]);
            console.log('$scope.KPIData', $scope.KPIData);
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

        // Previous Financial Year
        var previousFYStartYear = currentFYStartYear - 1;

        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        console.log("Previous Financial Year:", previousFinancialYear);

        var date = new Date($scope.SelectedDate);

        var selectedDate = date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');

        var districtId = $scope.UserDetails.DISTRICT_ID;
        var financialYear = previousFinancialYear;

        $http.get(SiteUrl + "AuditorNew/GetAuditCommiteeDetails", {
            params: {
                selectedDate: selectedDate,
                financialYear: financialYear,
                districtId: districtId
            }

        }).then(function (response) {
            var data = response.data;
            console.log('data', data);
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
            //angular.forEach(data, function (saved) {
            //    debugger;
            //    angular.forEach($scope.KPIData, function (item) {

            //        if (Number(item.CommiteeTypeId ||
            //            item.CommiteeTypeId) ===
            //            Number(saved.CommiteeTypeId)) {
            //            item.Id = 0;
            //            item.FinancialYear = saved.FinancialYear;
            //            item.Month = Number(saved.Month) || 0;
            //            item.OfficeId = Number(saved.OfficeId) || 0;
            //            item.DistrictId = Number(saved.DistrictId) || 0;
            //            item.TotalCommittees = Number(saved.TotalCommittees) || 0;
            //            item.TotalAllotmentDepartment = Number(saved.TotalAllotmentDepartment) || 0;
            //            item.TotalAllotmentCA = Number(saved.TotalAllotmentCA) || 0;
            //            item.AllotmentDepartmentLastMonth = Number(saved.AllotmentDepartmentLastMonth) || 0;
            //            item.AllotmentDepartmentInMonth = Number(saved.AllotmentDepartmentInMonth) || 0;
            //            item.AllotmentCALastMonth = Number(saved.AllotmentCALastMonth) || 0;
            //            item.AllotmentCAInMonth = Number(saved.AllotmentCAInMonth) || 0;
            //            item.balanceAllocation = Number(saved.balanceAllocation) || 0;

            //            console.log("Matched Committee:", item);
            //        }
            //    });
            //});

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

            console.log("Final KPIData:", $scope.KPIData);
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

        //// Target = 0 AND Achievement = 0
        //if (target == 0 && achievement == 0) {
        //    item.Progress = "0.00";
        //    item.MarksObtained = "0.00";
        //    return;
        //}

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

    //$scope.SaveKPIResult = function () {
    //    if ($scope.IsPLDBAdminLocked && $scope.UserDetails.RoleId == 10188) {
    //        alert("PLDB Admin has already filled the mandatory KPIs. You cannot modify them.");
    //        return;
    //    }

    //    var SaveData = [];
    //    var isValid = true;

    //    angular.forEach($scope.KPIAreas, function (area) {
    //        angular.forEach(area.Indicators, function (item) {
    //            SaveData.push({
    //                FY: $scope.FY,
    //                Month: $scope.PreviousMonthId,
    //                KPIFor: "PLDB",
    //                OfficeId: $scope.OfficeId,
    //                KPIIndicatorsId: item.KPIIndicatorsId,
    //                KPI_MarksId: item.SelectedOption,
    //                KPIIndicatorsTarget: item.Target || 0,
    //                KPIIndicatorsAchievement: item.Achievement || 0,
    //                ProgressPercentage: item.Progress || 0,
    //                WeightedMarks: item.WeightedMarks || 0,
    //                MarksObtained: item.MarksObtained || 0,
    //                PLDBId: $scope.PLDB.PldbId || 0
    //            });
    //        });

    //        if (!isValid)
    //            return false;
    //    });

    //    if (!isValid)
    //        return;

    //    $(".se-pre-con").fadeIn("slow");

    //    $http.post(SiteUrl + "/KPI/SaveKPIResult", SaveData)
    //        .success(function (data) {
    //            $(".se-pre-con").fadeOut("slow");
    //            if (data > 0) {
    //                alert("Record Saved Successfully.");
    //                location.reload();
    //            }
    //            else {
    //                alert("Unable to save.");
    //            }
    //        })
    //        .error(function () {
    //            $(".se-pre-con").fadeOut("slow");
    //            alert("Error while saving.");
    //        });
    //};


    $scope.ValidateDepartmentAudit = function (item, field) {
        debugger;

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
        debugger;

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
        debugger;
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

        console.log("Save Data:", angular.copy(SaveData));

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

    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + ($scope.GopalCreditCardModel.LoanDistrictId ?? 0) + "&LoanBankId=" + ($scope.GopalCreditCardModel.LoanBankID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
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

        $http.get(SiteUrl + "/Auditor/GetUserDetailsAuditBySSO")
            .success(function (data) {
                $scope.UserDetails = data[0];
                console.log('$scope.UserDetails', $scope.UserDetails);
                $scope.OfficeId = data[0].OfficeId;
                $scope.ShowTable = true;
                //scope.GetAuditorCurrentYearFormNew();

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
    // END : Get List of Districts

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.UserRegistration.IsRuralUrban;
        $("#ddlPanchayatSamiti").val('0');
        $("#ddlGramPanchayat").val('0');
        $("#ddlVillage").val('0');
        $("#ddlCity").val('0');
        $scope.UserRegistration.DivisionId = '0';
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.UserRegistration.GPId = '0';
        $scope.UserRegistration.VillageId = '0';
        $scope.CityList = [];
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        if (SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
        }
        else if (SelectedVal == 'false') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };
    // END : Set Rural/ Urban div show/hide

    // START : Get Ward List
    $scope.WardList = function () {
        $scope.Ward = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.UserRegistration.CityId).
            success(function (data, status) {
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
    };
    // END : Get Ward List

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function () {
        $scope.GramPanchayats = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.UserRegistration.BlockId).
            success(function (data, status) {
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
            });
    };
    // END : Get List of GramPanchayat

    // START : Get List of Villages
    $scope.VillageList = function () {
        $scope.Villages = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.UserRegistration.GPId).
            success(function (data, status) {
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $scope.Villages = [];
            });
    };
    // END : Get List of Villages
    //Start: Get Unit List
    $scope.UnitList = function () {
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=UNITS&MasterCode=" + $scope.UserRegistration.DEPTCODE).

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=UNITS&MasterCode=0").
            success(function (data, status) {
                $scope.Unit = data;
            }).
            error(function (data, status) {
                $scope.Unit = [];
            });
    };
    //End 
    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=3").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    //Start: Get Unit List
    $scope.LevelList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Level = [];
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.UserRegistration.OrganisationId).
            success(function (data, status) {
                $scope.Level = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.LeveldataonUrban = function (item) {
        if (item == '2') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.UserRegistration.WorkScopeLevel_ID = '0';
            $scope.UserRegistration.DIVISION_ID = '0';
            $scope.UserRegistration.DistrictId = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
        }
        else if (item == '1') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.UserRegistration.WorkScopeLevel_ID = '0';
            $scope.UserRegistration.CityTypes = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.UserRegistration.WARD_ID = '0';
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }

    }

    $scope.Levelofficedata = function () {
        if ($scope.UserRegistration.WorkScopeLevel_ID == '1') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = false;
            $scope.StateDiv = true;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '2') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = true;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];

        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '3') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = true;
            $scope.IsDivision = false;
            $scope.RuralUrbanRedioDiv = false;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '4') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '5') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.RuralurbanDiv = true;
            $scope.Addmore = true;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '6') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.RuralurbanDiv = true;
            $scope.IsBlock = true;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '7' || $scope.UserRegistration.WorkScopeLevel_ID == '8' || $scope.UserRegistration.WorkScopeLevel_ID == '9') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.IsBlock = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = false;
            $scope.IsResion = false;
            $scope.IsRural = false;
            $scope.IsRuralUrban = false;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralDiv = false;
            //$scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];

            $scope.GopalCreditCardModel.PACS_ID = '0';
            $scope.GopalCreditCardModel.LoanBranchId = '0';
            $scope.GopalCreditCardModel.LoanBankID = '0';

            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];
            $scope.BankMasterList = [];

        }
        else {
            $scope.StateDiv = false;
            $scope.DivisionDiv = false;
            $scope.IsDistrict = false;
            $scope.CitytypDiv = false;
        }

        $scope.GopalCreditCardModel.PACS_ID = '0';
        $scope.GopalCreditCardModel.LoanBranchId = '0';
        $scope.GopalCreditCardModel.LoanBankID = '0';

        $scope.PLDBBranches = [];
        $scope.PACSNameList = [];
        $scope.BankMasterList = [];
    };
    //End Level of office data

    //Get Post List for dropdown
    $scope.PostByDesignation = function () {
        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=" + $scope.UserRegistration.DESIGNATIONCODE).
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list");
                $scope.showLoader = false;
            });
        // END : Post List for dropdown

    };
    //START : Get City List
    $scope.GetCityList = function () {
        $scope.ResionIDs = "";
        $scope.UserRegistration.OfficeId = '0';
        $scope.ResionList = [];
        $scope.IsResion = false;
        if ($scope.UserRegistration.DistrictId != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.CityList = data;
                }).
                error(function (data, status) {
                    $scope.CityList = [];
                });
            //}
            //if ($scope.UserRegistration.IsRuralUrban == 'false') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });


            $scope.ProcessingUnitList = [];
            $scope.DEPORTList = [];
            $scope.WAREHOUSEList = [];

            if ($scope.UserRegistration.OrganisationId == 11) {
                $scope.PLDBListList();
            }

            if ($scope.UserRegistration.OrganisationId == 10) {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLANT&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.ProcessingUnitList = data;
                    }).
                    error(function (data, status) {
                        $scope.ProcessingUnitList = [];
                    });

                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPORT&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.DEPORTList = data;
                    }).
                    error(function (data, status) {
                        $scope.DEPORTList = [];
                    });
            }


            if ($scope.UserRegistration.OrganisationId == 17) {

                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WAREHOUSE&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.WAREHOUSEList = data;
                    }).
                    error(function (data, status) {
                        $scope.WAREHOUSEList = [];
                    });
            }
        }
        $scope.GramPanchayats = [];
    };
    $scope.flag = 0;

    $scope.GetResionMapping = function () {
        //alert($scope.UserRegistration.DistrictId);
        $scope.ResionIDs = "";
        $scope.ResionList = [];
        if ($scope.UserRegistration.OfficeId == 3 || $scope.UserRegistration.OfficeId == 41 || $scope.UserRegistration.OfficeId == 19) {  //
            if ($scope.UserRegistration.DistrictId != '0') {
                $scope.IsResion = true;
                // alert($scope.UserRegistration.DistrictId);
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ResionMapping&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $(".se-pre-con").fadeIn("slow");
                        $scope.IsResion = true;
                        data = data.filter(function (x) { return x.OrganisationId == $scope.UserRegistration.OrganisationId });

                        //Remove Workscope
                        //$("#ddlResion").empty();                       
                        //$.each(data, function () {
                        //    $("#ddlResion").append($("<option></option>").val(this.ResionId).html(this.ResionName));
                        //});
                        //if ($scope.flag == 1) {
                        //    $('#ddlResion').multiselect('destroy');
                        //    $(".se-pre-con").fadeOut("slow");
                        //}
                        //$('#ddlResion').multiselect({
                        //    includeSelectAllOption: true
                        //});
                        $scope.flag = 1;
                        $(".se-pre-con").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $scope.ResionList = [];
                        $scope.IsResion = false;
                    });
            }
            else {
                $scope.IsResion = false;
            }
        }
        else {
            $scope.IsResion = false;
        }
    };

    $scope.GetValuesForDesignationList = function () {
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.IsRuralUrban = false;
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.ProcessingUnitList = [];
        $scope.DEPORTList = [];
        $scope.PLDBBranches = [];
        $scope.WAREHOUSEList = [];
        $scope.Designations = [];

        var Level_OrganizationLevelsMappingID = $scope.UserRegistration.LevelId;
        if (Level_OrganizationLevelsMappingID.indexOf("~") >= 0) {
            var arrayOfStrings = Level_OrganizationLevelsMappingID.split("~");
            $scope.UserRegistration.OrganizationLevelsMappingID = arrayOfStrings[1];
            $scope.UserRegistration.WorkScopeLevel_ID = arrayOfStrings[0];
        }
        else {
            $scope.UserRegistration.OrganizationLevelsMappingID = 0;
            $scope.UserRegistration.WorkScopeLevel_ID = 0;
        }

        $scope.Levelofficedata();

        var selectedText = $('#ddllevel option:selected').text();
        var replacedText = selectedText.trim().replace(/ /g, '');

        if ($scope.UserRegistration.WorkScopeLevel_ID == 7 || $scope.UserRegistration.WorkScopeLevel_ID == 8 || $scope.UserRegistration.WorkScopeLevel_ID == 9) {
            if ($scope.UserRegistration.WorkScopeLevel_ID == 7) { $scope.gccData = 1 } else if ($scope.UserRegistration.WorkScopeLevel_ID == 8) { $scope.gccData = 2 } else { $scope.gccData = 3 }
            $scope.LoanBankList();
        }
        else {

            if ($scope.UserRegistration.OrganisationId == 11 && $scope.UserRegistration.LevelId == '4~24') {
                $scope.isPLDBBlock = true;
            }
            else {
                $scope.isPLDBBlock = false;
            }
        }
        $scope.OfficeList();
    };

    $scope.GetDesignationList = function () {
        $scope.Designations = [];

        $http.get(SiteUrl + "/MasterSettings/GetAllDesignationByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID + "&OfficeId=" + $scope.UserRegistration.OfficeId).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Designations = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });
    };

    $scope.OfficeList = function () {
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };

    $scope.KPIforPacs = function () {
        $scope.buldingtype = [];
        $scope.landavailble = [];
        $scope.godownmt = [];
        $scope.godownlessthen = [];
        $scope.businessut = [];
        $scope.membership = [];
        $scope.loanee = [];
        $scope.profit = [];
        $scope.share = [];
        $scope.deposit = [];
        $scope.Imbalance = [];
        $scope.dividend = [];
        $scope.otherkcclon = [];
        $scope.kccrecovery = [];
        $scope.gccrecovery = [];
        $scope.otherloan = [];
        $scope.auditcomplte = [];
        $scope.auditclasifiction = [];
        $scope.agmconvened = [];
        $scope.bodmeting = [];
        $scope.compliencaudit = [];
        $scope.fertilizer = [];
        $scope.seed = [];
        $scope.pesticide = [];
        $scope.animal = [];
        $scope.csc = [];
        $scope.pmksk = [];
        $scope.chc = [];
        $scope.grains = [];
        $scope.fpo = [];
        $scope.pmbjk = [];
        $scope.kusum = [];
        $scope.pds = [];
        $scope.anyother = [];
        $scope.epacs = [];
        $scope.systmaudit = [];
        $scope.dynamic = [];
        $scope.migration = [];

        $(".blockUI").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=KPIFORPACS").
            success(function (data, status) {
                $scope.KPIforPacsdata = data;
                $scope.buldingtype = data.filter(x => x.MeasurableKey == 'Own building');
                $scope.landavailble = data.filter(x => x.MeasurableKey == 'Land Available');
                $scope.godownmt = data.filter(x => x.MeasurableKey == 'Godown - 500 MT Godown Available in total');
                $scope.godownlessthen = data.filter(x => x.MeasurableKey == 'Godown - Less than 500 MT Godown Available in total');
                $scope.businessut = data.filter(x => x.MeasurableKey == 'Business utilisation i.e. other then for self consumption');
                $scope.membership = data.filter(x => x.MeasurableKey == 'Membership increased in comparison to last year');
                $scope.loanee = data.filter(x => x.MeasurableKey == 'Loanee members in comparison to total members');
                $scope.profit = data.filter(x => x.MeasurableKey == 'Profit & Loss for last year');
                $scope.share = data.filter(x => x.MeasurableKey == 'Share Capital growth in comparison to last year');
                $scope.Deposit = data.filter(x => x.MeasurableKey == 'Deposit');
                $scope.Imbalance = data.filter(x => x.MeasurableKey == 'Imbalance with DCCB');
                $scope.dividend = data.filter(x => x.MeasurableKey == 'Dividend distribution');
                $scope.otherkcclon = data.filter(x => x.MeasurableKey == 'Other than KCC Loan Disbursed');
                $scope.kccrecovery = data.filter(x => x.MeasurableKey == 'KCC Recovery against demand');
                $scope.gccrecovery = data.filter(x => x.MeasurableKey == 'GCC Recovery against demand');
                $scope.otherloan = data.filter(x => x.MeasurableKey == 'Other Loan Recovery against demand');
                $scope.auditcomplte = data.filter(x => x.MeasurableKey == 'Audit completed for last year');
                $scope.auditclasifiction = data.filter(x => x.MeasurableKey == 'Audit classification');
                $scope.agmconvened = data.filter(x => x.MeasurableKey == 'AGM convened of current year before 30 Sept');
                $scope.bodmeting = data.filter(x => x.MeasurableKey == 'At least 1 BOD meeting convened in last quarter');
                $scope.compliencaudit = data.filter(x => x.MeasurableKey == 'Compliance of Audit submitted within stipulated time');
                $scope.fertilizer = data.filter(x => x.MeasurableKey == 'Fertilizer distribution');
                $scope.seed = data.filter(x => x.MeasurableKey == 'Seed distribution');
                $scope.pesticide = data.filter(x => x.MeasurableKey == 'Pesticide distribution');
                $scope.animal = data.filter(x => x.MeasurableKey == 'Animal feed distribution');
                $scope.csc = data.filter(x => x.MeasurableKey == 'Working as CSC');
                $scope.pmksk = data.filter(x => x.MeasurableKey == 'Working as PMKSK');
                $scope.chc = data.filter(x => x.MeasurableKey == 'Working as CHC');
                $scope.grains = data.filter(x => x.MeasurableKey == 'Engaged in procurement of food grains');
                $scope.fpo = data.filter(x => x.MeasurableKey == 'Working as FPO');
                $scope.pmbjk = data.filter(x => x.MeasurableKey == 'Working as PMBJK');
                $scope.kusum = data.filter(x => x.MeasurableKey == 'Participating in PM KUSUM');
                $scope.pds = data.filter(x => x.MeasurableKey == 'Engaged in PDS');
                $scope.anyother = data.filter(x => x.MeasurableKey == 'Any other : Please Specify');
                $scope.epacs = data.filter(x => x.MeasurableKey == 'E-PACS declared');
                $scope.systmaudit = data.filter(x => x.MeasurableKey == 'On System Audit');
                $scope.dynamic = data.filter(x => x.MeasurableKey == 'Dynamic Day end');
                $scope.migration = data.filter(x => x.MeasurableKey == 'Migration diffrence');


                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.KPIforPacsdata = [];
                $(".blockUI").fadeOut("slow");
            });
    }

    // Check For Time to Form Disable Added By Narpat
    $scope.IsKPIExpired = false;
    $scope.IsExpiryAlertShown = false;

    $scope.GetFinancialYear = function () {
        debugger;

        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data, status) {
                $scope.FinancialYearList = data;
                console.log('$scope.FinancialYearList', $scope.FinancialYearList);

                // Get Financial Year Name using FY ID
                var selectedFY = $scope.FinancialYearList.filter(function (x) {
                    return x.FIN_YEAR_MASTR_ID == $scope.FY;
                });

                if (selectedFY.length > 0) {
                    $scope.FinancialYearName = selectedFY[0].FIN_YEAR_NAME;
                    console.log('$scope.FinancialYearName', $scope.FinancialYearName);
                }

                $(".se-pre-con").fadeOut("slow");

            })
            .error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //$scope.CheckKPIExpiry = function () {

    //    // District 3 ke liye expiry nahi lagegi
    //    if (Number($scope.PldbDistrictID) === 3) {
    //        $scope.IsKPIExpired = false;
    //        return;
    //    }

    //    var currentDate = new Date();
    //    var cutoffDate = new Date("2026-10-03T16:58:55+05:30");

    //    if (currentDate >= cutoffDate) {

    //        $scope.IsKPIExpired = true;

    //        if (!$scope.IsExpiryAlertShown) {
    //            $scope.IsExpiryAlertShown = true;
    //            alert("Time is over. You cannot save or update KPI.");
    //        }

    //    } else {

    //        $scope.IsKPIExpired = false;

    //        var remainingTime =
    //            cutoffDate.getTime() - currentDate.getTime();

    //        setTimeout(function () {

    //            $scope.$apply(function () {

    //                // Timeout hone ke baad bhi District 3 ko allow rakho
    //                if (Number($scope.PldbDistrictID) === 3) {
    //                    $scope.IsKPIExpired = false;
    //                    return;
    //                }

    //                $scope.IsKPIExpired = true;

    //                if (!$scope.IsExpiryAlertShown) {
    //                    $scope.IsExpiryAlertShown = true;
    //                    alert("Time is over. You cannot save or update KPI.");
    //                }

    //            });

    //        }, remainingTime);
    //    }
    //};
    //$scope.CheckKPIExpiryOld = function () {
    //    var currentDate = new Date();
    //    var cutoffDate = new Date("2026-10-03T16:58:55+05:30");
    //    if (currentDate >= cutoffDate) {
    //        $scope.IsKPIExpired = true;

    //        if (!$scope.IsExpiryAlertShown) {
    //            $scope.IsExpiryAlertShown = true;
    //            alert("Time is over. You cannot save or update KPI.");
    //        }

    //    } else {
    //        $scope.IsKPIExpired = false;
    //        var remainingTime = cutoffDate.getTime() - currentDate.getTime();
    //        setTimeout(function () {
    //            $scope.$apply(function () {
    //                $scope.IsKPIExpired = true;
    //                if (!$scope.IsExpiryAlertShown) {
    //                    $scope.IsExpiryAlertShown = true;
    //                    alert("Time is over. You cannot save or update KPI.");
    //                }

    //            });

    //        }, remainingTime);
    //    }
    //};
    //$scope.CheckKPIExpiry();

    $scope.OnPageLoad();

});