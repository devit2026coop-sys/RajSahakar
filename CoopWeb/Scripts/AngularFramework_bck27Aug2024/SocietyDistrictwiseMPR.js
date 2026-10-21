var societyDistrictwise = angular.module("societyDistrictwisemodule", ['angularUtils.directives.dirPagination']);
societyDistrictwise.controller("societyDistrictwiseCount", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DistrictId = 0;
  
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
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        };
        $scope.GetSocietyDistrictwiseMPR();
        $scope.AutoselectMonth();
    };

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
    $scope.AutoselectMonth();


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

    $scope.dateummu = '';


    $scope.GatMah = 0;
    $scope.MahMePanjikrit = 0;
    $scope.MahMeNirastPanjiyno = 0;
    $scope.MahKeAntMePanjikritsocietyKiKulSankhya = 0;
    $scope.KalamNo6MeSakriySamitiya = 0;
    $scope.KalamNo6MeNiskriySamitiya = 0;
    $scope.KalamNo6MeLiqividationSamitiya = 0;
    $scope.KalamNo6MeDarshayiAntTakKulSadashySankhya = 0;
    $scope.KalamNo6MeDarshayiAntTakHissaRashi = 0;
    $scope.KalamNo6MeDarshayiAntTakkaryshilPunji = 0;

    $scope.GetSocietyDistrictwiseMPR = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyDistrictwiseMPR = [];
       // debugger;
       //alert($('#txtFromMonth').val());
        if ($("#txtFromMonth").val() != '') {

            $scope.getdateproper();
        }
        //$http.post(SiteUrl + "MIS/GetSocietyDivisionDistrictwiseMPR", { "Type": "DistrictwiseMPR", "Dateval": $('#txtFromMonth').val(), "District": $scope.DistrictId }).

        $http.post(SiteUrl + "/MIS/GetSocietyDistrictwiseMPR", { "Dateval": $('#txtFromMonth').val(), "District": $scope.DistrictId }).
          success(function (data, status) {
            
                $(".se-pre-con").fadeOut("slow");
                $scope.SocietyDistrictwiseMPR = JSON.parse(data.DataList);
                $scope.GatMah = 0
                $scope.MahMePanjikrit = 0
                $scope.MahMeNirastPanjiyno = 0
                $scope.MahKeAntMePanjikritsocietyKiKulSankhya = 0
                $scope.KalamNo6MeSakriySamitiya = 0
                $scope.KalamNo6MeNiskriySamitiya = 0
                $scope.KalamNo6MeDarshayiAntTakKulSadashySankhya = 0
                $scope.KalamNo6MeDarshayiAntTakHissaRashi = 0
                $scope.KalamNo6MeDarshayiAntTakkaryshilPunji = 0
                $scope.KalamNo6MeLiqividationSamitiya = 0;
                $scope.SocietyDistrictwiseMPR.forEach(function (item) {
                    $scope.GatMah = parseInt($scope.GatMah) + parseInt(item.GatMah);
                    $scope.MahMePanjikrit = parseInt($scope.MahMePanjikrit) + parseInt(item.MahMePanjikrit);
                    $scope.MahMeNirastPanjiyno = parseInt($scope.MahMeNirastPanjiyno) + parseInt(item.MahMeNirastPanjiyno);
                   
                    $scope.MahKeAntMePanjikritsocietyKiKulSankhya = parseInt($scope.MahKeAntMePanjikritsocietyKiKulSankhya) + parseInt(item.MahKeAntMePanjikritsocietyKiKulSankhya);
                    $scope.KalamNo6MeSakriySamitiya = parseInt($scope.KalamNo6MeSakriySamitiya) + parseInt(item.KalamNo6MeSakriySamitiya);
                    $scope.KalamNo6MeNiskriySamitiya = parseInt($scope.KalamNo6MeNiskriySamitiya) + parseInt(item.KalamNo6MeNiskriySamitiya);
                    $scope.KalamNo6MeDarshayiAntTakKulSadashySankhya = parseInt($scope.KalamNo6MeDarshayiAntTakKulSadashySankhya) + parseInt(item.KalamNo6MeDarshayiAntTakKulSadashySankhya);
                    $scope.KalamNo6MeDarshayiAntTakHissaRashi = parseInt($scope.KalamNo6MeDarshayiAntTakHissaRashi) + parseInt(item.KalamNo6MeDarshayiAntTakHissaRashi);
                    $scope.KalamNo6MeDarshayiAntTakkaryshilPunji = parseInt($scope.KalamNo6MeDarshayiAntTakkaryshilPunji) + parseInt(item.KalamNo6MeDarshayiAntTakkaryshilPunji);
                    $scope.KalamNo6MeLiqividationSamitiya = parseInt($scope.KalamNo6MeLiqividationSamitiya) + parseInt(item.KalamNo6MeLiqividationSamitiya);
                })

            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            })
    };





    $scope.ExportData = function () {
       
                var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data" });

                // We want ROW 3 to be EXTRA TALL

        var headers = [$scope.translation.SRNO, $scope.translation.Sansthaokawarg, $scope.translation.Gatmahtakpanjikritsocietykisankhya, $scope.translation.Mahmepanjikritsocietykisankhya,
            $scope.translation.Mahmenirstpanjiynokisankhya, $scope.translation.Mahkeantmepanjikritsocietykikullsankhya, $scope.translation.columnsankhya6medarsayigyisamitiyomesesakriysamitiya,
            $scope.translation.columnsankhya6medarsayigyisamitiyomeseNiskriysamitiya, $scope.translation.columnsankhya6medarsayigyisamitiyomeseAAwasaynadhinsamitiya,
            $scope.translation.columnsankhya6medarsayigyisamitiyokemahkeanttakkulsadashyasankha, $scope.translation.columnsankhya6medarsayigyisamitiyokemahkeanttaksishsharashihajarome, $scope.translation.columnsankhya6medarsayigyisamitiyokemahkeanttakaryashilpunjihajarome
                     ];


                var curr = 1

                var formatHColumn = excel.addStyle({ 															// Format for headers
                    //border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 15 #324148"
                });


                excel.set(0, 0, curr, $scope.translation.MPR2001MembersShareCapital, formatHColumn);


        if ($('#txtFromMonth').val() != '' || $('#txtFromMonth').val() == undefined) {
                    ++curr;
                  excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtFromMonth').val(), formatHColumn);
                }

                if ($scope.DistrictId != '' || $scope.DistrictId == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, $scope.translation.DistrictHeading, formatHColumn);
                    excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $scope.DistrictId })[0].DISTRICT_MANGAL , formatHColumn);
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
        excel.set(0, 9, curr + 1, 10, formatColumn);
        excel.set(0, 10, curr + 1, 11, formatColumn);
        excel.set(0, 11, curr + 1, 12, formatColumn);




                curr = curr + 2;
        $scope.SocietyDistrictwiseMPR.forEach(function (item, index) {
            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.CategoryNameHi, formatColumn);
            excel.set(0, 2, index + curr, item.GatMah, formatColumn);
            excel.set(0, 3, index + curr, item.MahMePanjikrit, formatColumn);
            excel.set(0, 4, index + curr, item.MahMeNirastPanjiyno, formatColumn);
            excel.set(0, 5, index + curr, item.MahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
            excel.set(0, 6, index + curr, item.KalamNo6MeSakriySamitiya, formatColumn);
            excel.set(0, 7, index + curr, item.KalamNo6MeNiskriySamitiya, formatColumn);
            excel.set(0, 8, index + curr, item.KalamNo6MeLiqividationSamitiya, formatColumn);
            excel.set(0, 9, index + curr, item.KalamNo6MeDarshayiAntTakKulSadashySankhya, formatColumn);
            excel.set(0, 10, index + curr, item.KalamNo6MeDarshayiAntTakHissaRashi, formatColumn);
            excel.set(0, 11, index + curr, item.KalamNo6MeDarshayiAntTakkaryshilPunji, formatColumn);
        });

        curr = $scope.SocietyDistrictwiseMPR.length + curr - 1;;
        excel.set(0, 0, curr + 1, "", formatColumn);
        excel.set(0, 1, curr + 1, $scope.translation.Yog, formatColumn);
        excel.set(0, 2, curr + 1, $scope.GatMah, formatColumn);
        excel.set(0, 3, curr + 1, $scope.MahMePanjikrit, formatColumn);
        excel.set(0, 4, curr + 1, $scope.MahMeNirastPanjiyno, formatColumn);
        excel.set(0, 5, curr + 1, $scope.MahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
        excel.set(0, 6, curr + 1, $scope.KalamNo6MeSakriySamitiya, formatColumn);
        excel.set(0, 7, curr + 1, $scope.KalamNo6MeNiskriySamitiya, formatColumn);
        excel.set(0, 8, curr + 1, $scope.KalamNo6MeLiqividationSamitiya, formatColumn);
        excel.set(0, 9, curr + 1, $scope.KalamNo6MeDarshayiAntTakKulSadashySankhya, formatColumn);
        excel.set(0, 10, curr + 1, $scope.KalamNo6MeDarshayiAntTakHissaRashi, formatColumn);
        excel.set(0, 11, curr + 1, $scope.KalamNo6MeDarshayiAntTakkaryshilPunji, formatColumn);

        excel.generate($scope.translation.MPR2001MembersShareCapital+".xlsx");
           
    }



    //$scope.GetSocietyDistrictwiseMPR = function () {
    //    $http.post(SiteUrl + "MIS/GetSocietyDistrictwiseMPR").
    //        success(function (data, status) {
    //            $scope.GetSocietyDistrictwiseMPR = data;
    //            $scope.SocietyDistrictwiseMPR();
    //        }).
    //        error(function (data, status) {
    //            alert("Geting Some Errors");
    //        })
    //};
    //$scope.SocietyDistrictwiseMPR = function () {
    //    var SDWM = "fitDataFill";
    //    $scope.TableData = new Tabulator("#SocietyDistrictwiseMPR", {
    //        height: 500,
    //        data: $scope.GetSocietyDistrictwiseMPR,
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
    //            { title: "Shankhya Ka Warg", field: "", hozAlign: "center" },
    //            { title: "Gat Mah Tak Panjikrit Society ki Sankhya", field: "", hozAlign: "center" },
    //            { title: "Mah Me Panjikrit Society ki Sankhya ", field: "", hozAlign: "center" },
    //            { title: "Mah Me Nirast Panjiyno ki Sankhya ", field: "", hozAlign: "center" },
    //            { title: "Mah Ke Ant Me Panjikrit Society ki Kull Sankhya ", field: "", hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Me Se Sakriy Samitiya", field: "", hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Me Se Niskrit Samitiya", field: "", hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Me Se Awsaynadhin Samitiya", field: "", hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Ke Mah Ke Ant Tak Kul Sadshya Sankhaya", field: "", hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Ke  Mah Ke Ant Tak Hissa Rashi (Hajaro Me)", field: "", hozAlign: "center" },
    //            { title: "Kalam Snkhya-6 me Darsayi Gayi Samitiyo Ke  Mah Ke Ant Tak Karyshil Punji (Hajaro Me)", field: "", hozAlign: "center" },
    //        ],
    //    });
    //}
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.GetDistrictList();
    $scope.OnPageLoad();
});