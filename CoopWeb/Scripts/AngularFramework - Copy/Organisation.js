
var MasterMod = angular.module('OrganisationMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("OrganisationMasterCont", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.OrganisationMaster = [];

    $scope.SaveOrganisationForm = function () {
        $http.post(SiteUrl +"/Master/SaveOrganisationForm", { "OrganisationNameEn": $scope.OrganisationMaster.OrganisationNameEn, "OrganisationNameHi": $scope.OrganisationMaster.OrganisationNameHi, "IsActive": $scope.OrganisationMaster.IsActive, "OrganisationId": $scope.OrganisationMaster.OrganisationId, "DepartmentId": $scope.OrganisationMaster.DepartmentId }).
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
                window.location = '/master/Organisation';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSubOrganisationList('and 1=1');
                if (data.Status) {

                    $scope.OrganisationMaster = null;
                    $scope.ClearOrganisationForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };

    // on page load
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
        };

    $scope.GetDepartments = function () {
        $scope.Departments = [];
        $http.get(SiteUrl +"/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $scope.Departments = data;
            }).
            error(function (data, status) {
                $scope.Departments = '';
            });
    };

    $scope.ClearOrganisationForm = function () {
        $scope.OrganisationMaster.DepartmentId = '0';
        $scope.OrganisationMaster.OrganisationNameEn = '';
        $scope.OrganisationMaster.OrganisationNameHi = '';
        $scope.OrganisationMaster.IsActive = false;
        $scope.OrganisationMaster.OrganisationId = '0';
    };

    $scope.SetStatus = function (OrganisationId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangedeleteStatusOrganisation", { Id: OrganisationId, Action: ActionId }).
            success(function (data, status) {
                $scope.GetAllSubOrganisationList('and 1=1');
            }).
            error(function (data, status) {
            });
    };

    $scope.GetAllSubOrganisationList = function (DepartmentId) {
        $http.get(SiteUrl +"/Master/OrganisationsList/?DepartmentId=" + DepartmentId).
            success(function (data, status) {
                $scope.OrganisationData = data;
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

    $scope.GetAllSubOrganisationListByDept = function () {
        $scope.GetAllSubOrganisationList($scope.OrganisationMaster.DepartmentId);
        };

    $scope.update = function (item) {
        $scope.OrganisationMaster.DepartmentId = item.DepartmentId;
        $scope.OrganisationMaster.OrganisationNameEn = item.OrganisationNameEn;
        $scope.OrganisationMaster.OrganisationNameHi = item.OrganisationNameHi;
        $scope.OrganisationMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.OrganisationMaster.OrganisationId = item.OrganisationId;
    };

    $scope.OnPageLoad();
    $scope.GetAllSubOrganisationList('and 1=1');

});


