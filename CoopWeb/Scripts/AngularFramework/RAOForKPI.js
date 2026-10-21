var CooperativeSocietyFormModule = angular.module("ApplicationRAOKPIModule", ['angularUtils.directives.dirPagination']);

CooperativeSocietyFormModule.controller("CooperativeRAOKPIFormCont", function ($rootScope, $scope, $http) {

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

        // load form data from database once page/translations are ready
        $scope.GetKPIStatusForDisable();
        //$scope.KPIforPacs();
        //$scope.LoanBankList();
        $scope.GetSocietyByPacsSSO();
        $scope.GetSAKPIRAODetails();
        $scope.GetUserDetailsBySSO();
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


    $scope.GetSAKPIRAODetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetRAKPIDetails")
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
                        Target: item.KPIIndicatorsId == 19 ? 5 : (item.KPIIndicatorsTarget || ""),
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

    $scope.GetKPIStatusForDisable = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/KPI/GetDivisionKPIStatus")
            .success(function (data) {
                if (data != null && data.length > 0) {
                    $scope.DivisionKPIStatus = data;
                    $scope.KPIforPacs();
                    $scope.LoanBankList();
                    if ($scope.DivisionKPIStatus[0].DivisionStatus == "Enabled") {
                        $scope.PendingDistrictMessage = "";
                        $scope.IsDivisionKPIDisabled = false;
                    }
                    else {
                        $scope.PendingDistrictMessage = $scope.DivisionKPIStatus[0].PendingDistrictList;
                        $scope.IsDivisionKPIDisabled = true;

                    }
                }
                $(".se-pre-con").fadeOut("slow");

            });
    };

    $scope.GetKPIResult = function () {
        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {

                if (item.KPIIndicatorsId == 14 && $scope.DivisionKPIStatus && $scope.DivisionKPIStatus.length > 0 && $scope.DivisionKPIStatus[0].DivisionStatus == "Enabled") {

                    item.Target = Number($scope.DivisionKPIStatus[0].KPIIndicatorsTarget);
                    item.Achievement = Number($scope.DivisionKPIStatus[0].KPIIndicatorsAchivement);

                    $scope.Calculate(item);
                }

            });
        });

        $http.get(SiteUrl + "/Society/GetKPIDetails", {
            params: { officeId: $scope.OfficeId, month: $scope.PreviousMonthId, kpiFor: "RAO", DistrictId: "0" }
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
                            item.Target = item.KPIIndicatorsId == 19 ? 5 : Number(saved.KPIIndicatorsTarget);
                            if (item.KPIIndicatorsId == 19) {
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
    //$scope.Calculates = function (item) {
    //    var target = parseFloat(item.Target) || 0;
    //    var achievement = parseFloat(item.Achievement) || 0;
    //    var weightedMarks = parseFloat(item.WeightedMarks) || 0;

    //    if (target == 0 || achievement == 0) {
    //        item.MarksObtained = "0.00";
    //        item.WeightedMarks = "0.00";
    //        item.Progress = "0.00";
    //        return;
    //    }


    //    if (item.KPIIndicatorsId == 19) {

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

    //    // KPI 16      
    //    if (item.KPIIndicatorsId == 16) {
    //        var totalTarget = 0;
    //        angular.forEach($scope.KPIAreas, function (area) {
    //            if (area.KPIAreaId == 2) {
    //                angular.forEach(area.Indicators, function (ind) {
    //                    if (ind.KPIIndicatorsId == 6 || ind.KPIIndicatorsId == 7 || ind.KPIIndicatorsId == 8 || ind.KPIIndicatorsId == 9) {
    //                        totalTarget += parseFloat(ind.Target) || 0;
    //                    }
    //                });
    //            }
    //        });

    //        if (totalTarget == 0) {
    //            item.Progress = "100.00";
    //            item.MarksObtained = weightedMarks.toFixed(2);
    //        }
    //        else if (target == 0) {
    //            item.Progress = "0.00";
    //            item.MarksObtained = "0.00";
    //        }
    //        else {
    //            //new
    //            var progress = Math.min((achievement / target) * 100, 100);

    //            item.Progress = progress.toFixed(2);
    //            item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
    //        }

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

        if (target == 0 || achievement == 0) {
            item.MarksObtained = "0.00";
            item.WeightedMarks = "0.00";
            item.Progress = "0.00";

            return;
        }

        item.WeightedMarks = weightedMarks.toFixed(2);

        if (item.KPIIndicatorsId == 19) {

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

            item.WeightedMarks = weightedMarks.toFixed(2);
            item.MarksObtained = ((parseFloat(item.Progress) * weightedMarks) / 100).toFixed(2);
            return;
        }

        if (item.KPIIndicatorsId == 16) {

            var totalTarget = 0;

            angular.forEach($scope.KPIAreas, function (area) {
                if (area.KPIAreaId == 2) {
                    angular.forEach(area.Indicators, function (ind) {
                        if (ind.KPIIndicatorsId == 6 || ind.KPIIndicatorsId == 7 || ind.KPIIndicatorsId == 8 || ind.KPIIndicatorsId == 9) {
                            totalTarget += parseFloat(ind.Target) || 0;
                        }
                    });
                }
            });

            if (totalTarget == 0) {
                item.Progress = "100.00";
                item.MarksObtained = weightedMarks.toFixed(2);
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
                    KPIFor: "RAO",
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
                alert(data + ' Record Saved Successfully.');
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
                    KPIFor: "RAO",
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
                alert(data + ' Record Update Successfully.');
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
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
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
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=KPIFORPACS").success(function (data, status) {
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
        }).error(function (data, status) {
            $scope.KPIforPacsdata = [];
            $(".blockUI").fadeOut("slow");
        });
    }

    $scope.OnPageLoad();

});