var CooperativeSocietyFormModule = angular.module("ApplicationBhandarKPIModule", ['angularUtils.directives.dirPagination']);

CooperativeSocietyFormModule.controller("CooperativeBhandarKPIFormCont", function ($rootScope, $scope, $http) {

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

        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });


        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                //  $scope.ActList = data;
                $.each(data, function () {
                    $("#ddlAct").append($("<option></option>").val(this.ActId).html(this.ActNameEn));
                });
                $('#ddlAct').multiselect({
                    includeSelectAllOption: true
                });
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                // $scope.showLoader = false;
            });



        // load form data from database once page/translations are ready
        $scope.KPIforPacs();
        //$scope.LoanBankList();
        $scope.GetSocietyByPacsSSO();
        $scope.GetSAKPIRAODetails();
        //$scope.GetUserDetailsBySSO();
        $scope.SetPreviousMonth();
        $scope.DepartmentList();
        $scope.UnitList();
        $scope.DistrictList();
        $scope.GetSocietyTypeList();
        $scope.PostByDesignation();
        $scope.GetAllSubUnitList();
    }

    $scope.GetSocietyByPacsSSO = function (ActId, SocietyTypeId, ClassId) {
        debugger;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/PacsUserLoginElectionDetails", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data.filter(x => x.IsObjection == false);
            console.log('$scope.TableData', $scope.TableData);
            $scope.SocietyDataList = $scope.TableData;
            console.log('$scope.SocietyDataList', $scope.SocietyDataList);
            $scope.SocietyList = $scope.SocietyDataList;
            console.log("$scope.SocietyList", $scope.SocietyList);
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };

    $scope.GetSAKPIRAODetails = function () {
        debugger;
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetBhandarKPIDetails")
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
                        Target: item.KPIIndicatorsTarget || "",
                        Achievement: item.KPIIndicatorsAchievement || "",
                        Progress: item.ProgressPercentage || "",
                        MarksObtained: item.MarksObtained || ""
                    });
                });

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                $scope.KPIAreas = [];
            });

    };

    //old
    //$scope.Calculate = function (item) {
    //    debugger;

    //    var target = parseFloat(item.Target) || 0;
    //    var achievement = parseFloat(item.Achievement) || 0;
    //    var weightedMarks = parseFloat(item.WeightedMarks) || 0;

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

    //                    if (ind.KPIIndicatorsId == 6 || ind.KPIIndicatorsId == 7 ||  ind.KPIIndicatorsId == 8 || ind.KPIIndicatorsId == 9 ) {
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
    //            item.Progress = ((achievement / target) * 100).toFixed(2);
    //            item.MarksObtained = ((parseFloat(item.Progress) * weightedMarks) / 100).toFixed(2);
    //        }

    //        return;
    //    }

    //    if (target == 0) {
    //        item.Progress = "100.00";
    //        item.MarksObtained = weightedMarks.toFixed(2);
    //        return;
    //    }

    //    item.Progress = ((achievement / target) * 100).toFixed(2);
    //    item.MarksObtained = ((parseFloat(item.Progress) * weightedMarks) / 100).toFixed(2);
    //};

    //new
    $scope.Calculate = function (item) {
        debugger;

        var target = parseFloat(item.Target) || 0;
        var achievement = parseFloat(item.Achievement) || 0;
        var weightedMarks = parseFloat(item.WeightedMarks) || 0;

        if (target == 0) {
            item.Progress = "0.00";
            item.MarksObtained = weightedMarks.toFixed(2);
            return;
        }

        var progress = (achievement / target) * 100;

        // Maximum Progress = 100%
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

    $scope.SaveKPIResult = function () {
        debugger;
        var SaveData = [];
        var isValid = true;

        angular.forEach($scope.KPIAreas, function (area) {
            angular.forEach(area.Indicators, function (item) {             
                SaveData.push({
                    FY: $scope.FY,
                    Month: $scope.PreviousMonthId,
                    KPIFor: "UpbhoktaBhandars",
                    OfficeId: $scope.UserRegistration.OfficeId,
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

    $scope.GetAllSubUnitList = function () {
        $scope.ShowGrid = false;
        $http.get(SiteUrl + "/Login/UserSelfReqList").
            success(function (data, status) {
                if (data == "-1") {
                    window.location.href = "https://sso.rajasthan.gov.in";
                }
                $scope.UserReqData = data;
                if (data.length > 0) {
                    $scope.ShowGrid = true;
                }
            }).
            error(function (data1, status) {

            });
    };

    $scope.DepartmentList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Department = data;
                if (data.length > 0) {
                    var DeoptCode = data.filter(function (x) { return x.DEPTNAMEENGLISH == 'COOPERATIVE' })[0].ID;
                    if (DeoptCode != undefined && DeoptCode != '' && DeoptCode != null) {
                        $scope.UserRegistration.DEPTCODE = DeoptCode;
                        $scope.GetAllSubOrganisationList();
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else {
                        $scope.UserRegistration.DEPTCODE = '0';
                        $(".se-pre-con").fadeOut("slow");
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching department list");
                $scope.Department = [];
            });
    };

    $scope.GetAllSubOrganisationList = function () {
        $scope.Level = [];
        $http.get(SiteUrl + "/Master/OrganisationsList/?DepartmentId=" + $scope.UserRegistration.DEPTCODE).
            success(function (data, status) {
                $scope.OrganisationData = data;
                if (data.length > 0) {
                    if (data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' }).length > 0) {
                        var OrgCode = data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' })[0].OrganisationId;
                        if (OrgCode != undefined && OrgCode != '' && OrgCode != null) {
                            $scope.UserRegistration.OrganisationId = OrgCode;
                            $scope.LevelList();
                        }
                        else {
                            $scope.UserRegistration.OrganisationId = '0';
                        }
                    }
                }
            }).
            error(function (data1, status) {

            });
    };

    $scope.LoanBankList = function () {

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + ($scope.GopalCreditCardModel.LoanDistrictId ?? 0)).
            success(function (data, status) {

                $scope.BankMasterList = data;
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
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

    //$scope.GetUserDetailsBySSO = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    $http.get(SiteUrl + "/Society/GetUserDetailsBySSO")
    //        .success(function (data) {             
    //            $scope.UserDetails = data[0];
    //            $scope.OfficeId = data[0].OfficeId;
    //            $(".se-pre-con").fadeOut("slow");
    //        })
    //        .error(function () {
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
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
                console.log('$scope.OfficeListData', $scope.OfficeListData);
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

    $scope.OnPageLoad();

});