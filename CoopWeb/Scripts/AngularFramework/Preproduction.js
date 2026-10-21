var app = angular.module("myApp", ['angularUtils.directives.dirPagination']);
app.controller("myCtrl", function ($scope, $http) {

   

    $scope.GetTableSchemaList = function () {
        $http.post(SiteUrl + "DBMigration/GetTableList").
            success(function (data, status) {                
                $scope.TableSchema = data;
            }).
            error(function (data, status) {
                $scope.TableSchema = [];
            });
    };

    $scope.PushTable = function (table) {       
        LoaderStart();
        $http.post(SiteUrl + "/DBMigration/PushTableDataLocal", { "TableName": table.TableName}).
            success(function (data, status) {
                LoaderStop();
                ShowModelAlert($scope._language == 'en-us' ? '' : '', data);
            }).
            error(function (data, status) {
                //$scope.TableSchema = [];
            });        
    }

    //$scope.TruncateTable = function (table) {
       
    //    $http.post(SiteUrl + "/DBMigration/TurncateTable", { "TableName": table.TableName }).
    //        success(function (data, status) {
    //            ShowModelAlert($scope._language == 'en-us' ? '' : '', data);
    //        }).
    //        error(function (data, status) {               
    //            ShowModelAlert($scope._language == 'en-us' ? '' : '', data);
    //        });   
    //};
   
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        $scope.GetTableSchemaList();
       
    };
    $scope.OnPageLoad();
    
});