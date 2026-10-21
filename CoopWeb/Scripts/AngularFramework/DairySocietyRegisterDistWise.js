var SocietyRegistrationModule = angular.module("SocietyRegisterDistWiseMod", ['angularUtils.directives.dirPagination']);
SocietyRegistrationModule.controller("SocietyRegisterDistWiseCnt", function ($rootScope, $scope, $http, $location) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 1000,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
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


        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
    }
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
        if ($scope.Filter.FilterType == 'created_dated') {
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
        $scope.GetWarehouseList();
    }
    /*------------End Paging and Filter-----------------*/



    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 1000;
    $scope.ActId = '3';
    $scope.SocietyTypeId = '10';
    $scope.DIVISION_ID = '0';


    $scope.Reset = function () {
        $scope.ActId = "3";
        $scope.SocietyTypeId = "10";
        $scope.DIVISION_ID = "0";

        $("#isOnline").prop("checked", false);
        $("#isOffline").prop("checked", false);
        $scope.GetSocietyData();
    }


    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");    
        $scope._language = _language; // Set LAnguiage
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
            //$(".se-pre-con").fadeOut("slow");
        }
        $('#txtCreatetionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $scope.Divisions = [{}];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.Divisions = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Divisions = [];
        //    });
        $http.post(SiteUrl + "/MIS/GetUserLevelId").
            success(function (data, status) {
                if (data.length > 0 && data != 0) {
                    if (data == 1)
                        $scope.IsState = true;
                }
            }).
            error(function (data, status) {
            });

    

        $scope.GetSocietyData();
    };

    $scope.Actlist = [];
    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                data.forEach(function (item, index) {
                    //if ($("#hdnActId").val().split(',').indexOf(item.ActId) > -1) {
                    //    $scope.Actlist.push(item);
                    //}
                    $scope.Actlist.push(item);
                });
                //if ($("#hdnActId").val().split(',').length == 1) {
                //    $scope.ActId = $("#hdnActId").val();
                    
                //    $scope.GetSocietyTypeList();
                //    $("#ddrActId").attr("disabled", "disabled");
                //}
                $scope.ActId = '3'; //4 dec 2024
                $scope.GetSocietyTypeList();//4 dec 2024
                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };
    $scope.SocietyView = function (SocietyIdEnc) {
        window.location = ' /Society/DairySocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_MIS_SocietyRegisterDistWise~0';
    }

    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $scope.Typelist = [];
        $scope.SocietyTypeId = '0';
        if ($scope.ActId != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.ActId).
                success(function (data, status) {
                    $scope.Typelist = data;
                    $scope.SocietyTypeId = '10';
                    
                }).
                error(function (data, status) {
                    $scope.Typelist = [];
                });
        }
        
    };
    //END  

    $scope.IsOnline = '-1';
    $scope.GetSocietyData = function () {

        $scope.value = '';

        debugger;
        if ($scope.ActId == '0' || $scope.ActId == 0 || $scope.ActId == undefined) {
            var Temp = window.location.href.split('?');
            if (Temp.length > 1) {
                var Temp2 = Temp[1].split('=');
                $scope.value = Temp2[1].replace(/\%20/g, ' ');
            }
        }
        $scope.SocietyList = [];
        $scope.SocietyCombine = [];
        $scope.DivShowCount = true; $scope.DivShowDetail = false;
        $(".se-pre-con").fadeIn("slow");
        $http.post("../MIS/GetSocietyDistWiseData", {
            "ActId": $scope.ActId == "0" ? $("#hdnActId").val() : $scope.ActId,
            "SocietyTypeId": $scope.SocietyTypeId, "DIVISION_ID": $scope.DIVISION_ID, "IsOnline": $scope.IsOnline,
            "DistrictId": $scope.DISTRICT_ID, "Status": $scope.value
        }).
            success(function (data, status) {
                if (data.length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    $(".se-pre-con").fadeOut("slow");
                    return;

                }
                else {
                    $scope.SocietyCombine = data.Table;
                    $(".se-pre-con").fadeOut("slow");
                    // alert(JSON.stringify($scope.SocietyCombine));
                }

            }).
            error(function (data, status) {
                alert('Error while fetching society district wise data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };




    $scope.CurrentDistId = 0;
    $scope.CurrentStatusId = 0;

    $scope.DetailsReset = function () {
        $("#txtFromDate").val("");
        $("#txtToDate").val("");
        $scope.ShowSearchDetail();
    }


    $scope.GetDetail = function (DistId, StatusId) {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: 1000,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: []
        }
        $("#txtFromDate").val("");
        $("#txtToDate").val("");
        $scope.ShowDetail(DistId, StatusId)

    }


    $scope.ShowSearchDetail = function () {
        $scope.ShowDetail($scope.CurrentDistId, $scope.CurrentStatusId)
    }

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.DISTRICT_ID = $("#hdnDistrictId").val()
                    $("#ddrDistrictId").attr("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();


    $scope.ShowDetail = function (DistId, StatusId) {
        $scope.CurrentDistId = DistId;
        $scope.CurrentStatusId = StatusId;

        $scope.SocietyList = [];
        $scope.q = "";
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetSocietyReportData", JSON.stringify({ Details: jsonParameters, "DistrictId": DistId, "StatusId": StatusId, "ActId": $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId, "IsOnline": $scope.IsOnline })).
            success(function (data, status) {
                $scope.DivShowCount = false;
                $scope.DivShowDetail = true;
                $(".se-pre-con").fadeOut("slow");
                if (JSON.parse(data.DataList).length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                    $scope.SocietyList = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                }

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching society data.');
                $(".se-pre-con").fadeOut("slow");
            });
        $scope.DivShowDetail = true;
    };

    $scope.BackDetails = function () {
        $scope.DivShowCount = true;
        $scope.DivShowDetail = false;

    }
    // START : Call Functions on Page Load
    $scope.GetSocietyActList();
    // $scope.GetSocietyTypeList();
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});

