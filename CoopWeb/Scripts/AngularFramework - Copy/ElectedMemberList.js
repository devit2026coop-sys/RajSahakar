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
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize))+ (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
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
    $scope.ElectionOfficials = "";
    $scope.AddNewMember = {
        SrNo: 0,
        MemberId: '0',
        Name: '',
        ContactNo: '',
        AadharNo: '',
        DOB: '',
        Gender: '',
        CODEID: '',
        FHName: '',
        Address: '',
        Photo: '',
        IsDelete: false,
        AddRemarks: '',
        DeleteDocument: '',
        DeleteRemarks: '',
        AddDocument: '',
        SocietyIdEnc: $scope.SocietyIdForAdministrativeDetails,
        IsActive: true,
        ElectionMasterId: 0,
        UserType: 5,
        AdministratorDesignationId: -1,
        IsCEO: true,
        CategoryId: "0",
        AadharNo2: '',
        Index: 0,
        IsShow: true,
        ActionType: '',
        ActionOnCandidate: false,
        ActionRemark: '',
        ActionDocument: '',
        ActionDate: '',
        ElectionPostid: '',
        ElectionPostFile: ''
    }
    $scope.ReSetAddModel = function () {
        $scope.AddNewMember = {
            SrNo: 0,
            MemberId: '0',
            Name: '',
            ContactNo: '',
            AadharNo: '',
            DOB: '',
            Gender: '',
            CODEID: '',
            FHName: '',
            Address: '',
            Photo: '',
            IsDelete: false,
            AddRemarks: '',
            DeleteDocument: '',
            DeleteRemarks: '',
            AddDocument: '',
            SocietyIdEnc: $scope.SocietyIdForAdministrativeDetails,
            IsActive: true,
            ElectionMasterId: 0,
            UserType: 5,
            AdministratorDesignationId: -1,
            IsCEO: true,
            CategoryId: "0",
            AadharNo2: '',
            Index: 0,
            IsShow: true,
            ActionType: '0',
            ActionOnCandidate: false,
            ActionRemark: '',
            ActionDocument: '',
            ActionDate: '',
            ElectionPostid: '',
            ElectionPostFile: ''
        }
        $("#addModal").find('input[type="file"]').val("");
        $('#addNewDelete').prop('checked', false);
        $('.addNewIsDelete').hide();
        $("#MemberDeletePDFForm1").val("");
        $("#MemberEditPDFForm1").val("");
    }

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
    $scope.NominationActions = [];
    $scope.IsElectedList = true;
    $scope.IsSelectedList = false;
    $scope.IsNotCandidateList = false;
    $scope.IsCandidateList = false;
    $scope.btnExportdatval = "ElectedList";
    $scope.IsSave = false;
    $scope.StepTypeId = 9;
    $scope.selectCandidateType = function () {


        // alert($("input[name='candidatelist']:checked").val());
        $scope.IsElectedList = false;
        $scope.IsSelectedList = false;
        $scope.IsNotCandidateList = false;
        $scope.IsCandidateList = false;
       // debugger;
        var SearchText = $("input[name='candidatelist']:checked").val();
        if (SearchText == "Elected") {
            //$("#btnExport").attr("dat-val", "ElectedList");
            $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId, 9);
            $scope.IsElectedList = true; 
            $scope.btnExportdatval = "ElectedList";
            $scope.StepTypeId = 9;
        }
        else if (SearchText == "Selected") {
            //$("#btnExport").attr("dat-val", "CandidateList");
            $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId, 11);
            $scope.IsSelectedList = true; 
            $scope.btnExportdatval = "CandidateList";
            $scope.StepTypeId = 11;
        } else if (SearchText == "Reject") {
            //$("#btnExport").attr("dat-val", "RejectedCandidateList");
            $scope.IsNotCandidateList = true; 
            $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId, 10);
            $scope.StepTypeId = 10;
        }
        else if (SearchText == "Name Withdrawal") {
            //$("#btnExport").attr("dat-val", "WithdrawalCandidateList");
            $scope.IsNotCandidateList = true; 
            $scope.btnExportdatval = "WithdrawalCandidateList";
            $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId, 8);
            $scope.StepTypeId = 8;
        } else if (SearchText == "Candidate") {
            //$("#btnExport").attr("dat-val", "ResultList");
            $scope.IsCandidateList = true;
            $scope.btnExportdatval = "ResultList";
            $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId, 12);
            $scope.StepTypeId = 12;
        }



        // $("#myTable2").html("");
        // $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);
        //let data = $scope.EMemberDetailsData.find(ob => ob['AadharNo2'] === $scope.AddNewMember.AadharNo);
        var srno1 = 1;
        $scope.EMemberDetailsData.forEach(function (itm, idnx) {
            if (SearchText == itm.ActionName) {
                itm.IsShow = true;
                itm.SrNo = srno1++;
            }
            else {
                itm.IsShow = false;
            }
        });
        $scope.TotalMembers = $scope.EMemberDetailsData.filter(x => x.IsShow == true).length;

    }
    
    $http.get(SiteUrl + "/Election/GetNominationActions").
        success(function (data, status) {
            $scope.NominationActions = data;
            //  $scope.EditBasicDetailsForm(1);
        }).
        error(function (data, status) {
            alert("Error while fetching Post list")

        });
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
        debugger;
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




    $scope.SendOTPForIssueCirtificate = function (SocietyIdEnc, ElectionMasterId, MemberId,index) {
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
                debugger;
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
        debugger;
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/GenerateElectedCandidatePDF", { "OTP": $scope.OTP, "TXN": $scope.Certtrn, "SocietyIdEnc": $scope.CertSocietyIdEnc, "ElectionMasterId": $scope.CertElectionMasterId, "MemberId": $scope.CertMemberId, "StepType": 12 }).
                success(function (data, status) {
                    debugger;
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        $scope.EMemberDetailsData[$scope.CertIndex].OfficerCirtificate = "OfficerCertificate_" + $scope.CertElectionMasterId + "_" + $scope.CertMemberId + ".pdf";
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

    $scope.EditBasicDetailsForm = function (SocietyIdEnc, ElectionMasterId, StepType) {
        //  alert(1);
        //  alert(SocietyIdEnc)
        $scope.ShowSocietyList = false;
        $scope.ElectionMasterId = ElectionMasterId;
        $scope.ShowVoterList = true;
        $scope.SocietyIdEnc = SocietyIdEnc;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetVoterMemberDetailsBySocietyId", { "SocietyId": $scope.SocietyIdEnc, "IsCEO": false, "ElectionMasterId": ElectionMasterId, StepType: StepType }).
            success(function (data, status) {
                // var MemberDetails = [];
                var EMemberDetails = [];
                EMemberDetails = data;


                $scope.EMemberDetailsData = data;//.filter(x => x.PostId>0);
                $scope.AddCalender();
               // $scope.IsSubmit = false;
                $scope.IsValid = true;
                $scope.EMemberDetailsData.forEach(function (item, indx) {
                   // debugger;
                    item.IsCandidate = item.IsCandidate.toLowerCase() == 'true' ? true : false;
                    item.IsSubmit = item.IsSubmit.toLowerCase() == 'true' ? true : false;
                    item.IsValid = item.IsValid.toLowerCase() == 'true' ? true : false;
                    item.IsSubmitCandidate = item.IsSubmitCandidate.toLowerCase() == 'true' ? true : false;
                    item.IsWinner = item.IsWinner.toLowerCase() == 'true' ? true : false;
                    item.IsSave = item.IsSave.toLowerCase() == 'true' ? true : false;
                    if (indx == 0) {
                        $scope.SocietyDirectory = item.SocietyDirectory;
                        $scope.IsSubmit = item.IsSubmit;
                        $scope.LastSubmistionDate = item.LastDate + ' ' + item.StartTime + (item.LastTime != '' ? ' to ' : '') + item.LastTime;
                        $scope.IsSave = item.IsSave;
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

                if ($scope.IsElectedList) {
                    $scope.ElectionOfficials = $scope._language == 'en-us' ? "The Post of the elected candidates have been selected." : "निर्वाचित उम्मीदवारों के पदों का चयन कर लिया गया है"
                }
                else if ($scope.IsSelectedList) {
                    $scope.ElectionOfficials = $scope._language == 'en-us' ? "The list of candidates' posts has been published." : "उम्मीदवारों के पदों की सूची का प्रकाशन कर दिया गया है"
                } else {
                    $scope.ElectionOfficials = "";
                }
               

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

    $scope.SaveSocietyAdministrativeDetails = function (IsSubmit) {
        if ($scope.TableValiDate(3)) {
            //if (confirm("You can't update data after submit")) {
            var SocietyCandidateData = [];
            var formData = new FormData();
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            debugger;

            if (IsSubmit) {
                var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
                if (!confirm(message)) {
                    return;
                }
            }
            $(".blockUI").fadeIn("slow");
            //formData.append("IsSubmit", IsSubmit);
            var imgindx = 0;
            $("#myTable2").find('input[type="file"]').each(function (indx) {
                //alert($(this)[0].files.length);
                debugger;
              
                    var fileUpload = $(this).get(0);
                    var files = fileUpload.files;


                    formData.append("BODPDFForm_" + (imgindx + 1), files[0]);
                    ++imgindx;
               

            });
                       
            formData.append("StepType", $scope.StepTypeId);   
            //alert($scope.ElectionMasterId);
            // alert($scope.SocietyDirectory);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.EMemberDetailsData.forEach(function (item, index) {
                if ($scope.EMemberDetailsData[index].IsCandidate && $scope.EMemberDetailsData[index].IsCandidate != 'False') {

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
                            "PostId": item.PostId,
                            "UserType": "5",//Nominated member
                            "AdministratorDesignationId": "-1",
                            "IsCEO": false,
                            "RowNO": index,
                            "Photo": item.Photo,
                            "FHName": item.FHName,
                            "Address": item.Address,
                            "BODPDFForm": item.BODPDFForm == undefined ? "" : item.BODPDFForm,
                            "TotalVotes": item.TotalVotes,
                            "IsWinner": item.IsWinner,
                            "ResultRemark": item.ResultRemark,
                            "ResultDocument": item.ResultDocument,
                            "IsPostSubmited": IsSubmit,
                            "StepType": $scope.StepTypeId,
                            "ActionOnCandidate": item.ActionOnCandidate,
                            "ActionRemark": item.ActionRemark,
                            "ActionDocument": item.ActionDocument,
                            "ActionDate": item.ActionDate,
                            "OfficerSymbol": item.OfficerSymbol
                        });
                }
                //SocietyCandidateData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
            });
            //alert(JSON.stringify(SocietyCandidateData));
            formData.append("DataArray", JSON.stringify(SocietyCandidateData));
            $http.post(SiteUrl + "/Election/UpdateSocietyMemberDetails", formData,
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
    $scope.InsertUpdateElecionPost = function (IsSubmit) {
        debugger;
        if ($scope.IsCandidateList) {
            $scope.InsertUpdateElecionPostResult(IsSubmit);
            return;
        }
        if ($scope.TableValiDate(3)) {
            var SocietyCandidateData = [];
            var formData = new FormData();
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            debugger;

            if (IsSubmit) {
                var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
                if (!confirm(message)) {
                    return;
                }
            }

            if ($scope.IsSelectedList) {              
                formData.append("IsPublish", IsSubmit);
            }

            $(".blockUI").fadeIn("slow");
             var imgindx = 0;
            $("#myTable2").find('input[type="file"]').each(function (indx) {              
                debugger;
                var fileUpload = $(this).get(0);
                var files = fileUpload.files;
                formData.append("BODPDFForm_" + (imgindx + 1), files[0]);
                ++imgindx;
            });
            for (var Y = 0; Y < PDFDataList.length; Y++) {
                //for (; tempRowNo < PDFDataList[Y].RowNo; tempRowNo++) {
                //    formData.append("MemberTempPDFFile" + (tempRowNo + Count), PDFDataList[Y].File);
                //}
                //formData.append("MemberPDFFile" + (PDFDataList[Y].RowNo + Count), PDFDataList[Y].File);
                formData.append("MemberActionPDFFile_" + (PDFDataList[Y].RowNo) + "_" + PDFDataList[Y].Type, PDFDataList[Y].File);
            }
            formData.append("StepType", $scope.StepTypeId);        
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.EMemberDetailsData.forEach(function (item, index) {
                if (item.PostId != '0') {
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
                            "PostId": item.PostId,
                            "UserType": "5",//Nominated member
                            "AdministratorDesignationId": "-1",
                            "IsCEO": false,
                            "RowNO": index,
                            "Photo": item.Photo,
                            "FHName": item.FHName,
                            "Address": item.Address,
                            "BODPDFForm": item.BODPDFForm == undefined ? "" : item.BODPDFForm,
                            "TotalVotes": item.TotalVotes,
                            "IsWinner": item.IsWinner,
                            "ResultRemark": item.ResultRemark,
                            "ResultDocument": item.ResultDocument,
                            "IsPostSubmited": IsSubmit,
                            "StepType": $scope.StepTypeId,
                            "ActionOnCandidate": item.ActionOnCandidate,
                            "ActionRemark": item.ActionRemark,
                            "ActionDocument": item.ActionDocument,
                            "ActionDate": item.ActionDate,
                            "OfficerSymbol": item.OfficerSymbol
                        });
                }
                //SocietyCandidateData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
            });
            //alert(JSON.stringify(SocietyCandidateData));
            formData.append("DataArray", JSON.stringify(SocietyCandidateData));
            $http.post(SiteUrl + "/Election/InsertUpdateElecionPost", formData,
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
    $scope.InsertUpdateElecionPostResult = function (IsSubmit) {
        if ($scope.TableValiDate3(3)) {
            //if (confirm("You can't update data after submit")) {
            var SocietyCandidateData = [];
            var formData = new FormData();
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            debugger;

            if (IsSubmit) {
                var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
                if (!confirm(message)) {
                    return;
                }
            }
            $(".blockUI").fadeIn("slow");
            formData.append("StepType", 12);
            formData.append("IsSubmit", IsSubmit);
            var imgindx = 0;
            $("#myTable2").find('input[type="file"]').each(function (indx) {

                debugger;

                var fileUpload = $(this).get(0);
                var files = fileUpload.files;


                formData.append("ResultPDFFile_" + (imgindx + 1), files[0]);
                ++imgindx;


            });






            //alert($scope.ElectionMasterId);
            // alert($scope.SocietyDirectory);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.EMemberDetailsData.forEach(function (item, index) {

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
                            "IsWinner": item.IsWinner,
                            "ResultRemark": item.ResultRemark,
                            "ResultDocument": item.ResultDocument,
                            "StepType": 12,
                            "Type": 2,
                            "PostId": item.PostId
                        });
              
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

    $scope.ModifyMember = function () {
        debugger;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#MemberEditPDFForm1").removeClass("notValid");
        $("#ddrActionType").removeClass("notValid");
        $("#MemberDeleteRemarks").removeClass("notValid");
        // alert($("#ddrActionType").val());
        if ($("#ddrActionType").val() == '0' || $("#ddrActionType").val() == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Option for Edit.' : '\n <br /> कृपया संपादन के लिए विकल्प चुनें |';
            $("#ddrActionType").addClass("notValid");
        }

        if ($("#MemberEditPDFForm1").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select pdf for Edit Candidate.' : '\n <br /> कृपया उम्मीदवार को संपादित करने के लिए पीडीएफ का चयन करें |';
            $("#MemberEditPDFForm1").addClass("notValid");
        }
        if ($scope.AddNewMember.ActionRemark == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter remark.' : '\n <br /> कृपया टिप्पणी दर्ज करें |';
            $("#MemberDeleteRemarks").addClass("notValid");
        }

        //if ($scope.EMemberDetailsData.length = index + 1) {
        if (message.length > 29) {
            ShowModelAlert("Add Member", message);

        } else {
            debugger;
            $scope.EMemberDetailsData[$scope.AddNewMember.Index].ActionOnCandidate = $scope.AddNewMember.ActionType;
            $scope.EMemberDetailsData[$scope.AddNewMember.Index].ActionRemark = $scope.AddNewMember.ActionRemark;
            $scope.EMemberDetailsData[$scope.AddNewMember.Index].Delete = false;

            if ($("#MemberEditPDFForm1")[0].files.length > 0) {
                $scope.ElUploadPDF($("#MemberEditPDFForm1"), $scope.AddNewMember.Index + 1, "", $scope.AddNewMember.Index + 1);
            }
            var srno = 1;
            $scope.EMemberDetailsData.forEach(function (itm, idnx) {
                itm.IsShow = true;
                if (itm.Delete)
                    itm.SrNo = srno++;
            });
            $("#deleteModal").modal("hide");
        }
    }

    $scope.TableValiDate = function (tableflag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        
        $("#myTable2").find('select').removeClass("notValid");
        $("#myTable2").find('input[type="file"]').removeClass("notValid");


        if ($scope.EMemberDetailsData.filter(x => x.PostId == '0').length == $scope.EMemberDetailsData.length) {
            message += $scope._language == 'en-us' ? '\n <br />Please select Post.' : '\n <br />कृपया पोस्ट का चयन करें।';
        }
        if ($scope.StepTypeId == 11) {
            $scope.EMemberDetailsData.forEach(function (item, index) {
                if (item.OfficerSymbol == '' && !item.IsDelete) {
                    message += $scope._language == 'en-us' ? '\n <br />RowNo-' + (index + 1) + ', Please Select Candidate Election Symbol.' : '\n <br />RowNo-' + (index + 1) + ', कृपया उम्मीदवार के चुनाव चिन्ह का चयन करें ।';
                }
            });
        } else {
            $scope.EMemberDetailsData.forEach(function (item, index) {
                if ($("#myTable2").find('input[type="file"]').eq(index).val() == "" && item.BODPDFForm == '' && item.PostId != '0') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please upload Document' : 'कृपया फॉर्म अपलोड करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please upload Document.' : '\n <br />कृपया फॉर्म अपलोड करें।';
                    }
                    $("#myTable2").find('input[type="file"]').eq(index).addClass("notValid");
                }

            });
        }
        if (message.length > 29) {
            ShowModelAlert("Candidate List", message);
            return false;
        }
        return true;
    }

    $scope.TableValiDate3 = function (tableflag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';



        //$("#myTable2").find('input[type="file"]').removeClass("notValid");
        //if ($("#myTable2").find('input[type="checkbox"]:checked').length == 0) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Candidate.' : '\n <br />कृपया उम्मीदवार का चयन करें।';
        //}
        $("#myTable3").find('input[type="text"]').removeClass("notValid");
        $("#myTable3").find('input[type="file"]').removeClass("notValid");


        if ($("#myTable3").find('input[type="checkbox"]:checked').length == 0) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Check Result' : 'कृपया रिजल्ट चेक करें') < 0) {
                message += $scope._language == 'en-us' ? '\n <br />Please Check Result.' : '\n <br />कृपया रिजल्ट चेक करें।';
            }
            // $("#myTable2").find('input[type="file"]').eq(index).addClass("notValid");
        }


        $scope.EMemberDetailsData.forEach(function (item, index) {
            //if (item.IsCandidate && item.IsCandidate != 'False') {


            if (item.TotalVotes == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Number of Vote' : 'कृपया वोट की संख्या दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '\n <br />Please Enter Number of Vote.' : '\n <br />कृपया वोट की संख्या दर्ज करें।';
                }
                $("#myTable3").find('input[type="text"]').eq(index).addClass("notValid");
            }


            if ($("#myTable3").find('input[type="file"]').eq(index).val() == "" && item.ResultDocument == '' && item.IsWinner) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please upload Document' : 'कृपया मतगणना पत्र अपलोड करें') < 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please upload Document.' : '\n <br />कृपया मतगणना पत्र अपलोड करें।';
                }
                $("#myTable3").find('input[type="file"]').eq(index).addClass("notValid");
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
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        $scope.ClearClassForm = function () {
            $scope.Classes = '';
        };
        $('#txt_DOB').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D',
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
            yearRange: "-100:-18"
        });

        //Get Chairman Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        //Get Chairman Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.ChairmanPosts = data;
                //  $scope.EditBasicDetailsForm(1);
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown


        //Get Board Member Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-2").
            success(function (data, status) {
                $scope.BoardPosts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown

        $(".se-pre-con").fadeOut("slow");
        // END : Post List for dropdown

        $scope.GetElectionSocietyListData();
    }


    $scope.GetElectionSocietyListData = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Election/GetElectionSocietyListData_New", JSON.stringify({ Details: jsonParameters, StepType: 9 })).
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

    $scope.ElUploadPDF = function (file, row, type, index) {
        //  alert(JSON.stringify(PDFDataList));
        debugger;
        //for (var temRowNo1 = 0; temRowNo1 < PDFDataList.length; temRowNo1++) {
        //    if (PDFDataList[temRowNo1].RowNo == row) {
        //        PDFDataList.splice(temRowNo1);
        //    }
        //}

        var fileUpload = $(file).get(0);
        var files = fileUpload.files;



        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = files[0];// file[0];
        FileDataWithRowObj.RowNo = row;
        FileDataWithRowObj.Type = type;
        FileDataWithRowObj.Index = index;
        PDFDataList.push(FileDataWithRowObj);
        // alert(JSON.stringify(PDFDataList));
    }
    $scope.RemoveRowAdminiAct2001 = function (index) {
        //$scope.BulkInsertFile = file[0];
        // alert(1);
        $scope.ReSetAddModel();
        $("#deleteModal").modal("show");

        $scope.AddNewMember.Index = index;
    };
    $scope.OnPageLoad();
    $scope.AssignElectionSymbol = function (memberId, index) {
        $("#addElectionSymbolModal").modal('show');
        $("#candidateIndex").val(index);
        $scope.GetAllElectionSymbol(memberId);
    }
    $scope.ElectionSymbolsList = [];
    $scope.GetAllElectionSymbol = function (memberId) {
        $scope.ShowSocietyList = false;
        $scope.ShowVoterList = true;

        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetCandidateElectionSymbol", { ElectionMasterId: $scope.ElectionMasterId, CandidateId: memberId, Type: 1 }).
            success(function (data, status) {
                debugger;
                $scope.ElectionSymbolsList = data;
                $scope.ElectionSymbolsList.forEach(function (item, indx) {
                    debugger;
                    item.IsSelected = item.IsSelected.toLowerCase() == 'true' ? true : false;
                    item.IsShow = item.IsShow.toLowerCase() == 'true' ? true : false;
                    $scope.EMemberDetailsData.forEach(function (itm, idx) {
                        debugger;
                        if (item.symbol_url == itm.OfficerSymbol) {
                            item.IsShow = false;
                        }
                    });
                });
                $scope.TotalSymbols = $scope.ElectionSymbolsList.length;
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                $(".blockUI").fadeOut("slow");
            });

    }

    $scope.AddSymbole = function () {
        var cIndex = $("#candidateIndex").val();
        var index = 0;
        $scope.ElectionSymbolsList.forEach(function (item, indx) {
            debugger;
            if (item.IsSelected) {
                index = indx;
                item.IsShow = false;
            }
        });
        $scope.EMemberDetailsData[cIndex].OfficerSymbol = $scope.ElectionSymbolsList[index].symbol_url;
        $("#addElectionSymbolModal").modal('hide');
    }

    $scope.ChangeSymbol = function (index) {
        $scope.ElectionSymbolsList.forEach(function (item, indx) {
            if (index != indx) {
                item.IsSelected = false;
            }
        });
    }
});

AppClasses.filter('split', function () {
    return function (input, splitChar, splitIndex) {
        // do some bounds checking here to ensure it has that index
        return input.split(splitChar)[splitIndex];
    }
});
//{ { test | split: ',': 0 } }
//{ { test | split: ',': 0 } }