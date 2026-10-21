var CooperativeSocietyFormModule = angular.module("ApplicationStatusModule", ['angularUtils.directives.dirPagination']);

CooperativeSocietyFormModule.controller("CooperativeSocietyFormCont", function ($rootScope, $scope, $http) {

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
        $scope.KPIforPacs();
        $scope.LoanBankList();
        $scope.GetSocietyByPacsSSO();
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

    $scope.CalculateMembershipIncrease = function () {

        var lastYear = parseFloat($scope.FormData.MembershipIncreaseLastYear) || 0;
        var currentMonth = parseFloat($scope.FormData.MembershipIncreaseCurrentMonth) || 0;
        var loaneeMember = parseFloat($scope.FormData.MembershipLoaneeMember) || 0;

        var total = lastYear + currentMonth;

        $scope.FormData.TotalMembershipIncrease = total;

        if (total <= 0) {
            $scope.FormData.MembershipIncrease = "";
            return;
        }

        $scope.FormData.MembershipLoaneeMemberPercentage = percentage;
      

        var selectedItem = null;

        if (percentage <= 25) {
            selectedItem = $scope.loanee.find(function (x) {
                return x.inputtype == "0-25%";
            });
        }
        else if (percentage <= 50) {
            selectedItem = $scope.loanee.find(function (x) {
                return x.inputtype == "Above 25-50%";
            });
        }
        else {
            selectedItem = $scope.loanee.find(function (x) {
                return x.inputtype == "Above 50%";
            });
        }

        if (selectedItem) {
            $scope.FormData.loaneemember = selectedItem.KPI_MarksId;
        }
    };


    $scope.CalculateProfitLoss = function () {

        var type = $scope.FormData.ProfitLastYear;
        var amount = parseFloat($scope.FormData.ProfitLossAmount) || 0;

        var selectedItem = null;

        if (type === "Loss") {

            selectedItem = $scope.profit.find(function (x) {
                return x.inputtype.trim() === "Loss";
            });

        }
        else if (type === "Profit" ) {

            if (amount <= 100000) {
                selectedItem = $scope.profit.find(function (x) {
                    return x.inputtype.trim() === "0-1 lakh";
                });
            }
            else if (amount <= 250000) {
                selectedItem = $scope.profit.find(function (x) {
                    return x.inputtype.trim() === "Above 1-2.5 lakh";
                });
            }
            else {
                selectedItem = $scope.profit.find(function (x) {
                    return x.inputtype.trim() === "Above 2.5 lakh";
                });
            }
        }

        if (selectedItem) {
            $scope.FormData.profit = selectedItem.KPI_MarksId;
        } else {
            $scope.FormData.profit = "0";
        }
    };

    $scope.FormData = {};

    $scope.SaveFormData = function () {
        // basic guard so you don't post a totally empty object
        $scope.FormData.CCBID = $scope.GopalCreditCardModel.LoanBankID;
        if (!$scope.FormData || Object.keys($scope.FormData).length === 0) {
            alert("Please fill at least one field before saving.");
            return;
        }

        // check every dropdown actually on the form, not just what's in FormData
        var hasEmpty = false;
        $("select").each(function () {
            var val = $(this).val();
            if (val === null || val === undefined || val === "" || val === "--- Select ---") {
                hasEmpty = true;
            }
        });

        if (hasEmpty) {
            alert("Please fill all fields before saving.");
            return;
        }

        $(".blockUI").fadeIn("slow");
        $http({
            method: 'POST',
            url: SiteUrl + '/Society/SaveKPIForPacs',
            data: $scope.FormData
        }).success(function (data, status) {
            $(".blockUI").fadeOut("slow");
            if (data && data.DataList) {
                alert("Saved successfully");
                location.reload();
            } else {
                alert("Error while saving data");
            }
        }).error(function (data, status) {
            $(".blockUI").fadeOut("slow");
            alert("Error while saving data. Please try again.");
        });
    };


    $scope.OnPageLoad();

});