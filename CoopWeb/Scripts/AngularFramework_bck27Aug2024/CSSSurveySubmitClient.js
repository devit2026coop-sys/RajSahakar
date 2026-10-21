//const { post } = require("jquery");

var CCSConsolidatedReportController = angular.module("CCSConsolidatedReportCnt", ['angularUtils.directives.dirPagination']);
var CSSDataForEdit = [];
CCSConsolidatedReportController.controller("CCSConsolidatedReportMod", function ($rootScope, $scope, $http) {
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

   // START : Get Complaint Details
    $scope.GetComplaintDetail = function () {       
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Society/GetCCSConsolidatedReportData", { Details: jsonParameters, "Type": $scope.CCSReport.Type, "fromDate": $('#txtReportFromDate').val(), "toDate": $('#txtReportToDate').val(), "districtId": ($scope.CCSReport.Type == 2 ? 0 : $scope.CCSReport.DistrictId), "CCSId": $scope.CCSReport.CCSId }).
            success(function (data, status) {
                if (data.Result === false) {
                    $scope.divComplaintList = false;
                    $scope.ReportData = [];
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CCSConsolidatedReportHeading, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला');
                }
                else {
                    $scope.divComplaintList = true;
                    $scope.ReportData = data.DataList;
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();

                    $scope.colsort = "";
                    $scope.reverseSort = false;

                    $(".se-pre-con").fadeOut("slow");                   
                }
            }).
            error(function (data, status) {
                $scope.ComplaintList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END :  Get Complaint Details

    
    // END :  Get Complaint Details
    $scope.ShowForm = false;
    $scope.ShowForm1 = true;
    $scope.AddRoleClick = function () {        
        $scope.SingleChoise = false;
        $scope.userrole = '';
        $scope.IsVisible = false;
        $scope.userrole.LevelID = '0';
        $scope.ShowForm = true;
        $scope.ShowForm1 = false;
        $scope.AdministrativeIdhideshow = true;

    };
    $scope.SocietyOthersDocuments = [];
    $scope.AddOthersDocuments = function () {      
        $scope.SocietyOthersDocuments.push({ addmorelabel: "", addmore: "" });
    }   
    //START : CCSComplaintDetails
    $scope.ExportData = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnGeneratePDF').click();
    };
    // END : CCSComplaintDetails

    //START : Download Zip
    $scope.DownloadZip = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnZipDownload').click();
    };
    // END : Download Zip
    //Save CCS Serve Report By RamSwaroop    
    $scope.InsertSave = function (SubmitStatus) {        
        //Insert data List        
        $scope.SubmitStatus = SubmitStatus;              
        if (!$scope.validatdataSubmit(SubmitStatus)) {
            return false;
        }        
        else {       
            var formData = new FormData();
            $scope.DISTRICT_ID = $("#ddlDistrict").val();
            $scope.SelectOption = $("#CCSlist").val();
            formData.append("District", $scope.DISTRICT_ID);
            formData.append("CCSType", $scope.Type);
          //  formData.append("CCSType", $scope.Type == 'undefined' ? '0' : $scope.Type);
            formData.append("CCSName", $scope.SelectOption);
            formData.append("RegiNo", $scope.CCSReport.RegistrationNo == undefined ? null : $scope.CCSReport.RegistrationNo);
            //var today = new Date();
            //var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            //var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            //var dateTime = date + ' ' + time;
            //if ($('#idddate').val() == '') {
            //    $('#idddate').val(dateTime);
            //}            
            //return;
            
            formData.append("RegiDate", $('#idddate').val() );
            formData.append("Area_of_Operation", $scope.CCSReport.AreaOperation == undefined ? '' : $scope.CCSReport.AreaOperation);
            formData.append("HO_Address", $scope.CCSReport.HO_Address == undefined ? '' : $scope.CCSReport.HO_Address);
            formData.append("Num_location_branches", $scope.CCSReport.Num_location_branches == undefined ? '' : $scope.CCSReport.Num_location_branches);
            formData.append("Ordinary", $scope.CCSReport.Ordinary == undefined ? '' : $scope.CCSReport.Ordinary);
            formData.append("Nominal", $scope.CCSReport.Nominal == undefined ? '' : $scope.CCSReport.Nominal);
            formData.append("BOD_details", $scope.CCSReport.BOD_details == undefined ? '' : $scope.CCSReport.BOD_details);
            formData.append("BOD_meeting_regularly_or_not", $scope.CCSReport.BOD_meeting_regularly_or_not == undefined ? 2 : $scope.CCSReport.BOD_meeting_regularly_or_not);
            formData.append("ResolutiontakenBODRevocable", $scope.CCSReport.ResolutiontakenBODRevocable == undefined ? '' : $scope.CCSReport.ResolutiontakenBODRevocable);
            formData.append("General_meeting", $scope.CCSReport.General_meeting == undefined ? '' : $scope.CCSReport.General_meeting);
            formData.append("Audit_completed", $('#Audit_completed_date').val() == undefined ? '' : $('#Audit_completed_date').val());
            //formData.append("Audit_completed", $scope.CCSReport.Audit_completed == undefined ? '' : $scope.CCSReport.Audit_completed);
            //formData.append("long_audit_compliance", $scope.CCSReport.long_audit_compliance == undefined ? '' : $scope.CCSReport.long_audit_compliance);
            formData.append("long_audit_compliance", $('#long_audit_compliance_Date').val() == undefined ? '' : $('#long_audit_compliance_Date').val());
            formData.append("Status_inquiry", $scope.CCSReport.Status_inquiry == undefined ? 2 : $scope.CCSReport.Status_inquiry);
            formData.append("Complaints_Received", $scope.CCSReport.Complaints_Received == undefined ? '' : $scope.CCSReport.Complaints_Received);
            formData.append("Complaints_Disposed", $scope.CCSReport.Complaints_Disposed == undefined ? '' : $scope.CCSReport.Complaints_Disposed);
            formData.append("EmployeeDetails", $scope.CCSReport.EmployeeDetails == undefined ? '' : $scope.CCSReport.EmployeeDetails);            
            formData.append("Fund_Reserve", ($scope.CCSReport.Fund_Reserve == undefined ? '' : $scope.CCSReport.Fund_Reserve));
            formData.append("Deposit_SB", $scope.CCSReport.Deposit_SB == undefined ? '' : $scope.CCSReport.Deposit_SB);
            formData.append("Deposit_FD", $scope.CCSReport.Deposit_FD == undefined ? '' : $scope.CCSReport.Deposit_FD);
            formData.append("Deposit_RD", $scope.CCSReport.Deposit_RD == undefined ? '' : $scope.CCSReport.Deposit_RD);
            formData.append("appropriation_policy_applicable_in_committee", $scope.CCSReport.appropriation_policy_applicable_in_committee == undefined ? 2 : $scope.CCSReport.appropriation_policy_applicable_in_committee);
            formData.append("Investment_CCB", $scope.CCSReport.Investment_CCB == undefined ? '' : $scope.CCSReport.Investment_CCB);
            formData.append("Investment_OtherBank", $scope.CCSReport.Investment_OtherBank == undefined ? '' : $scope.CCSReport.Investment_OtherBank);
            formData.append("Investment_Share_Purchase", $scope.CCSReport.Investment_Share_Purchase == undefined ? '' : $scope.CCSReport.Investment_Share_Purchase);           
            var ccsRegistrationData = [];
            $('#Investmenaddmore tr').each(function (index) {                
                ccsRegistrationData.push(
                    {
                        "Investment_AddMorelabel": $(this).find('td:eq(1)').find('input').val(),
                        "Investment_Addmore": $(this).find('td:eq(2)').find('input').val(),                        
                        "AdministrativeId": $(this).find('td:eq(4)').find('input').val() == "" ? 0 : $(this).find('td:eq(4)').find('input').val()                                               
                    });
            });
            $scope.Total = $("#NumberTotal").val();
            formData.append("Total", $scope.Total);
            formData.append("loan_Disbursement_Objective", $scope.CCSReport.loan_Disbursement_Objective == undefined ? '' : $scope.CCSReport.loan_Disbursement_Objective);
            formData.append("loan_Disbursement_Amount", $scope.CCSReport.loan_Disbursement_Amount == undefined ? '' : $scope.CCSReport.loan_Disbursement_Amount);
            formData.append("Debt_Recovery_Demand", $scope.CCSReport.Debt_Recovery_Demand == undefined ? '' : $scope.CCSReport.Debt_Recovery_Demand);
            formData.append("Debt_Recovery_Recovery", $scope.CCSReport.Debt_Recovery_Recovery == undefined ? '' : $scope.CCSReport.Debt_Recovery_Recovery);
            formData.append("Debt_Recovery_RecoveryPer", $scope.CCSReport.Debt_Recovery_RecoveryPer == undefined ? '' : $scope.CCSReport.Debt_Recovery_RecoveryPer);
            formData.append("Term_fee_CaseNumber", $scope.CCSReport.Term_fee_CaseNumber == undefined ? '' : $scope.CCSReport.Term_fee_CaseNumber);
            formData.append("Term_fee_Amount", $scope.CCSReport.Term_fee_Amount == undefined ? '' : $scope.CCSReport.Term_fee_Amount);
            formData.append("StakeAmount_AuthorizedAmount", $scope.CCSReport.StakeAmount_AuthorizedAmount == undefined ? '' : $scope.CCSReport.StakeAmount_AuthorizedAmount);
            formData.append("StakeAmount_PaidupCapital", $scope.CCSReport.StakeAmount_PaidupCapital == undefined ? '' : $scope.CCSReport.StakeAmount_PaidupCapital);
            formData.append("StakeAmount_WorkingCapital", $scope.CCSReport.StakeAmount_WorkingCapital == undefined ? '' : $scope.CCSReport.StakeAmount_WorkingCapital);
            formData.append("Profit_AccumulatedProfit", $scope.CCSReport.Profit_AccumulatedProfit == undefined ? '' : $scope.CCSReport.Profit_AccumulatedProfit);
            formData.append("Profit_YearlyProfit", $scope.CCSReport.Profit_YearlyProfit == undefined ? '' : $scope.CCSReport.Profit_YearlyProfit);
            formData.append("GeneralMeeting", $scope.CCSReport.GeneralMeeting == undefined ? '' : $scope.CCSReport.GeneralMeeting);
            formData.append("ProfitDistribution", $scope.CCSReport.ProfitDistribution == undefined ? '' : $scope.CCSReport.ProfitDistribution);
            formData.append("Number_estgasau_registered", $scope.CCSReport.Number_estgasau_registered == undefined ? '' : $scope.CCSReport.Number_estgasau_registered);
            formData.append("Status_inquiry1", $scope.CCSReport.Status_inquiry1 == undefined ? 2 : $scope.CCSReport.Status_inquiry1);
            formData.append("Testing10BiggestDebtQuestionnaires", $scope.CCSReport.Testing10BiggestDebtQuestionnaires == undefined ? '' : $scope.CCSReport.Testing10BiggestDebtQuestionnaires);
            formData.append("Physical_Verification_10_Tested", $scope.CCSReport.Physical_Verification_10_Tested == undefined ? '' : $scope.CCSReport.Physical_Verification_10_Tested);
            formData.append("immovablepropertyheld_Society_Type", $scope.CCSReport.immovablepropertyheld_Society_Type == undefined ? '' : $scope.CCSReport.immovablepropertyheld_Society_Type);
            formData.append("immovablepropertyheld_Society_Bookvalue", $scope.CCSReport.immovablepropertyheld_Society_Bookvalue == undefined ? '' : $scope.CCSReport.immovablepropertyheld_Society_Bookvalue);
            formData.append("immovablepropertyheld_Society_PresentValue", $scope.CCSReport.immovablepropertyheld_Society_PresentValue == undefined ? '' : $scope.CCSReport.immovablepropertyheld_Society_PresentValue);
            formData.append("CommentCreditPolicy", $scope.CCSReport.CommentCreditPolicy == undefined ? '' : $scope.CCSReport.CommentCreditPolicy);
            formData.append("Submit_Status", $scope.SubmitStatus);
            formData.append("Id", $scope.CCSReport.Id);
            formData.append("Comment_compliance_auditreport_lastthreeyears", $scope.CCSReport.Comment_compliance_auditreport_lastthreeyears == undefined ? '' : $scope.CCSReport.Comment_compliance_auditreport_lastthreeyears);
            formData.append("Detailed_CommitteimplementationRCSActRulesandByelaws", $scope.CCSReport.Detailed_CommitteimplementationRCSActRulesandByelaws == undefined ? '' : $scope.CCSReport.Detailed_CommitteimplementationRCSActRulesandByelaws);
            $(".se-pre-con").fadeIn("slow");
            formData.append("DataArray", JSON.stringify(ccsRegistrationData));
            $http.post(SiteUrl + "/Society/insertCCSSurveySubmit", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    
                    if (parseInt(data, 10) >= 0) {
                        var Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                        alert(Msg);
                        window.location.reload();
                    }
                    else if (parseInt(data, 10) == -2) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                        alert(Msg);
                    }
                    else if (parseInt(data, 10) == -1 ){
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
    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
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
        }
    };
    //End
    //  Add validation By RamSwaroop
    $scope.validatdataSubmit = function (SubmitStatus) {
        $scope.SubmitStatus = SubmitStatus;        
        if (SubmitStatus == 0) {
            var message = $scope._language == 'en-us' ? 'निम्नलिखित की पुष्टि करें : ' : 'निम्नलिखित की पुष्टि करें:';
            if ($scope.Type == undefined || $scope.Type == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया सीसीएस प्रकार चुनें ' : '\n <br />  कृपया सीसीएस प्रकार चुनें |';               

            }
            if ($scope.CCSReport.DistrictId == undefined || $scope.CCSReport.DistrictId == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया जिला चुनें ' : '\n <br /> कृपया जिला चुनें |';

            }
            if ($scope.CCSReport.CCSlist == undefined || $scope.CCSReport.CCSlist == '') {
                message += $scope._language == 'en-us' ? '\n <br />  कृपया सीसीएस नाम चुनें ' : '\n <br />  कृपया सीसीएस नाम चुनें |';
            }
            
            if ($('#idddate').val().length == 0 || $('#idddate').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br />  कृपया पंजीकरण तिथि डालें ' : '\n <br />  कृपया पंजीकरण तिथि डालें|';
            }
            if ($scope.CCSReport.RegistrationNo == undefined || $scope.CCSReport.RegistrationNo == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया पंजीकरण संख्या डालें ' : '\n <br />  कृपया पंजीकरण संख्या डालें|';
            }
            if ($('#Audit_completed_date').val().length == 0 || $('#idddate').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऑडिट कब तक का सम्पन  होगी ?  ' : '\n <br /> ऑडिट कब तक का सम्पन  होगी ?';
            }

            if ($('#long_audit_compliance_Date').val().length == 0 || $('#idddate').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऑडिट अनुपालन कब तक की है , सम्मिलित करें ?  ' : '\n <br /> ऑडिट अनुपालन कब तक की है , सम्मिलित करें ?';
            }
        }
        else if (SubmitStatus == 1) {
            var message = $scope._language == 'en-us' ? 'निम्नलिखित की पुष्टि करें : ' : 'निम्नलिखित की पुष्टि करें:';
            if ($scope.Type == undefined || $scope.Type == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया सीसीएस प्रकार चुनें ' : '\n <br />  कृपया सीसीएस प्रकार चुनें|';                

            }
            if ($scope.CCSReport.DistrictId == undefined || $scope.CCSReport.DistrictId == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया जिला चुनें|' : '\n <br />  कृपया जिला चुनें|';
            }
            if ($scope.CCSReport.CCSlist == undefined || $scope.CCSReport.CCSlist == '') {
                message += $scope._language == 'en-us' ? '\n <br />  कृपया सीसीएस नाम चुनें ' : '\n <br />  कृपया सीसीएस नाम चुनें |';
            }
            if ($scope.CCSReport.Deposit_SB == undefined || $scope.CCSReport.Deposit_SB == '') {
                message += $scope._language == 'en-us' ? '\n <br />  कृपया जमा एसबी भरें ' : '\n <br />  कृपया जमा एसबी भरें |';
            }
            if ($scope.CCSReport.Deposit_FD == undefined || $scope.CCSReport.Deposit_FD == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया जमा FD भरें ' : '\n <br />  कृपया जमा FD भरें|';
            }
            if ($scope.CCSReport.Deposit_RD == undefined || $scope.CCSReport.Deposit_RD == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया जमा आरडी भरें ' : '\n <br />  कृपया जमा आरडी भरें |';
            }
            if ($('#idddate').val().length == 0 || $('#idddate').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br />  कृपया पंजीकरण तिथि डालें ' : '\n <br />  कृपया पंजीकरण तिथि डालें|';
            }
            if ($scope.CCSReport.Investment_CCB == undefined || $scope.CCSReport.Investment_CCB == '') {
                message += $scope._language == 'en-us' ? '\n <br /> सीसीबी में निवेश की राशि डालें ' : '\n <br />  सीसीबी में निवेश की राशि डालें,|';
            }
            if ($scope.CCSReport.Investment_OtherBank == undefined || $scope.CCSReport.Investment_OtherBank == '') {
                message += $scope._language == 'en-us' ? '\n <br /> अन्य बैंक में निवेश की राशि डालें ' : '\n <br />  अन्य बैंक में निवेश की राशि डालें |';
            }
            if ($scope.CCSReport.Investment_Share_Purchase == undefined || $scope.CCSReport.Investment_Share_Purchase == '') {
                message += $scope._language == 'en-us' ? '\n <br /> क्रय शेयरों की राशि डालें ' : '\n <br />  क्रय शेयरों की राशि डालें|';
            }
            if ($scope.CCSReport.RegistrationNo == undefined || $scope.CCSReport.RegistrationNo == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया पंजीकरण संख्या डालें' : '\n <br />  कृपया पंजीकरण संख्या डालें|';
            }           
            if ($scope.CCSReport.AreaOperation == undefined || $scope.CCSReport.AreaOperation == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया क्षेत्र संचालन सम्मिलित करें ' : '\n <br />  कृपया क्षेत्र संचालन सम्मिलित करें|';
            }
            if ($scope.CCSReport.HO_Address == undefined || $scope.CCSReport.HO_Address == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया शाखाओं की संख्या और स्थान का पता डालें|' : '\n <br />  कृपया शाखाओं की संख्या और स्थान का पता डालें';
            }
            if ($scope.CCSReport.Num_location_branches == undefined || $scope.CCSReport.Num_location_branches == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया प्रधान कार्यालय का पता डालें| ' : '\n <br />  कृपया प्रधान कार्यालय का पता डालें|';
            }
            if ($scope.CCSReport.Ordinary == undefined || $scope.CCSReport.Ordinary == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कृपया (साधारण) सदस्यों की संख्या डालें ' : '\n <br /> कृपया (साधारण) सदस्यों की संख्या डालें';
            }
            if ($scope.CCSReport.Nominal == undefined || $scope.CCSReport.Nominal == '') {
                message += $scope._language == 'en-us' ? '\n <br />  कृपया (सांकेतिक) सदस्यों की संख्या डालें  ' : '\n <br /> कृपया (सांकेतिक) सदस्यों की संख्या डालें';
            }
            if ($scope.CCSReport.BOD_details == undefined || $scope.CCSReport.BOD_details == '') {
                message += $scope._language == 'en-us' ? '\n <br /> संचालक मंडल का विवरण मय नाम, पता, मोबाइल नंबर डालें  ' : '\n <br /> संचालक मंडल का विवरण मय नाम, पता, मोबाइल नंबर डालें';
            }
            if ($scope.CCSReport.BOD_meeting_regularly_or_not == undefined || $scope.CCSReport.BOD_meeting_regularly_or_not == '') {
                message += $scope._language == 'en-us' ? '\n <br /> संचालक मंडल की बैठक नियमित रूप से हो रही है या नहीं? डालें?  ' : '\n <br /> संचालक मंडल की बैठक नियमित रूप से हो रही है या नहीं? डालें';
            }
            if ($scope.CCSReport.ResolutiontakenBODRevocable == undefined || $scope.CCSReport.ResolutiontakenBODRevocable == '') {
                message += $scope._language == 'en-us' ? '\n <br /> संचालक मंडल द्वारा लिया गया कोई भी प्रस्ताव जो अपखण्डन योग़य हो , डालें  ' : '\n <br /> संचालक मंडल द्वारा लिया गया कोई भी प्रस्ताव जो अपखण्डन योग़य हो , डालें';
            }
            if ($scope.CCSReport.General_meeting == undefined || $scope.CCSReport.General_meeting == '') {
                message += $scope._language == 'en-us' ? '\n <br /> आम सभा सम्मिलित करें  ' : '\n <br /> आम सभा सम्मिलित करें';
            }

            if ($('#Audit_completed_date').val().length == 0 || $('#Audit_completed_date').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऑडिट कब तक का सम्पन  होगी ?  ' : '\n <br /> ऑडिट कब तक का सम्पन  होगी ?';
            }            
           
            if ($('#long_audit_compliance_Date').val().length == 0 || $('#long_audit_compliance_Date').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऑडिट अनुपालन कब तक की है , सम्मिलित करें ?  ' : '\n <br /> ऑडिट अनुपालन कब तक की है , सम्मिलित करें ?';
            }
            //if ($scope.CCSReport.Status_inquiry == undefined || $scope.CCSReport.Status_inquiry == '') {
            //    message += $scope._language == 'en-us' ? '\n <br /> अधिनियम के तहत जांच/कार्यवाही की स्थिति: , सम्मिलित करें ?  ' : '\n <br /> अधिनियम के तहत जांच/कार्यवाही की स्थिति: , सम्मिलित करें ?';
            //}             
            if ($scope.CCSReport.Complaints_Received == undefined || $scope.CCSReport.Complaints_Received== '') {
                message += $scope._language == 'en-us' ? '\n <br /> प्राप्त शिकायत  दर्ज करे ?  ' : '\n <br /> प्राप्त शिकायत  दर्ज करे ?';
            }
            if ($scope.CCSReport.Complaints_Disposed == undefined || $scope.CCSReport.Complaints_Disposed == '') {
                message += $scope._language == 'en-us' ? '\n <br /> निस्तारित: शिकायत  दर्ज करे ?  ' : '\n <br /> निस्तारित: शिकायत  दर्ज करे ?';
            }
            if ($scope.CCSReport.EmployeeDetails == undefined || $scope.CCSReport.EmployeeDetails == '') {
                message += $scope._language == 'en-us' ? '\n <br /> कर्मचारी, विवरण, नाम पता, मोबाइल, नंबर  दर्ज करे ?  ' : '\n <br /> कर्मचारी, विवरण, नाम पता, मोबाइल, नंबर  दर्ज करे ?';
            }
            if ($scope.CCSReport.Fund_Reserve == undefined || $scope.CCSReport.Fund_Reserve == '') {
                message += $scope._language == 'en-us' ? '\n <br /> फंड/रिजर्व  दर्ज करे ?  ' : '\n <br /> फंड/रिजर्व दर्ज करे ?';
            }
            if ($scope.CCSReport.Deposit_SB == undefined || $scope.CCSReport.Deposit_SB == '') {
                message += $scope._language == 'en-us' ? '\n <br /> जमा कुल एसबी दर्ज करे ?  ' : '\n <br /> जमा कुल एसबी दर्ज करे ?';
            }
            if ($scope.CCSReport.Deposit_FD == undefined || $scope.CCSReport.Deposit_FD == '') {
                message += $scope._language == 'en-us' ? '\n <br /> जमा कुल FD दर्ज करे ?  ' : '\n <br /> जमा कुल FD दर्ज करे ?';
            }
            if ($scope.CCSReport.Deposit_RD == undefined || $scope.CCSReport.Deposit_RD == '') {
                message += $scope._language == 'en-us' ? '\n <br /> जमा कुल RD दर्ज करे ?  ' : '\n <br /> जमा कुल RD दर्ज करे ?';
            }
            if ($scope.CCSReport.appropriation_policy_applicable_in_committee == undefined || $scope.CCSReport.appropriation_policy_applicable_in_committee == '') {
                message += $scope._language == 'en-us' ? '\n <br /> विनियोग नीति समिति में लागू है या नहीं? दर्ज करे ?  ' : '\n <br /> विनियोग नीति समिति में लागू है या नहीं?, दर्ज करे ?';
            }
            if ($scope.CCSReport.Investment_CCB == undefined || $scope.CCSReport.Investment_CCB == '') {
                message += $scope._language == 'en-us' ? '\n <br /> निवेश की  गयी सी.सी.बी. दर्ज करे ?  ' : '\n <br /> निवेश की  गयी सी.सी.बी. दर्ज करे ?';
            }
            //if ($scope.CCSReport.Investment_CCB == undefined || $scope.CCSReport.Investment_CCB == '') {
            //    message += $scope._language == 'en-us' ? '\n <br /> निवेश की  गयी सी.सी.बी. दर्ज करे ?  ' : '\n <br /> निवेश की  गयी सी.सी.बी. दर्ज करे ?';
            //}
            if ($scope.CCSReport.Investment_OtherBank == undefined || $scope.CCSReport.Investment_OtherBank == '') {
                message += $scope._language == 'en-us' ? '\n <br /> निवेश की  गयी दूसरी बैंको में राशि दर्ज करे ?  ' : '\n <br /> निवेश की  गयी दूसरी बैंको में राशि दर्ज करे ?';
            }
            if ($scope.CCSReport.Investment_Share_Purchase == undefined || $scope.CCSReport.Investment_Share_Purchase == '') {
                message += $scope._language == 'en-us' ? '\n <br /> निवेश की  गये  खरीदे शेयर की  राशि  दर्ज करे ?  ' : '\n <br />निवेश की  गये  खरीदे शेयर की  राशि  दर्ज करे ?';
            }            
            if ($scope.CCSReport.loan_Disbursement_Objective == undefined || $scope.CCSReport.loan_Disbursement_Objective == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऋण वितरण का  उद्देश्य   दर्ज करे ?  ' : '\n <br />ऋण वितरण का  उद्देश्य   दर्ज करे ?';
            }
            if ($scope.CCSReport.loan_Disbursement_Amount == undefined || $scope.CCSReport.loan_Disbursement_Amount == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऋण वितरण की  रकम   दर्ज करे ?  ' : '\n <br />ऋण वितरण की  रकम   दर्ज करे ?';
            }
            if ($scope.CCSReport.Debt_Recovery_Demand == undefined || $scope.CCSReport.Debt_Recovery_Demand == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऋण वसूली  मांग को दर्ज करे ?  ' : '\n <br />ऋण वसूली  मांग को दर्ज करे ?';
            }
            if ($scope.CCSReport.Debt_Recovery_Recovery == undefined || $scope.CCSReport.Debt_Recovery_Recovery == '') {
                message += $scope._language == 'en-us' ? '\n <br /> ऋण वसूली  की गयी राशी   दर्ज करे ?  ' : '\n <br />ऋण वसूली  की गयी राशी   दर्ज करे ?';
            }
            if ($scope.CCSReport.Term_fee_CaseNumber == undefined || $scope.CCSReport.Term_fee_CaseNumber == '') {
                message += $scope._language == 'en-us' ? '\n <br /> अवधिपार राशि  प्रकरण संख्या दर्ज करे ?  ' : '\n <br />अवधिपार राशि  प्रकरण संख्या दर्ज करे ?';
            }
            if ($scope.CCSReport.Term_fee_Amount == undefined || $scope.CCSReport.Term_fee_Amount == '') {
                message += $scope._language == 'en-us' ? '\n <br /> अवधिपार राशि दर्ज करे ?  ' : '\n <br />अवधिपार राशि दर्ज करे ?';
            }
            if ($scope.CCSReport.StakeAmount_AuthorizedAmount == undefined || $scope.CCSReport.StakeAmount_AuthorizedAmount == '') {
                message += $scope._language == 'en-us' ? '\n <br /> हिस्सेदारी राशि मै अधिकृत राशि दर्ज करे ?  ' : '\n <br />हिस्सेदारी राशि मै अधिकृत राशि दर्ज करे ?';
            }
            if ($scope.CCSReport.StakeAmount_PaidupCapital == undefined || $scope.CCSReport.StakeAmount_PaidupCapital == '') {
                message += $scope._language == 'en-us' ? '\n <br /> हिस्सेदारी राशि मै पेडअप केपिटल  दर्ज करे ?  ' : '\n <br />हिस्सेदारी राशि मै पेडअप केपिटल  दर्ज करे ?';
            }
            if ($scope.CCSReport.StakeAmount_WorkingCapital == undefined || $scope.CCSReport.StakeAmount_WorkingCapital == '') {
                message += $scope._language == 'en-us' ? '\n <br /> हिस्सेदारी राशि मै कार्यशील पूंजी   दर्ज करे ?  ' : '\n <br />हिस्सेदारी राशि मै कार्यशील पूंजी   दर्ज करे ?';
            }
            if ($scope.CCSReport.Profit_AccumulatedProfit == undefined || $scope.CCSReport.Profit_AccumulatedProfit == '') {
                message += $scope._language == 'en-us' ? '\n <br />संचित लाभ राशि  दर्ज करे ?  ' : '\n <br />संचित लाभ राशिदर्ज करे ?';
            }
            if ($scope.CCSReport.Profit_YearlyProfit == undefined || $scope.CCSReport.Profit_YearlyProfit == '') {
                message += $scope._language == 'en-us' ? '\n <br />वार्षिक लाभ दर्ज करे ?  ' : '\n <br />वार्षिक लाभ राशि दर्ज करे ?';
            }
            if ($scope.CCSReport.GeneralMeeting == undefined || $scope.CCSReport.GeneralMeetingt == '') {
                message += $scope._language == 'en-us' ? '\n <br />आम सभा सम्मिलित करें ?  ' : '\n <br />आम सभा सम्मिलित करें ?';
            }
            if ($scope.CCSReport.ProfitDistribution == undefined || $scope.CCSReport.ProfitDistribution == '') {
                message += $scope._language == 'en-us' ? '\n <br />लाभ वितरण राशि सम्मिलित करें ?  ' : '\n <br />लाभ वितरण राशि सम्मिलित करें ?';
            }
            if ($scope.CCSReport.Number_estgasau_registered == undefined || $scope.CCSReport.Number_estgasau_registered == '') {
                message += $scope._language == 'en-us' ? '\n <br />बड्स एक्ट . के तहत पंजीकृत इस्तगासों की संख्या सम्मिलित करें ?  ' : '\n <br />बड्स एक्ट . के तहत पंजीकृत इस्तगासों की संख्या सम्मिलित करें ?';
            }
            
            //if ($scope.CCSReport.Status_inquiry1 == undefined || $scope.CCSReport.Status_inquiry1 == '') {
            //    message += $scope._language == 'en-us' ? '\n <br />अधिनियम के तहत जांच/कार्यवाही की स्थिति सम्मिलित करें ?  ' : '\n <br />अधिनियम के तहत जांच/कार्यवाही की स्थिति सम्मिलित करें ?';
            //}

            if ($('#Status_inquiry1').val().length == 0 || $('#Status_inquiry1').val().length == '') {
                message += $scope._language == 'en-us' ? '\n <br />अधिनियम के तहत जांच/कार्यवाही की स्थिति सम्मिलित करें ?  ' : '\n <br />अधिनियम के तहत जांच/कार्यवाही की स्थिति सम्मिलित करें ?';
            }

            if ($scope.CCSReport.Testing10BiggestDebtQuestionnaires == undefined || $scope.CCSReport.Testing10BiggestDebtQuestionnaires == '') {
                message += $scope._language == 'en-us' ? '\n <br />परीक्षण किए गए 10 सबसे बड़े ऋण प्रश्नावली का परीक्षण निष्कर्ष सम्मिलित करें ?  ' : '\n <br />परीक्षण किए गए 10 सबसे बड़े ऋण प्रश्नावली का परीक्षण निष्कर्ष सम्मिलित करें ?';
            }
            if ($scope.CCSReport.Physical_Verification_10_Tested == undefined || $scope.CCSReport.Physical_Verification_10_Tested == '') {
                message += $scope._language == 'en-us' ? '\n <br />10 परीक्षण किए गए क्रेडिट परीक्षणों के भौतिक सत्यापन का निष्कर्ष सम्मिलित करें ?  ' : '\n <br />10 परीक्षण किए गए क्रेडिट परीक्षणों के भौतिक सत्यापन का निष्कर्ष सम्मिलित करें ?';
            } 
            if ($scope.CCSReport.immovablepropertyheld_Society_Type == undefined || $scope.CCSReport.immovablepropertyheld_Society_Type == '') {
                message += $scope._language == 'en-us' ? '\n <br />सोसायटी द्वारा रखी गई अचल संपत्ति का विवरण के प्रकार सम्मिलित करें? ' : '\n <br />सोसायटी द्वारा रखी गई अचल संपत्ति का विवरण के प्रकार सम्मिलित करें ?';
            }           
            if ($scope.CCSReport.immovablepropertyheld_Society_Bookvalue == undefined || $scope.CCSReport.immovablepropertyheld_Society_Bookvalue == '') {
                message += $scope._language == 'en-us' ? '\n <br />सोसायटी द्वारा रखी गई अचल संपत्ति का विवरण के पुस्तक मूल्य सम्मिलित करें ? ' : '\n <br />सोसायटी द्वारा रखी गई अचल संपत्ति का विवरण के पुस्तक मूल्य सम्मिलित करें ?';
            }
            if ($scope.CCSReport.immovablepropertyheld_Society_PresentValue == undefined || $scope.CCSReport.immovablepropertyheld_Society_PresentValue == '') {
                message += $scope._language == 'en-us' ? '\n <br />सोसायटी द्वारा रखी गई अचल संपत्ति का विवरण के प्रकार का  वर्तमान मूल्य सम्मिलित करें ? ' : '\n <br />सोसायटी द्वारा रखी गई अचल संपत्ति का विवरण के प्रकार का  वर्तमान मूल्य सम्मिलित करें ?';
            }
            if ($scope.CCSReport.CommentCreditPolicy == undefined || $scope.CCSReport.CommentCreditPolicy == '') {
                message += $scope._language == 'en-us' ? '\n <br />समिति की ऋण नीति पर टिप्पणी ? ' : '\n <br />समिति की ऋण नीति पर टिप्पणी ?';
            }

            if ($scope.CCSReport.Detailed_CommitteimplementationRCSActRulesandByelaws == undefined || $scope.CCSReport.Detailed_CommitteimplementationRCSActRulesandByelaws == '') {
                message += $scope._language == 'en-us' ? '\n <br />समिति द्वारा RCS Act, Ruls एवं Bye-laws की पलना पर विस्तृत टिप्पणी ? ' : '\n <br />समिति द्वारा RCS Act, Ruls एवं Bye-laws की पलना पर विस्तृत टिप्पणी ?';
            }

            if ($scope.CCSReport.Comment_compliance_auditreport_lastthreeyears == undefined || $scope.CCSReport.Comment_compliance_auditreport_lastthreeyears == '') {
                message += $scope._language == 'en-us' ? '\n <br />गत तीन वर्षो की ऑडिट रिपोर्ट की अनुपालना पर टिप्पणी ? ' : '\n <br />गत तीन वर्षो की ऑडिट रिपोर्ट की अनुपालना पर टिप्पणी ?';
            }
        }
        if(message.length > 30) {
            ShowModelAlert("CCS Survey", message);
            return false;
        }
        return true;
    };

    $scope.Update = function (Id) {
        //
        $scope.ShowForm = true;
        $scope.ShowForm1 = false;
        $scope.DistrictName = true; 
        $scope.GetCCSListNew(); 
        if (CSSDataForEdit.length == 0) {
            $http.post(SiteUrl + "/Society/GetCCSSurveydetailsview/?Gid=" + Id).
                success(function (data, status) {
                    $scope.CSSDetails(data); 
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    alert('Error while auditor data list fetch.');
                });
        }
        else {
            $scope.CSSDetails(CSSDataForEdit);
        }
    }

    $scope.CSSDetails = function (data) {  
        var BasicDetails = data;        
        if ($scope.CCSReport.Id > 0) {
            $scope.AdministrativeIdhideshow = true;
                
        } else {
            $scope.AdministrativeIdhideshow = false;
        }
            var New = data;         
        $scope.CCSReport.Id = BasicDetails['Table'][0].ID;        
        $scope.Type = BasicDetails['Table'][0].CCSType;
        $scope.CCSReport.DistrictId = BasicDetails['Table'][0].District;
        $scope.CCSReport.CCSlist = BasicDetails['Table'][0].CCSName;       
        $scope.CCSReport.RegistrationNo = BasicDetails['Table'][0].RegiNo;
       // $scope.CCSReport.RegistrationDate = BasicDetails['Table'][0].Date;
        /*$('#idddate').val(BasicDetails['Table'][0].Date.indexOf('9999') > -1 ? '' : BasicDetails['Table'][0].Date);*/
        $('#idddate').val(BasicDetails['Table'][0].RegiDate);        
        $scope.CCSReport.AreaOperation = BasicDetails['Table'][0].Area_of_Operation;
        $scope.CCSReport.HO_Address = BasicDetails['Table'][0].HO_Address;
        $scope.CCSReport.Num_location_branches = BasicDetails['Table'][0].Num_location_branches;
        $scope.CCSReport.Ordinary = BasicDetails['Table'][0].Ordinary;
        $scope.CCSReport.Nominal = BasicDetails['Table'][0].Nominal;
        $scope.CCSReport.BOD_details = BasicDetails['Table'][0].BOD_details;
        $scope.CCSReport.BOD_meeting_regularly_or_not = BasicDetails['Table'][0].BOD_meeting_regularly_or_not;           
        $('#statusdetails').val(BasicDetails['Table'][0].Status_inquiry)  ;       
        $scope.CCSReport.ResolutiontakenBODRevocable = BasicDetails['Table'][0].ResolutiontakenBODRevocable;
        $scope.CCSReport.General_meeting = BasicDetails['Table'][0].General_meeting;        
        $('#Audit_completed_date').val(BasicDetails['Table'][0].Audit_completed1);
        $('#long_audit_compliance_Date').val(BasicDetails['Table'][0].long_audit_compliance1);
       // $scope.CCSReport.Audit_completed = BasicDetails['Table'][0].Audit_completed;
        //$scope.CCSReport.long_audit_compliance = BasicDetails['Table'][0].long_audit_compliance;
        $scope.CCSReport.Complaints_Received = BasicDetails['Table'][0].Complaints_Received;
        $scope.CCSReport.Complaints_Disposed = BasicDetails['Table'][0].Complaints_Disposed;
        $scope.CCSReport.EmployeeDetails = BasicDetails['Table'][0].EmployeeDetails;
        $scope.CCSReport.Fund_Reserve = BasicDetails['Table'][0].Fund_Reserve;
        $scope.CCSReport.Deposit_SB = BasicDetails['Table'][0].Deposit_SB;
        $scope.CCSReport.Deposit_FD = BasicDetails['Table'][0].Deposit_FD;
        $scope.CCSReport.Deposit_RD = BasicDetails['Table'][0].Deposit_RD;
        $scope.CCSReport.Investment_CCB = BasicDetails['Table'][0].Investment_CCB;
        $scope.CCSReport.Investment_OtherBank = BasicDetails['Table'][0].Investment_OtherBank;
        $scope.CCSReport.Investment_Share_Purchase = BasicDetails['Table'][0].Investment_Share_Purchase;
        /*$('#Aadditemmorelabel' + rows + '').val(BasicDetails['Table'][0].Investment_AddMorelabel);*/
        $scope.Total = BasicDetails['Table'][0].Total;
        $scope.CCSReport.loan_Disbursement_Objective = BasicDetails['Table'][0].loan_Disbursement_Objective;
        $scope.CCSReport.loan_Disbursement_Amount = BasicDetails['Table'][0].loan_Disbursement_Amount;
        $scope.CCSReport.Debt_Recovery_Demand = BasicDetails['Table'][0].Debt_Recovery_Demand;
        $scope.CCSReport.Debt_Recovery_Recovery = BasicDetails['Table'][0].Debt_Recovery_Recovery;
        $scope.CCSReport.Debt_Recovery_RecoveryPer = BasicDetails['Table'][0].Debt_Recovery_RecoveryPer;
        $scope.CCSReport.Term_fee_CaseNumber = BasicDetails['Table'][0].Term_fee_CaseNumber;
        $scope.CCSReport.Term_fee_Amount = BasicDetails['Table'][0].Term_fee_Amount;
        $scope.CCSReport.StakeAmount_AuthorizedAmount = BasicDetails['Table'][0].StakeAmount_AuthorizedAmount;
        $scope.CCSReport.StakeAmount_PaidupCapital = BasicDetails['Table'][0].StakeAmount_PaidupCapital;
        $scope.CCSReport.StakeAmount_WorkingCapital = BasicDetails['Table'][0].StakeAmount_WorkingCapital;
        $scope.CCSReport.Profit_AccumulatedProfit = BasicDetails['Table'][0].Profit_AccumulatedProfit;
        $scope.CCSReport.Profit_YearlyProfit = BasicDetails['Table'][0].Profit_YearlyProfit;
        $scope.CCSReport.GeneralMeeting = BasicDetails['Table'][0].GeneralMeeting;
        $scope.CCSReport.ProfitDistribution = BasicDetails['Table'][0].ProfitDistribution;
        $scope.CCSReport.Number_estgasau_registered = BasicDetails['Table'][0].Number_estgasau_registered;
        $scope.CCSReport.Testing10BiggestDebtQuestionnaires = BasicDetails['Table'][0].Testing10BiggestDebtQuestionnaires;
        $scope.CCSReport.Physical_Verification_10_Tested = BasicDetails['Table'][0].Physical_Verification_10_Tested;
        $scope.CCSReport.immovablepropertyheld_Society_Type = BasicDetails['Table'][0].immovablepropertyheld_Society_Type;
        $scope.CCSReport.immovablepropertyheld_Society_Bookvalue = BasicDetails['Table'][0].immovablepropertyheld_Society_Bookvalue;
        $scope.CCSReport.immovablepropertyheld_Society_PresentValue = BasicDetails['Table'][0].immovablepropertyheld_Society_PresentValue;
        $scope.CCSReport.CommentCreditPolicy = BasicDetails['Table'][0].CommentCreditPolicy;
        $scope.CCSReport.appropriation_policy_applicable_in_committee = BasicDetails['Table'][0].appropriation_policy_applicable_in_committee;       
        $('#Status_inquiry1').val(BasicDetails['Table'][0].Status_inquiry1);
        $scope.CCSReport.Comment_compliance_auditreport_lastthreeyears = BasicDetails['Table'][0].Comment_compliance_auditreport_lastthreeyears;
        $scope.CCSReport.Detailed_CommitteimplementationRCSActRulesandByelaws = BasicDetails['Table'][0].Detailed_CommitteimplementationRCSActRulesandByelaws;
        
        var SocietyOthersDocuments = [];
        for (var x = 0; x < BasicDetails.length; x++) { 
            SocietyOthersDocuments.push(
                {
                    
                    "addmorelabel": BasicDetails['Table'][x].Investment_AddMorelabel,
                    "addmore": BasicDetails['Table'][x].Investment_AddMore1,
                    "AdministrativeId": BasicDetails['Table'][x].AdministrativeId
                });
        }
        $scope.SocietyOthersDocuments = SocietyOthersDocuments;

    }
   
    $scope.Getccssurveydetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/getCCSSurveydetails/").
            success(function (data, status) {                
                $scope.AssignedAuditor = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
                $scope.showLoader = false;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching role list.");
                $scope.showLoader = false;
            });
    };  
    $scope.CCSSurveyView = [];
    $scope.CSSInvestmentAddmore = [];
    $scope.ShowDivForView = function (Id) {              
        $http.post(SiteUrl + "/Society/GetCCSSurveydetailsview/?Gid=" + Id).
            success(function (data, status) {
                $http.post(SiteUrl + "/Society/Investmentdetails/?Gid=" + Id).
                    success(function (data1, status) {
                            $scope.CSSInvestmentAddmore = JSON.parse(data1.DataList);                       
                    }).
                    error(function (data, status) {
                        $scope.CCSSurveyView = [];
                        alert('Error while auditor data list fetch.');
                    });
                $scope.CSSDetailsalldata = [];                
                $scope.CCSSurveyView = data;
                CSSDataForEdit = data[0];              
            }).
            error(function (data, status) {
                $scope.CCSSurveyView = [];
                alert('Error while auditor data list fetch.');
            });          
    };
    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope.CCSReport = {};
        $scope.CCSReport.Type = "1";
        $scope.Getccssurveydetail();
      
        //if (AssignedAuditor.Submit_Status == 0 || AssignedAuditor.TakeactionStatus == 0) {
        //    $scope.hideshow = true;
        //}
        //else {
        //    $scope.hideshow = false;
        //}
        $scope.DistrictList();
        r = $('#hdfLoggedInUserDistrictId').val();
        if (parseInt(r, 10) > 0) {
            $scope.IsControlDisable = true;
            $scope.CCSReport["DistrictId"] = (r).toString();
            $scope.GetCCSListNew();
            $scope.CCSReport.DistrictId = $('#hdfLoggedInUserDistrictId').val();
        }
        else {
            $scope.IsControlDisable = false;
            $scope.CCSReport.DistrictId = "";
        }
        $scope.CCSReport.CCSId = "0";
        //$scope.GetComplaintDetail();
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
        //$('#idddate').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd-mm-yy"
        //});
    };
    // END : Page load Function


    // START : Get District List
    $scope.DistrictList = function () {
        $scope.Districts = [];
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
                $scope.DistrictName = false;
              //  $scope.CCSReport.DistrictId = $scope.CCSReport.DistrictId;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END : Get District List
    // START : Get CCS List
    $scope.GetCCSListNew = function () {
        //alert($scope.CCSReport.DistrictId);
        $('#hdfLoggedInUserDistrictId').val($scope.CCSReport.DistrictId);
        $(".se-pre-con").fadeIn("slow");
        //$scope.GetComplaintDetail();
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $('#ddlDistrict').val() }];
        $http.post(SiteUrl + "/Society/GetCCSData", { paramlist: model, Type: $scope.Type }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.CCSReport.CCSId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeIn("slow");
    };
      // END : Get CCS List
    $scope.DeleteOtherDocument = function (index, id) {       
        ShowAlertMessage("Alert", "Are you sure you want to delete this document", function () {
            $scope.SocietyOthersDocuments.splice(index, 1);

            $http.post(SiteUrl + "/Society/DeleteInvestmentDetails", { "Id": id }).
                success(function (data, status) {
                    //$(".blockUI").fadeOut("slow");                   
                }).
                error(function (data, status) {
                    //$(".blockUI").fadeOut("slow");                  
                });
        });
    }; 
    //END : Clear Form
    // START : On Page Load
    $scope.OnPageLoad();
   // $scope.Getccssurveydetail();
    // END : On Page Load
});