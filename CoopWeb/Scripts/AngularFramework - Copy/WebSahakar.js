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


    $scope.SaveNewsContent = function () {
        $.ajax({
            url: '../WebSahakar/SaveNewsContent',
            data: JSON.stringify({
                "_Model": {
                    "NewsID": $scope.NewsContent.NewsID
                    , "NewsNameEN": $scope.NewsContent.NewsNameEN
                    , "NewsNameHI": $scope.NewsContent.NewsNameHI
                    , "NewsDescriptionEN": $('#editor').val()
                    , "NewsDescriptionHI": $('#editor1').val()
                    , "StartDate": $('#txtStartDate').val().split("/").reverse().join("-")
                    , "StartTime": $('#txtStartTime').val()
                    , "EndDate": $('#txtEndDate').val().split("/").reverse().join("-")
                    , "EndTime": $('#txtEndTime').val()
                    , "IsNew": $scope.NewsContent.IsNew
                    , "FileName": $scope.FileName
                    , "NewsDate": $('#txtNewsDate').val().split("/").reverse().join("-")
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
        $('#editor').parent().children().next().next().contents().find('body').html(Item.NewsDescriptionEN);
        $('#editor1').parent().children().next().next().contents().find('body').html(Item.NewsDescriptionHI);

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
    }

    $scope.BackToPage = function () {
        $scope.DivAddContent = false;
        $scope.DivPageContent = true;
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
