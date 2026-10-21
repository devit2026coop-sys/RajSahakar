
var MPRArticleController = angular.module('MPRArticle25FormCnt', ['angularUtils.directives.dirPagination']);

MPRArticleController.controller("MPRArticle25FormMod", function ($rootScope, $scope, $http) {
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
                //console.log(data,"FinancialYearList")
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };

    //$scope.GetDistrictList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
    //        success(function (data, status) {
    //            $scope.DistrictList = data;    
    //            console.log(data, "DistrictList" );
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictList = [];
    //        });
    //}; 

    $scope.GetMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
                //console.log(data, "MonthList");
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };

    $scope.clean = function () { 
        $scope.TsActive = 0;
        $scope.TsInactive = 0;
        $scope.TsUnderLiquidation = 0;
        $scope.TsTotal = 0;
        $scope.PmgmActive = 0;
        $scope.PmgmInactive = 0;
        $scope.PmgmUnderLiquidation = 0;
        $scope.PmTotal = 0;
        $scope.CmgmActive = 0;
        $scope.CmgmInactive = 0;
        $scope.CmgmUnderLiquidation = 0;
        $scope.CmTotal = 0;
        $scope.EmgmActive = 0;
        $scope.EmgmInactive = 0;
        $scope.EmgmUnderLiquidation = 0;
        $scope.EmTotal = 0;
        $scope.TmgmCount = 0;
    }

    $scope.GetMPRArticle25List = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle25ListDetails").success(function (data) {
            //console.log(data, "MPRArticle25List");
            $scope.clean();
            $scope.MPRArticle25List = JSON.parse(data.Result.Data.MPRArticle25List);
            $scope.MPRArticle25List.forEach(function (item, indx) {
                $scope.TsActive += parseInt(item.TsActive);
                $scope.TsInactive += parseInt(item.TsInactive);
                $scope.TsUnderLiquidation += parseInt(item.TsUnderLiquidation);
                $scope.TsTotal += parseInt(item.TsTotal);
                $scope.PmgmActive += parseInt(item.PmgmActive);
                $scope.PmgmInactive += parseInt(item.PmgmInactive);
                $scope.PmgmUnderLiquidation += parseInt(item.PmgmUnderLiquidation);
                $scope.PmTotal += parseInt(item.PmTotal);
                $scope.CmgmActive += parseInt(item.CmgmActive);
                $scope.CmgmInactive += parseInt(item.CmgmInactive);
                $scope.CmgmUnderLiquidation += parseInt(item.CmgmUnderLiquidation);
                $scope.CmTotal += parseInt(item.CmTotal);
                $scope.EmgmActive += parseInt(item.EmgmActive);
                $scope.EmgmInactive += parseInt(item.EmgmInactive);
                $scope.EmgmUnderLiquidation += parseInt(item.EmgmUnderLiquidation);
                $scope.EmTotal += parseInt(item.EmTotal);
                $scope.TmgmCount += parseInt(item.TmgmCount);
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
            formData.append("FinYearId", $scope.MPRArticleData.FinYearId);
            formData.append("MonthId", $scope.MPRArticleData.MonthId);
            formData.append("TsActive", $scope.MPRArticleData.TsActive == undefined ? null : $scope.MPRArticleData.TsActive);
            formData.append("TsInactive", $scope.MPRArticleData.TsInactive == undefined ? null : $scope.MPRArticleData.TsInactive);
            formData.append("TsUnderLiquidation", $scope.MPRArticleData.TsUnderLiquidation == undefined ? null : $scope.MPRArticleData.TsUnderLiquidation);
            formData.append("PmgmActive", $scope.MPRArticleData.PmgmActive == undefined ? null : $scope.MPRArticleData.PmgmActive);
            formData.append("PmgmInactive", $scope.MPRArticleData.PmgmInactive == undefined ? null : $scope.MPRArticleData.PmgmInactive);
            formData.append("PmgmUnderLiquidation", $scope.MPRArticleData.PmgmUnderLiquidation == undefined ? null : $scope.MPRArticleData.PmgmUnderLiquidation);
            formData.append("CmgmActive", $scope.MPRArticleData.CmgmActive == undefined ? null : $scope.MPRArticleData.CmgmActive);
            formData.append("CmgmInactive", $scope.MPRArticleData.CmgmInactive == undefined ? null : $scope.MPRArticleData.CmgmInactive);
            formData.append("CmgmUnderLiquidation", $scope.MPRArticleData.CmgmUnderLiquidation == undefined ? null : $scope.MPRArticleData.CmgmUnderLiquidation);
            formData.append("TmgmCount", $scope.MPRArticleData.TmgmCount == undefined ? null : $scope.MPRArticleData.TmgmCount);
            formData.append("EmgmActive", $scope.MPRArticleData.EmgmActive == undefined ? null : $scope.MPRArticleData.EmgmActive);
            formData.append("EmgmInactive", $scope.MPRArticleData.EmgmInactive == undefined ? null : $scope.MPRArticleData.EmgmInactive);
            formData.append("EmgmUnderLiquidation", $scope.MPRArticleData.EmgmUnderLiquidation == undefined ? null : $scope.MPRArticleData.EmgmUnderLiquidation);
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/MPR/SaveMPRArticle25Form", formData,
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
                        $scope.GetMPRArticle25List();
                        $scope.MPRArticleData = {};
                        $scope.EditId = 0;
                    }
                    else if (parseInt(data, 10) == 2) {
                        var Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट हो गया।');
                        alert(Msg);
                        $scope.GetMPRArticle25List();
                        $scope.MPRArticleData = {};
                    }
                    else if (parseInt(data, 10) == 0) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                        alert(Msg);
                        $scope.MPRArticleData = {};
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
        if ($scope.MPRArticleData.FinYearId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया वित्तीय वर्ष चुनें' : '\n <br /> कृपया वित्तीय वर्ष चुनें |';
        }
        if ($scope.MPRArticleData.MonthId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह चुनें' : '\n <br /> कृपया माह चुनें |';
        }
        if ($scope.MPRArticleData.TsActive == undefined || $scope.MPRArticleData.TsActive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया कुल पंजीकृत सक्रिय सोसाइटियों की संख्या डालें ' : '\n <br />  कृपया कुल पंजीकृत सक्रिय सोसाइटियों की संख्या डालें|';
        }
        if ($scope.MPRArticleData.TsInactive == undefined || $scope.MPRArticleData.TsInactive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया कुल पंजीकृत निष्क्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया कुल पंजीकृत निष्क्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.TsUnderLiquidation == undefined || $scope.MPRArticleData.TsUnderLiquidation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया कुल पंजीकृत परिसमापनाधीन सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया कुल परिसमापनाधीन सक्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.PmgmActive == undefined || $scope.MPRArticleData.PmgmActive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत माह तक आयोजित की गई साधारण सभाओं की सक्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया गत माह तक आयोजित की गई साधारण सभाओं की सक्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.PmgmInactive == undefined || $scope.MPRArticleData.PmgmInactive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत माह तक आयोजित की गई साधारण सभाओं की निष्क्रिय सोसाइटियों की संख्या डालें ' : '\n <br />  कृपया गत माह तक आयोजित की गई साधारण सभाओं की निष्क्रिय सोसाइटियों की संख्या डालें|';
        }
        if ($scope.MPRArticleData.PmgmUnderLiquidation == undefined || $scope.MPRArticleData.PmgmUnderLiquidation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत माह तक आयोजित की गई साधारण सभाओं की परिसमापनाधीन सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया गत माह तक आयोजित की गई साधारण सभाओं की परिसमापनाधीन सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.CmgmActive == undefined || $scope.MPRArticleData.CmgmActive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया वर्तमान माह में आयोजित साधारण सभाओं की सक्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया वर्तमान माह में आयोजित साधारण सभाओं की सक्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.CmgmInactive == undefined || $scope.MPRArticleData.CmgmInactive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया वर्तमान माह में आयोजित साधारण सभाओं की निष्क्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया वर्तमान माह में आयोजित साधारण सभाओं की निष्क्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.CmgmUnderLiquidation == undefined || $scope.MPRArticleData.CmgmUnderLiquidation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया वर्तमान माह में आयोजित साधारण सभाओं की परिसमापनाधीन सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया वर्तमान माह में आयोजित साधारण सभाओं की परिसमापनाधीन सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.TmgmCount == undefined || $scope.MPRArticleData.TmgmCount == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह तक आयोजित साधारण सभाओं की कुल संख्या डालें ' : '\n <br /> कृपया माह तक आयोजित साधारण सभाओं की कुल संख्या डालें';
        }
        if ($scope.MPRArticleData.EmgmActive == undefined || $scope.MPRArticleData.EmgmActive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह के अंत में साधारण सभा आयोजन से शेष सक्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया माह के अंत में साधारण सभा आयोजन से शेष सक्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.EmgmInactive == undefined || $scope.MPRArticleData.EmgmInactive == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह के अंत में साधारण सभा आयोजन से शेष निष्क्रिय सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया माह के अंत में साधारण सभा आयोजन से शेष निष्क्रिय सोसाइटियों की संख्या डालें';
        }
        if ($scope.MPRArticleData.EmgmUnderLiquidation == undefined || $scope.MPRArticleData.EmgmUnderLiquidation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह के अंत में साधारण सभा आयोजन से शेष परिसमापनाधीन सोसाइटियों की संख्या डालें ' : '\n <br /> कृपया माह के अंत में साधारण सभा आयोजन से शेष परिसमापनाधीन सोसाइटियों की संख्या डालें';
        }

        if (message.length > 30) {
            ShowModelAlert("धारा 25 की प्रगति का खण्डवार विवरण", message);
            return false;
        }
        return true;
    };

    $scope.EditRow = function (item) {
        //debugger;
        $scope.EditId = item.Id;
        $scope.MPRArticleData.FinYearId = item.FinYearId.toString();
        $scope.MPRArticleData.MonthId = item.MonthId.toString();
        $scope.MPRArticleData.TsActive = item.TsActive;
        $scope.MPRArticleData.TsInactive = item.TsInactive;
        $scope.MPRArticleData.TsUnderLiquidation = item.TsUnderLiquidation;
        $scope.MPRArticleData.PmgmActive = item.PmgmActive;
        $scope.MPRArticleData.PmgmInactive = item.PmgmInactive;
        $scope.MPRArticleData.PmgmUnderLiquidation = item.PmgmUnderLiquidation;
        $scope.MPRArticleData.CmgmActive = item.CmgmActive;
        $scope.MPRArticleData.CmgmInactive = item.CmgmInactive;
        $scope.MPRArticleData.CmgmUnderLiquidation = item.CmgmUnderLiquidation;
        $scope.MPRArticleData.TmgmCount = item.TmgmCount;
        $scope.MPRArticleData.EmgmActive = item.EmgmActive;
        $scope.MPRArticleData.EmgmInactive = item.EmgmInactive;
        $scope.MPRArticleData.EmgmUnderLiquidation = item.EmgmUnderLiquidation;
        window.scrollTo(0, 0);
    };

    $scope.OnPageLoad = function () {
        $scope.MPRArticleData = {};

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
        //$scope.GetDistrictList();
        $scope.GetMonthList();
        $scope.GetMPRArticle25List();
    };


    $scope.OnPageLoad();

});