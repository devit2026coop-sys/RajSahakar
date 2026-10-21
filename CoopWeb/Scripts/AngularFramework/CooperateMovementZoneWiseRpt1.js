var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AppEntryCooperateMovementZoneWiseRpt1", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("CooperateMovementZoneWiseRpt1cnt", function ($scope, $http) {
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
                $scope.FinancialYearList = data;
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
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/MIS/GetCoopMovmentZoneWiseRptData1?year=" + $("#ddlstate1").val()).
            success(function (data, status) {
                if (data.length > 0) {
                    $(".blockUI").fadeOut("slow");
                    $scope.GetMovementZoneWiseMPRRpt = data;
                }
                else {
                    $(".blockUI").fadeOut("slow");
                    alert(" No data Found ");
                    $scope.GetMovementZoneWiseMPRRpt = 0;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.SaveLiquidation = function () {
     
        $scope.Liqudation.LiqudationDataId = $scope.dataid;

    
        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.LiqudationFormValidation()) {
            $http.post(SiteUrl + "/Society/SaveLiquidationFormsData", $scope.Liqudation ).
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
                        $scope.ClearLiquidationForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Liquidation Data insert");
                });
        }

    };
    $scope.pageChangeHandler = function (num) {

    };
    $scope.update = function (item) {
        //alert(item.LiqudationDataId);
        $scope.showtringdiv = true;
        $scope.Liqudation.IsActive = item.IsActive = (item.IsActive == 1 ? true : false);
        $scope.dataid = item.LiqudationDataId;
        $scope.Liqudation.NameUnit = item.NameUnit;
        $scope.Liqudation.DetailsOfAcceptatnce = item.DetailsOfAcceptatnce;
        $scope.Liqudation.AcceptedAmount = parseInt(item.AcceptedAmount);
        $scope.Liqudation.PrincipalAmountOfRecovery = parseInt(item.PrincipalAmountOfRecovery);
        $scope.Liqudation.InterestAmountOfRecovery = parseInt(item.InterestAmountOfRecovery);
        $scope.Liqudation.RecoveryBalance = parseInt(item.RecoveryBalance);
        $scope.Liqudation.FdAmount = parseInt(item.FdAmount);
        $scope.Liqudation.FundSavingAccount = parseInt(item.FundSavingAccount);
                

       };

    $scope.ClearLiquidationForm = function () {
        $scope.dataid ='';
        $scope.Liqudation.NameUnit = '';
        $scope.Liqudation.NameUnit = '';
        $scope.Liqudation.DetailsOfAcceptatnce = '';
        $scope.Liqudation.AcceptedAmount = 0;
        $scope.Liqudation.PrincipalAmountOfRecovery = 0;
        $scope.Liqudation.InterestAmountOfRecovery = 0;
        $scope.Liqudation.RecoveryBalance = 0;
        $scope.Liqudation.FdAmount = 0;
        $scope.Liqudation.FundSavingAccount = 0;
        

        
    };
    
    // Update Template  

    $scope.SetStatus = function (ObjObjLiqudation) {
      //  alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Liqudation = ObjObjLiqudation;
        $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;
      //  alert("Status change" + ObjObjLiqudation.IsActive);
        $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.SaveLiquidation();
    };


    $scope.SetStatusTrash = function (ObjObjLiqudation) {
       // alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Liqudation = ObjObjLiqudation;
       // $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;
        $scope.Liqudation.IsActive =2;
        //alert("Status change" + ObjObjLiqudation.IsActive);
       // $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.SaveLiquidation();
    };



    // on page load
    $scope.OnPageLoad = function () {
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
        }
        //$scope.GetDistrictWithoutDivisionList();
        //$scope.GetFinancialYearList();
        //$scope.GetMonthList();
      
    };
    // Language Selector at page load



    $scope.ShowLiqudationContent = function () {
        $scope.showtringdiv = true;
        $scope.ClearLiquidationForm();
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }

    //validateAddForm 
    $scope.LiqudationFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Liqudation.NameUnit == undefined || $scope.Liqudation.NameUnit == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Name Unit' : '\n <br /> कृपया नाम इकाई का चयन करे |';
        }
        if ($scope.Liqudation.DetailsOfAcceptatnce == undefined || $scope.Liqudation.DetailsOfAcceptatnce == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Details Of Acceptatnce' : '\n <br /> कृपया स्वीकृति का विवरण का चयन करे |';
        }
        if ($scope.Liqudation.AcceptedAmount == undefined || $scope.Liqudation.AcceptedAmount == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter AcceptedAmount' : '\n <br /> कृपया स्वीकृत राशि दर्ज करें |';
        }
        if ($scope.Liqudation.PrincipalAmountOfRecovery == undefined || $scope.Liqudation.PrincipalAmountOfRecovery == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Principal Amount Of Recovery' : '\n <br /> कृपया मूल वसूली की गई राशि दर्ज करें |';
        }

        if ($scope.Liqudation.InterestAmountOfRecovery == undefined || $scope.Liqudation.InterestAmountOfRecovery == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Interest Amount Of Recovery' : '\n <br /> कृपया ब्याज वसूली की गई राशि दर्ज करें |';
        }

        if ($scope.Liqudation.RecoveryBalance == undefined || $scope.Liqudation.RecoveryBalance == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Recovery Balance ' : '\n <br /> कृपया वसूली  से  शेष राशि दर्ज करें |';
        }
        if ($scope.Liqudation.FdAmount == undefined || $scope.Liqudation.FdAmount == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fd Amount' : '\n <br /> कृपया एफ.डी फंड में कुल राशि दर्ज करें |';
        }
        if ($scope.Liqudation.FundSavingAccount == undefined || $scope.Liqudation.FundSavingAccount == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fund Saving Account' : '\n <br /> कृपया बचत खाता फंड में कुल राशि दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert("Liqquidation Form", message);
            return false;
        }
        return true;

    }
    //End
    $scope.GetFinancialYearList();
    $scope.OnPageLoad();
    $scope.GridData2();
    $scope.DivtringGrid = true;
});

