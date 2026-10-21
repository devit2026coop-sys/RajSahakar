var EsignCommonData = (function () {
    // Common Error Handler
    function handleError(message) {
        alert(message || "Server error occurred.");
    }

    // Validate Esign
    function validateEsign($http, data, successCallback) {
        $http.get(data.SiteUrl + "eSign/ValidateEsign?DocType=" + data.DocType + "&docid=" + data.docid + "&SocietyIDEnc=" + data.SocietyIDEnc + "&DocumentPath=" + data.DocumentPath + "&DocumentName=" + data.DocumentName + "&ReturnPath=" + data.ReturnPath + "&Remark=" + data.Remark +
            "&StatusId=" + data.StatusId).then(function (response) {
                if (response.data.success) {
                    successCallback(response);
                } else {
                    handleError(response.data.message);
                }
            }).catch(function () {
                handleError("Server error occurred.");
            });
    }

    // Start Esign
    function startEsign($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "eSign/StartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            ReceiptNumber: data.ReceiptNumber,
            AmendmendAmount: data.AmendmendAmount,
            Location: data.Location,
            Desination: data.Desination,
            PurposeOfLoanOther: data.PurposeOfLoanOther,
            LoanAmountRecommendedByDCS: data.LoanAmountRecommendedByDCS,
            PaySummaryId: data.PaySummaryId
        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function startEsignAuditOrder($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "Society/AuditStartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.DocType == "Auditor Department" ? data.Remark : data.docid,
            DocIds: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            AmendmendAmount: data.AmendmendAmount,
            ReceiptNumber: data.ReceiptNumber,
            AuditBy: data.AuditBy,
            AssignedIds: data.AssignedIds,
            IsFirm: data.IsFirm,
            DistrictId: data.DistrictId,
            FinancialYearId: data.FinancialYearId,
            FinancialYearName: data.FinancialYearName,
            AuditTillDate: data.AuditTillDate,
            IsForced: data.IsForced,
            AuditByDepartmentorCA: data.AuditByDepartmentorCA,
            DeleteResions: data.DeleteResions,
            UserRoleDeptMappingId: data.UserRoleDeptMappingId,
            PurposeOfLoanOther: data.PurposeOfLoanOther,
            Location: data.Location,
            Desination: data.Desination,
        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function startEsignWareHouse($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "FertilizerSupply/StartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            Details: data.Details,
            Location: data.Location,
            Desination: data.Desination,

        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function startEsignOTS($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "PLDB/StartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            _model: data.details,
            Location: data.Location,
            Designation: data.Desination,

        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function startEsignCSS($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "Society/StartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            requestData: data.details,
            Location: data.Location,
            Desination: data.Desination,

        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function startEsignMPacs($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "Society/MPacStartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            Details: data.Details,
            Location: data.Location,
            Desination: data.Desination,

        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function startEsignGCC($http, data, redirectCallback) {
        $http.post(data.SiteUrl + "GopalCreditCard/GCCStartEsign", {
            DocType: data.DocType,
            SocietyIDEnc: data.SocietyIDEnc,
            ReturnPath: data.ReturnPath,
            docid: data.docid,
            DocumentPath: data.DocumentPath,
            DocumentName: data.DocumentName,
            ReturnPath: data.ReturnPath,
            Remark: data.Remark,
            StatusId: data.StatusId,
            IsAmendment: data.IsAmendment,
            Details: data.Details,
            Location: data.Location,
            Designation: data.Desination,
            _Model: data._Model,
            _AppFlowModel: data._AppFlowModel

        }).then(function (response) {
            if (response.data.success) {
                redirectCallback(response.data.xml);
            } else {
                handleError(response.data.message);
            }
        }).catch(function () {
            handleError("Server error occurred.");
        });
    }

    function redirectToESP(base64Xml) {
        var form = document.createElement("form");
        form.method = "POST";
        //form.action = "https://esignuat.rajasthan.gov.in:9006/esign/2.1/signdoc/";
        form.action = "https://esign.rajasthan.gov.in/esign/2.1/signdoc/";
        form.enctype = "multipart/form-data";

        var textarea = document.createElement("textarea");
        textarea.name = "esignData";
        textarea.style.display = "none";

        try {
            textarea.value = atob(base64Xml.replace(/\s/g, ''));
        } catch (e) {
            textarea.value = base64Xml;
        }

        form.appendChild(textarea);
        document.body.appendChild(form);

        form.submit();
    }

    // Public methods
    return { validateEsign: validateEsign, startEsign: startEsign, redirectToESP: redirectToESP, handleError: handleError, startEsignWareHouse: startEsignWareHouse, startEsignOTS: startEsignOTS, startEsignCSS: startEsignCSS, startEsignAuditOrder: startEsignAuditOrder, startEsignMPacs: startEsignMPacs, startEsignGCC: startEsignGCC };

})();