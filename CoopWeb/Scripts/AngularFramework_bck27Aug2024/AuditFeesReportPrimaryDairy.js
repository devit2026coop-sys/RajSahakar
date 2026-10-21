
var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AuditProgressReportBySociety", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("AuditProgressReportBySocietycnt", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';


    function FileDataWithRow() {
        this.UserId = '';
    }
    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.BhandarList = data;
                //$scope.BhandarForm.BhandarId = '5';
            }).
            error(function (data, status) {
                $scope.BhandarList = [];
            });
    };

    //GET Financial Year
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.Liqudation.FYearId = '';
                $scope.FinancialYearList = data;
                //$("#ddlFYearId").val($scope.FinancialYearList[0].FIN_YEAR_END.toString());
                //$scope.Liqudation.FYearId = $scope.FinancialYearList[0].FIN_YEAR_END;
               
                $scope.GridData2();
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };

    //GET MONTH
    $scope.GetMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
                alert('');
            });
    };

    $scope.GetYearTargetData = function () {
        $http.post(SiteUrl + "/EntryForms/GetYearTargetData", $scope.BhandarForm).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.BhandarForm.ControlledBusinessTarget = data[0].ControlledBusinessTarget;
                    $scope.BhandarForm.UnControlledBusinessTarget = data[0].UnControlledBusinessTarget;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }
    $scope.GridData2 = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DataList = [];
       // alert($scope.Liqudation.FYearId + " category Id:" + $scope.Liqudation.CategoryId + "Month " + $("#txtFromMonth").val());
     //  if ($scope.Liqudation.FYearId==0)
       //  $scope.Liqudation.FYearId = '';

     //   debugger;
       
        //$http.post(SiteUrl + "/MIS/AuditFeesReportPrimaryDairy?year=" + $scope.Liqudation.FYearId + "&date=" + $("#txtFromMonth").val() + "&CategoryId=" + $scope.Liqudation.CategoryId).
        $http.post(SiteUrl + "/MIS/AuditFeesReportPrimaryDairy?year=" + '' + "&date=" + $("#txtFromMonth").val() + "&CategoryId=" + $scope.Liqudation.CategoryId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.length > 0) {
                    $scope.DataList = data;
                    $scope.DataList.forEach(function (x) {
                        x.TotalDepAuditor = parseInt(x.DepAuditor) + parseInt(x.CAAuditor);
                        x.TotalAuditComplete = parseInt(x.DepAuditComplete) + parseInt(x.CAAuditComplete);
                        x.TotalAuditNotComplete = parseInt(x.DepAuditNotComplete) + parseInt(x.CAAuditNotComplete);
                    });
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.SaveLoan = function () {

        $scope.Loan.LoanDataId = $scope.dataid;

        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.LoanFormValidation()) {
            $http.post(SiteUrl + "/MIS/SaveLoanFormsData", $scope.Loan).
                success(function (data, status) {
                    // alert("DataValue "+data);

                    var message = '';
                    if (data > 0) {
                        message = _language == 'en-us' ? 'Data Save Successfully\n' : 'डेटा सहेजा गया है\n';
                    }
                    else if (data == -1) {
                        message = _language == 'en-us' ? 'Data Update successfully\n' : 'डेटा सफलतापूर्वक अपडेट किया गया है\n';
                    }
                    else if (data == -2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                    else if (data == -3) {
                        message = _language == 'en-us' ? 'Data Delete Successfully\n' : 'डेटा सफलतापूर्वक हटाएं\n';

                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    }
                    alert(message);
                    window.location.reload();

                    if (data.Status) {
                        $scope.ClearLoanForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Loan Data insert");
                });
        }

    };
    $scope.pageChangeHandler = function (num) {

    };
    //$scope.update = function (item) {
    //    //alert(item.LiqudationDataId);
    //    $scope.showtringdiv = true;
    //    $scope.Loan.IsActive = item.IsActive = (item.IsActive == 1 ? true : false);
    //    $scope.dataid = item.LiqudationDataId;
    //    $scope.Loan.NameUnit = item.NameUnit;
    //    $scope.Loan.DetailsOfAcceptatnce = item.DetailsOfAcceptatnce;
    //    $scope.Loan.AcceptedAmount = parseInt(item.AcceptedAmount);
    //    $scope.Loan.PrincipalAmountOfRecovery = parseInt(item.PrincipalAmountOfRecovery);
    //    $scope.Loan.InterestAmountOfRecovery = parseInt(item.InterestAmountOfRecovery);
    //    $scope.Loan.RecoveryBalance = parseInt(item.RecoveryBalance);
    //    $scope.Loan.FdAmount = parseInt(item.FdAmount);
    //    $scope.Loan.FundSavingAccount = parseInt(item.FundSavingAccount);


    //   };




    $scope.ClearLoanForm = function () {
        $scope.dataid = '';
        $scope.Loan.AprStamt = 0;
        $scope.Loan.AprMtamt = 0;
        $scope.Loan.Aprltamt = 0;
        $scope.Loan.RecStamt = 0;
        $scope.Loan.RecMtamt = 0;
        $scope.Loan.Recltamt = 0;



    };

    // Update Template  

    //$scope.SetStatus = function (ObjLoan) {
    //  //  alert("Status " + ObjObjLiqudation.IsActive);
    //    $scope.Loan = ObjLoan;
    //    $scope.Loan.IsActive = ObjLoan.IsActive == 1 ? 0 : 1;
    //  //  alert("Status change" + ObjObjLiqudation.IsActive);
    //    $scope.dataid = ObjLoan.LoanDataId;
    //    $scope.SaveLoan();
    //};


    $scope.SetStatusTrash = function (ObjObjLiqudation) {
        // alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Liqudation = ObjObjLiqudation;
        // $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;
        $scope.Liqudation.IsActive = 2;
        //alert("Status change" + ObjObjLiqudation.IsActive);
        // $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.SaveLiquidation();
    };



    // on page load
    $scope.OnPageLoad = function () {
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
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
        }
        //$scope.GetDistrictWithoutDivisionList();
        $scope.GetFinancialYearList();
        //$scope.GetMonthList();

    };
    // Language Selector at page load



    $scope.ShowLoanAdvancementContent = function () {
        $scope.showtringdiv = true;
        $scope.ClearLiquidationForm();
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }


    //validateAddForm 
    $scope.LoanFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Loan.AprStamt == undefined || $scope.Loan.AprStamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Short Term Amount in Loan Advancement' : '\n <br /> कृपया ऋण अग्रिम में अल्पावधि राशि दर्ज करें |';
        }
        if ($scope.Loan.AprMtamt == undefined || $scope.Loan.AprMtamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Middle Term Amount in Loan Advancement' : '\n <br /> कृपया ऋण अग्रिम में मध्यावधि राशि दर्ज करें |';
        }
        if ($scope.Loan.Aprltamt == undefined || $scope.Loan.Aprltamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Long Term Amount in Loan Advancement' : '\n <br /> कृपया ऋण अग्रिम में दीर्घकालीन राशि दर्ज करें |';
        }

        if ($scope.Loan.RecStamt == undefined || $scope.Loan.RecStamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Short Term Amount in Loan Advancement' : '\n <br /> कृपया वसूले गए ऋण में v राशि दर्ज करें |';
        }
        if ($scope.Loan.RecMtamt == undefined || $scope.Loan.RecMtamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Middle Term Amount in Loan Advancement' : '\n <br /> कृपया वसूले गए ऋण में मध्यावधि राशि दर्ज करें |';
        }
        if ($scope.Loan.Recltamt == undefined || $scope.Loan.Recltamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Long Term Amount in Loan Advancement' : '\n <br /> कृपया वसूले गए ऋण में दीर्घकालीन राशि दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert("Loan Form", message);
            return false;
        }
        return true;

    }
    //End
    $scope.SelectedDate = "";
    $scope.AutoselectMonth = function () {
        var DT = new Date();
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getDate() + '-0' + (DT.getMonth() + 1) + '-' + DT.getFullYear();
        }
        else {
            date = DT.getDate() + '-' + (DT.getMonth() + 1) + '-' + DT.getFullYear();
        }
        $("#txtFromMonth").val(date);
        $scope.SelectedDate = date
    };
    $scope.SocietyCategory = [];
    $scope.GetSocietyCategory = function () {
        $http.post(SiteUrl + "/mis/GetSocietyCategory").
            success(function (data, status) {
               
                $scope.SocietyCategory = data;
                
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }
    $scope.GetSocietyCategory();
    $scope.AutoselectMonth();

    $scope.OnPageLoad();

    $scope.DivtringGrid = true;
});

