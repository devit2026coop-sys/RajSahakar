var MappingMod = angular.module('MappingModule', ['angularUtils.directives.dirPagination']);
MappingMod.controller("MappingCont", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 25;
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
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

        $scope.DepartmentList();
        $scope.OrganizationLevelsList();
        $(".se-pre-con").fadeOut("slow");
    };
    var ArryOrganizationMapping = new Array();
    function OrganizationMapping() {
        this.DepartmentId = 0;
        this.DepartmentId = 0;
        this.OrganisationId = 0;
        this.LevelId = 0;
        this.IsActive = 0;
    }

    //Get Department List for dropdown
    $scope.DepartmentList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $scope.Departments = data;
            }).
            error(function (data, status) {
                alert("Error while fetching department list")
                $scope.Departments = [];
            });
    };
    //END

    $scope.SaveOrganizationLevelsMapping = function () {
        ArryOrganizationMapping = [];
        if ($scope.FormValidation()) {
            angular.forEach($scope.Level, function (value) {
                if (value.Checked) {
                    var _OrganizationMapping = new OrganizationMapping();
                    _OrganizationMapping.DepartmentId = $scope.Mapping.DepartmentId;
                    _OrganizationMapping.OrganisationId = $scope.Mapping.OrganisationId;
                    _OrganizationMapping.LevelId = value.LevelId;
                    _OrganizationMapping.IsActive = $scope.Mapping.IsActive;
                    _OrganizationMapping.MappingID = $scope.Mapping.MappingID;
                    ArryOrganizationMapping.push(_OrganizationMapping);
                }
            });


            $http.post(SiteUrl + "/MasterSettings/SaveOrgLevelMapping", ArryOrganizationMapping).
                success(function (data, status) {                   
                    window.location = '/MasterSettings/OrganizationLevelsMappings';
                    if ($scope.ParentId == '') {
                        $scope.ParentId = 0;
                    }
                    if (data.Status) {
                        $scope.ClearForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Organization Levels Mappings insert");
                });
        }

    };

    //validateAddForm 
    $scope.FormValidation = function () {
        var IsLevel = false;
        angular.forEach($scope.Level, function (value) {
            if (value.Checked) {
                IsLevel = true;
            }
        });

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';


        if ($scope.Mapping.OrganisationId == undefined || $scope.Mapping.OrganisationId == 0) {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Organization.' : '\n<br />कृपया संगठन का चयन करें |';
        }

        if (IsLevel == false) {
            message += $scope._language == 'en-us' ? '\n<br />Please select atleast one level.' : '\n<br />कृपया कम से कम एक स्तर का चयन करें |';
        }

        if (message.length > 29) {
            ShowModelAlert("User Profiling", message);
            return false;
        }
        return true;
    }

    $scope.ClearForm = function () {

    };

    //Start: Get Unit List
    $scope.LevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS&MasterCode=0").
            success(function (data, status) {
                $scope.Level = data;
            }).
            error(function (data, status) {
                $scope.Level = [];
            });
    };
    //End 

    //Get Organisation List for dropdown
    $scope.GetAllOrganisationList = function () {       
        $http.get(SiteUrl +"/Master/OrganisationsList/?DepartmentId=" + $scope.Mapping.DepartmentId).
            success(function (data, status) {
                $scope.OrganisationData = data;
            }).
            error(function (data1, status) {

            });
    };
    //END
    //START
    $scope.SetStatus = function (RoleID, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/MasterSettings/ChangeMappingStatus", { Id: RoleID, Action: ActionId }).
            success(function (data, status) {
                $scope.OrganizationLevelsList();
            }).
            error(function (data, status) {
            });
    };
    //END

    $scope.update = function (tableData) {
        angular.forEach($scope.Level, function (value) {
            value.Checked = false;
        });
        $scope.Mapping.DepartmentId = tableData.DepartmentId;
        $scope.GetAllOrganisationList();
        $scope.Mapping.OrganisationId = tableData.OrganisationId;
        $scope.Mapping.MappingID = tableData.MappingID;        
        $scope.OrganizationLevelsList();
        angular.forEach($scope.Level, function (value) {
            if (value.LevelId == tableData.LevelId) {
                value.Checked = true;
            }
        });
        $scope.Mapping = tableData;

    };

    $scope.OrganizationLevelsList = function (WhereClause) {
       
        $http.get(SiteUrl +"/MasterSettings/OrganizationLevelsList/?WhereClause=" + WhereClause).
            success(function (data, status) {
                $scope.OrganizationLevel = data;
            }).
            error(function (data, status) {
                alert("Error while fetching role list.");
               
            });
    };


    $scope.OrganizationLevelsListByDept = function () {
        $scope.OrganizationLevelsList('and d.DEPTCODE=' + $scope.Mapping.DepartmentId);
    };
    $scope.OrganizationLevelsListByOrg = function () {
        $scope.OrganizationLevelsList('and o.OrganisationId=' + $scope.Mapping.OrganisationId);
    };


    $scope.OnPageLoad();
    $scope.LevelList();
});