var AppFeedBacks = angular.module("DynamicPageContentAction", ['angularUtils.directives.dirPagination']);
AppFeedBacks.controller("DynamicPageContentActionController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FeedBacks = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.SaveDynamicPage = function () {
        $.ajax({
            url: '../WebSahakar/SaveDynamicPage',
            data: JSON.stringify({
                "_Model": {
                    "PageId": $scope.PageContent.PageId
                    , "PageDescEn": $('#editor').val()
                    , "PageDescHi": $('#editor1').val()
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });

    };
    $scope.pageChangeHandler = function (num) {
    };

    


    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.PageContent = Item;
        $scope.PageContent.PageId = Item.PageId.toString();
        $('#DDLPageId').val(Item.PageId.toString());
        $('#editor').parent().children().next().next().contents().find('body').html(Item.PageDescEn);
        $('#editor1').parent().children().next().next().contents().find('body').html(Item.PageDescHi);

    };

    $scope.ClearDynamicPage = function () {
        $('#editor').parent().children().next().next().contents().find('body').html("");
        $('#editor1').parent().children().next().next().contents().find('body').html("");

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusPages", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllPageContentList();

            }).
            error(function (data, status) {
            });
    };

    $scope.GetPageList = function () {
        $http.get(SiteUrl + "/WebSahakar/PageContentList").
            success(function (data, status) {
                $scope.PageListData = data;
            }).
            error(function (data1, status) {
                $scope.PageListData = [];
            });
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllPageContentList();
    };

    $scope.GetAllPageContentList = function () {
        $http.get(SiteUrl + "/WebSahakar/PageContentList").
            success(function (data, status) {
                $scope.DynamicPageContentData = data;
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
                $scope.DynamicPageContentData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllPageContentList();
    $scope.GetPageList();
    // $scope.TargetList();

});

var AppNews = angular.module("NewsContentAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("NewsContentActionController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.NewsContent.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.NewsContent.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadNewsContent", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while case registration insert");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };

    $scope.NewsContent =
        {

       NewsNameEN : '',
       NewsNameHI : '',
       StartDate : '',
       StartTime : '',
       EndDate : '',
       EndTime : '',
       IsNew : false
      




        }

    $("#HtmlContentEn").summernote("code", "");
    $("#HtmlContentHi").summernote("code", "");

    $scope.SaveNewsContent = function () {
      
        if ($scope.ValidateNewsContent()) {
            $(".se-pre-con").fadeIn("slow");
            var documents = [];
            $scope.LOVFilesList.forEach(function (item, index) {
                if (item.IsUploadFileName) {
                    documents.push({
                        FileName: item.FileName,
                    });
                }
            });
            $.ajax({
                url: '../WebSahakar/SaveNewsContent',
                data: JSON.stringify({
                    "_Model": {
                        "NewsID": $scope.NewsContent.NewsID
                        , "NewsNameEN": $scope.NewsContent.NewsNameEN
                        , "NewsNameHI": $scope.NewsContent.NewsNameHI
                        , "NewsDescriptionEN": $("#HtmlContentEn").val() != "" || $("#HtmlContentEn").val() != null ? $("#HtmlContentEn").val() : ""
                        , "NewsDescriptionHI": $("#HtmlContentHi").val() != "" || $("#HtmlContentHi").val() != null ? $("#HtmlContentHi").val() : ""
                        , "StartDate": $('#txtStartDate').val().split("/").reverse().join("-")
                        , "StartTime": $('#txtStartTime').val()
                        , "EndDate": $('#txtEndDate').val().split("/").reverse().join("-")
                        , "EndTime": $('#txtEndTime').val()
                        , "IsNew": $scope.NewsContent.IsNew
                        , "FileList": documents
                        , "NewsDate": $('#txtNewsDate').val().split("/").reverse().join("-")
                    }
                }),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                beforeSend: function () {
                },
                success: function (data) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data > 0) {
                        $(".se-pre-con").fadeOut("slow");
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                        alert(message);
                        location.reload();
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        alert(message);
                        $(".se-pre-con").fadeOut("slow");
                        location.reload();
                    }
                },
                error: function (e) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                },

                complete: function () {
                }
            });
        }
    };




    // Start : Validations  
    $scope.ValidateNewsContent= function () {

        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.NewsContent.NewsNameEN == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter News Name in English.' : '\n <br />कृपया अंग्रेजी में समाचार का नाम दर्ज करें।';
            $("#oicnameEn").addClass("notValid");
        }
        else {
            $("#oicnameEn").removeClass("notValid");
        }

        if ($scope.NewsContent.NewsNameHI == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the News Name in Hindi.' : '\n <br />कृपया हिंदी में समाचार का नाम दर्ज करें।';
            $("#oicnameHi").addClass("notValid");
        }
        else {
            $("#oicnameHi").removeClass("notValid");
        }

        if ($('#txtStartDate').val()  == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Start Date.' : '\n <br />कृपया प्रारंभ की तिथि दर्ज करें।';
            $("#txtStartDate").addClass("notValid");
        }
        else {
            $("#txtStartDate").removeClass("notValid");
        }
        if ($('#txtStartTime').val() == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Start Time.' : '\n <br />कृपया प्रारंभ का समय दर्ज करें।';
            $("#txtStartTime").addClass("notValid");
        }
        else {
            $("#txtStartTime").removeClass("notValid");
        }




        if ($('#txtEndDate').val() == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the End Date.' : '\n <br />कृपया अंतिम तिथि दर्ज करें।';
            $("#txtEndDate").addClass("notValid");
        }
        else {
            $("#txtEndTime").removeClass("notValid");
        }
        if ($('#txtEndTime').val()  == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the End Time.' : '\n <br />कृपया अंतिम समय दर्ज करें।';
            $("#txtEndTime").addClass("notValid");
        }
        else {
            $("#txtEndTime").removeClass("notValid");
        }



        //if (item.IsUploadFileName && item.FileType == '') {
        //    message += $scope._language == 'en-us' ? '<br />Please Enter selected type docuemnt.' : '<br />कृपया चयनित दस्तावेज़ का प्रकार दर्ज करें।';
        //    $("#lvoFileType_" + indx).addClass("notValid");
        //    isValid = false;
        //}
        if ($scope.LOVFilesList[0].FileName == '' && $scope.LOVFiles.IsUploadFileName == false) {
            message += _language == 'en-us' ? '\n <br />Please upload selected docuemnt.' : '\n <br />कृपया चयनित दस्तावेज़ अपलोड करें।';
            isValid = false;
        }

        if ($('#HtmlContentEn').val() == '') {
            
            message += _language == 'en-us' ? '\n <br />Please Enter the Content in English.' : '\n <br />कृपया अंग्रेजी में विवरण दर्ज करें।';
            isValid = false;
            $("#HtmlContentEn").addClass("notValid");
        }
        else {
            $("#HtmlContentEn").removeClass("notValid");
        }


        if ($('#HtmlContentHi').val() == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Content in Hindi.' : '\n <br />कृपया हिंदी में विवरण दर्ज करें।।';
            $("#HtmlContentHi").addClass("notValid");
        }
        else {
            $("#HtmlContentHi").removeClass("notValid");
        }

        //if ($scope.FAQModel.IsActive == false) {
        //    message += _language == 'en-us' ? '\n <br />FAQ active? Please select.' : '\n <br /> FAQ सक्रिय है? कृपया चयन करें।';
        //    $("#IsActive").addClass("notValid");
        //}


        if (message.length > 29) {
            ShowModelAlert("News Content", message);
            return false;
        }
        return true;
    }

    //End Validation




    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.NewsContent = Item;
        $scope.NewsContent.NewsNameEN = Item.NewsNameEN;
        $scope.NewsContent.NewsNameHI = Item.NewsNameHI;
        $scope.NewsContent.StartDate = Item.StartDate;
        $scope.NewsContent.StartTime = Item.StartTime;
        $scope.NewsContent.EndDate = Item.EndDate;
        $scope.NewsContent.EndTime = Item.EndTime;
        $scope.NewsContent.NewsDate = Item.NewsDate;
        $scope.FileName = Item.FileName;
        $scope.NewsContent.IsNew = Item.IsNew == 1 ? true : false;
        $("#HtmlContentEn").summernote("code", Item.NewsDescriptionEN);
        $("#HtmlContentHi").summernote("code", Item.NewsDescriptionHI);


        var fileName = Item.FileName.split(",");
        

        $scope.LOVFilesList = [];
       // $scope.LOVFilesList = fileName;
        fileName.forEach(function (itm, index) {
            $scope.LOVFiles.FileName = fileName[index];
            var name = $scope.LOVFiles.FileName;
            $scope.LOVFilesList.push({
                FileName: name,
                IsUploadFileName: true,
                Is_Active: true,
                Is_Delete: true});
         
           // $scope.LOVFiles.IsUploadFileName = fileName[fileName] != '' && fileName[fileName] != null
            
                      
        });



        //$("#HtmlContentEn").val(Item.NewsDescriptionEN);
        //$('#HtmlContentHi').val(Item.NewsDescriptionHI);

    };

    $scope.ClearNewsContentPage = function () {
        $scope.NewsContent.NewsNameEN = '';
        $scope.NewsContent.NewsNameHI = '';
        $scope.NewsContent.StartDate = '';
        $scope.NewsContent.StartTime = '';
        $scope.NewsContent.EndDate = ''; 2939

        $scope.NewsContent.EndTime = '';
        $scope.NewsContent.NewsDate = '';
        $('#editor').parent().children().next().next().contents().find('body').html("");
        $('#editor1').parent().children().next().next().contents().find('body').html("");

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusNews", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllNewsContentList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;

        $scope.NewsContent.NewsNameEN = '';
        $scope.NewsContent.NewsNameHI= '';
        $scope.NewsContent.StartDate = '';
        $scope.NewsContent.StartTime = '';
        $scope.NewsContent.EndDate   = '';
        $scope.NewsContent.EndTime = '';
        $scope.NewsContent.IsNew = false;
        $("#HtmlContentEn").summernote("code", "");
        $("#HtmlContentHi").summernote("code", "");


     
        $scope.LOVFilesList = [{
            FileName: '',
            IsUploadFileName: false
        }]
      //  $scope.LOVFiles();

    }

    

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
        $scope.LOVFilesList = [];
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        //  $('#txtNewsDate').prop('autocomplete', 'off').timepicker();
        // $(".se-pre-con").fadeIn("slow");
        $('#txtStartTime').timepicker({
            showOn: "both",
            buttonImage: "../../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtEndTime').timepicker({
            showOn: "both",
            buttonImage: "../../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtStartDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: today
        });
        $('#txtEndDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: today
        });
        $('#txtNewsDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: today
        });
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
        $scope.GetAllNewsContentList();
    };

    $scope.GetAllNewsContentList = function () {
        $http.get(SiteUrl + "/WebSahakar/NewsContentList").
            success(function (data, status) {
                $scope.NewsContentData = data;
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
                $scope.DynamicPageContentData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllNewsContentList();

$scope.LOVFilesList = [];



$scope.UploadFileLVO = function (id, type) {
    try {
        // FormData
        if (window.FormData !== undefined) {
            var fileUpload = $("#" + id).get(0);
            var files = fileUpload.files;
            //var blob = b64toBlob(realData, contentType, 512);
            var formData = new FormData();
            formData.append("pldbfile", files[0]);
            $(".blockUI").show();
            $scope.xhrLVO(SiteUrl + "/WebSahakar/UploadLoanFile", formData, type);
        } else {
            alert("FormData is not supported.");
        }
    }
    catch (ex) { alert(ex) }
}

$scope.deleteFime = function (index) {


    if ($scope.LOVFilesList[index].Id > 0) {
        $scope.LOVFilesList[index].Is_Delete = true
    } else {
        var list = $scope.LOVFilesList;
        $scope.LOVFilesList = [];
        list.forEach(function (item, indx) {
            if (indx != index) {
                $scope.LOVFilesList.push(item);
            }
        });
    }
}



    $scope.ChangeLVOFile = function (type, index) {
        $scope.LOVFilesList[index].IsUploadFileName = false;
        $scope.LOVFilesList[index].FileName = "";
        $("#LovFileName_" + index).val("");
    }

    $scope.xhrLVO = function (url, data, index) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {

                        $scope.LOVFilesList[index].FileName = request.responseText;

                        if ($scope.LOVFilesList.filter(function (x) { return x.IsUploadFileName === false }).length <= 0) {
                            $scope.LOVFilesList.push({
                                //IsUploadFileName: "",
                                Id: 0,
                                FileName: "",
                                IsUploadFileName: false,
                                Is_Active: true,
                                Is_Delete: false
                            });
                        }
                        $scope.$apply();
                    }
                }
            };

            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }


    $scope.xhr = function (url, data, type, index) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {


                        //if (type == 8) {
                        //    $scope.PropertyDetailsList[index].PropertyDocument = request.responseText;
                        //}
                        //else if (type == 9) {
                        //    $scope.GuarantorPropertyDetailsList[index].GPropertyDocument = request.responseText;

                        //}

                        //if ($scope.LoanSSILoanApplicationForm) {
                        //    if (type == 1) {
                        //        $scope.LoanDocument.RicoFile = request.responseText;
                        //    } else if (type == 2) {
                        //        $scope.LoanDocument.WorkingCapitalFile = request.responseText;
                        //    } else if (type == 3) {
                        //        $scope.LoanDocument.ApplicantDealAfterApprovalFile = request.responseText;
                        //    } else if (type == 4) {
                        //        $scope.LoanDocument.BusinessAndRegiCertificateFile = request.responseText;
                        //    } else if (type == 5) {
                        //        $scope.LoanDocument.ProposedBuildingAndWorkStation = request.responseText;
                        //    } else if (type == 6) {
                        //        $scope.LoanDocument.Quotation = request.responseText;
                        //    } else if (type == 7) {
                        //        $scope.LoanDocument.IncomeTaxClearanceCertificate = request.responseText;
                        //    }
                        //}
                        //else if ($scope.ShowVehicleLoanApplication) {
                        //    if (type == 1) {
                        //        $scope.LoanMembers.forEach(function (item, i) { item.PartnershipCertifiedCopy = request.responseText });
                        //    } else if (type == 2) {
                        //        $scope.LoanMembers.forEach(function (item, i) { item.TransportCompanyRegCertification = request.responseText });
                        //    } else if (type == 3) {
                        //        $scope.LoanMembers.forEach(function (item, i) { item.DrivinglicenseCopy = request.responseText });
                        //    } else if (type == 4) {
                        //        $scope.LoanMembers.forEach(function (item, i) { item.VehicleRantProposalCopy = request.responseText });
                        //    }
                        //}


                        $scope.$apply();
                    }
                }
            };


            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {

                //if (($(".addprofilePhoto").length >= uploadIndex + 1) || ($(".addFamilyPhoto").length >= uploadIndex + 1)) {
                //    $(".blockUI").hide();
                //    LoadView('_PhotosVideos', 'tb-photosvideos');
                //}
                //++uploadIndex;
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    //$scope.LoanDocument = {
    //    RicoFile: "",
    //    IsUploadRicoFile: false,
    //    IsUploadWorkingCapitalFile: false,
    //    WorkingCapitalFile: "",
    //    IsUploadApplicantDealAfterApprovalFile: false,
    //    ApplicantDealAfterApprovalFile: "",
    //    IsUploadBusinessAndRegiCertificateFile: false,
    //    BusinessAndRegiCertificateFile: "",
    //    IsUploadProposedBuildingAndWorkStation: false,
    //    ProposedBuildingAndWorkStation: "",
    //    IsUploadQuotation: false,
    //    Quotation: "",
    //    IsUploadIncomeTaxClearanceCertificate: false,
    //    IncomeTaxClearanceCertificate: "",

    //}

    $scope.LOVFiles = {
        Id: 0, FileType: "", FileName: "", IsUploadFileName: false, Is_Active: true, Is_Delete: false
    };
    $scope.LOVFilesList = [];
    $scope.LOVFilesList1 = [];
    $scope.LoanStatusList = [];
    $scope.LOVFilesList.push($scope.LOVFiles);

});




AppNews.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    var id = elem.attr("id") == undefined ? "" : elem.attr("id");
                    //  alert(id.split('_')[0]);
                    if (id.split('_')[0] == "LovFileName") {
                        if (elem[0].files.length > 0) {
                            const fsize = elem[0].files[0].size;
                            const file = Math.round((fsize / 1024));

                            var indx = id.split()[1];
                            if (file >= 400) {
                                ShowOkMessage("Warning", scope._language == 'en-us' ? "File too Big, please select a file less than 400kb" : "फ़ाइल बहुत बड़ी है, कृपया 400kb से कम फ़ाइल का चयन करें", function () { });
                                elem[0].value = "";
                            } else {
                                //if (scope.FileType == 1) {
                                //alert(id.split('_')[1]);
                                scope.LOVFilesList[id.split('_')[1]].IsUploadFileName = true;
                                scope.LOVFilesList[id.split('_')[1]].FileName = "";
                                //}
                                scope.$apply();
                            }
                        }
                    } else {
                        if (elem[0].files.length > 0) {
                            const fsize = elem[0].files[0].size;
                            const file = Math.round((fsize / 1024));
                            // The size of the file. 
                            //alert(1);
                            if (file >= 4096) {
                                ShowOkMessage("Warning", scope_language == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                                elem[0].value = "";
                            } else {


                                //if (scope.FileType == 8) {
                                //    scope.PropertyDetailsList[scope.FileTypeIndex].IsUploadPropertyDocument = true;
                                //    scope.PropertyDetailsList[scope.FileTypeIndex].PropertyDocument = "";
                                //}
                                //else if (scope.FileType == 9) {
                                //    scope.GuarantorPropertyDetailsList[scope.FileTypeIndex].IsUploadGPropertyDocument = true;
                                //    scope.GuarantorPropertyDetailsList[scope.FileTypeIndex].GPropertyDocument = "";
                                //}

                                //if (scope.LoanSSILoanApplicationForm) {
                                //    if (scope.FileType == 1) {
                                //        scope.LoanDocument.RicoFile = "";
                                //        scope.LoanDocument.IsUploadRicoFile = true;
                                //    } else if (scope.FileType == 2) {
                                //        scope.LoanDocument.IsUploadWorkingCapitalFile = true;
                                //        scope.LoanDocument.WorkingCapitalFile = "";
                                //    } else if (scope.FileType == 3) {
                                //        scope.LoanDocument.IsUploadApplicantDealAfterApprovalFile = true;
                                //        scope.LoanDocument.ApplicantDealAfterApprovalFile = "";
                                //    } else if (scope.FileType == 4) {
                                //        scope.LoanDocument.IsUploadBusinessAndRegiCertificateFile = true;
                                //        scope.LoanDocument.BusinessAndRegiCertificateFile = "";
                                //    } else if (scope.FileType == 5) {
                                //        scope.LoanDocument.IsUploadProposedBuildingAndWorkStation = true;
                                //        scope.LoanDocument.ProposedBuildingAndWorkStation = "";
                                //    } else if (scope.FileType == 6) {
                                //        scope.LoanDocument.IsUploadQuotation = true;
                                //        scope.LoanDocument.Quotation = "";
                                //    }
                                //    else if (scope.FileType == 7) {
                                //        scope.LoanDocument.IsUploadIncomeTaxClearanceCertificate = true;
                                //        scope.LoanDocument.IncomeTaxClearanceCertificate = "";
                                //    }
                                //} else if (scope.ShowVehicleLoanApplication) {

                                //    if (scope.FileType == 1) {
                                //        scope.LoanMembers.forEach(function (item, i) { item.PartnershipCertifiedCopy = ""; item.IsPartnershipCertifiedCopy = true });
                                //    } else if (scope.FileType == 2) {
                                //        scope.LoanMembers.forEach(function (item, i) { item.TransportCompanyRegCertification = ""; item.IsTransportCompanyRegCertification = true });
                                //    } else if (scope.FileType == 3) {
                                //        scope.LoanMembers.forEach(function (item, i) { item.DrivinglicenseCopy = ""; item.IsDrivinglicenseCopy = true });
                                //    } else if (scope.FileType == 4) {
                                //        scope.LoanMembers.forEach(function (item, i) { item.VehicleRantProposalCopy = ""; item.IsVehicleRantProposalCopy = true });
                                //    }
                                //}
                                scope.$apply();
                            }
                        }
                    }
                });
            };
        }
    };
});




var AppNews = angular.module("CooperativeInstitutionAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("CooperativeInstitutionController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.CoopertativeInstitution.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.CoopertativeInstitution.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadCoopertativeInstitution", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while case registration insert");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveCoopertativeInstitution = function () {
        $.ajax({
            url: '../WebSahakar/SaveCoopertativeInstitution',
            data: JSON.stringify({
                "_Model": {
                    "CoopInstitutionId": $scope.CoopertativeInstitution.CoopInstitutionId
                    , "TitleNameEn": $scope.CoopertativeInstitution.TitleNameEn
                    , "TitleNameHi": $scope.CoopertativeInstitution.TitleNameHi
                    , "Link": $scope.CoopertativeInstitution.Link
                    , "ImagePath": $scope.FileName
                    , "TitleDescriptionEn": $('#editor').val()
                    , "TitleDescriptionHi": $('#editor1').val()
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.CoopertativeInstitution = Item;
        $scope.CoopertativeInstitution.TitleNameEn = Item.TitleNameEn;
        $scope.CoopertativeInstitution.TitleNameHi = Item.TitleNameHi;
        $scope.CoopertativeInstitution.Link = Item.Link;
        $scope.FileName = Item.ImagePath;
        $('#editor').parent().children().next().next().contents().find('body').html(Item.TitleDescriptionEn);
        $('#editor1').parent().children().next().next().contents().find('body').html(Item.TitleDescriptionHi);

    };

    $scope.ClearCoopertativeInstitution = function () {
        $scope.CoopertativeInstitution.TitleNameEn = '';
        $scope.CoopertativeInstitution.TitleNameHi = '';
        $scope.CoopertativeInstitution.Link = '';
        $('#editor').parent().children().next().next().contents().find('body').html("");
        $('#editor1').parent().children().next().next().contents().find('body').html("");

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusCoopertativeInstitution", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllCoopertativeInstitutionList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllCoopertativeInstitutionList();
    };

    $scope.GetAllCoopertativeInstitutionList = function () {
        $http.get(SiteUrl + "/WebSahakar/CoopertativeInstitutionList").
            success(function (data, status) {
                $scope.CoopertativeInstitutionData = data;
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
                $scope.CoopertativeInstitutionData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllCoopertativeInstitutionList();

});

var AppNews = angular.module("CooperativeAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("CooperativeController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Coopertative.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Coopertative.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadCoopertative", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveCoopertative = function () {
        $.ajax({
            url: '../WebSahakar/SaveCoopertative',
            data: JSON.stringify({
                "_Model": {
                    "CooperativeID": $scope.Coopertative.CooperativeID
                    , "TitleEN": $scope.Coopertative.TitleEN
                    , "TitleHI": $scope.Coopertative.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Coopertative = Item;
        $scope.Coopertative.TitleEN = Item.TitleEN;
        $scope.Coopertative.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearCoopertative = function () {
        $scope.Coopertative.TitleNameEn = '';
        $scope.Coopertative.TitleNameHi = '';
        $scope.Coopertative.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusCoopertative", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllCoopertativeList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllCoopertativeList();
    };

    $scope.GetAllCoopertativeList = function () {
        $http.get(SiteUrl + "/WebSahakar/CooperativeList").
            success(function (data, status) {
                $scope.CoopertativeData = data;
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
                $scope.CoopertativeData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllCoopertativeList();

});


var AppNews = angular.module("DownloadsAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("DownloadsController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Downloads.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Downloads.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadDownloads", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveDownloads = function () {
        $.ajax({
            url: '../WebSahakar/SaveDownloads',
            data: JSON.stringify({
                "_Model": {
                    "DownloadID": $scope.Downloads.DownloadID
                    , "TitleEN": $scope.Downloads.TitleEN
                    , "TitleHI": $scope.Downloads.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Downloads = Item;
        $scope.Downloads.TitleEN = Item.TitleEN;
        $scope.Downloads.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearDownloads = function () {
        $scope.Downloads.TitleNameEn = '';
        $scope.Downloads.TitleNameHi = '';
        $scope.Downloads.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusDownloads", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllDownloadsList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllDownloadsList();
    };

    $scope.GetAllDownloadsList = function () {
        $http.get(SiteUrl + "/WebSahakar/DownloadsList").
            success(function (data, status) {
                $scope.DownloadsData = data;
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
                $scope.DownloadsData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllDownloadsList();

});


var AppNews = angular.module("OrderCircularsAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("OrderCircularsController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.DivPageDetailContent = false;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.OrderCirculars.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.OrderCirculars.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadOrderCirculars", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveOrderCirculars = function () {
        $.ajax({
            url: '../WebSahakar/SaveOrderCirculars',
            data: JSON.stringify({
                "_Model": {
                    "OrderCircularsID": $scope.OrderCirculars.OrderCircularsID
                    , "OrderCatID": $scope.catid
                    , "TitleEN": $scope.OrderCirculars.TitleEN
                    , "TitleHI": $scope.OrderCirculars.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    $scope.DivAddContent = false;
                    $scope.DivPageContent = false;
                    $scope.DivPageDetailContent = true;
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $scope.DivAddContent = false;
                    $scope.DivPageContent = false;
                    $scope.DivPageDetailContent = true;
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.DivPageDetailContent = false;
        $scope.OrderCirculars = Item;
        $scope.OrderCirculars.TitleEN = Item.TitleEN;
        $scope.OrderCirculars.OrderCatID = Item.OrderCatID;
        $scope.OrderCirculars.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearOrderCirculars = function () {
        $scope.OrderCirculars.TitleNameEn = '';
        $scope.OrderCirculars.TitleNameHi = '';
        $scope.OrderCirculars.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusOrderCirculars", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                //$scope.GetAllOrderCircularsList();
                $scope.GetAllOrderCircularsList($scope.catid);

            }).
            error(function (data, status) {
            });
    };

    $scope.DeleteSSO = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteSSOOrderCirculars", { Id: PageId, Action: ActionId }).
            success(function (data, status) {
                var message = 'Data Deleted Successfully';
                alert(message);
                location.reload();
            }).
            error(function (data, status) {
            });
    };

    $scope.SaveOrderCircularsMap = function (elements) {

        var formData = new FormData();
        formData.append("lstOrderSSOMAPScope", JSON.stringify(elements));
        $http.post(SiteUrl + "/WebSahakar/OrderCircularSSOMAP", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var k = data;
                $scope.elements = [];
                var message = 'Data Save Successfully';
                alert(message);
                location.reload();
                if (data.Status) {
                }
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });

        //}
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.DivPageDetailContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageDetailContent = true;
    }
    $scope.BackToCatPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageDetailContent = false;
        $scope.DivPageContent = true;
    }

    $scope.MapSSO = function (SID) {
        $scope.catid = SID;
        $scope.elements = [{ CatID: SID, SSOId: '' }];

        $scope.DivAddContent = true;
        $scope.DivPageContent = true;
    }


    var counter = 0;


    $scope.newItem = function () {
        counter++;

        $scope.elements.push({
            "CatID": $scope.catid,
            "SSOId": ''
        });
    }



    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllOrderCircularCatList();
    };

    $scope.GETSSODETAIL = function (SID) {
        $scope.catid = SID;
        $(".se-pre-con").fadeIn("slow");
        $scope.OrderCircularsData = null;
        $http.get(SiteUrl + "/WebSahakar/OrderCircularsSSOList/?catid=" + SID).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.OrderCircularsSSOListData = data;
                $scope.colsort = "";
                $scope.ShowSSOMAP = true;
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
                $scope.ShowSSOMAP = false;
                $scope.OrderCircularsSSOListData = [];
                alert("No Record Found");
            });

    }

    $scope.GetAllOrderCircularsList = function (SID) {
        $scope.catid = SID;
        $(".se-pre-con").fadeIn("slow");
        $scope.OrderCircularsData = null;
        $http.get(SiteUrl + "/WebSahakar/OrderCircularsList/?catid=" + SID).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.OrderCircularsData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.DivAddContent = false;
                $scope.DivPageContent = false;
                $scope.DivPageDetailContent = true;
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
                $scope.OrderCircularsData = [];
            });
    }



    $scope.GetAllOrderCircularCatList = function () {
        $http.get(SiteUrl + "/WebSahakar/OrderCircularsCatList").
            success(function (data, status) {
                $scope.OrderCircularsCatListData = data;
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
                $scope.OrderCircularsCatListData = [];
            });
    }

    $scope.OnPageLoad();
    //  $scope.GetAllOrderCircularsList();
    $scope.GetAllOrderCircularCatList();

});

var AppNews = angular.module("FertilizerAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("FertilizerController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Fertilizer.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Fertilizer.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadFertilizer", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1)
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveFertilizer = function () {
        $.ajax({
            url: '../WebSahakar/SaveFertilizer',
            data: JSON.stringify({
                "_Model": {
                    "FertilizerID": $scope.Fertilizer.FertilizerID
                    , "TitleEN": $scope.Fertilizer.TitleEN
                    , "TitleHI": $scope.Fertilizer.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Fertilizer = Item;
        $scope.Fertilizer.TitleEN = Item.TitleEN;
        $scope.Fertilizer.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearFertilizer = function () {
        $scope.Fertilizer.TitleNameEn = '';
        $scope.Fertilizer.TitleNameHi = '';
        $scope.Fertilizer.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusFertilizer", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllFertilizerList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllFertilizerList();
    };

    $scope.GetAllFertilizerList = function () {
        $http.get(SiteUrl + "/WebSahakar/FertilizerList").
            success(function (data, status) {
                $scope.FertilizerData = data;
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
                $scope.FertilizerData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllFertilizerList();

});

var AppNews = angular.module("MessageAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("MessageController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Message.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Message.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadMessage", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1)
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveMessage = function () {
        $.ajax({
            url: '../WebSahakar/SaveMessage',
            data: JSON.stringify({
                "_Model": {
                    "MessageID": $scope.Message.MessageID
                    , "TitleEN": $scope.Message.TitleEN
                    , "TitleHI": $scope.Message.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Message = Item;
        $scope.Message.TitleEN = Item.TitleEN;
        $scope.Message.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearMessage = function () {
        $scope.Message.TitleNameEn = '';
        $scope.Message.TitleNameHi = '';
        $scope.Message.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusMessage", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllMessageList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllMessageList();
    };

    $scope.GetAllMessageList = function () {
        $http.get(SiteUrl + "/WebSahakar/MessageList").
            success(function (data, status) {
                $scope.MessageData = data;
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
                $scope.FertilizerData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllMessageList();

});

var MasterMod = angular.module('TelePhoneModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("TelePhoneCont", function ($scope, $http) {


    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.DivTelePhoneContent = false;
    $scope.DivAddContent = true;

    $scope.GetTelePhoneCatList = function () {
        $http.get(SiteUrl + "/WebSahakar/GetTelePhoneCatList").
            success(function (data, status) {
                $scope.TelePhoneCatList = data;
            }).
            error(function (data1, status) {
                $scope.TelePhoneCatList = [];
            });
    }

    $scope.TraningFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.TelePhone.Name == undefined || $scope.TelePhone.Name == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Name.' : '\n <br /> कृपया नाम दर्ज करें |';
        }
        if ($scope.TelePhone.Designation == undefined || $scope.TelePhone.Designation == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Designation' : '\n <br /> कृपया पद का नाम दर्ज करें |';
        }

        if ($scope.TelePhone.Mobile == undefined || $scope.TelePhone.Mobile == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
        }

        if ($scope.TelePhone.OFFICE == undefined || $scope.TelePhone.OFFICE == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Office Phone Number ' : '\n <br /> कृपया कार्यालय फ़ोन नंबर दर्ज करें |';
        }

        if ($scope.TelePhone.EMAILID == undefined || $scope.TelePhone.EMAILID == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Email Id' : '\n <br /> कृपया ईमेल-आईडी दर्ज करें |';
        }

        if ($scope.TelePhone.STDCODE == undefined || $scope.TelePhone.STDCODE == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter STD Code' : '\n <br /> कृपया एसटीडी कोड दर्ज करें |';
        }

        



        if (message.length > 23) {

            ShowModelAlert("Training", message);
            return false;
        }
        return true;

    }
    $scope.SaveTelePhone = function () {
        if ($scope.TraningFormValidation()) {
            var formData = new FormData();
            formData.append("TeleDirectoryID", $scope.TelePhone.TeleDirectoryID);
            formData.append("TeleCatID", $scope.TelePhone.TeleCatID);
            formData.append("Name", $scope.TelePhone.Name);
            formData.append("Designation", $scope.TelePhone.Designation);
            formData.append("Mobile", $scope.TelePhone.Mobile);
            formData.append("STDCODE", $scope.TelePhone.STDCODE);
            formData.append("OFFICE", $scope.TelePhone.OFFICE);
            formData.append("RESI", $scope.TelePhone.RESI);
            formData.append("FAX", $scope.TelePhone.FAX);
            formData.append("EMAILID", $scope.TelePhone.EMAILID);
            $http.post(SiteUrl + "/WebSahakar/SaveTelePhoneDirectory", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if ($scope.TelePhone.TeleDirectoryID != data) {
                        var k = data;
                        var message = 'Data Save Successfully';
                        alert(message);
                        $scope.DivPageContent = true;
                        var TeleCatID = $scope.TelePhone.TeleCatID;
                        $scope.GetTelePhoneList(TeleCatID);
                        $scope.ClearTelePhone();

                    }
                    else {
                        var message = 'Data Update Successfully';
                        alert(message);
                    }
                    if (data.Status) {
                    }
                }).
                error(function (data, status) {
                    alert("Error while TelePhone Directory insert");
                    $scope.ClearCaseRegister();
                    $scope.SubformShow = false;
                });
        }
    };

    $scope.SetStatus = function (PageId, ActionId, CatId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;


        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusTelePhone", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetTelePhoneList(CatId);

            }).
            error(function (data, status) {
            });
    };

    $scope.ClearTelePhone = function () {
        $scope.TelePhone.Name = '';
        $scope.TelePhone.Designation = '';
        $scope.TelePhone.Mobile = '';
        $scope.TelePhone.STDCODE = '';
        $scope.TelePhone.OFFICE = '';
        $scope.TelePhone.RESI = '';
        $scope.TelePhone.FAX = '';
        $scope.TelePhone.EMAILID = '';
    };
   

    $scope.GetTelePhoneList = function (SID) {
        $http.get(SiteUrl + "/WebSahakar/GetTelePhoneList/?CatId=" + SID).
            success(function (data, status) {
                $scope.TelePhoneList = data;
                $scope.DivTelePhoneContent = true;
            }).
            error(function (data1, status) {
                $scope.TelePhoneList = [];
            });
    }

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.TelePhone = Item;
        $scope.TelePhone.TeleCatID = Item.TeleCatID.toString();
        $scope.TelePhone.Name = Item.Name.toString();
        $scope.TelePhone.Designation = Item.Designation.toString();
        $scope.TelePhone.Mobile = Item.Mobile.toString();
        $scope.TelePhone.STDCODE = Item.STDCODE.toString();
        $scope.TelePhone.OFFICE = Item.OFFICE.toString();
        $scope.TelePhone.RESI = Item.RESI.toString();
        $scope.TelePhone.FAX = Item.FAX.toString();
        $scope.TelePhone.EMAILID = Item.EMAILID.toString();


    };

    $scope.getTranslation = function () {
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
    };


    $scope.OnPageLoad = function () {
        $scope.GetTelePhoneCatList();
        //$(".se-pre-con").fadeIn("slow");
        //$(".se-pre-con").fadeOut("slow");

    }



    $scope.getTranslation($scope);
    $scope.OnPageLoad();

});

var AppNews = angular.module("TransfersAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("TransfersController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Transfers.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Transfers.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadTransfers", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearTransfers();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveTransfers = function () {
        $.ajax({
            url: '../WebSahakar/SaveTransfers',
            data: JSON.stringify({
                "_Model": {
                    "TransferID": $scope.Transfers.TransferID
                    , "TitleEN": $scope.Transfers.TitleEN
                    , "TitleHI": $scope.Transfers.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Transfers = Item;
        $scope.Transfers.TitleEN = Item.TitleEN;
        $scope.Transfers.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearTransfers = function () {
        $scope.Transfers.TitleNameEn = '';
        $scope.Transfers.TitleNameHi = '';
        $scope.Transfers.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusTransfers", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllTransfersList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllTransfersList();
    };

    $scope.GetAllTransfersList = function () {
        $http.get(SiteUrl + "/WebSahakar/TransfersList").
            success(function (data, status) {
                $scope.TransfersData = data;
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
                $scope.TransfersData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllTransfersList();

});

var AppNews = angular.module("RighttoinformationAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("RighttoinformationController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Righttoinformation.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Righttoinformation.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadRighttoinformation", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearRighttoinformation();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveRighttoinformation = function () {
        $.ajax({
            url: '../WebSahakar/SaveRighttoinformation',
            data: JSON.stringify({
                "_Model": {
                    "RTIID": $scope.Righttoinformation.RTIID
                    , "TitleEN": $scope.Righttoinformation.TitleEN
                    , "TitleHI": $scope.Righttoinformation.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Righttoinformation = Item;
        $scope.Righttoinformation.TitleEN = Item.TitleEN;
        $scope.Righttoinformation.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearRighttoinformation = function () {
        $scope.Righttoinformation.TitleNameEn = '';
        $scope.Righttoinformation.TitleNameHi = '';
        $scope.Righttoinformation.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusRighttoinformation", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllRighttoinformationList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllRighttoinformationList();
    };

    $scope.GetAllRighttoinformationList = function () {
        $http.get(SiteUrl + "/WebSahakar/RighttoinformationList").
            success(function (data, status) {
                $scope.RighttoinformationData = data;
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
                $scope.RighttoinformationData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllRighttoinformationList();

});


var AppNews = angular.module("PrizesAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("PrizesController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Prizes.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Prizes.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadPrizes", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearCaseRegister();
                $scope.SubformShow = false;
            });
    };


    $scope.SavePrizes = function () {
        $.ajax({
            url: '../WebSahakar/SavePrizes',
            data: JSON.stringify({
                "_Model": {
                    "PrizesID": $scope.Prizes.PrizesID
                    , "TitleEN": $scope.Prizes.TitleEN
                    , "TitleHI": $scope.Prizes.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Prizes = Item;
        $scope.Prizes.TitleEN = Item.TitleEN;
        $scope.Prizes.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearPrizes = function () {
        $scope.Prizes.TitleNameEn = '';
        $scope.Prizes.TitleNameHi = '';
        $scope.Prizes.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusPrizes", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllPrizesList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllPrizesList();
    };

    $scope.GetAllPrizesList = function () {
        $http.get(SiteUrl + "/WebSahakar/PrizesList").
            success(function (data, status) {
                $scope.PrizesData = data;
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
                $scope.PrizesData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllPrizesList();

});


var AppNews = angular.module("LawandrulesAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("LawandrulesController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Lawandrules.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Lawandrules.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadLawandrules", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearLawandrules();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveLawandrules = function () {
        $.ajax({
            url: '../WebSahakar/SaveLawandrules',
            data: JSON.stringify({
                "_Model": {
                    "LawandrulesID": $scope.Lawandrules.LawandrulesID
                    , "TitleEN": $scope.Lawandrules.TitleEN
                    , "TitleHI": $scope.Lawandrules.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Lawandrules = Item;
        $scope.Lawandrules.TitleEN = Item.TitleEN;
        $scope.Lawandrules.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearLawandrules = function () {
        $scope.Lawandrules.TitleNameEn = '';
        $scope.Lawandrules.TitleNameHi = '';
        $scope.Lawandrules.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusLawandrules", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllLawandrulesList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllLawandrulesList();
    };

    $scope.GetAllLawandrulesList = function () {
        $http.get(SiteUrl + "/WebSahakar/LawandrulesList").
            success(function (data, status) {
                $scope.LawandrulesData = data;
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
                $scope.LawandrulesData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllLawandrulesList();
    //for New ruls model 2001
    $scope.SaveLawandrulesModel = function () {
        $.ajax({
            url: '../WebSahakar/SaveLawandrulesModel',
            data: JSON.stringify({
                "_Model": {
                    "LawandrulesmodelID": $scope.Lawandrules.LawandrulesmodelID
                    , "TitleEN": $scope.Lawandrules.TitleEN
                    , "TitleHI": $scope.Lawandrules.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : '???? ????? ??? ??\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : '??? ?????? ??? ??\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : '??? ?????? ??? ??\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.updatemodel = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Lawandrules = Item;
        $scope.Lawandrules.TitleEN = Item.TitleEN;
        $scope.Lawandrules.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearLawandrulesmodel = function () {
        $scope.Lawandrules.TitleNameEn = '';
        $scope.Lawandrules.TitleNameHi = '';
        $scope.Lawandrules.FileName = '';

    };
    $scope.SetStatusModel = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusLawandrules", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllLawandrulesList();

            }).
            error(function (data, status) {
            });
    };

    //End rules 2001
});

var AppNews = angular.module("PhotoGallaryAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("PhotoGallaryController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.PhotoGallary.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.PhotoGallary.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadPhotoGallary", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearLawandrules();
                $scope.SubformShow = false;
            });
    };


    $scope.SavePhotoGallary = function () {
        $.ajax({
            url: '../WebSahakar/SavePhotoGallary',
            data: JSON.stringify({
                "_Model": {
                    "PhotoGallaryID": $scope.PhotoGallary.PhotoGallaryID
                    , "TitleEN": $scope.PhotoGallary.TitleEN
                    , "TitleHI": $scope.PhotoGallary.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.PhotoGallary = Item;
        $scope.PhotoGallary.TitleEN = Item.TitleEN;
        $scope.PhotoGallary.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearPhotoGallary = function () {
        $scope.PhotoGallary.TitleNameEn = '';
        $scope.PhotoGallary.TitleNameHi = '';
        $scope.PhotoGallary.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusPhotoGallary", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllPhotoGallaryList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllPhotoGallaryList();
    };

    $scope.GetAllPhotoGallaryList = function () {
        $http.get(SiteUrl + "/WebSahakar/PhotoGallaryList").
            success(function (data, status) {
                $scope.PhotoGallaryData = data;
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
                $scope.PhotoGallaryData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllPhotoGallaryList();

});


var AppNews = angular.module("BulletienAction", ['angularUtils.directives.dirPagination']);
AppNews.controller("BulletienController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Bulletien.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Bulletien.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadBulletien", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearLawandrules();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveBulletien = function () {
        $.ajax({
            url: '../WebSahakar/SaveBulletien',
            data: JSON.stringify({
                "_Model": {
                    "BulletienID": $scope.Bulletien.BulletienID
                    , "TitleEN": $scope.Bulletien.TitleEN
                    , "TitleHI": $scope.Bulletien.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Bulletien = Item;
        $scope.Bulletien.TitleEN = Item.TitleEN;
        $scope.Bulletien.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearBulletien = function () {
        $scope.Bulletien.TitleNameEn = '';
        $scope.Bulletien.TitleNameHi = '';
        $scope.Bulletien.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusBulletien", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllBulletienList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllBulletienList();
    };

    $scope.GetAllBulletienList = function () {
        $http.get(SiteUrl + "/WebSahakar/BulletienList").
            success(function (data, status) {
                $scope.BulletienData = data;
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
                $scope.BulletienData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllBulletienList();

});

var AppPMSMS = angular.module("PMKISSAN", ['angularUtils.directives.dirPagination']);
AppPMSMS.controller("PMKISSANController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Bulletien.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.GetDataForSMS = function () {
        $http.get(SiteUrl + "/WebSahakar/PMKissanSMSList").
            success(function (data, status) {
                $scope.PMKisanSmsDataG = data;               
            }).
            error(function (data1, status) {
                $scope.PMKisanSmsDataG = [];
            });
    };

    $scope.SendSms = function () {
        $http.get(SiteUrl + "/WebSahakar/SendSMSPS").
            success(function (data, status) {
                if (data > 0)
                    alert('Message Send Successfully')
            }).
            error(function (data1, status) {
                alert('Some Error Occurred')
            });
    };
    
    $scope.FormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.PMKisanSms.TotalApplication == '' || $scope.PMKisanSms.TotalApplication == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Application' : '\n <br /> कृपया कुल आवेदन दर्ज करें |';
        }
        if ($scope.PMKisanSms.TotalUploded == '' || $scope.PMKisanSms.TotalUploded == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Uploaded' : '\n <br /> कृपया कुल अपलोड दर्ज करें |';
        }
        if (($scope.PMKisanSms.DisbursementI == undefined || $scope.SocietyRegistration.RegistrationNo == '')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Disbursement 1st Installment' : '\n <br /> Please Enter Disbursement 1st Installment';
        }       
        if (($scope.PMKisanSms.DisbursementII == undefined || $scope.PMKisanSms.DisbursementII == '')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Disbursement 2nd Installment.' : '\n <br />Please Enter Disbursement 2nd Installment |';
        }
        if (($scope.PMKisanSms.DisbursementIII == undefined || $scope.PMKisanSms.DisbursementIII == '')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Disbursement 3rd Installment.' : '\n <br />Please Enter Disbursement 3rd Installment |';
        }
        if (($scope.PMKisanSms.DisbursementIV == undefined || $scope.PMKisanSms.DisbursementIV == '')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Disbursement 4th Installment.' : '\n <br />Please Enter Disbursement 4th Installment |';
        }

        if (message.length > 29) {
            ShowModelAlert("PM Kisan Status", message);
            return false;
        }
        if ($scope.SubmitStatus == 0) {

            return false;
        }
        return true;

    };


    $scope.SavePMKissanSMS = function () {
        //if ($scope.FormValidation()) {
            $.ajax({
                url: '../WebSahakar/SavePMKissanDataForSMS',
                data: JSON.stringify({
                    "_Model": {
                        "TotalApplication": $scope.PMKisanSms.TotalApplication
                        , "TotalUploded": $scope.PMKisanSms.TotalUploded
                        , "DisbursementI": $scope.PMKisanSms.DisbursementI
                        , "DisbursementII": $scope.PMKisanSms.DisbursementII
                        , "DisbursementIII": $scope.PMKisanSms.DisbursementIII
                        , "DisbursementIV": $scope.PMKisanSms.DisbursementIV
                    }
                }),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                beforeSend: function () {
                },
                success: function (data) {
                    if (data > 0) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                        alert(message);
                        $scope.GetDataForSMS();
                        $scope.ClearData();
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        alert(message);
                        $scope.GetDataForSMS();

                    }
                },
                error: function (e) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                },

                complete: function () {
                }
            });
        //}
    };


    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (ItemNo) { 
        $scope.PMKisanSms = ItemNo;
      
    };

    $scope.ClearData = function () {
        $scope.PMKisanSms.TotalApplication = '';
        $scope.PMKisanSms.TotalUploded = '';
        $scope.PMKisanSms.DisbursementI = '';
        $scope.PMKisanSms.DisbursementII = '';
        $scope.PMKisanSms.DisbursementIII = '';
        $scope.PMKisanSms.DisbursementIV = '';
    };






    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetDataForSMS();
    };

  

    $scope.OnPageLoad();




});


var AppLawandrulesModal = angular.module("LawandrulesModal", ['angularUtils.directives.dirPagination']);
AppLawandrulesModal.controller("LawandrulesModalController", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivAddContent = false;
    $scope.DivPageContent = true;
    $scope.newdiv = "";
    $scope.FileName = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    var UseIdList = [];

    $scope.UploadNewsFile = function (file) {
        $scope.Lawandrules.FileName = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Lawandrules.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadLawandrules", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearLawandrules();
                $scope.SubformShow = false;
            });
    };


    $scope.SaveLawandrules = function () {
        $.ajax({
            url: '../WebSahakar/SaveLawandrules',
            data: JSON.stringify({
                "_Model": {
                    "LawandrulesID": $scope.Lawandrules.LawandrulesID
                    , "TitleEN": $scope.Lawandrules.TitleEN
                    , "TitleHI": $scope.Lawandrules.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.update = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Lawandrules = Item;
        $scope.Lawandrules.TitleEN = Item.TitleEN;
        $scope.Lawandrules.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearLawandrules = function () {
        $scope.Lawandrules.TitleNameEn = '';
        $scope.Lawandrules.TitleNameHi = '';
        $scope.Lawandrules.FileName = '';

    };
    $scope.SetStatus = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusLawandrules", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllLawandrulesList();

            }).
            error(function (data, status) {
            });
    };

    $scope.AddNewsContent = function () {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
    }

    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetAllLawandrulesList();
    };

    $scope.GetAllLawandrulesList = function () {
        $http.get(SiteUrl + "/WebSahakar/GetAllLawandrulesModalList").
            success(function (data, status) {
                $scope.LawandrulesData = data;
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
                $scope.LawandrulesData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllLawandrulesList();
    //for New ruls model 2001
    $scope.SaveLawandrulesModel = function () {
        $.ajax({
            url: '../WebSahakar/SaveLawandrulesModel',
            data: JSON.stringify({
                "_Model": {
                    "LawandrulesmodelID": $scope.Lawandrules.LawandrulesmodelID
                    , "TitleEN": $scope.Lawandrules.TitleEN
                    , "TitleHI": $scope.Lawandrules.TitleHI
                    , "FileName": $scope.FileName
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if (data > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : '???? ????? ??? ??\n';
                    alert(message);
                    location.reload();
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : '??? ?????? ??? ??\n';
                    alert(message);
                    location.reload();
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : '??? ?????? ??? ??\n';
                alert(message);
            },

            complete: function () {
            }
        });
    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.updatemodel = function (Item) {
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.Lawandrules = Item;
        $scope.Lawandrules.TitleEN = Item.TitleEN;
        $scope.Lawandrules.TitleHI = Item.TitleHI;
        $scope.FileName = Item.FileName;
    };

    $scope.ClearLawandrulesmodel = function () {
        $scope.Lawandrules.TitleNameEn = '';
        $scope.Lawandrules.TitleNameHi = '';
        $scope.Lawandrules.FileName = '';

    };
    $scope.SetStatusModel = function (PageId, ActionId) {
        //  alert(ActionId);
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusLawandrules", { Id: PageId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllLawandrulesList();

            }).
            error(function (data, status) {
            });
    };


    $scope.UploadNewsFilemodel = function (file) {
        $scope.Lawandrules.FileName = file[0];
        $scope.UploadNewsContentmodel();
    }

    $scope.UploadNewsContentmodel = function () {
        var formData = new FormData();
        formData.append("FileName", $scope.Lawandrules.FileName);
        $http.post(SiteUrl + "/WebSahakar/UploadLawandrulesModel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                $scope.FileName = data;
                $scope.FileName = $scope.FileName.substring(1, $scope.FileName.length - 1);
                //  var message = 'Data Save Successfully';
                // alert(message);
            }).
            error(function (data, status) {
                alert("Error while uploading");
                $scope.ClearLawandrules();
                $scope.SubformShow = false;
            });
    };

    //End rules 2001
});