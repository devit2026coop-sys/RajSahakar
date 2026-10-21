var AppClasses = angular.module("CourtModuleMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("CourtModuleCnt", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPage: 1,
        pageSize: 50,
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


        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
    }


    $scope.OnPageLoad = function () {
        $('#From_Time').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
        });
        $scope.GetAllSubUnitList();
        //$scope.GetApprovedSocietyList();
        $scope.GetApprovedSocietyAssignForIO();
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
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetWarehouseList();
    }
    /*------------End Paging and Filter-----------------*/
    $scope._language = 'en-us';
    $scope._language = _language;
    if (_language == 'en-us') {
        $http.get("../Translations/Translation_EN.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
    }
    else {
        $http.get("../Translations/Translation_HI.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });

    }

    $scope.SocietyId = "";
    $scope.StatusId = "";
    $scope.Complain_ID = "";

    $scope.AddNew = function (societyId) {
        $scope.SocietyId = societyId;
       // $("#addModal").modal("show");
        $scope.showPriView = true;

    }
    $scope.SocietyInfo = [];
    $scope.ResetAddNew = function () {
        $scope.SocietyId = 0;
        $scope.StatusId = 0;
        $scope.Complain_ID = 0;
        $("#HtmlContent").summernote("code", "");
        $scope.ComplainTrail.Remark = "";
        $("#ComplaintDate").val("");
        $scope.showPriView = false;
        $scope.ComplainUndeVlaue.AnyOtherPerson = false;
    }
    $scope.AddNewIO = function (item) {
        $scope.SocietyId = item.SocietyId;
        $scope.StatusId = item.StatusId
        $scope.Complain_ID = item.ComplainID;
        // $("#addModal").modal("show");
        $scope.showPriView = true;
        $scope.SocietyInfo = item;
    }


    $scope.ClearModel = function () {

        $("#HtmlContent").val("");
        $("#ComplaintDate").val('');
        $scope.ComplainTrail.FileName = "";
        $scope.ComplainTrail.IsUploadFileName = false;
        $scope.ComplainTrail.Remark = "";
    }

    $scope.ReadSubmitedData = function (itm) {
        $(".se-pre-con").fadeIn("slow");
        $scope.purposeList = false;
        $scope.addPurpose = true;
        $("#addModal").modal("show");
      
        $("#HtmlContent").val(itm.IODetails);
        //$scope.MasterModel.effective_date = itm.IOComplainLetter;
      
        //$scope.MasterModel.sector_id = itm.sector_id.toString();
        $(".blockUI").fadeOut("slow");
    };


    //------------------Start Complainant Details -----------------------------//




    $scope.ComplainantDetails = {
        CDetailID: "0",
        Name: "",
        MobileNo: "",
        Address: "",
        Post: "",
        IsSocietyMember: "",
        Ctype: "",
        IsActive: "",
        IsDelete: false
    }

    $scope.ResetComplainantDetails = function () {
        $scope.ComplainantDetails = {
            CDetailID: "0",
            Name: "",
            MobileNo: "",
            Address: "",
            Post: "",
            IsSocietyMember: "",
            Ctype: "",
            IsActive: "",
            IsDelete: false
        }
    }
    $scope.ComplainantDetailsList = [];
    $scope.ComplainantDetailsList.push($scope.ComplainantDetails);

    $scope.AddComplainantDetails = function () {
        if ($scope.ValidateComplainantDetails()) {
            $scope.ResetComplainantDetails();

            $scope.ComplainantDetailsList.push($scope.ComplainantDetails);
        }
    }

    $scope.RemoveComplainantDetailsRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से राशि विवरण निकालना चाहते हैं', function () {
            $(".blockUI").fadeOut("slow");
            $scope.ComplainantDetailsList[index].IsDelete = true;
            $scope.$apply();
            $(".blockUI").fadeOut("slow");
        });
    }

    $scope.ValidateComplainantDetails = function () {
        var message = "";// $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblComplainantDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblComplainantDetails").find('select').removeClass("notValid");

        $scope.ComplainantDetailsList.forEach(function (item, index) {

            if ($scope.ComplainUnderSection.ComplantBy == '2') {
                if (item.Name == '') {

                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter a Name' : 'कृपया नाम दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? 'कृपया नाम दर्ज करें।' : 'कृपया नाम दर्ज करें।';
                    }
                    $("#txtName").addClass("notValid");
                }
                else {
                    $("#txtName").removeClass("notValid");
                }

                if (item.Address == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter an Address' : 'कृपया पता दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया पता दर्ज करें।' : '<br />कृपया पता दर्ज करें।';
                    }
                    $("#Address").addClass("notValid");
                }
                else {
                    $("#Address").removeClass("notValid");
                }

                if (item.MobileNo == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Mobile Number' : 'कृपया मोबाइल नंबर दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया मोबाइल नंबर दर्ज करें।' : '<br />कृपया मोबाइल नंबर दर्ज करें।';
                    }
                    $("#txtMobile").addClass("notValid");
                }
                else {
                    $("#txtMobile").removeClass("notValid");
                }

                if (item.IsSocietyMember == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'This person is a member of the institution / committee? Choose' : 'यह व्यक्ति संस्था/समिति का सदस्य है? चुनें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />यह व्यक्ति संस्था/समिति का सदस्य है? चुनें।' : '<br />यह व्यक्ति संस्था/समिति का सदस्य है? चुनें।';
                    }
                }
            }

        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }




    //------------------End Complainant Details -----------------------------//







    //------------------Start Details of the complaint against -----------------------------//




    $scope.ComplaintAgainst = {

        CDetailID: "0",
        Name: "",
        MobileNo: "",
        Address: "",
        Post: "",
        IsSocietyMember: "",
        Ctype: "",
        IsActive: "",
        IsDelete: false
    }

    $scope.ResetComplaintAgainst = function () {
        $scope.ComplaintAgainst = {
            CDetailID: "0",
            Name: "",
            MobileNo: "",
            Address: "",
            Post: "",
            IsSocietyMember: "",
            Ctype: "",
            IsActive: "",
            IsDelete: false
        }
    }
    $scope.ComplaintAgainstList = [];
    $scope.ComplaintAgainstList.push($scope.ComplaintAgainst);

    $scope.AddComplaintAgainst = function () {
        if ($scope.ValidateComplaintAgainst()) {
            $scope.ResetComplaintAgainst();

            $scope.ComplaintAgainstList.push($scope.ComplaintAgainst);
        }
    }

    $scope.RemoveComplaintAgainstRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से राशि विवरण निकालना चाहते हैं', function () {
            $scope.ComplaintAgainstList[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.ValidateComplaintAgainst = function () {
        var message = "";// $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblComplainantDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblComplainantDetails").find('select').removeClass("notValid");

        $scope.ComplaintAgainstList.forEach(function (item, index) {

            if ($scope.ComplainUnderSection.IsComplentForSociety == '2') {
                if (item.Name == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'जिसके खिलाफ शिकायत की गई है कृपया उसका नाम दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका नाम दर्ज करें।' : '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका नाम दर्ज करें।';
                    }
                    $("#txtSocietyname").addClass("notValid");
                }
                else {
                    $("#txtSocietyname").removeClass("notValid");
                }

                if (item.Address == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'जिसके खिलाफ शिकायत की गई है कृपया उसका पता दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पता दर्ज करें।' : '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पता दर्ज करें।';
                    }
                    $("#txtAddress").addClass("notValid");
                }
                else {
                    $("#txtAddress").removeClass("notValid");
                }

                if (item.Post == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'जिसके खिलाफ शिकायत की गई है कृपया उसका पद दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पद दर्ज करें।' : '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पद दर्ज करें।';
                    }
                    $("#txtPost").addClass("notValid");
                }
                else {
                    $("#txtPost").removeClass("notValid");
                }
            }

        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }




    //------------------End Details of the complaint against -----------------------------//



    $scope.ComplainUnderSection = {

        ComplainID: "0",
        SocietyId: "",
        //SSOID: "",
        // UserID: "",
        ComplantBy: "",
        IsComplentForSociety: "",
        ComplaintDetails: "",
        StatusId: "",
        ComplainDate: "",
        IsActive: "",
        // CreatedBy: "",
        CreationDate: "0",
        //UpdatedBy: "0",
        UpdationDate: "",
        CreatedIP: "",
        UpdatedIP: ""

    }


    //$scope.FileUpload = "";
    //$scope.ComplaintDate = "";
    //$scope.IO_Name = "";
    //$scope.MemberType = "";
    //$scope.SocietyType = "";


    $scope.ComplainUnderSection = [];
    $scope.ComplainUnderSection.push();
    $scope.validateComplainUnderSection = function () {

        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");



        if ($scope.ComplainUnderSection.ComplantBy == '' || $scope.ComplainUnderSection.ComplantBy == undefined) {

            if (message.indexOf($scope._language == 'en-us' ? 'Please choose the complainant.' : 'कृपया शिकायतकर्ता चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता चुनें।' : '<br />कृपया शिकायतकर्ता चुनें।';
            }
        }

        if ($scope.ComplainUnderSection.IsComplentForSociety == '' || $scope.ComplainUnderSection.IsComplentForSociety == undefined) {

            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Small Industry Registration' : 'जिसके लिए शिकायत है| चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />जिसके लिए शिकायत है| चुनें।' : '<br />जिसके लिए शिकायत है| चुनें।';
            }
        }

        if ($scope.ComplainTrail.FileName == '' && $scope.ComplainTrail.IsUploadFileName == false || $scope.ComplainTrail.FileName == undefined ) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया फाइल अपलोड करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया फाइल अपलोड करें।' : '<br />कृपया फाइल अपलोड करें।';
            }
        }

        if ($("#ComplainDate").val() == '' && $("#ComplainDate").val() == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
            }
            $("#ComplaintDate").addClass("notValid");
        }
        else {
            $("#ComplaintDate").removeClass("notValid");
        }

        if ($scope.IO_Name == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया जांच अधिकारी का नाम चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया जांच अधिकारी का नाम चुनें।' : '<br />कृपया जांच अधिकारी का नाम चुनें।';
            }
            $("#ddlIO_Name").addClass("notValid");
        }
        else {
            $("#ddlIO_Name").removeClass("notValid");
        }

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }



    $scope.IODetail = {
        IODetailID: "0",
        ComplainID: "",
        SSOID: "",
        UserId: "",
        IsActive: ""

    }

    $scope.ComplainTrail = {
        ComplainTrailID: "0",
        ComplainID: "",
        ComplaintStatusId: "",
        FileName: "",
        Remark: "",
        IsActive: ""

    }





    //------------------Start Save User-----------------------------//


    $scope.SaveUser = function (TransId) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();


        var ComplainantDetailsList = [];
        $scope.ComplainantDetailsList.forEach(function (itm) {
            ComplainantDetailsList.push({
                CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
                Name: itm.Name,
                MobileNo: itm.MobileNo,
                Address: itm.Address,
                Post: itm.Post,
                IsSocietyMember: itm.IsSocietyMember == "1" ? true : false,
                Ctype: true,
                IsActive: itm.IsActive,
                IsDelete: itm.IsDelete
            });
        });



        var ComplaintAgainstList = [];
        $scope.ComplaintAgainstList.forEach(function (itm) {
            ComplaintAgainstList.push({
                CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
                Name: itm.Name,
                Address: itm.Address,
                Post: itm.Post,
                Ctype: false,
                IsActive: itm.IsActive == 'True' ? 1 : 0,
                IsDelete: itm.IsDelete
            });
        });



        var IODetailList = [];
        IODetailList.push({
            IODetailID: $scope.IODetail.IODetailID == undefined ? 0 : $scope.IODetail.IODetailID,
            UserId: $scope.IODetail.UserId,
            IsActive: $scope.IODetail.IsActive
        });

        var ComplainTrail = [];
        ComplainTrail.push({
            ComplainTrailID: $scope.ComplainTrail.ComplainTrailID == undefined ? 0 : $scope.ComplainTrail.ComplainTrailID,
            // ComplainID: $scope.IODetail.ComplainID,  
            ComplaintStatusId: $scope.ComplainTrail.ComplaintStatusId,
            FileName: $scope.ComplainTrail.FileName,
            Remark: $scope.ComplainTrail.Remark,
            IsActive: $scope.ComplainTrail.IsActive
        });

        var ComplainDate = $("#ComplaintDate").val();
        var complainDetails = $("#HtmlContent").val();
        var SocietyId = $("#SocietyID").val();
        var applicationDetails = {

            ComplainID: $scope.ComplainUnderSection.ComplainID == undefined ? 0 : $scope.ComplainUnderSection.ComplainID,
            SocietyId: $scope.SocietyId,
            ComplantBy: $scope.ComplainUnderSection.ComplantBy == "2" ? true : false,
            //SSOID: $scope.SSOID,
            // UserID: $scope.UserID,
            IsComplentForSociety: $scope.ComplainUnderSection.IsComplentForSociety == "2" ? true : false,
            ComplaintDetails: $("#HtmlContent").val() != "" || $("#HtmlContent").val() != null ? $("#HtmlContent").val() : "",//$scope.ComplainUnderSection.ComplaintDetails,
            StatusId: $scope.ComplainUnderSection.StatusId,
            ComplainDate: ComplainDate,
            IsActive: $scope.ComplainUnderSection.IsActive,

            ComplainantDetailsList: ComplainantDetailsList,
            ComplaintAgainstList: ComplaintAgainstList,
            IODetailList: IODetailList,
            ComplainTrailList: ComplainTrail,
            TXN: TransId

        }
        formData.append("Applicationdata", JSON.stringify(applicationDetails));
        formData.append("ComplainantDetailsList", JSON.stringify($scope.ComplainantDetailsList));
        formData.append("IODetailList", JSON.stringify($scope.IODetailList));
        formData.append("ComplainTrailList", JSON.stringify($scope.ComplainTrailList));

        $http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveComplainUnderSection", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
           // $("#addModal").modal("hide");
            if (data.Result != "-1") {
                ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                   // $("#addModal").modal("hide");
                    window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintCourtModule";
                });
            } else {
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                });
            }
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                });
            });

    };

    $scope.SaveComplaintAgainstSociety = function () {
        if ($scope.validateComplainUnderSection()) {
             if ($scope.ValidateComplainantDetails()) {
                 if ($scope.ValidateComplaintAgainst()) {
        $("#addModal").modal("hide");
        AadhaarAuth("", $http, "OTP", true, function (data) {
            $("#addModal").modal("show");
         $scope.SaveUser(data.TransId);
          });
                }
            }
        }

       // $scope.SaveUser();
    }






    //------------------End Save User -----------------------------//








    //------------------Start Save IO Details-----------------------------//
    
    $scope.IODetails = [];
    $scope.IODetails.push();
    $scope.validateIODetails = function () {

        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");


        if ($("#HtmlContent").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|.' : '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|';
            }
            $("#HtmlContent").addClass("notValid");
        } else {
            $("#HtmlContent").removeClass("notValid");
        }


        if ($("#ComplaintDate").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
            }
            $("#ComplaintDate").addClass("notValid");
        }
        else {
            $("#ComplaintDate").removeClass("notValid");
        }

        if ($scope.ComplainTrail.Places == '' || $scope.ComplainTrail.Places == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया स्थान दर्ज करें' : 'कृपया स्थान दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया स्थान दर्ज  करें।' : '<br />कृपया स्थान दर्ज करें।';
            }
            $("#Place").addClass("notValid");
        }
        else {
            $("#Place").removeClass("notValid");
        }


        if ($scope.ComplainTrail.Remark == '' || $scope.ComplainTrail.Remark == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया टिप्पणी दर्ज करें' : 'कृपया टिप्पणी दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया टिप्पणी दर्ज  करें।' : '<br />कृपया टिप्पणी दर्ज करें।';
            }
            $("#Remark").addClass("notValid");
        }
        else {
            $("#Remark").removeClass("notValid");
        }

        if (($("#Time_HHs").val() == '' || $("#Time_HHs").val() == undefined) && ($("#Time_MMs").val() == '' || $("#Time_MMs").val()== undefined)) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया समय दर्ज करें' : 'कृपया समय दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया समय दर्ज  करें।' : '<br />कृपया समय दर्ज करें।';
            }
            $("#Time_HHs").addClass("notValid");
            $("#Time_MMs").addClass("notValid");
        }
        else {
            $("#Time_HHs").removeClass("notValid");
            $("#Time_MMs").removeClass("notValid");
        }


        //if ($("#Time_MMs").val() == '' || $("#Time_MMs").val() == '' == undefined) {
        //    if (message.indexOf($scope._language == 'en-us' ? 'कृपया समय दर्ज करें' : 'कृपया समय दर्ज करें') < 0) {
        //        message += $scope._language == 'en-us' ? '<br />कृपया समय दर्ज  करें।' : '<br />कृपया समय दर्ज करें।';
        //    }
        //    $("#Time_MMs").addClass("notValid");
        //}
        //else {
        //    $("#Time_MMs").removeClass("notValid");
        //}

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }


    $scope.SaveIODetails = function (TransId) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();

        var IODetailList = [];
        IODetailList.push({
            IODetailID: $scope.IODetail.IODetailID == undefined ? 0 : $scope.IODetail.IODetailID,
            UserId: $scope.IODetail.UserId,
            IsActive: $scope.IODetail.IsActive,
            IODetails: $("#HtmlContent").val() != "" || $("#HtmlContent").val() != null ? $("#HtmlContent").val() : "",
            Places: $scope.ComplainTrail.Places,
            DateTime: $("#ComplaintDate").val() + " " + $("#Time_HHs").val() + ":" + $("#Time_MMs").val() + " " + $("#Time_APs").val()
        });

        var ComplainTrail = [];
        ComplainTrail.push({
            ComplainTrailID: $scope.ComplainTrail.ComplainTrailID == undefined ? 0 : $scope.ComplainTrail.ComplainTrailID,
            // ComplainID: $scope.IODetail.ComplainID,  
            ComplaintStatusId: $scope.StatusId,
            FileName: $scope.ComplainTrail.FileName,
            Remark: $scope.ComplainTrail.Remark,
            IsActive: $scope.ComplainTrail.IsActive
        });

        var ComplainDate = $("#ComplaintDate").val();
        var complainDetails = $("#HtmlContent").val();
        var SocietyId = $("#SocietyID").val();
        var applicationDetails = {

            ComplainID: $scope.Complain_ID,
            IODetailList: IODetailList,
            ComplainTrailList: ComplainTrail,
            TXN: TransId,
            SocietyName: ($scope.SocietyInfo.ClassId == 6 ? "गृह निर्माण सहकारी समिति " : "") + $scope.SocietyInfo.SocietyName + " " + $scope.SocietyInfo.YognaName,
        }
        formData.append("Applicationdata", JSON.stringify(applicationDetails));
        formData.append("ComplainantDetailsList", JSON.stringify($scope.ComplainantDetailsList));
        formData.append("IODetailList", JSON.stringify($scope.IODetailList));
        formData.append("ComplainTrailList", JSON.stringify($scope.ComplainTrailList));

        $http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveIODetails", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $("#addModal").modal("hide");

            //$http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveComplainUnderSection", formData,
            //    {
            //        withCredentials: true,
            //        headers: { 'Content-Type': undefined },
            //        transformRequest: angular.identity
            //    }).success(function (data, status) {
            //        $(".se-pre-con").fadeOut("slow");
            if (data.Result != "-1" && data.Result != "0") {
                ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully IO submitted report to DR.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                    window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintAssignIO";
                });
            } else {
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                });
            }
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                });
            });

    };

    $scope.SaveIODetailsUser = function () {
        if ($scope.validateIODetails()) {
            $scope.StatusId = "0",
                $("#addModal").modal("hide");
            AadhaarAuth("", $http, "OTP", true, function (data) {
                $("#addModal").modal("show");
                $scope.SaveIODetails(data.TransId);
            });
        }
       
    }

    $scope.SaveIODetailsTransferDR = function () {
        if ($scope.validateIODetailsTransferDR()) {
            $scope.StatusId = "3",
                $scope.SaveIODetails();
        }
    }



    $scope.IODetailsTransferDR = [];
    $scope.IODetailsTransferDR.push();
    $scope.validateIODetailsTransferDR = function () {

        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");


        if ($scope.ComplainTrail.FileName == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया फाइल अपलोड करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया फाइल अपलोड करें।' : '<br />कृपया फाइल अपलोड करें।';
            }
        }
        

        if ($scope.ComplainTrail.Remark == '' || $scope.ComplainTrail.Remark == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया टिप्पणी दर्ज करें' : 'कृपया टिप्पणी दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया टिप्पणी दर्ज  करें।' : '<br />कृपया टिप्पणी दर्ज करें।';
            }
            $("#Remark").addClass("notValid");
        }
        else {
            $("#Remark").removeClass("notValid");
        }

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }



    //------------------End Save IO Details -----------------------------//














    $scope.GetAllSubUnitList = function () {

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/User/GetUserDetailList").
            success(function (data, status) {
                $scope.UserReqData = data;
                $(".se-pre-con").fadeOut("slow");

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

            });
    }




    $scope.GetApprovedSocietyList = function () {
        return $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetApprovedSocietyList").
            success(function (data, status) {
                $scope.GetApprovedSocietyACT2001List = [];
                $scope.GetApprovedSocietyACT2001List = data;
                //$scope.TotalRecord = $scope.LoanSchemeList.length;

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
            error(function (data, status) {
                alert("Error while fetching Society Details.");

            });
    }


    $scope.GetApprovedSocietyAssignForIO = function () {
        return $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetApprovedSocietyAssignIOList").
            success(function (data, status) {
                $scope.GetApprovedSocietyAssignForIOList = [];
                // $scope.GetApprovedSocietyAssignForIOList = data;
                //$scope.TotalRecord = $scope.LoanSchemeList.length;








                if (data.Table.length > 0) {
                    $scope.GetApprovedSocietyAssignForIOList = [];
                    $scope.GetApprovedSocietyAssignForIOList = data.Table;
                }








                //if (dataList.Table3.length > 0) {
                //    $scope.PropertyDetailsList = [];
                //    if (dataList.Table3.filter(function (x) { return x.MemberType === 1 }).length > 0) {
                //        dataList.Table3.filter(function (x) { return x.MemberType === 1 }).forEach(function (itm, index) {
                //            $scope.PropertyDetailsList.push({
                //                Id: itm.loan_property_id,
                //                Khasara: itm.khasara,
                //                Address: itm.Address,
                //                TotalArea: itm.TotalArea,
                //                OtherDetails: itm.other_property_details,
                //                Specifications: itm.specifications,
                //                IsDelete: false
                //            });
                //        });
                //    } else {
                //        $scope.PropertyDetailsList.push($scope.PropertyDetails);
                //    }

                //    $scope.GuarantorPropertyDetailsList = [];
                //    if (dataList.Table3.filter(function (x) { return x.MemberType === 2 }).length > 0) {
                //        dataList.Table3.filter(function (x) { return x.MemberType === 2 }).forEach(function (itm, index) {

                //            $scope.GuarantorPropertyDetailsList.push({
                //                Id: itm.loan_property_id,
                //                Khasara: itm.khasara,
                //                Address: itm.Address,
                //                TotalArea: itm.TotalArea,
                //                OtherDetails: itm.other_property_details,
                //                Specifications: itm.specifications,
                //                IsDelete: false,
                //                GPropertyDocument: itm.PropertyDocument,
                //                PropertyDocumentType: $scope.PropertyDocumentType.filter(function (x) { return x.PropertyDocumentType === itm.PropertyDocumentType }).length > 0 ? itm.PropertyDocumentType : "OtherType",
                //                PropertyDocumentTypeList: $scope.PropertyDocumentType,
                //                PropertyDocumentTypeOther: $scope.PropertyDocumentType.filter(function (x) { return x.PropertyDocumentType === itm.PropertyDocumentType }).length > 0 ? "" : itm.PropertyDocumentType,
                //                IsUploadGPropertyDocument: itm.PropertyDocument != "" && itm.PropertyDocument != null
                //            });
                //        });
                //    } else {
                //        $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);
                //    }
                //}





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
            error(function (data, status) {
                alert("Error while fetching Society Details.");

            });
    }




    $scope.LetterData = {
        ComplaintId: "",
        Date: "",
        Time: "",
        Place: "",
        CEOName: "",
        CEOMobile: "",
        complainant_letter: "",
        ceo_letter: "",
        ComplainerName: "",
        MobileNo: "",
        SocietyName: "",
        ReceiptNo: ""
    }
    $scope.GetFilterbyAction = function () {


    }




    $scope.OnPageLoad();
    //});






    $scope.FileType = "";

    $scope.AddFile = function (type) {
        $scope.FileType = type;
    }
    $scope.ChangeFile = function (type) {
        if (type == 1) {
            $scope.ComplainTrail.IsUploadFileName = false;
            $scope.ComplainTrail.FileName = "";
        }
    }

    $scope.UploadFile = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr(SiteUrl + "/SocietyComplaintCourtModule/UploadComplainFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr = function (url, data, type) {
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
                        if (type == 1) {
                            $scope.ComplainTrail.FileName = request.responseText;
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

    $scope.ComplainTrail = {
        IsUploadFileName: false,
        FileName: "",

    }
    $scope.ComplainTrailList = [];
    $scope.GetComplainTrail = function (ComplainID) {

        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainTrail/?ComplainID=" + ComplainID).
            success(function (data, status) {
                $scope.ComplainTrailList = JSON.parse(data.DataList);
                $(".se-pre-con").fadeOut("slow");
                $("#mdlTrailList").modal("show");

            }).
            error(function (data, status) {
                $scope.ComplainTrailList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

});

AppClasses.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 
                        if (file >= 4096) {
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {
                            if (scope.FileType == 1) {
                                scope.ComplainTrail.IsUploadFileName = true;
                                scope.ComplainTrail.FileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});
