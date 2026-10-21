//const { post } = require("jquery");

var CSSComplaintDetailController = angular.module("CSSComplaintDetailsCnt", ['angularUtils.directives.dirPagination']);
var CSSDataForEdit = [];
CSSComplaintDetailController.controller("CSSComplaintDetailsMod", function ($rootScope, $scope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 5,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    var TableData10NomiAll = [];
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

    $scope.divComplaintList = false;
    $scope.IsDisabled = true;
    $scope.IsControlDisable = false;
    $scope.DistrictId = 0;
    $scope.ShowSubmitButton = true;
    $scope.EditId = 0

    $scope.ShowForm = true;
    $scope.ShowForm1 = false;
    $scope.AddRoleClick = function () {
        $scope.SingleChoise = false;
        $scope.userrole = '';
        $scope.IsVisible = false;
        $scope.userrole.LevelID = '0';
        $scope.ShowForm = true;
        $scope.ShowForm1 = false;
        $scope.AdministrativeIdhideshow = true;

    };

    $scope.GetCCSComplaintList = function () {
        $http.get(SiteUrl + "/Society/GetCCSComplaintListDetails").success(function (data) {
            $scope.CCSComplaintList = JSON.parse(data.Result.Data.CCSComplaintList);
        }).error(function () {
            alert("Error");
        });
    };

    //Save CCS Complaint Data    
    $scope.InsertSave = function () {
        //Insert data List        
        if (!$scope.validatdataSubmit()) {
            return false;
        }
        else {
            var formData = new FormData();
            formData.append("Id", $scope.EditId);
            //formData.append("FinYrMonth", $scope.finMonth);
            formData.append("FinYrMonth", 0);
            formData.append("OtherComplaints", $scope.CCSComplaintData.Other_Complaints_count == undefined ? null : $scope.CCSComplaintData.Other_Complaints_count);
            formData.append("DocumentsReceived", $scope.CCSComplaintData.Documents_Received_count == undefined ? null : $scope.CCSComplaintData.Documents_Received_count);
            formData.append("NoticesIssued", $scope.CCSComplaintData.Notices_Issued_count == undefined ? null : $scope.CCSComplaintData.Notices_Issued_count);
            formData.append("HearingDocuments", $scope.CCSComplaintData.Hearing_Documents_count == undefined ? null : $scope.CCSComplaintData.Hearing_Documents_count);
            formData.append("Pp_DocumentsShared", $scope.CCSComplaintData.Pp_Documents_Shared == undefined ? null : $scope.CCSComplaintData.Pp_Documents_Shared);
            formData.append("CourtFiled", $scope.CCSComplaintData.Court_Filed_cases == undefined ? null : $scope.CCSComplaintData.Court_Filed_cases);
            formData.append("BudsActFIR", $scope.CCSComplaintData.Buds_Act_FIR_cases == undefined ? null : $scope.CCSComplaintData.Buds_Act_FIR_cases);
            formData.append("FIR_Registered", $scope.CCSComplaintData.FIR_Registered_count == undefined ? null : $scope.CCSComplaintData.FIR_Registered_count);

            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/InsertCCSComplaintForm", formData,
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
                        $scope.GetCCSComplaintList();
                        $scope.CCSComplaintData = {};
                        $scope.EditId = 0;
                    }
                    else if (parseInt(data, 10) == 2) {
                        var Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट हो गया।');
                        alert(Msg);
                        $scope.GetCCSComplaintList();
                        $scope.CCSComplaintData = {};
                    }
                    else if (parseInt(data, 10) == 0) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                        alert(Msg);
                        $scope.GetCCSComplaintList();
                        $scope.CCSComplaintData = {};
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


    $scope.EditRow = function (item) {
        $scope.EditId = item.Id;
        //$scope.finMonth = item.FinYrMonth;
        $scope.CCSComplaintData.Other_Complaints_count = item.OtherComplaints;
        $scope.CCSComplaintData.Documents_Received_count = item.DocumentsReceived;
        $scope.CCSComplaintData.Notices_Issued_count = item.NoticesIssued;
        $scope.CCSComplaintData.Hearing_Documents_count = item.HearingDocuments;
        $scope.CCSComplaintData.Pp_Documents_Shared = item.Pp_DocumentsShared;
        $scope.CCSComplaintData.Court_Filed_cases = item.CourtFiled;
        $scope.CCSComplaintData.Buds_Act_FIR_cases = item.BudsActFIR;
        $scope.CCSComplaintData.FIR_Registered_count = item.FIR_Registered;
        window.scrollTo(0, 0);
    };


    $scope.validatdataSubmit = function () {

        var message = $scope._language == 'en-us' ? 'निम्नलिखित की पुष्टि करें : ' : 'निम्नलिखित की पुष्टि करें:';

        if ($scope.CCSComplaintData.Other_Complaints_count == undefined || $scope.CCSComplaintData.Other_Complaints_count == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया अन्य माध्यमों से प्राप्त शिकायतों की संख्या डालें ' : '\n <br />  कृपया अन्य माध्यमों से प्राप्त शिकायतों की संख्या डालें|';
        }
        if ($scope.CCSComplaintData.Documents_Received_count == undefined || $scope.CCSComplaintData.Documents_Received_count == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया इन शिकायतों में से कितने प्रकरणों में डॉक्यूमेंट प्राप्त हुए की संख्या डालें ' : '\n <br /> कृपया इन शिकायतों में से कितने प्रकरणों में डॉक्यूमेंट प्राप्त हुए की संख्या डालें';
        }
        if ($scope.CCSComplaintData.Notices_Issued_count == undefined || $scope.CCSComplaintData.Notices_Issued_count == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया प्राप्त डॉक्यूमेंट में कितने प्रकरणों में से संबंधित के विरुद्ध नोटिस जारी किए गए की संख्या डालें ' : '\n <br /> कृपया प्राप्त डॉक्यूमेंट में कितने प्रकरणों में से संबंधित के विरुद्ध नोटिस जारी किए गए की संख्या डालें';
        }
        if ($scope.CCSComplaintData.Hearing_Documents_count == undefined || $scope.CCSComplaintData.Hearing_Documents_count == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया जारी नोटिसों के विरुद्ध कितने प्रकरणों में सुनवाई कर दस्तावेज प्राप्त किए गए की संख्या डालें ' : '\n <br /> कृपया जारी नोटिसों के विरुद्ध कितने प्रकरणों में सुनवाई कर दस्तावेज प्राप्त किए गए की संख्या डालें';
        }
        if ($scope.CCSComplaintData.Pp_Documents_Shared == undefined || $scope.CCSComplaintData.Pp_Documents_Shared == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया कितने प्रकरणों में सुनवाई तिथि के पश्चात् इस्तगासा संबंधी पत्रादि पीपी को उपलब्ध करवाए गए की संख्या डालें ' : '\n <br />  कृपया कितने प्रकरणों में सुनवाई तिथि के पश्चात् इस्तगासा संबंधी पत्रादि पीपी को उपलब्ध करवाए गए की संख्या डालें|';
        }
        if ($scope.CCSComplaintData.Court_Filed_cases == undefined || $scope.CCSComplaintData.Court_Filed_cases == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया कितने प्रकरणों में इस्तगासे न्यायालय में प्रस्तुत किए गए की संख्या डालें ' : '\n <br /> कृपया कितने प्रकरणों में इस्तगासे न्यायालय में प्रस्तुत किए गए की संख्या डालें';
        }
        if ($scope.CCSComplaintData.Buds_Act_FIR_cases == undefined || $scope.CCSComplaintData.Buds_Act_FIR_cases == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया BUDS Act 2019 की धारा 3/21 के तहत FIR दर्ज करवाए जाने योग्य प्रकरण की संख्या डालें ' : '\n <br /> कृपया BUDS Act 2019 की धारा 3/21 के तहत FIR दर्ज करवाए जाने योग्य प्रकरण की संख्या डालें';
        }
        if ($scope.CCSComplaintData.FIR_Registered_count == undefined || $scope.CCSComplaintData.FIR_Registered_count == '') {
            message += $scope._language == 'en-us' ? '\n <br /> कृपया कितने प्रकरणों में पुलिस थानों में FIR दर्ज कराई गई की संख्या डालें ' : '\n <br /> कृपया कितने प्रकरणों में पुलिस थानों में FIR दर्ज कराई गई की संख्या डालें';
        }

        if (message.length > 30) {
            ShowModelAlert("सर्वेक्षण विवरण", message);
            return false;
        }
        return true;
    };



    $scope.GetCSSComplaintOnlineDistrictWise = function (districtId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ComplaintOnlineDistrictWise = [];
        $http.get(SiteUrl + "/society/GetCSSComplaintOnlineDistrictWise?districtId=" + districtId)
            .success(function (data, status) {

                if (!data || !data.Result || !data.Result.Data || !data.Result.Data.ComplaintOnlineDistrictWise) {
                    $scope.ComplaintOnlineDistrictWise = [];
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                $scope.ComplaintOnlineDistrictWise = JSON.parse(data.Result.Data.ComplaintOnlineDistrictWise);
                $(".se-pre-con").fadeOut("slow");
            })

            .error(function () {
                $scope.clean();
                $scope.ComplaintOnlineDistrictWise = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };



    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope.CCSComplaintData = {};

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


        var disrictid = $('#hdnDistrictId').val();
        $scope.GetCSSComplaintOnlineDistrictWise(disrictid);
        $scope.GetCCSComplaintList();
        //$scope.GetFinancialYearMonths();

    };
    // END : Page load Function
    $scope.AutoselectMonth = function () {
        var DT = new Date();
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getFullYear() + '-0' + (DT.getMonth() + 1);
        }
        else {
            date = DT.getFullYear() + '-' + (DT.getMonth() + 1);
        }
        $("#txtFromMonth").val(date);
    };
    $scope.AutoselectMonth();
    //$scope.GetFinancialYearMonths = function () {

    //    var currentDate = new Date();
    //    var currentMonth = currentDate.getMonth() + 1;
    //    var currentYear = currentDate.getFullYear();

    //    var startYear, endYear;

    //    // Financial year calculation
    //    if (currentMonth >= 4) {
    //        startYear = currentYear;
    //        endYear = currentYear + 1;
    //    } else {
    //        startYear = currentYear - 1;
    //        endYear = currentYear;
    //    }

    //    $scope.FinancialYearMonths = [];

    //    // April to December
    //    for (var m = 4; m <= 12; m++) {
    //        $scope.FinancialYearMonths.push(
    //            startYear + '-' + (m < 10 ? '0' + m : m)
    //        );
    //    }

    //    // January to March
    //    for (var m = 1; m <= 3; m++) {
    //        $scope.FinancialYearMonths.push(
    //            endYear + '-' + (m < 10 ? '0' + m : m)
    //        );
    //    }

    //    // Auto select current month
    //    $scope.finMonth =
    //        currentYear + '-' + (currentMonth < 10 ? '0' + currentMonth : currentMonth);
    //};



    $scope.GetFinancialYearMonths = function () {
        var currentDate = new Date();
        var currentMonth = currentDate.getMonth() + 1;
        var currentYear = currentDate.getFullYear();
        var startYear, endYear;
        // Financial year calculation
        if (currentMonth >= 4) {
            startYear = currentYear;
            endYear = currentYear + 1;
        } else {
            startYear = currentYear - 1;
            endYear = currentYear;
        }

        $scope.FinancialYearMonths = [];
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        // April to December
        for (var m = 4; m <= 12; m++) {
            $scope.FinancialYearMonths.push({
                Id: startYear + '-' + (m < 10 ? '0' + m : m),
                MonthName: monthNames[m - 1]
            });
        }

        // January to March
        for (var m = 1; m <= 3; m++) {
            $scope.FinancialYearMonths.push({
                Id: endYear + '-' + (m < 10 ? '0' + m : m),
                MonthName: monthNames[m - 1]
            });
        }
        $scope.finMonth = currentYear + '-' + (currentMonth < 10 ? '0' + currentMonth : currentMonth);
    };


    // Call function on load
    //$scope.GetFinancialYearMonths();
    // START : On Page Load
    $scope.OnPageLoad();
    // END : On Page Load
});