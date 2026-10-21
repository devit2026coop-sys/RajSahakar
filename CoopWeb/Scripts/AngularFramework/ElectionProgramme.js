var ElectionProgrammeModule = angular.module('ElectionProgrammeMod', ['angularUtils.directives.dirPagination']);
ElectionProgrammeModule.controller("ElectionProgrammeCnt", function ($scope, $http) {


 
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12; 
    $scope.OTP = '';
    $scope.TXN = '';
    // on page load
    $scope.OnPageLoad = function () {
        $(".blockUI").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
            //$(".se-pre-con").fadeOut("slow");
        }
    };

    // START :Get List of Societies
    $scope.GetSocietyListForElectionOfficer = function () {
        $(".blockUI").fadeIn("slow");
        $http.post("../Election/GetElectionPrograameForSociety/").
            success(function (data, status) {
                
                $scope.ElectionOfficerData = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                alert('Error while fetching Society List For Election Officer.');
                $(".blockUI").fadeOut("slow");
            });
    };
    // END :Get List of Societies

    $scope.OfficerComment = function (item) {
        
        $scope.ElectionMasterid = item.ElectionMasterId;
      
        $scope.SocietyId = item.SocietyId;
        $scope.GetCommentListbySociety($scope.SocietyId );
        $("#Commentpopup").modal("show");
    }


    $scope.GetCommentListbySociety = function (SocietyId) {
        $(".blockUI").fadeIn("slow");

        debugger
        $http.post("/Election/GetCommentListbySociety", { "SocietyId": SocietyId }).
            success(function (data, status) {
                //alert(JSON.stringify(data));
            
                $scope.CommentListbySociety = data;
              
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CommentListbySociety = [];
                alert('Error while Society Election Detail data.');
            });

      
    }
    $scope.Flag = 1;

    $scope.CommentEdit = function (item) {
        $scope.OfficerCommentId = item.OfficerCommentId;
        $scope.OfficerRemark = item.OfficerComment;
        $scope.Flag = 2;
    }

    $scope.SaveCommnet = function () {
        

        var message = "";

        if ($scope.OfficerRemark == "" || $scope.OfficerRemark == null || $scope.OfficerRemark == undefined) {
            message += $scope._language == 'en-us' ? 'Please Enter  Comment।' : '<br />कृपया टिप्पणी दर्ज करें।';
            if (message.length > 1) {
                ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
                return false;
            }
            return true;
        }
        else {
            var formData = new FormData();
            if ($scope.Flag == 2) {
                formData.append("OfficerCommentId", $scope.OfficerCommentId);
                formData.append("OfficerComment", $scope.OfficerRemark);
            }
            else {
                formData.append("ElectionMasterid", $scope.ElectionMasterid);
                formData.append("SocietyId", $scope.SocietyId);
                formData.append("OfficerComment", $scope.OfficerRemark);
            }
            $http.post(SiteUrl + "/Election/InsertOfficerCommentBySociety", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if (data == "-1") {
                        $scope.GetCommentListbySociety($scope.SocietyId)
                        $(".blockUI").fadeOut("slow");
                        $scope.OfficerRemark = "";
                    }
                    else {
                        $(".blockUI").fadeOut("slow");
                        ShowOkMessage("", "Some technical error occur", function () { });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                });
        }
       
    }





    $scope.OnPageLoad();
    $scope.GetSocietyListForElectionOfficer();
});


var ElectionProgrammePublishModule = angular.module('ElectionProgrammePublishMod', ['angularUtils.directives.dirPagination']);
ElectionProgrammePublishModule.controller("ElectionProgrammePublishCnt", function ($scope, $http, $location) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    // on page load
    $scope.OnPageLoad = function () {
        $(".blockUI").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
            //$(".se-pre-con").fadeOut("slow");
        }
    };

    // START :Get List of Societies
    $scope.GetPublishedData = function () {
        var id = $location.absUrl().split('?')[1].replace('Id=', '');
       // alert(id);
        $(".blockUI").fadeIn("slow");
        $http.post("/Election/GetElectionProgrammeData", { "Id": id}).
            success(function (data, status) {
                $scope.PublishData = data;
                console.log(data);
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                alert('Error while fetching Society List For Election Officer.');
                $(".blockUI").fadeOut("slow");
            });
    };
    // END :Get List of Societies

    // START :Get List of Societies




    $scope.SendOTP = function () {

        var id = $location.absUrl().split('?')[1].replace('Id=', '')
        $(".blockUI").fadeIn("slow");
            $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                if (SessionStatus.Status == false) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                } else {
                    $.ajax({
                        url: '../Election/GenerateAadhaarOTPForEsing',
                        //data: JSON.stringify({
                        //    "objElectionSocietyData": ArrDates,
                        //    "ElectionMasterIdEnc": $('#btnautoclick').val(),
                        //}),
                        type: 'GET',
                        contentType: 'application/json',
                        // dataType: 'json',
                        beforeSend: function () {
                        },
                        success: function (data) {

                            if (data.indexOf('NO#') >= 0) {
                                ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                            } else {
                                if (data != "LOGOUT") {
                                    // alert(data);
                                    $('#lbltrn').html(data);
                                    $('#ShowDateDiv').hide();
                                    $('#ShowOTPDiv').show();
                                    $("html, body").animate({ scrollTop: 0 }, "slow");
                                    ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });
                                }
                                else {
                                    window.location = 'https://sso.rajasthan.gov.in/signin';
                                }
                            }
                            // window.location = '../Election/ElectionSocietyList';
                            $(".se-pre-con").fadeOut("slow");
                        },
                        error: function (e) {
                            message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                            alert(message);
                            $(".se-pre-con").fadeOut("slow");
                        },
                        complete: function () {
                            $(".se-pre-con").fadeOut("slow");
                        }
                    })
                }
            });        
    };



    $scope.Publish = function () {
        var id = $location.absUrl().split('?')[1].replace('Id=', '')
        if ($scope.OTP != '') {
        $(".blockUI").fadeIn("slow");
        $http.post("../Election/GenPDFHinForElectionInfo", { "OTP": $scope.OTP, "TXN": $('#lbltrn').html(),"Id": id }).
            success(function (data, status) {
                
                if (data == "PDF Not Generate") {
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be Publishing. Please try after some time." : "आदेश प्रकाशित नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                } else
                    if (data == "Success") {
                        ShowOkMessage("", 'Society election order successfully published', function () {
                            window.location = '../Election/ElectionProgrammeForSociety';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "0") {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    }else if (data == "LOGOUT") {
                        ShowOkMessage("", 'Session out.\n', function () {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        });
                    } else if (data == "Invalid OTP!") {
                        ShowOkMessage("", data, function () {

                        });
                    } else  {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    }                
                    
                // window.location = '../Election/ElectionSocietyList';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                ShowOkMessage("", 'Error while Publishing Election Programme Order PDF.', function () { });
                $(".blockUI").fadeOut("slow");
            });
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
    };
   

  
    // END :Get List of Societies
    $scope.VerifyOTP = function () {
        
        var id = $location.absUrl().split('?')[1].replace('Id=', '')
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/VerifyAadhaarOTPForElectionPublish", { "OTP": $scope.OTP, "TXN": $('#lbltrn').html(), "ElectionMasterIdEnc": $('#btnautoclick').val(), Id: id}).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data != "Success") {
                        ShowOkMessage("", data, function () { });
                    }
                    else if (data == "Success") {
                        ShowOkMessage("", 'Society election order successfully published', function () {
                            window.location = '../Election/ElectionProgrammeForSociety';
                        });
                     
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
        $scope.showLoader = false;
    };

    $scope.OnPageLoad();
    $scope.GetPublishedData();
});



