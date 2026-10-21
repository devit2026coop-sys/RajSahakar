
var MPRArticlebController = angular.module('MPRArticle25bFormCnt', ['angularUtils.directives.dirPagination']);

MPRArticlebController.controller("MPRArticle25bFormMod", function ($rootScope, $scope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 5,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }

    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $scope.pageChangeHandler = function (num) {
    };
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);
            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {
            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.SearchData();
    }
    /*------------End Paging and Filter-----------------*/

    $scope.FinancialYearList = [];
    //$scope.DistrictList = [];
    $scope.MonthList = [];
    $scope.EditId = 0;

    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };

    $scope.GetMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };

    $scope.clean = function () {
        $scope.PyActive = 0;
        $scope.PyInactive = 0;
        $scope.PyUnderLiquidation = 0;
        $scope.PyTotal = 0;
        $scope.CmgmActive = 0;
        $scope.CmgmInactive = 0;
        $scope.CmgmUnderLiquidation = 0;
        $scope.CmTotal = 0;
        $scope.EmgmActive = 0;
        $scope.EmgmInactive = 0;
        $scope.EmgmUnderLiquidation = 0;
        $scope.EmTotal = 0;
    }

    $scope.GetMPRArticle25bList = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle25bListDetails").success(function (data) {
            //console.log(data, "MPRArticle25bList");
            $scope.clean();
            $scope.MPRArticle25bList = JSON.parse(data.Result.Data.MPRArticle25bList);
            $scope.MPRArticle25bList.forEach(function (item, indx) {
                $scope.PyActive += parseInt(item.PyActive);
                $scope.PyInactive += parseInt(item.PyInactive);
                $scope.PyUnderLiquidation += parseInt(item.PyUnderLiquidation);
                $scope.PyTotal += parseInt(item.PyTotal);
                $scope.CmgmActive += parseInt(item.CmgmActive);
                $scope.CmgmInactive += parseInt(item.CmgmInactive);
                $scope.CmgmUnderLiquidation += parseInt(item.CmgmUnderLiquidation);
                $scope.CmTotal += parseInt(item.CmTotal);
                $scope.EmgmActive += parseInt(item.EmgmActive);
                $scope.EmgmInactive += parseInt(item.EmgmInactive);
                $scope.EmgmUnderLiquidation += parseInt(item.EmgmUnderLiquidation);
                $scope.EmTotal += parseInt(item.EmTotal);
            })
        }).error(function () {
            alert("Error");
        });
    };

    $scope.SaveArticle = function () {
        //debugger;
        if (!$scope.validatdataSubmit()) {
            return false;
        }
        else {
            var formData = new FormData();
            formData.append("Id", $scope.EditId);
            formData.append("FinYearId", $scope.MPRArticlebData.FinYearId);
            formData.append("MonthId", $scope.MPRArticlebData.MonthId);
            formData.append("PyActive", $scope.MPRArticlebData.PyActive == undefined ? null : $scope.MPRArticlebData.PyActive);
            formData.append("PyInactive", $scope.MPRArticlebData.PyInactive == undefined ? null : $scope.MPRArticlebData.PyInactive);
            formData.append("PyUnderLiquidation", $scope.MPRArticlebData.PyUnderLiquidation == undefined ? null : $scope.MPRArticlebData.PyUnderLiquidation);
            formData.append("CmgmActive", $scope.MPRArticlebData.CmgmActive == undefined ? null : $scope.MPRArticlebData.CmgmActive);
            formData.append("CmgmInactive", $scope.MPRArticlebData.CmgmInactive == undefined ? null : $scope.MPRArticlebData.CmgmInactive);
            formData.append("CmgmUnderLiquidation", $scope.MPRArticlebData.CmgmUnderLiquidation == undefined ? null : $scope.MPRArticlebData.CmgmUnderLiquidation);
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/MPR/SaveMPRArticle25bForm", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");

                    if (parseInt(data, 10) == 1) {
                        var Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                        alert(Msg);
                        $scope.GetMPRArticle25bList();
                        $scope.MPRArticlebData = {};
                        $scope.EditId = 0;
                    }
                    else if (parseInt(data, 10) == 2) {
                        var Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट हो गया।');
                        alert(Msg);
                        $scope.GetMPRArticle25bList();
                        $scope.MPRArticlebData = {};
                    }
                    else if (parseInt(data, 10) == 0) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                        alert(Msg);
                        $scope.MPRArticlebData = {};
                    }
                    else if (parseInt(data, 10) == -1) {
                        var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                        alert(Msg);
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while auditor assignment insert");

                });
        }
    };

    $scope.validatdataSubmit = function () {

        var message = $scope._language == 'en-us' ? 'निम्नलिखित की पुष्टि करें : ' : 'निम्नलिखित की पुष्टि करें:';
        if ($scope.MPRArticlebData.FinYearId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया वित्तीय वर्ष चुनें' : '\n <br /> कृपया वित्तीय वर्ष चुनें |';
        }
        if ($scope.MPRArticlebData.MonthId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह चुनें' : '\n <br /> कृपया माह चुनें |';
        }
        if ($scope.MPRArticlebData.PyActive == undefined || $scope.MPRArticlebData.PyActive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत वर्ष तक साधारण सभा आयोजन से शेष सक्रिय सोसायटियों की संख्या डालें ' : '\n <br /> कृपया गत वर्ष तक साधारण सभा आयोजन से शेष सक्रिय सोसायटियों की संख्या डालें';
        }
        if ($scope.MPRArticlebData.PyInactive == undefined || $scope.MPRArticlebData.PyInactive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत वर्ष तक साधारण सभा आयोजन से शेष निष्क्रिय सोसायटियों की संख्या डालें ' : '\n <br /> कृपया गत वर्ष तक साधारण सभा आयोजन से शेष निष्क्रिय सोसायटियों की संख्या डालें|';
        }
        if ($scope.MPRArticlebData.PyUnderLiquidation == undefined || $scope.MPRArticlebData.PyUnderLiquidation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत वर्ष तक साधारण सभा आयोजन से शेष परिसमापनाधीन सोसायटियों की संख्या डालें ' : '\n <br /> कृपया गत वर्ष तक साधारण सभा आयोजन से शेष परिसमापनाधीन सोसायटियों की संख्या डालें';
        }
        if ($scope.MPRArticlebData.CmgmActive == undefined || $scope.MPRArticlebData.CmgmActive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह में आयोजित साधारण सभाओं की सक्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया माह में आयोजित साधारण सभाओं की सक्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticlebData.CmgmInactive == undefined || $scope.MPRArticlebData.CmgmInactive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह में आयोजित साधारण सभाओं की निष्क्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया माह में आयोजित साधारण सभाओं की निष्क्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticlebData.CmgmUnderLiquidation == undefined || $scope.MPRArticlebData.CmgmUnderLiquidation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह में आयोजित साधारण सभाओं की परिसमापनाधीन सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया माह में आयोजित साधारण सभाओं की परिसमापनाधीन सोसाइटियों की संख्या डालें';
        }

        if (message.length > 30) {
            ShowModelAlert("गत वर्ष तक की साधारण सभा आयोजन से शेष सोसायटियों का विवरण", message);
            return false;
        }
        return true;
    };

    $scope.EditRow = function (item) {
        //debugger;
        $scope.EditId = item.Id;
        $scope.MPRArticlebData.FinYearId = item.FinYearId.toString();
        $scope.MPRArticlebData.MonthId = item.MonthId.toString();
        $scope.MPRArticlebData.PyActive = item.PyActive;
        $scope.MPRArticlebData.PyInactive = item.PyInactive;
        $scope.MPRArticlebData.PyUnderLiquidation = item.PyUnderLiquidation;
        $scope.MPRArticlebData.CmgmActive = item.CmgmActive;
        $scope.MPRArticlebData.CmgmInactive = item.CmgmInactive;
        $scope.MPRArticlebData.CmgmUnderLiquidation = item.CmgmUnderLiquidation;
        window.scrollTo(0, 0);
    };

    $scope.OnPageLoad = function () {
        $scope.MPRArticlebData = {};

        $scope._language = _language;
        if (_language !== 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $scope.GetFinancialYearList();
        $scope.GetMonthList();
        $scope.GetMPRArticle25bList();
    };


    $scope.OnPageLoad();

});