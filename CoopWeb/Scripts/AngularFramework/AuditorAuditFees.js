var CooperativeSocietyFormModule = angular.module("AuditorAuditFeesModule", ['angularUtils.directives.dirPagination']);

CooperativeSocietyFormModule.controller("AuditorAuditFeesCont", function ($rootScope, $scope, $http) {

    // MPR Audit Fees Form
    $scope.AuditFeesList = [];
    $scope.DistrictId = null;

    $scope.GotoCurrent = function () {
        window.location.href = '/Auditor/AuditorCurrentYearForm';
    };

    $scope.GotoBackLog = function () {
        window.location.href = '/Auditornew/AuditorCurrentyearFormNew';
    };

    $scope.GetUserDetailsBySSO = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Auditor/GetUserDetailsAuditBySSO")
            .success(function (data) {
                $scope.UserDetails = data[0];
                console.log('$scope.UserDetails', $scope.UserDetails);
                $scope.OfficeId = data[0].OfficeId;
                $scope.DISTRICT_ENG = data[0].DISTRICT_ENG;

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SetPreviousMonth = function (month = 0) {

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

        //var currentMonth = new Date().getMonth(); // 0-11
        var currentMonth = month;

        if (currentMonth == 0) {
            $scope.PreviousMonthId = 12;
            $scope.PreviousMonth = "December";
        }
        else {
            $scope.PreviousMonthId = months[currentMonth - 1].Id;
            $scope.PreviousMonth = months[currentMonth - 1].Name;
        }
    };

    $scope.LoadAuditFeesForm = function () {

        if (!$scope.SelectedDate) {
            $scope.AuditFeesList = [];
            return;
        }

        var date = new Date($scope.SelectedDate);

        var year = date.getFullYear();
        var month = date.getMonth();

        $scope.SetPreviousMonth(month);

        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        // Previous Financial Year
        var previousFYStartYear = currentFYStartYear - 1;
        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        var distId = $scope.UserDetails.DISTRICT_ID;
        var finYear = previousFinancialYear;
        $scope.FinancialYearName = finYear;

        $http.get(SiteUrl + "Auditor/GetAuditFeesDetails", {
            params: { month: month, financialYear: finYear, districtId: distId }
        }).success(function (response) {

            //var data = response || {};
            $scope.AuditFeesList = response;
            //console.log("API Response:", data);
            //console.log('data', data);
            //console.log('AuditFeesList', $scope.AuditFeesList);
            //$scope.AuditFeesList = angular.isArray(data) ? data : (data.Records || []);

            $(".se-pre-con").fadeOut("slow");
        }, function (error) {

            console.error("GetAuditFeesDetails Error:", error);
            alert("Unable to load Audit Fees details.");
            $(".se-pre-con").fadeOut("slow");
        });

    }

    $scope.ValidateBackLog = function (item, field) {

        var total = Number(item.PreviousOutstanding) || 0;
        var PMBackLog = Number(item.PreviousMonthBacklogRecovery) || 0;
        var CMBackLog = Number(item.CurrentMonthBacklogRecovery) || 0;

        if (total < 0 || PMBackLog < 0 || CMBackLog < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.ValidateBackLog(item);
            return;
        }

        if ((PMBackLog + CMBackLog) > total) {

            alert("गत माह के बैकलॉग + माह के बैकलॉग का योग कुल बैकलॉग से अधिक नहीं हो सकता।");

            if (field === "PreviousMonthBacklogRecovery") {
                item.PreviousMonthBacklogRecovery = 0;
            }
            else if (field === "CurrentMonthBacklogRecovery") {
                item.CurrentMonthBacklogRecovery = 0;
            }
            else if (field === "PreviousOutstanding") {
                item.PreviousOutstanding = PMBackLog + CMBackLog;
            }

            total = Number(item.PreviousOutstanding) || 0;
            PMBackLog = Number(item.PreviousMonthBacklogRecovery) || 0;
            CMBackLog = Number(item.CurrentMonthBacklogRecovery) || 0;
        }
    };

    $scope.ValidateAssessmentRecovery = function (item, field) {

        var PAssessment = Number(item.PreviousMonthCurrentYearAssessment) || 0;
        var PRecovery = Number(item.PreviousMonthCurrentYearRecovery) || 0;
        var CAssessment = Number(item.CurrentMonthCurrentYearAssessment) || 0;
        var CRecovery = Number(item.CurrentMonthCurrentYearRecovery) || 0;

        if (PAssessment < 0 || PRecovery < 0 || CAssessment < 0 || CRecovery < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.ValidateAssessmentRecovery(item);
            return;
        }

        if ((PAssessment + PRecovery) <= (CAssessment + CRecovery)) {

            alert("गत माह में निर्धारण + माह में निर्धारण का योग और गत माह की वसूली + माह की वसूली का योग से अधिक नहीं हो सकता।");

            if (field === "PreviousMonthCurrentYearAssessment") {
                item.PreviousMonthCurrentYearAssessment = 0;
            }
            else if (field === "PreviousMonthCurrentYearRecovery") {
                item.PreviousMonthCurrentYearRecovery = 0;
            }
            else if (field === "CurrentMonthCurrentYearAssessment") {
                item.CurrentMonthCurrentYearAssessment = 0;
            }
            else if (field === "CurrentMonthCurrentYearRecovery") {
                item.CurrentMonthCurrentYearRecovery = 0;
            }

            PAssessment = Number(item.PreviousMonthCurrentYearAssessment) || 0;
            PRecovery = Number(item.PreviousMonthCurrentYearRecovery) || 0;
            CAssessment = Number(item.CurrentMonthCurrentYearAssessment) || 0;
            CRecovery = Number(item.CurrentMonthCurrentYearRecovery) || 0;
        }
    };

    $scope.GetSelectedMonth = function () {

        if (!$scope.SelectedDate) {
            return 0;
        }

        var date = new Date($scope.SelectedDate);
        console.log('date', date);

        return date.getMonth() + 1;
    };

    $scope.SaveAuditFeesForm = function () {

        var date = new Date($scope.SelectedDate);

        var year = date.getFullYear();
        var month = date.getMonth();

        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        // Previous Financial Year
        var previousFYStartYear = currentFYStartYear - 1;
        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        //var distId = $scope.UserDetails.DISTRICT_ID;
        var finYear = previousFinancialYear;

        var SaveData = [];

        if (!$scope.AuditFeesList || $scope.AuditFeesList.length === 0) {
            alert("No audit data available.");
            return;
        }

        //for (var i = 0; i < $scope.AuditFeesList.length; i++) {
        //    var area = $scope.AuditFeesList[i];
        //    if (!$scope.ValidateDepartmentAudit(area)) {
        //        return;
        //    }
        //}

        angular.forEach($scope.AuditFeesList, function (area) {

            SaveData.push({
                FinancialYear: finYear,
                Month: month,
                CommitteeTypeId: Number(area.CommitteeTypeId || area.CommiteeTypeId) || 0,
                PreviousOutstanding: Number(area.PreviousOutstanding) || 0,
                PreviousMonthCurrentYearAssessment: Number(area.PreviousMonthCurrentYearAssessment) || 0,
                PreviousMonthBacklogRecovery: Number(area.PreviousMonthBacklogRecovery) || 0,
                PreviousMonthCurrentYearRecovery: Number(area.PreviousMonthCurrentYearRecovery) || 0,
                CurrentMonthCurrentYearAssessment: Number(area.CurrentMonthCurrentYearAssessment) || 0,
                CurrentMonthBacklogRecovery: Number(area.CurrentMonthBacklogRecovery) || 0,
                CurrentMonthCurrentYearRecovery: Number(area.CurrentMonthCurrentYearRecovery) || 0
            });

        });

        if (SaveData.length === 0) {
            alert("No records found.");
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Auditor/SaveAuditFees", SaveData)
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    alert("Record Saved Successfully.");
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
        $scope.GetUserDetailsBySSO();
        //$scope.LoadAuditFeesForm();
    }

    $scope.OnPageLoad();

});