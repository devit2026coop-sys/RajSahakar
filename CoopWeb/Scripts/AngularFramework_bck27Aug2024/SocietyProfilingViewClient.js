var SocietyProfilingModule = angular.module("SocietyProfilingMod", []);
SocietyProfilingModule.controller("SocietyProfilingCnt", function ($scope, $http) {

    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language; // Set LAnguiage
        if (_language == 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }


        // GET Society Details
        $http.get(SiteUrl+"/Society/GetSocietyProfilingForSocietyDetails").
            success(function (data, status) {
                $scope.SocietyDetails = data[0];
            }).
            error(function (data, status) {
                $scope.SocietyDetails = [];
            });

        // GET Personnel Details
        $http.get(SiteUrl+"/Society/GetSocietyProfilingForPersonnelDetails").
            success(function (data, status) {
                $scope.SocietyPersonnelDetails = data;
            }).
            error(function (data, status) {
                $scope.SocietyPersonnelDetails = [];
            });

        // GET Office Details
        $http.get(SiteUrl+"/Society/GetSocietyProfilingForSocietyOfficeDetails").
            success(function (data, status) {
                $scope.SocietyOfficeDetails = data;
            }).
            error(function (data, status) {
                $scope.SocietyOfficeDetails = [];
            });

        // GET Members Details
        $http.get(SiteUrl+"/Society/GetSocietyProfilingForSocietyMembersDetails").
            success(function (data, status) {
                $scope.SocietyMembersDetails = data;
            }).
            error(function (data, status) {
                $scope.SocietyMembersDetails = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load
    

    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});

