

var AppForRajSewadwaarMasterdata = angular.module("modRajSewadwaar", []);

AppForRajSewadwaarMasterdata.controller("contRajSewadwaar", function ($scope, $http) {

    // global variables
    $scope.Divisions = [];
    $scope.Districts = [];
    $scope.Tehsils = [];
    $scope.GP = [];
    $scope.Villages = [];
    $scope.Wards = [];
    $scope.Titles = [];


    // Get List of Titles
    $scope.TitleList = function () {
        $scope.Titles = [];
        var _Result = JSON.parse([{ "Text": "1", "Value": "Mr." }, { "Text": "2", "Value": "Mrs." }, { "Text": "3", "Value": "Mrs." }]);
        var _data = [{ "Text": "-- Select Title --", "Value": "0" }];
        _Result.forEach(function (j) { _data.push({ "Text": j.Text, "Value": j.Value }); });
        $scope.Titles = _data;
    };

    // Get List of countries
    $scope.DivisionList = function () {
        $scope.Divisions = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=division&GeographyCode=-1").
            success(function (data, status) {
                var _Result = JSON.parse(data);
                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Division --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.DIVISION_ENG, "Value": j.DIVISION_ID }); });
                    $scope.Divisions = _data;
                }
                else {
                    var _data = [{ "Text": "-- संभाग चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.DIVISION_MANGAL, "Value": j.DIVISION_ID }); });
                    $scope.Divisions = _data;
                }

            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
    };

    $scope.DivisionList();

    $scope.GetDistrict = function () {
        $scope.Districts = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=district&GeographyCode=" + $scope.ddlDivisions).
            success(function (data, status) {
                var _Result = JSON.parse(data);

                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select District --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.DISTRICT_ENG, "Value": j.DISTRICT_ID }); });
                    $scope.Districts = _data;
                }
                else {
                    var _data = [{ "Text": "-- जिला चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.DISTRICT_MANGAL, "Value": j.DISTRICT_ID }); });
                    $scope.Districts = _data;
                }

            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
    }

    $scope.GetPS = function () {
        $scope.PanchayatSamitis = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=PANCHAYATSAMITY&GeographyCode=" + $scope.ddlDistrict.Value).
            success(function (data, status) {
                var _Result = JSON.parse(data);

                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Tehsil --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.BLOCK_ENG, "Value": j.BLOCK_ID }); });
                    $scope.PanchayatSamitis = _data;
                    
                }
                else {
                    var _data = [{ "Text": "-- तहसील चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.BLOCK_MANGAL, "Value": j.BLOCK_ID }); });
                    $scope.PanchayatSamitis = _data;
                }

            }).
            error(function (data, status) {
                $scope.PanchayatSamiti = [];
            });
    }

    $scope.GetGP = function () {
        $scope.GramPanchayats = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=GRAMPANCHAYAT&GeographyCode=" + $scope.ddlPS.Value).
            success(function (data, status) {
                var _Result = JSON.parse(data);


                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Panchayat Samiti --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.GP_ENG, "Value": j.GP_ID }); });
                    $scope.GramPanchayats = _data;
                }
                else {
                    var _data = [{ "Text": "-- पंचायत समिति चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.GP_MANGAL, "Value": j.GP_ID }); });
                    $scope.GramPanchayats = _data;
                }
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
            });
    }

    $scope.GetVillage = function () {
        $scope.Villages = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=village&GeographyCode=" + $scope.ddlGP.Value).
            success(function (data, status) {
                var _Result = JSON.parse(data);
                //_Result = _Result.filter(function (s) {
                //    return s.GP == $scope.ddlPS.Value;
                //});

                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Village --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.VILLAGE_ENG, "Value": j.VILLAGE_ID }); });
                    $scope.Villages = _data;
                }
                else {
                    var _data = [{ "Text": "-- गाँव चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.VILLAGE_MANGAL, "Value": j.VILLAGE_ID }); });
                    $scope.Villages = _data;
                }

            }).
            error(function (data, status) {
                $scope.Villages = [];
            });
    }

    $scope.GetCity = function () {
        $scope.Cities = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=city&GeographyCode=" + $scope.ddlDistrict.Value).
            success(function (data, status) {
                var _Result = JSON.parse(data);

                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Village --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.CITY_ENG, "Value": j.CITY_ID }); });
                    $scope.Cities = _data;
                }
                else {
                    var _data = [{ "Text": "-- गाँव चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.CITY_MANGAL, "Value": j.CITY_ID }); });
                    $scope.Cities = _data;
                }

            }).
            error(function (data, status) {
                $scope.Cities = [];
            });
    }


    $scope.GetWard = function () {
        $scope.Wards = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=ward&GeographyCode=" + $scope.ddlCity.Value).
            success(function (data, status) {
                var _Result = JSON.parse(data);

                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Village --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.WARD_ENG, "Value": j.WARD_ID }); });
                    $scope.Wards = _data;
                }
                else {
                    var _data = [{ "Text": "-- गाँव चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.WARD_MANGAL, "Value": j.WARD_ID }); });
                    $scope.Wards = _data;
                }

            }).
            error(function (data, status) {
                $scope.Wards = [];
            });
    }

    $scope.GetTehsil = function () {
        $scope.Tehsils = [];
        $http.get(SiteUrl+"/RajSewadwaar/GeographicalDetails/?GeographyType=tehsil&GeographyCode=" + $scope.ddlDistrict.Value).
            success(function (data, status) {
                var _Result = JSON.parse(data);

                if (_language == 'en-us') {
                    var _data = [{ "Text": "-- Select Tehsil --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.TEHSIL_ENG, "Value": j.TEHSIL_ID }); });
                    $scope.Tehsils = _data;
                }
                else {
                    var _data = [{ "Text": "-- तहसील चुनें --", "Value": "0" }];
                    _Result.forEach(function (j) { _data.push({ "Text": j.TEHSIL_MANGAL, "Value": j.TEHSIL_ID }); });
                    $scope.Tehsils = _data;
                }

            }).
            error(function (data, status) {
                $scope.Tehsils = [];
            });
    }
});