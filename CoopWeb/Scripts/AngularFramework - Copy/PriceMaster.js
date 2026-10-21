var AppClasses = angular.module("TransportMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("TransportCnt", function ($scope, $http, $rootScope) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 2,
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
        if ($scope.Filter.FilterType == 'date') {
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
        $scope.GetPriceMasterList();
    }
    /*------------End Paging and Filter-----------------*/

    $scope._language = 'en-us';
    $scope._language = _language;
    $scope.MasterModel = {
        PriceId: 0,
        ProductId: "0",
        SubProductId: "0",
        UnitId: "0",
        Quantity: "",
        Price: "",
        Date: "",      
    }

    $scope.FinancialYear = []
    $scope.TodayDate = "";
    $scope.CurrentFY = ""
    $scope.GetCurrentDate = function () {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        debugger;
        $scope.TodayDate = dd + '/' + mm + '/' + yyyy;
        $scope.MasterModel.FinancialYear = yyyy + '-' + (parseInt(yyyy.toString().substring(2, 4)) + 1);
        $scope.FinancialYear.push({ FinancialYear: $scope.MasterModel.FinancialYear, Id: yyyy });
        for (var i = 1; i <= 5; i++) {
            $scope.FinancialYear.push({ FinancialYear: (yyyy - i) + '-' + (parseInt((yyyy - i).toString().substring(2, 4)) + 1), Id: (yyyy - i) });
        }
    }
    $scope.GetCurrentDate();
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

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }
    $scope.AddNew = function () {
        $scope.ResetModel();
        $("#addModal").modal("show");
    }
    $scope.SaveMasterData = function () {
        if ($scope.ValidateMaster()) {
            $(".blockUI").fadeIn("slow");
            $scope.MasterModel.Date = $("#PriceDate").val();
            var formData = $scope.MasterModel;
            $http.post(SiteUrl + "/Stock/PriceMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Price Already Exist." : "मूल्य पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Price successfully submited." : "मूल्य सफलतापूर्वक सबमिट किया गया", function () {
                            window.location.href = "/Stock/PriceMaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }

    $scope.ValidateMaster = function () {
       
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.ProductId.replace(/ /g, "") == "0") {
            message += _language == 'en-us' ? '\n <br />Please choose Product Name.' : '\n <br />कृपया उत्पाद का नाम चुनें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.SubProductId.replace(/ /g, "") == "0") {
            message += _language == 'en-us' ? '\n <br />Please choose Sub Product category Name.' : '\n <br />कृपया उप उत्पाद श्रेणी का नाम चुनें।';
            $("#name_hi").addClass("notValid");
        }

        if ($scope.MasterModel.UnitId.replace(/ /g, "") == "0") {
            message += _language == 'en-us' ? '\n <br />Please choose Unit.' : '\n <br />कृपया यूनिट चुनें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.Quantity.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter quantity.' : '\n <br />कृपया मात्रा दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
        if ($scope.MasterModel.Price.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter price.' : '\n <br />कृपया मूल्य दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
        if ($("#PriceDate").val() == "") {
            message += _language == 'en-us' ? '\n <br />Please choose date.' : '\n <br />कृपया तारीख चुनें।';
            $("#name_hi").addClass("notValid");
        }
        if (message.length > 29) {
            ShowModelAlert("Scheme Master", message);
            return false;
        }
        return true;
    }

    $scope.ResetModel = function () {
        $scope.MasterModel = {
            PriceId: 0,
            ProductId: "0",
            SubProductId: "0",
            UnitId: "0",
            Quantity: "",
            Price: "",
            Date: "",   
        }
    }
    $scope.PriceList = [];
    $scope.GetPriceMasterList = function () {
        $(".blockUI").fadeIn("slow");
        $scope.PriceList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetPriceMasterList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.PriceList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.PriceList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetPriceMasterList();


    $scope.ProductList = [];
    $scope.GetProductList = function () {
       // $(".blockUI").fadeIn("slow");
        $scope.ProductList = [];
        return $http.post(SiteUrl + "/Stock/ProductList", JSON.stringify({ ProductId: 0 })).
            success(function (data, status) {
               // $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.ProductList = data;               
               // $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.ProductList = [];
                //$(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetProductList();

    $scope.SubProductList = [];
    $scope.GetSubProductList = function () {
        $(".blockUI").fadeIn("slow");
        $scope.SubProductList = [];
        return $http.post(SiteUrl + "/Stock/ProductSubCategoryList", JSON.stringify({ ProductSubCategoryId: 0, ProductId: $scope.MasterModel.ProductId })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.SubProductList = data;
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.SubProductList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };

    $scope.UnitList = [];
    $scope.GetUnitListList = function () {
        //$(".blockUI").fadeIn("slow");
        $scope.UnitList = [];
        return $http.post(SiteUrl + "/Stock/UnitMasterList", JSON.stringify({ UnitId: 0 })).
            success(function (data, status) {
               // $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.UnitList = data;
               // $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.UnitList = [];
                //$(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetUnitListList();




    $scope.EditMaster = function (item) {

        $("#addModal").modal("show");

        $scope.MasterModel = {
            PriceId: item.price_id,
            ProductId: item.ProductId.toString(),
            SubProductId: item.sub_product_id.toString(),
            UnitId: item.unit_id.toString(),
            Quantity: item.quantity.toString(),
            Price: item.price,
            Date: item.ForamatedDate
        };
        $scope.GetSubProductList().then(function () {
            $scope.MasterModel.SubProductId = $scope.MasterModel.SubProductId;
        });

    };
});