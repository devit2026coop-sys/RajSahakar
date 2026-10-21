
var MPRArticleController = angular.module('MPRArticle30FormCnt', ['angularUtils.directives.dirPagination']);

MPRArticleController.controller("MPRArticle30FormMod", function ($rootScope, $scope, $http) {
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
        $scope.InYearPendingCase = 0;
        $scope.LastMonthCaseReceived = 0;
        $scope.InMonthCaseReceived = 0;
        $scope.TotalReceived = 0;
        $scope.LastMonthCaseDisposed = 0;
        $scope.InMonthCaseDisposed = 0;
        $scope.TotalDisposed = 0;
        $scope.TotalPending = 0;
    }

    $scope.GetMPRArticle30List = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle30ListDetails").success(function (data) {
            //console.log(data, "MPRArticle30List");
            $scope.clean();
            $scope.MPRArticle30List = JSON.parse(data.Result.Data.MPRArticle30List);
            $scope.MPRArticle30List.forEach(function (item, indx) {
                $scope.InYearPendingCase += parseInt(item.InYearPendingCase);
                $scope.LastMonthCaseReceived += parseInt(item.LastMonthCaseReceived);
                $scope.InMonthCaseReceived += parseInt(item.InMonthCaseReceived);
                $scope.TotalReceived += parseInt(item.TotalReceived);
                $scope.LastMonthCaseDisposed += parseInt(item.LastMonthCaseDisposed);
                $scope.InMonthCaseDisposed += parseInt(item.InMonthCaseDisposed);
                $scope.TotalDisposed += parseInt(item.TotalDisposed);
                $scope.TotalPending += parseInt(item.TotalPending);
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
            formData.append("InYearPendingCase", $scope.MPRArticleData.InYearPendingCase == undefined ? null : $scope.MPRArticleData.InYearPendingCase);
            formData.append("LastMonthCaseReceived", $scope.MPRArticleData.LastMonthCaseReceived == undefined ? null : $scope.MPRArticleData.LastMonthCaseReceived);
            formData.append("InMonthCaseReceived", $scope.MPRArticleData.InMonthCaseReceived == undefined ? null : $scope.MPRArticleData.InMonthCaseReceived);
            formData.append("LastMonthCaseDisposed", $scope.MPRArticleData.LastMonthCaseDisposed == undefined ? null : $scope.MPRArticleData.LastMonthCaseDisposed);
            formData.append("InMonthCaseDisposed", $scope.MPRArticleData.InMonthCaseDisposed == undefined ? null : $scope.MPRArticleData.InMonthCaseDisposed);
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/MPR/SaveMPRArticle30Form", formData,
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
                        $scope.GetMPRArticle30List();
                        $scope.MPRArticleData = {};
                        $scope.EditId = 0;
                    }
                    else if (parseInt(data, 10) == 2) {
                        var Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट हो गया।');
                        alert(Msg);
                        $scope.GetMPRArticle30List();
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
        if ($scope.MPRArticleData.InYearPendingCase == undefined || $scope.MPRArticleData.InYearPendingCase == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया वर्ष के प्रारंभ में बकाया रहे प्रकरण की संख्या दर्ज करें ' : '\n <br /> कृपया वर्ष के प्रारंभ में बकाया रहे प्रकरण की संख्या दर्ज करें';
        }
        if ($scope.MPRArticleData.LastMonthCaseReceived == undefined || $scope.MPRArticleData.LastMonthCaseReceived == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत माह तक प्राप्त प्रकरण की संख्या दर्ज करें ' : '\n <br /> कृपया गत माह तक प्राप्त प्रकरण की संख्या दर्ज करें|';
        }
        if ($scope.MPRArticleData.InMonthCaseReceived == undefined || $scope.MPRArticleData.InMonthCaseReceived == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह में प्राप्त प्रकरण की संख्या दर्ज करें ' : '\n <br /> कृपया माह में प्राप्त प्रकरण की संख्या दर्ज करें';
        }
        if ($scope.MPRArticleData.LastMonthCaseDisposed == undefined || $scope.MPRArticleData.LastMonthCaseDisposed == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया गत माह तक निस्तारित प्रकरण की संख्या दर्ज करें ' : '\n <br /> कृपया गत माह तक निस्तारित प्रकरण की संख्या दर्ज करें';
        }
        if ($scope.MPRArticleData.InMonthCaseDisposed == undefined || $scope.MPRArticleData.InMonthCaseDisposed == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया माह में निस्तारित प्रकरण की संख्या दर्ज करें ' : '\n <br /> कृपया माह में निस्तारित प्रकरण की संख्या दर्ज करें';
        }

        if (message.length > 30) {
            ShowModelAlert("धारा 30 के अंतर्गत दर्ज प्रकरणों की प्रगति का खण्डवार विवरण", message);
            return false;
        }
        return true;
    };

    $scope.EditRow = function (item) {
        //debugger;
        $scope.EditId = item.Id;
        $scope.MPRArticleData.FinYearId = item.FinYearId.toString();
        $scope.MPRArticleData.MonthId = item.MonthId.toString();
        $scope.MPRArticleData.InYearPendingCase = item.InYearPendingCase;
        $scope.MPRArticleData.LastMonthCaseReceived = item.LastMonthCaseReceived;
        $scope.MPRArticleData.InMonthCaseReceived = item.InMonthCaseReceived;
        $scope.MPRArticleData.LastMonthCaseDisposed = item.LastMonthCaseDisposed;
        $scope.MPRArticleData.InMonthCaseDisposed = item.InMonthCaseDisposed;
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
        $scope.GetMonthList();
        $scope.GetMPRArticle30List();
    };


    $scope.OnPageLoad();

});