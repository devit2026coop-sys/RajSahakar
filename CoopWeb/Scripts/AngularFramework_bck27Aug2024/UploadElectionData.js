var AppClasses = angular.module("CandidateListMod", ['angularUtils.directives.dirPagination']);

var ImageDataList = [];
var PDFDataList = [];
function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
    this.FileName = '';
}
AppClasses.controller("CandidateListCnt", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 10,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }


    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
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
                //  _currentPageID = 1;
                callback();
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
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
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
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'SubmissionDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetElectionSocietyListData();
    }
    /*------------End Paging and Filter-----------------*/


    $scope.SocietyIdEnc = '';
    $scope.ElctionVoterListData = [];
    $scope.ShowSocietyList = true;
    $scope.ElectionMasterId = 0;
    $scope.ShowVoterList = false;
    $scope.EMemberDetailsData = [];
    $scope.SearchText = "";
    $scope.IsSubmit = false;
    $scope.IsValid = false;
    $scope.IsValidMessages = "";
    //$scope.IsPublish = false;
    $scope.TotalMembers = 0;
    $scope.LastSubmistionDate = "";

    $scope.AddMoreMemberDetailsWithoutValidation = function () {
        //  $(".clsShowHide").hide();
        var rows = $('#myTable2 tr').length + 1;
        for (; typeof $("#lblElAdministrativeFile" + rows + "").html() !== "undefined";) {
            rows = rows + 1;
        }
        $('#myTable2').append('<tr>' +
            '<td>' + rows + '</td>' +
            ' <td>   <input type="checkbox" id="ckbIsProccess' + rows + '"> </td>' +
            '<td><input type= "text" disabled="disabled" id="ElDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyCandidate.ElDirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input type="text" disabled="disabled" id="ElContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyCandidate.ElContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
            '<td><input type="text" disabled="disabled" id="ElAADHAARNo' + rows + '" name="AdhareNo" class="form-control" placeholder="आधार संख्या" ng-model="SocietyCandidate.ElAADHAARNo1' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
            '<td><input disabled="disabled" id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyCandidate.ElDOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
            '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" disabled="disabled" name="ElGender' + rows + '" value="F" ng-model="SocietyCandidate.ElGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" disabled="disabled" value="M" name="ElGender' + rows + '" ng-model="SocietyCandidate.ElGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
            '<td><div style="width:100%"><input type="hidden" id="MemberId' + rows + '" name="MemberId" ng-model="SocietyCandidate.MemberId' + rows + '" ><select disabled="disabled" id="ElCategoryId' + rows + '" ng- model="SocietyCandidate.ElCategoryId' + rows + '" class="form-control" ng- init="SocietyCandidate.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
            // '<td><div style="width:100%"><select id="ElPostId' + rows + '" ng-model="SocietyCandidate.ElPostId' + rows + '" class="form-control" ng-init="SocietyCandidate.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
            '<td><label style="display:none" id="lblElAdministrativeFile' + rows + '"></label><a id="LinkPhoto' + rows + '" target="_blank">Photo</a><input disabled="disabled" type="file" id="ElFile' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyCandidate.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
            '<td><input  disabled="disabled"  type= "text" id="ElFHName' + rows + '" name= "FHName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति नाम" ng- model="SocietyRegistration.ElFHName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input  disabled="disabled"  type= "text" id="ElAddress' + rows + '" name= "Address" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पता" ng- model="SocietyRegistration.ElAddress' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><label style="display:none" id="lblElCandidatePDFForm' + rows + '"></label><a id="LinkDoc' + rows + '" target="_blank">PDF</a><input type="file" id="CandidatePDFForm' + rows + '" filetype="PDF" name="File" class="form-control" ng-model="SocietyRegistration.CandidatePDFForm' + rows + '" onchange="angular.element(this).scope().ElUploadPDF(this.files,' + rows + ')"></td >' +
            //  '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
            '</tr>');
        $scope.AddCalender();

        //FIll Category DropDown
        // alert(JSON.stringify($scope.CategoryList));
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language) {
                //  alert(item.CODEID);
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                // alert(item.CODEID);
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        //  alert(ddHTML);
        $("#ElCategoryId" + rows).html(ddHTML);
        //  alert($("#ElCategoryId" + rows).html());
        //END
        //FIll Post DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#ElPostId" + rows).html(ddHTML);
        //END

        $('input[type="file"]').unbind('change').bind('change', function () {
            if ($(this)[0].files.length > 0) {
                if (checkExtension($(this).val(), $(this).attr('filetype'))) {
                    var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
                    if ($(this)[0].files[0].name.match(format)) {
                        alert(_language ? "Invalid File" : "अवैध फाइल");
                        $(this).val('');
                        $(this).parent().find('.filesize').text('');
                        if ($(this).attr('filetype').toLowerCase() == "pdf")
                            PDFDataList.splice(PDFDataList.length - 1);
                        else
                            ImageDataList.splice(ImageDataList.length - 1);
                        //  alert(JSON.stringify(PDFDataList));
                        return;
                    }
                    if (!checkFileLength($(this)[0].files[0].size)) {
                        $(this).parent().find('.filesize').text(formateBitRates($(this)[0].files[0].size));
                    }
                    else {
                        alert(_language ? "File size exceeded.\n Please select files have size less then 1 MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 1 MB कम करें|");
                        $(this).val('');
                        $(this).parent().find('.filesize').text('');
                        if ($(this).attr('filetype').toLowerCase() == "pdf")
                            PDFDataList.splice(PDFDataList.length - 1);
                        else
                            ImageDataList.splice(ImageDataList.length - 1);
                        // alert(JSON.stringify(PDFDataList));
                        return;
                    }
                }
                else {
                    $(this).val('');
                    $(this).parent().find('.filesize').text('');
                    if ($(this).attr('filetype').toLowerCase() == "pdf")
                        PDFDataList.splice(PDFDataList.length - 1);
                    else
                        ImageDataList.splice(ImageDataList.length - 1);
                    // alert(JSON.stringify(PDFDataList));
                    return;
                }
            }
            else {
                alert(_language ? "Please select file." : "फ़ाइल का चयन करें|");
                $(this).parent().find('.filesize').text('');
            }
        });

    };
    $scope.SearchVoter = function () {
        if ($scope.SearchText != '') {
            // $("#myTable2").html("");
            // $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);

            //let data = $scope.EMemberDetailsData.find(ob => ob['AadharNo2'] === $scope.AddNewMember.AadharNo);
            var srno1 = 1;
            $scope.EMemberDetailsData.forEach(function (itm, idnx) {
                if ($scope.SearchText == itm.AadharNo2 && itm.Delete) {
                    itm.IsShow = true;
                    itm.SrNo = srno1++;
                }
                else {
                    itm.IsShow = false;
                }
            });


        } else {
            $scope.EMemberDetailsData.forEach(function (itm, idnx) {
                itm.IsShow = true;
            });
        }
    }
    $scope.SelectCandidate = function (th) {
        
        //alert(th.val());
    }
    $scope.CleaSearchVoter = function () {
        $scope.SearchText = "";
        //$("#myTable2").html("");
        //$scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);
        var srno2 = 1;
        $scope.EMemberDetailsData.forEach(function (itm, idnx) {
            itm.IsShow = true;
            if (itm.Delete)
                itm.SrNo = srno2++;

        });
    }

    $scope.OTP = "";
    $scope.Certtrn = "";
    $scope.CertSocietyIdEnc = "";
    $scope.CertElectionMasterId = "";
    $scope.CertIndex = "";
    $scope.CertMemberId = "";




    $scope.SendOTPForIssueCirtificate = function (SocietyIdEnc, ElectionMasterId, MemberId, index) {
        $(".se-pre-con").fadeIn("slow");
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
                    ShowOkMessage("", _language == 'en-us' ? "Certificate could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "प्रमाण पत्र जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    // window.location = '../Election/ElectionSocietyList';
                } else {
                    if (data != "LOGOUT") {
                        // alert(data);
                        //$('#lbltrn').html(data);
                        //$('#lblSocietyIdEnc').html(SocietyIdEnc);
                        //$('#lblElectionMasterId').html(ElectionMasterId);
                        //$('#CertIndex').html(index);
                        //$("#MemberId").html(MemberId);

                        $scope.Certtrn = data;
                        $scope.CertSocietyIdEnc = SocietyIdEnc;
                        $scope.CertElectionMasterId = ElectionMasterId;
                        $scope.CertIndex = index;
                        $scope.CertMemberId = MemberId;




                        ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                            $scope.OTP = "";
                            $('#myOTPModal').modal("show");
                        });
                    }
                    else if (data == "LOGOUT") {
                        ShowOkMessage("", 'Session out.\n', function () {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        });

                    }
                    // window.location = '../Election/ElectionSocietyList';
                    $(".se-pre-con").fadeOut("slow");
                }
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                ShowOkMessage("", message, function () {
                });
                $(".se-pre-con").fadeOut("slow");
            },
            complete: function () {
                $(".se-pre-con").fadeOut("slow");
            }
        });
    }

    $scope.VerifyOTP = function () {
        
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/GenerateElectedCandidatePDF", { "OTP": $scope.OTP, "TXN": $scope.Certtrn, "SocietyIdEnc": $scope.CertSocietyIdEnc, "ElectionMasterId": $scope.CertElectionMasterId, "MemberId": $scope.CertMemberId, "StepType": 7 }).
                success(function (data, status) {
                    
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        $scope.EMemberDetailsData[$scope.CertIndex].CandidateCirtificate = "Certificate_" + $scope.CertElectionMasterId + "_" + $scope.CertMemberId + ".pdf";
                        ShowOkMessage("", 'Certificate successfully Issued', function () {
                            $('#myOTPModal').modal("hide");

                            //window.location = '../Election/ElectionSocietyList';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "PDF Not Generate") {
                        ShowOkMessage("", _language == 'en-us' ? "Certificate could not be generated. Please try after some time." : "प्रमाण पत्र जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    }

                    else if (data == "LOGOUT") {
                        ShowOkMessage("", 'Session out.\n', function () {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        });
                    } else {
                        ShowOkMessage("", data, function () {

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
    $scope.EditBasicDetailsForm = function (SocietyIdEnc, ElectionMasterId) {
        //  alert(1);
        //  alert(SocietyIdEnc)
        $scope.ShowSocietyList = false;
        $scope.ElectionMasterId = ElectionMasterId;
        $scope.ShowVoterList = true;
        $scope.SocietyIdEnc = SocietyIdEnc;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetVoterMemberDetailsBySocietyId", { "SocietyId": $scope.SocietyIdEnc, "IsCEO": false, "ElectionMasterId": ElectionMasterId, StepType: 7 }).
            success(function (data, status) {
                // var MemberDetails = [];
                var EMemberDetails = [];
                EMemberDetails = data;


                $scope.EMemberDetailsData = data;//.filter(x => x.PostId>0);
                $scope.AddCalender();
                // $scope.IsSubmit = false;
                $scope.IsValid = true;
                $scope.EMemberDetailsData.forEach(function (item, indx) {
                    
                    //item.IsCandidate = item.IsCandidate.toLowerCase() == 'true' ? true : false;
                    //item.IsSubmit = item.IsSubmit.toLowerCase() == 'true' ? true : false;
                    //item.IsValid = item.IsValid.toLowerCase() == 'true' ? true : false;
                    //item.IsSubmitCandidate = item.IsSubmitCandidate.toLowerCase() == 'true' ? true : false;
                    //item.IsWinner = item.IsWinner.toLowerCase() == 'true' ? true : false;
                    item.CandidateCirtificate = item.CandidateCirtificate == null ? "" : item.CandidateCirtificate;
                    item.IsWinner = item.IsWinner == null ? false : item.IsWinner;
                    item.ResultDocument = item.ResultDocument == null ? "" : item.ResultDocument;
                    if (indx == 0) {
                        $scope.SocietyDirectory = item.SocietyDirectory;
                        $scope.IsSubmit = item.IsSubmit;
                        $scope.LastSubmistionDate = item.LastDate + ' ' + item.LastTime;
                        if (!item.IsValid) {
                            //$scope.IsSubmit = true;
                            $scope.IsValid = false;
                            $scope.IsValidMessages = $scope._language == 'en-us' ? 'Submisstion date has paased, so you are not allow to submit member list.' : 'सबमिशन की तारीख निकल चुकी है, इसलिए आपको सदस्य सूची जमा करने की अनुमति नहीं है। |';
                        }
                    }
                });
                //let data = $scope.EMemberDetailsData.find(ob => ob['IsSubmitCandidate'] === true);
                //$scope.IsSubmit = data.length > 0 ? true : false;

                $scope.TotalMembers = $scope.EMemberDetailsData.length;


                $(".blockUI").fadeOut("slow");

                //for (var k = 0; k < EMemberDetails.length; k++) {

                //    if (k == 0) {
                //        $scope.SocietyCandidate.ElDirectorsName1 = EMemberDetails[k].Name;
                //        $scope.SocietyDirectory = EMemberDetails[k].SocietyDirectory;
                //        $scope.SocietyCandidate.ElContactNo1 = EMemberDetails[k].ContactNo;
                //        $scope.SocietyCandidate.MemberId1 = EMemberDetails[k].MemberId;
                //        $scope.SocietyCandidate.ElAADHAARNo1 = EMemberDetails[k].AadharNo2;
                //        $('#txt_DOB').val(EMemberDetails[k].DOB);
                //        $('#MemberId').val(EMemberDetails[k].MemberId);
                //        $scope.SocietyCandidate.ElGender1 = EMemberDetails[k].Gender;
                //        $scope.SocietyCandidate.ElCategoryId1 = EMemberDetails[k].CategoryId;
                //        $scope.SocietyCandidate.Address1 = EMemberDetails[k].Address;
                //        $scope.SocietyCandidate.FHName1 = EMemberDetails[k].FHName;
                //          $scope.SocietyCandidate.ElPostId1 = EMemberDetails[k].PostId;
                //           if (EMemberDetails[k].PostId > 0)
                //          {

                //              $('#ckbIsProccess' + (k + 1)).prop('checked', true);
                //          }
                //        $('#lblElAdministrativeFile1').html(EMemberDetails[k].Photo);
                //        $('#lblCandidatePDFForm1').html(EMemberDetails[k].CandidatePDFForm);
                //        if (EMemberDetails[k].PhotoURL != '') {
                //            $('#LinkPhoto1').attr("href", EMemberDetails[k].PhotoURL);
                //        }
                //        else {
                //            $("#LinkPhoto1").css("display", "none")
                //        }
                //        if (EMemberDetails[k].CandidatePDFFormURL != '') {
                //            $('#LinkDoc1').attr("href", EMemberDetails[k].CandidatePDFFormURL);
                //        }
                //        else {
                //            $("#LinkDoc1").css("display", "none")
                //        }
                //    }
                //    else {
                //        $scope.AddMoreMemberDetailsWithoutValidation();

                //        $('#ElDirectorsName' + (k + 1)).val(EMemberDetails[k].Name);
                //        $('#ElContactNo' + (k + 1)).val(EMemberDetails[k].ContactNo);
                //        $('#ElAADHAARNo' + (k + 1)).val(EMemberDetails[k].AadharNo2);
                //        $('#MemberId' + (k + 1)).val(EMemberDetails[k].MemberId);

                //        $('#txt_DOB' + (k + 1)).val(EMemberDetails[k].DOB);
                //        $('input:radio[name=ElGender' + (k + 1) + ']')[0].checked = EMemberDetails[k].Gender == "F" ? true : false;
                //        $('input:radio[name=ElGender' + (k + 1) + ']')[1].checked = EMemberDetails[k].Gender == "M" ? true : false;

                //        $('#ElCategoryId' + (k + 1)).val(EMemberDetails[k].CategoryId);
                //        $('#ElAddress' + (k + 1)).val(EMemberDetails[k].Address);
                //        $('#ElFHName' + (k + 1)).val(EMemberDetails[k].FHName);
                //        $('#lblElCandidatePDFForm' + (k + 1)).html(EMemberDetails[k].CandidatePDFForm);
                //          $('#ElPostId' + (k + 1)).val(EMemberDetails[k].PostId);
                //          $('#lblElAdministrativeFile' + (k + 1)).html(EMemberDetails[k].Photo);
                //          if (EMemberDetails[k].PostId > 0) {
                //              $('#ckbIsProccess' + (k + 1)).prop('checked', true);
                //          }
                //          if (EMemberDetails[k].PhotoURL != '') {
                //              $('#LinkPhoto' + (k + 1)).attr("href", EMemberDetails[k].PhotoURL);
                //          }
                //          else {
                //              $('#LinkPhoto' + (k + 1)).css("display", "none")
                //          }

                //          if (EMemberDetails[k].CandidatePDFFormURL != '') {
                //              $('#LinkDoc' + (k + 1)).attr("href", EMemberDetails[k].CandidatePDFFormURL);
                //          }
                //          else {
                //              $("#LinkDoc" + (k + 1)).css("display", "none")
                //          }
                //    }

                //}
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                $(".blockUI").fadeOut("slow");
            });

    }


    $scope.validateData = function (status) {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        $("#electedSocietyDetails").find("input").removeClass("validdata");
        $("#electedSocietyDetails").find("select").removeClass("validdata");

        var SocietyName = $scope.ElectionSocietyDetails.SocietyName == undefined ? "" : $scope.ElectionSocietyDetails.SocietyName.replace(/ /g, '');
        var RegistrationNo = $scope.ElectionSocietyDetails.RegistrationNo == undefined ? "" : $scope.ElectionSocietyDetails.RegistrationNo.replace(/ /g, '');
        var SocietyCode = $scope.ElectionSocietyDetails.SocietyCode == undefined ? "" : $scope.ElectionSocietyDetails.SocietyCode.replace(/ /g, '');
        if (SocietyName == "") {
            message += _language == 'en-us' ? '<br />Please enter the society name\n' : '<br />कृपया सोसाइटी का नाम दर्ज करें।\n';
            $("#SocietyName").addClass("validdata");
        }

        if (RegistrationNo == "") {
            message += _language == 'en-us' ? '<br />Please enter the society registration number.\n' : '<br />कृपया सोसाइटी का रजिस्ट्रेशन नंबर दर्ज करें।\n';
            $("#RegistrationNo").addClass("validdata");
        }

        if (SocietyCode == "") {
            message += _language == 'en-us' ? '<br />Please enter the society code\n' : '<br />कृपया सोसाइटी का कोड दर्ज करें।\n';
            $("#SocietyCode").addClass("validdata");
        }

        if ($("#txtFromDate").val() == "") {
            message += _language == 'en-us' ? '<br />Please select the election start date\n' : '<br />कृपया चुनाव प्रारंभ तिथि का चयन करें।\n';
            $("#txtFromDate").addClass("validdata");
        }
        if ($("#txtToDate").val() == "") {
            message += _language == 'en-us' ? '<br />Please select the election end date\n' : '<br />कृपया चुनाव की समाप्ति तिथि चुनें।\n';
            $("#txtToDate").addClass("validdata");
        }

        $scope.MemberAddress = GetAddressinfo('MemberAdd');

        if ($scope.MemberAddress.HouseBuildingNo == undefined || $scope.MemberAddress.HouseBuildingNo == '' || $scope.MemberAddress.HouseBuildingNo == ' ' || $scope.MemberAddress.HouseBuildingNo == null) {
            message += _language == 'en-us' ? '<br />Please enter the house/Building Number\n' : '<br />कृपया घर/भवन संख्या दर्ज करें।\n';
            $("#HouseBuildingNo_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.LaneStreetName == undefined || $scope.MemberAddress.LaneStreetName == '' || $scope.MemberAddress.LaneStreetName == ' ' || $scope.MemberAddress.LaneStreetName == null) {
            message += _language == 'en-us' ? '<br />Please enter LaneStreetName\n' : '<br />कृपया पथ/गली नाम दर्ज करें।\n';
            $("#LaneStreetName_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.Locality == undefined || $scope.MemberAddress.Locality == '' || $scope.MemberAddress.Locality == ' ' || $scope.MemberAddress.Locality == null) {
            message += _language == 'en-us' ? '<br />Please enter Locality \n' : '<br />कृपया स्थानीयता दर्ज करें।\n';
            $("#Locality_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.Stateid == undefined || $scope.MemberAddress.Stateid == '' || $scope.MemberAddress.Stateid == null) {
            message += _language == 'en-us' ? '<br />Please select the State\n' : '<br />कृपया राज्य चुनें।\n';
            $("#Stateid_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.DivisionId == undefined || $scope.MemberAddress.DivisionId == '0' || $scope.MemberAddress.DivisionId == '' || $scope.MemberAddress.DivisionId == null) {
            message += _language == 'en-us' ? '<br />Please select the Division\n' : '<br />कृपया संभाग का चयन करें।\n';
            $("#DivisionId_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.DistrictId == undefined || $scope.MemberAddress.DistrictId == '0' || $scope.MemberAddress.DistrictId == '' || $scope.MemberAddress.DistrictId == null) {
            message += _language == 'en-us' ? '<br />Please select the District\n' : '<br />कृपया जिले का चयन करें।\n';
            $("#DistrictId_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.Pincode == undefined || $scope.MemberAddress.Pincode == '' || $scope.MemberAddress.Pincode == ' ' || $scope.MemberAddress.Pincode == null) {
            message += _language == 'en-us' ? '<br />Please enter Pincode\n' : '<br />कृपया पिन कोड दर्ज करें।\n';
            $("#pincode_MemberAdd").addClass("validdata");
        }

        if ($scope.MemberAddress.Urban_Rural == "" || $scope.MemberAddress.Urban_Rural == "0" || $scope.MemberAddress.Urban_Rural == undefined) {
            message += _language == 'en-us' ? 'Please select the Urban/Rural\n' : '<br /> कृपया शहरी/ग्रामीण का चयन करें\n';
            $("#Urban_Rural_MemberAdd").addClass("validdata");
        } else {
            if ($scope.MemberAddress.Urban_Rural == "Urban") {
                if ($scope.MemberAddress.CityId == undefined || $scope.MemberAddress.CityId == '0' || $scope.MemberAddress.CityId == '' || $scope.MemberAddress.CityId == null) {
                    message += _language == 'en-us' ? '<br />Please select the City\n' : '<br />कृपया शहर का चयन करें।\n';
                    $("#ddlcity_MemberAdd").addClass("validdata");
                }
                if ($scope.MemberAddress.WardId == undefined || $scope.MemberAddress.WardId == '0' || $scope.MemberAddress.WardId == null || $scope.MemberAddress.WardId == '? string: ?') {
                    message += _language == 'en-us' ? '<br />Please select the Ward\n' : '<br />कृपया वार्ड का चयन करें।\n';
                    $("#ddlward_MemberAdd").addClass("validdata");
                }
            }
            if ($scope.MemberAddress.Urban_Rural == "Rurals") {
                if ($scope.MemberAddress.BlockId == undefined || $scope.MemberAddress.BlockId == '0' || $scope.MemberAddress.BlockId == '' || $scope.MemberAddress.BlockId == null) {
                    message += _language == 'en-us' ? '<br />Please select the Block\n' : '<br />कृपया ब्लॉक का चयन करें।\n';
                    $("#ddlblocki_MemberAdd").addClass("validdata");
                }
                if ($scope.MemberAddress.PanchayatId == undefined || $scope.MemberAddress.PanchayatId == '0' || $scope.MemberAddress.PanchayatId == null || $scope.MemberAddress.PanchayatId == '? string: ?') {
                    message += _language == 'en-us' ? '<br />Please select the Gram Panchayat\n' : '<br />कृपया ग्राम पंचायत का चयन करें।\n';
                    $("#ddlGP_MemberAdd").addClass("validdata");
                }
                if ($scope.MemberAddress.VillageId == undefined || $scope.MemberAddress.VillageId == '0' || $scope.MemberAddress.VillageId == null || $scope.MemberAddress.VillageId == '? string: ?') {
                    message += _language == 'en-us' ? '<br />Please select the Village\n' : '<br />कृपया गाँव का चयन करें।\n';
                    $("#ddlVillage_MemberAdd").addClass("validdata");
                }
            }
        }

        if (status) {

            $scope.ElectionSocietyMembersDetails.forEach(function (itm, index) {
                var name = itm.Name == undefined || itm.Name == null ? "" : itm.Name.replace(/ /g, '');
                var ContactNo = itm.ContactNo == undefined || itm.ContactNo == null ? "" : itm.ContactNo.replace(/ /g, '');
                var AADHAARNo = itm.AADHAARNo == undefined || itm.AADHAARNo == null ? "" : itm.AADHAARNo.replace(/ /g, '');

                if (name == undefined || name == '' || name == ' ' || name == null) {
                    if (message.indexOf(_language == 'en-us' ? 'Please enter member name' : 'कृपया सदस्य का नाम दर्ज करें') < 0) {
                        message += _language == 'en-us' ? '<br />Please enter member name\n' : '<br />कृपया सदस्य का नाम दर्ज करें।\n';
                    }
                    $("#memberName_" + index).addClass("validdata");
                }
                if (itm.Post == undefined || itm.Post == '0' || itm.Post == '' || itm.Post == null) {
                    if (message.indexOf(_language == 'en-us' ? 'Please select member post' : 'कृपया सदस्य का पद चुनें') < 0) {
                        message += _language == 'en-us' ? '<br />Please select member post\n' : '<br />कृपया सदस्य का पद चुनें।\n';
                    }
                    $("#memberPost_" + index).addClass("validdata");
                }

                if (ContactNo == undefined || ContactNo == '' || ContactNo == ' ' || ContactNo == null) {
                    if (message.indexOf(_language == 'en-us' ? 'Please enter contact no of member' : 'कृपया सदस्य का संपर्क नंबर दर्ज करें') < 0) {
                        message += _language == 'en-us' ? '<br />Please enter contact no of member\n' : '<br />कृपया सदस्य का संपर्क नंबर दर्ज करें।\n';
                    }
                    $("#memberContactNo_" + index).addClass("validdata");
                }

                if (itm.CategoryId == undefined || itm.CategoryId == '0' || itm.CategoryId == '' || itm.CategoryId == null) {
                    if (message.indexOf(_language == 'en-us' ? 'Please select Category of member' : 'कृपया सदस्य की श्रेणी चुनें।') < 0) {
                        message += _language == 'en-us' ? '<br />Please select Category of member\n' : '<br />कृपया सदस्य की श्रेणी चुनें।\n';
                    }
                    $("#memberCategoryId_" + index).addClass("validdata");
                }

                if (itm.Post == 'अध्यक्ष' && (AADHAARNo == '' || AADHAARNo == null || AADHAARNo.length < 12)) {
                    if (message.indexOf(_language == 'en-us' ? 'Please enter 12 digit aadhar number of secretory' : 'कृपया अध्यक्ष का 12 अंकों का आधार नंबर दर्ज करें') < 0) {
                        message += _language == 'en-us' ? '<br />Please enter 12 digit aadhar number of secretory\n' : '<br />कृपया अध्यक्ष का 12 अंकों का आधार नंबर दर्ज करें।\n';
                    }
                    $("#memberAADHAARNo_" + index).addClass("validdata");
                }
            });
        }

        if (message.length > 29) {
            ShowOkMessage("Auditor Request", message, function () { });
            return false;
        }

        return true;
    }

    $scope.SubmitData = function () {
        if ($scope.validateData(true)) {
            $scope.SaveElectionData(false, '1').then(function (d) {

                if (d.data.data == '-5') {

                }
                else if (d.data.data == '-10') {

                }
                else
                    if (d.data.data == '-1') {

                    }
                    else if ((d.data.data.split('@')).length > 1) {
                        AadhaarAuth("", $http, "", true, function (authData) {
                            $scope.GenerateElectionResultLetter(true, authData.TransId);
                        });
                    }
                    else if (d.data.data.length > 10) {

                    }
            });
        }
    }

    $scope.GenerateElectionResultLetter = function (status, transid) {
        if ($scope.validateData(status)) {
            $(".se-pre-con").fadeIn("slow");
            var memberdata = [];

            $scope.ElectionSocietyMembersDetails.forEach(function (itm, index) {
                if (itm.Name != undefined && itm.Name != "") {
                    memberdata.push({
                        SocietyIdEnc: itm.SocietyIdEnc,
                        AdministrativeId: itm.MemberId,
                        Name: itm.Name,
                        PostId: 0,
                        PostName: itm.Post,
                        MEMBERTYPE: "ELECTEDMEMBERS",
                        ContactNo: itm.ContactNo,
                        AADHAARNo: itm.AADHAARNo,
                        CategoryId: itm.CategoryId,
                        FatherName: itm.FatherName
                    });
                }

            });
            $scope.Address = GetAddressinfo('MemberAdd');



            $scope.SocietyRegisteredOffice = {
                SocietyRegOfficeDetailId: $scope.ElectionSocietyDetails.SocietyRegOfficeDetailId,
                IsRural: $scope.Address.Urban_Rural == 'Rurals' ? true : false,
                HouseBuildingNo: $scope.Address.HouseBuildingNo,
                LaneStreetName: $scope.Address.LaneStreetName,
                Locality: $scope.Address.Locality,
                STATE_ID: $scope.Address.Stateid,
                DIVISION_ID: $scope.Address.DivisionId,
                DISTRICT_ID: $scope.Address.DistrictId,
                //CityTypes: $scope.Address.CityTypes,
                CITY_ID: $scope.Address.CityId,
                BLOCK_ID: $scope.Address.BlockId,
                WARD_ID: $scope.Address.WardId,
                GP_ID: $scope.Address.PanchayatId,
                VILLAGE_ID: $scope.Address.VillageId,
                PinCode: $scope.Address.Pincode,
                SocietyIdEnc: '',
                CreatedBy: 0,
                UpdatedBy: 0,
                CreatedIP: '',
                UpdatedIP: '',
                IsSameCEOAddress: false,
                AddressFor: 0,
                //SocietyRegOfficeDetailId: "",
                SocietyId: "0",
                ActId: 3,
                DeleteFlag: false
            }



            var PostData = {
                SocietyIDEnc: $scope.ElectionSocietyDetails.SocietyIdEnc == undefined || $scope.ElectionSocietyDetails.SocietyIdEnc == null ? "" : $scope.ElectionSocietyDetails.SocietyIdEnc,
                SocietyName: $scope.ElectionSocietyDetails.SocietyName,
                RegistrationNo: $scope.ElectionSocietyDetails.RegistrationNo,
                SocietyCode: $scope.ElectionSocietyDetails.SocietyCode,
                AddressDetails: $scope.SocietyRegisteredOffice,
                AdministrativeDataList: memberdata,
                SocietyTypeId: 10,
                ActId: 3,
                ElectionStartDate: $("#txtFromDate").val(),
                ElectionEndDate: $("#txtToDate").val(),
                ElectionMasterId: $scope.ElectionSocietyDetails.ElectionMasterId
            }


            return $http.post(SiteUrl + "/Election/GenerateElectionResultLetter", JSON.stringify({ _Entity: PostData, Transid: transid })).
                success(function (data, status) {
                    $("#InnerModelPopup").addClass("alert alert-danger fade in");

                    if (data.data == '-5') {
                        ShowModelAlert("Error", "A Society already exist with this name or registration");
                    }
                    else if (data.data == '-10') {
                        ShowModelAlert("Error", "YOU ALREADY SUBMITTED 2 SOCIETY WITH THE SAME SSOID");
                    }
                    else
                        if (data.data == '-1') {
                            $(".SaveSocietyInfo").show();
                            ShowModelAlert("Error", "Some error occurred please try after some time ");
                        }
                        else {
                           
                            
                            var message1 = $scope.IsEditMode == false ? "Election Result Saved Sucessfully" : "Election Result Saved Sucessfully";
                              
                                    ShowOkMessage("Success", message1, function () {
                                        window.location = "../election/UploadElectionData";
                                    });
                                
                            
                        }
                   
                    $(".se-pre-con").fadeOut("slow");
                    //$scope.ClearForm();
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                    $('#btnAdministrativedetails').trigger('click');
                });

        }
    }

    $scope.SaveElectionData = function (status, transid) {

        if ($scope.validateData(status)) {
            $(".se-pre-con").fadeIn("slow");
            var memberdata = [];

            $scope.ElectionSocietyMembersDetails.forEach(function (itm, index) {
                if (itm.Name != undefined && itm.Name != "") {
                    memberdata.push({
                        SocietyIdEnc: itm.SocietyIdEnc,
                        AdministrativeId: itm.MemberId,
                        Name: itm.Name,
                        PostId: 0,
                        PostName: itm.Post,
                        MEMBERTYPE: "ELECTEDMEMBERS",
                        ContactNo: itm.ContactNo,
                        AADHAARNo: itm.AADHAARNo,
                        CategoryId: itm.CategoryId,
                        FatherName: itm.FatherName
                    });
                }

            });
            $scope.Address = GetAddressinfo('MemberAdd');



            $scope.SocietyRegisteredOffice = {
                SocietyRegOfficeDetailId: $scope.ElectionSocietyDetails.SocietyRegOfficeDetailId,
                IsRural: $scope.Address.Urban_Rural == 'Rurals' ? true : false,
                HouseBuildingNo: $scope.Address.HouseBuildingNo,
                LaneStreetName: $scope.Address.LaneStreetName,
                Locality: $scope.Address.Locality,
                STATE_ID: $scope.Address.Stateid,
                DIVISION_ID: $scope.Address.DivisionId,
                DISTRICT_ID: $scope.Address.DistrictId,
                //CityTypes: $scope.Address.CityTypes,
                CITY_ID: $scope.Address.CityId,
                BLOCK_ID: $scope.Address.BlockId,
                WARD_ID: $scope.Address.WardId,
                GP_ID: $scope.Address.PanchayatId,
                VILLAGE_ID: $scope.Address.VillageId,
                PinCode: $scope.Address.Pincode,
                SocietyIdEnc: '',
                CreatedBy: 0,
                UpdatedBy: 0,
                CreatedIP: '',
                UpdatedIP: '',
                IsSameCEOAddress: false,
                AddressFor: 0,
                //SocietyRegOfficeDetailId: "",
                SocietyId: "0",
                ActId: 3,
                DeleteFlag: false
            }



            var PostData = {
                SocietyIDEnc: $scope.ElectionSocietyDetails.SocietyIdEnc == undefined || $scope.ElectionSocietyDetails.SocietyIdEnc == null ? "" : $scope.ElectionSocietyDetails.SocietyIdEnc,
                SocietyName: $scope.ElectionSocietyDetails.SocietyName,
                RegistrationNo: $scope.ElectionSocietyDetails.RegistrationNo,
                SocietyCode: $scope.ElectionSocietyDetails.SocietyCode,
                AddressDetails: $scope.SocietyRegisteredOffice,
                AdministrativeDataList: memberdata,
                SocietyTypeId: 10,
                ActId: 3,
                ElectionStartDate: $("#txtFromDate").val(),
                ElectionEndDate: $("#txtToDate").val(),
                ElectionMasterId: $scope.ElectionSocietyDetails.ElectionMasterId
            }

            //formData.append("SocietyIDEnc", $scope.ElectionSocietyDetails.SocietyIdEnc);
            //formData.append("SocietyName", $scope.ElectionSocietyDetails.SocietyName);
            //formData.append("RegistrationNo", $scope.ElectionSocietyDetails.RegistrationNo);
            //formData.append("SocietyCode", $scope.ElectionSocietyDetails.SocietyCode);
            //formData.append("FromDate", $('#txtFromDate').val());
            //formData.append("ToDate", $('#txtToDate').val());
            //formData.append("AddressDetails", JSON.stringify($scope.SocietyRegisteredOffice));
            //formData.append("DataArray", JSON.stringify(memberdata));

            return $http.post(SiteUrl + "/Election/SaveElectionUploaddata", JSON.stringify({ _Entity: PostData, Transid: transid })).
                success(function (data, status1) {
                    $("#InnerModelPopup").addClass("alert alert-danger fade in");

                    if (data.data == '-1') {
                        ShowModelAlert("Error", "A Society already exist with this name");
                    }
                    if (data.data == '-2') {
                        ShowModelAlert("Error", "A Society already exist with this registration");
                    } if (data.data == '-3') {
                        ShowModelAlert("Error", "A Society already exist with this Society Code");
                    }                    
                    else
                        if (data.data == '0') {
                            $(".SaveSocietyInfo").show();
                            ShowModelAlert("Error", "Some error occurred please try after some time ");
                        }
                        else if ((data.data.split('@')).length > 1) {
                            //ShowModelAlert("Society Profiling", "Basic Details Update Sucessfully");
                            //if ($scope.IsEditMode == false) {
                            //    ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                            //}
                            //window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                            if (data.data.split('#')[1] != undefined) {
                                $scope.ElectionSocietyDetails.ElectionMasterId = data.data.split('#')[1];
                            }
                            if (data.data.split('@')[0] != undefined) {
                                $scope.ElectionSocietyDetails.SocietyIdEnc = data.data.split('@')[0];
                            }
                            

                            if (transid == "0") {
                                var message1 = $scope.IsEditMode == false ? "Basic Details Saved Sucessfully" : "Basic Details Update Sucessfully";
                                if (transid == "0") {
                                    ShowOkMessage("Success", message1, function () {
                                        window.location = "../election/UploadElectionData";
                                    });
                                }
                            }
                        }
                        else if (data.data.length > 10) {
                            //ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                            //window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";

                            if (transid == "0") {
                                ShowOkMessage("Success", "Basic Details Saved Sucessfully", function () {
                                    window.location = "../election/UploadElectionData";
                                });
                            }

                        }
                    $(".se-pre-con").fadeOut("slow");
                    //$scope.ClearForm();
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                    $('#btnAdministrativedetails').trigger('click');
                });

        }
    }

    $scope.SaveSocietyAdministrativeDetails = function (IsSubmit) {
        if ($scope.TableValiDate(3)) {

            //if (confirm("You can't update data after submit")) {
            var SocietyCandidateData = [];
            var formData = new FormData();
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });

            if (IsSubmit) {
                var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
                if (!confirm(message)) {
                    return;
                }
            }
            $(".blockUI").fadeIn("slow");
            formData.append("StepType", 7);
            formData.append("IsSubmit", IsSubmit);
            var imgindx = 0;
            $("#myTable2").find('input[type="file"]').each(function (indx) {



                var fileUpload = $(this).get(0);
                var files = fileUpload.files;


                formData.append("ResultPDFFile_" + (imgindx + 1), files[0]);
                ++imgindx;


            });






            //alert($scope.ElectionMasterId);
            // alert($scope.SocietyDirectory);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.EMemberDetailsData.forEach(function (item, index) {
                if ($scope.EMemberDetailsData[index].IsCandidate && $scope.EMemberDetailsData[index].IsCandidate != 'False') {


                    if (IsSubmit == true) {

                        $scope.ActivityStatus = 'Election result published by election officer' + " -" + item.Name;
                    }
                    else {
                        $scope.ActivityStatus = 'Election result save by election officer' + " -" + item.Name;
                    }
                    SocietyCandidateData.push(
                        {

                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "Name": item.Name,
                            "MemberId": item.MemberId,
                            "IsActive": true,
                            "ContactNo": item.ContactNo,
                            "AADHAARNo": item.AadharNo2,
                            "DOB": item.DOB.split("/").reverse().join("-"),
                            "Gender": item.Gender,
                            "CategoryId": item.CategoryId,
                            "ElectionMasterId": item.ElectionMasterId,
                            //"PostId": $(this).find('td:eq(7)').find('select').val(),
                            "UserType": "5",//Nominated member
                            "AdministratorDesignationId": "-1",
                            "IsCEO": false,
                            "RowNO": index,
                            "Photo": item.Photo,
                            "FHName": item.FHName,
                            "Address": item.Address,
                            "CandidatePDFForm": item.CandidatePDFForm == undefined ? "" : item.CandidatePDFForm,
                            "TotalVotes": item.TotalVotes,
                            "IsWinner": item.IsWinner == null ? false : item.IsWinner,
                            "ResultRemark": item.ResultRemark,
                            "ResultDocument": item.ResultDocument,
                            "StepType": 3,
                            "Type": 1,
                            "ActivityStatus": $scope.ActivityStatus
                        });
                }
                //SocietyCandidateData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
            });
            //alert(JSON.stringify(SocietyCandidateData));
            formData.append("DataArray", JSON.stringify(SocietyCandidateData));
            $http.post(SiteUrl + "/Election/InsertElectionResult", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.indexOf("-2") > -1) {
                        alert("You can't change the member detail.");
                        location.reload();
                    }
                    else if (data.indexOf("-1") > -1) {
                        alert("Some technical error occur");
                    }
                    else {
                        alert("Data saved successfully");
                        location.reload();
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                });
            // }
        }

    }

    $scope.TableValiDate = function (tableflag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        //$("#myTable2").find('input[type="file"]').removeClass("notValid");
        //if ($("#myTable2").find('input[type="checkbox"]:checked').length == 0) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Candidate.' : '\n <br />कृपया उम्मीदवार का चयन करें।';
        //}
        $("#myTable2").find('input[type="text"]').removeClass("notValid");
        $("#myTable2").find('input[type="file"]').removeClass("notValid");


        if ($("#myTable2").find('input[type="checkbox"]:checked').length == 0) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Check Result' : 'कृपया रिजल्ट चेक करें') < 0) {
                message += $scope._language == 'en-us' ? '\n <br />Please Check Result.' : '\n <br />कृपया रिजल्ट चेक करें।';
            }
            // $("#myTable2").find('input[type="file"]').eq(index).addClass("notValid");
        }


        $scope.EMemberDetailsData.forEach(function (item, index) {
            //if (item.IsCandidate && item.IsCandidate != 'False') {


            if (item.TotalVotes == '' || item.TotalVotes == null) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Number of Vote' : 'कृपया वोट की संख्या दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '\n <br />Please Enter Number of Vote.' : '\n <br />कृपया वोट की संख्या दर्ज करें।';
                }
                $("#myTable2").find('input[type="text"]').eq(index).addClass("notValid");
            }


            if ($("#myTable2").find('input[type="file"]').eq(index).val() == "" && (item.ResultDocument == '' || item.ResultDocument == null)) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please upload Document' : 'कृपया मतगणना पत्र अपलोड करें') < 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please upload Document.' : '\n <br />कृपया मतगणना पत्र अपलोड करें।';
                }
                $("#myTable2").find('input[type="file"]').eq(index).addClass("notValid");
            }



            //}

        });


        if (message.length > 29) {
            ShowModelAlert("Candidate List", message);
            return false;
        }
        return true;
    }
    //Add Row Calender Css Add
    $scope.AddCalender = function () {
        $('.AddDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D',
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
            yearRange: "-100:-18"

        });
    }
    //END


    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnddrDISTRICT_ID").val() != "0" && $("#hdnddrDISTRICT_ID").val() != "") {
                    $scope.ElectionData.DistrictId = $("#hdnddrDISTRICT_ID").val();
                    $("#ddrDISTRICT_ID").attr("disabled", "disabled")
                } else {
                    $scope.ElectionData.DistrictId = "";
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    var addressdataset1 = {
                        Housebuilding: "",
                        State: "6",
                        District: "  ",
                        Lanestreet: "",
                        Locality: "",
                        Pincode: ""
                    }
                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    var addressdataset1 = {
                        Housebuilding: "",
                        State: "6",
                        District: "  ",
                        Lanestreet: "",
                        Locality: "",
                        Pincode: ""
                    }
                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        //Get Board Member Post List for dropdown


        $(".se-pre-con").fadeOut("slow");
        // END : Post List for dropdown
        $scope.GetDistrictList()
        $scope.GetElectionSocietyListData();
    }


    $scope.GetElectionSocietyListData = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Election/GetElectionSocietyListData_New", JSON.stringify({ Details: jsonParameters, StepType: 7 })).
            success(function (data, status) {

                $scope.ElectionSocietyListData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }


    $scope.ElUploadAdministrative = function (file, row) {
        // alert(JSON.stringify(ImageDataList));
        for (var temRowNo = 0; temRowNo < ImageDataList.length; temRowNo++) {
            if (ImageDataList[temRowNo].RowNo == row) {
                ImageDataList.splice(temRowNo);
            }
        }
        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = file[0];
        FileDataWithRowObj.RowNo = row;
        ImageDataList.push(FileDataWithRowObj);
        //  alert(JSON.stringify(ImageDataList));
    }

    $scope.ElUploadPDF = function (file, row) {
        //  alert(JSON.stringify(PDFDataList));
        for (var temRowNo1 = 0; temRowNo1 < PDFDataList.length; temRowNo1++) {
            if (PDFDataList[temRowNo1].RowNo == row) {
                PDFDataList.splice(temRowNo1);
            }
        }
        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = file[0];
        FileDataWithRowObj.RowNo = row;
        PDFDataList.push(FileDataWithRowObj);
        // alert(JSON.stringify(PDFDataList));
    }

    $scope.OnPageLoad();
    $scope.ishowSocietyTable = false;
    $scope.IsShowAddNew = true;
    $scope.UploadElection = function () {
        $scope.ShowSocietyList = false;
        $scope.ShowVoterList = true;
        $scope.ElectionData = { DistrictId: "", SearchText: "" };
        $scope.ElectionSocietyLis = [];
        $scope.SocietyListShow = true;
        $scope.ElectionSocietyDetails = { SocietyName: "", RegistrationNo: "", SocietyCode: "" };
        $scope.ishowSocietyTable = false;
        $scope.IsShowAddNew = false;
        $scope.ishowSocietyTable = false;

        if ($("#hdnddrDISTRICT_ID").val() != "0" && $("#hdnddrDISTRICT_ID").val() != "") {
            $scope.ElectionData.DistrictId = $("#hdnddrDISTRICT_ID").val();
            $("#ddrDISTRICT_ID").attr("disabled", "disabled")
        } else {
            $scope.ElectionData.DistrictId = "";
        }

    }
    $scope.ElectionSocietyLis = [];
    $scope.ElectionData = { DistrictId: "", SearchText: "" };
    $scope.q = "";
    $scope.pageSize = 10;
    $scope.currentPage = 1;

    $scope.BackToSocietyList = function () {
        $scope.SocietyListShow = true;
    }
    $scope.BackToMainList = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $scope.IsShowAddNew = true;
        $scope.ishowSocietyTable = false;
    }


    $scope.FindElectionSocieties = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetElectionSocieties", JSON.stringify({ DistrictId: $scope.ElectionData.DistrictId, SearchText: $scope.ElectionData.SearchText })).
            success(function (data, status) {

                $scope.ElectionSocietyLis = JSON.parse(data.DataList);
                if ($scope.ElectionSocietyLis.length <= 0) {
                    ShowAlertMessage("", "No match found. Please add the basic details and information of elected members", function () {
                        $scope.ishowSocietyTable = true;
                        $scope.SelectSocietyForExecutive([], true);
                        addressdataset1 = {
                            Housebuilding: "",
                            State: "6",
                            District: " " + $scope.ElectionData.DistrictId,
                            Lanestreet: "",
                            Locality: "",
                            Pincode: "",
                            CITY_ID: "",
                            BLOCK_ID: "",
                            GP_ID: "",
                            WARD_Id: "",
                            VILLAGE_ID: ""
                        }

                        $("#ddlDivision_MemberAdd").attr("disabled", "disabled");
                        $("#district_MemberAdd").attr("disabled", "disabled");
                        $("#ddlstate_MemberAdd").attr("disabled", "disabled");

                        SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function () {
                        });
                        $scope.$apply();
                    });
                } else {
                }
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionSocietyLis = [];
                $(".blockUI").fadeOut("slow");
            });
    }
    $scope.ElectionSocietyDetails = {};
    $scope.ElectionSocietyMembersDetails = [];
    $scope.ElectionSocietyMembersDetailsAll = [];
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
    //$scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });


    $scope.SocietyIdForExecutive = "";
    $scope.IsNewSociety = false;

    var sourceAutoComplete = [];
    var mappingAutoComplete = {};


    $scope.SelectSocietyForExecutive = function (Item, IsNew) {
        //$scope.SocietyIdForExecutive = Item.SocietyIdEnc;
        $scope.SocietyListShow = false;
        $scope.IsNewSociety = IsNew;
        $scope.ElectionSocietyMembersDetails = [];
        $scope.ElectionSocietyDetails = { SocietyName: "", RegistrationNo: "", SocietyCode: "" };
        $(".blockUI").fadeIn("slow");
        if (!IsNew) {
            $http.post(SiteUrl + "/Election/GetSocietyDetailsWithMembers", JSON.stringify({ SocietyIdENC: Item.SocietyIdEnc })).
                success(function (data, status) {
                    $scope.ElectionSocietyDetails = JSON.parse(data.DataList)[0];

                    $("#txtFromDate").val($scope.ElectionSocietyDetails.ElectionStartDate);
                    $("#txtToDate").val($scope.ElectionSocietyDetails.ElectionEndDate);

                    var editdatata = [];
                    sourceAutoComplete = [];
                    mappingAutoComplete = {};

                    $scope.ElectionSocietyMembersDetailsAll = JSON.parse(data.DataList);
                    $scope.ElectionSocietyMembersDetailsAll.forEach(function (item, inddex) {
                        if (editdatata.filter(function (x) { return x.id === item.AdministrativeId }).length<=0) {
                            editdatata.push({ id: item.AdministrativeId });
                            sourceAutoComplete.push(item.Name);
                            mappingAutoComplete[item.Name] = item.AdministrativeId;
                        }
                        if ($scope.ElectionSocietyMembersDetails.filter(function (x) { return x.MemberId === item.AdministrativeId }).length <= 0 && item.MEMBERTYPE === "ELECTEDMEMBERS" ) {
                            $scope.ElectionSocietyMembersDetails.push({
                                MemberId: item.AdministrativeId, Name: item.Name, FatherName: item.FatherName, ContactNo: item.ContactNo,
                                AADHAARNo: item.Aadh == undefined || item.Aath == '' || item.Aadh == null || item.Aadh.length < 12 ? "" : "XXXXXXXX" + item.Aadh.substring(8, 12)
                                , CategoryId: item.CategoryId != null ? item.CategoryId.toString() : "0", Post: item.POSTNAME
                            });                           
                        }
                    });
                    var length = $scope.ElectionSocietyMembersDetails.length;
                    if (length < 12) {
                        for (var i = 0; i < 12 - length; i++) {
                            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
                        }
                    }

                    addressdataset1 = {
                        Housebuilding: $scope.ElectionSocietyDetails.HouseNo1,
                        State: $scope.ElectionSocietyDetails.STATE_ID1,
                        District: " " + $scope.ElectionSocietyDetails.DISTRICT_ID1,
                        Lanestreet: $scope.ElectionSocietyDetails.LaneStreetName1,// + ' ' + addressdata1[1],
                        Locality: $scope.ElectionSocietyDetails.Locality1,//+ ' ' + addressdata1[11],
                        Pincode: $scope.ElectionSocietyDetails.PinCode1,
                        CITY_ID: $scope.ElectionSocietyDetails.CITY_ID1,
                        BLOCK_ID: $scope.ElectionSocietyDetails.BLOCK_ID1,
                        GP_ID: $scope.ElectionSocietyDetails.GP_ID1,
                        WARD_Id: $scope.ElectionSocietyDetails.WARD_ID1,
                        VILLAGE_ID: $scope.ElectionSocietyDetails.VILLAGE_ID1,
                        IsRurals: $scope.ElectionSocietyDetails.IsRural1 || $scope.ElectionSocietyDetails.BLOCK_ID1!=0
                    }

                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function () {
                        //$scope.Empanelled.AddressDetials = GetAddressDispplay('MemberAdd');
                        //$scope.EmpanelledAddress = GetAddressinfo('MemberAdd');
                    });
                    $("#ddlDivision_MemberAdd").attr("disabled", "disabled");
                    $("#district_MemberAdd").attr("disabled", "disabled");
                    $("#ddlstate_MemberAdd").attr("disabled", "disabled");

                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.ElectionSocietyLis = [];
                    $(".blockUI").fadeOut("slow");
                });
        } else {
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $scope.ElectionSocietyMembersDetails.push({ MemberId: 0, Name: "", FatherName: "", ContactNo: "", AADHAARNo: "", CategoryId: "0", Post: "0" });
            $(".blockUI").fadeOut("slow");
        }
    }
    //Get Chairman Category List for dropdown
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
        success(function (data, status) {
            $scope.CategoryList = data;
        }).
        error(function (data, status) {
            alert("Error while fetching Post list")

        });

    $scope.GetSubmitedElectionSocietyListData = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetSocietiesForElectionUpdate", JSON.stringify({ SearchText: $scope.Filter.SearchText1 })).
            success(function (data, status) {
                $scope.SubmitedElectionSocietyList = JSON.parse(data.DataList);
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    $scope.GetSubmitedElectionSocietyListData();

    $scope.complete = function (th) {

        $scope.ElectionSocietyMembersDetails[th].MemberId = "0";
        $("#memberName_" + th).autocomplete({
            source: sourceAutoComplete,
            //select: selectItem,
            //minLength: 4,


            minLength: 1,
            // source: source,
            select: function (event, ui) {
                var id = mappingAutoComplete[ui.item.value];
                var curItem = $scope.ElectionSocietyMembersDetailsAll.filter(function (x) { return x.AdministrativeId == id })[0];
                //var edititem = $scope.ElectionSocietyMembersDetails[th];
                $scope.ElectionSocietyMembersDetails[th].MemberId = curItem.AdministrativeId;
                //$scope.ElectionSocietyMembersDetails[th].MemberId = curItem.AdministrativeId;
                //edititem.Name = curItem.Name;
                $scope.ElectionSocietyMembersDetails[th].ContactNo = curItem.ContactNo;
                $scope.ElectionSocietyMembersDetails[th].AADHAARNo = curItem.Aadh == undefined || curItem.Aadh == '' || curItem.Aadh == null || curItem.Aadh.length < 12 ? "" : "XXXXXXXX" + curItem.Aadh.substring(8, 12); //curItem.AADHAARNo;
                $scope.ElectionSocietyMembersDetails[th].CategoryId = curItem.CategoryId != null ? curItem.CategoryId.toString() : "0";
                $scope.ElectionSocietyMembersDetails[th].Post = curItem.POSTNAME;
                $scope.ElectionSocietyMembersDetails[th].FatherName = curItem.FatherName;              

                $scope.$apply();
            }
        });


    }




    //$("#tags_"+th).autocomplete({
    //    source: $scope.availableTags
    //});



});

AppClasses.filter('split', function () {
    return function (input, splitChar, splitIndex) {
        // do some bounds checking here to ensure it has that index
        return input.split(splitChar)[splitIndex];
    }
});
//{ { test | split: ',': 0 } }
//{ { test | split: ',': 0 } }