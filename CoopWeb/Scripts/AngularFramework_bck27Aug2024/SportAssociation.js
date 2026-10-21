
var MasterMod = angular.module('SportMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SportMasterCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.SportAssociation = '';


    $scope.SaveSportAssociation = function () {       
        $http.post(SiteUrl +"/SportAssociation/SaveSportAssociationnew", $scope.SportAssociation).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                var k = data;
                var message = '';

                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                alert(message);
                //ShowModelAlert(_language == 'en-us' ? 'Sport Association' : 'खेल एसोसिएशन', message);
                window.location = '/master/sportassociation';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSportAssociationList();
                if (data.Status) {
                    $scope.ClearSportsAsso();
                    $scope.GetAllSportAssociationList();


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

        $(".se-pre-con").fadeOut("slow");

    };
    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatusdelete = function (AssociationId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/SportAssociation/Deleteandstatus", { Id: AssociationId, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.GetAllSportAssociationList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
            });
    };
    $scope.GetAllSportAssociationList = function () {
        $http.get(SiteUrl +"/SportAssociation/SportAssociationList").
            success(function (data, status) {

                $scope.sportdata = data

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
    }

    $scope.update = function (item) {
        $scope.SportAssociation = item;
    };

    $scope.ClearSportsAsso = function () {
        $scope.SportAssociation = '';
    }

    $scope.OnPageLoad();
    $scope.GetAllSportAssociationList();

});






