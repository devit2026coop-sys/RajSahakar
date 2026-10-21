
var MasterMod = angular.module('UnitMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UnitMasterCont", function ($scope, $http, $rootScope, $timeout) {

    // SCOPE

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 10,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }

    $scope.dataPayload = {};
    $scope.esignData = {};
    $scope.responseMessage = '';
    $scope.payload = [];
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            var type = $(this).attr("type");

            if (type == "checkbox" || type == "radio") {

                if ($(this).is(":checked")) {

                    var parameter = {
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    };

                    $scope.DataPaging.searchParameters.push(parameter);
                }

            } else {

                if ($(this).val() != '' &&
                    $(this).val() != null &&
                    $(this).attr('name') != undefined) {

                    var parameter = {
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    };

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

        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);

        var controls = $('#' + $scope.DataPaging.searchBoxDivId)
            .find('input:text,select,input:checkbox,input:radio');

        var ln = controls.length;
        var indx = 1;

        controls.each(function () {

            var type = $(this).attr('type');

            // Checkbox & Radio Button
            if (type != undefined && (type == "checkbox" || type == "radio")) {

                if ($(this).is(":checked")) {

                    var parameter = {
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    };

                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            else {

                // Textbox / Select
                if ($(this).val() != '' &&
                    $(this).val() != null &&
                    $(this).attr('name') != undefined) {

                    var parameter = {
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    };

                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }

            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }

            ++indx;
        });
    };

    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();```1    
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }


    $scope._language = 'en-us';
    //  $scope.currentPage = 1;
    //$scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];

    $scope.IsShowAssignForm = true;
    $scope.IsShowAssignForm1 = false;

    $scope.UserRegistration = {};
    $scope.Level = [];
    $scope.DESIGNATION = [];
    $scope.Districts = [];
    $scope.ROList = [];
    $scope.actionObj = {};
    $scope.actionAGMObj = {};


    $scope.chkBulkSelected = false;

    // on page load
    $scope.OnPageLoad = function () {
        $('#ddlInspector').multiselect();
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.LevelList();

        var LoginDivisionId = $('#hdnDivisionId').val();

        var hdnUserRoleDeptMappingId = $('#hdnUserRoleDeptMappingId').val();

        var LoginDistrictId = $('#hdnDistrictId').val();
        $scope.actionAGMObj.DistrictId = LoginDistrictId;
        $scope.DistrictLists();




        $scope.ShowResponseMessage();

        //$scope.responseMessage = $(".hdnResponse").val();
        //if ($scope.responseMessage == "1") {
        //    ShowOkMessage("", $scope._language == 'en-us' ? "Successfully submitted." : "सफलतापूर्वक जमा कर लिया गया है।", function () {
        //        $(".hdnResponse").val('');
        //        window.location.href = "../society/InspactorTargetsAction";
        //    });

        //} else {
        //    ShowOkMessage("", $scope._language == 'en-us' ? "There is a problem while submitting the request." : "अनुरोध सबमिट करते समय समस्या आ रही है।",
        //        function () { $(".hdnResponse").val(''); });
        //}



        var roleid = $('#hdnRoleId').val();
    };


    $scope.IsShowAssignForm = false;




    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };


    $scope.update = function (item) {

        $scope.UnitMaster.DepartmentId = item.departmentId;
        $scope.UnitMaster.UnitNameEn = item.UnitNameEn;
        $scope.UnitMaster.UnitNameHi = item.UnitNameHi;
        $scope.UnitMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.UnitMaster.UnitId = item.UnitId;
    };


    //$scope.GetInspactor();

    $scope.ischeckdtrue = false;

    $scope.OtherPersonList = [];
    $scope.OtherPersonListNew = [];
    $scope.GSSStatusList = [];


    $scope.Backtomove = function () {
        $scope.IsShowAssignForm = true;
        $scope.IsShowAssignForm1 = false;
    }

    $scope.RemoveOtherPersonListRow = function (index) {
        $scope.OtherPersonListNew.splice(index, 1);
        $scope.$apply();
    }

    $scope.AllDetails = false;



    $scope.ClearSearch = function () {

        $scope.InspactorId = '0';
        InspactorId = $scope.InspactorId;
        $scope.GetInspactorTargetsCount(InspactorId);
    }

    $scope.InspactorId = '0';

    $scope.GetInspactorTargetsDetails1 = function (month) {
        $(".blockUI").show();
        $scope.setParameterValue('pending', $scope.search.pendingType || 'all');
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        $http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: $scope.InspactorId, StatusId: $scope.Status1, GSSSStatusId: $scope.GSSSStatusId, Flag: 2, Month: month })).
            success(function (data, status) {
                $scope.InspactorTargetsListDetils = JSON.parse(data.DataList);
                console.log('$scope.InspactorTargetsListDetils', $scope.InspactorTargetsListDetils);
                $scope.AllDetails = true;
                $scope.IsShowAssignForm = false;
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").hide();

            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.InspactorTargetsListDetils = [];
                alert('Error while Society Election Detail data.');
            });
    };


    $scope.BacktoMainDiv = function () {

        $scope.Ischeckedall = true;
        $scope.ischeckdtrue = false;
        $scope.InspactorId = '0';
        $scope.AllDetails = false;
        $scope.IsShowAssignForm = true;
    }



    $scope.validatestop = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue }).length == 0) {
            message += $scope._language == 'en-us' ? '<br />Please select at least one record for process.' : '<br />कृपया प्रक्रिया के लिए कम से कम एक रिकॉर्ड का चयन करें।';
        }


        if ($scope.GSSSStatusId != 4) {
            if ($scope.ReqoestStatus == undefined || $scope.ReqoestStatus == "") {
                message += $scope._language == 'en-us' ? '<br />Please select Approved/Rreject.' : '<br />कृपया स्वीकृत/अस्वीकार का चयन करें।';
            }

            if ($scope.ReqoestStatus == 7 && ($scope.Remarks == null || $scope.Remarks == "" || $scope.Remarks == undefined)) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया रिमार्क दर्ज़ करें।';
            }

            if ($scope.ReqoestStatus == 3 && ($scope.FileName == null || $scope.FileName == "" || $scope.FileName == undefined)) {
                message += $scope._language == 'en-us' ? '<br />Please select the proposal report file.' : '<br />कृपया प्रस्ताव रिपोर्ट फ़ाइल का चयन करें।';
            }
        }


        if ($scope.GSSSStatusId == 4 && ($scope.RemarksSenction == null || $scope.RemarksSenction == "" || $scope.RemarksSenction == undefined)) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया रिमार्क दर्ज़ करें।';
        }

        if (message.length > 29) {
            ShowOkMessage("Inspector Status ", message, function () { });
            $scope.checksta = false;
            return false;

        }
        $scope.checksta = true;
        return true;
    }



    $scope.FileName = "";
    $scope.FileType = "";
    $scope.IsUploadFileName = false;

    $scope.AddFile1 = function (type, id) {

        $scope.FileType = type;
        $scope.IsUploadFileName = true;
        $scope.IsUpload = false;
    }
    $scope.ChangeFile1 = function (type, id) {

        if (type == 1) {
            $scope.IsUploadFileName = false;
            $scope.FileName = "";
            $scope.IsUpload = false;
        }
    }
    $scope.fileChanged = function (input, fieldName, type) {
        if (input.files && input.files.length > 0) {
            var file = input.files[0];
            $scope.$apply(function () {
                const acceptTypes = (input.getAttribute("accept") || "").split(",");
                const allowed = acceptTypes.some(t => file.type === t.trim());
                if (!allowed) {
                    alert("Invalid file type. Allowed: " + acceptTypes.join(", "));
                    input.value = "";        // clear the input
                    return;                  // stop further execution
                }

                // 2️⃣  Check size (e.g., < 4 MB)
                const sizeKB = file.size / 1024;
                if (sizeKB >= 4096) {
                    ShowOkMessage("Warning",
                        $scope._language === 'en-us'
                            ? "File too big, please select a file less than 4 MB"
                            : "फ़ाइल बहुत बड़ी है, कृपया 4MB से कम फ़ाइल का चयन करें",
                        function () { }
                    );
                    input.value = "";
                    return;
                }



                $scope.FileType = type;
                $scope.IsUploadFileName = true;
                $scope.IsUpload = false;
                //$scope[fieldName] = input.files[0].name; // store file name                
                $scope.UploadFile_new1(input, fieldName, 1);
            });
        } else {
            // User clicked cancel, reset
            $scope.$apply(function () {
                $scope.IsUploadFileName = false;
                $scope[fieldName] = "";
            });
        }
    };
    $scope.UploadFile_new1 = function (input, id, type) {

        try {
            // FormData
            if (window.FormData !== undefined) {
                var files = input.files[0];
                var formData = new FormData();
                formData.append("MemberFile", files);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Society/UploadinInspactorFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }


    $scope.UploadFile_new = function (id, type) {

        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("MemberFile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Society/UploadinInspactorFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr1 = function (url, data, type) {

        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        if (type == 1) {

                            $scope.FileName = request.responseText;
                            $scope.IsUpload = true;
                            $scope.IsUploadFileName = true;
                        }
                        $scope.$apply();
                    }
                }
            };

            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    $scope.GSSTargetProceesStatusList = function (StatusId, InspactorTargetsId) {
        $scope.GccProceesStatus_Temp = [];
        return $http.post("../Society/GSSTargetProceesStatusList", { "StatusId": StatusId, "InspactorTargetsId": InspactorTargetsId }).
            success(function (data, status) {
                $scope.GSSStatusList = data;

            }).
            error(function (data, status) {
                $scope.GSSStatusList = [];
            });
    };

    $scope.Action = function (item) {
        $scope.actionObj = {};

        //console.log(item);

        $scope.actionObj.InspactorTargetsId = item.InspactorTargetsId;
        $scope.actionObj.Inspector_ID = item.Inspector_ID;
        $scope.actionObj.InspacorName = item.InspacorName;
        $scope.actionObj.PrecentNameGSS = item.PrecentNameGSS;
        $scope.actionObj.NewGSSName = item.NewGSSName;
        $scope.actionObj.InspectorStatusId = item.InspectorStatusId;
        $scope.actionObj.IsTspArea = item.IsTspArea;
        $scope.actionObj.NewGSSFinanceAmount = Number(item.NewGSSFinanceAmount) || 0;
        $scope.actionObj.NewGSSprecious = Number(item.NewGSSprecious) || 0;
        $scope.actionObj.NoofMember = Number(item.NoofMember) || 0;
        $scope.actionObj.AreaType = item.AreaType;
        $scope.actionObj.DrName = item.DrName;
        $scope.actionObj.IsMahilaMpacs = item.IsMahilaMpacs;


        $scope.GSSTargetProceesStatusList(item.InspectorStatusId, item.InspactorTargetsId);


        $scope.ChangeFile1(1, 0);
        $scope.FileName = '';
        $('#FileName').val('');


        $scope.FormName = "";
        $scope.DocumentName = "Upload Document";

        $scope.actionObj.StatusId = '0';

        if (item.InspectorStatusId == '1') {
            $scope.FormName = "Committee Approval Form";
            $scope.DocumentName = "Upload Committee Report";


        }
        else if (item.InspectorStatusId == '8' || item.InspectorStatusId == '9' || item.InspectorStatusId == '12') //
        {
            $scope.FormName = "Joint Registrar Plan";
            // $scope.FormName = "Joint Registrar Office Approval Form";           
        }

        else if (item.InspectorStatusId == '3') {
            $scope.FormName = "Approval Form";
            $scope.DocumentName = "Upload Sanction Letter";

        }
        else if (item.InspectorStatusId == '6') {
            $scope.FormName = "Objection Removal Form";

        } else if (item.InspectorStatusId == '4') {

            $scope.FormName = "RCS Approval Form";
            $scope.GSSStatusList = [];
        }

        if (item.InspectorStatusId == '4' || item.InspectorStatusId == '6' || item.InspectorStatusId == '3' || item.InspectorStatusId == '8' || item.InspectorStatusId == '9' || item.InspectorStatusId == '12' || item.InspectorStatusId == '1') { $('#Actionmodal_JRPlan').modal('show'); }


        //$scope.$apply();


    }

    $scope.OTPVerify = function () {
        $scope.ShowOTPDiv = true;
        $('#otpmodal').modal('show');

    };

    $scope.VerifyOTP = function () {
        $scope.TakeAction();
    }



    $scope.ValidationAction = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $(".se-pre-con").fadeIn("slow");
        //var checkedData = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue });
        $scope.ReqestStatus = $scope.actionObj.StatusId;

        if ($scope.actionObj.StatusId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please Select Action.' : '<br />कृपया कार्रवाई का चयन करें।';
        }

        $scope.SelectedList = [];
        $scope.SelectedList = $scope.actionObj;

        if ($scope.actionObj.StatusId == "8") {
            var SurveyDateObj = new Date($scope.actionObj.DateOfSubmission ?? new Date());
            $scope.actionObj.DateOfSubmission = (SurveyDateObj.getMonth() + 1) + '/' + SurveyDateObj.getDate() + '/' + SurveyDateObj.getFullYear();

            var SurveyDateObj1 = new Date($scope.actionObj.DateOfApproval ?? new Date());
            $scope.actionObj.DateOfApproval = (SurveyDateObj1.getMonth() + 1) + '/' + SurveyDateObj1.getDate() + '/' + SurveyDateObj1.getFullYear();

            if (!$scope.FileName || $scope.FileName == "undefined" || $scope.FileName == '') {
                message += $scope._language == 'en-us' ? '<br />Please Upload Commitee Approval Document.' : '<br />कृपया समिति अनुमोदन दस्तावेज़ अपलोड करें।';
            }
        }
        if ($scope.actionObj.StatusId == "15") {
            var SanctionDateObj = new Date($scope.actionObj.SanctionDate ?? new Date());
            $scope.actionObj.SanctionDate = (SanctionDateObj.getMonth() + 1) + '/' + SanctionDateObj.getDate() + '/' + SanctionDateObj.getFullYear();

            if (!$scope.FileName || $scope.FileName == "undefined" || $scope.FileName == '') { message += $scope._language == 'en-us' ? '<br />Please Upload Sanction Letter.' : '<br />कृपया स्वीकृति पत्र अपलोड करें।'; }

        }
        if ($scope.actionObj.StatusId == "9") {

            if ($scope.actionObj.NewGSSFinanceAmount == "" || $scope.actionObj.NewGSSFinanceAmount == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Share capital.' : '<br />नई प्रस्तावित ग्राम सेवा सहकारी समिति की हिस्सा पूंजी।';
            }
            if (($scope.actionObj.IsTspArea || (!$scope.actionObj.IsTspArea && $scope.actionObj.AreaType == "2")) && $scope.actionObj.NoofMember < 75) {
                message += $scope._language == 'en-us' ? '<br />Number of members should be equal or greater than 75.' : '<br />सदस्यों की संख्या 75 के बराबर या उससे अधिक होनी चाहिए।';
            }

            if (!$scope.actionObj.IsTspArea && $scope.actionObj.NoofMember < 150) {
                message += $scope._language == 'en-us' ? '<br />Number of members should be equal or greater than 150.' : '<br />सदस्यों की संख्या 150 के बराबर या उससे अधिक होनी चाहिए।';
            }

            if (!$scope.actionObj.IsMahilaMpacs && ($scope.actionObj.IsTspArea || (!$scope.actionObj.IsTspArea && $scope.actionObj.AreaType == "2")) && $scope.actionObj.NewGSSFinanceAmount < 100000) {
                message += $scope._language == 'en-us' ? '<br />Amount of share capital should be equal or greater than Rs 1 lakh.' : '<br />कृपया 1 लाख रुपये से अधिक या उसके बराबर प्रस्तावित एम.पैक्स शेयर पूंजी दर्ज करें।।';
            }


            if (!$scope.actionObj.IsTspArea && $scope.actionObj.AreaType != "2" && $scope.actionObj.NewGSSprecious < 75000) {
                message += $scope._language == 'en-us' ? '<br />Amount of proposed MPACS should be equal or greater than Rs 75,000.' : '<br />कृपया 75,000 रुपये से अधिक या उसके बराबर प्रस्तावित एम.पैक्स दर्ज करें।।';
            }

            if (!$scope.actionObj.IsMahilaMpacs && !$scope.actionObj.IsTspArea && $scope.actionObj.AreaType != "2" && $scope.actionObj.NewGSSFinanceAmount < 150000) {
                message += $scope._language == 'en-us' ? '<br />Amount of share capital should be equal or greater than Rs 1 lakh 50 Thousand.' : '<br />कृपया 1 लाख 50 हजार रुपये से अधिक या उसके बराबर प्रस्तावित एम.पैक्स शेयर पूंजी दर्ज करें।।';
            }

        }


        if ($scope.actionObj.StatusId == "3") {
            $scope.FileName = "";
        }



        if (message.length > 29) {
            $(".se-pre-con").fadeOut("slow");
            ShowOkMessage("Inspector Status ", message, function () { });
            return false;

        }

        return true;
    }


    var payload = [];
    $scope.SaveAction = function () {


        payload = [];
        if ($scope.ValidationAction()) {
            if ($scope.chkBulkSelected && $scope.bulkAppStatusid != "0" && $scope.SelectedAppflowList !== 'undefined' && $scope.SelectedAppflowList.length > 0) {
                angular.forEach($scope.SelectedAppflowList, function (item) {
                    // create a copy of the item and add statusid from $scope.actionObj
                    var obj = angular.copy(item);
                    obj.statusid = $scope.actionObj.StatusId;
                    obj.Remarks = $scope.actionObj.Remarks;
                    obj.Inspectorfile = $scope.FileName;
                    obj.SanctionDate = $scope.actionObj.SanctionDate;

                    payload.push(obj);
                });

            } else {
                payload = [$scope.actionObj];

            }

            if (!payload || payload.length === 0) {
                // payload is null, undefined, or has no elements
                return;
            }

            if ($scope.actionObj.StatusId == "4") {
                $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data.indexOf('NO#') >= 0) {
                            alert("Due to some issue OTP not send");
                            //location.reload();
                            //window.location = '../ApplicationFlow/ApplicationFlow';
                        } else {
                            if (data != "LOGOUT") {
                                // alert(data);
                                $scope.actionObj.TXN = data;
                                alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                                //$scope.ShowOTPDiv = true;
                                $scope.OTPVerify();
                            }
                            else {
                                $(".se-pre-con").fadeOut("slow");
                                window.location = 'https://sso.rajasthan.gov.in/signin';
                            }
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                    });



            }
            else {
                $scope.TakeAction();
            }
        }

    };


    $scope.TakeAction = function () {

        $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: payload, Remarks: $scope.actionObj.Remarks, Inspectorfile: $scope.FileName, status: $scope.actionObj.StatusId, TXNno: $scope.actionObj.TXN, SenctionDate: "", OTP: $scope.actionObj.OTP }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == "1") {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                        window.location.href = "../society/InspactorTargetsAction";
                    });
                } else {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "There is a problem while submitting the request. Please try after some time.<br/></b>" : "अनुरोध सबमिट करते समय समस्या आ रही  है। कृपया कुछ समय बाद प्रयास करें।<br/></b>", function () {
                        //$(".se-pre-con").fadeOut("slow");
                        //$("#otpmodal").modal("hide");
                        //$("#Actionmodal_JRPlan").modal("hide");
                    });


                }
            }).
            error(function (data, status) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
                // ✅ Close OTP modal on error
                //$("#otpmodal").modal("hide");
                $(".se-pre-con").fadeOut("slow");
            });
        // }
    }
    $scope.InspectorStatus = false;
    $scope.GetInspectorStatusList = function (InspactorTargetsId) {

        $http.post("/Society/GetInspectorStatusDetails", JSON.stringify({ InspactorTargetsId: InspactorTargetsId })).
            success(function (data, status) {

                $scope.GetInspectorStatus = JSON.parse(data.DataList);
                if ($scope.GetInspectorStatus.length > 0) {
                    $scope.InspectorStatus = true;
                    $scope.AllDetails = false;
                }
                else {
                    $scope.InspectorStatus = false;
                    $scope.AllDetails = true;
                }
            }).
            error(function (data, status) {
                $scope.GetInspectorStatus = [];
                alert('Error while Society Election Detail data.');
            });
    }

    $scope.backStaushistory = function () {

        $scope.AllDetails = true;
        $scope.InspectorStatus = false;
    }

    $scope.historyshow = function (item) {
        $scope.InspactorTargetsId = item.InspactorTargetsId;
        $scope.GetInspectorStatusList($scope.InspactorTargetsId)
    }

    $scope.bulkAppStatusid = 0;
    $scope.bulkAppDistid = 0;
    $scope.toggleSelection = function (item) {

        //console.log(item);
        $scope.chkBulkSelected = false;

        if (!$scope.SelectedAppflowList) {
            $scope.SelectedAppflowList = [];
        }

        if (item.selected) {
            $scope.SelectedAppflowList.push(item);
            $scope.chkBulkSelected = true;
        } else {
            $scope.SelectedAppflowList = $scope.SelectedAppflowList.filter(
                member => member.InspactorTargetsId !== item.InspactorTargetsId
            );
        }

        // $scope.payObj.GccId = item.GopalCreditCardId;

        if ($scope.SelectedAppflowList.length > 0) { $scope.chkBulkSelected = true; $scope.bulkAppStatusid = item.InspectorStatusId; $scope.bulkAppDistid = item.DistictId; }
        else { $scope.chkBulkSelected = false; $scope.bulkAppStatusid = 0; $scope.bulkAppDistid = 0; }

    };
    $scope.Edit = function (item) {
        sessionStorage.setItem('GSSData', JSON.stringify(item));
        window.location.href = 'InspactorTargets';
    }

    $scope.BulkAction = function () {
        $scope.actionObj = {};
        //   $scope.GetInspectorStatusList(item.statusid, item.InspactorTargetsId);
        $scope.FormName = "Approval Form";
        $scope.DocumentName = "Upload Document";

        if ($scope.bulkAppStatusid == '8' || $scope.bulkAppStatusid == '9' || $scope.bulkAppStatusid == '12') //
        {
            $scope.FormName = "Joint Registrar Plan";
            //  $scope.FormName = "Joint Registrar Office Approval Form";           
            //  $scope.DocumentName = "Upload Document";

        }

        else if ($scope.bulkAppStatusid == '15' || $scope.bulkAppStatusid == '3') {
            $scope.FormName = "Approval Form";
            $scope.DocumentName = "Upload Sanction Letter";

        }


        $scope.ChangeFile1(1, 0);
        $scope.FileName = '';
        $('#FileName').val('');
        // alert($scope.bulkAppStatusid);

        $scope.GSSTargetProceesStatusList($scope.bulkAppStatusid, 0).then(function () { $scope.actionObj.StatusId = "0"; });

        $('#Actionmodal_JRPlan').modal('show');

    }
    $scope.DistrictListData = function () {
        $scope.UserRegistration.DistrictId = "0";
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictLst = data;
                $scope.AllDistrictList = data;
                if ($scope.AllDistrictList && $scope.InspactorTargetsListDetils && $scope.InspactorTargetsListDetils.length > 0) {
                    let distIds = $scope.InspactorTargetsListDetils[0].DistinctDistrictIds;
                    $scope.DistrictList = $scope.AllDistrictList.filter(x => distIds.includes(x.DISTRICT_ID.toString()));
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DistrictLst = [];
            });
    };
    $scope.DistrictListData();
    $scope.setParameterValue = function (key, value) {
        var parameter = { Key: key, Value: value }
        //  $scope.DataPaging.searchParameters.push(parameter);

        var existing = $scope.DataPaging.searchParameters
            .find(function (p) { return p.Key === key; });

        if (existing) {
            // Key already present – update its Value
            existing.Value = value;
        } else {
            // Key not present – add new object
            $scope.DataPaging.searchParameters.push({ Key: key, Value: value });
        }
    }

    $scope.GetInspactorTargetsDetails = function () {
        $(".blockUI").show();

        $scope.setParameterValue('Action', "GetGssTargetApplications");
        var isExport = false;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };

        return $http.post(SiteUrl + "/MIS/GccLoanReports", JSON.stringify({ Details: jsonParameters, Exportjson: null, Action: "GSSTargetData" })).
            success(function (responcedata, status) {
                if (isExport) {
                    $scope.downloadFile(responcedata.filename);
                }
                else {
                    if (responcedata.DataList != null && responcedata.DataList.length > 0) {

                        var data = JSON.parse(responcedata.DataList);
                        $scope.InspactorTargetsListDetils = data; // JSON.parse(data);
                        console.log($scope.InspactorTargetsListDetils);


                        if ($scope.AllDistrictList && $scope.InspactorTargetsListDetils && data.length > 0) {
                            let distIds = $scope.InspactorTargetsListDetils[0].DistinctDistrictIds;
                            // Convert to array safely
                            if (typeof distIds === 'string') {
                                distIds = distIds.split(',').map(id => id.trim());
                            }

                            // Then filter with strict matching
                            $scope.DistrictList = $scope.AllDistrictList.filter(
                                x => distIds.includes(x.DISTRICT_ID.toString())
                            );
                            $scope.DistrictList = $scope.AllDistrictList.filter(x => distIds.includes(x.DISTRICT_ID));
                        }

                        $scope.AllDetails = true;
                        $scope.IsShowAssignForm = false;

                        $rootScope.DataPagingDetails = responcedata.DataPaging;
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
                }
                $(".blockUI").hide();
            }).
            error(function (responcedata, status) {
                $(".blockUI").hide();
                $scope.InspactorTargetsListDetils = []
            });



        //var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        //$http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: $scope.InspactorId, StatusId: $scope.Status1, GSSSStatusId: $scope.GSSSStatusId, Flag: 2, Month: month })).
        //    success(function (data, status) {
        //        $scope.InspactorTargetsListDetils = JSON.parse(data.DataList);
        //        $scope.AllDetails = true;
        //        $scope.IsShowAssignForm = false;
        //        $rootScope.DataPagingDetails = data.DataPaging;
        //        $rootScope.SetDataPaging();
        //        $(".blockUI").hide();

        //    }).
        //    error(function (data, status) {
        //        $(".blockUI").hide();
        //        $scope.InspactorTargetsListDetils = [];
        //        alert('Error while Society Election Detail data.');
        //    });
    };
    $scope.SubmitSurveyReport = function () {
        if ($('#hdnRoleId').val() == 10088) {

            $scope.GSSSStatusId = '8';
        }
        else {
            $scope.GSSSStatusId = '1';

            // $scope.GSSSStatusId = '1';
        }
        //$scope.GSSSStatusId = '1'
        $scope.Status1 = '6';
        $scope.Ischeckedall = false;
        $scope.InspactorId = 0;
        $scope.DataPaging.currentPageID = 1;
        $scope.GetInspactorTargetsDetails();
        $scope.ischeckdtrue = false;
        $scope.PrintDatalist = [];
    }

    $scope.ActionforAGM = function (item) {
        $scope.actionAGMObj = {};
        //debugger;
        $scope.actionAGMObj.InspactorTargetsId = item.InspactorTargetsId;
        $scope.actionAGMObj.Inspector_ID = item.Inspector_ID;
        $scope.actionAGMObj.InspacorName = item.InspacorName;
        $scope.actionAGMObj.PrecentNameGSS = item.PrecentNameGSS;
        $scope.actionAGMObj.NewGSSName = item.NewGSSName;
        $scope.actionAGMObj.InspectorStatusId = item.InspectorStatusId;
        $scope.actionAGMObj.IsTspArea = item.IsTspArea;
        $scope.actionAGMObj.NewGSSFinanceAmount = Number(item.NewGSSFinanceAmount) || 0;
        $scope.actionAGMObj.NewGSSprecious = Number(item.NewGSSprecious) || 0;
        $scope.actionAGMObj.NoofMember = Number(item.NoofMember) || 0;
        $scope.actionAGMObj.AreaType = item.AreaType;
        $scope.actionAGMObj.DrName = item.DrName;
        $scope.actionAGMObj.IsMahilaMpacs = item.IsMahilaMpacs;

        $scope.actionAGMObj.LevelId = "0";
        $scope.actionAGMObj.DivisionId = "0";
        $scope.actionAGMObj.DistrictId = "0";
        $scope.actionAGMObj.OfficeId = "0";
        $scope.actionAGMObj.InspactorId = "0";
        $scope.GSSTargetProceesStatusList(item.InspectorStatusId, item.InspactorTargetsId).then(function () { $scope.actionAGMObj.StatusId = "16"; });
        //sessionStorage.setItem("AGMData", JSON.stringify($scope.actionAGMObj));
        $('#Actionmodal_AGM').modal('show');
    }

    $scope.ActionforNewInspector = function (item) {
        debugger;
        $scope.actionAGMObj = {};
        $scope.actionAGMObj.InspactorTargetsId = item.InspactorTargetsId;
        $scope.actionAGMObj.Remarks = "";

        $scope.GetAGMMultiInspectorDetails(item.InspactorTargetsId).then(function () {

            console.log("MultiInspectorList", $scope.MultiInspectorList);
            var firstRecord = $scope.MultiInspectorList.length > 0 ? $scope.MultiInspectorList[0] : null;

            $scope.actionAGMObj.DistrictId = firstRecord ? (firstRecord.DistrictId || firstRecord.DistrictID || "0") : "0";
            $scope.actionAGMObj.OfficeId = firstRecord ? (firstRecord.OfficeId || firstRecord.OFFICE_ID || "0") : "0";
            $scope.actionAGMObj.DivisionId = firstRecord ? (firstRecord.DivisionId || "0") : "0";
            $scope.actionAGMObj.LevelId = firstRecord.LevelId.toString();
            $scope.GetRAJFEDRO();
            $scope.DistrictLists();

            $timeout(function () {

                $scope.actionAGMObj.DistrictId = firstRecord.DistrictId.toString();

                $scope.OfficeList();

                $timeout(function () {

                    $scope.actionAGMObj.OfficeId = firstRecord.OfficeId.toString();

                }, 200);

            }, 200);

            $http.post(SiteUrl + "/Master/InspectorList/?OfficeId=" + $scope.actionAGMObj.OfficeId + "&DistrictId=" + $scope.actionAGMObj.DistrictId + "&DivisionId=" + $scope.actionAGMObj.DivisionId)
                .success(function (data, status) {
                    $scope.InspacorData = data;

                    var assignedIds = $scope.MultiInspectorList.map(function (x) {
                        return (x.MappingID || x.Inspector_ID).toString();
                    });
                    $timeout(function () {
                        var $dropdown = $('#Actionmodal_AGMAssignInpector #ddlInspectorAssign');
                        $dropdown.multiselect('destroy');
                        $dropdown.empty();

                        angular.forEach(data, function (itm) {
                            var text = ($scope._language == 'en-us') ? itm.OfficerNameEn : itm.OfficerNameHi;
                            var option = $('<option></option>').val(itm.MappingID).text(text);
                            if (assignedIds.includes(itm.MappingID.toString())) {
                                option.attr('selected', 'selected');
                            }
                            $dropdown.append(option);
                        });

                        $dropdown.multiselect({
                            includeSelectAllOption: false,
                            enableFiltering: false,
                            buttonWidth: '100%',
                            nonSelectedText: 'Select Inspector',
                            maxHeight: 300,
                            onChange: function (option, checked) {
                                var selected = $dropdown.find('option:selected');
                                if (selected.length > 2) {
                                    $(option).prop('selected', false);
                                    $dropdown.multiselect('refresh');
                                    alert("Maximum 2 inspectors allowed");
                                }
                            }
                        });
                        $scope.GSSTargetProceesStatusList(item.InspectorStatusId, item.InspactorTargetsId).then(function () { $scope.actionAGMObj.StatusId = "16"; });
                        $('#Actionmodal_AGMAssignInpector').modal('show');

                    }, 200);
                })
                .error(function () {
                    alert('Error while fetching inspector list.');
                });
        });
    };


    function loadInspectorList() {

        $http.post(SiteUrl + "/Master/InspectorList/?OfficeId=" + $scope.actionAGMObj.OfficeId +
            "&DistrictId=" + ($scope.actionAGMObj.DistrictId || 0) +
            "&DivisionId=" + ($scope.actionAGMObj.DivisionId || 0))
            .success(function (data) {

                $scope.InspacorData = data;

                var assignedIds = $scope.MultiInspectorList.map(function (x) {
                    return (x.MappingID || x.Inspector_ID).toString();
                });

                $timeout(function () {

                    var $dropdown = $('#ddlInspectorAssign');
                    $dropdown.multiselect('destroy');
                    $dropdown.empty();

                    angular.forEach(data, function (itm) {

                        var text = ($scope._language == 'en-us')
                            ? itm.OfficerNameEn
                            : itm.OfficerNameHi;

                        var option = $('<option></option>')
                            .val(itm.MappingID)
                            .text(text);

                        if (assignedIds.includes(itm.MappingID.toString())) {
                            option.attr('selected', 'selected');
                        }

                        $dropdown.append(option);
                    });

                    $dropdown.multiselect({
                        buttonWidth: '100%',
                        maxHeight: 300
                    });

                }, 200);
            });
    }


    $scope.ValidateAddAGM = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        var selectedIds = $('#ddlInspector').val();

        if (!selectedIds || selectedIds.length === 0) {
            message += $scope._language == 'en-us' ? '<br />Please select at least one inspector.' : '<br />कृपया कम से कम एक इंस्पेक्टर चुनें।';
        }

        //if ($scope.actionAGMObj.InspactorId == "0") {
        //    message += $scope._language == 'en-us' ? '<br />Please  Select Inspactor.' : '<br />कृपया इंस्पेक्टर चुनें।';
        //    //$("#txtAccountName_" + index).addClass("notValid");            
        //}

        if ($scope.actionAGMObj.StatusId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Action.' : '<br />कृपया कार्रवाई का चयन करें।';
        }

        //if ($scope.InspactorId == $scope.OldInspactorId) {
        //    message += $scope._language == 'en-us' ? '<br />Please Select Another Inspactor.' : '<br />कृपया अन्य निरीक्षक का चयन करें।';
        //    //$("#txtAccountName_" + index).addClass("notValid");            
        //}

        if (message.length > 29) {
            ShowOkMessage("CONDUCT FOR AGM ", message, function () { });
            $scope.checksta = false;
            return false;
        }
        $scope.checksta = true;
        return true;

    }

    var payloadAGM = [];
    $scope.SaveforAGM = function () {
        //debugger;
        payloadAGM = [];

        var selectedIds = $('#ddlInspector').val(); // array      

        if (selectedIds.length > 2) {
            alert("You cannot select more than 2 inspectors");
            return;
        }
        var multiIds = selectedIds.join(',');
        $scope.actionAGMObj.Inspector_ID = selectedIds[0];

        //console.log("selectedIds", selectedIds);

        //$scope.actionAGMObj.Inspector_ID = $scope.actionAGMObj.InspactorId;
        payloadAGM = [$scope.actionAGMObj];
        //payloadAGM = [$scope.actionAGMObj];
        //$scope.actionAGMObj.LevelId;
        //$scope.actionAGMObj.DistrictId;
        //$scope.actionAGMObj.OfficeId;
        //$scope.actionAGMObj.InspactorId;
        //var InspacorName = ($scope.getDropDownListName($scope.InspacorData, $scope.actionAGMObj.InspactorId, "MappingID", "OfficerNameEn"));
        var StatusId = $scope.actionAGMObj.StatusId;
        //alert(InspacorName);
        //debugger;
        if ($scope.ValidateAddAGM()) {
            $http.post(SiteUrl + "/Society/SaveAGMStatusDR", { InspactorStatusDetails: payloadAGM, Remarks: $scope.actionAGMObj.Remarks, Inspectorfile: "", status: StatusId, SenctionDate: "", MultiInspactorIds: multiIds, DistrictId: $scope.actionAGMObj.DistrictId, DivisionId: $scope.actionAGMObj.DivisionId, LevelId: $scope.actionAGMObj.LevelId, OfficeId: $scope.actionAGMObj.OfficeId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            $("#Actionmodal_AGM").modal("hide");
                            window.location.href = "../society/InspactorTargetsAction";
                        });
                    } else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "There is a problem while submitting the request. Please try after some time.<br/></b>" : "अनुरोध सबमिट करते समय समस्या आ रही  है। कृपया कुछ समय बाद प्रयास करें।<br/></b>", function () {
                            //$("#Actionmodal_JRPlan").modal("hide");
                        });
                    }
                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };



    $scope.SaveAssignInspector = function () {

        var selectedIds = $('#ddlInspectorAssign').val();

        if (!selectedIds || selectedIds.length === 0) {
            selectedIds = $scope.MultiInspectorList.map(x =>
                (x.MappingID || x.Inspector_ID).toString()
            );
        }

        if (selectedIds.length > 2) {
            alert("Max 2 inspectors allowed");
            return;
        }

        var multiIds = selectedIds.join(',');

        var payload = [{
            InspactorTargetsId: $scope.actionAGMObj.InspactorTargetsId,
            Inspector_ID: selectedIds[0]
        }];

        $http.post(SiteUrl + "/Society/SaveAGMStatusDR", {
            InspactorStatusDetails: payload,
            MultiInspactorIds: multiIds,
            DistrictId: $scope.actionAGMObj.DistrictId,
            DivisionId: $scope.actionAGMObj.DivisionId,
            LevelId: $scope.actionAGMObj.LevelId,
            OfficeId: $scope.actionAGMObj.OfficeId,
            Remarks: $scope.actionAGMObj.Remarks
        })
            .success(function (data) {

                if (data == "1") {
                    ShowOkMessage("Success", "Inspector assigned successfully", function () {
                        $("#Actionmodal_AGMAssignInpector").modal("hide");
                        window.location.reload();
                    });
                }
            });
    };

    $scope.LevelList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Level = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS").
            success(function (data, status) {
                $scope.Level = data;
                $scope.Level = data.filter(x => x.LevelId == 2 || x.LevelId == 3)
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetRAJFEDRO = function () {
        //debugger;
        $scope.actionAGMObj.DivisionId = "0";
        $scope.ROList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=0").
            success(function (data, status) {
                $scope.ROList = data;
            }).
            error(function (data, status) {
                // alert("Error while fetching Post list")
            });
    };

    $scope.IsDistrictShow = false;

    $scope.DistrictLists = function () {
        //debugger;
        //$scope.actionAGMObj.DistrictId = "0";
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;

                if ($("#hdnDistrictId").val() != "0") {
                    $scope.actionAGMObj.DistrictId = $("#hdnDistrictId").val();
                    $scope.IsDistrictShow = true;
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
    };

    $scope.OfficeList = function () {
        //$scope.actionAGMObj.DistrictId = "0";
        //if ($scope.CurrentUserDistrictId == $scope.actionAGMObj.DistrictId) {
        //    $scope.FileName = "";
        //}

        $scope.actionAGMObj.InspactorId = "0";
        $scope.InspacorData = [];
        $scope.actionAGMObj.OfficeId = "0";
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 " + ($scope.actionAGMObj.LevelId && $scope.actionAGMObj.LevelId != 0 ? " and l.LevelId=" + $scope.actionAGMObj.LevelId : "")).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                if (window.location.href.includes("localhost")) {
                    $scope.OfficeListData = data.filter(x => x.OfficeId == 48 || x.OfficeId == 42 || x.OfficeId == 10 || x.OfficeId == 3);
                }
                else {
                    //   $scope.OfficeListData = data.filter(x => x.OfficeId == 3 || x.OfficeId == 27 || x.OfficeId == 10 || x.OfficeId == 65      );// 3,27 district ,10,65 division
                    $scope.OfficeListData = data.filter(x => x.OfficeId == 3 || x.OfficeId == 27 || x.OfficeId == 2 || x.OfficeId == 65); //3,27 district 
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };



    $scope.BindInspectorDropdown = function (data) {



        var $dropdown = $('#ddlInspector');


        $dropdown.multiselect('destroy');

        $dropdown.empty();

        angular.forEach(data, function (itm) {

            var text = ($scope._language == 'en-us')
                ? itm.OfficerNameEn
                : itm.OfficerNameHi;

            $dropdown.append(
                $('<option></option>').val(itm.MappingID).text(text)
            );
        });

        $timeout(function () {

            $dropdown.multiselect({
                includeSelectAllOption: false,
                enableFiltering: false,
                enableCaseInsensitiveFiltering: false,
                buttonWidth: '100%',
                nonSelectedText: 'Select Inspector',
                maxHeight: 300,

                onChange: function (option, checked) {

                    var selected = $dropdown.find('option:selected');

                    if (selected.length > 2) {
                        $(option).prop('selected', false);

                        $dropdown.multiselect('refresh');

                        alert("Maximum 2 inspectors allowed");
                    }
                }
            });

        }, 0);
    };

    $scope.MultiInspectorList = [];
    $scope.GetAGMMultiInspectorDetails = function (id) {
        //debugger;
        return $http.get(SiteUrl + "/Society/GetAGMMultiInspectorDetails?id=" + id)
            .then(function (response) {
                $scope.MultiInspectorList = response.data;
                console.log("$scope.MultiInspectorList", $scope.MultiInspectorList);

            });
    };



    $scope.InspacorData = [];
    $scope.GetInspactor = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Master/InspectorList/?OfficeId=" + $scope.actionAGMObj.OfficeId + "&DistrictId=" + $scope.actionAGMObj.DistrictId + "&DivisionId=" + $scope.actionAGMObj.DivisionId).

            success(function (data, status) {
                $scope.InspacorData = data;
                // bind multiselect
                $scope.BindInspectorDropdown(data);
            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.getDropDownListName = function (list, selectedId, key, returnColumn) {
        selectedItem = list.find(function (item) { return item[key] == selectedId; });
        return selectedItem ? selectedItem[returnColumn] : null;
    };

    $scope.search = {};
    $scope.search.pendingType = "all";
    $scope.SubmitSurveyReport();

    //eSign 2.1 Added By Narpat Kumawat on 23-06-2026 
    $scope.RedirectToESPportal = function () {
        $('#mPacsdocumentAction').modal('hide');
        $scope.payload = [];
        $(".se-pre-con").fadeIn("slow");
        if ($scope.ValidationAction()) {
            if ($scope.chkBulkSelected && $scope.bulkAppStatusid != "0" && Array.isArray($scope.SelectedAppflowList) && $scope.SelectedAppflowList.length > 0) {
                angular.forEach($scope.SelectedAppflowList, function (item) {
                    var obj = angular.copy(item);
                    obj.statusid = $scope.actionObj.StatusId;
                    obj.Remarks = $scope.actionObj.Remarks;
                    obj.Inspectorfile = $scope.FileName;
                    obj.SanctionDate = $scope.actionObj.SanctionDate;
                    $scope.payload.push(obj);
                });

            } else {
                $scope.payload = [$scope.actionObj];

            }

            if (!$scope.payload || $scope.payload.length === 0) {
                return;
            }

            $http.post(SiteUrl + "/Society/SaveInspactorStatusDRForEsign", { InspactorStatusDetails: $scope.payload, Remarks: $scope.actionObj.Remarks, Inspectorfile: $scope.FileName, status: $scope.actionObj.StatusId }).success(function (response, status) {
                $(".se-pre-con").fadeOut("slow");
                if ($scope.actionObj.StatusId != "4") {
                    if (response.Success && response.Result == 1) {
                        ShowOkMessage("", $scope._language == 'en-us' ? "Successfully submitted." : "सफलतापूर्वक जमा कर लिया गया है।", function () {
                            window.location.href = "../society/InspactorTargetsAction";
                        });

                    } else {
                        ShowOkMessage("", $scope._language == 'en-us' ? "There is a problem while submitting the request." : "अनुरोध सबमिट करते समय समस्या आ रही है।",
                            function () { });
                    }
                    return;
                }

                $scope.dataPayload = response;
                $scope.esignData = {
                    SiteUrl: SiteUrl,
                    DocType: $scope.dataPayload.DocType,
                    ReturnPath: $scope.dataPayload.ReturnPath,
                    docid: $scope.dataPayload.Result,
                    SocietyIDEnc: $scope.payload.map(x => x.InspactorTargetsId).join(','),
                    DocumentPath: $scope.dataPayload.DocPath,
                    DocumentName: $scope.dataPayload.DocName,
                    Remark: $scope.actionObj.Remarks,
                    StatusId: $scope.actionObj.StatusId,
                    IsAmendment: false,
                    AmendmendAmount: "",
                    ReceiptNumber: "",
                    Details: $scope.payload,
                    Location: $scope.dataPayload.Location,
                    Desination: $scope.dataPayload.Designation
                };
                if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                    ShowOkMessage("Warning", _language == 'en-us' ? "Document not valid . Please try after some time." : "Document not valid . Please try after some time.", function () { });
                    return;
                }
                EsignCommonData.validateEsign($http, $scope.esignData, function (data) {
                    EsignCommonData.startEsignMPacs($http, $scope.esignData, function (xml) {
                        EsignCommonData.redirectToESP(xml);
                    });
                });
                $(".se-pre-con").fadeOut("slow");

            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
            });
        }
    }
    $scope.ShowResponseMessage = function () {
        $scope.responseMessage = $(".hdnResponse").val();
        if (!$scope.responseMessage || $scope.responseMessage.trim() === '') { return; }

        if ($scope.responseMessage == "1") {
            ShowOkMessage("", $scope._language == 'en-us' ? "Successfully submitted." : "सफलतापूर्वक जमा कर लिया गया है।",
                function () { $(".hdnResponse").val(''); });
        } else {
            ShowOkMessage("", $scope._language == 'en-us' ? "There is a problem while submitting the request." : "अनुरोध सबमिट करते समय समस्या आ रही है।",
                function () { $(".hdnResponse").val(''); });
        }
    };

    $scope.ViewMpacsDocument = function () {
        if ($scope.actionObj.StatusId == "4") {
            if (!$scope.actionObj.StatusId || $scope.actionObj.StatusId == "0") {
                ShowOkMessage("", _language == 'en-us' ? "Please select an action." : "कृपया एक्शन चुनें।", function () { });
                return;
            }

            if (!$scope.actionObj.Remarks || $scope.actionObj.Remarks.trim() === '') {
                ShowOkMessage("", _language == 'en-us' ? "Please enter remark." : "कृपया टिप्पणी दर्ज करें।", function () { });
                return;
            }
            $('#mPacsdocumentAction').modal('show');
        } else {
            $scope.RedirectToESPportal();
        }
    };

    $scope.ViewDocument = function () {
        $('#mPacsdocumentAction').modal('show');
        $scope.payload = [];
        $(".se-pre-con").fadeIn("slow");
        if ($scope.ValidationAction()) {
            if ($scope.chkBulkSelected && $scope.bulkAppStatusid != "0" && Array.isArray($scope.SelectedAppflowList) && $scope.SelectedAppflowList.length > 0) {
                angular.forEach($scope.SelectedAppflowList, function (item) {
                    var obj = angular.copy(item);
                    obj.statusid = $scope.actionObj.StatusId;
                    obj.Remarks = $scope.actionObj.Remarks;
                    obj.Inspectorfile = $scope.FileName;
                    obj.SanctionDate = $scope.actionObj.SanctionDate;
                    $scope.payload.push(obj);
                });

            } else {
                $scope.payload = [$scope.actionObj];

            }

            if (!$scope.payload || $scope.payload.length === 0) {
                return;
            }

            $http.post(SiteUrl + "/Society/SaveInspactorStatusDRForEsign", { InspactorStatusDetails: $scope.payload, Remarks: $scope.actionObj.Remarks, Inspectorfile: $scope.FileName, status: $scope.actionObj.StatusId }).success(function (response, status) {
                $(".se-pre-con").fadeOut("slow");
                if ($scope.actionObj.StatusId == "4") {
                    if (response.Success == true) {
                        $scope.dataPayload = response;
                        $scope.esignData = {
                            DocumentPath: response.DocPath, DocumentName: response.DocName, Result: response.Result
                        };

                        if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                            ShowOkMessage("Warning", _language == 'en-us' ? "Document not generated. Please try after some time." : "Document not generated. Please try after some time.", function () { });
                            return;
                        }

                        if ($scope.esignData.Result > 0) {
                            $('#mPacsdocumentAction').modal('hide');
                            ShowOkMessage("Success", _language == 'en-us' ? "Letter generated successfully." : "Letter generated successfully.", function () {
                                var url = "/Content/InspactorDocument/" + $scope.esignData.DocumentName;
                                window.open(url, '_blank');
                            });
                            return;
                        }
                        ShowOkMessage("Warning", _language == 'en-us' ? "Letter could not be generated. Please try after some time." : "Letter could not be generated. Please try after some time.", function () { });

                    } else {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    }
                    return;
                }
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try again later." : "आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद पुनः प्रयास करें।", function () { });
            });
        }
    };

    $scope.OnPageLoad();
});

MasterMod.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];

    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    var id = elem.attr("id");
                    const input = elem.target;
                    const fileInput = elem[0].files[0];
                    if (!fileInput) return;
                    const acceptTypes = elem.attr("accept").split(",");;

                    // check if file type matches allowed list
                    const isValid = acceptTypes.some(type => fileInput.type === type.trim());
                    if (!isValid) {
                        const fileTypeNames = {
                            "application/pdf": "PDF files",
                            ".pdf": "PDF files",
                            "application/vnd.ms-excel": "Excel files",
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": "Excel files",
                            ".xls": "Excel files",
                            ".xlsx": "Excel files",
                            "image/jpeg": "JPEG images",
                            "image/png": "PNG images"
                        };

                        elem[0].value = "";
                        let friendly = acceptTypes.map(t => fileTypeNames[t.trim()] || t.trim()).filter((v, i, arr) => arr.indexOf(v) === i); // remove duplicates
                        alert("Only " + friendly + " are allowed!");
                        scope.IsUploadFileName = false;
                    }

                    else if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 
                        var id = elem.attr("id");
                        var maxLength = scope.SocietyDocuments.filter(function (x) { return x.FileId == id })[0].Size;
                        if (file >= 4096) {

                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                            scope.IsUploadFileName = false;
                        } else {
                            if (scope.FileType == 1) {
                                scope.IsUploadFileName = true;
                            }
                            scope.$apply();

                        }
                    }


                });
            };
        }
    };



});



