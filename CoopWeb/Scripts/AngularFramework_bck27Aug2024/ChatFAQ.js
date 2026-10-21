
var ChatFAQ = angular.module('ChatFAQMod', []);
ChatFAQ.controller("ChatFAQCont", function ($scope, $rootScope, $http) {
    $scope._language = 'en-us';

    $scope.ChatFAQ = [];

    $scope.SearchText = "";  

    $scope.GetChatFAQ = function () {      
        $http.get(SiteUrl + "/login/GetChatFAQ", { SearchText: $scope.SearchText, LN: $scope._language} ).
            success(function (data, status) {
                $scope.ChatFAQ = JSON.stringify(data.DataList);
            }).
            error(function (data1, status) {
                $scope.ChatFAQ
            });
    }

});


ChatFAQ.directive('myEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.myEnter);
                });
                event.preventDefault();            }
        });
    };
});
