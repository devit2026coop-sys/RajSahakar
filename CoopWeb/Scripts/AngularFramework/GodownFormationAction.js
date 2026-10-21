
var GodownFormationActionMod = angular.module('GodownFormationActionModule', ['angularUtils.directives.dirPagination']);
GodownFormationActionMod.controller("GodownFormationActionCont", function ($scope, $http, $rootScope) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.SearchText = '';
    $scope.IsPartialPayment = false;
    $scope.selectedParams = {};
    $scope.ApplicationId = "0";

    $scope.OnPageLoad = function () {
        $scope.IsProposalSubmission = false;
        $scope.IsEsignbyPacsManager = false;
        $scope.IsEsignbyPresident = false;
        $scope.IsEsignbyBranchManager = false;
        $scope.IsApproveNforward = false;
        $scope.IsRevert = false;
        $scope._language = _language;
        $scope.DistrictId = 0;
        $scope.RegisteringDocumentPath = "";
        //esign 
        $scope.dataPayload = {};
        $scope.esignData = {};
        $scope.FsType = "";

        $(".se-pre-con").fadeIn("slow");
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
        $(".se-pre-con").fadeOut("slow");
        $scope.GETActions();
        $scope.GETGodownFormationList();
    };

    $scope.DeleteGodown = function (id) {
        if (confirm("Are you sure you want to delete this record?")) {
            $http.post(SiteUrl + "/Godown/DeleteGodownApplication", { id: id }).success(function () {
                alert("Record deleted successfully");
                $scope.GETGodownFormationList();
            });
        }
    };

    $scope.ViewDocuments = function (ApplicationId) {
        $http.post(SiteUrl + "/Godown/GetDocumentsByApplicationId", { ApplicationId: ApplicationId }).then(function (response) {
            $scope.UploadedDocuments = response.data;
            $("#DocumentModal").modal("show");
        });
    };
    $scope.ClearSearch = function () {
        $scope.SearchText = '';
        $scope.GETGodownFormationList();
    };

    $scope.GETGodownFormationList = function (SearchTextFilter) {
        $('.loading').show();
        $scope.GETGodownFormationListData = [];
        $scope.IsASAvailable = false;
        $scope.IsFSAvailable = false;
        $http.post(SiteUrl + "/Godown/GETGodownFormationList", { SearchText: SearchTextFilter })
            .success(function (data, status) {
                if (data != null && data.length > 0) {
                    $scope.GETGodownFormationListData = data;
                    $scope.IsASAvailable = $scope.GETGodownFormationListData.some(function (item) {
                        return item.ASGeneratePath != null && item.ASGeneratePath != "" && item.ASGenerate != 0;
                    });

                    $scope.IsFSAvailable = $scope.GETGodownFormationListData.some(function (item) {
                        return item.FSGeneratePath != null && item.FSGeneratePath != "" && item.FSGenerate != 0;;
                    });
                }

                $('.loading').hide();

            }).error(function (data, status) {
                $scope.GETGodownFormationListData = [];
                $scope.IsASAvailable = false;
                $scope.IsFSAvailable = false;
                $('.loading').hide();
                message = _language === 'en-us' ? 'Some error occurred !!' : 'कुछ त्रुटि हुई है !!';
            });
    };
    $scope.GETActions = function () {
        $('.loading').show();
        $scope.ActionsData = [];
        $http.post(SiteUrl + "/Godown/GETActionsList").success(function (data, status) {
            $('.loading').show();
            $scope.ActionsData = data;
            $scope.IsProposalSubmission = data.some(x => x.LovCode === "Proposal Submission") ? true : false;
            $scope.IsApproveNforward = data.some(x => x.LovCode === "Approve & Forward") ? true : false;
            $scope.IsApproveNforward = data.some(x => x.LovCode === "Approve & Forward Inspector Processing") ? true : false;
            $scope.IsRevert = data.some(x => x.LovCode === "Revert") ? true : false;
            $('.loading').hide();

        }).error(function (data, status) {
            $scope.ActionsData = [];
            message = _language === 'en-us' ? 'Some error occurred !!' : 'कुछ त्रुटि हुई है !!';
            bootbox.alert(message);
        });
    };
    $scope.Validation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($("input[name='ChkBoxSPP']:checked").length == 0) {
            message += $scope._language == 'en-us' ? '<br />Please select Application .' : '<br />कृपया एप्लिकेशन का चयन करें !';
        }
        if ($scope.Reasonforstatus == null || $scope.Reasonforstatus == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Reason .' : '<br />कृपया कारण दर्ज करें।';
        }

        //if ($scope.RegisteringDocumentPath == null || $scope.RegisteringDocumentPath == undefined ) { // Commented and updated for manage mandatory field value // By RK on dated 2025-11-18 //
        if ($scope.RegisteringDocumentPath == null || $scope.RegisteringDocumentPath == undefined || $scope.RegisteringDocumentPath == "") { // Add new condition || $scope.RegisteringDocumentPath == "" for manage blank path value due to mandatory field show by (*) in page// BY RK on dated 2025-11-18 //
            message += $scope._language == 'en-us' ? '<br />Please Upload Documents .' : '<br />कृपया दस्तावेज़ अपलोड करें';
        }

        // Added new condition for manage blank PartialPaymentAmount value if IsPartialPayment check true by user on page// BY RK on dated 2025-11-18 //
        if ($scope.IsPartialPayment == true || $scope.IsPartialPayment == 1) {
            if ($scope.PartialPaymentAmount == null || $scope.PartialPaymentAmount == undefined || $scope.PartialPaymentAmount == 0 || $scope.PartialPaymentAmount == "") {
                message += $scope._language == 'en-us' ? '<br />Please Enter Partial Payment Amount.' : '<br />कृपया आंशिक भुगतान राशि दर्ज करें।';
            }
        }

        if (message.length > 30) {

            ShowModelAlert($scope._language == 'en-us' ? ' Please Chose Following Fields' : 'कृपया निम्नलिखित फ़ील्ड चुनें', message);
            return false;
        }
        return true;

    }
    $scope.UpdateAction = function (applicationId, Status) {
        var selectedApplicationId = applicationId;
        if (selectedApplicationId) {
            $http.post(SiteUrl + "Godown/UpdateAction", { selectedApplications: selectedApplicationId, Status: Status })
                .success(function (data, status) {
                    alert(_language == 'en-us' ? 'Application status has been Updated.\n' : 'आवेदन की स्थिति अपडेट की गई है\n');
                    window.location.reload();
                }).error(function (data, status) {
                    alert(_language == 'en-us' ? 'Some Error is Occured.\n' : 'कुछ त्रुटि हुई है\n');
                });
        } else {
            alert("Please select an application.");
        }
    };

    $scope.ViewTrail = function (proposalId) {
        //$http.get(SiteUrl + "/Godown/GetProposalTrail?ProposalId=" + proposalId).success(function (result) {
        //$scope.TrailList = result;
        //$("#TrailModal").modal("show");
        //});
    };

    $scope.RegisteringSaveDocument = function (documentShortname, DocumentMasterId) {
        PleaseWaitStart();
        if (document.getElementById(documentShortname + "RegisterFileInput").files.length != 0) {
            var formData = new FormData();
            formData.append("ApplicationId", 1);
            formData.append("documentShortname", documentShortname);
            formData.append(DocumentMasterId + '_', document.getElementById(documentShortname + "RegisterFileInput").files[0]);
            $http.post(SiteUrl + "Report/SaveDocumentOnDisk", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status, message) {
                    if (data != "") {
                        $scope.RegisteringDocumentPath = data;

                        $("#showDispatchDoc" + documentShortname).show();
                        $("#PreviousDispatchDoc" + documentShortname).attr("href", data)
                        // ErrorAlert(_language == 'en-us' ? $("#lbl" + documentShortname).text().replace('Upload', '') + ' Successfully Uploaded' : $("#lbl" + documentShortname).text().replace('अपलोड करें', '') + ' - सफलतापूर्वक अपलोड किया गया');
                    }
                    else {
                        ErrorAlert(_language == 'en-us' ? $("#lbl" + documentShortname).text().replace('Upload', '') + ' Document not uploaded' : $("#lbl" + documentShortname).text().replace('अपलोड करें', '') + ' - दस्तावेज़ अपलोड नहीं किया गया');
                    }
                    PleaseWaitStop();
                }).
                error(function (data, status) {
                    ErrorAlert(_language == 'en-us' ? 'Some error occurred \n' : 'कोई त्रुटी हुई है \n');
                    PleaseWaitStop();
                });
        }
    }
    $scope.RegisteringFileforUploadDoc = function (file, documentname) {
        $(".se-pre-con").fadeIn("slow");
        try {
            _validFileExtensions = [".jpg", ".jpeg", ".png", ".pdf"];
            var fileinp = document.getElementById(documentname);
            var sFileName = fileinp.value;
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }

            if (!blnValid) {
                var message = _language == 'en-us' ? 'Sorry, File is invalid, allowed extensions are: ' + _validFileExtensions.join(", ") + ' \n' : 'क्षमा करें, फ़ाइल अमान्य है, अनुमत एक्सटेंशन हैं:' + _validFileExtensions.join(", ") + ' \n';
                alert(message);
                return false;
            }
            documentname = documentname.replace("RegisterFileInput", "");
            var MstDocId = $('#' + documentname + 'RD').val();
            $scope.SelectedFileForUpload = fileinp;
            if ($scope.SelectedFileForUpload.size < 4000000) {
                $scope.RegisteringSaveDocument(documentname, MstDocId);
            }
        }
        catch (ex) {
            console.log(ex)
        }
        finally {
            $(".se-pre-con").fadeOut("slow");
        }
    };

    $scope.GetEventDistrictList = function () {
        $scope.DistrictList = [];
        $http.post(SiteUrl + "Report/DistrictList").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($scope.DistrictList.length <= 1) {
                    $scope.DistrictId = $scope.DistrictList[0].DistrictId;
                }
                else {
                    $scope.DistrictList = data;
                }
            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            })
    };

    $scope.onlyAmount = function (event) {
        let char = String.fromCharCode(event.which);
        // allow digits and only one decimal point
        if (!/[\d.]/.test(char)) {
            event.preventDefault();
            return;
        }

        // block more than one decimal point
        if (char === '.' && $scope.PartialPaymentAmount && $scope.PartialPaymentAmount.includes('.')) {
            event.preventDefault();
        }
    };

    $scope.togglePaymentAmountInput = function () {
        if ($scope.IsPartialPayment) {
        } else {
            $scope.PartialPaymentAmount = undefined;
        }
    };


    //$scope.IsEsignbyPacsManager = false;
    //$scope.IsEsignbyPresident = false;
    //$scope.IsEsignbyBranchManager = false;
    //$scope.IsEsignbyJointRegistrar = false;

    $scope.CheckSelection = function (item) {
        if (item != null) {
            $scope.selectedParams = item;
            $scope.IsApproveNforward = true;
            $scope.btnText = $scope.selectedParams.NextStatus;
            $scope.NextStatusId = $scope.selectedParams.NextStatusId;
            $scope.FsType = $scope.selectedParams.MTType;
        }
    };


    // eSign process 2.1 on 20-07-2026
    $scope.RedirectToESPportal = function (status) {
        debugger;
        $scope.ApplicationId = $("input[name='SelectedApplication']:checked").val() ?? 0;
        if (status == 19) { $scope.GenerateASViewPath($scope.ApplicationId, status); }
        else if (status == 20) {
            $scope.ASGenerateEsign($scope.ApplicationId, status);
        }
        else if (status == 2 || status == 3 || status == 4) {
            $scope.GenerateOTPForEsign($scope.ApplicationId, status);
        }
        else if (status == 21) {
            debugger;
            $scope.GenerateFsViewPath($scope.ApplicationId, status, $scope.FsType)
        }
        else if (status == 22) {
            $scope.FSGenerateEsign($scope.ApplicationId, status, $scope.FsType)
        }
        else {
            $scope.UpdateAction($scope.ApplicationId, status);
        }
    }


    $scope.GenerateOTPForEsign = function (applicationId, status) {
        $scope.Remark = "";
        $scope.Desination = "";
        $scope.docType = "";
        $scope.docName = '';
        $scope.dataPayload = $scope.GETGodownFormationListData.filter(a => a.Applicationid == applicationId);
        if (status == 2) {
            $scope.Remark = "";
            $scope.Desination = "Pacs Manager";
            $scope.docType = "Godown Document";
        } else if (status == 3) {
            $scope.Remark = $scope.dataPayload[0].PresidentName;
            $scope.Desination = "President";
            $scope.docType = "Godown President";
        }
        else if (status == 4) {
            $scope.Remark = "";
            $scope.Desination = "Branch Manager";
            $scope.docType = "Godown BranchManager";
        }

        $scope.esignData = {
            SiteUrl: SiteUrl,
            DocType: $scope.docType,
            ReturnPath: "/godown/GodownFormationAction",
            DocumentPath: $scope.dataPayload[0].FormationPath,
            DocumentName: $scope.dataPayload[0].DocName,
            docid: applicationId,
            SocietyIDEnc: $scope.dataPayload[0].EncProposalId,
            Remark: $scope.Remark,
            StatusId: 0,
            IsAmendment: false,
            AmendmendAmount: "",
            ReceiptNumber: status,
            Location: $scope.dataPayload[0].District,
            Desination: $scope.Desination
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
    };

    $scope.ASGenerateEsign = function (applicationId, nextStatus) {
        $scope.GenerateAS(applicationId, nextStatus).then(function (response) {
            $scope.dataPayload = response.data;
            if (!$scope.dataPayload.Success) {
                alert("Document generation failed");
                return;
            }

            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                Location: $scope.dataPayload.Location,
                Desination: $scope.dataPayload.Designation,
                docid: applicationId,
                SocietyIDEnc: "",
                Remark: "",
                StatusId: 0,
                IsAmendment: false,
                AmendmendAmount: "",
                ReceiptNumber: nextStatus,
            };
            console.log($scope.esignData);
            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentPath) {
                alert("Not valid document");
                return;
            }

            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {

            console.error(err);
            alert("Error generating eSign certificate");

        });
    };

    $scope.GenerateAS = function (applicationId, nextStatus) {
        var selectedApplicationId = applicationId;
        return $http.get(SiteUrl + "/Godown/GenerateASPDF?ApplicationId=" + selectedApplicationId + "&nextStatusId=" + nextStatus).success(function (data, status) {
        }).error(function () {
            alert("Something went wrong.");
        });
    };

    $scope.GenerateASViewPath = function (applicationId, nextStatus) {
        debugger;
        return $http.get(SiteUrl + "/Godown/GenerateASPDF?ApplicationId=" + applicationId + "&nextStatusId=" + nextStatus).success(function (data, status) {
            if (data.Success) {
                //window.open(data.ViewPath, '_blank');
                //window.location.reload();             
                ShowOkMessage("", "AS generated successfully.", function () {
                    window.location.reload();
                });
            } else {
                ShowOkMessage("", "Document Path Not Found.", function () {
                    window.location.reload();
                });
            }
        }).error(function () {
            alert("Something went wrong.");
        });
    };

    $scope.RedirectToFormation = function () {
        window.location.href = '/Godown/AddGodownForm';
    };

    // Fs eSign process on 06-10-2026
    $scope.GenerateFsViewPath = function (applicationId, nextStatus, type) {
        debugger;
        return $http.get(SiteUrl + "/Godown/GenerateFSPdfByTemplate?ApplicationId=" + applicationId + "&nextStatusId=" + nextStatus + "&fsType=" + type).success(function (data, status) {
            if (data.Success) {
                //window.open(data.ViewPath, '_blank');
                //window.location.reload();              
                ShowOkMessage("", "FS generated successfully.", function () {
                    window.location.reload();
                });
            } else {
                ShowOkMessage("", "Document Path Not Found.", function () {
                    window.location.reload();
                });
            }
        }).error(function () {
            alert("Something went wrong.");
        });
    };
    $scope.FSGenerateEsign = function (applicationId, nextStatus, fsType) {
        debugger;
        $scope.GenerateFS(applicationId, nextStatus, fsType).then(function (response) {
            console.log(response)
            $scope.dataPayload = response.data;
            if (!$scope.dataPayload.Success) {
                ShowOkMessage("", "Document generation failed", function () {
                    window.location.reload();
                });
                return;
                // alert("Document generation failed");               
            }

            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                Location: $scope.dataPayload.Location,
                Desination: $scope.dataPayload.Designation,
                docid: applicationId,
                SocietyIDEnc: "",
                Remark: "",
                StatusId: 0,
                IsAmendment: false,
                AmendmendAmount: "",
                ReceiptNumber: nextStatus,
            };
            console.log($scope.esignData);
            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                ShowOkMessage("", "Not valid document", function () {
                    window.location.reload();
                });
                return;
                //alert("Not valid document");
                //return;
            }

            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {
            console.error(err);
            alert("Error generating eSign certificate");

        });
    };
    $scope.GenerateFS = function (applicationId, nextStatus, type) {
        debugger;
        return $http.get(SiteUrl + "/Godown/GenerateFSPdfByTemplate?ApplicationId=" + applicationId + "&nextStatusId=" + nextStatus + "&fsType=" + type).success(function (data, status) {
            console.log("Generate FS", data);
        }).error(function () {
            alert("Something went wrong.");
        });
    };

    $scope.OnPageLoad();
});


