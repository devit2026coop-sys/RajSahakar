var MasterMod = angular.module('OfficeMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("OfficeMasterCont", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.OfficeMaster = [];

    $scope.SaveOfficeForm = function () {
        if ($scope.OfficeMaster.OrganizationLevelsMappingID != 0) {
            $http.post(SiteUrl +"/Master/SaveOfficeForm", { "OfficeNameEn": $scope.OfficeMaster.OfficeNameEn, "OfficeNameHi": $scope.OfficeMaster.OfficeNameHi, "IsActive": $scope.OfficeMaster.IsActive, "OfficeId": $scope.OfficeMaster.OfficeId, "OrganizationLevelsMappingID": $scope.OfficeMaster.OrganizationLevelsMappingID }).
                success(function (data, status) {
                    var k = data;
                    var message = '';

                    if (k == 1) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    }
                    else if (k == -2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    }
                    alert(message);
                    //ShowModelAlert(_language == 'en-us' ? 'Sport Association' : 'खेल एसोसिएशन', message);
                    window.location = '/master/Office';
                    if ($scope.ParentId == '') {
                        $scope.ParentId = 0;
                    }

                    $scope.GetAllOfficeList('AND 1=1');
                    if (data.Status) {

                        $scope.OfficeMaster = [];
                        $scope.ClearOfficeForm();
                        //$scope.GetAllSubClassesList();


                    }
                }).
                error(function (data, status) {
                    alert("Error while subclass insert");
                });
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Level.\n' : 'कृपया स्तर का चयन करे \n');
            return;

        }

    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

        }
        $scope.GetDepartments();
        $(".se-pre-con").fadeOut("slow");

        ////$scope.ClearOfficeForm();
    };


    $scope.GetDepartments = function () {
        $scope.Departments = [];
        $scope.Levels = [];
        $scope.OfficeMaster.OrganisationId = 0;
        $scope.Organisations = [];
        $scope.OfficeMaster.OrganizationLevelsMappingID = 0;
        $http.get(SiteUrl +"/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $scope.Departments = data;
            }).
            error(function (data, status) {
                $scope.Departments = '';
            });
    };
    $scope.GetOrganisations = function () {
        $scope.Levels = [];
        $scope.OfficeMaster.OrganisationId = 0;
        $scope.Organisations = [];
        $scope.OfficeMaster.OrganizationLevelsMappingID = 0;
        $http.get(SiteUrl +"/Master/OrganisationsList/?DepartmentId=" + $scope.OfficeMaster.DepartmentId).
            success(function (data1, status) {
                $scope.Organisations = data1;
            }).
            error(function (data, status) {
                $scope.Organisations = '';
            });
    };
    $scope.GetLevels = function () {
        $scope.Levels = [];
        $scope.OfficeMaster.OrganizationLevelsMappingID = 0;
        $http.get(SiteUrl +"/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.OfficeMaster.OrganisationId ).
            success(function (data, status) {
                $scope.Levels = data;
            }).
            error(function (data, status) {
                $scope.GetLevels = '';
            });
    };

    
    $scope.ClearOfficeForm = function () {
        $scope.OfficeMaster.DepartmentId = '0';
        $scope.Organisations ='0';
        $scope.OfficeMaster.OrganisationId = '0';
        $scope.Levels = '0';
        $scope.OfficeMaster.OrganizationLevelsMappingID = '0';
        $scope.OfficeMaster.OfficeNameEn = '';
        $scope.OfficeMaster.OfficeNameHi = '';
        $scope.OfficeMaster.IsActive = false;
        $scope.OfficeMaster.OfficeId = '0';
    };

    $scope.SetStatus = function (OfficeId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusOffice", { Id: OfficeId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllOfficeList('AND 1=1');

            }).
            error(function (data, status) {
            });
    };

    $scope.GetAllOfficeList = function (WhereClause) {
        $http.get(SiteUrl +"/Master/OfficesList/?WhereClause=" + WhereClause).
            success(function (data, status) {
                $scope.OfficeData = data

                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };


                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {

            });
    };

    $scope.GetAllOfficeListByDept = function () {
        $scope.GetAllOfficeList('and d.DEPTCODE=' + $scope.OfficeMaster.DepartmentId);
    };

    $scope.GetAllOfficeListByOrg = function () {
        $scope.GetAllOfficeList('and org.OrganisationId=' + $scope.OfficeMaster.OrganisationId);
    };

    $scope.GetAllOfficeListBylbl = function () {
        //alert($scope.OfficeMaster.OrganizationLevelsMappingID);
        $scope.GetAllOfficeList('and o.OrganizationLevelsMappingID=' + $scope.OfficeMaster.OrganizationLevelsMappingID);
    };




    $scope.update = function (item) {
        $scope.OfficeMaster.DepartmentId = item.DepartmentId;
        $scope.GetOrganisations();
        $scope.OfficeMaster.OrganisationId = item.OrganisationId;
        $scope.GetLevels();
        $scope.OfficeMaster.OrganizationLevelsMappingID = item.OrganizationLevelsMappingID;
        $scope.OfficeMaster.LevelId = item.LevelId;
        $scope.OfficeMaster.OfficeNameEn = item.OfficeNameEn;
        $scope.OfficeMaster.OfficeNameHi = item.OfficeNameHi;
        $scope.OfficeMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.OfficeMaster.OfficeId = item.OfficeId;
    };

    $scope.OnPageLoad();
    $scope.GetAllOfficeList('AND 1=1');

});



