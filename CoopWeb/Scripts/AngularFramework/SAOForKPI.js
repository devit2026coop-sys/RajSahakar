var CooperativeSocietyFormModule = angular.module("ApplicationSAOKPIModule", ['angularUtils.directives.dirPagination']);
CooperativeSocietyFormModule.controller("CooperativeSAOKPIFormCont", function ($rootScope, $scope, $http) {

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

        //$scope.LoanBankList();
        $scope.GetSAKPISAODetails();
        $scope.GetUserDetailsBySSO();
        $scope.SetPreviousMonth();
    }
    $scope.IsUpdate = false;

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


    $scope.GetSAKPISAODetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetSAKPIDetails")
            .success(function (data) {
                $scope.KPIData = data;
                console.log('$scope.KPIData', $scope.KPIData);
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
                        Target: item.KPIIndicatorsId == 39 ? 5 : (item.KPIIndicatorsTarget || ""),
                        Achievement: item.KPIIndicatorsAchievement || "",
                        Progress: item.ProgressPercentage || "",
                        MarksObtained: item.MarksObtained || ""
                    });
                });

                $scope.GetKPIResult();
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                $scope.KPIAreas = [];
            });
    };


    $scope.GetKPIResult = function () {
        $http.get(SiteUrl + "/KPI/GetKPIDetails", {
            params: { officeId: $scope.OfficeId, month: $scope.PreviousMonthId, kpiFor: "SA", DistrictId: $scope.DistrictId }
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
                            item.Target = item.KPIIndicatorsId == 39 ? 5 : Number(saved.KPIIndicatorsTarget);
                            if (item.KPIIndicatorsId == 39) {
                                item.Achievement = saved.KPIIndicatorsAchivement.toString();
                            }
                            else {
                                item.Achievement = Number(saved.KPIIndicatorsAchivement);
                            }
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

    //old
    //$scope.Calculate = function (item) {
    //    var target = parseFloat(item.Target) || 0;
    //    var achievement = parseFloat(item.Achievement) || 0;
    //    var weightedMarks = parseFloat(item.WeightedMarks) || 0;

    //    if (target == 0 && achievement == 0) {
    //        item.Progress = "0.00";
    //        item.MarksObtained = "0.00";
    //        item.WeightedMarks = "0.00";
    //        return;
    //    }

    //    // KPI 36      
    //    if (item.KPIIndicatorsId == 36) {
    //        var totalTarget = 0;
    //        angular.forEach($scope.KPIAreas, function (area) {
    //            if (area.KPIAreaId == 11) {
    //                angular.forEach(area.Indicators, function (ind) {
    //                    if (ind.KPIIndicatorsId == 23 || ind.KPIIndicatorsId == 24 || ind.KPIIndicatorsId == 25) {
    //                        totalTarget += parseFloat(ind.Target) || 0;
    //                    }
    //                });
    //            }
    //        });

    //        if (totalTarget == 0) {
    //            item.Progress = "100.00";
    //            item.MarksObtained = Math.min((achievement / target) * 100, 100);
    //        }
    //        else if (target == 0) {
    //            item.Progress = "0.00";
    //            item.MarksObtained = "0.00";
    //        }
    //        else {
    //            var progress = Math.min((achievement / target) * 100, 100);
    //            item.Progress = progress.toFixed(2);
    //            item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
    //        }

    //        return;
    //    }

    //    if (item.KPIIndicatorsId == 39) {
    //        switch (parseInt(item.Achievement)) {
    //            case 1:
    //                item.Progress = "100.00";
    //                break;
    //            case 2:
    //                item.Progress = "80.00";
    //                break;
    //            case 3:
    //                item.Progress = "60.00";
    //                break;
    //            case 4:
    //                item.Progress = "40.00";
    //                break;
    //            case 5:
    //                item.Progress = "20.00";
    //                break;
    //            case 6:
    //                item.Progress = "0.00";
    //                break;
    //            default:
    //                item.Progress = "0.00";
    //                break;
    //        }
    //        item.MarksObtained = ((parseFloat(item.Progress) * weightedMarks) / 100).toFixed(2);
    //        return;
    //    }

    //    if (target == 0) {
    //        item.Progress = "100.00";
    //        item.MarksObtained = weightedMarks.toFixed(2);
    //        return;
    //    }

    //    //new
    //    var progress = Math.min((achievement / target) * 100, 100);
    //    item.Progress = progress.toFixed(2);
    //    item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
    //};

    $scope.Calculate = function (item) {

        var target = parseFloat(item.Target) || 0;
        var achievement = parseFloat(item.Achievement) || 0;

        if (item.OriginalWeightedMarks === undefined || item.OriginalWeightedMarks === null) {
            item.OriginalWeightedMarks = parseFloat(item.WeightedMarks) || 0;
        }

        var weightedMarks = parseFloat(item.OriginalWeightedMarks) || 0;

        if (target == 0 && achievement == 0) {
            item.Progress = "0.00";
            item.MarksObtained = "0.00";
            item.WeightedMarks = "0.00";

            return;
        }

        item.WeightedMarks = weightedMarks.toFixed(2);

        if (item.KPIIndicatorsId == 36) {

            var totalTarget = 0;

            angular.forEach($scope.KPIAreas, function (area) {
                if (area.KPIAreaId == 11) {
                    angular.forEach(area.Indicators, function (ind) {
                        if (ind.KPIIndicatorsId == 23 || ind.KPIIndicatorsId == 24 || ind.KPIIndicatorsId == 25) {
                            totalTarget += parseFloat(ind.Target) || 0;
                        }
                    });
                }
            });


            if (totalTarget == 0) {
                item.Progress = "100.00";
                item.MarksObtained = Math.min((achievement / target) * 100, 100).toFixed(2);
            }
            else if (target == 0) {
                item.Progress = "0.00";
                item.MarksObtained = "0.00";
            }
            else {
                var progress = Math.min((achievement / target) * 100, 100);
                item.Progress = progress.toFixed(2);
                item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
            }

            return;
        }

        if (item.KPIIndicatorsId == 39) {

            switch (parseInt(item.Achievement)) {

                case 1:
                    item.Progress = "100.00";
                    break;

                case 2:
                    item.Progress = "80.00";
                    break;

                case 3:
                    item.Progress = "60.00";
                    break;

                case 4:
                    item.Progress = "40.00";
                    break;

                case 5:
                    item.Progress = "20.00";
                    break;

                case 6:
                    item.Progress = "0.00";
                    break;

                default:
                    item.Progress = "0.00";
                    break;
            }

            item.MarksObtained = ((parseFloat(item.Progress) * weightedMarks) / 100).toFixed(2);
            return;
        }

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
                    KPIFor: "SA",
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
        $http.post(SiteUrl + "/Society/SaveKPIResult", SaveData).success(function (data) {
            $(".se-pre-con").fadeOut("slow");
            if (data > 0) {
                alert("Record Saved Successfully.");
                window.location.reload();
            }
            else {
                alert("Unable to save.");
                window.location.reload();
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
                    KPIFor: "SA",
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
        $http.post(SiteUrl + "/Society/SaveKPIResult", SaveData)
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    alert("Record Update Successfully.");
                    window.location.reload();
                }
                else {
                    alert("Unable to Update.");
                    window.location.reload();
                }
            }).error(function () {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while Update.");

            });
    };

    $scope.GetUserDetailsBySSO = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetUserDetailsBySSO").success(function (data) {
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