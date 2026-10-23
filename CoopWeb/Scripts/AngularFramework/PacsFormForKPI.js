
var KPIForPacsMod = angular.module("KPIForPacsMod", ['angularUtils.directives.dirPagination']);
KPIForPacsMod.controller("KPIForPacsCtrl", function ($rootScope, $scope, $http) {
    $scope.IsUpdate = false;
    $scope.FY = "";
    $scope.pacsDataList = [];
    $scope.pacsDetail = {
        DistrictId: "0",
        CCBId: "0",
        CCBName: "",
        CCBBranchName: "",
        PacsName: "",
        PacsManagerName: "",
        PacsRegistrationNo: "",
        GCCLoginId: $("#hdnGCCLoginId").val() ?? 0,
        LoginMappingId: $("#hdnLoginMappingId").val() ?? 0,
    }
    $scope.SaveData = [];
    $scope.OnPageLoad = function () {
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
        };

        //$scope.LoanBankList();
        $scope.SetCurrentFY();
        $scope.GetSAKPISAODetails();

        //$scope.GetUserDetailsBySSO();
        //$scope.GetKPIResult();
        $scope.SetPreviousMonth();

        //$scope.GetSocietyBySSO();
        if ($scope.pacsDetail.GCCLoginId != null && $scope.pacsDetail.GCCLoginId > 0) {
            $scope.GetDetailsByPacs();
            $scope.IsFormDisabled = false;
        } else {
            $scope.IsFormDisabled = true;
            toastr.warning("You cannot fill this form.", "", {
                timeOut: 10000
            });
            //alert("You cannot fill this form.");
        }
    }

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

    //$scope.SocietyDataList = function (ActId, SocietyTypeId, ClassId) {
    //    $(".se-pre-con").fadeIn("slow");
    //    $http.post(SiteUrl + "/KPI/CitizenDashboardDataWithDepartmentUser", {
    //        "ActId": ActId == undefined ? 0 : parseInt(ActId),
    //        "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId),
    //        "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
    //    }).success(function (data, status) {
    //        $scope.TableData = data.filter(x => x.IsObjection == false);
    //        $scope.SocietyDataList = $scope.TableData;
    //        $scope.SocietyList = $scope.SocietyDataList;
    //        $(".se-pre-con").fadeOut("slow");
    //        if ($scope.SocietyDataList.length == 1) {
    //            $scope.godown.SocietyID = $scope.SocietyDataList[0].SocietyID.toString();
    //            $scope.OnSocietyChange();
    //            $scope.IsSocietyDisabled = true;
    //        } else {
    //            $scope.godown.SocietyID = "";
    //            $scope.IsSocietyDisabled = false;
    //            $scope.godown.ApplicantName = "";
    //            $scope.district_MemberAdd = "";
    //            $scope.ddlblocki_MemberAdd = "";
    //            $scope.ddlGP_MemberAdd = "";
    //            $scope.ddlVillage_MemberAdd = "";
    //        }

    //    }).error(function (data, status) {
    //        $(".se-pre-con").fadeOut("slow");
    //        $scope.TableData = [];
    //        $scope.SocietyList = [];
    //    });
    //};

    $scope.Calculate = function (indicator) {

        // ==========================================
        // KPI 83 - Any Other
        // ==========================================
        if (indicator.KPIIndicatorsId == 83) {

            // First Yes = 2.00
            indicator.MarksObtained = $scope.isYesOption83(indicator) ? "2.00" : "0.00";

            // Second Yes = 2.00
            indicator.MarksObtained2 = $scope.isYesOption83Option2(indicator) ? "2.00" : "0.00";

            // Third Yes = 2.00
            indicator.MarksObtained3 = $scope.isYesOption83Option3(indicator) ? "2.00" : "0.00";

            // Optional total
            indicator.TotalMarksObtained = (parseFloat(indicator.MarksObtained || 0) + parseFloat(indicator.MarksObtained2 || 0) + parseFloat(indicator.MarksObtained3 || 0)).toFixed(2);

            return;
        }

        var selected = null;

        angular.forEach(indicator.Options, function (item) {

            if (item.KPI_KEYId == indicator.SelectedOption) {
                selected = item;
            }

        });

        if (selected) {
            indicator.MarksObtained = selected.Marks;
        }
        else {
            indicator.MarksObtained = "0.00";
        }
    };

    // ==========================================
    // KPI 83 - First Yes
    // ==========================================
    $scope.isYesOption83 = function (indicator) {

        if (!indicator || !indicator.Options || indicator.Options.length === 0 || !indicator.SelectedOption) {
            return false;
        }

        var selected = indicator.Options.find(function (option) {
            return option.KPI_KEYId == indicator.SelectedOption;
        });

        return selected && selected.InputType == "Yes";
    };


    // ==========================================
    // KPI 83 - Second Yes
    // ==========================================
    $scope.isYesOption83Option2 = function (indicator) {

        if (!indicator || !indicator.Options || indicator.Options.length === 0 || !indicator.selectOptionAnyOther1) {
            return false;
        }

        var selected = indicator.Options.find(function (option) {
            return option.KPI_KEYId == indicator.selectOptionAnyOther1;
        });

        return selected && selected.InputType == "Yes";
    };


    // ==========================================
    // KPI 83 - Third Yes
    // ==========================================
    $scope.isYesOption83Option3 = function (indicator) {

        if (!indicator || !indicator.Options || indicator.Options.length === 0 || !indicator.selectOptionAnyOther2) {
            return false;
        }

        var selected = indicator.Options.find(function (option) {
            return String(option.KPI_KEYId) === String(indicator.selectOptionAnyOther2);
        });

        return selected && selected.InputType === "Yes";
    };

    //$scope.Calculates = function (item) {
    //    var target = parseFloat(item.Target) || 0;
    //    var achievement = parseFloat(item.Achievement) || 0;
    //    var weightedMarks = parseFloat(item.WeightedMarks) || 0;

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
    //            item.MarksObtained = weightedMarks.toFixed(2);
    //        }
    //        else if (target == 0) {
    //            item.Progress = "0.00";
    //            item.MarksObtained = "0.00";
    //        }
    //        else {
    //            var progress = Math.min((achievement / target) * 100, 100);

    //            item.Progress = progress.toFixed(2);
    //            item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
    //            //item.Progress = ((achievement / target) * 100).toFixed(2);
    //            //item.MarksObtained = ((parseFloat(item.Progress) * weightedMarks) / 100).toFixed(2);
    //        }

    //        return;
    //    }
    //    //if (item.KPIIndicatorsId == 39) {

    //    //    if (target <= 5) {
    //    //        item.Progress = "100.00";
    //    //        item.MarksObtained = weightedMarks.toFixed(2);
    //    //    }
    //    //    else {
    //    //        item.Progress = "0.00";
    //    //        item.MarksObtained = "0.00";
    //    //    }
    //    //    return;
    //    //}

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

    //    var progress = Math.min((achievement / target) * 100, 100);

    //    item.Progress = progress.toFixed(2);
    //    item.MarksObtained = ((progress * weightedMarks) / 100).toFixed(2);
    //};
    $scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/KPI/CitizenDashboardDataWithDepartmentUser", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId),
            "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId),
            "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
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

        return $http.get(SiteUrl + "/KPI/GetPacsKPIDetails", {
            params: {
                financialYear: $scope.FY
            }
        }).success(function (data) {

            $scope.KPIData = data;
            $scope.KPIAreas = [];

            if ($scope.KPIData != null && $scope.KPIData.length > 0) {
                $scope.pacsDetail.PacsRegistrationNo =
                    $scope.KPIData[0].PacsRegistrationNo &&
                        $scope.KPIData[0].PacsRegistrationNo.trim()
                        ? $scope.KPIData[0].PacsRegistrationNo.trim()
                        : "";
            }

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

                var indicator = null;

                for (var j = 0; j < area.Indicators.length; j++) {
                    if (area.Indicators[j].KPIIndicatorsId == item.KPIIndicatorsId) {
                        indicator = area.Indicators[j];
                        break;
                    }
                }

                if (indicator == null) {
                    indicator = {
                        KPIIndicatorsId: item.KPIIndicatorsId,
                        KPIIndicators: item.KPIIndicators,
                        WeightedMarks: item.WeightedMarks,
                        Target: item.KPIIndicatorsTarget || "",
                        Achievement: item.KPIIndicatorsAchievement || "",
                        Progress: item.ProgressPercentage || "",
                        MarksObtained: item.MarksObtained || "",
                        KPIAreaId: item.KPIAreaId ?? 0,
                        Options: []
                    };

                    area.Indicators.push(indicator);
                }

                indicator.Options.push({
                    KPI_KEYId: item.KPI_MarksId,
                    InputType: item.inputtype,
                    Marks: item.marks
                });
            });

            $(".se-pre-con").fadeOut("slow");

        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
            $scope.KPIAreas = [];
        });
    };


    $scope.GetKPIResult = function () {

        $http.get(SiteUrl + "/KPI/GetKPIDetails", {
            params: { officeId: $scope.pacsDetail.GCCLoginId, month: $scope.PreviousMonthId, kpiFor: "PACS", DistrictId: "0" }
        }).success(function (data) {
            angular.forEach($scope.KPIAreas, function (area) {
                angular.forEach(area.Indicators, function (indicator) {
                    $scope.CCBId = "0";
                    indicator.Target = "";
                    indicator.Achievement = "";
                    indicator.Progress = "";
                    indicator.MarksObtained = "";
                    indicator.SelectedOption = null;
                    // KPI 83
                    indicator.selectOptionAnyOther1 = null;
                    indicator.selectOptionAnyOther2 = null;
                    indicator.MarksObtained2 = "";
                    indicator.MarksObtained3 = "";
                });
            });

            if (data && data.length > 0) {
                if (data[0].Packs_CCBId != null && data[0].Packs_CCBId > 0) {
                    $scope.CCBId = data[0].Packs_CCBId.toString();
                }
                $scope.IsUpdate = true;
            } else {
                $scope.IsUpdate = false;
            }

            angular.forEach($scope.KPIAreas, function (area) {
                angular.forEach(area.Indicators, function (indicator) {
                    var dbItem = data.find(function (x) {
                        return x.KPIIndicatorsId == indicator.KPIIndicatorsId;
                    });


                    if (dbItem) {
                        indicator.Target = dbItem.KPIIndicatorsTarget;
                        indicator.Achievement = dbItem.KPIIndicatorsAchievement;
                        indicator.Progress = dbItem.ProgressPercentage;
                        indicator.MarksObtained = dbItem.MarksObtained;
                        indicator.SelectedOption = dbItem.Packs_KPI_MarksId;
                        $scope.pacsDetail.PacsRegistrationNo = dbItem.PacsRegistrationNo;

                        if (dbItem.KPIIndicatorsId == 83) {
                            var isOption2Yes = dbItem.selectOptionAnyOther1 === true || dbItem.selectOptionAnyOther1 === 1 || dbItem.selectOptionAnyOther1 === "1" || String(dbItem.selectOptionAnyOther1).toLowerCase() === "true";
                            var isOption3Yes = dbItem.selectOptionAnyOther2 === true || dbItem.selectOptionAnyOther2 === 1 || dbItem.selectOptionAnyOther2 === "1" || String(dbItem.selectOptionAnyOther2).toLowerCase() === "true";

                            var yesOption2 = indicator.Options.find(function (x) {
                                return String(x.InputType).trim().toLowerCase() === "yes";
                            });

                            var noOption2 = indicator.Options.find(function (x) {
                                return String(x.InputType).trim().toLowerCase() === "no";
                            });

                            if (isOption2Yes) {
                                if (yesOption2) {
                                    indicator.selectOptionAnyOther1 = String(yesOption2.KPI_KEYId);
                                }
                                indicator.MarksObtained2 = "2.00";
                            }
                            else {
                                if (noOption2) {
                                    indicator.selectOptionAnyOther1 = String(noOption2.KPI_KEYId);
                                }
                                indicator.MarksObtained2 = 0;
                            }

                            var yesOption3 = indicator.Options.find(function (x) {
                                return String(x.InputType).trim().toLowerCase() === "yes";
                            });

                            var noOption3 = indicator.Options.find(function (x) {
                                return String(x.InputType).trim().toLowerCase() === "no";
                            });


                            if (isOption3Yes) {
                                if (yesOption3) {
                                    indicator.selectOptionAnyOther2 = String(yesOption3.KPI_KEYId);
                                }
                                indicator.MarksObtained3 = "2.00";
                            }
                            else {
                                if (noOption3) {
                                    indicator.selectOptionAnyOther2 = String(noOption3.KPI_KEYId);
                                }
                                indicator.MarksObtained3 = 0;
                            }
                        }

                        if (dbItem.KPIIndicatorsId == 113) {
                            indicator.KCCLoanDisbursedAmt = Number(dbItem.KCCLoanDisbursedAmt);
                        }

                        if (dbItem.KPIIndicatorsId == 55) {
                            indicator.MembershipPrevious = Number(dbItem.MembershipPreviousInd);
                            indicator.MembershipCurrent = Number(dbItem.MembershipCurrentInd);
                        }

                        if (dbItem.KPIIndicatorsId == 58) {
                            indicator.ShareCapitalPrevious = Number(dbItem.ShareCurrentInd);
                            indicator.ShareCapitalCurrent = Number(dbItem.SharePreviousInd);
                        }

                        if (dbItem.KPIIndicatorsId == 71 || dbItem.KPIIndicatorsId == 72 || dbItem.KPIIndicatorsId == 73 || dbItem.KPIIndicatorsId == 74) {
                            indicator.AgricultureTrunOver = Number(dbItem.AgricultureTrunOver);
                        }
                    }
                });
            });

            $scope.$applyAsync();

        }).error(function (err) {

            console.log(err);

        });

    };
    $scope.GetKPIResultold = function () {
        $http.get(SiteUrl + "/KPI/GetKPIDetails", {
            params: { officeId: $scope.pacsDetail.GCCLoginId, month: $scope.PreviousMonthId, kpiFor: "PACS", DistrictId: "0" }
        }).success(function (data) {
            console.log(data);
            angular.forEach($scope.KPIAreas, function (area) {
                angular.forEach(area.Indicators, function (indicator) {
                    $scope.CCBId = "0";
                    indicator.Target = "";
                    indicator.Achievement = "";
                    indicator.Progress = "";
                    indicator.MarksObtained = "";
                    indicator.SelectedOption = null;
                });
            });

            if (data && data.length > 0) {
                //$scope.CCBId = data[0].Packs_CCBId.toString(); 
                if (data[0].Packs_CCBId != null && data[0].Packs_CCBId > 0) { $scope.CCBId = data[0].Packs_CCBId.toString(); }
                $scope.IsUpdate = true;
            } else {
                $scope.IsUpdate = false;
            }

            angular.forEach($scope.KPIAreas, function (area) {
                angular.forEach(area.Indicators, function (indicator) {

                    var dbItem = data.find(function (x) {
                        return x.KPIIndicatorsId == indicator.KPIIndicatorsId;
                    });
                    console.log('Indicator', indicator);
                    console.log('dbItem', dbItem);
                    if (dbItem) {
                        indicator.Target = dbItem.KPIIndicatorsTarget;
                        indicator.Achievement = dbItem.KPIIndicatorsAchievement;
                        indicator.Progress = dbItem.ProgressPercentage;
                        indicator.MarksObtained = dbItem.MarksObtained;
                        indicator.SelectedOption = dbItem.Packs_KPI_MarksId;
                        $scope.pacsDetail.PacsRegistrationNo = dbItem.PacsRegistrationNo;
                        if (dbItem.KPIIndicatorsId == 113) {
                            indicator.KCCLoanDisbursedAmt = Number(dbItem.KCCLoanDisbursedAmt);
                        }
                        if (dbItem.KPIIndicatorsId == 55) {
                            indicator.MembershipPrevious = Number(dbItem.MembershipPreviousInd);
                            indicator.MembershipCurrent = Number(dbItem.MembershipCurrentInd);
                        }
                        if (dbItem.KPIIndicatorsId == 58) {
                            indicator.ShareCapitalPrevious = Number(dbItem.ShareCurrentInd);
                            indicator.ShareCapitalCurrent = Number(dbItem.SharePreviousInd);
                        }
                        if (dbItem.KPIIndicatorsId == 71 || dbItem.KPIIndicatorsId == 72 || dbItem.KPIIndicatorsId == 73 || dbItem.KPIIndicatorsId == 74) {
                            indicator.AgricultureTrunOver = Number(dbItem.AgricultureTrunOver);
                        }

                    }
                });

            });

            $scope.$applyAsync();

        }).error(function (err) {
            console.log(err);
        });

    };
    $scope.isTextInput = function (indicator) {
        if (!indicator || !indicator.Options || indicator.Options.length === 0) {
            return false;
        }

        return indicator.Options[0].InputType === "Text";
    };

    $scope.isLoanInput = function (indicator) {
        if (!indicator || !indicator.Options || indicator.Options.length === 0) {
            return false;
        }

        return indicator.Options[0].InputType === "Loan";
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
        debugger;

        var SaveData = [];
        var isValid = true;

        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {

                var kpiId = parseInt(item.KPIIndicatorsId);

                if (kpiId === 55) {

                    if (!item.MembershipCurrent) {
                        toastr.error("Please enter Membership Current and Membership Previous");
                        isValid = false;
                        return;
                    }

                }
                else if (kpiId === 58) {

                    if (!item.ShareCapitalCurrent) {
                        toastr.error("Please enter Share Capital Current and Share Capital Previous");
                        isValid = false;
                        return;
                    }

                }
                else if (kpiId === 113) {

                    if (!item.KCCLoanDisbursedAmt) {
                        toastr.error("Please enter KCC Loan Disbursed Amount");
                        isValid = false;
                        return;
                    }

                }
                else {
                    if (!item.SelectedOption) {
                        toastr.error("Please select option");
                        isValid = false;
                        return;
                    }
                }

                $scope.SaveData.push({
                    //FY: $scope.FY,
                    Month: $scope.PreviousMonthId,
                    KPIFor: "PACS",
                    //OfficeId: $scope.GccId ?? 0,
                    OfficeId: $scope.pacsDetail.GCCLoginId,
                    //CCBId: $scope.CCBId,
                    CCBId: $scope.pacsDetail.CCBId,
                    KPIIndicatorsId: item.KPIIndicatorsId,
                    KPI_MarksId: item.SelectedOption,
                    KPIIndicatorsTarget: item.Target ?? 0,
                    KPIIndicatorsAchievement: item.Achievement ?? 0,
                    ProgressPercentage: item.Progress ?? 0,
                    WeightedMarks: item.WeightedMarks ?? 0,
                    MarksObtained: item.MarksObtained ?? 0,
                    CCBDistrictId: $scope.pacsDetail.DistrictId,
                    PacsRegistrationNo: !$scope.pacsDetail.PacsRegistrationNo ? "" : $scope.pacsDetail.PacsRegistrationNo,
                    KCCLoanDisbursedAmt: !item.KCCLoanDisbursedAmt ? "0.00" : item.KCCLoanDisbursedAmt,
                    AgricultureTrunOver: !item.AgricultureTrunOver ? "0.00" : item.AgricultureTrunOver,
                    MembershipPrevious: !item.MembershipPrevious ? "0.00" : item.MembershipPrevious,
                    MembershipCurrent: !item.MembershipCurrent ? "0.00" : item.MembershipCurrent,
                    ShareCapitalPrevious: !item.ShareCapitalPrevious ? "0.00" : item.ShareCapitalPrevious,
                    ShareCapitalCurrent: !item.ShareCapitalCurrent ? "0.00" : item.ShareCapitalCurrent,
                    selectOptionAnyOther1: $scope.isYesOption83Option2(item),
                    selectOptionAnyOther2: $scope.isYesOption83Option3(item)
                });
            });

            return;

            if (!isValid)
                return false;
        });

        if (!isValid)
            return;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/KPI/SaveKPIResult", $scope.SaveData)
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    alert("Record Saved Successfully.");
                    //toastr.success("Record Saved Successfully.");
                    window.location.reload();
                }
                else {
                    alert("Unable to save.");
                    window.location.reload();
                }
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while saving.");

            });
    };


    $scope.UpdateKPIResult = function () {
        $scope.SaveData = [];
        var isValid = true;
        var rowNo = 0;
        var errorMessage = "";

        angular.forEach($scope.KPIAreas, function (area) {
            console.log($scope.KPIAreas);
            angular.forEach(area.Indicators, function (item) {

                console.log(item);
                $scope.SaveData.push({
                    Month: $scope.PreviousMonthId,
                    KPIFor: "PACS",
                    //OfficeId: $scope.GccId ?? 0,
                    OfficeId: $scope.pacsDetail.GCCLoginId,
                    //CCBId: $scope.CCBId,
                    KPIIndicatorsId: item.KPIIndicatorsId,
                    KPI_MarksId: item.SelectedOption,
                    KPIIndicatorsTarget: item.Target,
                    KPIIndicatorsAchievement: item.Achievement,
                    ProgressPercentage: item.Progress,
                    WeightedMarks: item.WeightedMarks ?? 0,
                    MarksObtained: item.MarksObtained ?? 0,
                    CCBId: $scope.pacsDetail.CCBId,
                    CCBDistrictId: $scope.pacsDetail.DistrictId,
                    CCBName: $scope.pacsDetail.CCBName,
                    CCBDistrictName: $scope.pacsDetail.DistrictName,
                    CCBBranchName: $scope.pacsDetail.CCBBranchName,
                    PacsManagerName: $scope.pacsDetail.PacsManagerName,
                    PacsName: $scope.pacsDetail.PacsName,
                    PacsRegistrationNo: !$scope.pacsDetail.PacsRegistrationNo ? "" : $scope.pacsDetail.PacsRegistrationNo,
                    KCCLoanDisbursedAmt: !item.KCCLoanDisbursedAmt ? "0.00" : item.KCCLoanDisbursedAmt,
                    AgricultureTrunOver: !item.AgricultureTrunOver ? "0.00" : item.AgricultureTrunOver,
                    MembershipPrevious: !item.MembershipPrevious ? "0.00" : item.MembershipPrevious,
                    MembershipCurrent: !item.MembershipCurrent ? "0.00" : item.MembershipCurrent,
                    ShareCapitalPrevious: !item.ShareCapitalPrevious ? "0.00" : item.ShareCapitalPrevious,
                    ShareCapitalCurrent: !item.ShareCapitalCurrent ? "0.00" : item.ShareCapitalCurrent,
                    selectOptionAnyOther1: !item.selectOptionAnyOther1 ? "0" : item.selectOptionAnyOther1,
                    selectOptionAnyOther2: !item.selectOptionAnyOther2 ? "0 " : item.selectOptionAnyOther2
                });
            });
        });

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/KPI/SaveKPIResult", $scope.SaveData)
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    // alert("Record Update Successfully.");
                    ShowOkMessage("", "Record Update Successfully.", function () {
                        window.location.reload();
                    });
                    //window.location.reload();
                }
                else {
                    ShowOkMessage("", "Unable to Update.", function () {
                    });
                }
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("", "Error while Update not updateing data.", function () {
                });
            });
    };


    $scope.SetCurrentFY = function () {
        $http.get(SiteUrl + "/Society/GetCurrentFinancialYear")
            .success(function (data) {
                $scope.FY = data[0].CurrentFY;
                console.log('$scope.FY', $scope.FY);
            })
            .error(function () {
                alert("Unable to load Current Financial Year.");
            });
    };

    $scope.GetUserDetailsBySSO = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetUserDetailsBySSO")
            .success(function (data) {
                $scope.UserDetails = data[0];
                $scope.OfficeId = data[0].OfficeId;
                $scope.GccId = data[0].GccId;
                $scope.PACSName = data[0].PACSNameEn;

                if (!$scope.GccId || parseInt($scope.GccId) === 0) {
                    $scope.IsFormDisabled = true;
                    alert("You cannot fill this form.");
                }
                else {
                    $scope.IsFormDisabled = false;
                }

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //Added new function for pacs details 18-09-2026 
    $scope.GetDistricts = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Districts = [];
            });
    };
    $scope.GetDetailsByPacs = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/KPI/PacsManagerDetails?ActionType=" + "GetDetail").then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null && response.data.Result.Data.pacsDetails != null) {
                $scope.pacsDataList = JSON.parse(response.data.Result.Data.pacsDetails);
                if ($scope.pacsDataList != null && $scope.pacsDataList.length > 0) {
                    $scope.pacsDetail = $scope.pacsDataList[0];
                    $scope.pacsDetail.GCCLoginId = $("#hdnGCCLoginId").val() ?? 0;
                    $scope.pacsDetail.LoginMappingId = $("#hdnLoginMappingId").val() ?? 0;
                    $scope.GetDistricts();
                    $scope.GetKPIResult();
                }
                else {
                    $scope.pacsDataList = [];
                }
            }
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.commonCalculate = function (indicator) {
        $scope.Calculate(indicator);
    };

    $scope.isEPACSDependent = function (indicator) {
        var indicatorId = parseInt(indicator.KPIIndicatorsId);
        if (indicatorId !== 85 && indicatorId !== 86 && indicatorId !== 87) {
            return false;
        }

        let selectedInputType = "";
        let epacs = null;
        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {

                if (parseInt(item.KPIIndicatorsId) === 84) {
                    epacs = item;
                }

            });
        });

        if (!epacs) {
            return false;
        }

        angular.forEach(epacs.Options || [], function (option) {
            if (String(option.KPI_KEYId) === String(epacs.SelectedOption)) {
                selectedInputType = option.InputType;
            }
        });

        if (selectedInputType && selectedInputType === "No") {
            return true;
        }
        return false;
    };
    $scope.setEPACSDependent = function () {
        let epacs = null;
        let selectedInputType = "";
        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {
                if (parseInt(item.KPIIndicatorsId) === 84) {
                    epacs = item;
                }
            });
        });

        if (!epacs) {
            return;
        }
        angular.forEach(epacs.Options || [], function (option) {
            if (String(option.KPI_KEYId) === String(epacs.SelectedOption)) {
                selectedInputType = option.InputType;
            }
        });

        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (indicator) {
                let indicatorId = parseInt(indicator.KPIIndicatorsId);
                if (indicatorId === 85 || indicatorId === 86 || indicatorId === 87) {
                    if (selectedInputType === "No" || selectedInputType == "N/A") {
                        angular.forEach(indicator.Options || [], function (option) {
                            if (option.InputType === "No" || option.InputType === "N/A") {
                                indicator.SelectedOption = option.KPI_KEYId;
                                indicator.MarksObtained = option.Marks;
                            }
                        });
                    }
                }
            });
        });
    };
    $scope.isYesOption = function (indicator) {
        if (!indicator || !indicator.Options || indicator.Options.length === 0 || !indicator.SelectedOption) {
            return false;
        }

        for (var i = 0; i < indicator.Options.length; i++) {
            if (indicator.Options[i].KPI_KEYId == indicator.SelectedOption) {
                indicator.Options[i].InputType === "Yes";
                return true;
            }
            else { return false; }
        }
        return false;
    };

    $scope.isAgricultureColumnVisible = function () {

        if (!$scope.KPIAreas || $scope.KPIAreas.length === 0) {
            return false;
        }

        for (var i = 0; i < $scope.KPIAreas.length; i++) {

            var area = $scope.KPIAreas[i];

            if (area.KPIAreaId == 34 &&
                area.Indicators &&
                area.Indicators.length > 0 &&
                $scope.isYesOption(area.Indicators[0])) {

                return true;
            }
        }

        return false;
    };
    $scope.isMembershipCalculate = function (item) {

        let previous = parseFloat(item.MembershipPrevious) || 0;
        let current = parseFloat(item.MembershipCurrent) || 0;
        let weightedMarks = parseFloat(item.WeightedMarks) || 0;

        if (previous <= 0) {
            item.MarksObtained = "0.00";
            return "0.00";
        }

        let percentage = ((current - previous) / previous) * 100;

        let result = 0;

        if (percentage >= 5) {
            result = weightedMarks;
        }
        else {
            result = (percentage / 5) * weightedMarks;

            if (result < 0) {
                result = 0;
            }
        }

        if (result > weightedMarks) {
            result = weightedMarks;
        }

        item.MarksObtained = result.toFixed(2);

        return item.MarksObtained;
    };
    $scope.isShareCapitalCalculate = function (item) {

        let previous = parseFloat(item.ShareCapitalPrevious) || 0;
        let current = parseFloat(item.ShareCapitalCurrent) || 0;
        let weightedMarks = parseFloat(item.WeightedMarks) || 0;

        if (previous <= 0) {
            item.MarksObtained = "0.00";
            return "0.00";
        }

        let percentage = ((current - previous) / previous) * 100;

        let result = 0;

        if (percentage >= 5) {
            result = weightedMarks;
        }
        else {
            result = (percentage / 5) * weightedMarks;

            if (result < 0) {
                result = 0;
            }
        }

        if (result > weightedMarks) {
            result = weightedMarks;
        }

        item.MarksObtained = result.toFixed(2);

        return item.MarksObtained;
    };

    $scope.OnPageLoad();

});
KPIForPacsMod.directive('maxValue', function () {
    return {
        restrict: 'A', require: 'ngModel', link: function (scope, element, attrs, ngModelCtrl) {
            var maxNo = parseFloat(attrs.maxValue);
            element.on('input', function () {
                var disbursedAmt = element.val();
                if (disbursedAmt === '') {
                    return;
                }

                var loanAmt = parseFloat(disbursedAmt);
                if (loanAmt > maxNo) {
                    element.val(maxNo);
                    scope.$apply(function () {
                        ngModelCtrl.$setViewValue(maxNo);
                    });
                }
            });

            scope.$on('$destroy', function () {
                element.off('input');
            });
        }
    };
});

KPIForPacsMod.directive('numberOnly', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            element.on('input', function () {
                var value = element.val();

                var hasDecimal = value.indexOf('.') !== -1;

                if (hasDecimal) {
                    value = value.split('.')[0];
                    element.val(value);
                    ngModelCtrl.$setViewValue(value);
                    ngModelCtrl.$render();
                }

                scope.$applyAsync(function () {
                    scope.decimalError = hasDecimal;
                });
            });

            scope.$on('$destroy', function () {
                element.off('input');
            });
        }
    };
});

