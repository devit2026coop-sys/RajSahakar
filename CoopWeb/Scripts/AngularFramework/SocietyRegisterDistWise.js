var SocietyRegistrationModule = angular.module("SocietyRegisterDistWiseMod", [
    'angularUtils.directives.dirPagination'
]);

SocietyRegistrationModule.controller("SocietyRegisterDistWiseCnt",
    function ($rootScope, $scope, $http, $location) {

        /* ================= PAGING ================= */
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: 1000,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: []
        };

        function buildSearchParams(callback) {
            $scope.DataPaging.searchParameters = [];

            var elements = $('#' + $scope.DataPaging.searchBoxDivId)
                .find('input:text,select,input:checkbox');

            elements.each(function (index) {

                if ($(this).attr("type") === "checkbox") {
                    if ($(this).is(":checked")) {
                        $scope.DataPaging.searchParameters.push({
                            Key: $(this).attr('name') + '#' + (index + 1),
                            Value: $(this).val()
                        });
                    }
                } else {
                    if ($(this).val() && $(this).attr('name')) {
                        $scope.DataPaging.searchParameters.push({
                            Key: $(this).attr('name'),
                            Value: $(this).val()
                        });
                    }
                }

                if (index === elements.length - 1 && callback) {
                    callback();
                }
            });
        }

        $rootScope.pageRequest = function (pageid, callback) {
            $scope.DataPaging.currentPageID = parseInt(pageid);
            buildSearchParams(callback);
        };

        $scope.searchRequest = function (callback) {
            $scope.DataPaging.currentPageID = 1;
            buildSearchParams(callback);
        };

        /* ================= FILTER ================= */
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.Filter = {};
        $scope.FinancialYearList = [];
        var params = new URLSearchParams(window.location.search);
        $scope.FinYear = params.get("FinYear") || 0;
        $scope.FilterTypeChange = function () {
            $scope.Filter.SearchText = "";
            $scope.Filter.FromDate = "";
            $scope.Filter.ToDate = "";

            if ($scope.Filter.FilterType === 'created_dated') {
                $scope.showSearchDate = true;
                $scope.showSearchText = false;
            } else {
                $scope.showSearchDate = false;
                $scope.showSearchText = true;
            }
        };

        $scope.ClearSearch = function () {
            $scope.Filter = {};
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
            $scope.DataPaging.currentPageID = 1;
            $scope.DataPaging.searchParameters = [];
            $scope.GetSocietyData();
        };

        /* ================= DEFAULT VALUES ================= */
        $scope._language = 'en-us';
        $scope.ActId = '0';
        $scope.SocietyTypeId = '0';
        $scope.DIVISION_ID = '0';
        $scope.IsOnline = '-1';
        if ($scope.FinYear != null && $scope.FinYear == 0) {
            $scope.IsOnline = '1';
        }

        /* ================= RESET ================= */

        $scope.Reset = function () {
            $scope.ActId = "0";
            $scope.SocietyTypeId = "0";
            $scope.DIVISION_ID = "0";
            $scope.FromDate = "";
            $scope.ToDate = "";
            $("#txtFromDates").val("");
            $("#txtToDates").val("");
            $("#isOnline").prop("checked", false);
            $("#isOffline").prop("checked", false);
            $scope.GetSocietyData();
        }

        //old
        //$scope.Reset = function () {
        //    $scope.ActId = "0";
        //    $scope.SocietyTypeId = "0";
        //    $scope.DIVISION_ID = "0";

        //    $("#isOnline").prop("checked", false);
        //    $("#isOffline").prop("checked", false);

        //    $scope.GetSocietyData();
        //};

        /* ================= PAGE LOAD ================= */
        $scope.OnPageLoad = function () {

            $scope._language = _language;

            var langFile = _language === 'en-us'
                ? "/Translations/Translation_EN.json"
                : "/Translations/Translation_HI.json";

            $http.get(SiteUrl + langFile).then(function (res) {
                $scope.translation = res.data;
            });

            $('#txtCreatetionDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: '0D'
            });

            $scope.GetSocietyData();
        };

        /* ================= ACT LIST ================= */
        $scope.Actlist = [];

        $scope.GetSocietyActList = function () {
            $http.get(SiteUrl + "/Society/GetSocietyActList")
                .then(function (res) {

                    var allowed = $("#hdnActId").val().split(',');

                    $scope.Actlist = res.data.filter(x =>
                        allowed.indexOf(x.ActId.toString()) > -1
                    );

                    if (allowed.length === 1) {
                        $scope.ActId = allowed[0];
                        $scope.GetSocietyTypeList();
                        $("#ddrActId").prop("disabled", true);
                    }
                });
        };


        $scope.SocietyView = function (SocietyIdEnc) {
            window.location = ' /Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_MIS_SocietyRegisterDistWise~0';
        }

        /* ================= TYPE LIST ================= */
        $scope.GetSocietyTypeList = function () {
            $scope.Typelist = [];
            $scope.SocietyTypeId = '0';

            if ($scope.ActId !== '0') {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.ActId)
                    .then(function (res) {
                        $scope.Typelist = res.data;
                    });
            }
        };

        /* ================= MAIN DATA ================= */
        $scope.GetSocietyData = function () {
            debugger;
            $(".se-pre-con").fadeIn("slow");

            var fromdate = $('#txtFromDates').val();
            var todate = $('#txtToDates').val();
            $http.post("../MIS/GetSocietyDistWiseData", { ActId: $scope.ActId === "0" ? $("#hdnActId").val() : $scope.ActId, SocietyTypeId: $scope.SocietyTypeId, DIVISION_ID: $scope.DIVISION_ID, IsOnline: $scope.IsOnline, DistrictId: $scope.DISTRICT_ID, FromDate: fromdate, ToDate: todate, FinancialYear: $scope.FinYear }).then(function (res) {
                if (!res.data.Table.length) {
                    alert("No data found");
                    return;
                }

                $scope.SocietyCombine = res.data.Table;
                //console.log("$scope.SocietyCombine", $scope.SocietyCombine);
                $scope.DivShowCount = true;
                $scope.DivShowDetail = false;

            }).finally(function () {
                $(".se-pre-con").fadeOut("slow");
            });
        };


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
                //pageSize: 2,
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

        /* ================= DETAIL ================= */
        $scope.ShowDetail = function (DistId, StatusId) {
            $scope.CurrentDistId = DistId;
            $scope.CurrentStatusId = StatusId;

            $scope.SocietyList = [];
            $scope.q = "";
            $(".se-pre-con").fadeIn("slow");
            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
            return $http.post(SiteUrl + "/MIS/GetSocietyReportData", JSON.stringify({ Details: jsonParameters, "DistrictId": DistId, "StatusId": StatusId, "ActId": $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId, "IsOnline": $scope.IsOnline, "FinancialYear": $scope.FinYear })).
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

        /* ================= EXPORT ================= */
        $scope.exportToExcel = function (params) {

            $(".se-pre-con").fadeIn("slow");

            $http({
                url: SiteUrl + "/MIS/ExportSocietyReport",
                method: "POST",
                data: params,
                responseType: 'arraybuffer'
            }).then(function (response) {

                var blob = new Blob([response.data], {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                });

                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = "Society_Report.xlsx";
                link.click();

            }).finally(function () {
                $(".se-pre-con").fadeOut("slow");
            });
        };

        $scope.GetFinancialYear = function () {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").success(function (data) {
                if (data != null) {
                    $scope.FinancialYearList = data.map(function (item) {
                        return {
                            FinYearId: item.FIN_YEAR_MASTR_ID,
                            FinYearName: item.FIN_YEAR_START + '-' + item.FIN_YEAR_END
                        };
                    });
                    if ($scope.FinancialYearList != null && $scope.FinancialYearList.length > 0) {
                        if ($scope.FinYear != null && $scope.FinYear > 0) {
                            $scope.FinYear = $scope.FinancialYearList.filter(x => x.FinYearId === $scope.FinYear)[0].FinYearId;
                        } else {
                            $scope.FinYear = "0";
                        }
                    }

                } else {
                    $scope.FinancialYearList = [];
                }
            }).error(function () {
                $scope.FinancialYearList = [];
            });
        };

        $scope.GetFinancialYear();

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
        /* ================= INIT ================= */
        $scope.GetSocietyActList();
        $scope.OnPageLoad();

    });