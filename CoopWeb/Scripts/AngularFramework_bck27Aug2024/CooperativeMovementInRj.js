var CooperativeMovementRj = angular.module("CooperativeMovementRjModule", ['angularUtils.directives.dirPagination']);
CooperativeMovementRj.controller("CooperativeMovementRjCount", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DataList = [];
    $scope.OnPageLoad = function () {
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
        };
        $scope.GetCooperativeMovement();
    };
    $scope.GetCooperativeMovement = function () {        
        $scope.CooperativeMovement = [];
        $http.post(SiteUrl + "MIS/GetCooperativeMovement").
            success(function (data, status){
                $scope.CooperativeMovement = data;
            }).
            error(function (data, status) {
                alert("Geting Some Error");
            })
    };
    
    $scope.CooperativeMovementInsert = function () {  
        $scope.obj = [];
        $("#CMRJ > tbody > tr").each(function () {
            if (($(this).find('.clshfItemId').val() != null || $(this).find('.clstxtValue').val().length > 0)) {
                $scope.obj.push(
                    {
                        "CoopMovementId": $scope.CoopMovementId,
                        "ItemId": $(this).find('.clshfItemId').val(),                        
                        "ItemValue": $(this).find('.clstxtValue').val()
                    }
                );
            }
        });        
        $http.post(SiteUrl + "MIS/InsertCoop", $scope.obj).
            success(function (data, status) {
                var k = data;
                if (k == 0) {
                    var message = '';
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    var message = '';
                    message = _language == 'en-us' ? 'Data Update successfully\n' : 'डेटा अपडेट सफलतापूर्वक किया गया है | \n';
                    alert(message);
                    location.reload();
                }
            }).
            error(function (data, status) {
                alert("Geting Some Error");
            })
    };
    $scope.OnPageLoad();
});