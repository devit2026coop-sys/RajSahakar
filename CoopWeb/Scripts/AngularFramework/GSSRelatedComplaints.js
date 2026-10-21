
var MasterMod = angular.module('GSSComplaintsModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("GSSComplaintsCont", function ($scope, $http) {

    // SCOPE
    _language = 'hi-in';
    $scope._language = 'hi-in';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.GssComplaint = {};
    $scope.Department = [];
    $scope.GetGssComplaint = [];
    
    // START : Code for Language label setting
    $scope.getTranslation = function () {
        _language = 'hi-in';
        $scope._language = _language;

        if (_language === 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;

                }).
                error(function (data, status) {
                    alert("Error while translation-En");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;

                }).
                error(function (data, status) {
                    alert("Error while translation-Hi");
                });
        }
    };
    $scope.getTranslation();
    // END : Code for Language label setting


    //Madhuri code start

    $scope.SSOUserDetails = function () {
        $http.get(SiteUrl + "/Citizen/GetSSOUserDetails/").
            success(function (data, status) {
                if (data != null && data.SSOID != "Data Could Not Be Retrieved From The Server Please Try After Some Time") {
                    $scope.UserRegistration = data;

                    $scope.GssComplaint.AadharNo = $scope.UserRegistration.AadharNo;
                    $scope.GssComplaint.SSOID = $scope.UserRegistration.SSOID;
                    $scope.GssComplaint.EmailId = $scope.UserRegistration.EmailId;
                    $scope.GssComplaint.NameEn = $scope.UserRegistration.OfficerNameEn;
                    $scope.GssComplaint.MobileNo = $scope.UserRegistration.MobileNo;
                    $scope.GssComplaint.Gender = $scope.UserRegistration.Gender;
                    $scope.GssComplaint.Address = $scope.UserRegistration.Address;

                }
                else
                    alert("Data Could Not Be Retrieved From The Server Please Try After Some Time");
            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
            });
    };

    //$scope.GssComplaintValidate = function () {
    //    var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
    //    if ($scope.GssComplaint.IsMember == 0) {
    //        if ($scope.GssComplaint.IsAppRejected == 1) {
    //            if ( $('#RejectedDate').val() == '') {
    //                message += $scope._language == 'en-us' ? '<br />Please Fill The Rejected Date' : '<br />कृपया अस्वीकृत तिथि भरें।';
    //            }
    //            else {
    //                $scope.GssComplaint.RejectedDate = $('#RejectedDate').val();
    //            }
    //            if ($scope.GssComplaint.GssSocietyName == '') {
    //                message += $scope._language == 'en-us' ? '<br />Please Fill Gram Seva Cooperative Society Name' : '<br />कृपया ग्राम सेवा सहकारी समिति का नाम भरें।';

    //            }
    //        }
    //        else if ($scope.GssComplaint.IsAppRejected == 0) {
    //            message += $scope._language == 'en-us' ? '<br />Form can not be filled ' : '<br />फॉर्म नहीं भरा जा सकता';

    //        }
    //        else if ($scope.GssComplaint.IsAppRejected == null) {
    //            message += $scope._language == 'en-us' ? '<br />Please choose- Has your application been rejected ' : '<br />कृपया भरें। -क्या आपका आवेदन अस्वीकार कर दिया गया है?';
    //        }


    //    }
    //    else if ($scope.GssComplaint.IsMember == 1) {
    //        if ($scope.GssComplaint.GssSocietyName == '') {
    //            message += $scope._language == 'en-us' ? '<br />Please Fill Gram Seva Cooperative Society Name' : '<br />कृपया ग्राम सेवा सहकारी समिति का नाम भरें।';

    //        }
    //        if ($scope.GssComplaint.IsGetLoan == 1) {
    //            if ($scope.GssComplaint.txtGetloan == '') {
    //                message += $scope._language == 'en-us' ? '<br />Please Fill The Loan Amt' : '<br />कृपया ऋण राशि भरें।';
    //            }

    //        }
    //        else if ($scope.GssComplaint.IsGetLoan == 0) {
    //            if ($scope.GssComplaint.IsApplyForLoan == 1 && $scope.GssComplaint.txtRemark == '') {
    //                message += $scope._language == 'en-us' ? '<br />Please Apply for loan field Text' : '<br />कृपया ऋण के लिए आवेदन करें।';
    //            }
    //        }
    //    }
    //    else { message += $scope._language == 'en-us' ? '<br />Please choose- Are you a member of Gram Seva Cooperative Society ' : '<br />कृपया भरें। - क्या आप ग्राम सेवा सहकारी समिति के सदस्य हैं?'; }

    //    if ($scope.GssComplaint.CoopBankName == '') { message += $scope._language == 'en-us' ? '<br />Please Enter Name of Central Cooperative Bank' : '<br />कृपया केंद्रीय सहकारी बैंक का नाम भरें।'; }


    //    if ($scope.GssComplaint.Address == '' || $scope.GssComplaint.Address == undefined || $scope.GssComplaint.Address == null) {        message += $scope._language == 'en-us' ? '<br />Please Fill The Address' : '<br />कृपया पता भरें।'; }
    //    if ($scope.GssComplaint.MobileNo == '' || $scope.GssComplaint.MobileNo == undefined || $scope.GssComplaint.MobileNo == null) { message += $scope._language == 'en-us' ? '<br />Please Fill The MobileNo' : '<br />कृपया मोबाइल नंबर भरें।'; }

    //    if ($scope.GssComplaint.NameEn == '' || $scope.GssComplaint.NameEn == undefined || $scope.GssComplaint.NameEn == null) { message += $scope._language == 'en-us' ? '<br />Please Fill The Name' : '<br />कृपया नाम दर्ज करें।'; }
    //    if ($scope.GssComplaint.EmailId == '' || $scope.GssComplaint.EmailId == undefined || $scope.GssComplaint.EmailId == null) { message += $scope._language == 'en-us' ? '<br />Please Fill The EmailId' : '<br />कृपया ईमेल आईडी  भरें।'; }

    //    if (message.length > 29) {
    //       // alert(message);
    //        ShowOkMessage("Gram Seva Society Complaint ", message, function () { });
    //        return false;
    //    } else {
    //        return true;
    //    }

    //}

    $scope.GssComplaintValidate = function () {

        var message = $scope._language == 'en-us'? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if (!$scope.GssComplaint.EmailId) {
            message += $scope._language == 'en-us'? '<br/>Please Enter Email ID' : '<br/>कृपया ईमेल आईडी भरें।';
        }
        else {
            var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailPattern.test($scope.GssComplaint.EmailId)) {
                message += $scope._language == 'en-us' ? '<br/>Please Enter Valid Email ID': '<br/>कृपया सही ईमेल आईडी दर्ज करें।';
            }
        }

        if (!$scope.GssComplaint.NameEn) {
            message += $scope._language == 'en-us' ? '<br/>Please Enter Name' : '<br/>कृपया नाम दर्ज करें।';
        }

        if (!$scope.GssComplaint.MobileNo) {
            message += $scope._language == 'en-us' ? '<br/>Please Enter Mobile Number' : '<br/>कृपया मोबाइल नंबर दर्ज करें।';
        }
        else {
            var mobilePattern = /^[0-9]{10}$/;
            if (!mobilePattern.test($scope.GssComplaint.MobileNo)) {
                message += $scope._language == 'en-us'? '<br/>Please Enter Valid 10 Digit Mobile Number' : '<br/>कृपया सही 10 अंकों का मोबाइल नंबर दर्ज करें।';
            }
        }

        if (!$scope.GssComplaint.Gender) {
            message += $scope._language == 'en-us' ? '<br/>Please Enter Gender': '<br/>कृपया लिंग दर्ज करें।';
        }

        if (!$scope.GssComplaint.Address) {
            message += $scope._language == 'en-us' ? '<br/>Please Enter Address' : '<br/>कृपया पता दर्ज करें।';
        }


        if (!$scope.GssComplaint.ComplaintTypeId ||
            $scope.GssComplaint.ComplaintTypeId == '0') {
            message += $scope._language == 'en-us'  ? '<br/>Please Select Complaint Type' : '<br/>कृपया शिकायत का प्रकार चुनें।';
        }

        if (!$scope.GssComplaint.MembershipNo) {
            message += $scope._language == 'en-us' ? '<br/>Please Enter Membership Number': '<br/>कृपया सदस्यता संख्या दर्ज करें।';
        }

        if (!$scope.GssComplaint.CCBId || $scope.GssComplaint.CCBId == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Bank': '<br/>कृपया बैंक चुनें।';
        }        

        if (!$scope.GssComplaint.BranchId || $scope.GssComplaint.BranchId == '0') {
            message += $scope._language == 'en-us'? '<br/>Please Select Branch' : '<br/>कृपया शाखा चुनें।';
        }

        if (!$scope.GssComplaint.CCBPacsId || $scope.GssComplaint.CCBPacsId == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select PACS' : '<br/>कृपया PACS चुनें।';
        }

        if ($scope.GssComplaint.ComplaintTypeId == '1') {

            if (!$scope.GssComplaint.ApplicationCreatedDate) {
                message += $scope._language == 'en-us' ? '<br/>Please Select Application Date' : '<br/>कृपया आवेदन तिथि चुनें।';
            }
        }

        if ($scope.GssComplaint.ComplaintTypeId == '2') {
            if (!$scope.GssComplaint.IsComplaintReason) {
                message += $scope._language == 'en-us' ? '<br/>Please Select Complaint Reason' : '<br/>कृपया शिकायत कारण चुनें।';
            }
           

            if ($scope.GssComplaint.IsComplaintReason == '1') {

                if (!$scope.GssComplaint.ApprovedAmount) {
                    message += $scope._language == 'en-us'? '<br/>Please Enter Approved Amount' : '<br/>कृपया स्वीकृत राशि दर्ज करें।';
                }

                if (!$scope.GssComplaint.ReceivedAmount) {
                    message += $scope._language == 'en-us' ? '<br/>Please Enter Received Amount' : '<br/>कृपया प्राप्त राशि दर्ज करें।';
                }

                if ($scope.GssComplaint.ApprovedAmount && $scope.GssComplaint.ReceivedAmount &&  parseFloat($scope.GssComplaint.ReceivedAmount) > parseFloat($scope.GssComplaint.ApprovedAmount)) {

                    message += $scope._language == 'en-us'? '<br/>Received Amount Cannot Be Greater Than Approved Amount' : '<br/>प्राप्त राशि स्वीकृत राशि से अधिक नहीं हो सकती।';
                }

                if (!$scope.GssComplaint.UploadDocument) {
                    message += $scope._language == 'en-us'? '<br/>Please Upload Document': '<br/>कृपया दस्तावेज अपलोड करें।';
                }
            }

            if ($scope.GssComplaint.IsComplaintReason == '2') {

                if (!$scope.GssComplaint.ApplicationDate) {
                    message += $scope._language == 'en-us' ? '<br/>Please Select Application Date' : '<br/>कृपया आवेदन तिथि चुनें।';
                }

                if (!$scope.GssComplaint.ApprovalDate) {
                    message += $scope._language == 'en-us'? '<br/>Please Select Approval Date' : '<br/>कृपया स्वीकृत दिनांक चुनें।';
                }
            }
        }

        if (!$scope.GssComplaint.Remark) {
            message += $scope._language == 'en-us' ? '<br/>Please Enter Reason / Remark' : '<br/>कृपया कारण दर्ज करें।';
        }
        

        if (message.length > 29) {

            ShowOkMessage( "Gram Seva Society Complaint",message,function () { }  );

            return false;
        }

        return true;
    };

    $scope.SaveGssComplaint = function () {
        if (!$scope.GssComplaintValidate()) {
            return;
        }

        $http.post(SiteUrl + "/Citizen/SaveGssComplaint", $scope.GssComplaint).
            success(function (data, status) {
                var message = '';
                if (data >0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    window.location.reload();                  
                    $(".se-pre-con").fadeOut("slow");
                }
                //else if (data == -1) {
                //    message = _language == 'en-us' ? 'You do not have access to fill the form' : 'डुप्लिकेट प्रविष्टि\n';
                //    $(".se-pre-con").fadeOut("slow");
                //    alert(message);
                //}
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    $(".se-pre-con").fadeOut("slow");
                    alert(message);
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while GSS Complaint Insert");

            });


    }


    //Bank List
    $scope.LoanBankList = function () {     
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=0").
            success(function (data, status) {
                $scope.BankMasterList = data;       
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };


    //Bank Branch
    $scope.Get_LoanBankBranch = function (branch_ID) {  
        var districtId = '0';      
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + districtId + "&LoanBankId=" + branch_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;            
                $scope.Get_LoanBranch_PACS(branch_ID);                
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //Bank Pacs
    $scope.Get_LoanBranch_PACS = function (branch_ID) {          
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GssComplaint.BranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;       
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.ShowApplicationForm = function () {
        $scope.divRegistration = true;
        $scope.GSSComplaintDiv = false;
    }
    $scope.ResetGssComplaint = function () {
        $scope.GssComplaint.Id = 0;
        $scope.GssComplaint.SSOID = '';
        $scope.GssComplaint.NameEn = '';
        $scope.GssComplaint.EmailId = '';
        $scope.GssComplaint.Gender = '';
        $scope.GssComplaint.MobileNo = '';
        $scope.GssComplaint.Address = '';
        $scope.GssComplaint.IsMember = null;
        $scope.GssComplaint.IsAppRejected = null;
        $scope.GssComplaint.IsGetLoan = null;
        $scope.GssComplaint.IsApplyForLoan = null;
        $scope.GssComplaint.txtRemark = '';
        $scope.GssComplaint.txtGetloan = '';
        $scope.GssComplaint.RejectedDate = null;
        $scope.GssComplaint.AadharNo = '';
        $scope.GssComplaint.GssSocietyName = '';
        $scope.GssComplaint.CoopBankName = '';
        

    }
    $scope.ResetGssComplaint();
    $scope.GetGssComplaintList = function () {
        $(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/Auditor/GetAuditEmpanelledAllDetails"JSON.stringify({ Details: jsonParameters})).
        $http.post(SiteUrl + "/Citizen/GetGssComplaintDetails").
            success(function (data, status) {
                $scope.GetGssComplaint = JSON.parse(data.DataList);
                $(".se-pre-con").fadeOut("slow");
                console.log($scope.GetGssComplaint);
            }).
            error(function (data, status) {
                $scope.GetGssComplaint = [];
                $(".se-pre-con").fadeOut("slow");
                alert('Error while Society Election Detail data.');
               
            });
       
    }


    //SelectComplaintFile
    $scope.SelectComplaintFile = function (files) {

        if (files && files.length > 0) {

            if (!$scope.GssComplaint) {
                $scope.GssComplaint = {};
            }

            if (!$scope.GssComplaint.UploadDocument) {
                $scope.GssComplaint.UploadDocument = {};
            }

            $scope.GssComplaint.UploadDocument = files[0].name;

            $scope.UploadComplainDocument(files);
        }
    };

    //UploadComplainDocument
    $scope.UploadComplainDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("UploadDocument", file);

        $http.post(SiteUrl + "/Citizen/UploadComplainDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.GssComplaint.UploadDocument = r.fileName;
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

    var msj = "";


    $scope.rdoMemberchange = function () {
        $scope.GssComplaint.IsAppRejected = null;
        $scope.GssComplaint.IsGetLoan = null;
        $scope.GssComplaint.IsApplyForLoan = null;
        $scope.GssComplaint.txtRemark = '';
        $scope.GssComplaint.txtGetloan = '';
        //$scope.GssComplaint.RejectedDate = null;
        $('#RejectedDate').val('');
        $scope.GssComplaint.GssSocietyName = '';
        


    }
    $scope.rdoAppRejectedchange = function () {
        $scope.GssComplaint.IsGetLoan = null;
        $scope.GssComplaint.IsApplyForLoan = null;
        $scope.GssComplaint.txtRemark = '';
        $scope.GssComplaint.txtGetloan = '';
        //$scope.GssComplaint.RejectedDate = null;
        $('#RejectedDate').val('') ;
    }
    $scope.rdoGetLoanchange = function () {
        $scope.GssComplaint.IsApplyForLoan = null;
        $scope.GssComplaint.txtRemark = '';
        $scope.GssComplaint.txtGetloan = '';
       // $scope.GssComplaint.RejectedDate = null;
        $('#RejectedDate').val('') ;
    }

    $scope.rdoApplyForLoanchange = function () {
        $scope.GssComplaint.txtRemark = '';
        $scope.GssComplaint.txtGetloan = '';
       // $scope.GssComplaint.RejectedDate = null;
        $('#RejectedDate').val('');
    }
    //End
    $scope.OnPageLoad = function () {
        $scope.SSOUserDetails();
        $scope.ResetGssComplaint();
        $scope.GetGssComplaintList();
        $scope.GSSComplaintDiv = false;
        $scope.divRegistration = true;
        $scope.LoanBankList();

    };
    $scope.OnPageLoad();
});
