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

        $scope.SetCurrentFY();
        $scope.SetPreviousMonth();
        if ($scope.pacsDetail.GCCLoginId != null && $scope.pacsDetail.GCCLoginId > 0) {
            $scope.GetDetailsByPacs();
            $scope.IsFormDisabled = false;
        } else {
            $scope.IsFormDisabled = true;
            toastr.warning("You cannot fill this form.", "", {
                timeOut: 10000
            });
        }

        $scope.RoleId = parseInt($('#hdnRoleId').val()) || 0;

        $scope.ProfitFinancialYearList = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .then(function (response) {
                var data = response.data || [];
                var today = new Date();
                var year = today.getFullYear();
                var month = today.getMonth() + 1;

                var currentFYStartYear = month >= 4 ? year : year - 1;
                var fromYear = currentFYStartYear - 3;

                $scope.ProfitFinancialYearList = data.filter(function (x) {
                    var fyStart = Number(x.FIN_YEAR_START);
                    var fyEnd = Number(x.FIN_YEAR_END);

                    return fyStart >= fromYear && fyStart < currentFYStartYear && fyEnd === fyStart + 1 && Number(x.IS_ACTIVE) === 1 && Number(x.IS_DELETED) === 0;
                });
                $scope.ProfitFinancialYearList.sort(function (a, b) {
                    return Number(b.FIN_YEAR_START) - Number(a.FIN_YEAR_START);
                });
            })
            .catch(function (error) {
                console.error(error);
                $scope.ProfitFinancialYearList = [];
            });

        $scope.AGMFinancialYearList = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .then(function (response) {
                var data = response.data || [];
                var today = new Date();
                var year = today.getFullYear();
                var month = today.getMonth() + 1;

                var currentFYStartYear = month >= 4 ? year : year - 1;
                var fromYear = currentFYStartYear - 3;

                $scope.AGMFinancialYearList = data.filter(function (x) {
                    var fyStart = Number(x.FIN_YEAR_START);
                    var fyEnd = Number(x.FIN_YEAR_END);

                    return fyStart >= fromYear && fyStart < currentFYStartYear && fyEnd === fyStart + 1 && Number(x.IS_ACTIVE) === 1 && Number(x.IS_DELETED) === 0;
                });
                $scope.AGMFinancialYearList.sort(function (a, b) {
                    return Number(b.FIN_YEAR_START) - Number(a.FIN_YEAR_START);
                });
            })
            .catch(function (error) {
                console.error(error);
                $scope.AGMFinancialYearList = [];
            });
    }

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

    $scope.isYesOption83Option2 = function (indicator) {

        if (!indicator || !indicator.Options || indicator.Options.length === 0 || !indicator.selectOptionAnyOther1) {
            return false;
        }

        var selected = indicator.Options.find(function (option) {
            return option.KPI_KEYId == indicator.selectOptionAnyOther1;
        });

        return selected && selected.InputType == "Yes";
    };

    $scope.isYesOption83Option3 = function (indicator) {

        if (!indicator || !indicator.Options || indicator.Options.length === 0 || !indicator.selectOptionAnyOther2) {
            return false;
        }

        var selected = indicator.Options.find(function (option) {
            return String(option.KPI_KEYId) === String(indicator.selectOptionAnyOther2);
        });

        return selected && selected.InputType === "Yes";
    };

    $scope.GetSAKPISAODetails = function (financialYear) {
        $(".se-pre-con").fadeIn("slow");

        return $http.get(SiteUrl + "/KPI/GetPacsKPIDetails", {
            params: {
                financialYear: financialYear
            }
        }).success(function (data) {
            $scope.KPIData = data;
            $scope.KPIAreas = [];

            if ($scope.KPIData != null && $scope.KPIData.length > 0) {
                $scope.pacsDetail.PacsRegistrationNo = $scope.KPIData[0].PacsRegistrationNo && $scope.KPIData[0].PacsRegistrationNo.trim() ? $scope.KPIData[0].PacsRegistrationNo.trim() : "";
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
                        DynamicDate: null,
                        ProfitFinancialid: null,
                        AGMFinancialId: null,
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

            $scope.GetKPIResult();

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
                        indicator.DynamicDate = dbItem.DynamicDate ? new Date(dbItem.DynamicDate) : null;
                        indicator.AuditCompleteDate = dbItem.AuditCompleteDate ? new Date(dbItem.AuditCompleteDate) : null;
                        indicator.BODMeetingDate = dbItem.BODMeetingDate ? new Date(dbItem.BODMeetingDate) : null;
                        indicator.ProfitFinancialid = dbItem.ProfitFinancialid != null ? String(dbItem.ProfitFinancialid) : "";
                        if (dbItem.KPIIndicatorsId == 68) {
                            indicator.AGMFinancialId = dbItem.AGMFinancialId != null ? String(dbItem.AGMFinancialId) : "";
                        }

                        if (dbItem.KPIIndicatorsId == 59) {
                            indicator.DepositAmount = dbItem.DepositAmount != null ? Number(dbItem.DepositAmount) : null;
                        }

                        if (dbItem.KPIIndicatorsId == 63) {
                            indicator.KCCRecoveryAmount = dbItem.KCCRecoveryAmount != null ? Number(dbItem.KCCRecoveryAmount) : null;
                        }

                        if (dbItem.KPIIndicatorsId == 64) {
                            indicator.GCCRecoveryAmount = dbItem.GCCRecoveryAmount != null ? Number(dbItem.GCCRecoveryAmount) : null;
                        }
                        if (dbItem.KPIIndicatorsId == 65) {
                            indicator.LoanRecoveryAmount = dbItem.LoanRecoveryAmount != null ? Number(dbItem.LoanRecoveryAmount) : null;
                        }

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
                            indicator.MembershipDifference = Number(dbItem.MembershipDifference);
                        }

                        if (dbItem.KPIIndicatorsId == 58) {
                            indicator.ShareCapitalDifference = Number(dbItem.ShareCapitalDifference);
                        }

                        if (dbItem.KPIIndicatorsId == 57) {
                            indicator.ProfitLastYearAmount = Number(dbItem.ProfitLastYearAmount);
                            indicator.ProfitLossType = dbItem.ProfitLossType;
                        }

                        if (dbItem.KPIIndicatorsId == 62) {
                            indicator.OtherThanKccLoanDisAmount = Number(dbItem.OtherThanKccLoanDisAmount);
                        }

                        if (dbItem.KPIIndicatorsId == 204) {
                            indicator.NoGodown = dbItem.NoGodown != null ? Number(dbItem.NoGodown) : "";
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
                        if (dbItem.KPIIndicatorsId == 75 || dbItem.KPIIndicatorsId == 76 || dbItem.KPIIndicatorsId == 77 || dbItem.KPIIndicatorsId == 78 || dbItem.KPIIndicatorsId == 79 || dbItem.KPIIndicatorsId == 80 || dbItem.KPIIndicatorsId == 81 || dbItem.KPIIndicatorsId == 82 || dbItem.KPIIndicatorsId == 83) {
                            indicator.OtherActivitiesTurnOver = Number(dbItem.OtherActivitiesTurnOver);
                        }
                    }
                });
            });
            $scope.CalculateTotalLoaneeNumber();
            $scope.$applyAsync();

        }).error(function (err) {

            console.log(err);

        });

    };

    $scope.GetKPIResultold = function () {
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
                });
            });

            if (data && data.length > 0) {
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
            $scope.setAgricultureDependent();
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

    $scope.isNoGodownInput = function (indicator) {
        if (!indicator || !indicator.Options || indicator.Options.length === 0) {
            return false;
        }

        return indicator.Options[0].InputType === "NoGodown";
    };

    $scope.isLandAvailableYes = function () {

        var land = null;

        angular.forEach($scope.KPIAreas || [], function (area) {
            angular.forEach(area.Indicators || [], function (item) {
                if (parseInt(item.KPIIndicatorsId, 10) === 51) {
                    land = item;
                }
            });
        });

        if (!land || !land.SelectedOption) {
            return false;
        }

        var selected = (land.Options || []).find(function (option) {
            return String(option.KPI_KEYId) === String(land.SelectedOption);
        });

        return selected && String(selected.InputType).trim().toLowerCase() === "yes";
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

    $scope.DateFormat = function (date) {
        if (!date) {
            return null;
        }

        var d = new Date(date);

        if (isNaN(d.getTime())) {
            return null;
        }

        var year = d.getFullYear();
        var month = String(d.getMonth() + 1).padStart(2, '0');
        var day = String(d.getDate()).padStart(2, '0');

        return year + '-' + month + '-' + day;
    };

    $scope.SaveKPIResult = function () {

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
                    if (!item.SelectedOption && kpiId != 59 && kpiId != 63 && kpiId != 64 && kpiId != 65 && kpiId != 204 && kpiId != 62 && kpiId != 71 &&
                        kpiId != 72 && kpiId != 73 && kpiId != 74 && kpiId != 57 && kpiId != 52) {
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
                    OfficeId: $scope.pacsDetail.OfficeId,
                    //CCBId: $scope.CCBId,
                    CCBId: $scope.pacsDetail.CCBId,
                    PacsBranchId: $scope.pacsDetail.BranchId,
                    PacsId: $scope.pacsDetail.PACS_ID,
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
                    selectOptionAnyOther2: $scope.isYesOption83Option3(item),
                    DynamicDate: parseInt(item.KPIIndicatorsId, 10) === 86 ? $scope.DateFormat(item.DynamicDate) : null,
                    ProfitFinancialid: parseInt(item.KPIIndicatorsId, 10) === 57 && item.ProfitFinancialid ? parseInt(item.ProfitFinancialid, 10) : null,
                    AGMFinancialId: parseInt(item.KPIIndicatorsId, 10) === 68 && item.AGMFinancialId ? parseInt(item.AGMFinancialId, 10) : null,
                    DepositAmount: parseInt(item.KPIIndicatorsId) === 59 ? item.DepositAmount : null,
                    KCCRecoveryAmount: parseInt(item.KPIIndicatorsId) === 63 ? item.KCCRecoveryAmount : null,
                    GCCRecoveryAmount: parseInt(item.KPIIndicatorsId) === 64 ? item.GCCRecoveryAmount : null,
                    LoanRecoveryAmount: parseInt(item.KPIIndicatorsId) === 65 ? item.LoanRecoveryAmount : null,
                    NoGodown: parseInt(item.KPIIndicatorsId) === 204 ? item.NoGodown : null,
                    ProfitLastYearAmount: parseInt(item.KPIIndicatorsId) === 57 ? item.ProfitLastYearAmount : null,
                    OtherThanKccLoanDisAmount: parseInt(item.KPIIndicatorsId) === 62 ? item.OtherThanKccLoanDisAmount : null,
                    ProfitLossType: item.ProfitLossType ?? "",
                    IncrementPercentage: item.IncrementPercentage || "0.00",
                    AuditCompleteDate: parseInt(item.KPIIndicatorsId, 10) === 66 ? $scope.DateFormat(item.AuditCompleteDate) : null,
                    BODMeetingDate: parseInt(item.KPIIndicatorsId, 10) === 69 ? $scope.DateFormat(item.BODMeetingDate) : null,
                    OtherActivitiesTurnOver: !item.OtherActivitiesTurnOver ? "0.00" : item.OtherActivitiesTurnOver,
                    MembershipDifference: !item.MembershipDifference ? "0.00" : item.MembershipDifference,
                    ShareCapitalDifference: !item.ShareCapitalDifference ? "0.00" : item.ShareCapitalDifference
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
            angular.forEach(area.Indicators, function (item) {
                $scope.SaveData.push({
                    Month: $scope.PreviousMonthId,
                    KPIFor: "PACS",
                    //OfficeId: $scope.GccId ?? 0,
                    OfficeId: $scope.pacsDetail.OfficeId,
                    PacsId: $scope.pacsDetail.PACS_ID,
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
                    selectOptionAnyOther2: !item.selectOptionAnyOther2 ? "0 " : item.selectOptionAnyOther2,
                    DynamicDate: parseInt(item.KPIIndicatorsId, 10) === 86 ? $scope.DateFormat(item.DynamicDate) : null,
                    ProfitFinancialid: parseInt(item.KPIIndicatorsId, 10) === 57 && item.ProfitFinancialid ? parseInt(item.ProfitFinancialid, 10) : null,
                    AGMFinancialId: parseInt(item.KPIIndicatorsId, 10) === 68 && item.AGMFinancialId ? parseInt(item.AGMFinancialId, 10) : null,
                    DepositAmount: parseInt(item.KPIIndicatorsId) === 59 ? item.DepositAmount : null,
                    KCCRecoveryAmount: parseInt(item.KPIIndicatorsId) === 63 ? item.KCCRecoveryAmount : null,
                    GCCRecoveryAmount: parseInt(item.KPIIndicatorsId) === 64 ? item.GCCRecoveryAmount : null,
                    LoanRecoveryAmount: parseInt(item.KPIIndicatorsId) === 65 ? item.LoanRecoveryAmount : null,
                    NoGodown: parseInt(item.KPIIndicatorsId) === 204 ? item.NoGodown : null,
                    ProfitLastYearAmount: parseInt(item.KPIIndicatorsId) === 57 ? item.ProfitLastYearAmount : null,
                    OtherThanKccLoanDisAmount: parseInt(item.KPIIndicatorsId) === 62 ? item.OtherThanKccLoanDisAmount : null,
                    ProfitLossType: item.ProfitLossType ?? "",
                    IncrementPercentage: item.IncrementPercentage || "0.00",
                    AuditCompleteDate: parseInt(item.KPIIndicatorsId, 10) === 66 ? $scope.DateFormat(item.AuditCompleteDate) : null,
                    BODMeetingDate: parseInt(item.KPIIndicatorsId, 10) === 69 ? $scope.DateFormat(item.BODMeetingDate) : null,
                    OtherActivitiesTurnOver: !item.OtherActivitiesTurnOver ? "0.00" : item.OtherActivitiesTurnOver,
                    MembershipDifference: !item.MembershipDifference ? "0.00" : item.MembershipDifference,
                    ShareCapitalDifference: !item.ShareCapitalDifference ? "0.00" : item.ShareCapitalDifference
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

    $scope.CalculateDepositWeight = function (item) {

        if (item.DepositAmount !== null && item.DepositAmount !== undefined && item.DepositAmount !== "") {

            var value = item.DepositAmount.toString();

            if (value.includes('.')) {
                var parts = value.split('.');

                if (parts[1].length > 2) {
                    item.DepositAmount = parseFloat(parseFloat(value).toFixed(2));
                }
            }
        }

        var amount = parseFloat(item.DepositAmount);

        if (item.DepositAmount === "" || item.DepositAmount === null || item.DepositAmount === undefined || isNaN(amount) || amount == 0) {
            item.MarksObtained = "0.00";
        }
        else if (amount <= 100) {
            item.MarksObtained = "1.00";
        }
        else if (amount <= 300) {
            item.MarksObtained = "2.00";
        }
        else {
            item.MarksObtained = "3.00";
        }
    };

    $scope.CalculateKCCRecoveryMarks = function (item) {

        var percentage = parseFloat(item.KCCRecoveryAmount);

        if (item.KCCRecoveryAmount === "" || item.KCCRecoveryAmount === null || item.KCCRecoveryAmount === undefined || isNaN(percentage) || percentage < 0 || percentage > 100 || item.KCCRecoveryAmount == 0) {
            item.MarksObtained = "0.00";
            return;
        }

        if (percentage <= 90) {
            item.MarksObtained = "0.00";
        }
        else if (percentage <= 95) {
            item.MarksObtained = "2.00";
        }
        else if (percentage < 100) {
            item.MarksObtained = "3.00";
        }
        else {
            item.MarksObtained = "4.00";
        }
    };

    $scope.CalculateGCCRecoveryMarks = function (item) {
        var percentage = parseFloat(item.GCCRecoveryAmount);

        if (item.GCCRecoveryAmount === "" || item.GCCRecoveryAmount === null || item.GCCRecoveryAmount === undefined || isNaN(percentage) || percentage < 0 || percentage > 100 || item.GCCRecoveryAmount == 0) {
            item.MarksObtained = "0.00";
        }
        else if (percentage <= 90) {
            item.MarksObtained = "0.00";
        }
        else if (percentage <= 95) {
            item.MarksObtained = "1.00";
        }
        else if (percentage < 100) {
            item.MarksObtained = "2.00";
        }
        else {
            item.MarksObtained = "3.00";
        }
    };

    $scope.CalculateLoanRecoveryMarks = function (item) {
        var percentage = parseFloat(item.LoanRecoveryAmount);

        if (item.LoanRecoveryAmount === "" || item.LoanRecoveryAmount === null || item.LoanRecoveryAmount === undefined || isNaN(percentage) || percentage < 0 || percentage > 100 || item.LoanRecoveryAmount == 0) {
            item.MarksObtained = "0.00";
        }
        else if (percentage <= 90) {
            item.MarksObtained = "0.00";
        }
        else if (percentage <= 95) {
            item.MarksObtained = "1.00";
        }
        else if (percentage < 100) {
            item.MarksObtained = "2.00";
        }
        else {
            item.MarksObtained = "3.00";
        }
    };

    $scope.SetCurrentFY = function () {
        $http.get(SiteUrl + "/Society/GetCurrentFinancialYear")
            .success(function (data) {
                $scope.FY = data[0].CurrentFY;
                console.log('$scope.FY', $scope.FY);
                $scope.GetSAKPISAODetails($scope.FY);
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
                    console.table($scope.pacsDetail);
                    $scope.GetDistricts();

                    if ($scope.pacsDetail.GCCLoginId != null && $scope.pacsDetail.GCCLoginId != "" && $scope.pacsDetail.GCCLoginId != 0) {
                        $scope.GetSAKPISAODetails();
                    }
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

            if (area.KPIAreaId == 34 && area.Indicators && area.Indicators.length > 0 && $scope.isYesOption(area.Indicators[0])) {
                return true;
            }
        }

        return false;
    };

    $scope.isOtherActivitiesColumnVisible = function () {

        var area = ($scope.KPIAreas || []).find(function (x) {
            return x.KPIAreaId == 35;
        });

        if (!area || !area.Indicators) {
            return false;
        }

        return area.Indicators.some(function (indicator) {
            return $scope.isYesOption(indicator);
        });
    };

    $scope.isAgricultureInputEnabled = function () {

        var fertilizer = null;

        angular.forEach($scope.KPIAreas || [], function (area) {

            angular.forEach(area.Indicators || [], function (item) {

                if (parseInt(item.KPIIndicatorsId) === 71) {
                    fertilizer = item;
                }

            });

        });

        if (!fertilizer || !fertilizer.SelectedOption) {
            return false;
        }

        var selected = (fertilizer.Options || []).find(function (option) {
            return String(option.KPI_KEYId) === String(fertilizer.SelectedOption);
        });

        return selected && String(selected.InputType).trim().toLowerCase() === "yes";
    };

    $scope.setAgricultureDependent = function () {

        var fertilizer = null;

        angular.forEach($scope.KPIAreas || [], function (area) {
            angular.forEach(area.Indicators || [], function (item) {
                if (parseInt(item.KPIIndicatorsId) === 71) {
                    fertilizer = item;
                }
            });
        });

        if (!fertilizer || !fertilizer.SelectedOption) {
            return;
        }

        var selected = (fertilizer.Options || []).find(function (option) {
            return String(option.KPI_KEYId) === String(fertilizer.SelectedOption);
        });

        if (!selected) return;

        var selectedType = String(selected.InputType).trim().toLowerCase();

        if (selectedType === "no") {

            angular.forEach($scope.KPIAreas || [], function (area) {

                angular.forEach(area.Indicators || [], function (item) {

                    var id = parseInt(item.KPIIndicatorsId);

                    if (id === 72 || id === 73 || id === 74) {

                        var noOption = (item.Options || []).find(function (option) {
                            return String(option.InputType).trim().toLowerCase() === "no";
                        });

                        if (noOption) {
                            item.SelectedOption = String(noOption.KPI_KEYId);
                            item.MarksObtained = noOption.Marks;
                        }
                    }

                });

            });
        }
    };

    $scope.isMembershipCalculate = function (item) {

        if (!item) return;

        var previous = parseFloat(item.MembershipPrevious);
        var current = parseFloat(item.MembershipCurrent);

        // Calculate Difference: Current - Previous
        if (item.MembershipPrevious !== "" && item.MembershipPrevious != null && item.MembershipCurrent !== "" && item.MembershipCurrent != null && !isNaN(previous) && !isNaN(current)) {
            item.MembershipDifference = current - previous;
        }
        else {
            item.MembershipDifference = "";
        }

        var weightedMarks = parseFloat(item.WeightedMarks) || 0;

        if (isNaN(previous) || previous <= 0 || isNaN(current)) {

            item.MarksObtained = "0.00";
        }
        else {

            var percentage = ((current - previous) / previous) * 100;
            var result = 0;

            if (percentage >= 5) {
                result = weightedMarks;
            }
            else {
                result = Math.max(0, (percentage / 5) * weightedMarks);
            }

            item.MarksObtained = Math.min(result, weightedMarks).toFixed(2);
        }

        $scope.CalculateTotalLoaneeNumber();

        return item.MarksObtained;
    };

    $scope.CalculateTotalLoaneeNumber = function () {

        var membership = null;
        var loanee = null;

        angular.forEach($scope.KPIAreas || [], function (area) {
            angular.forEach(area.Indicators || [], function (item) {

                if (parseInt(item.KPIIndicatorsId) === 55) {
                    membership = item;
                }

                if (parseInt(item.KPIIndicatorsId) === 56) {
                    loanee = item;
                }
            });
        });

        if (!membership || !loanee) return;

        var previous = parseFloat(membership.MembershipPrevious);
        var current = parseFloat(membership.MembershipCurrent);

        if (isNaN(previous) || previous <= 0 ||
            membership.MembershipCurrent === "" ||
            membership.MembershipCurrent == null) {

            loanee.IncrementPercentage = "0.00";
            loanee.MarksObtained = "0.00";
            loanee.SelectedOption = null;
            return;
        }

        var percentage = ((current - previous) / previous) * 100;

        loanee.IncrementPercentage = percentage.toFixed(2);

        var marks = 0;

        if (percentage > 50) {
            marks = 2;
        }
        else if (percentage > 25) {
            marks = 1;
        }

        loanee.MarksObtained = marks.toFixed(2);

        var selectedOption = (loanee.Options || []).find(function (option) {
            return parseFloat(option.Marks) === marks;
        });

        loanee.SelectedOption = selectedOption
            ? String(selectedOption.KPI_KEYId)
            : null;
    };

    $scope.isShareCapitalCalculate = function (item) {

        if (!item) return;

        var previous = parseFloat(item.ShareCapitalPrevious);
        var current = parseFloat(item.ShareCapitalCurrent);

        // Calculate Difference
        if (item.ShareCapitalPrevious !== "" && item.ShareCapitalPrevious != null && item.ShareCapitalCurrent !== "" && item.ShareCapitalCurrent != null && !isNaN(previous) && !isNaN(current)) {

            item.ShareCapitalDifference = current - previous;
        }
        else {
            item.ShareCapitalDifference = "";
        }

        // Existing Marks Calculation
        var weightedMarks = parseFloat(item.WeightedMarks) || 0;

        if (isNaN(previous) || previous <= 0 || isNaN(current)) {
            item.MarksObtained = "0.00";
            return "0.00";
        }

        var percentage = ((current - previous) / previous) * 100;

        var result = 0;

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

    $scope.isLandAvailableYes = function () {
        var land = null;

        angular.forEach($scope.KPIAreas || [], function (area) {
            angular.forEach(area.Indicators || [], function (item) {
                if (parseInt(item.KPIIndicatorsId, 10) === 51) {
                    land = item;
                }
            });
        });

        if (!land || !land.SelectedOption) {
            return false;
        }

        var selected = (land.Options || []).find(function (option) {
            return String(option.KPI_KEYId) === String(land.SelectedOption);
        });

        return selected && String(selected.InputType).trim().toLowerCase() === "yes";
    };

    $scope.getVisibleIndicatorCount = function (area) {

        if (!area || !area.Indicators) {
            return 0;
        }

        var count = 0;

        angular.forEach(area.Indicators, function (indicator) {
            // Hide Godown indicators when Land Available is No
            if (!$scope.isLandAvailableYes() && (parseInt(indicator.KPIIndicatorsId, 10) === 204 || $scope.isGodownIndicator(indicator))) {
                return;
            }

            count++;
        });

        return count;
    };

    // Identify Godown Available in MT indicator
    $scope.isGodownIndicator = function (indicator) {
        return String(indicator.KPIIndicators || "").trim().toLowerCase() === "godown available in mt";
    };

    // Clear Godown data when Land Available is No
    $scope.LandAvailabilityChanged = function () {

        if (!$scope.isLandAvailableYes()) {

            angular.forEach($scope.KPIAreas || [], function (area) {

                angular.forEach(area.Indicators || [], function (indicator) {

                    // No. of Godown
                    if (parseInt(indicator.KPIIndicatorsId, 10) === 204) {
                        indicator.NoGodown = null;
                    }

                    // Godown Available in MT
                    if ($scope.isGodownIndicator(indicator)) {
                        indicator.SelectedOption = null;
                        indicator.MarksObtained = "0.00";
                    }

                });
            });
        }
    };

    $scope.isGodownIndicator = function (indicator) {
        return String(indicator.KPIIndicators || "").trim().toLowerCase() === "godown available in mt";
    };

    $scope.isGodownSelected = function () {
        var godown = null;

        angular.forEach($scope.KPIAreas || [], function (area) {
            angular.forEach(area.Indicators || [], function (item) {
                if ($scope.isGodownIndicator(item)) {
                    godown = item;
                }
            });
        });

        return !!(godown && godown.SelectedOption);
    };

    $scope.LimitRecoveryPercentage = function (item, property) {
        var value = item[property];

        if (value === null || value === undefined || value === "") {
            item.MarksObtained = "0.00";
            return;
        }

        var percentage = parseFloat(value);

        if (isNaN(percentage) || percentage < 0) {
            item[property] = 0;
        }
        else if (percentage > 100) {
            item[property] = 100;
        }

        if (item.KPIIndicatorsId == 63) {
            $scope.CalculateKCCRecoveryMarks(item);
        }
        else if (item.KPIIndicatorsId == 64) {
            $scope.CalculateGCCRecoveryMarks(item);
        }
        else if (item.KPIIndicatorsId == 65) {
            $scope.CalculateLoanRecoveryMarks(item);
        }
    };

    $scope.CalculateProfitLastYearMarks = function (indicator) {

        if (!indicator) return;

        var amount = parseFloat(indicator.ProfitLastYearAmount) || 0;

        if (amount <= 0) {
            indicator.MarksObtained = "0.00";
        }
        else if (amount <= 100000) {
            indicator.MarksObtained = "1.00";
        }
        else if (amount <= 250000) {
            indicator.MarksObtained = "2.00";
        }
        else {
            indicator.MarksObtained = "3.00";
        }
    };

    $scope.CalculateOtherThanKccLoanMarks = function (indicator) {

        var amount = Number(indicator.OtherThanKccLoanDisAmount) || 0;

        if (amount <= 0) {
            indicator.MarksObtained = "0.00";
        }
        else if (amount <= 10) {
            indicator.MarksObtained = "1.00";
        }
        else {
            indicator.MarksObtained = "2.00";
        }
    };

    $scope.LimitNoOfGodown = function (indicator) {

        var value = Number(indicator.NoGodown);

        if (value > 9) {
            indicator.NoGodown = 9;
        }
        else if (value < 0) {
            indicator.NoGodown = 0;
        }
    };

    $scope.isAuditCompletedYes = function (indicator) {

        if (!indicator || !indicator.SelectedOption) {
            return false;
        }

        var selectedOption = (indicator.Options || []).find(function (option) {
            return String(option.KPI_KEYId) === String(indicator.SelectedOption);
        });

        return selectedOption && String(selectedOption.InputType).trim().toLowerCase() === "yes";
    };

    $scope.isBODMeetingDate = function (indicator) {

        if (!indicator || !indicator.SelectedOption) {
            return false;
        }

        var selectedOption = (indicator.Options || []).find(function (option) {
            return String(option.KPI_KEYId) === String(indicator.SelectedOption);
        });

        return selectedOption && String(selectedOption.InputType).trim().toLowerCase() === "yes";
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

