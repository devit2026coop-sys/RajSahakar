
var MasterMod = angular.module('directMod', ['angularUtils.directives.dirPagination']);
MasterMod.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);
MasterMod.directive('uppercaseOnly', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            ngModelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == null) return '';

                var transformedInput = inputValue.toUpperCase();

                if (transformedInput !== inputValue) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            });
        }
    };
});
MasterMod.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {

            ngModel.$parsers.push(function (inputValue) {
                if (!inputValue) return '';

                var transformedInput = inputValue.replace(/[^0-9]/g, '');
                if (transformedInput !== inputValue) {
                    ngModel.$setViewValue(transformedInput);
                    ngModel.$render();
                }
                return transformedInput;
            });
        }
    };
});
MasterMod.directive('inputTransform', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            inputTransform: '='
        },
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (value) {
                if (!value || !scope.inputTransform) return value;

                let newValue = value;

                if (scope.inputTransform === 1) {
                    newValue = value.replace(/[^0-9]/g, '');
                } else {
                    newValue = value.toUpperCase();
                }
                if (newValue !== value) {
                    ngModel.$setViewValue(newValue);
                    ngModel.$render();
                }
                return newValue;
            });
        }
    };
});
MasterMod.directive('disableBeforeDate', function () {
    return {
        restrict: 'A',
        link: function (scope, element) {
            const today = new Date().toISOString().slice(0, 10);
            scope.$evalAsync(() => {
                element.attr('max', today);
            });
            scope.$watch(() => element.attr('max'), function () {
                element.attr('max', today);
            });
            element.on('keydown', function (e) {
                e.preventDefault();
            });
            element.on('paste drop', function (e) {
                e.preventDefault();
            });
        }
    };
});
MasterMod.directive("mobileNumber", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModel) {
            element.on("input", function () {
                let val = element.val();
                val = val.replace(/[^0-9]/g, "");
                if (val.length > 10) val = val.substring(0, 10);

                element.val(val);
                ngModel.$setViewValue(val);
                ngModel.$render();
            });
            ngModel.$validators.mobileNumber = function (value) {
                if (!value) return false;
                const validFormat = /^[6-9]\d{9}$/.test(value);
                const notRepeated = !/^(\d)\1{9}$/.test(value);
                return validFormat && notRepeated;
            };
            element.on("blur", function () {
                ngModel.$setTouched();
                scope.$apply();
            });
        }
    };
});

MasterMod.controller("directCtrl", function ($scope, $rootScope, $http, $location) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.districtDataList = [];
    $scope.DirectorDataList = [];
    $scope.isNoRecord = false;
    $scope.SectorDataList = [];
    $scope.NcdDesignationsList = [];
    $scope.invalidFields = {};
    $scope.documentTypeList = [
        { id: 1, name: 'Aadhaar Card' },
        { id: 2, name: 'PAN Card' },
        { id: 3, name: 'Driving License' },
        { id: 4, name: 'Voter ID' }];



    var params = new URLSearchParams(window.location.search);
    $scope.search = {
        cooperativeId: params.get("cooperativeId") || '',
        dist_code: params.get("districtcode") || "0",
        sector_code: params.get("sectorcode") || "0",
        gender: "1",
        is_any_other_cooperative: "0"
    }
    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $scope.getDistrictDetailByState();
        $scope.getDirectorDesignation();
        $scope.getSectorDetail();
        if ($scope.search.cooperativeId != null || $scope.search.cooperativeId != '') {
            $scope.getDirectorDetailByCooperative($scope.search.cooperativeId);
        }
    };
    $scope.getDistrictDetailByState = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).success(function (data, status) {
            if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                $scope.districtDataList = JSON.parse(data.Result.Data.NCDDistrictDetail);
                if ($scope.districtDataList != null && $scope.districtDataList.length > 0) {
                    if ($scope.search.dist_code != null && $scope.search.dist_code > 0) {
                        $scope.search.dist_code = $scope.districtDataList.filter(x => x.district_code == $scope.search.dist_code)[0].district_code;
                    }
                }
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $scope.districtDataList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.getSectorDetail = function () {
        $scope.SectorDataList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNCDSectorDetails").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDSectorDetail != null) {
                    $scope.SectorDataList = JSON.parse(data.Result.Data.NCDSectorDetail);
                    if ($scope.SectorDataList != null && $scope.SectorDataList.length > 0) {
                        if ($scope.search.sector_code != null && $scope.search.sector_code > 0) {
                            $scope.search.sector_code = $scope.SectorDataList.filter(x => x.id == $scope.search.sector_code)[0].id;
                        }
                    }
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $scope.SectorDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.getDirectorDesignation = function () {
        $scope.NcdDesignationsList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNcdDesignations")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.NcdDesignationDetail) {
                        $scope.NcdDesignationsList = JSON.parse(response.data.Result.Data.NcdDesignationDetail);
                    }
                } else {
                    console.error("Error fetching designations:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching designations:", error);
            });
    };

    $scope.SaveDirectorData = function () {
        $scope.invalidFields = {};
        $scope.search.is_any_other_cooperative = $scope.search.is_any_other_cooperative ?? 0;
        $scope.search.other_cooperative_society_name = $scope.search.is_any_other_cooperative == 0 ? null : $scope.search.other_cooperative_society_name;

        if (!$scope.search.bod_Member_name || $scope.search.bod_Member_name.trim() === "") {
            $scope.invalidFields.bod_Member_name = true; toastr.error("Please enter the name of the director.");
            return;
        }
        if (!$scope.search.mobile_number) {
            $scope.invalidFields.mobile_number = true;
            toastr.error("Please enter a valid mobile number; repeated digits or invalid numbers are not allowed!");
            return;
        }
        if ($scope.search.id_proof_type == 1 && (!$scope.search.id_proof_no || $scope.search.id_proof_no.length !== 12)) {
            $scope.invalidFields.id_proof_no = true;
            toastr.error("Please enter a valid 12-digit Aadhaar number.");
            return;
        }
        if (!$scope.search.id_proof_type || $scope.search.id_proof_type == "0") {
            $scope.invalidFields.id_proof_type = true;
            toastr.error("Please select document type.");
            return;
        }
        if (!$scope.search.bod_designation || $scope.search.bod_designation == "0") {
            $scope.invalidFields.bod_designation = true;
            toastr.error("Please select designation.");
            return;
        }
        if ($scope.search.is_any_other_cooperative == 1) {
            if (!$scope.search.other_cooperative_society_name || $scope.search.other_cooperative_society_name.trim() === "") {
                $scope.invalidFields.other_cooperative_society_name = true;
                toastr.error("Please enter other cooperative society name.");
                return;
            }
        }
        if (!$scope.search.from_date) {
            $scope.invalidFields.from_date = true;
            toastr.error("Please select From Date.");
            return;
        }
        if (!$scope.search.to_date) {
            $scope.invalidFields.to_date = true;
            toastr.error("Please select To Date.");
            return;
        }
        var fromDate = new Date($scope.search.from_date);
        var toDate = new Date($scope.search.to_date);
        if (isNaN(fromDate.getTime()) || isNaN(toDate.getTime())) {
            $scope.invalidFields.from_date = true;
            $scope.invalidFields.to_date = true;
            toastr.error("Invalid date selected.");
            return;
        }
        if (fromDate >= toDate) {
            $scope.invalidFields.from_date = true;
            $scope.invalidFields.to_date = true;
            toastr.error("From Date cannot be greater than To Date.");
            return;
        }

        $scope.search.from_date = $scope.convertToUTC($scope.search.from_date);
        $scope.search.to_date = $scope.convertToUTC($scope.search.to_date);
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/NcdData/SaveDirectorData", { jsonData: JSON.stringify($scope.search) }).then(function (res) {
            $(".se-pre-con").fadeOut("slow");
            if (res.data && res.data.Result && res.data.Result.Status === "200") {
                if (res.data.Result.Data.directorData !== null) {
                    $scope.directorData = JSON.parse(res.data.Result.Data.directorData);
                    if ($scope.directorData[0].Message !== '' && $scope.directorData[0].Status == 200) {
                        toastr.success($scope.directorData[0].Message);
                        $scope.resetForm();
                        $scope.getDirectorDetailByCooperative($scope.search.cooperativeId);
                    }
                    else {
                        toastr.error("Some error occurred.");
                    }
                }
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (err) {
            $scope.saving = false;
            console.error("HTTP error:", err);
            alert("HTTP error while saving. See console.");
            $(".se-pre-con").fadeOut("slow");
        });
    }

    $scope.getDirectorDetailByCooperative = function (cooperative) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/NcdData/GetBoardofDirectorByCooperativeId?cooperativeId=" + cooperative).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.Result.Data != null && data.Result.Data.directorDetails != null) {
                $scope.DirectorDataList = JSON.parse(data.Result.Data.directorDetails);
            } else {
                $scope.DirectorDataList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $scope.DirectorDataList = [];
            $scope.districtDataList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.getDirectorDetailById = function (item) {
        let filterData = $scope.DirectorDataList.filter(x => x.ncd_bod_id == item);
        if (filterData != null) {
            $scope.search = filterData[0];
            if ($scope.search.from_date != null) {
                $scope.search.from_date = new Date($scope.convertDate($scope.search.from_date));
            }
            if ($scope.search.to_date != null) {
                $scope.search.to_date = new Date($scope.convertDate($scope.search.to_date));
            }
        }
    };
    $scope.convertDate = function (dateStr) {
        if (!dateStr) return ''; let parts = dateStr.split('-'); if (parts.length !== 3) return dateStr;
        let dd = parts[2]; let mm = parts[1]; let yyyy = parts[0]; return `${yyyy}-${mm}-${dd}`;
    };
    $scope.resetForm = function () {
        $scope.search = {
            cooperativeId: params.get("cooperativeId"),
            dist_code: params.get("districtcode"),
            sector_code: params.get("sectorcode"),
            gender: "1",
            is_any_other_cooperative: "0"
        };
        $scope.getDistrictDetailByState();
        $scope.getSectorDetail();
    }

    $scope.searchFilterData = function () {
        let searchParams = new URLSearchParams();
        if ($scope.search.cooperativeId) searchParams.set('cooperativeId', $scope.search.cooperativeId);
        if ($scope.search.dist_code) searchParams.set('districtcode', $scope.search.dist_code);
        if ($scope.search.sector_code) searchParams.set('sectorcode', $scope.search.sector_code);
        window.history.replaceState(null, '', '/NcdData/NCDBoardOfDirector?' + searchParams.toString());
    };

    $scope.goBackToList = function () {
        window.location.href = '/NCDData/NcdBasicDetailsList';
    }

    $scope.getPlaceholder = function (type) {
        let placeHolderType = $scope.documentTypeList.find(item => item.id === type);
        return placeHolderType ? placeHolderType.placeholder : '';
    };

    $scope.getMaxLength = function (type) {
        let lengthType = $scope.documentTypeList.find(item => item.id === type);
        return lengthType ? lengthType.maxLength : '';
    };
    $scope.changeDocumentType = function () {
        $scope.search.id_proof_no = '';
    };

    $scope.convertToUTC = function (date) {
        return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate())
        ).toISOString();
    }

    $scope.OnPageLoad();

});

