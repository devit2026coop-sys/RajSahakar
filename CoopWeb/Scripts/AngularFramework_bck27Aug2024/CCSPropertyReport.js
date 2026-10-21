var CCSComplaintController = angular.module("CCSPropertyReportCnt", ['angularUtils.directives.dirPagination']);
CCSComplaintController.controller("CCSPropertyReportMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridWarehouses = true;
  

    $scope.GetReportData = function (SId) {
            $http.get(SiteUrl + "/MIS/GetPropertyDetailsReport?SID=" + SId).
            success(function (data, status) {
                $scope.PropertyReportdata = data.Table;
             
            }).
            error(function (data, status) {
                $scope.CaseRegDetailListData = [];
            });
    }

    // START : Page load Function
    $scope.OnPageLoad = function () {
        
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

        $('#txtRegistrationDate1').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0'
        });

     
        //$scope.AddNewRow.push({ "UId": 1, "FileName": '', "File": "" });


    };
    // END : Page load Function
    $scope.Statedata = function () {
        $http.post("/CommonServices/Mobile_GetStateListCCSLocation").
            success(function (data, status) {
                $scope.StateListdata = data;
            }).
            error(function (data, status) {
                $scope.StateListdata = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Complaint = [];
        $scope.Complaint.ComplaintId = '0';
        $scope.Complaint.ComplainerName = '';
        $scope.Complaint.ComplainerFatherName = '';
        $scope.Complaint.DistrictId = '0';
        $scope.Complaint.UrbanRural = '1';
        $scope.CityList = [];
        $scope.Complaint.CityId = '0';
        $scope.Ward = [];
        $scope.Complaint.WardId = '0';
        $scope.Blocks = [];
        $scope.Complaint.BlockId = '0';
        $scope.GramPanchayats = [];
        $scope.Complaint.GPId = '0';
        $scope.Villages = [];
        $scope.Complaint.VillageId = '0';
        $scope.Complaint.Address = '';
        $scope.Complaint.MobileNo = '';
        $scope.Complaint.Email = '';
        $scope.Complaint.CCSId = '0';
        $scope.Complaint.BranchId = '0';
        $scope.Complaint.IFSC = '0';
        $scope.Complaint.AmountDepoisted = '0';
        $('#DepositDate').val('');

    };
    //END : Clear Form




    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Complaint === undefined || $scope.Complaint.ComplainerName === undefined || $scope.Complaint.ComplainerName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Complainant Name ' : '\n <br /> कृपया शिकायतकर्ता का नाम दर्ज करें |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.ComplainerFatherName === undefined || $scope.Complaint.ComplainerFatherName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Father Name' : '\n <br /> कृपया पिता का नाम दर्ज करें  |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.Address === undefined || $scope.Complaint.Address.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Address ' : '\n <br /> कृपया पता दर्ज करें |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.MobileNo === undefined || $scope.Complaint.MobileNo.length < 10) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Mobile no. in 10 digits' : '\n <br /> कृपया 10 अंकों का मोबाइल नंबर दर्ज करें |';
        }

        //if ($scope.Complaint === undefined || $scope.Complaint.Email === undefined || $scope.Complaint.Email.length < 1) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please Enter e-mail Id' : '\n <br /> कृपया ईमेल आईडी दर्ज करें |';
        //}
        if ($scope.Complaint === undefined || $scope.Complaint.DistrictId === undefined || $scope.Complaint.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select district' : '\n <br /> कृपया ज़िला चुने|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.CCSId === undefined || $scope.Complaint.CCSId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Credit Credit Society' : '\n <br /> कृपया क्रेडिट को-ऑपरेटिव सोसायटी नाम चुनें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.BranchName === undefined || $scope.Complaint.BranchName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Branch' : '\n <br /> कृपया ब्रांच दर्ज करें  |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.BPNo === undefined || $scope.Complaint.BPNo.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Customer No. (BP No.)' : '\n <br /> कृपया ग्राहक संख्या (बीपी नंबर) दर्ज करें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.AccountNoInSociety === undefined || $scope.Complaint.AccountNoInSociety.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Bank A/C No. in Society' : '\n <br /> कृपया रेमिटेंस  के बैंक खाता संख्या दर्ज करें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.AccountIFSCInSociety === undefined || $scope.Complaint.AccountIFSCInSociety.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter IFSC Code of Bank A/C in Committee' : '\n <br /> कृपया रेमिटेंस के बैंक खाता आई एफ एस सी कोड दर्ज करें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.AccountHolderName === undefined || $scope.Complaint.AccountHolderName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter A/C Holder Name' : '\n <br /> कृपया खाता धारक का नाम दर्ज करें|';
        }

        if ($scope.Passbook === undefined || $scope.Passbook.length === 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Upload Passbook/Cross Cheque Copy' : '\n <br /> कृपया पासबुक / क्रॉस चेक कॉपी अपलोड करें ';
        }


        //if ($scope.Complaint === undefined || $scope.Complaint.MembershipId === undefined || $scope.Complaint.MembershipId.length < 1) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please Enter Membership Id' : '\n <br /> कृपया सदस्यता क्रमांक दर्ज करें|';
        //}

        //if ($scope.Complaint === undefined || $scope.Complaint.AccountNo === undefined || $scope.Complaint.AccountNo.length < 1) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please Enter Bank Account No.' : '\n <br /> कृपया बैंक खाता नम्बर दर्ज करें |';
        //}



        //if ($scope.Complaint === undefined || $scope.Complaint.IFSCCode === undefined || $scope.Complaint.IFSCCode.length < 1) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please Enter IFSC Code' : '\n <br /> कृपया आई एफ एस सी नंबर दर्ज करें |';
        //}


        if ($scope.Complaint === undefined || $scope.Complaint.AmountDeposited === undefined || $scope.Complaint.AmountDeposited.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Total Deposited Amount' : '\n <br /> कृपया कुल जमा राशि दर्ज करें |';
        }

        //if ($('#txtDepositDate').val().length < 1) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please Enter Deposit Date' : '\n <br /> कृपया जमा करने की तारीख दर्ज करें |';
        //}

        if ($scope.Complaint === undefined || $scope.Complaint.Remarks === undefined || $scope.Complaint.Remarks.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Remark' : '\n <br /> कृपया टिप्पणी दर्ज करें  |';
        }

        var fileNotAvailable = 0;
        var CalculateAmount = 0;

        angular.forEach($scope.UploadedDocuments, function (i, j) {
            if (i.File === undefined || i.File === '' ||
                i.FileTypeId === undefined || i.FileTypeId === '0' ||
                i.AccountNo === undefined || i.AccountNo < 1 ||
                i.PaidAmount === undefined || i.PaidAmount < 1 ||
                i.PayDate === undefined || i.PayDate < 1 ||
                i.MaturityAmount === undefined || i.MaturityAmount < 1 ||
                i.MaturityDate === undefined || i.MaturityDate < 1) {
                fileNotAvailable++;
            }
        });

        $scope.UploadedDocuments.forEach(function (i, j) {
            if (i.PaidAmount !== undefined && parseInt(i.PaidAmount, 10) > 0) {
                CalculateAmount += parseInt(i.PaidAmount, 10);
            }
        });
        if ($scope.Complaint.DocumentUploadType === '1') {
            $('#tblBody input:visible').each(function () {
                if ($(this).val().length == 0) {
                    $(this).css('border', 'solid 1px red');
                    ValidationChaeck = false;
                }
                else {
                    $(this).css('border', 'solid 1px');
                }
            });
            $('#tblBody select:visible').each(function () {
                if ($(this).val().length > 3) {
                    $(this).css('border', 'solid 1px red');
                    ValidationChaeck = false;
                }
                else {
                    $(this).css('border', 'solid 1px');
                }
            });
        }
        else if ($scope.divExcelUploadView == true) {
            $('#tblBody2 input').each(function () {
                if ($(this).val().length == 0) {
                    $(this).css('border', 'solid 1px red');
                    ValidationChaeck = false;
                }
                else {
                    $(this).css('border', 'solid 1px');
                }
            });
            $('#tblBody2 select').each(function () {
                if ($(this).val().length > 3) {
                    $(this).css('border', 'solid 1px red');
                    ValidationChaeck = false;
                }
                else {
                    $(this).css('border', 'solid 1px');
                }
            });
        }

        if (fileNotAvailable > 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please provide all records(red Marked) related to documents ' : '\n <br /> कृपया दस्तावेजों से संबंधित सभी रिकॉर्ड(लाल रंग चिह्नित) प्रदान करें |';
        }

        if (parseInt(CalculateAmount, 10) !== parseInt($scope.Complaint.AmountDeposited, 10)) {
            message += $scope._language === 'en-us' ? '\n <br /> The sum of the deposited amount and the total amount deposited are not equal' : '\n <br />  जमा कराई गई राशि का योग एवं कुल जमा राशि बराबर नहीं है  |';
        }

        if (message.length > 5) {
            ShowModelAlert($scope.translation.CCSComplaint, message);
            return false;
        }
        return true;
    };
    //END : Validate Form



    // START : On Page Load
    $scope.OnPageLoad();
    $scope.GetReportData();
    $scope.Statedata();

    // END : On Page Load
});