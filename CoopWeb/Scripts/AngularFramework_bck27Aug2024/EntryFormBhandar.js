var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AppEntryFormBhandar", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("Bhandar", function ($scope, $http) {
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
                $scope.BhandarForm.BhandarId = '5';
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
    $scope.GridData = function () {
        $http.post(SiteUrl + "/EntryForms/GetYearTargetData", $scope.BhandarForm).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.BhandarData = data;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.SaveBhandar = function (IsvalidationChecked) {
        alert($scope.BhandarForm.IsActive + 'RAJ');
        $scope.BhandarForm.IsActive == true ? 1 : 0;
        if ($scope.BhandarFormValidation() || !IsvalidationChecked) {
            $http.post(SiteUrl + "/EntryForms/SaveBhandarForms", $scope.BhandarForm).
                success(function (data, status) {
                    var message = '';
                    if (data > 0) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    }
                    else if (data == -1) {
                        message = _language == 'en-us' ? 'Data Update successfully\n' : 'डेटा सफलतापूर्वक अपडेट किया गया है\n';
                    }
                    else if (data == -2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    }
                    alert(message);
                    window.location.reload();

                    if (data.Status) {
                        $scope.ClearBhandarForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Bhandar Data insert");
                });
        }

    };
    $scope.pageChangeHandler = function (num) {

    };
    $scope.update = function (item) {
        $scope.showtringdiv = true;
        item.IsActive = (item.IsActive == 1 ? true : false);
        $scope.BhandarForm = item;
    };

    $scope.ClearBhandarForm = function () {
        $scope.BhandarForm.FYearId = 0;
        $scope.BhandarForm.MonthId = 0;
        $scope.BhandarForm.ControlledBusinessfulfillment = '';
        $scope.BhandarForm.UnControlledBusinessFulfillment = '';
        $scope.BhandarForm.IsActive = '0';
        $scope.BhandarForm.BhandarDataId = 0;
        
    };
    // Update Template  

    $scope.SetStatus = function (ObjBhandarForm) {
        alert(ObjBhandarForm.IsActive);
        $scope.BhandarForm = ObjBhandarForm;
        $scope.BhandarForm.IsActive = ObjBhandarForm.IsActive == 1 ? 0 : 1;
        $scope.SaveBhandar(false);
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
        $scope.GetDistrictWithoutDivisionList();
        $scope.GetFinancialYearList();
        $scope.GetMonthList();
        $scope.GridData();
    };
    // Language Selector at page load



    $scope.ShowBhandarContent = function () {
        $scope.showtringdiv = true;
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }
    //validateAddForm 
    $scope.BhandarFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BhandarForm.BhandarId == undefined || $scope.BhandarForm.BhandarId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Bhandar' : '\n <br /> कृपया भंडार का चयन करे |';
        }
        if ($scope.BhandarForm.FYearId == undefined || $scope.BhandarForm.FYearId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Year' : '\n <br /> कृपया साल का चयन करे |';
        }
        if ($scope.BhandarForm.ControlledBusinessTarget == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Controlled Business Target(Yearly Basis).' : '\n <br /> कृपया नियंत्रित व्यवसाय लक्ष्य (वार्षिक आधार) दर्ज करें |';
        }
        if ($scope.BhandarForm.UnControlledBusinessTarget == undefined || $scope.BhandarForm.UnControlledBusinessTarget == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter UnControlled Business Target(Yearly Basis)' : '\n <br /> कृपया अनियंत्रित व्यवसाय लक्ष्य (वार्षिक आधार) दर्ज करें |';
        }

        if ($scope.BhandarForm.MonthId == undefined || $scope.BhandarForm.MonthId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Month' : '\n <br /> कृपया माह का चयन करे |';
        }

        if ($scope.BhandarForm.ControlledBusinessfulfillment == undefined || $scope.BhandarForm.ControlledBusinessfulfillment == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Controlled Business fulfillment(Monthly Basis) ' : '\n <br /> कृपया नियंत्रित व्यवसाय पूर्ति(माह वार) दर्ज करें |';
        }
        if ($scope.BhandarForm.UnControlledBusinessFulfillment == undefined || $scope.BhandarForm.UnControlledBusinessFulfillment == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Un Controlled Business fulfillment(Monthly Basis)' : '\n <br /> कृपया अनियंत्रित व्यवसाय पूर्ति(माह वार) दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert("Bhandar Form", message);
            return false;
        }
        return true;

    }
    //End

    $scope.OnPageLoad();
    $scope.DivtringGrid = true;
});

