
var MasterMod = angular.module('ESignModule', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ESignCont", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.isEsignProcessing = false;
    $scope.dataPayload = {};
    $scope.payload = {
        SocietyIDEnc: '0x02000000120A4F6F431982EF757C000845D4430DB6939C965B78AAB8A490F0E17DF030F865231A7A67C175C506EA4FFBE70483DB',
        StatusId: 17,
        Remark: "Test For Esign",
        IsAmendment: false
    }

    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get("/Translations/Translation_HI.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
    };
    $scope.RedirectToESPportalWorking = function () {
        $scope.GenerateCertificate().then(function (response) {
            $scope.dataPayload = response.data;
            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                docid: $scope.payload.SocietyIDEnc,
                SocietyIDEnc: $scope.payload.SocietyIDEnc,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                Remark: $scope.dataPayload.Remark,
                IsAmendment: false,
                StatusId: 14,
                AmendmendAmount: "",
                ReceiptNumber: ""
            };
            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });
        });

    };
    $scope.GenerateCertificate = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/ApplicationFlow/VerifyOTPForEsignProcessTest?SocietyIDEnc=" + $scope.payload.SocietyIDEnc + "&StatusId=" + $scope.payload.StatusId + "&Remark=" + $scope.payload.Remark + "&IsAmendment=" + $scope.payload.IsAmendment).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.RedirectToESPportal = function () {
        //$scope.BankOpeningCertificate();
        //return;
        $scope.esignData = {
            SiteUrl: SiteUrl,
            DocType: "Test Esign",
            ReturnPath: "eSign/EsignSuccess",
            docid: "1",
            SocietyIDEnc: $scope.payload.SocietyIDEnc,
            DocumentPath: "~/Content/AuditReport/0E4A68696F04410EB4EB4E98B9B4BE57.pdf",
            //DocumentPath: "https://rajsahakarapp.rajasthan.gov.in/Content/AuditReport/D7B86C384BAE4A17828EB958C297881B.pdf",
            //DocumentPath: "D:/Narpat/CoopWeb/Content/AuditReport/D7B86C384BAE4A17828EB958C297881B.pdf",
            DocumentName: "D7B86C384BAE4A17828EB958C297881B.pdf",
            Remark: "Test For Esign",
            StatusId: "0",
            IsAmendment: false,
            AmendmendAmount: "",
            ReceiptNumber: ""
        };

        if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
            alert("Not valid document");
            return;
        }
        console.log($scope.esignData);
        EsignCommonData.validateEsign($http, $scope.esignData, function () {
            EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                EsignCommonData.redirectToESP(xml);
            });
        });
    };

    //$scope.BankOpeningCertificate = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    return $http.post(SiteUrl + "/ApplicationFlow/GenerateBankAccountLatter?SocietyIDEnc=" + $scope.payload.SocietyIDEnc).success(function (data, status) {
    //        console.log(data);
    //        $(".se-pre-con").fadeOut("slow");
    //    }).error(function (data, status) {
    //        $(".se-pre-con").fadeOut("slow");
    //    });
    //};


    $scope.IssueCertificateLatter = function (type) {
        $(".se-pre-con").fadeIn("slow");
        $scope.IsType = type;
        return $http.post(SiteUrl + "/ApplicationFlow/GenerateAmendmentCertificateIssue", { "SocietyIDEnc": $scope.payload.SocietyIDEnc, "StatusId": $scope.payload.StatusId, "Remark": $scope.payload.Remark, "IsAmendment": $scope.payload.IsAmendment, "type": $scope.IsType, "AmendmentId": 67 }).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            console.log(data);
            return data;
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    };



    $scope.OnPageLoad();
});