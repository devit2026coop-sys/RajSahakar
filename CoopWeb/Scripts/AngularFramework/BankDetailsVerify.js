var CCSComplaintController = angular.module("CCSComplainMod", ['angularUtils.directives.dirPagination']);
CCSComplaintController.controller("CCSComplainCnt", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridWarehouses = true;
    $scope.divInsert = false;
    $scope.IsDisabled = true;
    $scope.Warehouse = [];
    $scope.Complaint = [];
    $scope.UploadedDocuments = [];
    $scope.DocumentTypes = [];
    $scope.divDocumentList = false;
    $scope.FileCollection = [];
    $scope.divAADHARVerificationStep_1 = true;
    var lstEnclouserupload = [];
    var records = [];
    $scope.divExcelUpload = false;
    $scope.divExcelUploadView = false;
    $scope.ShowSubmitButton = true;
    $scope.Complaint = [];

    function Document() {
        this.FileName = '';
        this.File = '';
        this.DDLID = "DDLDocTypeID1";
    }

    //START : Add New Row
    $scope.AddNewRow = function () {
        var maxId = 0;



        if ($scope.UploadedDocuments.length > 9) {
            var overflowMessage = _language === 'en-us' ? 'Maximum upload limit for documents is 10.' : 'दस्तावेजों के लिए अधिकतम अपलोड सीमा 10 है।';
            alert(overflowMessage);
            return;
        }
        angular.forEach($scope.UploadedDocuments, function (i, j) {
            maxId = i.UId;
        });

        $scope.UploadedDocuments.push({ "UId": parseInt(maxId, 10) + 1, "FileName": '', "File": "" });

        //$('.SetCalender').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    maxDate: '0'
        //});

        //setTimeout(function () {
        //$(".dateed1").each(function () {
        //    $('.SetCalender').datepicker({
        //        showOn: "both",
        //        changeMonth: true,
        //        buttonImage: "../images/calender.png",
        //        changeYear: true,
        //        dateFormat: "dd/mm/yy",

        //    });
        //});

        //}, 1000);
        setTimeout(function () {
            $('.SetCalender').datepicker({
                showOn: "both",
                changeMonth: true,
                buttonImage: "../images/calender.png",
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: '0'
            });
        }, 2000);



    };
    //END: Add New Row

    //START : rdoDocTypeChange
    $scope.rdoDocTypeChange = function () {
        var checkedVal = parseInt($('input[type=radio][name=rdoDocumentUploadType]:checked').val(), 10);
        $scope.UploadedDocuments = [];
        $scope.divExcelUpload = false;
        $scope.divExcelUploadView = false;
        $('#file_Excel').val('');

        $scope.ExcelFile = null;
        if (checkedVal === 1) {
            $scope.UploadedDocuments.push({ "UId": 1, "FileName": '', "File": "" });
        }
        else {
            $scope.divExcelUpload = true;
        }
    };
    //END: rdoDocTypeChange

    //START : Add New Row
    $scope.GenerateRows = function () {

        if ($scope.Complaint.NoOfDocument === undefined || $scope.Complaint.NoOfDocument === '' || $scope.Complaint.NoOfDocument === '0') {
            $scope.divDocumentList = false;
            alert(_language === 'en-us' ? 'Value must be greater than 0' : 'मान 0 से अधिक होना चाहिए');
            return;
        }

        $scope.UploadedDocuments = [];
        for (var i = 0; i < $scope.Complaint.NoOfDocument; i++) {
            $scope.UploadedDocuments.push(new Document());
        }
        $scope.divDocumentList = true;
    };
    //END: Add New Row


    //START : Add Files For Passbook/Cross Cheque
    $scope.UploadPassbook = function (file, row) {
        if (file.length === 0) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            $scope.Passbook = null;
            $('#passbookEntry').val('');
            alert(_language === 'en-us' ? 'Please select file.' : 'कृपया फ़ाइल चुनें।');
            return;
        }

        var isValid = false;
        var filename = file[0].name;
        var message = '';
        var extension = filename.split("").reverse().join("");
        extension = extension.substring(0, extension.indexOf('.'));
        extension = extension.split("").reverse().join("").toLowerCase();

        if (extension === 'pdf' || extension === 'jpg' || extension === 'jpeg' || extension === 'png') {
            isValid = true;
        }
        else {
            message = _language === 'en-us' ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.jpg,.jpeg,.png' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.jpg,.jpeg,.png |';
        }


        if (!isValid) {
            $scope.Passbook = null;
            $('#passbookEntry').val('');
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            alert(message);
            return;
        }



        if (!checkFileLengthPage(file[0].size)) {
            $scope.Passbook = null;
            $('#passbookEntry').val('');
            alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 200 KB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 200 KB से कम करें|");
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            return;
        }

        $scope.Passbook = file[0];
    };
    // END : Add Files For Passbook/Cross Cheque

    //START : Check File Size
    function checkFileLengthPage(fileSized) {

        if (fileSized < 204800) {
            return true;
        }
        return false;
    }
    //END: 



    //START : Add Files in Collection
    $scope.UploadEnclouserFile = function (file, row) {

        if (file.length === 0) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            alert(_language === 'en-us' ? 'Please select file.' : 'कृपया फ़ाइल चुनें।');
            return;
        }

        var isValid = false;
        var filename = file[0].name;
        var message = '';
        var extension = filename.split("").reverse().join("");
        extension = extension.substring(0, extension.indexOf('.'));
        extension = extension.split("").reverse().join("").toLowerCase();

        if (extension === 'pdf' || extension === 'jpg' || extension === 'jpeg' || extension === 'png') {
            isValid = true;
        }
        else {
            message = _language === 'en-us' ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.jpg,.jpeg,.png' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.jpg,.jpeg,.png |';
        }


        if (!isValid) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            alert(message);
            return;
        }



        if (!checkFileLengthPage(file[0].size)) {
            alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 500 KB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 500 KB से कम करें|");
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            return;
        }

        var checkedVal = parseInt($('input[type=radio][name=rdoDocumentUploadType]:checked').val(), 10);

        var fileid = $(row).attr("fileid");
        var uidtemp = fileid.split('_')[2];
        var ddlId = "#ddl_" + fileid.split('_')[1];
        var ddllId = "#ddll_" + fileid.split('_')[1];

        var DDLVal = checkedVal === 1 ? $(ddlId).val() : $(ddllId).val();

        var payDate = $("#txtPayDate_" + fileid.split('_')[1]).val();
        var maturityDate = $("#txtMaturityDate_" + fileid.split('_')[1]).val();

        angular.forEach($scope.UploadedDocuments, function (i, j) {
            if (parseInt(i.UId, 10) === parseInt(uidtemp, 10)) {
                i.File = file[0];
                i.FileName = DDLVal;
                //i.FileTypeId = DDLVal;
                i.CCSComplaintDocTypeId = DDLVal;
                i.PayDate = payDate;
                i.MaturityDate = maturityDate;
            }
        });

        console.log($scope.UploadedDocuments);
    };
    // END : Add Files in Collection


    //START : Add Files For Excel Upload
    $scope.UploadExcelFile = function (file, row) {

        if (file.length === 0) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            $scope.ExcelFile = null;
            alert(_language === 'en-us' ? 'Please select file.' : 'कृपया फ़ाइल चुनें।');
            return;
        }

        var isValid = false;
        var filename = file[0].name;
        var message = '';
        var extension = filename.split("").reverse().join("");
        extension = extension.substring(0, extension.indexOf('.'));
        extension = extension.split("").reverse().join("").toLowerCase();

        if (extension === 'xlsx') {
            isValid = true;
        }
        else {
            message = _language === 'en-us' ? 'You have selected invalid Filetype. \n Valid Filetype .xlsx' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .xlsx |';
        }


        if (!isValid) {
            $(row).val('');
            $scope.ExcelFile = null;
            $(row).parent().find('.filesize').text('');
            alert(message);
            return;
        }
        $scope.ExcelFile = file[0];
    };
    // END : Add Files For Excel Upload

    //START : DocumentMoreThanTenInsert
    $scope.DocumentMoreThanTenInsert = function (file, row) {
        $(".se-pre-con").fadeIn("slow");

        //if (!$scope.ValidateForm()) {
        //    $(".se-pre-con").fadeOut("slow");
        //    return false;
        //}

        if ($scope.ExcelFile === undefined || $scope.ExcelFile === null || $scope.ExcelFile.length === 0) {
            $scope.ExcelFile = null;
            $(".se-pre-con").fadeOut("slow");
            alert(_language === 'en-us' ? 'Please select file.' : 'कृपया फ़ाइल चुनें।');
            return;
        }

        $scope.GetDocumentTypeList();
        var formData = new FormData();
        formData.append("ExcelFile", $scope.ExcelFile);

        $http.post(SiteUrl + "/Citizen/CCSComplaintDocumentUploadedList", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $scope.UploadedDocuments = [];

                angular.forEach(data, function (i, j) {
                    $scope.UploadedDocuments.push({
                        "UId": parseInt(j, 10) + 1,
                        "CCSComplaintDocTypeId": String(i.FileTypeId),
                        "FileTypeId": String(i.FileTypeId),
                        "AccountNo": i.AccountNo,
                        "PaidAmount": i.PaidAmount,
                        "PayDate": i.PayDate,
                        "MaturityAmount": i.MaturityAmount,
                        "MaturityDate": i.MaturityDate,
                        "FileName": '',
                        "File": ""
                    });
                });



                if ($scope.UploadedDocuments.length < 1) {
                    alert(_language === 'en-us' ? 'Excel has not any data.Please upload excel with proper data.' : 'एक्सेल में कोई डेटा नहीं है। कृपया उचित डेटा के साथ एक्सेल अपलोड करें।');
                    $scope.divExcelUpload = true;
                    $scope.divExcelUploadView = false;
                }
                else {
                    $scope.divExcelUpload = false;
                    $scope.divExcelUploadView = true;
                }


            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowModelAlert("Message", "Error");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    // END : DocumentMoreThanTenInsert

    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_1_2 = function () {

        //$scope.disablebtnSendOtp = true;


        if (!$('#ckkAADHARAcceptance').prop('checked')) {
            alert('Please allow system for your AADHAAR based Authentication/ eKYC.');
            return;
        }

        if ($scope.Complaint.AADHARNo === undefined || $scope.Complaint.AADHARNo.length < 12) {
            var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
            $scope.disablebtnSendOtp = false;
            ShowOkMessage($scope.translation.CCSComplaint, msg);
            return;
        }


        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;
        $scope.divInsert = false;



        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.Complaint.AADHARNo }).
            success(function (data, status) {

                if (data === 'LOGOUT') {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                if (data.Result) {
                    $scope.Complaint.txn = data.txnid;
                    $scope.Complaint.AADHARNoEnc = data.AadharNo;
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    alert(msg1);
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'INVALID AADHAAR No.' : 'अमान्य आधार नंबर';
                    $scope.divAADHARVerificationStep_1 = true;
                    alert(msg2);
                }
            }).
            error(function (data, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Verify AADHAr No.

    //START : ResendOTP
    $scope.ResendOTP = function () {

        $scope.disablebtnSendOtp = true;
        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;
        $scope.divInsert = false;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.Complaint.AADHARNo }).
            success(function (data, status) {

                if (data === 'LOGOUT') {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                if (data.Result) {
                    $scope.Complaint.txn = data.txnid;
                    $scope.Complaint.AADHARNoEnc = data.AadharNo;
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    alert(msg1);
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'INVALID AADHAAR No.' : 'अमान्य आधार नंबर';
                    alert(msg2);
                }
            }).
            error(function (data, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : ResendOTP

    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function () {

        if ($scope.Complaint.EnterOTP === undefined || $scope.Complaint.EnterOTP.length != 6) {
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            alert(msg);
            return;
        }

        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.Complaint.EnterOTP, "txn": $scope.Complaint.txn, "_AadharNo": $scope.Complaint.AADHARNoEnc }).
            success(function (data, status) {

                if (data === 'LOGOUT') {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                if (data.Result === false) {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2);
                    return;
                }
                else if (data.Result === true) {
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.divInsert = true;
                    $scope.Complaint.ComplainerName = data.AADHARDetails[0].Column2;
                    $scope.Complaint.ComplainerFatherName = data.AADHARDetails[0].Column9;
                    $scope.Complaint.Address = data.AADHARDetails[0].Column10;
                }
                else {
                    return;
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdatanew = [];
            });
    };
    // END : Verify AADHAr No.

    //START : RemoveRow
    $scope.RemoveRow = function (index) {
        $scope.UploadedDocuments.splice(index, 1);
        $scope.FileCollection.splice(index, 1);
    };
    //END: RemoveRow

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

     
        //$scope.AddNewRow.push({ "UId": 1, "FileName": '', "File": "" });


    };
    // END : Page load Function

    // START : Get Document Type List
    $scope.GetDocumentTypeList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCSCOMPLAINTDOCTYPE").
            success(function (data, status) {
                $scope.DocumentTypes = data;
            }).
            error(function (data, status) {
                $scope.DocumentTypes = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get Document Type List

    //START : District Change 
    $scope.SelectionChange = function () {
        if ($scope.Complaint.UrbanRural === '2') {
            $scope.BlockList();
            $scope.Complaint.BlockId = '0';
            $scope.GramPanchayats = [];
            $scope.Complaint.GPId = '0';
            $scope.Villages = [];
            $scope.Complaint.VillageId = '0';
        }
        else {
            $scope.GetCityList();
            $scope.Complaint.CityId = '0';
            $scope.Ward = [];
            $scope.Complaint.WardId = '0';
        }
    };
    //END : District Change

    // START : Get City List By City Type
    $scope.GetCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.CityList = [];
        $scope.Warehouse.CityId = '0';

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.Complaint.DistrictId + "&MasterCode2=0").
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get City List By City Type

    // START : Get Ward List
    $scope.GetWardList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Ward = [];
        $scope.Complaint.WardId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.Complaint.CityId).
            success(function (data, status) {
                $scope.Ward = data;

            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get Ward List

    $scope.Verify = function (Id) {
        $(".se-pre-con").fadeIn("slow");
        $scope.BankData = [];
        $http.post(SiteUrl + "/Society/Updateverifydata", {"BankDetailId":Id }).
            success(function (data, status) {
                $scope.BankData = data;
            }).
            error(function (data, status) {
                $scope.BankData = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
        $scope.GetBankDetailsVaryfy();
    }
    
    // START : Get District List
    $scope.GetBankDetailsVaryfy = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.BankData = [];

        $http.get(SiteUrl + "/Society/GetBankDetailsForVerification").
            success(function (data, status) {
                $scope.BankData = data;
            }).
            error(function (data, status) {
                $scope.BankData = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get District List

    // START : Get Block List
    $scope.BlockList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];
        $scope.Warehouse.BlockId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.Complaint.DistrictId).
            success(function (data, status) {
                $scope.Blocks = data;
            }).
            error(function (data, status) {
                $scope.Blocks = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get Block List

    // START : Get GP List
    $scope.GramPanchayatList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GramPanchayats = [];
        $scope.Complaint.GPId = '0';
        $scope.Villages = [];
        $scope.Complaint.VillageId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.Complaint.BlockId).
            success(function (data, status) {
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get GP List

    // START : Get Village List
    $scope.VillageList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Villages = [];
        $scope.Complaint.VillageId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.Complaint.GPId).
            success(function (data, status) {
                $scope.Villages = data; console.log(data);
            }).
            error(function (data, status) {
                $scope.Villages = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get Village List

    // START : Get Data List
    $scope.WarehouseList = function () {
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": "0" }).
            success(function (data, status) {
                $scope.Warehouses = data;
                $scope.TotalWarehouses = data.length;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function () {
                $scope.Warehouses = [];
            });
    };
    // END : Get Data List

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



    //START : Back To List
    $scope.BackToList = function () {
        //$scope.ClearForm();
        //$scope.GridWarehouses = true;
        //$scope.divInsert = false;
        window.location.reload();
    };
    //END : Back To List

    //START :Excel Again Upload
    $scope.ExcelAgainUpload = function () {
        $scope.divExcelUploadView = false;
        $scope.divExcelUpload = true;
        $scope.ExcelFile = null;
        $('#file_Excel').val('');
        $scope.UploadedDocuments = [];
    };
    //END : Excel Again Upload

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Complaint === undefined || $scope.Complaint.ComplainerName === undefined || $scope.Complaint.ComplainerName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Complainer Name ' : '\n <br /> कृपया शिकायतकर्ता का नाम दर्ज करें |';
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

    //START : Save/Edit Data
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ShowSubmitButton = false;

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            $scope.ShowSubmitButton = true;
            return false;
        }
        var email = $scope.Complaint === undefined || $scope.Complaint.Email === undefined || $scope.Complaint.Email.length < 1 ? '' : $scope.Complaint.Email;
        var formData = new FormData();
        formData.append("ComplaintId", $scope.Complaint.ComplaintId);
        formData.append("ComplainerName", $scope.Complaint.ComplainerName);
        formData.append("ComplainerFatherName", $scope.Complaint.ComplainerFatherName);
        formData.append("DistrictId", $scope.Complaint.DistrictId);
        formData.append("Address", $scope.Complaint.Address);
        formData.append("MobileNo", $scope.Complaint.MobileNo);
        formData.append("Email", email);
        formData.append("CCSId", $scope.Complaint.CCSId);
        formData.append("BranchName", $scope.Complaint.BranchName);
        formData.append("AccountNo", $scope.Complaint.AccountNo === undefined || $scope.Complaint.AccountNo.length < 1 ? '' : $scope.Complaint.AccountNo);
        formData.append("IFSCCode", ($scope.Complaint === undefined || $scope.Complaint.IFSCCode === undefined || $scope.Complaint.IFSCCode.length < 1) ? '' : $scope.Complaint.IFSCCode);
        formData.append("AmountDeposited", $scope.Complaint.AmountDeposited);
        formData.append("DepositDate", '');
        formData.append("AADHAR", $scope.Complaint.AADHARNoEnc);
        formData.append("UserType", $('#hdnUserType').val());
        formData.append("MembershipId", $scope.Complaint.MembershipId === undefined || $scope.Complaint.MembershipId.length < 1 ? '' : $scope.Complaint.MembershipId);
        formData.append("Remarks", $scope.Complaint.Remarks);

        formData.append("AccountNoInSociety", $scope.Complaint.AccountNoInSociety);
        formData.append("AccountIFSCInSociety", $scope.Complaint.AccountIFSCInSociety);
        formData.append("AccountHolderName", $scope.Complaint.AccountHolderName);
        formData.append("RemittancePassbook", $scope.Passbook);
        formData.append("BPNo", $scope.Complaint.BPNo);


        angular.forEach($scope.UploadedDocuments, function (i, j) {
            formData.append("File_" + j, i.File);
            formData.append("FileDetails[" + j + "].FileTypeId", i.FileTypeId);
            formData.append("FileDetails[" + j + "].AccountNo", i.AccountNo);
            formData.append("FileDetails[" + j + "].PaidAmount", i.PaidAmount);
            formData.append("FileDetails[" + j + "].PayDate", i.PayDate);
            formData.append("FileDetails[" + j + "].MaturityAmount", i.MaturityAmount);
            formData.append("FileDetails[" + j + "].MaturityDate", i.MaturityDate);
        });



        $http.post(SiteUrl + "/Citizen/SaveCCSComplaint", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data === 'LOGOUT' || data.length > 1500) {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                $scope.divInsert = false;
                $scope.IsDisabled = true;
                $scope.Complaint = [];
                $scope.UploadedDocuments = [];
                $scope.DocumentTypes = [];
                $scope.divDocumentList = false;
                $scope.FileCollection = [];
                $scope.divAADHARVerificationStep_1 = true;
                $scope.divExcelUpload = false;
                $scope.divExcelUploadView = false;

                $scope.ClearForm();
                $scope.Complaint.AADHARNo = '';
                $scope.Complaint.EnterOTP = '';
                $scope.divAADHARVerificationStep_1 = true;
                $scope.divAADHARVerificationStep_2 = false;
                $scope.divInsert = false;
                $scope.DivResponce = true;
                $('#Responce').html(data);
                $(".se-pre-con").fadeOut("slow");
                $("#addModal").modal("show");
                $scope.ShowSubmitButton = false;




                //if (parseInt(data, 10) > 0) {
                //    $(".se-pre-con").fadeOut("slow");
                //    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                //}
                //else if (parseInt(data, 10) === -1) {
                //    $(".se-pre-con").fadeOut("slow");
                //    ShowModelAlert($scope.translation.CCSComplaint, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                //}
                //else if (parseInt(data, 10) === -2) {
                //    $(".se-pre-con").fadeOut("slow");
                //    ShowModelAlert($scope.translation.CCSComplaint, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                //}
                //window.location.reload();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowModelAlert("Message", "Error");
            });

        $(".se-pre-con").fadeOut("slow");

    };
    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        $scope.Warehouse["WarehousId"] = item.WarehousId;
        $scope.Warehouse["WarehousNameEn"] = item.WarehousNameEn;
        $scope.Warehouse["WarehousNameHi"] = item.WarehousNameHi;
        $scope.Warehouse["LicenceNo"] = item.LicenceNo;
        $('#txtLicenceValidUpto').val(item.LicenceValidUpTo);
        $scope.Warehouse["UrbanRural"] = item.ISCITY === true || item.ISCITY === 'TRUE' || item.ISCITY === 'True' || item.ISCITY === '1' || item.ISCITY === 1 ? '1' : '2';
        $scope.Warehouse["IsCentralWarehouse"] = item.IsCentralWarehouse === true || item.IsCentralWarehouse === 'TRUE' || item.IsCentralWarehouse === 'True' || item.IsCentralWarehouse === '1' || item.IsCentralWarehouse === 1 ? '1' : '2';

        $scope.DistrictList();
        $scope.Warehouse["DistrictId"] = item.DISTRICT_ID;

        if (item.ISCITY === true || item.ISCITY === 'TRUE' || item.ISCITY === 'True' || item.ISCITY === '1' || item.ISCITY === 1) {
            $scope.GetCityList();
            $scope.Warehouse["CityId"] = item.CITY_ID;
        }
        else {
            $scope.BlockList();
            $scope.Warehouse["BlockId"] = item.BLOCK_ID;
        }

        $scope.Warehouse["Address"] = item.Address;
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
    };
    //END : Edit

    //START : Set Status
    $scope.SetStatus = function (Id, ActionId) {
        $(".se-pre-con").fadeIn("slow");

        if (ActionId === 2) {
            var confirmMsg = _language === 'en-us' ? 'Are you sure to delete this record?' : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं?';

            if (!confirm(confirmMsg)) {
                $(".se-pre-con").fadeOut("slow");
                return false;
            }
        }

        $http.post(SiteUrl + "/Master/RajfedWarehouseSetStatus/", { "WarehousId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.Warehous, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.WarehouseList();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Set Status

    $scope.GetCCSList = function () {
        $(".blockUI").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $scope.Complaint.DistrictId }];
        $http.post(SiteUrl + "/Citizen/GetCreditCooperativeSocieties", { paramlist: model }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.Complaint.CCSId = '0';
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".blockUI").fadeOut("slow");
            });
        $(".blockUI").fadeOut("slow");
    };

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.GetBankDetailsVaryfy();
    // END : On Page Load
});