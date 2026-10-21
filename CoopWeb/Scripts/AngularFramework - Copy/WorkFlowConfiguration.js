var WorkFlowConfigurationMod = angular.module('WorkFlowConfigurationModule', ['angularUtils.directives.dirPagination']);
WorkFlowConfigurationMod.controller("WorkFlowConfigurationCont", function ($scope, $http) {

    // SCOPE
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Configuration = [];

    // Code for Language label setting
    $scope.getTranslation = function () {
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
    // END : Translation

   //START : Page list
    $scope.PageList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MENUCONFIGURATION&MasterCode=-1").
            success(function (data, status) {
                $scope.Pages = data;
            }).
            error(function (data, status) {
                $scope.Pages = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Page list


    //START : Page list
    $scope.ActionListList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CONFIGURATIONACTION&MasterCode=-1").
            success(function (data, status) {
                $scope.Actions = data;
            }).
            error(function (data, status) {
                $scope.Actions = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Page list

    // START : Get List of all Role
    $scope.LevelList = function (WhereClause) {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS&MasterCode=-1" ).
            success(function (data, status) {
                $scope.LevelList = data;
            }).
            error(function (data, status) {
                $scope.LevelList = [];
            });
    };
    // END : Get List of all Role
    // START : Get List of all Role
    $scope.GetRoleList = function (WhereClause) {
        $scope.Roles = [];
        $http.get(SiteUrl + "/User/SelectRole?LevelId=" + $scope.Configuration.LevelId).
            success(function (data, status) {
                $scope.Roles = data;
            }).
            error(function (data, status) {
                $scope.Roles = [];
            });
    };
    // END : Get List of all Role

    //START : Validation While Action Save
    function ValidateConfigurationSaveForm() {
        var msg = '';

        if ($scope.Configuration.MenuId === undefined || $scope.Configuration.MenuId === "0") {
            msg += _language === 'en-us' ? 'Please select page</br>' : 'कृपया पेज चुनें </br>';
        }

        if ($scope.Configuration.RoleId === undefined || $scope.Configuration.RoleId === "0") {
            msg += _language === 'en-us' ? 'Please select role</br>' : 'कृपया भूमिका चुनें </br>';
        }

        if ($scope.Configuration.SocietyProceesStatusIdParent === undefined || $scope.Configuration.SocietyProceesStatusIdParent === "0") {
            msg += _language === 'en-us' ? 'Please select action</br>' : 'कृपया कार्य चुनें</br>';
        }

        if (msg.length > 0) {
            ShowModelAlert(_language === 'en-us' ? 'Work Flow Configuration' : 'कार्य प्रवाह विन्यास', msg); //,
            return false;
        }
        return true;
    }
    //END : Validation While Action Save

    // START : Save Actions
    $scope.SaveConfiguration = function () {

        if (!ValidateConfigurationSaveForm()) {
            return;
        }

        var obj = {
            "ApplicationNextActionId": $scope.Configuration.ApplicationNextActionId,
            "SocietyProceesStatusIdChild": $scope.Configuration.SocietyProceesStatusIdChild,
            "SocietyProceesStatusIdParent": $scope.Configuration.SocietyProceesStatusIdParent,
            "MenuId": $scope.Configuration.MenuId,
            "RoleId": $scope.Configuration.RoleId,
            "NextRoleId": $scope.Configuration.NextRoleId,
            "WorkOnActId": $scope.Configuration.WorkOnActId,
            "LevelId": $scope.Configuration.LevelId
        };


        $http.post("../Master/SaveWorkFlowConfiguration", obj).
            success(function (data, status) {
                if (data > 0) { // Insert
                    ShowModelAlert(_language ==='en-us' ? 'Work Flow Configuration' : 'कार्य प्रवाह विन्यास', _language === 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।'); //,
                    $scope.ClearForm();
                    $scope.GetWorkFlowListList();
                }

                if (data === -1) { // Error
                    ShowModelAlert(_language ==='en-us' ? 'Work Flow Configuration' : 'कार्य प्रवाह विन्यास', _language === 'en-us' ? 'Error !!!' : 'त्रुटि !!!'); //,
                }

                if (data === -2) { // Edit
                    $scope.GetWorkFlowListList();
                    ShowModelAlert(_language ==='en-us' ? 'Work Flow Configuration' : 'कार्य प्रवाह विन्यास', _language === 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया।'); //,
                    $scope.ClearForm();
                }

                if (data === -3) { // Duplicate
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data already exists.' : 'डेटा पहले से मौजूद है।'); //,
                }
            }).
            error(function (data, status) {
                ShowModelAlert(_language ==='en-us' ? 'Work Flow Configuration' : 'कार्य प्रवाह विन्यास', _language === 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है');
            });
    };
    // END : Save Actions

    // START : Save Actions
    $scope.ChangeActionMasterStatus = function (id) {

        var obj = {
            "ApplicationNextActionId": id,
            "SocietyProceesStatusIdChild": -1,
            "SocietyProceesStatusIdParent": -1,
            "MenuId": -1,
            "RoleId": -1,
            "NextRoleId": -1,
            "WorkOnActId": 0,
            "LevelId":-1
        };


        $http.post("../Master/ChangeWorkFlowConfiguration", obj).
            success(function (data, status) {
                if (data > 0) { // Insert
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।'); //,
                    $scope.ClearForm();
                   // $scope.GetActionList();
                    $scope.GetWorkFlowListList();
                }

                if (data === -1) { // Error
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Error !!!' : 'त्रुटि !!!'); //,
                }

                if (data === -2) { // Edit
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया।'); //,
                    $scope.ClearForm();
                 //   $scope.GetActionList();
                    $scope.GetWorkFlowListList();
                }
                
            }).
            error(function (data, status) {
                ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है');
            });
    };
    // END : Save Actions

    // START : Save Actions
    $scope.DeleteActionMasterData = function (id) {

        if (!confirm(_language === 'en-us' ? 'Are you sure to delete this record?' : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं?')) {
            return;
        }

        var obj = {
            "ApplicationNextActionId": id,
            "SocietyProceesStatusIdChild": -1,
            "SocietyProceesStatusIdParent": -1,
            "MenuId": -1,
            "RoleId": -1,
            "NextRoleId": -1,
            "WorkOnActId": 0,
            "LevelId":-1
        };


        $http.post("../Master/DeleteWorkFlowConfiguration", obj).
            success(function (data, status) {
                if (data === -1) { // Error
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Error !!!' : 'त्रुटि !!!'); //,
                }

                if (data === -2) { // Edit
                    ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Data deleted Successfully.' : 'डेटा सफलतापूर्वक हटा दिया गया।'); //,
                    // $scope.GetActionList();
                    location.reload();
                }
            }).
            error(function (data, status) {
                ShowModelAlert(_language === 'en-us' ? 'Action Master' : 'एक्शन मास्टर', _language === 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है');
            });
    };
    // END : Save Actions


    // STAT : Get All Actions
    $scope.GetWorkFlowListList = function () {
        $http.post("../Master/GetWorkFlowConfiguration", { clause: "" }).
            success(function (data, status) {
                $scope.AllWorkFlow = data;
            }).
            error(function (data, status) {
                $scope.AllWorkFlow = [];
            });
    };
    //END : Get All Actions

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Configuration.MenuId = '0';
        $scope.Configuration.RoleId = '0';
        $scope.Configuration.SocietyProceesStatusIdParent = '0';
        $scope.Configuration.NextRoleId = '0';
        $scope.Configuration.ApplicationNextActionId = '0';
        $scope.Configuration.SocietyProceesStatusIdChild = '0';
        $scope.Configuration.WorkOnActId = '0';
        $scope.Configuration.LevelId = '0';
    };
    //End : Clear Form


    //START : Update Action
    $scope.Update = function (item) {
        $scope.Configuration = [];
        console.log(item);
        $scope.Configuration.LevelId = item.LevelId;
        $scope.GetRoleList();
        $scope.Configuration.MenuId = item.MenuId;
        $scope.Configuration.RoleId = item.RoleId;
        $scope.Configuration.SocietyProceesStatusIdParent = item.SocietyProceesStatusIdParent;
        $scope.Configuration.NextRoleId = item.NextRoleId;
        $scope.Configuration.ApplicationNextActionId = item.ApplicationNextActionId;
        $scope.Configuration.SocietyProceesStatusIdChild = item.SocietyProceesStatusIdChild;
        $scope.Configuration.WorkOnActId = item.WorkOnActId;
    };
    //END : Update Action

    // Language Selector
    $scope.getTranslation();
    $scope.PageList();
    $scope.ActionListList();
   // $scope.RoleList('and 1=1');
    $scope.LevelList();
    $scope.GetWorkFlowListList();// $scope.GetRoleList();

});