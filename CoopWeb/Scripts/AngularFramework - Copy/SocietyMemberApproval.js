var MasterMod = angular.module('SocietyMemApprMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller('SocietyMemApprCnt', function ($scope, $http) {
    
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divGrid = true;
    $scope.UserRegistration = [];
    $scope.UserRegistration.ActionRemark = '';
    $scope.UserRegistration.ActionStatus = '';
    $scope.UserRegistration.RequestId = 0;    



    // START : Code for Language label setting
    $scope.getTranslation = function () { 
        $scope._language = _language;
        if (_language === 'en-us') {
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

        $scope.GetSocietyMemberData();

    };
    // END : Code for Language label setting

    $scope.GetSocietyMemberData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyMemberData = [];
        $http.get(SiteUrl + "/SocietyMemberRegistration/GetSocietyMemberRequestData" ).
            success(function (data, status) {               
                $scope.SocietyMemberData = data;
            }).
            error(function (data, status) {
                $scope.SocietyMemberData = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };


    $scope.ViewMemberApplication = function (ReqIdEnc) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/SocietyMemberRegistration/GetOrganizationUserRegistrationDetails?ApplicationIdEnc=" + ReqIdEnc).
            success(function (data, status) {

               
                $scope.divViewShow = true;
                $scope.divGrid = false;
                $scope.BackBtnLast = true;
                if (data.SocietyMemberRegistration[0].ApplicationTypeId == 2) {
                    //$scope.divViewDairyFormShow = true;
                    //$scope.divViewShow = false;
                    $scope.MemberRegistration = data.SocietyMemberRegistration[0];
                }
                else {

                    $scope.MemberRegistration = data.SocietyMemberRegistration[0];
                    $scope.LandDetailsGSS = data.SocietyMemberLandDetail;
                    $scope.SownCropDetailGSS = data.SocietyMemberSownCropDetail;
                    $scope.CommercialLoanDetail = data.SocietyMemberOtherBankLoanDetail;
                    //$scope.divViewDairyFormShow = false;
                    //$scope.divViewShow = true;
                    console.log($scope.MemberRegistration);
                }


                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;


                //if (data[0].ResponseString != null && data[0].ResponseString != "") {
                //    var temp = [];
                //    temp.push(JSON.parse(data[0].ResponseString));
                //    if (temp.length > 0) {
                //        if (data[0].KioskID == 'SSO' ? (temp[0].RESPONSEMESSAGE.indexOf('Success') > 0) : (temp[0].MSG.indexOf('Success') > 0)) {
                //            $scope.IsPaymentDone = true;

                //            $scope.TRANSACTIONID = temp[0].TRANSACTIONID;
                //            $scope.ReceiptNo = temp[0].RECEIPTNO;
                //            $scope.BANK_NAME = temp[0].BANK_NAME;
                //            $scope.BANK_ACC_NO = temp[0].BANK_ACC_NO;
                //            if (data[0].KioskID == 'SSO') {
                //                $scope.IsCityzen = true;
                //                $scope.IsKiosk = false;
                //                $scope.Fee = temp[0].PAIDAMOUNT;
                //                $scope.RESPONSEMESSAGE = temp[0].RESPONSEMESSAGE;
                //            }
                //            else {
                //                $scope.RESPONSEMESSAGE = temp[0].MSG;
                //                $scope.Fee = temp[0].TRANSAMT;
                //                $scope.IsCityzen = false;
                //                $scope.IsKiosk = true;
                //            }

                //        }
                //    }
                //}

                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
            });
        $(".se-pre-con").fadeOut("slow");














        //alert(ReqIdEnc);
       // window.location = '../Citizen/MemberApplicationView?ApplicationIdEnc=' + ReqIdEnc;
        //$scope.MemberDetailsList = $scope.SocietyMemberData.filter(X => X.RequestIdEnc == ReqIdEnc)[0];
        $scope.divViewShow = true;
    };
    // START : Get All All List
    $scope.GetMemberDetailsList = function (IdEnc) {
        $http.get(SiteUrl + "/SocietyMemberRegistration/GetOrganizationUserRegistrationDetails?ApplicationIdEnc=" + IdEnc).
            success(function (data, status) {
                if (data[0].ResponseString != null && data[0].ResponseString != "") {
                    var temp = [];
                    console.log(data[0].ResponseString);
                    temp.push(JSON.parse(data[0].ResponseString));
                    if (temp.length > 0) {
                        if (data[0].KioskID == 'SSO' ? (temp[0].RESPONSEMESSAGE.indexOf('Success') > 0) : (temp[0].MSG.indexOf('Success') > 0)) {
                            $scope.IsPaymentDone = true;

                            $scope.TRANSACTIONID = temp[0].TRANSACTIONID;
                            $scope.ReceiptNo = temp[0].RECEIPTNO;
                            $scope.BANK_NAME = temp[0].BANK_NAME;
                            $scope.BANK_ACC_NO = temp[0].BANK_ACC_NO;
                            if (data[0].KioskID == 'SSO') {
                                $scope.IsCityzen = true;
                                $scope.IsKiosk = false;
                                $scope.Fee = temp[0].PAIDAMOUNT;
                                $scope.RESPONSEMESSAGE = temp[0].RESPONSEMESSAGE;
                            }
                            else {
                                $scope.RESPONSEMESSAGE = temp[0].MSG;
                                $scope.Fee = temp[0].TRANSAMT;
                                $scope.IsCityzen = false;
                                $scope.IsKiosk = true;
                            }

                        }
                    }
                }

                $scope.MemberDetailsList = data[0];
                $scope.divViewShow = true;
                $scope.divRegistration = false;
                $scope.divGrid = false;
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get All All List

    // START : Get All All List
    $scope.ClearUserApprovalForm = function () {
        $scope.UserRegistration.ActionRemark = '';
        $scope.UserRegistration.ActionStatus = '';
    };
    // START : Get All All List
    $scope.SetBack = function () {
        $scope.UserRegistration.ActionRemark = '';
        $scope.UserRegistration.ActionStatus = '';
        $scope.UserRegistration.RequestId = 0;
        $scope.divViewShow = false;
    };
    // START : Save User
    $scope.SaveUser = function () {

        var Msg = '';
        $scope.UserRegistration.RequestId = $('#hfRequestId').val();

        if ($scope.UserRegistration.ActionStatus === undefined || $scope.UserRegistration.ActionStatus =='') {
            Msg += (_language === 'en-us' ? 'Please select Acton \n' : 'कृपया कार्रवाई चुनें\n');
        }

        if ($scope.UserRegistration.ActionRemark === undefined || $scope.UserRegistration.ActionRemark =='') {
            Msg += (_language === 'en-us' ? 'Please enter Remark?\n' : 'कृपया टिप्पणी दें\n');
        }

        if (Msg.length > 1) {
            alert(Msg);
            return;
        }

        $http.post(SiteUrl + "/SocietyMemberRegistration/SaveSocietyMemberRegiApproval", { RequestId: $scope.UserRegistration.RequestId, ActionStatus: $scope.UserRegistration.ActionStatus, ActionRemark:$scope.UserRegistration.ActionRemark }).
            success(function (data, status) {
                // $scope.MemberDetailsList = data[0];
                alert("Data saved successfully.");
                window.location.reload();
            }).
            error(function (data, status) {
                alert('Error occurred.');
            });


    };
  
    //// START : On Page Load
    $scope.getTranslation();


    //// END : On Page Load

});
