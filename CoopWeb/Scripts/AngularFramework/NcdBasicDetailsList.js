
var MasterMod = angular.module('ncdMod', ['angularUtils.directives.dirPagination']);


MasterMod.controller("ncdCtrl", function ($scope, $rootScope, $http, $timeout) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.NCDDistrictDetail = [];
    $scope.primary_activity = "";
    $scope.NcdListData = [];
    $scope.searchText = "";
    $scope.searchTextsearchText = "";
    $scope.exportToExcel = [];
    $scope.districtcode = [];
    $scope.sectorOperationcode = [];
    $scope.DISTRICT_ID = null;
    $scope.isNoRecord = false;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    /*------------Paging and Filter-----------------*/

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.obj.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetListData(false);
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
                $scope.GetListData(false);
            }
            ++indx;
        });

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
                callback($scope.SocietyCountId_ss, $scope.DistrictId_ss);
            }
            ++indx;
        });
    };

    $scope.ncd = {};

    if ($("#hdnDistrictId").val() != null && $("#hdnDistrictId").val() != "0") {
        $scope.DISTRICT_ID = $("#hdnDistrictId").val();
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
        $scope.GetSocietyFunctionalStatus();
        $scope.GetDistrictByStatecode();

    };

    $scope.ClearSearch = function () {
        $scope.DISTRICT_ID = "";
        $scope.cooperative_society_name = "";
        $scope.registration_number = "";
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];

        window.location = "../NcdData/NcdBasicDetailsList";
    }


    $scope.GetListData = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
        if (!isExport)
            $rootScope.DataPagingDetails = {};
        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/NcdData/GetNcdBasicDetailsList", JSON.stringify({ Details: jsonParameters, isExport: isExport })).
            success(function (responcedata, status) {
                var data = responcedata;
                if (isExport) {
                    $scope.downloadFile(data);
                }
                else {
                    if (data != null && data.length > 0) {
                        $scope.isNoRecord = false;
                        $scope.NcdListData = data;
                        if ($scope.NcdListData != null && $scope.NcdListData.length > 0) {
                            if ($scope.DataPaging.currentPageID == 1) {
                                $scope.TotalRecords = data[0].TotalRecords;
                            }
                            $rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                            $rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                            $rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                            $rootScope.SetDataPaging();
                            $scope.colsort = "";
                            $scope.reverseSort = false;
                            $scope.sortData = function (column) {
                                $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                                $scope.colsort = column;
                            };
                            $scope.getSortClass = function (column) {
                                if ($scope.colsort == column) {
                                    return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                                }
                                return '';
                            };
                        }
                    } else {
                        $scope.NcdListData = [];
                        $scope.isNoRecord = true;
                        $scope.colspan = 11;
                    }
                }
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };


    /*   Export Data*/

    $scope.exportToExcel = function () {

        $scope.GetListData(true);

    };

    $scope.downloadFile = function (fileUrl) {
        const fileName = fileUrl.split('/').pop();
        $http({
            url: fileUrl,
            method: 'GET',
            responseType: 'blob'
        }).then(function (response) {
            var blob = new Blob([response.data], { type: response.headers('Content-Type') });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }).catch(function (error) {
            console.error('Error downloading file:', error);
        });
    };

    $scope.exportToExcelWorking = function () {

        if (!$scope.NcdListData || $scope.NcdListData.length === 0) {
            alert("No data to export!");
            return;
        }

        const data = $scope.NcdListData.map((item, index) => ({
            "S.No": index + 1,
            "Cooperative Society Name": item.cooperative_society_name || "",
            "District Name": item.DistrictName || "",
            "State Name": "Rajasthan",
            "Block / Urban Local Body": item.blockname || "",
            "Sector Name": item.Sectorname,
            "Registration No": item.registration_number || "",
            "No of Member": item.members_of_society || "",
            "Functional Status": item.FunctionalStatus || "",
            "Cooperative ID (NCD ID)": item.cooperative_id || "",
            "Date of Registration": item.date_registration ? new Date(item.date_registration).toLocaleDateString('en-GB') : ""
        }));
        const worksheet = XLSX.utils.json_to_sheet(data);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "NCD_List");
        XLSX.writeFile(workbook, "NCDListData.xlsx");

    };

    $scope.FilteredNCDSectorDetails = [];

    $scope.filterPrimaryActivity = function (sid) {
        var sid = $scope.ncd.sector_of_operation_type;
        if (!sid) {
            $scope.FilteredNCDSectorDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSectorDetailsByIsCredit?actionName=GetSectorByCredit&sectorId=" + sid)
            .success(function (data) {
                if (data.Result.Data && data.Result.Data.NCDSectorDetail) {
                    $scope.FilteredNCDSectorDetails = JSON.parse(data.Result.Data.NCDSectorDetail);
                } else {
                    $scope.FilteredNCDSectorDetails = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.SocietyFunctionalStatusList = [];

    $scope.GetSocietyFunctionalStatus = function () {

        $scope.SocietyFunctionalStatusList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSocietyFunctionalStatus")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.SocietyFunctionalStatusDetail) {
                        $scope.SocietyFunctionalStatusList = JSON.parse(response.data.Result.Data.SocietyFunctionalStatusDetail);
                    }
                } else {
                    console.error("Error fetching functional status:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching functional status:", error);
            });
    };

    $scope.GetDistrictByStatecode = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                    $scope.NCDDistrictDetails = JSON.parse(data.Result.Data.NCDDistrictDetail);
                    if ($("#hdnDistrictId").val() != "0") {
                        $scope.ncd.districtcode = $scope.NCDDistrictDetails.filter(x => x.DistrictId == $("#hdnDistrictId").val())[0].district_code;
                        if ($scope.ncd.districtcode != null && $scope.ncd.districtcode > 0) {
                            var parameter = { Key: 'districtcode', Value: $scope.ncd.districtcode }
                            $scope.DataPaging.searchParameters.push(parameter);

                        }
                    }
                    $scope.GetListData(false);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.viewItem = function (eid) {

        window.location.href = '/NCDData/cooperativeregistrationsview?eid=' + item.ncd_coop_reg_id;
    };

    $scope.openDirector = function (item) {
        var url = "../NCDData/NCDBoardOfDirector?cooperativeId=" + item.cooperative_id.trim();
        if (item.districtcode)
            url += "&districtcode=" + item.districtcode;

        if (item.sectorOperationcode)
            url += "&sectorcode=" + item.sectorOperationcode;

        window.open(url, '_blank');
    };
    $scope.confirmRemoveSocietyDetail = function (item) {
        if (confirm("Are you sure you want to remove this society detail?")) {
            $scope.RemoveSocietyDetail(item);
        }
    };
    $scope.RemoveSocietyDetail = function (item) {
        $(".se-pre-con").fadeIn("slow");
        var requestData = {
            ncd_id: item.cooperative_id,
            cooperative_society_name: item.cooperative_society_name,
            registration_number: item.registration_number,
            date_registration: item.date_registration,
            sector_of_operation: item.sector_of_operation
        };
        $http.post(SiteUrl + "/NcdService/DeleteSocietyDetails", requestData).then(function (res) {
            console.log(res);
            $(".se-pre-con").fadeOut("slow");
            if (res.data && res.data.statuscode == 200) {
                alert(res.data.message);
                location.reload();
            }
            else {
                alert(res.data.message);
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (err) {
            console.error("HTTP error:", err);
            alert("HTTP error while deleting.");
            $(".se-pre-con").fadeOut("slow");
        });
    };


    $scope.OnPageLoad();

});