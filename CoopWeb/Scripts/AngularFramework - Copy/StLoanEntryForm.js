
var AppTrainning = angular.module("AppEntryFormStLone", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("StLone", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Sum = 0;
    $scope.IsExcelUpload = false;
    $scope.ShowExcelData = false;
    $scope.ShowTableData = false;

    //START : Show Hide div
    $scope.ShowStLoneContent = function () {
        if ($scope.IsExcelUpload) {
            $scope.ShowExcelData = true;
            $scope.ShowTableData = false;
        }
        else {
            $scope.ShowExcelData = false;
            $scope.ShowTableData = false;
        }
    }
    //END : Show/Hide data
    //var StLoanFormDataList = [];
    //$scope.AuditorRequest = {};
    //var today = new Date();
    //var dd = today.getDate();
    //var mm = today.getMonth() + 1; //January is 0!

    //var yyyy = today.getFullYear();
    //if (dd < 10) {
    //    dd = '0' + dd;
    //}
    //if (mm < 10) {
    //    mm = '0' + mm;
    //}
    //var today = dd + '/' + mm + '/' + yyyy;

    //$scope.AuditorRequest["RegistrationDate"] = today;
    function StLoanFormDataTranlst() {
        this.Id = '0';
        this.TranID = '0';
        this.CCBId = '0';
        this.TotalTarget = '0.0';
        this.FIN_YEAR_MASTR_ID = '0';
        this.Total_Application_Received = '0';
        this.Approved_By_Pacs = '0';
        this.Approved_By_Branch = '0';
        this.DMR_Credited_And_Loan_Disbursement = '0'
        this.TillDate = '';
        this.CreatedBy = '0';

    }


    function FileDataWithRow() {
        this.UserId = '';
    }


    //GET Financial Year
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };



    $scope.GetYearTargetData = function () {
        if (!$scope.IsExcelUpload) {
            $scope.GridData();
        }
        else {
            $scope.ShowTableData = false;
        }

    }
    $scope.GridData = function () {
        //$scope.StLoneForm = {};
        $scope.StLoneForm.FIN_YEAR_MASTR_ID = $scope.FINYEARID;
        $http.post(SiteUrl + "/EntryForms/GetStLoanData", $scope.StLoneForm).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.StLoanData = data;
                    $scope.ShowTableData = true;
                    $scope.StLoneForm.ApexLoanDisbursementFYearId = $scope.StLoanData[0].ApexLoanDisbursementFYearId;
                    $scope.StLoneForm.TotalTarget = $scope.StLoanData[0].TotalTarget;
                    $('#txtRegistrationDate').val($scope.StLoanData[0].TillDateStr);

                    angular.forEach(data, function (item) {
                        $scope.Sum += parseInt(item.DMR_Credited_And_Loan_Disbursement);
                    });
                    $scope.StLoneForm.DMR_Credited_And_Loan_Disbursement = $scope.Sum;
                }
                else
                    $scope.ShowTableData = false;
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }
    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    }
    // START : Save Bulk Insert
    $scope.SaveBulkInsert = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.FINYEARID == undefined || $scope.FINYEARID =='0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Year' : '<br/>कृपया वर्ष चुनें |';
        }
        console.log($scope.AuditorRequest.RegistrationDate.length);
        if ($('#txtRegistrationDate').val().length ==0) {
            message += $scope._language == 'en-us' ? '<br/>Please Select Date' : '<br/>कृपया तिथि चुनें |';
        }

        if ($scope.StLoneForm.TotalTarget == undefined || $scope.StLoneForm.TotalTarget.length == 0) {
            message += $scope._language == 'en-us' ? '<br/>Please Write Total Target' : '<br/>कृपया कुल लक्ष्य लिखें |';
        }

        if ($scope.BulkInsertFile == undefined) {
            message += $scope._language == 'en-us' ? '<br/>Please Upload Excel Sheet' : '<br/>कृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Bulk Insert", message);
                return false;
            }
        }

        var formData = new FormData();
        alert($scope.StLoneForm.ApexLoanDisbursementFYearId);
        formData.append("ApexLoanDisbursementFYearId", $scope.StLoneForm.ApexLoanDisbursementFYearId);
        alert($scope.StLoneForm.FIN_YEAR_MASTR_ID);
        formData.append("FIN_YEAR_MASTR_ID", $scope.StLoneForm.FIN_YEAR_MASTR_ID);
        formData.append("TotalTarget", $scope.StLoneForm.TotalTarget);
        formData.append("TillDate", $('#txtRegistrationDate').val().split("/").reverse().join("-"));
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        $http.post(SiteUrl + "/EntryForms/SaveStLoanFormsExl", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                alert(data);
                location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert("Bulk Insert", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Society Profiling
    $scope.SaveStLone = function () {
        $scope.StLoneForm.FIN_YEAR_MASTR_ID = $scope.FINYEARID;
        if ($scope.StLoanFormValidation()) {
            var formData = new FormData();
            alert($scope.StLoneForm.ApexLoanDisbursementFYearId);
            formData.append("ApexLoanDisbursementFYearId", $scope.StLoneForm.ApexLoanDisbursementFYearId);
            formData.append("FIN_YEAR_MASTR_ID", $scope.StLoneForm.FIN_YEAR_MASTR_ID);
            formData.append("TotalTarget", $scope.StLoneForm.TotalTarget);
            formData.append("TillDate", $('#txtRegistrationDate').val().split("/").reverse().join("-"));
            formData.append("StLoanFormDataTranlst", JSON.stringify(StLoanFormDataList));
            $http.post(SiteUrl + "/EntryForms/SaveStLoanForms", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    var message = '';
                    if (data > 0) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    }
                    else if (data == -1) {
                        message = _language == 'en-us' ? 'Data Update successfully\n' : 'डेटा सफलतापूर्वक अपडेट किया गया है\n';
                    }
                    else if (data == -2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    }
                    alert(message);
                    window.location.reload();

                    if (data.Status) {
                        $scope.ClearBhandarForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Bhandar Data insert");
                });
        }

    };




    $scope.pageChangeHandler = function (num) {

    };
    $scope.update = function (item) {
        $scope.showtringdiv = true;
        item.IsActive = (item.IsActive == 1 ? true : false);
        $scope.BhandarForm = item;
    };

    $scope.ClearBhandarForm = function () {
        $scope.BhandarForm.FYearId = 0;
        $scope.BhandarForm.MonthId = 0;
        $scope.BhandarForm.ControlledBusinessfulfillment = '';
        $scope.BhandarForm.UnControlledBusinessFulfillment = '';
        $scope.BhandarForm.IsActive = '0';
        $scope.BhandarForm.BhandarDataId = 0;

    };
    // Update Template  

    $scope.SetStatus = function (ObjBhandarForm) {
        alert(ObjBhandarForm.IsActive);
        $scope.BhandarForm = ObjBhandarForm;
        $scope.BhandarForm.IsActive = ObjBhandarForm.IsActive == 1 ? 0 : 1;
        $scope.SaveBhandar(false);
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
        }
        $scope.GetFinancialYearList();

        $('#txtRegistrationDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            //defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear()).toString(),
            maxDate: '0D'
        });
    };
    // Language Selector at page load



    $scope.ShowBhandarContent = function () {
        $scope.showtringdiv = true;
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }
    //validateAddForm 
    $scope.StLoanFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        if ($scope.FINYEARID == undefined || $scope.FINYEARID == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select financial year' : '\n <br /> कृपया वित्तीय वर्ष का चयन करें |';
        }
        if ($('#txtRegistrationDate').val().length == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select Date' : '\n <br /> कृपया तारीख का चयन करें |';
        }
        if ($scope.StLoneForm.TotalTarget == undefined || $scope.StLoneForm.TotalTarget == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Controlled Business Target(Yearly Basis).' : '\n <br /> कृपया नियंत्रित व्यवसाय लक्ष्य (वार्षिक आधार) दर्ज करें |';
        }
        var IsValueMissing = false;

        var StLoanFormDataList = [];
        $('#TblStLoanData tbody tr').each(function () {
            var id = $(this).find('td:eq(0)').find('label').html();

            var txtTotal_Application_Received = $(this).find('td:eq(3)').find('input').val();

            if (txtTotal_Application_Received == '' || txtTotal_Application_Received == undefined) {
                $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px red');
                IsValueMissing = true;
            }
            else {
                $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px white');
            }

            var txtApproved_By_Pacs = $(this).find('td:eq(4)').find('input').val();
            if (txtApproved_By_Pacs == '' || txtApproved_By_Pacs == undefined) {
                $($($(this).find('td')[4]).find('input')).css('border', 'solid 1px red');
                IsValueMissing = true;
            }
            else {
                $($($(this).find('td')[4]).find('input')).css('border', 'solid 1px white');
            }

            var txtApproved_By_Branch = $(this).find('td:eq(5)').find('input').val();
            if (txtApproved_By_Branch == '' || txtApproved_By_Branch == undefined) {
                $($($(this).find('td')[5]).find('input')).css('border', 'solid 1px red');
                IsValueMissing = true;
            }
            else {
                $($($(this).find('td')[5]).find('input')).css('border', 'solid 1px white');
            }

            var txtDMR_Credited_And_Loan_Disbursement = $(this).find('td:eq(6)').find('input').val();
            if (txtDMR_Credited_And_Loan_Disbursement == '' || txtDMR_Credited_And_Loan_Disbursement == undefined) {
                $($($(this).find('td')[6]).find('input')).css('border', 'solid 1px red');
                IsValueMissing = true;
            }
            else {
                $($($(this).find('td')[5]).find('input')).css('border', 'solid 1px white');
            }
            message = "";
            if (IsValueMissing == true) {
                message += $scope._language == 'en-us' ? '\nPlease Fill Data where contain red border' : '\nकृपया फ़ाइल अपलोड करें जहाँ लाल सीमा है |';
                message += '<br />';

            }


            var StLoanFormDataTranlstObj = new StLoanFormDataTranlst();

            StLoanFormDataTranlstObj.CCBId = id;
            StLoanFormDataTranlstObj.Total_Application_Received = txtTotal_Application_Received;
            StLoanFormDataTranlstObj.Approved_By_Pacs = txtApproved_By_Pacs;
            StLoanFormDataTranlstObj.DMR_Credited_And_Loan_Disbursement = txtDMR_Credited_And_Loan_Disbursement;
            StLoanFormDataTranlstObj.Approved_By_Branch = txtApproved_By_Branch;
            StLoanFormDataTranlstObj.TillDate = $('#txtRegistrationDate').val().split("/").reverse().join("-");
            StLoanFormDataList.push(StLoanFormDataTranlstObj);

        });

        if (message.length > 23) {
            ShowModelAlert("St Loan Form", message);
            StLoanFormDataList = [];
            return false;
        }
        return true;

    }
    //End

    $scope.OnPageLoad();
    $scope.DivtringGrid = true;
});

