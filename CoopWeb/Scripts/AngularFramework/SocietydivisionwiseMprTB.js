var societydivisionwise = angular.module("societydivisionwisemodule", ['angularUtils.directives.dirPagination']);
societydivisionwise.controller("societydivisionwiseCount", function ($scope, $http, $rootScope) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DivisionId = 0;
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
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
        };
        $scope.GetSocietyDivisionwiseMPR();
        $scope.AutoselectMonth();
        
    };

    $scope.dateummu = '';

    $scope.GatMah = 0;
    $scope.MahMePanjikrit = 0;
    $scope.MahMeNirastPanjiyno = 0;
    $scope.MahKeAntMePanjikritsocietyKiKulSankhya = 0;
    $scope.KalamNo6MeSakriySamitiya = 0;
    $scope.KalamNo6MeNiskriySamitiya = 0;
    $scope.KalamNo6MeLiqividationamitiya = 0;


    $scope.MahaGatMah = 0;
    $scope.MahaMahMePanjikrit = 0;
    $scope.MahaMahMeNirastPanjiyno = 0;
    $scope.MahaMahKeAntMePanjikritsocietyKiKulSankhya = 0;
    $scope.MahaKalamNo6MeSakriySamitiya = 0;
    $scope.MahaKalamNo6MeNiskriySamitiya = 0;
    $scope.MahaKalamNo6MeLiqividationamitiya = 0;


    $scope.ShirshGatMah = 0;
    $scope.ShirshMahMePanjikrit = 0;
    $scope.ShirshMahMeNirastPanjiyno = 0;
    $scope.ShirshMahKeAntMePanjikritsocietyKiKulSankhya = 0;
    $scope.ShirshKalamNo6MeSakriySamitiya = 0;
    $scope.ShirshKalamNo6MeNiskriySamitiya = 0;
    $scope.ShirshKalamNo6MeLiqividationamitiya = 0;

    $scope.AutoselectMonth = function () {
        var DT = new Date();
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getFullYear() + '-0' + (DT.getMonth() + 1);
        }
        else {
            date = DT.getFullYear() + '-' + (DT.getMonth() + 1);
        }
        $("#txtFromMonth").val(date);
    };
   



    $scope.getdateproper = function () {
        //debugger;
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];

        // alert($("#txtFromMonth").val());
        const today = new Date($("#txtFromMonth").val());
        $scope.dateummu = monthNames[today.getMonth()];
        $scope.dateummu = $scope.dateummu + '-' + today.getFullYear();

        let mm = today.getMonth() + 1; // Months start at 0!
        let dd = today.getDate();
        let yyyy = today.getFullYear();

        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        $scope.todaydate = dd + '/' + mm + '/' + yyyy;
        // alert($scope.todaydate);

    };

    



    $scope.GetSocietyDivisionwiseMPR = function () {       
        $scope.SocietyDivisionwiseMPR = [];
        $(".se-pre-con").fadeIn("slow");
       // $http.post(SiteUrl + "MIS/GetSocietyDivisionDistrictwiseMPR", { "Type": "DivisionwiseMPR", "Dateval": $('#txtFromMonth').val(), "DIVISION_ID": $scope.DivisionId }).
       
        $http.post(SiteUrl + "MIS/GetSocietydivisionwiseMPR", {  "Dateval": $('#txtFromMonth').val(), "DIVISION_ID": $scope.DivisionId }).
            success(function (data, status) {
                
                $(".se-pre-con").fadeOut("slow");
                if ($("#txtFromMonth").val() != '') {

                    $scope.getdateproper();
                }
                $scope.SocietyDivisionwiseMPR = JSON.parse(data.DataList);;
                $scope.GatMah = 0
                $scope.MahMePanjikrit = 0
                $scope.MahMeNirastPanjiyno = 0
                $scope.MahKeAntMePanjikritsocietyKiKulSankhya = 0
                $scope.KalamNo6MeSakriySamitiya = 0
                $scope.KalamNo6MeNiskriySamitiya = 0
                $scope.KalamNo6MeLiqividationamitiya = 0;
                $scope.SocietyDivisionwiseMPR.forEach(function (item) {
                    $scope.GatMah = parseInt($scope.GatMah) + parseInt(item.GatMah);
                    $scope.MahMePanjikrit = parseInt($scope.MahMePanjikrit) + parseInt(item.MahMePanjikrit);
                    $scope.MahMeNirastPanjiyno = parseInt($scope.MahMeNirastPanjiyno) + parseInt(item.MahMeNirastPanjiyno);
                    $scope.MahKeAntMePanjikritsocietyKiKulSankhya = parseInt($scope.MahKeAntMePanjikritsocietyKiKulSankhya) + parseInt(item.MahKeAntMePanjikritsocietyKiKulSankhya);
                    $scope.KalamNo6MeSakriySamitiya = parseInt($scope.KalamNo6MeSakriySamitiya) + parseInt(item.KalamNo6MeSakriySamitiya);
                    $scope.KalamNo6MeNiskriySamitiya = parseInt($scope.KalamNo6MeNiskriySamitiya) + parseInt(item.KalamNo6MeNiskriySamitiya);
                    $scope.KalamNo6MeLiqividationamitiya = parseInt($scope.KalamNo6MeLiqividationamitiya) + parseInt(item.KalamNo6MeLiqividationamitiya);;
                });

                $scope.MahaGatMah = $scope.GatMah;
                $scope.MahaMahMePanjikrit = $scope.MahMePanjikrit;
                $scope.MahaMahMeNirastPanjiyno = $scope.MahMeNirastPanjiyno;
                $scope.MahaMahKeAntMePanjikritsocietyKiKulSankhya = $scope.MahKeAntMePanjikritsocietyKiKulSankhya;
                $scope.MahaKalamNo6MeSakriySamitiya = $scope.KalamNo6MeSakriySamitiya;
                $scope.MahaKalamNo6MeNiskriySamitiya = $scope.KalamNo6MeNiskriySamitiya;
                $scope.MahaKalamNo6MeLiqividationamitiya = $scope.KalamNo6MeLiqividationamitiya;



            }).
            error(function (data, status) {
                $scope.SocietyDivisionwiseMPR = [];
                alert("Getinf Some Errors");
            })
    };
    $scope.ExportData = function () {

        var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data" });

        // We want ROW 3 to be EXTRA TALL

        var headers = [$scope.translation.SRNO, $scope.translation.Sansthaokawarg, $scope.translation.Gatmahtakpanjikritsocietykisankhya, $scope.translation.Mahmepanjikritsocietykisankhya,
        $scope.translation.Mahmenirstpanjiynokisankhya, $scope.translation.Mahkeantmepanjikritsocietykikullsankhya, $scope.translation.columnsankhya6medarsayigyisamitiyomesesakriysamitiya,
        $scope.translation.columnsankhya6medarsayigyisamitiyomeseNiskriysamitiya, $scope.translation.columnsankhya6medarsayigyisamitiyomeseAAwasaynadhinsamitiya       
        ];


        var curr = 1

        var formatHColumn = excel.addStyle({ 															// Format for headers
            //border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 15 #324148"
        });


        excel.set(0, 0, curr, $scope.translation.MPR2001SocietyRegistered, formatHColumn);


        if ($('#txtFromMonth').val() != '' || $('#txtFromMonth').val() == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtFromMonth').val(), formatHColumn);
        }

        if ($scope.DivisionId != '' || $scope.DivisionId == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Division, formatHColumn);
            excel.set(0, 1, curr, $scope.DivisionList.filter(function (x) { return x.DIVISION_ID == $scope.DivisionId })[0].DIVISION_MANGAL , formatHColumn);
        }

        // This array holds the HEADERS text
        var formatHeader = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 12 #fff B",
            background: "#64c0c5",
        }); 														// 		Font for headers
        curr = curr + 1;
        for (var i = 0; i < headers.length; i++) {
            //++curr;// Loop all the haders
            excel.set(0, i, curr + 1, headers[i], formatHeader);													// Set CELL with header text, using header format
            excel.set(0, i, undefined, "auto");															// Set COLUMN width to auto (according to the standard this is only valid for numeric columns)
        }
        // 		in color green

        var formatColumn = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 10 #324148"
        });

        curr = curr + 1;
        excel.set(0, 0, curr + 1, 1, formatColumn);
        excel.set(0, 1, curr + 1, 2, formatColumn);
        excel.set(0, 2, curr + 1, 3, formatColumn);
        excel.set(0, 3, curr + 1, 4, formatColumn);
        excel.set(0, 4, curr + 1, 5, formatColumn);
        excel.set(0, 5, curr + 1, 6, formatColumn);
        excel.set(0, 6, curr + 1, 7, formatColumn);
        excel.set(0, 7, curr + 1, 8, formatColumn);
        excel.set(0, 8, curr + 1, 9, formatColumn);

        curr = curr + 2;
        $scope.SocietyDivisionwiseMPR.forEach(function (item, index) {
            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.CategoryNameHi, formatColumn);
            excel.set(0, 2, index + curr, item.GatMah, formatColumn);
            excel.set(0, 3, index + curr, item.MahMePanjikrit, formatColumn);
            excel.set(0, 4, index + curr, item.MahMeNirastPanjiyno, formatColumn);
            excel.set(0, 5, index + curr, item.MahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
            excel.set(0, 6, index + curr, item.KalamNo6MeSakriySamitiya, formatColumn);
            excel.set(0, 7, index + curr, item.KalamNo6MeNiskriySamitiya, formatColumn);
            excel.set(0, 8, index + curr, item.KalamNo6MeLiqividationamitiya, formatColumn);
        });

        curr = $scope.SocietyDivisionwiseMPR.length + curr - 1;;
        excel.set(0, 0, curr + 1, "", formatColumn);
        excel.set(0, 1, curr + 1, $scope.translation.Yog, formatColumn);
        excel.set(0, 2, curr + 1, $scope.GatMah, formatColumn);
        excel.set(0, 3, curr + 1, $scope.MahMePanjikrit, formatColumn);
        excel.set(0, 4, curr + 1, $scope.MahMeNirastPanjiyno, formatColumn);
        excel.set(0, 5, curr + 1, $scope.MahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
        excel.set(0, 6, curr + 1, $scope.KalamNo6MeSakriySamitiya, formatColumn);
        excel.set(0, 7, curr + 1, $scope.KalamNo6MeNiskriySamitiya, formatColumn);
        excel.set(0, 8, curr + 1, $scope.KalamNo6MeLiqividationamitiya, formatColumn);

        curr =  curr + 1;;
        excel.set(0, 0, curr + 1, "", formatColumn);
        excel.set(0, 1, curr + 1, $scope.translation.Shirshsansthaye, formatColumn);
        excel.set(0, 2, curr + 1, $scope.ShirshGatMah, formatColumn);
        excel.set(0, 3, curr + 1, $scope.ShirshMahMePanjikrit, formatColumn);
        excel.set(0, 4, curr + 1, $scope.ShirshMahMeNirastPanjiyno, formatColumn);
        excel.set(0, 5, curr + 1, $scope.ShirshMahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
        excel.set(0, 6, curr + 1, $scope.ShirshKalamNo6MeSakriySamitiya, formatColumn);
        excel.set(0, 7, curr + 1, $scope.ShirshKalamNo6MeNiskriySamitiya, formatColumn);
        excel.set(0, 8, curr + 1, $scope.ShirshKalamNo6MeLiqividationamitiya, formatColumn);




        curr = curr + 1;;
        excel.set(0, 0, curr + 1, "", formatColumn);
        excel.set(0, 1, curr + 1, $scope.translation.Mahayog, formatColumn);
        excel.set(0, 2, curr + 1, $scope.MahaGatMah, formatColumn);
        excel.set(0, 3, curr + 1, $scope.MahaMahMePanjikrit, formatColumn);
        excel.set(0, 4, curr + 1, $scope.MahaMahMeNirastPanjiyno, formatColumn);
        excel.set(0, 5, curr + 1, $scope.MahaMahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
        excel.set(0, 6, curr + 1, $scope.MahaKalamNo6MeSakriySamitiya, formatColumn);
        excel.set(0, 7, curr + 1, $scope.MahaKalamNo6MeNiskriySamitiya, formatColumn);
        excel.set(0, 8, curr + 1, $scope.MahaKalamNo6MeLiqividationamitiya, formatColumn);

        excel.generate($scope.translation.MPR2001SocietyRegistered + ".xlsx");

    }


    $scope.DivisionList = [];   
    $scope.GetDistrictDivisionReasion = function () {
        $scope.DivisionList = [];       
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetDistrictDivisionReasion").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");               
                data.List.forEach(function (itm, index) {
                    if ($scope.DivisionList.filter(function (x) { return x.DIVISION_ENG == itm.DIVISION_ENG }).length <= 0) {
                        $scope.DivisionList.push(itm);
                    }                    
                });
            }).
            error(function (data, status) {
                $scope.DivisionList = [];
            });
    }

    $scope.GetDistrictDivisionReasion();

    $scope.OnPageLoad();
    //$scope.SearchData();
});
//tabulator code
//$scope.GetSocietydivisionwiseMPR = function () {     
    //    $http.post(SiteUrl + "MIS/GetSocietydivisionwiseMPR").
    //        success(function (data, status) {
    //            $scope.GetSocietydivisionwiseMPR = data;
    //            $scope.SocietydivisionwiseMPR();
    //        }).
    //        error(function (data, status) {
    //            alert("Geting Some Errors");
    //        })
    //};
    //$scope.SocietydivisionwiseMPR = function () {  
    //    var SDWM = "fitDataFill";
    //    $scope.TableData = new Tabulator("#SocietydivisionwiseMPR", {
    //        height: 500,
    //        data: $scope.GetSocietydivisionwiseMPR,
    //        layout: "fitDataFill",
    //        pagination: "local",
    //        paginationSize: 500,
    //        tooltips: false,           
    //        groupBy: "",
    //        langs: {
    //            "hi-hi": { 
    //                "columns": {
    //                    "SNNO": "एसएन नं",
    //                },
    //                "title": {
    //                    "Work Info": "काम की जानकारी",
    //                },
    //                "pagination": {
    //                    "first": "पहला",
    //                    "first_title": "पहला पृष्ठ",
    //                    "last": "अंतिम",
    //                    "last_title": "अंतिम पृष्ठ",
    //                    "prev": "पिछला",
    //                    "prev_title": "पिछला पृष्ठ",
    //                    "next": "अगला",
    //                    "next_title": "अगला पृष्ठ",
    //                    "all": "सभी",
    //                },
    //            },
    //        },
    //        columns: [

    //            { title: "SR.NO", field: "SNNO", sorter: "int", hozAlign: "center" },
    //            { title: "Shankhya Ka Warg",field:"CategoryNameEn",hozAlign: "center" },
    //            { title: "Gat Mah Tak Panjikrit Society ki Sankhya",field:"GatMah",hozAlign:"center" },
    //            { title: "Mah Me Panjikrit Society ki Sankhya",field:"MahMePanjikrit",hozAlign: "center" },
    //            { title: "Mah Me Nirast Panjiyno ki Sankhya",field:"MahMeNirastPanjiyno",hozAlign: "center" },
    //            { title: "Mah Ke Ant Me Panjikrit Society ki Kull Sankhya",field:"MahKeAntMePanjikritsocietyKiKulSankhya",hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Me Se Sakriy Samitiya",field:"KalamNo6MeSakriySamitiya",hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Me Se Niskrit Samitiya",field:"KalamNo6MeNiskriySamitiya",hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Me Se Awsaynadhin Samitiya",field:"",hozAlign: "center" },
    //        ],
    //    });
    //}
