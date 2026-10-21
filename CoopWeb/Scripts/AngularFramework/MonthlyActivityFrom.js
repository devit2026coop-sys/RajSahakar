var AppClasses = angular.module("MafMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("MafCnt", function ($scope, $http, $rootScope) {

    $scope._language = 'en-us';
    $scope._language = _language;
    $scope.StateList = [];
    $scope.LevelList = [];
    $scope.DistrictList = [];
    $scope.DivisionList = [];

    if (_language == 'en-us') {
        $http.get("../Translations/Translation_EN.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
    }
    else {
        $http.get("../Translations/Translation_HI.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
    }

    $scope.GetStateList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=STATE&MasterCode=0").
            success(function (data, status) {
                $scope.StateList = data;
            }).
            error(function (data, status) {
                $scope.StateList = [];
            });
    };

    $scope.GetDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=0").
            success(function (data, status) {
                $scope.DivisionList = data;
            }).
            error(function (data, status) {
                $scope.DivisionList = [];
            });
    };
    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.mafObj.DIVISION_ID).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };


    $scope.GetLevelList = function () {
        $scope.LevelList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS&MasterCode=0").
            success(function (data, status) {
                $scope.LevelList = data.filter(x => (["1", "3", "6"]).includes(x.LevelId));
            }).
            error(function (data, status) {
                $scope.LevelList = [];
            });
    };
    $scope.GetDivisionList = function () {
        $scope.DivisionList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=0").
            success(function (data, status) {
                $scope.DivisionList = data;
            }).
            error(function (data, status) {
                $scope.DivisionList = [];
            });
    };
    $scope.ChangeLevel = function () {
        if ($scope.mafObj.LEVEL_ID == 3)
            $scope.GetDivisionList();
    };

    $scope.mafObj = {};
    $scope.SaveActivityReport = function () {
        $scope.mafObj.PhotoFilePath = $('#Photographs').val();
        $scope.mafObj.AudioVideoFilePath = $('#AVContent').val();
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/MIS/SaveMonthlyActivity", $scope.mafObj).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result.Message == 'Success')
                    alert("Event/Activity submitted successfully.");
                $scope.ResetControls();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.ResetControls = function () {
        $scope.mafObj = {};
        $scope.mafObj.STATE_ID = "6";
        $scope.mafObj.DISTRICT_ID = "0";
        $('#Photographs,#AVContent,input[type="file"]').val('');
    }

    $scope.FileUploader = function (file, e) {
        var formData = new FormData();
        formData.append("file", file[0], file[0].name);
        $(".se-pre-con").fadeIn("slow");
        $.ajax({
            type: "POST",
            url: SiteUrl + "/FileUploaders/CommonFileUploader",
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $('#' + e).val(data.filePath);
            },
            error: function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            }
        });
    }
    $scope.GetStateList();
    $scope.GetLevelList();

});