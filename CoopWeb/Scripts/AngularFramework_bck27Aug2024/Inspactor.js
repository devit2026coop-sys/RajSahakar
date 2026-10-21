
var MasterMod = angular.module('UnitMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UnitMasterCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = []; 

    $scope.SaveUnitForm = function () {
        $http.post(SiteUrl + "/Master/SaveUnitForm", $scope.UnitMaster).
            success(function (data, status) {
                alert(data);
                if (data > 0) {
                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearUnitForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearUnitForm();
                    return;
                }
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSubUnitList();
                if (data.Status) {

                    $scope.UnitMaster = null;
                    $scope.ClearUnitForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        

        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {
               
                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });
       
    };

    

    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();
               
            }).
            error(function (data, status) {
            });
    };

    $scope.GetInspactor = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetInspactor").
            success(function (data, status) {
                $scope.InspacorData = data;
                if (data != null && data.length == 0) {
                    $scope.divAuditorData = false;
                }
                else {
                    $scope.divAuditorData = true;
                }

            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.update = function (item) {
        
        $scope.UnitMaster.DepartmentId = item.departmentId;
        $scope.UnitMaster.UnitNameEn = item.UnitNameEn;
        $scope.UnitMaster.UnitNameHi = item.UnitNameHi;
        $scope.UnitMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.UnitMaster.UnitId = item.UnitId;
    };

    $scope.OnPageLoad();
    $scope.GetInspactor();

});






