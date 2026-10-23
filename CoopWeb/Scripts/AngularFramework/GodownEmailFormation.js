
var godownEmailFormationMod = angular.module('GodownEmailFormationModule', ['angularUtils.directives.dirPagination']);

godownEmailFormationMod.directive('select2', function () {
    return {
        restrict: 'A', require: 'ngModel', scope: { ngModel: '=', ngChange: '&?' },
        link: function (scope, element, attrs, ngModelCtrl) {
            $(element).select2();
            $(element).on('change', function () {
                var value = $(element).val();
                scope.$applyAsync(function () {
                    ngModelCtrl.$setViewValue(value);
                    if (attrs.ngChange) {
                        scope.ngChange();
                    }
                });
            });
            ngModelCtrl.$render = function () {
                $(element).val(ngModelCtrl.$viewValue).trigger("change.select2");
            };
        }
    };
});

godownEmailFormationMod.controller("GodownEmailFormationCtrl", function ($scope, $http, $rootScope) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.godownFormationDataList = [];
    $scope.EmailDataList = [];


    $scope.EmailData = {
        ApplicationId: "0",
        Subject: "",
        Body: "",
        selectedDocument: null,
        documentPreviewUrl: null,
        ToEmailList: [],
        CCList: [],
        BCCList: []
    };
    $scope.searchParams = {
        searchingData: '',
        mappingId: $("#hdnMappingId").val() ?? 0
    }

    $scope.OnPageLoad = function () {
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
        $scope.GetGodownEmailFormationList();
    };
    $scope.ClearSearch = function () {
        $scope.searchParams.searchingData = "";
        $scope.GetGodownEmailFormationList();
    };

    $scope.GetGodownEmailFormationList = function (search) {
        $('.loading').show();
        $scope.godownFormationDataList = [];
        $scope.IsASAvailable = false;
        $scope.IsFSAvailable = false;
        $http.post(SiteUrl + "/Godown/GetGodownFsEmailList", { SearchText: search })
            .success(function (data, status) {
                if (data != null && data.length > 0) {
                    $scope.godownFormationDataList = data;
                    $scope.IsASAvailable = $scope.godownFormationDataList.some(function (item) {
                        return item.ASGeneratePath != null && item.ASGeneratePath != "" && item.ASGenerate != 0 && item.ASEsignDate != null && item.ASEsignDate != "" && item.IsASEsign === true;
                    });

                    $scope.IsFSAvailable = $scope.godownFormationDataList.some(function (item) {
                        return item.FSGeneratePath != null && item.FSGeneratePath != "" && item.FSGenerate != 0 && item.FsEsignDate != null && item.FsEsignDate != "" && item.IsFsEsign === true;
                    });
                }

                $('.loading').hide();

            }).error(function (data, status) {
                $scope.godownFormationDataList = [];
                $scope.IsASAvailable = false;
                $scope.IsFSAvailable = false;
                $('.loading').hide();
                message = _language === 'en-us' ? 'Some error occurred !!' : 'कुछ त्रुटि हुई है !!';
            });
    };
    $scope.OpenSendEmailPopup = function (item) {
        if (!$scope.SelectedApplicationId) {
            ShowOkMessage("Warning", "Please select an application first.", function () { });
            return;
        }

        if ($scope.SelectedApplicationId != item.Applicationid) {
            ShowOkMessage("Warning", "Please select this application first.", function () { });
            return;
        }

        if ($scope.searchParams.mappingId != null && $scope.searchParams.mappingId != 0) {
            $scope.GetEmailListByUser();
        }

        $scope.EmailData = {
            ApplicationId: item.Applicationid,
            ToEmailList: [],
            CCList: [],
            BCCList: [],
            Subject: "",
            Body: "",
            //DocumentFormat: "PDF",
            //DocumentName: item.FSFileName || "Godown FS Document.pdf",
            //DocumentUrl: item.FSFilePath || ""
        };
        $('#sendEmailModal').modal('show');
    };

    $scope.SendEmail1 = function () {
        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!$scope.EmailData.Subject || !$scope.EmailData.Subject.trim()) {
            ShowOkMessage("Warning", "Please enter subject.");
            return;
        }

        //if (!emailPattern.test($scope.EmailData.ToEmail.trim())) {
        //    ShowOkMessage("Warning", "Please enter a valid email address.");
        //    return;
        //}

        //if (!$scope.EmailData.selectedDocument) {
        //    ShowOkMessage("Warning", "Please select a document."); return;
        //}

        var formData = new FormData();
        formData.append("ApplicationId", $scope.EmailData.ApplicationId ?? 0);
        formData.append("ToEmail", $scope.EmailData.ToEmail ?? "");
        formData.append("CC", $scope.EmailData.CC ?? "");
        formData.append("BCC", $scope.EmailData.BCC ?? "");
        formData.append("Subject", $scope.EmailData.Subject ?? "");
        formData.append("Body", $scope.EmailData.Body ?? "");
        if ($scope.EmailData.selectedDocument) {
            formData.append("selectedDocument", $scope.EmailData.selectedDocument, $scope.EmailData.selectedDocument.name);
        }

        $('.loading').show();
        $http.post(SiteUrl + "/Godown/SendFsEmailFromGodown", formData, {
            transformRequest: angular.identity, headers: { "Content-Type": undefined }
        }).then(function (response) {
            let data = response.data;
            if (data && data.Success) {
                ShowOkMessage("Success", data.Message || "Email sent successfully.");
                $('#sendEmailModal').modal('hide');
            } else {
                ShowOkMessage("error", (data && data.Message) || "Unable to send email.");
            }

        }, function (error) {
            var errorData = error && error.data;
            ShowOkMessage("error", (errorData && errorData.Message) || "Error occurred while sending email.");
        }).finally(function () {
            $('.loading').hide();
        });
    };


    $scope.SendEmail = function () {
        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        // Convert selected email lists into comma-separated strings
        $scope.EmailData.ToEmail = ($scope.EmailData.ToEmailList || []).join(',');
        $scope.EmailData.CC = ($scope.EmailData.CCList || []).join(',');
        $scope.EmailData.BCC = ($scope.EmailData.BCCList || []).join(',');

        // Validate To email list
        if (!$scope.EmailData.ToEmail || !$scope.EmailData.ToEmail.trim()) {
            ShowOkMessage("Warning", "Please select recipient email.");
            return;
        }

        let toEmails = $scope.EmailData.ToEmail.split(',').map(email => email.trim()).filter(email => email !== "");

        if (toEmails.some(email => !emailPattern.test(email))) {
            ShowOkMessage("Warning", "Please select valid recipient email addresses.");
            return;
        }

        // Validate CC email list
        let ccEmails = $scope.EmailData.CC.split(',').map(email => email.trim()).filter(email => email !== "");

        if (ccEmails.some(email => !emailPattern.test(email))) {
            ShowOkMessage("Warning", "Please select valid CC email addresses.");
            return;
        }

        // Validate BCC email list
        let bccEmails = $scope.EmailData.BCC.split(',').map(email => email.trim()).filter(email => email !== "");
        if (bccEmails.some(email => !emailPattern.test(email))) {
            ShowOkMessage("Warning", "Please select valid BCC email addresses.");
            return;
        }

        if (!$scope.EmailData.selectedDocument) {
            ShowOkMessage("Warning", "Please select a document.");
            return;
        }

        var formData = new FormData();
        formData.append("ApplicationId", $scope.EmailData.ApplicationId ?? 0);
        formData.append("ToEmail", $scope.EmailData.ToEmail ?? "");
        formData.append("CC", $scope.EmailData.CC ?? "");
        formData.append("BCC", $scope.EmailData.BCC ?? "");
        formData.append("Subject", $scope.EmailData.Subject ?? "");
        formData.append("Body", $scope.EmailData.Body ?? "");

        if ($scope.EmailData.selectedDocument) {
            formData.append("selectedDocument", $scope.EmailData.selectedDocument, $scope.EmailData.selectedDocument.name);
        }

        $('.loading').show();
        $http.post(SiteUrl + "/Godown/SendFsEmailFromGodown", formData, {
            transformRequest: angular.identity,
            headers: { "Content-Type": undefined }
        }).then(function (response) {
            let data = response.data;
            if (data && data.Success) {
                ShowOkMessage("Success", data.Message || "Email sent successfully.");
                $('#sendEmailModal').modal('hide');
            } else {
                ShowOkMessage("error", (data && data.Message) || "Unable to send email.");
            }

        }, function (error) {
            var errorData = error && error.data;
            ShowOkMessage(
                "error",
                (errorData && errorData.Message) || "Error occurred while sending email."
            );
        }).finally(function () {
            $('.loading').hide();
        });
    };



    $scope.onDocumentSelected = function (input) {
        var file = input.files && input.files[0];
        if (!file) {
            return;
        }

        // Allow PDF files only
        if (file.type !== "application/pdf" &&
            !file.name.toLowerCase().endsWith(".pdf")) {
            alert("Please select a PDF file.");

            input.value = "";
            $scope.$applyAsync(function () {
                $scope.EmailData.selectedDocument = null;
            });
            return;
        }

        // Revoke previous preview URL
        if ($scope.EmailData.documentPreviewUrl) {
            URL.revokeObjectURL($scope.EmailData.documentPreviewUrl);
        }

        $scope.$applyAsync(function () {
            $scope.EmailData.selectedDocument = file;
            $scope.EmailData.documentPreviewUrl = URL.createObjectURL(file);
        });
    };

    $scope.CheckSelection = function (item) {
        if (item != null) {
            $scope.selectedParams = item;
        }
    };
    $scope.viewDocument = function () {
        if ($scope.EmailData.documentPreviewUrl) {
            window.open($scope.EmailData.documentPreviewUrl, "_blank");
        }
    };
    $scope.ClearModal = function () {
        $scope.EmailData.selectedDocument = null;
        $scope.SelectedApplicationId = null;
        $('#sendEmailModal').modal('hide');
    };

    $scope.removeDocument = function () {
        if ($scope.EmailData.documentPreviewUrl) {
            URL.revokeObjectURL($scope.EmailData.documentPreviewUrl);
            $scope.EmailData.documentPreviewUrl = null;
        }

        $scope.EmailData.selectedDocument = null;
        var fileInput = document.getElementById("attachedDocument");
        if (fileInput) {
            fileInput.value = "";
        }
    };

    $scope.GetEmailListByUser = function () {
        $('.loading').show();
        $scope.EmailDataList = [];
        $http.post(SiteUrl + "/Godown/GetEmailListByUser").success(function (data, status) {
            if (data != null && data.Result.Data != null) {
                $scope.EmailDataList = JSON.parse(data.Result.Data.emailData);
            }
            else {
                $scope.EmailDataList = []
                $('.loading').hide();
            }
            $('.loading').hide();
        }).error(function (data, status) {
            $scope.EmailDataList = [];
            $('.loading').hide();
            message = _language === 'en-us' ? 'Some error occurred !!' : 'कुछ त्रुटि हुई है !!';
        });
    };

    $scope.OnPageLoad();
});




