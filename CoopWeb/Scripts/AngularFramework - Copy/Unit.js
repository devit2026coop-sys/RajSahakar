
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
        //{ "ClassNameEn": $scope.UnitMaster.UnitNameEn, "ClassNameHi": $scope.UnitMaster.UnitNameHi, "IsActive": $scope.UnitMaster.IsActive, "UnitId": $scope.UnitMaster.UnitId, "UnitId": $scope.UnitMaster.DepartmentId}).
        $http.post(SiteUrl +"/Master/SaveUnitForm", $scope.UnitMaster).
            success(function (data, status) {
                // alert(data);

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
                window.location = '/master/unit';

                //if (data > 0) {
                //    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                //    window.location = "/master/unit";
                //}
                //else {
                //    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearUnitForm();
                //    return;
                //}
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


        $scope.ClearUnitForm = function () {
            //$scope.UnitMaster = null;
            $scope.UnitMaster.DepartmentId = 0;
            $scope.UnitMaster.UnitNameEn = '';
            $scope.UnitMaster.UnitNameHi = '';
            $scope.UnitMaster.IsActive = false;
            $scope.UnitMaster.UnitId = 0;
        };

        $scope.Department = [];
        $http.get(SiteUrl +"/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });

    };


    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangedeleteStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };

    $scope.GetAllSubUnitList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl +"/Master/UnitsList").
            success(function (data, status) {
                $scope.UnitData = data;
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
    $scope.GetAllSubUnitList();

});






