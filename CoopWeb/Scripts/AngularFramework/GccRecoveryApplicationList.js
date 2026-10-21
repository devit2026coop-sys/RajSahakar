
var MasterMod = angular.module('GccApplicationListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("GccApplicationListCnt", function ($rootScope, $scope, $location, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divClass = false;
    $scope.GccApplicationList = [];
    $scope.TotalGccApplicationList = [];
    $scope.payObj = {};
    $scope.payObj.PaymentDate = $scope.today;
    const urlParams = new URLSearchParams(window.location.search);
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }
    $scope.search = {};
    $scope.search.LoanBankID = '0';
    $scope.ApplicationFlowMaster = {}
    $scope.GccApplicationFlowModel = {}
    $scope.showGccReportFile = false;
    $scope.ShowOTPDiv = false;
    $scope.ImageValidationMessage = '';
    $scope.FileCollection = [];

    // Initialise Main form control
    $scope.GopalCreditCardModel = {}
    $scope.GopalCreditCardModel.LoanDistrictId = '0';
    $scope.GopalCreditCardModel.LoanBankID = "0";
    $scope.GopalCreditCardModel.LoanBranchId = "0";
    $scope.GopalCreditCardModel.PACS_ID = "0";
    $scope.payObj = {};
    $scope.SanctionDetails = {};

    $scope.IsResendOTP = false;   // default: first time

    //$scope.DataPaging.PageSize = $scope.obj.pageSize ?? 50;

    //$scope.selectedStatus = "PA";

    //Esign 2.1
    $scope.isEsignProcessing = false;
    $scope.dataPayload = {};
    $scope.esignData = {};

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.obj.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetGccApplicationList(false);
    }


    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,date,input:checkbox').length;
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
                $scope.GetGccApplicationList(false);
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
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback($scope.SocietyCountId_ss, $scope.DistrictId_ss);
            }
            ++indx;
        });

    }

    $scope.levelId = 0;

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
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
        $scope.GSSProceesStatusList();
        $scope.levelId = $('#hdnLevelId').val();

        if ($scope.levelId == 1) {
            $scope.LoanBankList();
            $scope.GetGccApplicationList(false);
        }
        else if ($scope.levelId == 7 || $scope.levelId == 8 || $scope.levelId == 9) {
            if ($scope.levelId == 7) {
                $scope.search.LoanBankID = $('#hdnGccId').val();
                $scope.Get_LoanBankBranch();
            }
            else if ($scope.levelId == 8) {
                $scope.search.LoanBranchId = $('#hdnGccId').val();

                $scope.Get_LoanBranch_PACS();
            }
            else if ($scope.levelId == 9) {
                $scope.search.PACS_ID = $('#hdnGccId').val();
            }

            $scope.currentUserId = $('#hdnUserId').val();


            $scope.GetGccApplicationList(false);
            //$scope.searchRequest($scope.GetGccApplicationList(false));
        }
        else {

        }
        // ✅ Default radio selection
        $scope.selectedStatus = "PA";

    };
    $scope.ApplicationView = function (eId, eRno) {
        if (eId != '' && eRno != '') {
            var url = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId;
            window.open(url, '_blank');
        }
    }
    $scope.ApplicationActionView = function (eId, eRno) {
        if (eId != '' && eRno != '') {
            var url = "../GopalCreditCard/GopalApplicationFormActionView?eId=" + eId;
            window.open(url, '_blank');
        }
    }


    $scope.ApplicationActionEdit = function (eId, eRno) {
        if (eId != '' && eRno != '') {
            var url = "../GopalCreditCard/GopalApplicationFormActionView?eId=" + eId;
            window.open(url, '_blank');
        }
    }


    $scope.GetGccApplicationList = function (isExport) {



        $(".se-pre-con").fadeIn("slow");

        var key = "";
        var keyValue = "";
        if ($scope.levelId == 7 || $scope.levelId == 8 || $scope.levelId == 9) {

            var key = "";
            var keyValue = "";

            if ($scope.levelId == 7) {
                key = "CcbId";
                $scope.search.LoanBankID = $('#hdnGccId').val();
            }
            else if ($scope.levelId == 8) {
                key = "CcbBranchId";
                $scope.search.LoanBankID = '0';
                $scope.search.LoanBranchId = $('#hdnGccId').val();
            }
            else if ($scope.levelId == 9) {
                key = "PacsId";
                $scope.search.LoanBankID = '0';
                $scope.search.LoanBranchId = '0';
                $scope.search.PACS_ID = $('#hdnGccId').val();
            }

            var index = $scope.DataPaging.searchParameters.findIndex(function (p) { return p.Key === key; });

            if (index !== -1) {
                // If found, replace the value
                $scope.DataPaging.searchParameters[index].Value = $('#hdnGccId').val();
            }
            else {
                var parameter = { Key: key, Value: $('#hdnGccId').val() }
                $scope.DataPaging.searchParameters.push(parameter);
            }

            //$scope.GetGccApplicationList(false);
            //$scope.searchRequest($scope.GetGccApplicationList(false));
        }

        $scope.selectedStatus = $scope.selectedStatus || "PA";
        var statusIndex = $scope.DataPaging.searchParameters.findIndex(function (p) {
            return p.Key === "ApplicationStatus";
        });

        if (statusIndex !== -1) {
            $scope.DataPaging.searchParameters[statusIndex].Value = $scope.selectedStatus;
        } else {
            var parameter = { Key: "ApplicationStatus", Value: $scope.selectedStatus };
            $scope.DataPaging.searchParameters.push(parameter);
        }

        if (!isExport)
            $rootScope.DataPagingDetails = {};

        var Exportjson = "";
        if (isExport) {
            Exportjson = {
                "RemoveColumns": ["TotalRecords", "encGccid", "DisbursedDate", "DueDate"],
                "RenameColumns": {
                    "CCBName": "CCB Name"

                }
            }
        }

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/MIS/GccLoanReports", JSON.stringify({ Details: jsonParameters, Exportjson: Exportjson, Action: "RecoveryList" })).
            success(function (responcedata, status) {
                if (isExport) {
                    $scope.downloadFile(responcedata.filename);
                }
                else {
                    if (responcedata.DataList != null && responcedata.DataList.length > 0) {

                        var data = JSON.parse(responcedata.DataList);
                        $scope.GccApplicationList = data; // JSON.parse(data);

                        $rootScope.DataPagingDetails = responcedata.DataPaging;
                        $rootScope.SetDataPaging();


                        //if ($scope.DataPaging.currentPageID == 1) {
                        //    $scope.TotalRecords = data[0].TotalRecords;
                        //}
                        //$rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                        //$rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                        //$rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                        //$rootScope.SetDataPaging();
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
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (responcedata, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GccApplicationList = [];
                $scope.TotalGccApplicationList = []
            });
    };
    $scope.exportExcel = function () {
        $scope.GetGccApplicationList(true);
    }
    $scope.downloadFile = function (fileUrl) {
        const fileName = fileUrl.split('/').pop();
        $http({
            url: fileUrl,
            method: 'GET',
            responseType: 'blob'
        }).then(function (response) {
            var blob = new Blob([response.data], { type: response.headers('Content-Type') });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }).catch(function (error) {
            console.error('Error downloading file:', error);
        });
    };

    $scope.LoanBankList = function () {
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=0").
            success(function (data, status) {
                $scope.BankMasterList = data;
                $scope.search.LoanBranchId = '0';
                $scope.search.PACS_ID = '0';

            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };
    //document.getElementById('BankID').setAttribute('disabled', 'disabled');
    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?" + "LoanBankId=" + $scope.search.LoanBankID).
            success(function (data, status) {
                $scope.BankBranches = data;
                $scope.search.LoanBranchId = '0';
                $scope.search.PACS_ID = '0';
                //  $scope.GetGccSummary();
                // $scope.GetGccApplicationList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.search.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;
                $scope.search.PACS_ID = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.NewPayment = function (item) {
        $scope.payObj = {};
        $scope.payObj.GccIdEnc = item.encGccid;
        $scope.payObj.PrincipalAmount = item.sanctionAmt;
        $scope.payObj.ApplicationNo = item.ApplicationNo;
        $scope.payObj.ApplicantName = item.ApplicantName;
        $scope.payObj.PaymentType = (item.IsOD == 1) ? 'OD' : 'NOD';
        $scope.payObj.penalinterest = 0;
        $scope.payObj.DueDate = item.DueDate;
        $scope.payObj.DisbursedDate = item.DisbursedDate;
        $('#repaymentmodal').modal('show');

    }

    $scope.NewSanction = function (GopalCreditCardId) {
        $http.get("../GopalCreditCard/GetGopalCreditById", { params: { GopalCreditCardId: GopalCreditCardId } })
            .then(function (response) {
                if (response.data) {
                    $scope.SanctionDetails = response.data[0];
                    $scope.payObj = {};
                    $scope.payObj.GccId = $scope.SanctionDetails.GopalCreditCardId;
                    $scope.payObj.GccIdEnc = $scope.SanctionDetails.encGccid;
                    $scope.payObj.PrincipalAmount = $scope.SanctionDetails.sanctionAmt || 0;
                    $scope.payObj.ApplicationNo = $scope.SanctionDetails.ApplicationNo;
                    $scope.payObj.ApplicantName = $scope.SanctionDetails.ApplicantName;
                    $scope.payObj.PaymentType = ($scope.SanctionDetails.IsOD == 1) ? 'OD' : 'NOD';
                    $scope.payObj.PenalInterestAmount = $scope.SanctionDetails.PenalInterestAmount || 0;
                    $scope.payObj.DueDate = $scope.SanctionDetails.DueDate;
                    $scope.payObj.DisbursedDate = $scope.SanctionDetails.DisbursedDate;
                    $scope.payObj.PaymentDate = $scope.SanctionDetails.PaymentDate;
                    $scope.payObj.NOD_Days = $scope.SanctionDetails.NOD_Days;
                    $scope.payObj.OD_Days = $scope.SanctionDetails.OD_Days;
                    $scope.payObj.NOD_InterestRate = $scope.SanctionDetails.NOD_InterestRate;
                    $scope.payObj.OD_InterestRate = $scope.SanctionDetails.OD_InterestRate;
                    $scope.payObj.PaymentDate = new Date();
                    $scope.onPaymentDateChange();
                    $scope.CalaculaeInterest();
                    $('#repaymentmodal').modal('show');
                } else {
                    alert("No record found!");
                }
            })
            .catch(function (error) {
                console.error("Error fetching sanction details:", error);
            });
    };



    $scope.EditPayment = function (item) {
        debugger;
        $scope.payObj = {};
        $scope.payObj.GccIdEnc = item.encGccid;
        $scope.payObj.PrincipalAmount = item.sanctionAmt;
        $scope.payObj.ApplicationNo = item.ApplicationNo;
        $scope.payObj.ApplicantName = item.ApplicantName;
        $scope.payObj.PaymentType = (item.IsOD == 1) ? 'OD' : 'NOD';
        $scope.payObj.RecoverableAmount = item.TotalRecoveryAmt;
        $scope.payObj.InterestAmount = item.InterestAmount;
        $scope.payObj.Remark = item.Remark;
        $scope.payObj.PaymentSummaryId = item.PaymentSummaryId;
        $scope.payObj.GccId = item.GopalCreditCardId;
        $scope.payObj.penalinterest = 0;
        $scope.payObj.IsSatisfiedWithInttCalcByPacs = item.IsSatisfiedWithInttCalcByPacs;
        $scope.payObj.IsRenewable = item.IsRenewable;
        $scope.payObj.DueDate = item.DueDate;
        $scope.payObj.DisbursedDate = item.DisbursedDate;
        $scope.payObj.PaymentDate = item.PaymentDate ? new Date(item.PaymentDate) : new Date();
        $scope.payObj.NOD_Days = item.NOD_Days;
        $scope.payObj.OD_Days = item.OD_Days;
        $scope.payObj.NOD_InterestRate = item.NOD_InterestRate;
        $scope.payObj.OD_InterestRate = item.OD_InterestRate;

        $scope.onPaymentDateChange();
        $scope.CalaculaeInterest();

        $('#repaymentmodal').modal('show');
    };


    //$scope.EditPayment = function (GopalCreditCardId) {
    //    $http.get("../GopalCreditCard/GetGopalCreditById", { params: { GopalCreditCardId: GopalCreditCardId } })
    //        .then(function (response) {
    //            if (response.data) {

    //                $scope.SanctionDetails = response.data[0];
    //                $scope.payObj = {};

    //                $scope.payObj.GccId = $scope.SanctionDetails.GopalCreditCardId;
    //                $scope.payObj.GccIdEnc = $scope.SanctionDetails.encGccid;
    //                $scope.payObj.PrincipalAmount = $scope.SanctionDetails.sanctionAmt || 0;
    //                $scope.payObj.ApplicationNo = $scope.SanctionDetails.ApplicationNo;
    //                $scope.payObj.ApplicantName = $scope.SanctionDetails.ApplicantName;
    //                $scope.payObj.PaymentType = ($scope.SanctionDetails.IsOD == 1) ? 'OD' : 'NOD';
    //                $scope.payObj.PenalInterestAmount = $scope.SanctionDetails.PenalInterestAmount || 0;
    //                $scope.payObj.DueDate = $scope.SanctionDetails.DueDate;
    //                $scope.payObj.DisbursedDate = $scope.SanctionDetails.DisbursedDate;
    //                $scope.payObj.PaymentDate = $scope.SanctionDetails.PaymentDate ? new Date($scope.SanctionDetails.PaymentDate) : new Date();
    //                $scope.payObj.NOD_Days = $scope.SanctionDetails.NOD_Days;
    //                $scope.payObj.OD_Days = $scope.SanctionDetails.OD_Days;
    //                $scope.payObj.NOD_InterestRate = $scope.SanctionDetails.NOD_InterestRate;
    //                $scope.payObj.OD_InterestRate = $scope.SanctionDetails.OD_InterestRate;
    //                $scope.onPaymentDateChange();
    //                $scope.CalaculaeInterest();

    //                $('#repaymentmodal').modal('show');
    //            } else {
    //                alert("No record found!");
    //            }
    //        })
    //        .catch(function (error) {
    //            console.error("Error fetching sanction details:", error);
    //        });
    //};


    function convertToJSDate(dateInput) {
        if (dateInput instanceof Date) return dateInput;

        if (typeof dateInput === "string" && dateInput.includes("-")) {
            let parts = dateInput.split("-");
            return new Date(parts[0], parts[1] - 1, parts[2]);
        }

        if (typeof dateInput === "string" && dateInput.includes("/")) {
            let parts = dateInput.split("/");
            return new Date(parts[2], parts[1] - 1, parts[0]);
        }

        return new Date(dateInput);
    }
    function diffDays(d1, d2) {
        const oneDay = 1000 * 60 * 60 * 24;
        const date1 = new Date(d1.getFullYear(), d1.getMonth(), d1.getDate());
        const date2 = new Date(d2.getFullYear(), d2.getMonth(), d2.getDate());

        return Math.abs((date2 - date1) / oneDay);
    }

    $scope.onPaymentDateChange = function () {
        //
        var disbursedDate = new Date($scope.payObj.DisbursedDate);
        //var duedate = new Date($scope.payObj.DueDate);

        ////
        //const input = $scope.payObj.DueDate;
        //const [day, month, year] = input.split("/");

        //const date = new Date(year, month - 1, day, 12, 0, 0);

        //const formatted =
        //    `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, "0")}-${String(date.getDate()).padStart(2, "0")}T${String(date.getHours()).padStart(2, "0")}:${String(date.getMinutes()).padStart(2, "0")}:${String(date.getSeconds()).padStart(2, "0")}.00`;

        ////

        //// Subtract 1 day
        //var endDate = endDate.setDate(formatted.getDate() - 1);

        //// Difference in days
        //var diffTime = endDate - disbursedDate;
        //var Daysdiff = Math.floor(diffTime / (1000 * 60 * 60 * 24));

        const [day, month, year] = $scope.payObj.DueDate.split("/");
        const dueDate = new Date(year, month - 1, day, 23, 59, 59);

        // Clone due date and subtract 1 day
        const endDate = new Date(dueDate);
        endDate.setDate(endDate.getDate());

        // Difference in days
        const diffTime = endDate.getTime() - disbursedDate.getTime();
        const Daysdiff = Math.floor(diffTime / (1000 * 60 * 60 * 24));

        //



        if (!$scope.payObj.PaymentDate || !$scope.payObj.DueDate || !$scope.payObj.DisbursedDate)
            return;

        let pay = convertToJSDate($scope.payObj.PaymentDate);
        let due = convertToJSDate($scope.payObj.DueDate);
        let loan = convertToJSDate($scope.payObj.DisbursedDate);

        if (pay.getDate() === due.getDate() && pay.getMonth() === due.getMonth() && pay.getFullYear() === due.getFullYear()) {
            $scope.payObj.PaymentType = "NOD";
            $scope.payObj.NOD_Days = Daysdiff;
            $scope.payObj.OD_Days = 0;
            $scope.CalaculaeInterest();
            $scope.payObj.PenalInterestAmount = 0;
            return;
        }

        if (pay < due) {
            $scope.payObj.PaymentType = "NOD";
            let dayDifference = diffDays(pay, due);
            let nodDays = Daysdiff - dayDifference;
            if (nodDays < 0) nodDays = 0;
            if (nodDays > Daysdiff) nodDays = Daysdiff;
            $scope.payObj.NOD_Days = nodDays;
            $scope.payObj.OD_Days = 0;
            //changes
            $scope.payObj.PenalInterestAmount = 0;
        }
        else {

            $scope.payObj.PaymentType = "OD";
            let odDays = diffDays(due, pay);
            let nodDays = Daysdiff;
            $scope.payObj.NOD_Days = nodDays;
            $scope.payObj.OD_Days = odDays;
        }

        $scope.CalaculaeInterest();
    };


    $scope.CalaculaeInterest = function () {
        if ($scope.payObj.PaymentType == 'OD') {
            var intNOD = ($scope.payObj.PrincipalAmount * $scope.payObj.NOD_Days * $scope.payObj.NOD_InterestRate) / 36500;
            var intOD = ($scope.payObj.PrincipalAmount * $scope.payObj.OD_Days * $scope.payObj.OD_InterestRate) / 36500;

            $scope.payObj.TotalAmount = Math.round(intNOD + intOD + $scope.payObj.PrincipalAmount);
            $scope.payObj.InterestAmount = Math.round(intNOD);
            $scope.payObj.PenalInterestAmount = Math.round(intOD);
            $scope.payObj.RecoverableAmount = Math.round(intNOD + intOD + $scope.payObj.PrincipalAmount);

        }
        else if ($scope.payObj.PaymentType == 'NOD') {
            var intNOD = ($scope.payObj.PrincipalAmount * $scope.payObj.NOD_Days * $scope.payObj.NOD_InterestRate) / 36500;

            $scope.payObj.TotalAmount = Math.round(intNOD + $scope.payObj.PrincipalAmount);
            $scope.payObj.InterestAmount = Math.round(intNOD);
            $scope.payObj.RecoverableAmount = Math.round($scope.payObj.PrincipalAmount);
            // ✅ IMPORTANT FIX
            $scope.payObj.PenalInterestAmount = 0;

        }
    }

    $scope.Action = function (item) {
        $scope.payObj = {};
        $scope.payObj.GccIdEnc = item.encGccid;
        $scope.payObj.PrincipalAmount = item.sanctionAmt;
        $scope.payObj.ApplicationNo = item.ApplicationNo;
        $scope.payObj.ApplicantName = item.ApplicantName;
        //$scope.payObj.PaymentType = (item.IsOD == 1) ? 'OD' : 'NOD';
        $scope.payObj.PaymentType = (item.PaymentType == 1) ? 'OD' : 'NOD';
        $scope.payObj.penalinterest = 0;
        $scope.payObj.GccId = item.GopalCreditCardId;
        $scope.payObj.RegistrationNo = item.RegistrationNo;
        $scope.payObj.encGccid = item.encGccid;
        $scope.payObj.PaymentSummaryId = item.PaymentSummaryId;
        $scope.payObj.PaymentSummaryId = item.PaymentSummaryId;
        $scope.payObj.DisbursedDate = item.DisbursedDate;
        $scope.payObj.PaymentDate = item.PaymentDate;
        $scope.payObj.sanctionAmt = item.TotalRecoveryAmt;
        $scope.payObj.RecieptDocument = item.RecieptDocument;
        $scope.payObj.PACSName_ENG = item.PACSName;
        $scope.payObj.Address_ENG = item.Address_ENG;
        $scope.IsRenewDisabled = false;


        $scope.ApplicationFlowMaster.SelectedOption = 'renew';
        $scope.ApplicationFlowMaster.IsEligibleForRenewable = 1;
        $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 0;

        if (item.statusid == 32) {

            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = item.IsSatisfiedWithInttCalcByPacs == true ? 1 : 0;
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = item.IsEligibleForRenewable == true ? 1 : 0;

            if ($scope.ApplicationFlowMaster.IsEligibleForRenewable == 1) {
                $scope.ApplicationFlowMaster.SelectedOption = 'renew';
            }
            else if ($scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs == 1) {
                $scope.ApplicationFlowMaster.SelectedOption = 'closer';
            }
            else {
                $scope.ApplicationFlowMaster.SelectedOption = null;
            }
            $scope.IsRenewDisabled = true;
        }

        $scope.payObj.PenalInterestAmount = item.PenalInterestAmount;
        $scope.payObj.InterestAmount = item.InterestAmount;

        $scope.payObj.RecoverableAmount = item.TotalRecoveryAmt;

        $scope.GSSProceesStatusList(item.statusid, item.GopalCreditCardId);

        if (!$scope.$$phase && !$scope.$root.$$phase) {
            $scope.$apply();
        }


        $('#actionmodal').modal('show');

    }


    //created by jaya
    $scope.SaveAction = function () {
        debugger;
        // Prepare the payload
        var payload = {
            ApplicationNo: $scope.payObj.ApplicationNo,
            ApplicantName: $scope.payObj.ApplicantName,
            PaymentType: $scope.payObj.PaymentType,
            PrincipalAmount: $scope.payObj.PrincipalAmount,
            RecoverableAmount: $scope.payObj.RecoverableAmount,
            StatusId: $scope.ApplicationFlowMaster.StatusId,
            Remark: $scope.ApplicationFlowMaster.Remark,
            IsEligibleForRenewable: $scope.ApplicationFlowMaster.IsEligibleForRenewable ? 1 : 0,
            ReportGccFile: $scope.ApplicationFlowMaster.ReportGccFile || null
        };

        $http.post('../GopalCreditCard/SaveRecoveryAction', payload)
            .then(function (response) {
                if (response.data && response.data.success) {
                    debugger
                    alert('Data saved successfully!');
                    $('#actionmodal').modal('hide');
                    $scope.GetGCCApplications();
                } else {
                    alert('Save failed: ' + (response.data.message || 'Unknown error.'));
                }
            })
            .catch(function (error) {
                alert('An error occurred while saving.');
            });
    };


    $scope.SavePayment = function () {
        debugger;
        if (($scope.payObj.PaymentType ?? null) == null) {
            alert("Please select Recovery as.");
            return;
        }

        if ($scope.payObj.Remark == null || $scope.payObj.Remark.trim() === "") {
            alert("Please fill the remark.");
            return;
        }

        if (!$scope.payObj.ReceiptFile) {
            $scope.showReceiptError = true;
            alert("Please upload receipt PDF file.");
            return;
        }

        $scope.showReceiptError = false;

        if (!confirm('Are you want to submit payment'))
            return;

        $(".se-pre-con").fadeIn("slow");

        if (!$scope.payObj.PaymentSummaryId == 1) {
            $scope.payObj.PaymentSummaryId = 0;
        }

        // Convert JSON to string
        var jsonStr = JSON.stringify($scope.payObj);

        var formData = new FormData();
        formData.append("json", jsonStr);

        if ($scope.payObj.ReceiptFile) {
            formData.append("ReceiptFile", $scope.payObj.ReceiptFile);
        }

        $http.post(SiteUrl + "/GopalCreditCard/SetGccRecoverablePayment",
            formData,
            {
                transformRequest: angular.identity,
                headers: { "Content-Type": undefined }
            }
        )
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");

                if (data > 0) {
                    $('#repaymentmodal').modal('hide');
                    ShowOkMessage("Success", "Payment Save successfully", function () {
                        $scope.payObj = {};
                        $scope.GetGccApplicationList(false);
                    });
                }
                else if (data == 2) {
                    $('#repaymentmodal').modal('hide');
                    ShowOkMessage("Success", "Date Updated successfully", function () {
                        $scope.payObj = {};
                        $scope.GetGccApplicationList(false);
                    });
                }
                else if (data == -2) {
                    $('#repaymentmodal').modal('hide');
                    ShowOkMessage("Duplicate", "Data already exists.", function () {
                        $scope.payObj = {};
                        $scope.GetGccApplicationList(false);
                    });
                }
                else if (data == -3) {
                    $('#repaymentmodal').modal('hide');
                    ShowOkMessage("Warning", "Loanee not eligible for payment.", function () {
                        $scope.payObj = {};
                        $scope.GetGccApplicationList(false);
                    });
                }
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GSSProceesStatusList = function (StatusId, GccId) {
        $scope.GccProceesStatus_Temp = [];
        $http.post("../gopalCreditCard/GSSProceesStatusList", { "StatusId": StatusId, "GCC_Id": 0 }).
            success(function (data, status) {
                $scope.StatusList = data.filter(x => (x.GccProceesStatusId > 30 && x.GccProceesStatusId < 34) || x.GccProceesStatusId == 11);
            }).
            error(function (data, status) {
                $scope.StatusList = [];
            });
    };
    $scope.OnPageLoad();

    $scope.GSSProceesStatusList = function (StatusId, GccId) {
        $scope.GccProceesStatus_Temp = [];

        $http.post("../gopalCreditCard/GSSProceesStatusList", { "StatusId": StatusId, "GCC_Id": GccId }).
            success(function (data, status) {
                $scope.GccProceesStatus_Temp = data;
                $scope.ProceesStatus = $scope.getDistinctProceesStatus();

                if ($scope.ProceesStatus.length == 1) {
                    $scope.Appstatus1 = false;
                    $scope.ApplicationFlowMaster.StatusId = $scope.ProceesStatus[0].GccProceesStatusId;
                }

                //if ($scope.GccProceesStatus_Temp.length == 1) { $scope.Appstatus = false; $scope.GccProceesStatus = $scope.GccProceesStatus_Temp; }
                //else { $scope.Appstatus = true; $scope.ProceesStatus = $scope.getDistinctProceesStatus(); }    
            }).
            error(function (data, status) {
                $scope.GccProceesStatus_Temp = [];
            });

    };

    $scope.StatusChange = function () {
        //debugger;
        var ddlStatus = $scope.ddlStatus;
        if (ddlStatus != '')
            $scope.GccProceesStatus = $scope.GccProceesStatus_Temp.filter(x => x.ProcessStatusType == ddlStatus);

        $scope.Appstatus1 = true;
        if ($scope.GccProceesStatus.length == 1) {
            $scope.Appstatus1 = false;
            $scope.ApplicationFlowMaster.StatusId = $scope.GccProceesStatus[0].GccProceesStatusId;
        }

        $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS = 0;
        $scope.ApplicationFlowMaster.PurposeOfLoan = "0";
        $scope.ApplicationFlowMaster.PurposeOfLoanOther = '';
    }

    $scope.getDistinctProceesStatus = function () {
        const statusMap = new Map();
        $scope.GccProceesStatus_Temp.forEach(item => {
            if (!statusMap.has(item.ProcessStatusType)) {
                statusMap.set(item.ProcessStatusType, item);
            }
        });
        return Array.from(statusMap.values());
    };



    $scope.saveDataInDB = function () {

        debugger;

        $scope.showLoader = true;
        var DocumentCheck = "";


        if ($scope.ApplicationFlowMaster.DocumentURL == undefined && ($scope.ApplicationFlowMaster.StatusId == 7 || $scope.ApplicationFlowMaster.StatusId == 8)) {

            alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');

            return;
        }

        if ($scope.ddlStatus == 'A') {

            var selectedOption = $scope.ApplicationFlowMaster.SelectedOption;

            if (selectedOption == '' || selectedOption == null || selectedOption == undefined) {
                alert(_language == 'en-us'
                    ? "Please select either 'Eligible for Renewal' or 'Loan Closer' before submitting."
                    : "कृपया 'Eligible for Renewal' या 'Loan Closer' में से कोई एक विकल्प चुनें।");
                $scope.showLoader = false;
                return;
            }
        }

        if ($scope.ApplicationFlowMaster.SelectedOption == 'renew') {
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = 1;
            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 0;
        }
        else if ($scope.ApplicationFlowMaster.SelectedOption == 'closer') {
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = 0;
            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 1;
        }
        else {
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = 0;
            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 0;
        }

        $scope.GopalCreditCardFormModel = $scope.GopalCreditCardModel;

        if ($scope.ApplicationFlowMaster.StatusId == 33) {

            if (!confirm((_language == 'en-us' ? '\n  Are you sure you want to reject this application ?' : '\n  क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?'))) {
                return;
            }

            $scope.ApplicationFlowMaster.IsEligibleForRenewable = false;
        }

        $scope.ApplicationFlowMaster.DocResStatus = DocumentCheck;
        $scope.ApplicationFlowMaster.GccId = $scope.payObj.GccId;
        $scope.GopalCreditCardModel.RegistrationNo = $scope.payObj.RegistrationNo;
        $scope.GopalCreditCardModel.GccIdDBEnc = $scope.payObj.encGccid;
        $scope.ApplicationFlowMaster.PaymentSummaryId = $scope.payObj.PaymentSummaryId;
        $scope.GopalCreditCardModel.DisbursedDate = $scope.payObj.DisbursedDate;
        $scope.GopalCreditCardModel.PaymentDate = $scope.payObj.PaymentDate;
        $scope.GopalCreditCardModel.SanctionAmt = $scope.payObj.sanctionAmt;
        $scope.GopalCreditCardModel.PACSName_ENG = $scope.payObj.PACSName_ENG;
        $scope.GopalCreditCardModel.Address_ENG = $scope.payObj.Address_ENG;
        //$scope.GopalCreditCardModel.IsEligibleForRenewable = $scope.payObj.IsEligibleForRenewable;
        //$scope.GopalCreditCardModel.IsSatisfiedWithInttCalcByPacs = $scope.payObj.IsSatisfiedWithInttCalcByPacs;
        $scope.GopalCreditCardModel.IsEligibleForRenewable = $scope.ApplicationFlowMaster.IsEligibleForRenewable;
        $scope.GopalCreditCardModel.IsSatisfiedWithInttCalcByPacs = $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs;

        $(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/gopalCreditCard/SaveGSSApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "GccId": $scope.GGG_Id, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId, "DocResStatus": DocumentCheck, "_Model": $scope.GopalCreditCardModel }).
        $http.post(SiteUrl + "/gopalCreditCard/SaveGSSApplicationFlowStatus_Recovery", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "_Model": $scope.GopalCreditCardModel, "_AppFlowModel": $scope.ApplicationFlowMaster, generalNumber: $scope.ApplicationFlowMaster.GeneralNumber }).

            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {

                    let msg = '';
                    if ($scope.ApplicationFlowMaster.StatusId == 31) {
                        msg = _language == 'en-us'
                            ? 'Approve payment successful\n'
                            : 'भुगतान स्वीकृति सफल रही\n';
                    }
                    else if ($scope.ApplicationFlowMaster.StatusId == 32) {
                        msg = _language == 'en-us'
                            ? 'NOC forwarded for Apporval to Pacs\n'
                            : 'एनओसी सफलतापूर्वक अग्रेषित हुआ\n';
                    }
                    else if ($scope.ApplicationFlowMaster.StatusId == 34) {
                        msg = _language == 'en-us'
                            ? 'NOC generated successfully\n'
                            : 'एनओसी सफलतापूर्वक जनरेट हुआ\n';
                    } else if ($scope.ApplicationFlowMaster.StatusId == 35) {
                        msg = _language == 'en-us'
                            ? 'Revert successfully\n'
                            : 'एनओसी सफलतापूर्वक जनरेट हुआ\n';
                    }
                    else {
                        msg = _language == 'en-us'
                            ? 'Data saved successfully\n'
                            : 'डेटा सफलतापूर्वक सहेजा गया\n';
                    }

                    ShowOkMessage("Success", msg, function () {
                        window.location.reload();
                    });

                    //ShowOkMessage("Success", _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n', function () { window.location.reload(); });
                }
                else if (data == -1) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
                }
                else if (data == -2) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                }
                else if (data == -3) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'Duplicate Jan Aadhaar Number  One Jan Aadhaar (Family) -> One Loan \n' : 'डुप्लीकेट जन आधार नंबर एक जन आधार (परिवार) -> एक ऋण\n');
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });

        $scope.showLoader = false;
    };


    function formatDate(date) {
        if (!date) return "";
        var d = new Date(date);
        var day = ("0" + d.getDate()).slice(-2);
        var month = ("0" + (d.getMonth() + 1)).slice(-2);
        var year = d.getFullYear();
        return day + "-" + month + "-" + year;
    }


    $scope.UploadReportFile = function (file) {
        debugger

        var maxSize = 5 * 1024 * 1024; // element.getAttribute("checkmaxsize") 
        if (file[0].size > maxSize) { return; }

        if (file) {
            if (file[0].type === 'application/pdf') {
                $scope.GccReportFile = file[0];
                $scope.ApplicationFlowMaster.DocumentURL = file[0];
                $scope.UploadeReportFileInDB();
            } else {
                alert('Please upload a PDF file only.');
            }
        }
    }

    $scope.UploadeReportFileInDB = function () {

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();


        if ($scope.ApplicationFlowMaster.StatusId == 7) {
            $scope.ApplicationFlowMaster.DocId = 1;//MokaReport
        }
        else if ($scope.ApplicationFlowMaster.StatusId == 8) {

            $scope.ApplicationFlowMaster.DocId = 2;//Branch Level Report
        }
        else if ($scope.ApplicationFlowMaster.StatusId == 4) {

            $scope.ApplicationFlowMaster.DocId = 5;//Branch Level Report mool dastavej
        }
        else {
            $scope.ApplicationFlowMaster.DocId = 4;
        }

        $scope.ApplicationFlowMaster.GccId = $scope.payObj.GccId;

        formData.append("id", 0);
        formData.append("DocId", $scope.ApplicationFlowMaster.DocId);
        formData.append("GccId", $scope.payObj.GccId);
        formData.append("RegistrationNo", $scope.payObj.RegistrationNo);
        formData.append("GccReportFile", $scope.GccReportFile);
        //formData.append("GccReportFile", $scope.GopalCreditCardModel.NocDocument);
        //formData.append("DocumentURL", $scope.ApplicationFlowMaster.DocumentURL);
        //formData.append("NocDocument", $scope.GopalCreditCardModel.NocDocument);



        $http.post(SiteUrl + "/gopalCreditCard/SaveGCCVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {

                $scope.ApplicationFlowMaster.DocumentURL = data.fileName;
                $scope.ApplicationFlowMaster.DocId = data.docId;
                //$scope.GopalCreditCardModel.NocDocument = data.NocDocument;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.VerifyOTP = function () {
        $scope.saveDataInDB();
    }

    $scope.SaveGSSApplicationFlowStatus = function (isResend) {

        $scope.IsResendOTP == true;

        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
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

                        $scope.ShowOTPDiv = true;
                        $('#otpmodal').modal('show');
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


    $scope.toggleSelectionWorking = function (item) {
        debugger;

        $scope.chkBulkSelected = false;

        if (!$scope.SelectedAppflowList) {
            $scope.SelectedAppflowList = [];
        }

        if (item.selected) {
            $scope.SelectedAppflowList.push(item);
            $scope.chkBulkSelected = true;
        } else {
            $scope.SelectedAppflowList = $scope.SelectedAppflowList.filter(
                member => member.GccIdDBEnc !== item.GccIdDBEnc
            );
        }

        $scope.payObj.GccId = item.GopalCreditCardId;

        if ($scope.SelectedAppflowList.length > 0) $scope.chkBulkSelected = true;
        else $scope.chkBulkSelected = false;

        $scope.GSSProceesStatusList(item.statusid, item.GopalCreditCardId);

    };

    $scope.toggleSelection = function (item) {
        if (item.selected) {
            $scope.Action(item);
        } else {
            item.selected = false;
        }
    };

    $scope.checkInput = function (event) {
        var key = event.keyCode || event.which;
        var forbiddenKeys = [34, 39, 47];

        if (forbiddenKeys.indexOf(key) !== -1) {
            event.preventDefault();
        }
    };


    //old
    $scope.SelectReceiptFile = function (files) {
        if (files && files.length > 0) {
            $scope.payObj.ReceiptFile = files[0];   // file object
            $scope.$apply();
        }
    };

    //Added by Narpat Kumawat on 03-06-2026
    $scope.SaveGSSApplicationFlowStatusNewEsign = function () {
        $scope.saveDataInDBNew();
    };
    $scope.RedirectToESPportal = function (data) {
        if (data && data.Message !== "Fail" && data.DocPath !== "" && data.DocPath !== null && data.DocPath !== undefined && data.DocPath !== "Fail") {
            $scope.dataPayload = data;
            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                docid: $scope.ApplicationFlowMaster.GccId,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                SocietyIDEnc: $scope.payObj.RegistrationNo,
                Remark: $scope.ApplicationFlowMaster.Remark,
                StatusId: $scope.ApplicationFlowMaster.StatusId,
                IsAmendment: false,
                AmendmendAmount: "",
                ReceiptNumber: "",
                Location: $scope.dataPayload.Location,
                Desination: $scope.dataPayload.Designation,
                _Model: $scope.GopalCreditCardModel,
                _AppFlowModel: $scope.ApplicationFlowMaster
            };

            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsignGCC($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        } else { ShowOkMessage("Error", "Invalid document generated"); }
    };
    $scope.saveDataInDBNew = function () {
        $scope.showLoader = true;
        var DocumentCheck = "";

        if ($scope.ApplicationFlowMaster.DocumentURL == undefined && ($scope.ApplicationFlowMaster.StatusId == 7 || $scope.ApplicationFlowMaster.StatusId == 8)) {
            alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');
            return;
        }

        if ($scope.ddlStatus == 'A') {
            var selectedOption = $scope.ApplicationFlowMaster.SelectedOption;

            if (selectedOption == '' || selectedOption == null || selectedOption == undefined) {
                alert(_language == 'en-us'
                    ? "Please select either 'Eligible for Renewal' or 'Loan Closer' before submitting."
                    : "कृपया 'Eligible for Renewal' या 'Loan Closer' में से कोई एक विकल्प चुनें।");
                $scope.showLoader = false;
                return;
            }
        }

        if ($scope.ApplicationFlowMaster.SelectedOption == 'renew') {
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = 1;
            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 0;
        }
        else if ($scope.ApplicationFlowMaster.SelectedOption == 'closer') {
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = 0;
            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 1;
        }
        else {
            $scope.ApplicationFlowMaster.IsEligibleForRenewable = 0;
            $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs = 0;
        }

        $scope.GopalCreditCardFormModel = $scope.GopalCreditCardModel;

        if ($scope.ApplicationFlowMaster.StatusId == 33) {

            if (!confirm((_language == 'en-us' ? '\n  Are you sure you want to reject this application ?' : '\n  क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?'))) {
                return;
            }

            $scope.ApplicationFlowMaster.IsEligibleForRenewable = false;
        }

        $scope.ApplicationFlowMaster.DocResStatus = DocumentCheck;
        $scope.ApplicationFlowMaster.GccId = $scope.payObj.GccId;
        $scope.GopalCreditCardModel.RegistrationNo = $scope.payObj.RegistrationNo;
        $scope.GopalCreditCardModel.GccIdDBEnc = $scope.payObj.encGccid;
        $scope.ApplicationFlowMaster.PaymentSummaryId = $scope.payObj.PaymentSummaryId;
        $scope.GopalCreditCardModel.DisbursedDate = $scope.payObj.DisbursedDate;
        $scope.GopalCreditCardModel.PaymentDate = $scope.payObj.PaymentDate;
        $scope.GopalCreditCardModel.SanctionAmt = $scope.payObj.sanctionAmt;
        $scope.GopalCreditCardModel.PACSName_ENG = $scope.payObj.PACSName_ENG;
        $scope.GopalCreditCardModel.Address_ENG = $scope.payObj.Address_ENG;
        $scope.GopalCreditCardModel.IsEligibleForRenewable = $scope.ApplicationFlowMaster.IsEligibleForRenewable;
        $scope.GopalCreditCardModel.IsSatisfiedWithInttCalcByPacs = $scope.ApplicationFlowMaster.IsSatisfiedWithInttCalcByPacs;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/gopalCreditCard/generategccnoc", { "_Model": $scope.GopalCreditCardModel, "_AppFlowModel": $scope.ApplicationFlowMaster, generalNumber: $scope.ApplicationFlowMaster.GeneralNumber }).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if ($scope.ApplicationFlowMaster.StatusId == 34 && angular.isObject(data)) {
                $scope.RedirectToESPportal(data);
                return;
            }

            if (data.Result > 0) {
                let msg = '';
                if ($scope.ApplicationFlowMaster.StatusId == 31) {
                    msg = _language == 'en-us' ? 'Approve payment successful\n' : 'भुगतान स्वीकृति सफल रही\n';
                }
                else if ($scope.ApplicationFlowMaster.StatusId == 32) {
                    msg = _language == 'en-us' ? 'NOC forwarded for Apporval to Pacs\n' : 'एनओसी सफलतापूर्वक अग्रेषित हुआ\n';
                }
                else if ($scope.ApplicationFlowMaster.StatusId == 35) {
                    msg = _language == 'en-us' ? 'Revert successfully\n' : 'एनओसी सफलतापूर्वक जनरेट हुआ\n';
                } else {
                    msg = _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सफलतापूर्वक सहेजा गया\n';
                }

                ShowOkMessage("Success", msg, function () {
                    window.location.reload();
                });
            }
            else if (data.Result == -1) {
                ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
            }
            else if (data.Result == -2) {
                ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
            }
            else if (data.Result == -3) {
                ShowOkMessage("Error", _language == 'en-us' ? 'Duplicate Jan Aadhaar Number  One Jan Aadhaar (Family) -> One Loan \n' : 'डुप्लीकेट जन आधार नंबर एक जन आधार (परिवार) -> एक ऋण\n');
            }
            else {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            alert("Error while insert");
        });

        $scope.showLoader = false;
    };
});

