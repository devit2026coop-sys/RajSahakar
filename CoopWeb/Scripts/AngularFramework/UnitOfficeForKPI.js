var CooperativeSocietyFormModule = angular.module("ApplicationUnitOfficeKPIModule", ['angularUtils.directives.dirPagination']);

CooperativeSocietyFormModule.controller("CooperativeUnitOfficeKPIFormCont", function ($rootScope, $scope, $http) {

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
        $scope.GetUnitOfficeDetail();
        $scope.SetPreviousMonth();

    }
    $scope.IsUpdate = false;
    $scope.IsDivisionKPIDisabled = false;
    $scope.PendingDistrictMessage = "";
    $scope.LoanBankList = function () {
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=0").
            success(function (data, status) {
                $scope.BankMasterList = data;
                var districtid = $('#hdnDistrictId').val();
                var levelid = $('#hdnLevelIdId').val();
                var ccbObj = $scope.BankMasterList.filter(function (x) { return x.DistrictId == districtid; })[0];
                var CCBId = ccbObj ? ccbObj.CCBId : null;
                // Bank login
                if (levelid == "7" && districtid) {
                    $scope.IsBankFreezed = true;
                    $scope.GopalCreditCardModel.LoanBankID = CCBId.toString();
                }
                else {
                    $scope.IsBankFreezed = false;
                }
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };


    $scope.GetSocietyByPacsSSO = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/PacsUserLoginElectionDetails", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data.filter(x => x.IsObjection == false);
            $scope.SocietyDataList = $scope.TableData;
            $scope.SocietyList = $scope.SocietyDataList;
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };


    $scope.GetUnitOfficeDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/KPI/GetUnitOfficeKPIDetails")
            .success(function (data) {
                $scope.KPIData = data;
                $scope.KPIAreas = [];
                angular.forEach($scope.KPIData, function (item) {
                    var area = null;
                    for (var i = 0; i < $scope.KPIAreas.length; i++) {
                        if ($scope.KPIAreas[i].KPIAreaId == item.KPIAreaId) {
                            area = $scope.KPIAreas[i];
                            break;
                        }
                    }
                    if (area == null) {
                        area = {
                            KPIAreaId: item.KPIAreaId,
                            KPIArea: item.KPIArea,
                            Indicators: []
                        };

                        $scope.KPIAreas.push(area);
                    }

                    area.Indicators.push({
                        KPIIndicatorsId: item.KPIIndicatorsId,
                        KPIIndicators: item.KPIIndicators,
                        WeightedMarks: item.WeightedMarks,
                        Target: item.KPIIndicatorsTarget,
                        Achievement: item.KPIIndicatorsAchievement || "",
                        Progress: item.ProgressPercentage || "",
                        MarksObtained: item.MarksObtained || ""
                    });
                    $scope.GetKPIResult();
                });

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                $scope.KPIAreas = [];
            });
    };

    $scope.GetKPIResult = function () {

        $http.get(SiteUrl + "/KPI/GetKPIDetails", {
            params: { officeId: $scope.OfficeId, month: $scope.PreviousMonthId, kpiFor: "UnitOffice", DistrictId: $scope.DistrictId }
        }).success(function (data) {

            if (data && data.length > 0) {
                $scope.IsUpdate = true;
            } else {
                $scope.IsUpdate = false;
            }

            angular.forEach(data, function (saved) {
                angular.forEach($scope.KPIAreas, function (area) {
                    angular.forEach(area.Indicators, function (item) {
                        if (item.KPIIndicatorsId == saved.KPIIndicatorsId) {
                            item.Target = Number(saved.KPIIndicatorsTarget);
                            item.Achievement = Number(saved.KPIIndicatorsAchivement);
                            item.Progress = Number(saved.ProgressPercentage);
                            item.MarksObtained = Number(saved.MarksObtained);
                            item.WeightedMarks = Number(saved.WeightedMarks);
                        }
                    });
                });
            });

            $scope.$applyAsync();

        }).error(function (err) {
            console.log(err);
        });

    };

    $scope.Calculate = function (item) {

        var target = parseFloat(item.Target) || 0;
        var achievement = parseFloat(item.Achievement) || 0;

        if (item.OriginalWeightedMarks === undefined || item.OriginalWeightedMarks === null) {
            item.OriginalWeightedMarks = parseFloat(item.WeightedMarks) || 0;
        }

        var weightedMarks = parseFloat(item.OriginalWeightedMarks) || 0;

        if (target == 0 || achievement == 0) {
            item.MarksObtained = "0.00";
            item.WeightedMarks = "0.00";
            item.Progress = "0.00";

            return;
        }

        item.WeightedMarks = weightedMarks.toFixed(2);

        if (target == 0) {
            item.Progress = "100.00";
            item.MarksObtained = weightedMarks.toFixed(2);
            return;
        }

        var progress = Math.min((achievement / target) * 100, 100);
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

        var currentMonth = new Date().getMonth();

        if (currentMonth == 0) {
            $scope.PreviousMonthId = 12;
            $scope.PreviousMonth = "December";
        }
        else {
            $scope.PreviousMonthId = months[currentMonth - 1].Id;
            $scope.PreviousMonth = months[currentMonth - 1].Name;
        }
    };

    $scope.SaveKPIResult = function () {
        var SaveData = [];
        var isValid = true;
        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {

                if (item.Target === undefined || item.Target === null || String(item.Target).trim() === "") {
                    toastr.error("Please enter Target for: " + item.KPIIndicators);
                    isValid = false;
                    return;
                }

                if (item.Achievement === undefined || item.Achievement === null || String(item.Achievement).trim() === "") {
                    toastr.error("Please enter Achievement for: " + item.KPIIndicators);
                    isValid = false;
                    return;
                }
                SaveData.push({
                    FY: $scope.FY,
                    Month: $scope.PreviousMonthId,
                    KPIFor: "UnitOffice",
                    OfficeId: $scope.OfficeId,
                    KPIIndicatorsId: item.KPIIndicatorsId,
                    KPIIndicatorsTarget: item.Target || 0,
                    KPIIndicatorsAchievement: item.Achievement || 0,
                    ProgressPercentage: item.Progress || 0,
                    WeightedMarks: item.WeightedMarks || 0,
                    MarksObtained: item.MarksObtained || 0
                });
            });

            if (!isValid)
                return false;
        });

        if (!isValid)
            return;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/KPI/SaveKPIResult", SaveData).success(function (data) {
            $(".se-pre-con").fadeOut("slow");
            if (data > 0) {
                alert('Record Saved Successfully.');
                location.reload();
            }
            else {
                alert("Unable to save.");
            }
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
            alert("Error while saving.");

        });
    };

    $scope.UpdateKPIResult = function () {
        var SaveData = [];
        var isValid = true;
        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {

                SaveData.push({
                    FY: $scope.FY,
                    Month: $scope.PreviousMonthId,
                    KPIFor: "UnitOffice",
                    OfficeId: $scope.OfficeId,
                    KPIIndicatorsId: item.KPIIndicatorsId,
                    KPIIndicatorsTarget: item.Target || 0,
                    KPIIndicatorsAchievement: item.Achievement || 0,
                    ProgressPercentage: item.Progress || 0,
                    WeightedMarks: item.WeightedMarks || 0,
                    MarksObtained: item.MarksObtained || 0
                });
            });

            if (!isValid)
                return false;
        });

        if (!isValid)
            return;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/KPI/SaveKPIResult", SaveData).success(function (data) {
            $(".se-pre-con").fadeOut("slow");
            if (data > 0) {
                alert(' Record Update Successfully.');
            }
            else {
                alert("Unable to Update.");
            }
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
            alert("Error while saving.");

        });
    };

    $scope.GetUserDetailsBySSO = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetUserDetailsBySSO")
            .success(function (data) {
                $scope.UserDetails = data[0];
                $scope.OfficeId = data[0].OfficeId;
                $scope.DistrictId = data[0].DISTRICT_ID;
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.OnPageLoad();

});