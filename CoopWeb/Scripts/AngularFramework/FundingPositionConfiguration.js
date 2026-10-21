
var ActionMasterMod = angular.module('FundingPositionConfigurationMod', ['angularUtils.directives.dirPagination']);
ActionMasterMod.controller("FundingPositionConfigurationCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Action = [];

    //START : Validation While Action Save
    function ValidateActionSaveForm(actids) {
        var msg = '';

        if ($scope.Action.ProcessStatusEn === undefined || $scope.Action.ProcessStatusEn.length < 1) {
            msg += _language === 'en-us' ? 'Please write Process status (English)</br>' : 'कृपया प्रक्रिया स्थिति (अंग्रेजी) लिखें</br>';
        }

        if ($scope.Action.ProcessStatusHi === undefined || $scope.Action.ProcessStatusHi.length < 1) {
            msg += _language === 'en-us' ? 'Please write Process status (Hindi)</br>' : 'कृपया प्रक्रिया की स्थिति (हिंदी) लिखें </br>';
        }

        if ( actids.length < 1) {
            msg += _language === 'en-us' ? 'Please Select Act</br>' : 'कृपया एक्ट लिखें</br>';
        }

        if (msg.length > 0) {
            ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', msg); //,
            return false;
        }
        return true;
    }
    //END : Validation While Action Save

    // START : Save Actions
    $scope.SaveActionMasterEntry = function () {

        $(".se-pre-con").fadeIn("slow");
        var actids = '';
        $('input[type=checkbox][name=chkActList]:checked').each(function (i, j) {
            actids += $(j).val() + ',';
        });

        if (actids.length > 0) {
            actids = actids.substr(0, actids.length - 1);
        }

        if (!ValidateActionSaveForm(actids)) {
            return;
        }

        var obj = {
            "SocietyProceesStatusId": $scope.Action.SocietyProceesStatusId,
            "ProceesStatusTextEn": $scope.Action.ProcessStatusEn,
            "ProceesStatusTextHi": $scope.Action.ProcessStatusHi,
            "ActId": actids,
            "IsActive": $scope.Action.IsActive === undefined ? 0 : $scope.Action.IsActive ? 1 : 0,
            "IsEnduserRelated": $scope.Action.IsEndUserRelated === undefined ? 0 : $scope.Action.IsEndUserRelated ? 1 : 0
        };


        $http.post("../Master/SaveSocietyProcessStatus", obj).
            success(function (data, status) {
                if (data > 0) { // Insert
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।'); //,
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ClearForm();
                    $scope.GetActionList();
                }

                if (data === -1) { // Error
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Error !!!' : 'त्रुटि !!!'); //,
                    $(".se-pre-con").fadeOut("slow");
                }

                if (data === -2) { // Edit
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया।'); //,
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ClearForm();
                    $scope.GetActionList();
                }

                if (data === -3) { // Duplicate
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data already exists.' : 'डेटा पहले से मौजूद है।'); //,
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data, status) {
                ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END : Save Actions

    // START : Save Actions
    $scope.ChangeActionMasterStatus = function (id) {
        $(".se-pre-con").fadeIn("slow");
        var obj = {
            "SocietyProceesStatusId": id,
            "ProceesStatusTextEn": '',
            "ProceesStatusTextHi": '',
            "ActId": '',
            "IsActive": -1,
            "IsEnduserRelated": -1
        };


        $http.post("../Master/ChangeSocietyProcessStatus", obj).
            success(function (data, status) {
                if (data > 0) { // Insert
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।'); //,
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ClearForm();
                    $scope.GetActionList();
                }

                if (data === -1) { // Error
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Error !!!' : 'त्रुटि !!!'); //,
                    $(".se-pre-con").fadeOut("slow");
                }

                if (data === -2) { // Edit
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया।'); //,
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ClearForm();
                    $scope.GetActionList();
                }
            }).
            error(function (data, status) {
                ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END : Save Actions

    // START : Save Actions
    $scope.DeleteActionMasterData = function (id) {
        $(".se-pre-con").fadeIn("slow");
        if (!confirm(_language === 'en-us' ? 'Are you sure to delete this record?' : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं?')) {
            return;
        }

        var obj = {
            "SocietyProceesStatusId": id,
            "ProceesStatusTextEn": '',
            "ProceesStatusTextHi": '',
            "ActId": '',
            "IsActive": -1,
            "IsEnduserRelated": -1
        };


        $http.post("../Master/DeleteSocietyProcessStatus", obj).
            success(function (data, status) {
                if (data === -1) { // Error
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Error !!!' : 'त्रुटि !!!'); //,
                    $(".se-pre-con").fadeOut("slow");
                }

                if (data === -2) { // Edit
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data deleted Successfully.' : 'डेटा सफलतापूर्वक हटा दिया गया।'); //,
                    $(".se-pre-con").fadeOut("slow");
                    $scope.ClearForm();
                    $scope.GetActionList();
                }
            }).
            error(function (data, status) {
                ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END : Save Actions


    // STAT : Get All Actions
    $scope.GetActionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post("../Master/GetSocietyProcessStatus", { clause: "" }).
            success(function (data, status) {
                $scope.AllActions = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.AllActions = [];
            });
    };
    //END : Get All Actions

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Action.ProcessStatusEn = '';
        $scope.Action.ProcessStatusHi = '';
        $scope.Action.Act = '';
        $scope.Action.IsActive = null;
        $scope.Action.IsEndUserRelated = null;
        $scope.Action.SocietyProceesStatusId = 0;
        $('input[type=checkbox][name=chkActList]').each(function (i, j) {
            $(this).prop("checked", false);
        });
    };
    //End : Clear Form


    //START : Update Action
    $scope.Update = function (item) {
        $scope.Action = [];
        $scope.Action.ProcessStatusEn = item.ProceesStatusTextEn;
        $scope.Action.ProcessStatusHi = item.ProceesStatusTextHi;
        $scope.Action.Act = item.ActId;
        $scope.Action.IsEndUserRelated = item.IsEnduserRelated === '0' ? false : true;
        $scope.Action.IsActive = item.IsActive === '0' ? false : true;
        $scope.Action.SocietyProceesStatusId = item.SocietyProceesStatusId;
        $('input[type=checkbox][name=chkActList]').each(function (i, j) {
            $(this).prop("checked", false);
            if ($scope.Action.Act.indexOf($(j).val()) > -1) {
                $(this).prop("checked", true);
            };
        });
    };
    //END : Update Action
   

   

    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language === 'en-us') {
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                $scope.showLoader = false;
            });
    };

    $scope.OnPageLoad();
    $scope.GetActionList();
});






