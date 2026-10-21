var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("PurchaseSalesSocietyMod", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("PurchaseSalesSocietyCon", function ($scope, $http) {
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
    $scope.GetDistrictWithoutKVSSList = function () {
        $http.post(SiteUrl + "/EntryForms/KVSSociety").
            success(function (data, status) {
                $scope.KVSSList = data;
               // $scope.KVSSForm.KVSSId = '5';
            }).
            error(function (data, status) {
                $scope.KVSSList = [];
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
    
    $scope.GridData = function () {
        $scope.KVSSData = [];
        $http.post(SiteUrl + "/EntryForms/KVSSData", $scope.KVSSForm).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.KVSSData = data;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.SaveKVSS = function (IsvalidationChecked) {
       /// alert($scope.KVSSForm.IsActive + 'RAJ');
        $scope.KVSSForm.IsActive == true ? 1 : 0;
        if ($scope.KVSSFormValidation() || !IsvalidationChecked) {
            $http.post(SiteUrl + "/EntryForms/SaveKVSSForms", $scope.KVSSForm).
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
                        $scope.ClearKVSSForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while KVSS Data insert");
                });
        }

    };
    $scope.pageChangeHandler = function (num) {

    };
    $scope.update = function (item) {
        $scope.showtringdiv = true;
        item.IsActive = (item.IsActive == 1 ? true : false);
        $scope.KVSSForm = item;
    };

    $scope.ClearKVSSForm = function () {
        $scope.KVSSForm.FYearId = 0;
        $scope.KVSSForm.AgriProduction = '';
        $scope.KVSSForm.AgriAadan = '';
        $scope.KVSSForm.MaterialControl = '';
        $scope.KVSSForm.MaterialUncontrol = '';
        $scope.KVSSForm.MaterialMedical = '';
        $scope.KVSSForm.IsActive = 0;
        $scope.KVSSForm.KVSSId = 0;

    };
    // Update Template  

    $scope.SetStatus = function (ObjKVSSForm) {
        //alert(ObjKVSSForm.IsActive);
        $scope.KVSSForm = ObjKVSSForm;
        $scope.KVSSForm.IsActive = ObjKVSSForm.IsActive == 1 ? 0 : 1;
        $scope.SaveKVSS(false);
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
        $scope.GetDistrictWithoutKVSSList();
        $scope.GetFinancialYearList();
        $scope.GridData();
    };
    // Language Selector at page load



    $scope.ShowKVSSContent = function () {
        $scope.showtringdiv = true;
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }
    //validateAddForm 
    $scope.KVSSFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        //if ($scope.KVSSForm.Id == undefined || $scope.KVSSForm.Id == '0') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select KVSS' : '\n <br /> कृपया भंडार का चयन करे |';
        //}
        if ($scope.KVSSForm.FYearId == undefined || $scope.KVSSForm.FYearId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Year' : '\n <br /> कृपया साल का चयन करे |';
        }
        if ($scope.KVSSForm.AgriProduction == undefined || $scope.KVSSForm.AgriProduction == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Agri. Production.' : '\n <br /> कृपया कृषि उत्पादन दर्ज करें |';
        }
        if ($scope.KVSSForm.AgriAadan == undefined || $scope.KVSSForm.AgriAadan == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Agri. Input' : '\n <br /> कृपया कृषि Aadan दर्ज करें |';
        }

        if ($scope.KVSSForm.KVSSId == undefined || $scope.KVSSForm.KVSSId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select KVSS' : '\n <br /> कृपया केवीएस सोसायटी का चयन करे |';
        }

        if ($scope.KVSSForm.MaterialControl == undefined || $scope.KVSSForm.MaterialControl == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Material Control ' : '\n <br /> कृपया सामग्री नियंत्रण दर्ज करें |';
        }
        if ($scope.KVSSForm.MaterialMedical == undefined || $scope.KVSSForm.MaterialMedical == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Material Medical' : '\n <br /> कृपया सामग्री मेडिकल दर्ज करें |';
        }
        if ($scope.KVSSForm.MaterialUncontrol == undefined || $scope.KVSSForm.MaterialUncontrol == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Material Uncontrol' : '\n <br /> कृपया सामग्री अनियंत्रण दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert("KVSS Form", message);
            return false;
        }
        return true;

    }
    //End

    $scope.OnPageLoad();
    $scope.DivtringGrid = true;
});

