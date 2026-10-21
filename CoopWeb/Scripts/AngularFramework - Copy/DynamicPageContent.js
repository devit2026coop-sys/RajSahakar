var AppFeedBacks = angular.module("DynamicPageContentAction", ['angularUtils.directives.dirPagination']);
AppFeedBacks.controller("DynamicPageContentAction", function ($scope, $http) {
    // Save Sms Template 

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
    $scope.divimage = false;
    $scope.FileName = '';

    //$scope.SaveDynamicPage = function () {
    //    //$http.post("/FeedBacks/SaveFeedBacks", { "FeedBacksId": $scope.FeedBacks.FeedBacksId, "ProgrammeNameHi": $scope.FeedBacks.ProgrammeNameEn, "ProgrammeNameHi": $scope.FeedBacks.ProgrammeNameHi, "IsActive": $scope.FeedBacks.IsActive, "classEncID": $scope.FeedBacks.classEncID }).
    //    // success(function (data, status) {
    //    var formData = new FormData();
    //    formData.append("PageId", $scope.PageContent.PageId);
    //    //formData.append("PageDescEn", $scope.PageContent.PageDescEn);
    //    //formData.append("PageDescHi", $scope.PageContent.PageDescHi);
    //    alert($('#editor').val())
    //    alert($('#editor1').val())
    //    formData.append("PageDescEn", $('#editor').val());
    //    formData.append("PageDescHi", $('#editor1').val());
    //    debugger;
    //    $http.post(SiteUrl + "/WebSahakar/SaveDynamicPage", formData,
    //        {
    //            withCredentials: true,
    //            headers: { 'Content-Type': undefined },
    //            transformRequest: angular.identity
    //        }).
    //        success(function (data, status) {
    //            var k = data;
    //            var message = '';
    //            if (k == 1) {
    //                message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
    //            }
    //            else if (k == -2) {
    //                message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
    //            }
    //            else {
    //                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
    //            }
    //            alert(message);
    //            window.location = '/User/DynamicPageContent';
    //            if ($scope.ParentId == '') {
    //                $scope.ParentId = 0;
    //            }
    //            $scope.GetAllFeedBacksList();
    //            if (data.Status) {
    //                $scope.FeedBacks = null;
    //                $scope.ClearFeedBacksForm();
    //                $scope.GetAllFeedBacksList();
    //            }
    //        }).
    //        error(function (data, status) {
    //            alert(data + "Error while page content insert" + status);
    //        });
    //};





    $scope.UploadNewsFile = function (file) {
        $scope.PageContent.Image = file[0];
        $scope.UploadNewsContent();
    }

    $scope.UploadNewsContent = function () {
        var formData = new FormData();
        formData.append("Image", $scope.PageContent.Image);
        $http.post(SiteUrl + "/WebSahakar/UploadPageContent", formData,
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


    $scope.SaveDynamicPage = function () {

        $.ajax({
            url: '../WebSahakar/SaveDynamicPage',
            data: JSON.stringify({
                "_Model": {
                    "PageId": $scope.PageContent.PageId
                    , "PageDescEn": $('#editor').val()
                    , "PageDescHi": $('#editor1').val()
                    , "Image": $scope.FileName
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
    // Update Template  
    $scope.update = function (Item) {
        if (Item.PageId.toString() == "10") {
            $scope.divimage = true;
        }
        else
        { $scope.divimage = false; } 
        $scope.DivAddContent = true;
        $scope.DivPageContent = false;
        $scope.PageContent = Item;
        $scope.PageContent.PageId = Item.PageId.toString();
        $('#DDLPageId').val(Item.PageId.toString());
        $('#editor').parent().children().next().next().contents().find('body').html(Item.PageDescEn);
        $('#editor1').parent().children().next().next().contents().find('body').html(Item.PageDescHi);

    };

    $scope.ClearDynamicPage = function () {
       // $scope.PageContent.PageId = Item.PageId.toString();
       // $('#DDLPageId').val(Item.PageId.toString());
        $('#editor').parent().children().next().next().contents().find('body').html("");
        $('#editor1').parent().children().next().next().contents().find('body').html("");

    };
    $scope.SetStatus = function (PageId, ActionId) {
        alert(ActionId);
        if (ActionId == false)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WebSahakar/DeleteandstatusPages", { Id: PageId, Action: ActionId == true ? "1" : "0" }).
            success(function (data, status) {

                $scope.GetAllFeedBacksList();

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
        //$scope.ClearFeedBacksForm() = function () {
        //    $scope.FeedBacks = '';
        //};


        //Get list of sms template page list
        $scope.GetAllPageContentList();
        //$(".se-pre-con").fadeOut("slow");

        //$scope.GetAllMailData();

    };
    // Language Selector at page load

    $scope.GetAllPageContentList = function () {
      //  alert(1);
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
    //$scope.GetAllPageContentList();
    $scope.GetPageList();
    // $scope.TargetList();

});

