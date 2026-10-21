
var MasterMod = angular.module('DesignationLevelMappingMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("DesignationLevelMappingMasterCont", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.DesignationLevelMappingMaster = [];

    $scope.SaveDesignationLevelMappingForm = function () {
        if ($scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID != 0) {
            if ($scope.DesignationLevelMappingMaster.OfficeId != 0) {
                $http.post(SiteUrl + "/MasterSettings/SaveDesignationLevelMappingForm", { "DESIGNATIONCODE": $scope.DesignationLevelMappingMaster.DESIGNATIONCODE, "IsHOD": $scope.DesignationLevelMappingMaster.IsHOD, "IsActive": $scope.DesignationLevelMappingMaster.IsActive, "DesignationLevelMappingId": $scope.DesignationLevelMappingMaster.DesignationLevelMappingId, "OrganizationLevelsMappingID": $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID, "OfficeId": $scope.DesignationLevelMappingMaster.OfficeId }).
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
                        window.location = '/mastersettings/DesignationLevelMapping';
                        if ($scope.ParentId == '') {
                            $scope.ParentId = 0;
                        }

                        $scope.GetAllDesignationLevelMappingList('And 1=1');
                        if (data.Status) {

                            $scope.DesignationLevelMappingMaster = null;
                            $scope.ClearDesignationLevelMappingForm();
                            //$scope.GetAllSubClassesList();


                        }
                    }).
                    error(function (data, status) {
                        alert("Error while subclass insert");
                    });
            }
            else {
                alert(_language == 'en-us' ? 'Please Select Office.\n' : 'कृपया कार्यालय का चयन करे \n');
                return;
            }
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Level.\n' : 'कृपया स्तर का चयन करे \n');
            return;
        }

    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

        }
        $scope.GetDepartments();
        $(".se-pre-con").fadeOut("slow");

        ////$scope.ClearDesignationLevelMappingForm();
    };
    $scope.GetDepartments = function () {
        $scope.Departments = [];
        $scope.Levels = [];
        $scope.DesignationLevelMappingMaster.OrganisationId = 0;
        $scope.Organisations = [];
        $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID = 0;
        $scope.DesignationLevelMappingMaster.OfficeId = 0;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $scope.Departments = data;
            }).
            error(function (data, status) {
                $scope.Departments = '';
            });
    };
    $scope.GetOrganisations = function () {
        //  alert($scope.DesignationLevelMappingMaster.DepartmentId);
        $scope.Levels = [];
        $scope.DesignationLevelMappingMaster.OrganisationId = 0;
        $scope.Organisations = [];
        $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID = 0;
        $scope.DesignationLevelMappingMaster.OfficeId = 0;
        //$http.get("/Master/OrganisationsList/?DepartmentId=" + $scope.DesignationLevelMappingMaster.DepartmentId + "").
        $http.get(SiteUrl + "/Master/OrganisationsList/?DepartmentId=" + $scope.DesignationLevelMappingMaster.DepartmentId).
            success(function (data1, status) {
                //  alert(JSON.stringify(data1));
                $scope.Organisations = data1;
            }).
            error(function (data, status) {
                $scope.Organisations = '';
            });
    };
    $scope.GetLevels = function () {
        $scope.Levels = [];
        $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID = 0;
        $scope.DesignationLevelMappingMaster.OfficeId = 0;
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.DesignationLevelMappingMaster.OrganisationId).
            success(function (data, status) {
                $scope.Levels = data;
            }).
            error(function (data, status) {
                $scope.GetLevels = '';
            });
    };

    $scope.GetDesignations = function () {
        $scope.Designations = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=-1").
            success(function (data, status) {
                $scope.Designations = data;
            }).
            error(function (data, status) {
                $scope.GetLevels = '';
            });
    };

    $scope.OfficeList = function () {
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID).
            success(function (data, status) {
                $scope.OfficeListData = data;
            }).
            error(function (data, status) {
                console.log("Error while fetching designation list")
                $scope.OfficeListData = [];
                // $scope.showLoader = false;
            });
    };

    $scope.ClearDesignationLevelMappingForm = function () {
        $scope.DesignationLevelMappingMaster.DepartmentId = 0;
        $scope.Organisations = [];
        $scope.DesignationLevelMappingMaster.OrganisationId = 0;
        $scope.Levels = [];
        $scope.DesignationLevelMappingMaster.OfficeId = 0;
        $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID = 0;
        $scope.DesignationLevelMappingMaster.DESIGNATIONCODE = 0;
        $scope.DesignationLevelMappingMaster.IsActive = false;
        $scope.DesignationLevelMappingMaster.IsHOD = false;
        $scope.DesignationLevelMappingMaster.DesignationLevelMappingId = 0;
    };

    $scope.SetStatus = function (DesignationLevelMappingId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/MasterSettings/ChangeStatusDesignationLevelMapping", { Id: DesignationLevelMappingId, Action: ActionId }).
            success(function (data, status) {
                $scope.GetAllDesignationLevelMappingList('And 1=1');
            }).
            error(function (data, status) {
            });
    };

    $scope.GetAllDesignationLevelMappingList = function (WhereClause) {
        $http.get(SiteUrl + "/MasterSettings/DesignationLevelMappingsList/?WhereClause=" + WhereClause).
            success(function (data, status) {
                $scope.DesignationLevelMappingData = data
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


    $scope.GetAllDesignationLevelMappingListByDept = function () {
        $scope.GetAllDesignationLevelMappingList('and d.DEPTCODE=' + $scope.DesignationLevelMappingMaster.DepartmentId);
    };

    $scope.GetAllDesignationLevelMappingListByOrg = function () {
        $scope.GetAllDesignationLevelMappingList('and org.OrganisationId=' + $scope.DesignationLevelMappingMaster.OrganisationId);
    };

    $scope.GetAllDesignationLevelMappingListBylbl = function () {
        $scope.GetAllDesignationLevelMappingList('and o.OrganizationLevelsMappingID=' + $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID);
    };

    $scope.GetAllDesignationLevelMappingListByDesig = function () {
        $scope.GetAllDesignationLevelMappingList('and des.DESIGNATIONCODE=' + $scope.DesignationLevelMappingMaster.DESIGNATIONCODE);
    };

    $scope.update = function (item) {
        $scope.DesignationLevelMappingMaster.DepartmentId = item.DepartmentId;
        $scope.GetOrganisations();
        $scope.DesignationLevelMappingMaster.OrganisationId = item.OrganisationId;
        $scope.GetLevels();
        $scope.DesignationLevelMappingMaster.OrganizationLevelsMappingID = item.OrganizationLevelsMappingID;
        $scope.OfficeList();
        $scope.DesignationLevelMappingMaster.OfficeId = item.OfficeId;
        $scope.DesignationLevelMappingMaster.LevelId = item.LevelId;
        $scope.DesignationLevelMappingMaster.DESIGNATIONCODE = item.DESIGNATIONCODE;
        $scope.DesignationLevelMappingMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.DesignationLevelMappingMaster.IsHOD = item.IsHOD == 1 ? true : false;
        $scope.DesignationLevelMappingMaster.DesignationLevelMappingId = item.DesignationLevelMappingId;
    };

    $scope.OnPageLoad();
    $scope.GetAllDesignationLevelMappingList('And 1=1');
    $scope.GetDesignations();

});



