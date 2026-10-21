var ApproveAttendanceMod = angular.module("ApproveAttendanceMod", ['angularUtils.directives.dirPagination']);
ApproveAttendanceMod.controller("ApproveAttendanceCnt", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.AttendanceUsers = [];

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
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
        $scope.GetAllTrainningList();
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.GetAllTrainningList = function () {
        $http.post(SiteUrl + "/Trainning/ApproveAttendanceTrainningList", $scope.Trainning).
            success(function (data, status) {
                $scope.TrainningData = data.filter(x => x.ChannelId != '');
                console.log($scope.TrainningData)
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
            }).
            error(function (data1, status) {
                $scope.TrainningData = [];
            });
    }
    $scope.GenerateAttenderESign = function (trainingId) {
        $scope.TrainningId = trainingId
        $('#AttenderModal').modal('show');
    };
    $scope.GenerateInvoiceESign = function (trainingId) {
        $scope.TrainningId = trainingId
        $('#InvoiceModal').modal('show');
    };


    $scope.GenerateDocument = function (type) {
        $http.post(SiteUrl + "../Trainning/GenerateDocumentAttendance", {
            "TrainingId": $scope.TrainningId
        }).success(function (data, status) {
            if (data.Success)
                if (type == 1) {
                    { window.open(data.ViewPath, '_blank'); }
                } else if (type == 2) {
                    $scope.RedirectToESPportal(data);
                }
        }).error(function (data, status) {
            alert("Error while");
        });
    };
    $scope.RedirectToESPportal = function (data) {
        $scope.dataPayload = data;
        if (!$scope.dataPayload) {
            return;
        }
        $scope.esignData = {
            SiteUrl: SiteUrl,
            DocType: $scope.dataPayload.DocType,
            ReturnPath: $scope.dataPayload.ReturnPath,
            DocumentPath: $scope.dataPayload.DocPath,
            DocumentName: $scope.dataPayload.DocName,
            docid: $scope.TrainningId,
            SocietyIDEnc: "",
            Remark: "",
            StatusId: 0,
            IsAmendment: "",
            AmendmendAmount: "",
            ReceiptNumber: "",
            Location: $scope.dataPayload.Location,
            Desination: $scope.dataPayload.Designation,
            PurposeOfLoanOther: ""
        };
        if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
            alert("Not valid document");
            return;
        }
        EsignCommonData.validateEsign($http, $scope.esignData, function () {
            EsignCommonData.startEsign($http, $scope.esignData, function (xml) { EsignCommonData.redirectToESP(xml); });
        });


    };

    $scope.GenerateTaxInvoice = function (type) {
        $http.post(SiteUrl + "../Trainning/GenerateTaxInvoice", { "TrainingId": $scope.TrainningId }).success(function (data, status) {
            if (data.Success)
                if (type == 1) {
                    { window.open(data.ViewPath, '_blank'); }
                } else if (type == 2) {
                    $scope.RedirectToESPportal(data);
                }
        }).error(function (data, status) {
            alert("Error while Menu insert");
        });
    };
    $scope.ViewAttendance = function (trainingId) {
        $scope.TrainningId = trainingId;
        $('#TrainingAttendanceModal').modal('show');
        $scope.GetTrainingAttendanceUsers($scope.TrainningId);
    };


    $scope.GetTrainingAttendanceUsers = function (TrainingId) {
        $http.post(SiteUrl + "/Trainning/TrainningAttendanceUsersList", { "TrainingId": TrainingId }).
            success(function (data, status) {
                $scope.AttendanceUsers = data;
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
            }).
            error(function (data1, status) {
                $scope.AttendanceUsers = [];
            });
    };

    $scope.ViewAttenderESign = function (pdfPath) {
        if (pdfPath) {
            window.open(pdfPath, "_blank");
        }
        else {
            alert("PDF not found.");
        }
    };
    $scope.ViewInvoice = function (pdfPath) {
        if (pdfPath) {
            window.open(pdfPath, "_blank");
        }
        else {
            alert("PDF not found.");
        }
    };


    $scope.OnPageLoad();

});


