
var AppClasses = angular.module("UnitMastersModule", ['angularUtils.directives.dirPagination']);
AppClasses.controller("UnitMasterCont", function ($scope, $http, $rootScope) {

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
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

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.actionObj = {};
    $scope.actionObjAGMF = {};
    $scope.ApplicationFlowMaster = {};

    $scope.UserRegistration = {};
    $scope.SaveUnitForm = function () {
        $http.post(SiteUrl + "/Master/SaveUnitForm", $scope.UnitMaster).
            success(function (data, status) {
                // alert(data);
                if (data > 0) {
                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearUnitForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearUnitForm();
                    return;
                }
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSubUnitList();
                if (data.Status) {

                    $scope.UnitMaster = null;
                    $scope.ClearUnitForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };

    // on page load
    $scope.OnPageLoad = function () {
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



        $scope.Department = [];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=INSPECTOR&MasterCode=-1").

        //    success(function (data, status) {

        //        $scope.Department = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Department = [];
        //    });

    };

    $scope.ActionforGN = function (item) {
        $scope.actionObj = {};
        //debugger;
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

        $scope.GSSTargetProceesStatusList(item.InspectorStatusId, item.InspactorTargetsId).then(function () { $scope.actionObj.StatusId = "17"; });

        $('#Actionmodal_GN').modal('show');
    }

    //Added by jaya

    $scope.SaveGSSApplicationFlowStatus = function (isResend) {

        $scope.IsResendOTP == false;
        $scope.ApplicationFlowMaster.TXN = '';

        $http.get("../Society/GenerateAadhaarOTPForEsingAuditOrder").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.indexOf('NO#') >= 0) {
                    alert("Due to some issue OTP not send");
                } else {
                    if (data != "LOGOUT") {
                        $scope.ApplicationFlowMaster.TXN = data;

                        if ($scope.IsResendOTP == true) {
                            alert(_language == 'en-us' ? 'OTP has been re-sent to Aadhaar mobile number.\n' : 'ओटीपी आधार मोबाइल नंबर पर पुनः भेजा गया है|\n');
                        } else {
                            alert(_language == 'en-us' ? 'OTP has been sent to Aadhaar mobile number.\n' : 'ओटीपी आधार मोबाइल नंबर पर भेजा गया है|\n');
                        }

                        //$scope.ShowOTPDiv = true;

                        $('#otpmodal').modal('show');
                        //$scope.SaveforGN();
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

    };

    var payloadGN = [];
    $scope.SaveforGN = function () {
        payloadGN = [];
        payloadGN = [$scope.actionObj];
        var StatusId = $scope.actionObj.StatusId;
        var AGMDateObj = new Date($scope.actionObj.AGMDate ?? new Date());
        $scope.actionObj.AGMDate = (AGMDateObj.getMonth() + 1) + '/' + AGMDateObj.getDate() + '/' + AGMDateObj.getFullYear();
        //debugger;
        var localTime = $scope.actionObj.AGMTime;
        var utcDate = new Date(localTime.getTime() - localTime.getTimezoneOffset() * 60000);
        let hours = String(utcDate.getUTCHours()).padStart(2, '0');
        let minutes = String(utcDate.getUTCMinutes()).padStart(2, '0');
        let utcTime = `${hours}:${minutes}`;
        //var formattedAGMTime = formatTime($scope.actionObj.AGMTime);

        if ($scope.ValidateAddGN()) {
            $http.post(SiteUrl + "/Society/SaveGNStatus", { InspactorStatusDetails: payloadGN, Remarks: $scope.actionObj.Remarks, agmTime: utcTime, status: StatusId, agmDate: $scope.actionObj.AGMDate, agmPlace: $scope.actionObj.AGMPlace }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            $("#Actionmodal_GN").modal("hide");
                            window.location.href = "../society/InspactorGSS";
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

    $scope.ValidateAddGN = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.actionObj.AGMDate == "" || $scope.actionObj.AGMDate == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please Enter AGM Date.' : '<br />कृपया AGM Date दर्ज़ करें।';
        }
        if ($scope.actionObj.AGMTime == "" || $scope.actionObj.AGMTime == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please Enter AGM Time.' : '<br />कृपया AGM Time दर्ज़ करें।';
        }
        if ($scope.actionObj.AGMPlace == "" || $scope.actionObj.AGMPlace == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please Enter AGM Place.' : '<br />कृपया AGM Place दर्ज़ करें।';
        }

        if ($scope.actionObj.StatusId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Action.' : '<br />कृपया कार्रवाई का चयन करें।';
        }

        if ($scope.actionObj.Remarks == "" || $scope.actionObj.Remarks == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया Remarks दर्ज़ करें।';
        }

        if (message.length > 29) {
            ShowOkMessage("CONDUCT FOR AGM ", message, function () { });
            $scope.checksta = false;
            return false;
        }
        $scope.checksta = true;
        return true;

    }

    $scope.ActionforAGMFormation = function (item) {
        $scope.actionObjAGMF = {};
        //debugger;
        $scope.actionObjAGMF.InspactorTargetsId = item.InspactorTargetsId;
        $scope.actionObjAGMF.Inspector_ID = item.Inspector_ID;
        $scope.actionObjAGMF.InspacorName = item.InspacorName;
        $scope.actionObjAGMF.PrecentNameGSS = item.PrecentNameGSS;
        $scope.actionObjAGMF.NewGSSName = item.NewGSSName;
        $scope.actionObjAGMF.InspectorStatusId = item.InspectorStatusId;
        $scope.actionObjAGMF.IsTspArea = item.IsTspArea;
        $scope.actionObjAGMF.NewGSSFinanceAmount = Number(item.NewGSSFinanceAmount) || 0;
        $scope.actionObjAGMF.NewGSSprecious = Number(item.NewGSSprecious) || 0;
        $scope.actionObjAGMF.NoofMember = Number(item.NoofMember) || 0;
        $scope.actionObjAGMF.AreaType = item.AreaType;
        $scope.actionObjAGMF.DrName = item.DrName;
        $scope.actionObjAGMF.IsMahilaMpacs = item.IsMahilaMpacs;

        $scope.GSSTargetProceesStatusList(item.InspectorStatusId, item.InspactorTargetsId).then(function () { $scope.actionObjAGMF.StatusId = "0"; });

        $('#Actionmodal_AGMF').modal('show');
    }

    $scope.VerifyOTP = function () {
        // $scope.SaveGSSApplicationFlowStatus(true);
        $("#otpmodal").modal("hide");
        //if (!$scope.ApplicationFlowMaster.OTP) {
        //    alert("Enter OTP");
        //    return;
        //}

        // 👉 CALL FINAL SAVE HERE
        $scope.SaveforGN();
    };


    var payloadAGMF = [];
    $scope.SaveAGMFormation = function () {
        //debugger;
        payloadAGMF = [];
        payloadAGMF = [$scope.actionObjAGMF];
        var StatusId = $scope.actionObjAGMF.StatusId;
        var AGMConductDateObj = new Date($scope.actionObjAGMF.AGMConductDate ?? new Date());
        var uploadDocs = $scope.actionObjAGMF.UploadDocument;
        $scope.actionObjAGMF.AGMConductDate = (AGMConductDateObj.getMonth() + 1) + '/' + AGMConductDateObj.getDate() + '/' + AGMConductDateObj.getFullYear();
        //debugger;
        if ($scope.ValidateAddAGMFormation()) {
            $http.post(SiteUrl + "/Society/SaveAGMFStatus", { InspactorStatusDetails: payloadAGMF, Remarks: $scope.actionObjAGMF.Remarks, IsAGMConduct: $scope.actionObjAGMF.IsAGMConduct, status: StatusId, agmConductDate: $scope.actionObjAGMF.AGMConductDate, agmReasons: $scope.actionObjAGMF.AGMReasons, fileNames: $scope.actionObjAGMF.UploadDocument }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            $("#Actionmodal_AGMF").modal("hide");
                            window.location.href = "../society/InspactorGSS";
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



    $scope.SelectReceiptFile = function (files) {
        if (files && files.length > 0) {
            $scope.actionObjAGMF = $scope.actionObjAGMF || {};
            $scope.actionObjAGMF.UploadDocument = $scope.actionObjAGMF.UploadDocument || {};
            $scope.actionObjAGMF.UploadDocument = files[0].name;
            $scope.UploadDocument(files);
        }
    };

    $scope.UploadDocument = function (files) {
        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("UploadDocument", file);

        $http.post(SiteUrl + "/Society/UploadDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.actionObjAGMF.UploadDocument = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };

    $scope.RemoveDocument = function () {
        if (!confirm("Are you sure you want to remove this document?"))
            return;

        var fileName = $scope.PLDB.LoanDisbursement.LoanDocument;

        $http.post(SiteUrl + "/Society/DeleteLoanDocument", { fileName: fileName })
            .then(function () {
                $scope.PLDB.LoanDisbursement.LoanDocument = "";
                document.getElementById("flReportFile").value = "";
            }, function () {
                alert("Failed to remove document");
            });
    };





    $scope.ValidateAddAGMFormation = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.actionObjAGMF.IsAGMConduct == "" || $scope.actionObjAGMF.IsAGMConduct == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please  Select AGM Conduct option.' : '<br />कृपया AGM कार्रवाई का चयन करें।';
        }
        if ($scope.actionObjAGMF.IsAGMConduct == 1) {

            if ($scope.actionObjAGMF.AGMConductDate == "" || $scope.actionObjAGMF.AGMConductDate == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter AGM Conduct Date.' : '<br />कृपया AGM Conduct Date दर्ज़ करें।';
            }

            if ($scope.actionObjAGMF.StatusId == "0") {
                message += $scope._language == 'en-us' ? '<br />Please  Select Action.' : '<br />कृपया कार्रवाई का चयन करें।';
            }

            if ($scope.actionObjAGMF.Remarks == "" || $scope.actionObjAGMF.Remarks == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया Remarks दर्ज़ करें।';
            }

        }
        if ($scope.actionObjAGMF.IsAGMConduct == 0) {

            if ($scope.actionObjAGMF.AGMReasons == "" || $scope.actionObjAGMF.AGMReasons == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Reasons.' : '<br />कृपया Reasons दर्ज़ करें।';
            }
        }

        if (message.length > 29) {
            ShowOkMessage("AGM Formation ", message, function () { });
            $scope.checksta = false;
            return false;
        }
        $scope.checksta = true;
        return true;

    }

    function formatTime(date) {
        const h = String(date.getHours()).padStart(2, '0');
        const m = String(date.getMinutes()).padStart(2, '0');
        return `${h}:${m}`;
    };

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

    $scope.backStaushistory = function () {
        $scope.IsShowAssignForm = false;
        $scope.iscomplete = true;
        $scope.InspectorStatus = false;
    }

    $scope.historyshow = function (item) {

        $scope.IsShowAssignForm = true;
        $scope.statusid = 8;
        $scope.iscomplete = false;
        $scope.InspectorStatus = true;
        $scope.InspactorTargetsId = item.InspactorTargetsId;
        $scope.GetInspectorStatusList($scope.InspactorTargetsId)
    }
    $scope.InspectorStatus = false;


    $scope.ComplainTrailFiles = {
        FileName: "",
        IsUploadFileName: false,
    };
    $scope.AssignNew = function (item) {
        $scope.cityhide = false;
        if (item.GP_ENG == '' || item.GP_ENG == null) {
            $scope.InspectorData.CityName = item.CITY_ENG;
            $scope.citydisabled = true;
            $scope.cityhide = true;
        }
        else {
            $scope.gramdisabled = true;
            $scope.InspectorData.GramPanchayatName = item.GP_ENG;
            $scope.InspectorData.BlockName = item.BLOCK_ENG;


        }
        $scope.InspectorData.PrecentNameGSS = '';
        $scope.InspectorData.NewGSSName = '';
        $scope.InspectorData.NoofMember = '';
        $scope.InspectorData.NewGSSprecious = '';
        $scope.InspectorData.NewGSSFinanceAmount = '';
        $scope.InspectorData.Remarks = '';

        $scope.InspectorData.StatusId = '0';
        $scope.ComplainTrailFilesList = [];
        $scope.ComplainTrailFilesList.push($scope.ComplainTrailFiles);
        $scope.iscomplete = true;

        $scope.IsShowAssignForm = true;
        $scope.statusid = item.InspectorStatusId;
        $scope.IsTspArea = item.IsTspArea;
        $scope.AreaType = item.AreaType;
        $scope.IsMahilaMpacs = item.IsMahilaMpacs;


        console.log(item);
        $scope.InspectorData.Inspector_ID = item.Inspector_ID;
        $scope.InspectorData.InspactorTargetsId = item.InspactorTargetsId;
        $scope.CommonDistrictId = item.DistictId;




        if ($scope.GSSStatusList.length === 1) {
            $scope.InspectorData.StatusId = $scope.GSSStatusList[0].StatusId;
        }

        //madhuri

        $scope.ClearBindingGssData();
        $scope.DistrictId = item.DistictId.toString();
        $scope.getBlock();

    }

    //GetInspectorStatusList
    $scope.InspectorStatus = false;
    $scope.GetInspectorStatusList = function (InspactorTargetsId) {

        $http.post("/Society/GetInspectorStatusDetails", JSON.stringify({ InspactorTargetsId: InspactorTargetsId })).
            success(function (data, status) {

                $scope.GetInspectorStatus = JSON.parse(data.DataList);
                if ($scope.GetInspectorStatus.length > 0) {
                    $scope.InspectorStatus = true;
                }
                else {
                    $scope.InspectorStatus = false;
                }
            }).
            error(function (data, status) {
                $scope.GetInspectorStatus = [];
                alert('Error while Society Election Detail data.');
            });
    }

    $scope.InspacorData = [];
    $scope.GSSStatusList = [];

    //$scope.GSSStatusList.push({ StatusId: "1", StatusNameEn: "Done Survey", StatusNameHi: "Done Survey" })
    $scope.GSSStatusList.push({ StatusId: "1", StatusNameEn: "Complete Survey Report", StatusNameHi: "Complete Survey Report" })
    // $scope.GSSStatusList.push({ StatusId: "2", StatusNameEn: "Not Feasible", StatusNameHi: "Not Feasible" })


    $scope.GSSStatusList1 = [];
    $scope.GSSStatusList1.push({ StatusId: "5", StatusNameEn: "Registered", StatusNameHi: "Registered" })




    $scope.GetInspactor = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=INSPECTOR&MasterCode=-1").
            success(function (data, status) {
                $scope.InspacorData = data;

            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }


    $scope.IsShowAssignForm = false;





    $("#HouseBuildingNo_MemberAdd").parent().hide();
    $("#LaneStreetName_MemberAdd").parent().hide();
    $("#Locality_MemberAdd").parent().hide();
    $("#ddlstate_MemberAdd").parent().hide();
    $("#ddlDivision_MemberAdd").parent().hide();
    $("#ddlDivision_MemberAdd").parent().hide();
    //$("#district_MemberAdd").parent().hide();
    $("#district_MemberAdd").attr("disabled", "disabled");
    $("#ddlward_MemberAdd").parent().hide();
    $("#ddlVillage_MemberAdd").parent().hide();
    $("#pincode_MemberAdd").parent().hide();






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

    $scope.OnPageLoad();
    $scope.GetInspactor();


    $scope.OtherPersonList = [];
    $scope.OtherPersonListNew = [];
    //$scope.OtherPersonList.push({ "PersonId": 0, "AdId": 0, "Name": "", "Address": "", "Post": "", "Mobile": "" });
    //$scope.OtherPersonListNew.push({ "PersonId": 0, "AdId": 0, "Name": "", "Address": "", "Post": "", "Mobile": "" });

    $scope.OtherPersonListNew.push({ "Inspector": "OM Prakash", "Urban_Rural": "Rural", "City_Block": "Churu", "GramPanchayat": "Jobner", "Inspector_ID": 1, "City_Block_ID": 3, "GramPanchayat_ID": 5 });


    $scope.InspactorId = "0";
    $scope.AddGSS = function () {
        if ($scope.ValidateAddGSS()) {
            var data = GetAddressinfo('MemberAdd');
            var InspacorName = $scope._language == 'en-us' ? $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspactorId })[0].OfficerNameEn : $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspacorId })[0].OfficerNameHi;
            $scope.OtherPersonListNew.push({ "Inspector": InspacorName, "Urban_Rural": data.Urban_Rural, "City_Block": data.Checkboxvalue == "2" ? data.Block_Name : data.City_Name, "GramPanchayat": data.Checkboxvalue == "2" ? data.Panchayat_Name : "", "Inspector_ID": $scope.InspacorId, "City_Block_ID": data.Checkboxvalue == "2" ? data.BlockId : data.CityId, "GramPanchayat_ID": data.PanchayatId });
        }
    }



    $scope.ClearSearch = function () {

        $scope.StatusId = '0';
        $scope.GetInspactorTargetsDetails(-1);
    }
    $scope.StatusId = 0;
    $scope.GetStatuswisedata = function (StatusId) {
        $scope.StatusId = StatusId;
        $scope.GetInspactorTargetsDetails(-1);
    }


    $scope.GetInspactorTargetsDetails = function (month) {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        $http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: 0, StatusId: $scope.StatusId, GSSSStatusId: 0, Flag: 1, Month: month })).
            success(function (data, status) {
                $scope.InspactorTargetsListDetils = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.InspactorTargetsListDetils = [];
                alert('Error while Society Election Detail data.');
            });
    };

    $scope.GetInspactorTargetsDetails(-1);




    $scope.ValidateInspactor = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';



        if ($scope.InspectorData.Remarks == "" || $scope.InspectorData.Remarks == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Remark.' : '<br />कृपया टिप्पणी दर्ज करें।';
        }
        if ($scope.InspectorData.StatusId == undefined || $scope.InspectorData.StatusId == "" || $scope.InspectorData.StatusId == '0') {
            message += $scope._language == 'en-us' ? '<br />Please  Select Status.' : '<br />कृपया स्थिति चुनें।';
        }
        //if ($scope.ComplainTrailFilesList.length == 1 ) {
        //    message += $scope._language == 'en-us' ? '<br />Please  upload file.' : '<br />कृपया फ़ाइल अपलोड करें।';
        //}

        if ($scope.InspectorData.StatusId != undefined && $scope.InspectorData.StatusId == "1") {

            if ($scope.InspectorData.PrecentNameGSS == "" || $scope.InspectorData.PrecentNameGSS == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Present M.PACS Name.' : '<br />कृपया वर्तमान एम.पैक्स समिति का नाम दर्ज करें।';
            }
            if ($scope.InspectorData.NewGSSName == undefined || $scope.InspectorData.NewGSSName == "") {
                message += $scope._language == 'en-us' ? '<br />Please Enter New M.PACS Name.' : '<br />कृपया नई प्रस्तावित एम.पैक्स समिति का नाम दर्ज करें।';
            }
            if ($scope.InspectorData.NoofMember == "" || $scope.InspectorData.NoofMember == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter New M.PACS No.of member.' : '<br />नई प्रस्तावित एम.पैक्स समिति की सदस्य संख्या।';
            }
            if ($scope.InspectorData.NewGSSprecious == undefined || $scope.InspectorData.NewGSSprecious == "") {
                message += $scope._language == 'en-us' ? '<br />Please Enter New M.PACS Amaanat.' : '<br />नई प्रस्तावित एम.पैक्स समिति की अमानत।';
            }
            if ($scope.InspectorData.NewGSSFinanceAmount == "" || $scope.InspectorData.NewGSSFinanceAmount == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter New M.PACS Share capital.' : '<br />नई प्रस्तावित एम.पैक्स समिति की हिस्सा पूंजी।';
            }


            //if ($scope.InspectorData.NewGSSFinanceAmount == "" || $scope.InspectorData.NewGSSFinanceAmount == undefined) {
            //    message += $scope._language == 'en-us' ? '<br />Please Enter New MPACS Share capital.' : '<br />नई प्रस्तावित ग्राम सेवा सहकारी समिति की हिस्सा पूंजी।';
            //}


            if (($scope.IsTspArea || (!$scope.IsTspArea && $scope.AreaType == "2")) && $scope.InspectorData.NoofMember < 75) {
                message += $scope._language == 'en-us' ? '<br />Number of members should be equal or greater than 75.' : '<br />सदस्यों की संख्या 75 के बराबर या उससे अधिक होनी चाहिए।';
            }

            if (!$scope.IsTspArea && $scope.InspectorData.NoofMember < 150) {
                message += $scope._language == 'en-us' ? '<br />Number of members should be equal or greater than 150.' : '<br />सदस्यों की संख्या 150 के बराबर या उससे अधिक होनी चाहिए।';
            }

            if ($scope.IsMahilaMpacs == false && ($scope.IsTspArea || (!$scope.IsTspArea && $scope.AreaType == "2")) && $scope.InspectorData.NewGSSFinanceAmount < 100000) {
                message += $scope._language == 'en-us' ? '<br />Amount of share capital should be equal or greater than Rs 1 lakh.' : '<br />कृपया 1 लाख रुपये से अधिक या उसके बराबर प्रस्तावित एम.पैक्स शेयर पूंजी दर्ज करें।।';
            }


            if (!$scope.IsTspArea && $scope.AreaType != "2" && $scope.InspectorData.NewGSSprecious < 75000) {
                message += $scope._language == 'en-us' ? '<br />Amount of proposed MPACS should be equal or greater than Rs 75,000.' : '<br />कृपया 75,000 रुपये से अधिक या उसके बराबर प्रस्तावित एम.पैक्स दर्ज करें।।';
            }

            if ($scope.IsMahilaMpacs == false && !$scope.IsTspArea && $scope.AreaType != "2" && $scope.InspectorData.NewGSSFinanceAmount < 150000) {
                message += $scope._language == 'en-us' ? '<br />Amount of share capital should be equal or greater than Rs 1 lakh 50 Thousand.' : '<br />कृपया 1 लाख 50 हजार रुपये से अधिक या उसके बराबर प्रस्तावित एम.पैक्स शेयर पूंजी दर्ज करें।।';
            }



        }

        if ($scope.InspectorData.StatusId != undefined && $scope.InspectorData.StatusId == "5") {

            if ($scope.InspectorData.RagistrationNo == "" || $scope.InspectorData.RagistrationNo == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Ragistration No.' : '<br />कृपया रजिस्ट्रेशन संख्या दर्ज करें।';
            }
            if ($scope.InspectorData.GssName == undefined || $scope.InspectorData.GssName == "") {
                message += $scope._language == 'en-us' ? '<br />Please Enter GSS Name.' : '<br />कृपया   ग्राम सेवा सहकारी समिति का नाम दर्ज करें।';
            }
            if ($scope.InspectorData.Address == "" || $scope.InspectorData.Address == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Address.' : '<br /> कृपया पता दर्ज करें।';
            }

        }

        if ($scope.ComplainTrailFilesList.length == 1) {
            if ($scope.ComplainTrailFilesList[0].FileName == "")
                message += ($scope._language == 'en-us' ? '<br />Please upload Survey Report .' : '<br />कृपया सर्वेक्षण रिपोर्ट अपलोड करें।');
        }

        if (message.length > 29) {
            ShowOkMessage("Validation ", message, function () { });
            return false;
        }
        return true;
    }

    $scope.SaveInspectorstatus = function () {

        if (!$scope.AddedGSSList || $scope.AddedGSSList.length === 0) {
            alert("Please add at least one present M.PACS");
            return;
        }

        if ($scope.AddedGSSList.length != (Number($scope.NoOfAddMpacs) || 0)) {
            //    alert("Please add Remaining Mention the number of existing M.PACS from which the porposed M.PACS is being constituted .");
            alert("Add all " + (Number($scope.NoOfAddMpacs) || 0) + " existing MPACS details from which new MPACS is being constituted.");
            return;
        }

        $scope.InspectorData.IsMahilaMpacs = $scope.IsMahilaMpacs;
        // Extract SocietyIds and join with comma
        $scope.InspectorData.PrecentNameGSS = $scope.AddedGSSList.map(function (x) { return x.SocietyId; }).join(",");


        if ($scope.ValidateInspactor()) {
            $(".se-pre-con").fadeIn("slow");
            var Inspectorfile = '';
            var InspectorFilesList = [];
            $scope.ComplainTrailFilesList.forEach(function (itm) {
                if (itm.FileName != '' && itm.FileName != 'undefined') {
                    Inspectorfile += (Inspectorfile != '' ? ',' : '') + itm.FileName;
                    // encloserfile += ',';
                }
            });
            $scope.InspectorData.Inspectorfile = Inspectorfile;
            $http.post(SiteUrl + "/Society/SaveInspactorstausDetails", { "_InspactorStatusDetails": $scope.InspectorData }).
                success(function (data, status) {
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            $scope.StatusId = '0';
                            $scope.Remarks = '';
                            $scope.GetInspactorTargetsDetails(-1);
                            $scope.IsShowAssignForm = false;
                            $scope.iscomplete = false;
                            $scope.InspectorStatus = false;
                            $scope.ComplainTrailFilesList = [];
                            $scope.ComplainTrailFilesList.push($scope.ComplainTrailFiles);

                        });

                        $(".se-pre-con").fadeOut("slow");
                    }

                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                })
        }

    }




    $scope.backtomove = function () {
        $scope.IsShowAssignForm = false;
    }

    $scope.RemoveOtherPersonListRow = function (index) {
        $scope.OtherPersonListNew.splice(index, 1);
        $scope.$apply();
    }

    $scope.ComplainUndeVlaue = {};
    $scope.ValidateAddGSS = function () {
        var data = GetAddressinfo('MemberAdd');
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.InspactorId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Inspactor.' : '<br />कृपया इंस्पेक्टर चुनें।';
        }

        if (data.Checkboxvalue == undefined || data.Checkboxvalue == "") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Urban/Rural.' : '<br />कृपया शहरी/ग्रामीण चुनें।';
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "2" && data.BlockId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Block.' : '<br />कृपया ब्लॉक चुनें।';
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "2" && data.PanchayatId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत चुनें।';
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "1" && data.CityId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select City .' : '<br />कृपया सिटी चुनें।';
        }

        if (message.length > 29) {
            ShowOkMessage("Society Complaint ", message, function () { });
            return false;
        }
        return true;
    }


    //$scope.ComplainTrailFilesList = [];
    //$scope.ComplainTrailFilesList.push($scope.ComplainTrailFiles);
    $scope.ComplainTrailFiles = {
        //IsUploadFileName: "",
        FileName: "",
        IsUploadFileName: false,
    };

    $scope.ComplainTrailFilesList = [];
    $scope.ComplainTrailFilesList.push($scope.ComplainTrailFiles);

    $scope.FileType = "";
    $scope.FileIndex = 0;


    $scope.AddFile = function (type, index) {
        $scope.FileType = type;
        $scope.FileIndex = index;
    }
    $scope.ChangeFile = function (id, type, index) {
        if (type == 1) {
            $scope.ComplainTrailFilesList[index].IsUploadFileName = false;
            $scope.ComplainTrailFilesList[index].FileName = "";
        } else if ($scope.FileType == -1) {
            //scope.IsUploadReportFileName = true;
            $scope.ReportFileName = "";
        }
        $("#" + id).val("");
    }

    $scope.UploadFile = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                // $scope.xhr(SiteUrl + "/SocietyComplaintCourtModule/UploadComplainFile", formData, type);
                $scope.xhr(SiteUrl + "/Society/UploadinInspactorFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }



    $scope.xhr = function (url, data, type) {
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
                            $scope.ComplainTrailFilesList[$scope.FileIndex].FileName = request.responseText;

                            //if ($scope.ComplainTrailFilesList.filter(function (x) { return x.IsUploadFileName === false }).length <= 0) {
                            //    $scope.ComplainTrailFilesList.push({
                            //        //IsUploadFileName: "",
                            //        FileName: "",
                            //        IsUploadFileName: false,
                            //    });
                            //}
                        } else if ($scope.FileType == -1) {
                            //scope.IsUploadReportFileName = true;
                            $scope.ReportFileName = request.responseText;
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
    $scope.deleteFime = function (index) {
        var list = $scope.ComplainTrailFilesList;
        $scope.ComplainTrailFilesList = [];
        list.forEach(function (item, indx) {
            if (indx != index) {
                $scope.ComplainTrailFilesList.push(item);
            }
        });
    }


    //madhuri
    $scope.DistrictListData = function () {


        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictLst = data;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DistrictLst = [];
            });
    };
    $scope.DistrictListData();

    $scope.ClearBindingGssData = function () {
        $scope.AddedGSSList = [];
        $scope.UserRegistration.DistrictId = "0";
        $scope.PresentGSSList = [];
        $scope.InspectorData.PrecentNameGSS = '0';
        $scope.DistrictId = '0';

        $scope.BlockDataList = [];
        $scope.BlockId = '0';
        $scope.GramPancyatList = [];
        $scope.PanchayatId = '0';

        $scope.DistrictLst = [];
        $scope.SearchMPACS = '';
        $scope.DistrictListData();
    }


    $scope.getBlock = function () {
        $scope.PresentGSSList = [];
        $scope.InspectorData.PrecentNameGSS = '0';

        if ($scope.DistrictId && $scope.CommonDistrictId && $scope.DistrictId.toString() !== $scope.CommonDistrictId.toString()) {
            if (!confirm("The selected districts are not the same. Do you want to continue?")) {
                $scope.DistrictId = "0";
                return false;
            }
        }
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.DistrictId).
            success(function (data, status) {
                $scope.BlockDataList = data;
                $scope.BlockId = '0';
                $(".se-pre-con").fadeOut("slow");
                // _callback(Addressgetinfo);
            }).
            error(function (data, status) {
                $scope.BlockDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.getGrampanchyat = function () {
        $scope.PresentGSSList = [];
        $scope.InspectorData.PrecentNameGSS = '0';

        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.BlockId).success(function (data, status) {
            $scope.GramPancyatList = data;
            $scope.PanchayatId = '0';
            $(".se-pre-con").fadeOut("slow");
        }).
            error(function (data, status) {
                $scope.GramPancyatList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    }
    // Load Present GSS when Panchayat changes
    $scope.getPresentGSS = function (panchayatId) {
        // API call to fetch GSS list for Panchayat
        // $scope.PresentGSSList = result;
    };

    $scope.AddedGSSList = $scope.AddedGSSList || [];

    $scope.addGSS = function (gssId) {
        if (!gssId || gssId === "0") return; // ignore if nothing selected

        if ($scope.AddedGSSList.length > (Number($scope.NoOfAddMpacs) || 0) - 1) {

            alert("You have already added details of existing MPACS according to the count you have given. .");
            return;
        }
        var selected = $scope.PresentGSSList.find(function (x) { return x.SocietyId == gssId; });

        if (selected && !$scope.AddedGSSList.some(function (x) { return x.SocietyId === selected.SocietyId; })) {
            $scope.AddedGSSList.push(angular.copy(selected)); // push a copy to avoid binding issues
        }
    };

    $scope.removeGSS = function (index) {
        $scope.AddedGSSList.splice(index, 1);
    };



    $scope.societysearch = function (callback) {
        $scope.DataPaging.searchBoxDivId = 'search-societies',
            $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                $scope.DataPaging.searchParameters.push(parameter);
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }

    $scope.validateGSS = function () {
        $scope.InspectorData.PrecentNameGSS = "0";
        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": 500, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/MIS/GccLoanReports", JSON.stringify({ Details: jsonParameters, Exportjson: null, Action: "MPacsData" })).
            success(function (responcedata, status) {
                {
                    if (responcedata.DataList != null && responcedata.DataList.length > 0) {
                        var data = JSON.parse(responcedata.DataList);
                        $scope.PresentGSSList = data; // JSON.parse(data);
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (responcedata, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentGSSList = [];
            });
    };

    $scope.InspactorTargetsDetils = [];
    $scope.GetInspactorTargetsDetailsByIds = function (month) {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        return $http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: 0, StatusId: 13, GSSSStatusId: $scope.GSSSStatusId, Flag: 1, Month: month })).
            success(function (data, status) {
                $scope.InspactorTargetsDetils = JSON.parse(data.DataList);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.InspactorTargetsListDetils = [];
                alert('Error while Society Election Detail data.');
            });
    };


    $scope.EditForm = function (item) {
        $scope.AddedGSSList = [];
        $scope.InspectorData = {};

        if (item.GP_ENG == '' || item.GP_ENG == null) {
            $scope.InspectorData.CityName = item.CITY_ENG;
            $scope.citydisabled = true;
            $scope.cityhide = true;
        }
        else {
            $scope.gramdisabled = true;
            $scope.InspectorData.GramPanchayatName = item.GP_ENG;
            $scope.InspectorData.BlockName = item.BLOCK_ENG;
        }

        $scope.GSSSStatusId = item.InspactorTargetsId;
        $scope.iscomplete = true;
        $scope.IsShowAssignForm = true;
        $scope.InspectorData.Inspector_ID = item.Inspector_ID;
        $scope.InspectorData.InspactorTargetsId = item.InspactorTargetsId;

        $scope.IsTspArea = item.IsTspArea;
        $scope.AreaType = item.AreaType;
        $scope.IsMahilaMpacs = item.IsMahilaMpacs;


        $scope.GetInspactorTargetsDetailsByIds().then(function () {

            $scope.InspectorData.NewGSSName = $scope.InspactorTargetsDetils[0].NewGSSName;
            $scope.InspectorData.NewGSSFinanceAmount = $scope.InspactorTargetsDetils[0].NewGSSFinanceAmount;
            $scope.InspectorData.NewGSSprecious = $scope.InspactorTargetsDetils[0].NewGSSprecious;
            $scope.InspectorData.NoofMember = $scope.InspactorTargetsDetils[0].NoofMember;
            $scope.InspectorData.StatusId = "1";
            $scope.AddedGSSList = $scope.InspactorTargetsDetils;
            $scope.NoOfAddMpacs = $scope.AddedGSSList.length.toString();
            //$scope.ComplainTrailFilesList = $scope.InspactorTargetsDetils[0].SurveyReport;
            item.IsUploadFileName = $scope.InspactorTargetsDetils[0].SurveyReport && $scope.InspactorTargetsDetils[0].SurveyReport != '' ? true : false;

            // alert($scope.InspactorTargetsDetils[0].SurveyReport);
            if ($scope.InspactorTargetsDetils[0].SurveyReport != 'undefined' && $scope.InspactorTargetsDetils[0].SurveyReport != null && $scope.InspactorTargetsDetils[0].SurveyReport != '') {
                $scope.ComplainTrailFilesList[0].IsUploadFileName = true;
                $scope.ComplainTrailFilesList[0].FileName = $scope.InspactorTargetsDetils[0].SurveyReport;

            }


            console.log($scope.InspactorTargetsDetils);
        });


    }

    //New esign process for 2.1    
    $scope.RedirectToESPportal = function () {
        $scope.GenerateAGMNoticeLetter().then(function (response) {
            $scope.dataPayload = response;
            if (!$scope.dataPayload) {
                return;
            }
            console.table($scope.dataPayload);
            $scope.payloadGN = [];
            $scope.payloadGN = [$scope.actionObj];
            var localTime = $scope.actionObj.AGMTime;
            var utcDate = new Date(localTime.getTime() - localTime.getTimezoneOffset() * 60000);
            let hours = String(utcDate.getUTCHours()).padStart(2, '0');
            let minutes = String(utcDate.getUTCMinutes()).padStart(2, '0');
            let utcTime = `${hours}:${minutes}`;

            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                docid: $scope.payloadGN[0].InspactorTargetsId,
                SocietyIDEnc: utcTime,
                Remark: $scope.actionObj.Remarks,
                StatusId: $scope.actionObj.StatusId,
                IsAmendment: "",
                AmendmendAmount: $scope.actionObj.AGMDate,
                ReceiptNumber: $scope.actionObj.AGMPlace,
                Location: $scope.dataPayload.Location,
                Desination: $scope.dataPayload.Designation,
                PurposeOfLoanOther: JSON.stringify($scope.payloadGN)
            };

            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }
            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (error) {
            console.error(error);
            alert("Error while generating AGM Notice Letter");
        });
    };

    $scope.GenerateAGMNoticeLetter = function () {
        var payloadGN = [$scope.actionObj];
        var StatusId = $scope.actionObj.StatusId;

        var AGMDateObj = new Date($scope.actionObj.AGMDate ?? new Date());
        $scope.actionObj.AGMDate = (AGMDateObj.getMonth() + 1) + '/' + AGMDateObj.getDate() + '/' + AGMDateObj.getFullYear();
        var localTime = $scope.actionObj.AGMTime;

        var utcDate = new Date(localTime.getTime() - localTime.getTimezoneOffset() * 60000);

        let hours = String(utcDate.getUTCHours()).padStart(2, '0');
        let minutes = String(utcDate.getUTCMinutes()).padStart(2, '0');
        let utcTime = `${hours}:${minutes}`;

        if (!$scope.ValidateAddGN()) {
            return $q.reject("Validation failed");
        }

        $(".se-pre-con").fadeOut("slow");

        return $http.post(SiteUrl + "/Society/AGMLetter", {
            InspactorStatusDetails: payloadGN,
            Remarks: $scope.actionObj.Remarks,
            agmTime: utcTime,
            status: StatusId,
            agmDate: $scope.actionObj.AGMDate,
            agmPlace: $scope.actionObj.AGMPlace
        }).then(function (response) {
            return response.data;
        });
    };
});

//AppClasses.directive('validfile', function validFile() {
//    var validFormats = ['jpg', 'gif'];
//    return {
//        require: 'ngModel',
//        link: function (scope, elem, attrs, ctrl) {
//            ctrl.$validators.validFile = function () {
//                elem.on('change', function () {
//                    if (elem[0].files.length > 0) {
//                        const fsize = elem[0].files[0].size;
//                        const file = Math.round((fsize / 1024));
//                        // The size of the file. 
//                        if (file >= 4096) {
//                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
//                            elem[0].value = "";
//                        } else {
//                            if (scope.FileType == 1) {
//                                deb
//                                scope.ComplainTrailFilesList[scope.FileIndex].IsUploadFileName = true;
//                                scope.ComplainTrailFilesList[scope.FileIndex].FileName = "";
//                            } else if (scope.FileType == -1) {
//                                scope.IsUploadReportFileName = true;
//                                scope.ReportFileName = "";
//                            }
//                            scope.$apply();
//                        }
//                    }

//                });
//            };
//        }
//    };
//});




AppClasses.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 
                        if (file >= 4096) {
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {
                            if (scope.FileType == 1) {

                                scope.ComplainTrailFilesList[scope.FileIndex].IsUploadFileName = true;
                                scope.ComplainTrailFilesList[scope.FileIndex].FileName = "";
                            } else if (scope.FileType == -1) {
                                scope.IsUploadReportFileName = true;
                                scope.ReportFileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});

