var KVSSAuditAccountModule = angular.module("KVSSAuditAccountModule", []);

KVSSAuditAccountModule.controller("KVSSAuditAccountCont", ["$scope", "$http", function ($scope, $http) {
    "use strict";
    var root = (typeof SiteUrl !== "undefined" ? SiteUrl : "");
    var API = { Get: root + "/KVSSAuditAccount/GetAccount", Save: root + "/KVSSAuditAccount/SaveAccount" };
    var round = function (n) { return Math.round((Number(n) || 0) * 100) / 100; };
    var flag = function (v) { return v === true || v === 1 || v === "1"; };
    var validFY = function (fy) { return /^\d{4}-\d{2}$/.test(fy || "") && ((Number(fy.substring(0, 4)) + 1) % 100) === Number(fy.substring(5)); };
    var notify = function (msg, error) {
        if (typeof toastr !== "undefined") { error ? toastr.error(msg) : toastr.success(msg); }
        else { alert(msg); }
    };

    $scope.SocietyId = "1";// Number(document.getElementById("hdnSocietyId").value) || 0;
    $scope.ReportingFY = "2026-27";// document.getElementById("hdnFinancialYear").value || "2026-27";
    $scope.PreviousFY = function (fy) {
        if (!validFY(fy)) return "";
        var y = Number(fy.substring(0, 4)) - 1;
        return y + "-" + String(y + 1).slice(-2);
    };
    $scope.FinancialYear = $scope.PreviousFY($scope.ReportingFY);
    $scope.AccountId = 1;
    $scope.EntryId = 0;
    $scope.EntryStatus = "Draft";
    $scope.AccountData = [];
    $scope.DebitData = [];
    $scope.CreditData = [];
    $scope.AccountRows = [];
    $scope.IsLoading = false;
    $scope.IsSaving = false;
    $scope.ErrorMessage = "";
    $scope.TotalDebit = 0;
    $scope.TotalCredit = 0;
    $scope.GrandTotal = 0;

    function childrenOf(id) {
        return $scope.AccountData.filter(function (x) { return Number(x.ParentParticularId) === Number(id); });
    }
    function label(item) { return String(item.ParticularNameEnglish || "").trim().toLowerCase(); }
    function balanceType(item) {
        var s = label(item);
        if (s === "gross profit" || s === "gross profit b/d") return "grossprofit";
        if (s === "gross loss") return "grossloss";
        if (s === "net profit") return "netprofit";
        if (s === "net loss") return "netloss";
        if (s === "total" || s === "grand total") return "total";
        return "";
    }
    function hasChildren(item) { return childrenOf(item.ParticularId).length > 0; }
    $scope.IsAmountReadonly = function (item) {
        return $scope.EntryStatus === "Final" || flag(item.IsCalculated) || flag(item.IsBreakupAllowed) || hasChildren(item);
    };

    $scope.UpdateAccountTotals = function () {

        var debitTotal = 0;
        var creditTotal = 0;

        angular.forEach($scope.AccountData || [], function (item) {

            var amount = Number(item.Amount) || 0;

            // Grand Total rows ko exclude karein
            var name = (item.ParticularNameEnglish || "")
                .toLowerCase()
                .trim();

            if (name === "total" || name === "grand total") {
                return;
            }

            // Parent rows ko exclude karein,
            // agar unki amount children ka total hai.
            if (item.HasChildren) {
                return;
            }

            if (item.EntrySide === "Dr") {
                debitTotal += amount;
            }
            else if (item.EntrySide === "Cr") {
                creditTotal += amount;
            }
        });

        $scope.TotalDebit =
            Math.round(debitTotal * 100) / 100;

        $scope.TotalCredit =
            Math.round(creditTotal * 100) / 100;
    };


    $scope.BuildRows = function () {
        function flatten(side) {
            var items = $scope.AccountData.filter(function (x) { return x.EntrySide === side; });
            var seen = {};
            var result = [];
            function sort(a, b) { return Number(a.DisplayOrder) - Number(b.DisplayOrder) || Number(a.ParticularId) - Number(b.ParticularId); }
            function visit(item, level) {
                if (seen[item.ParticularId]) return;
                seen[item.ParticularId] = true;
                item.Level = level;
                result.push(item);
                items.filter(function (x) { return Number(x.ParentParticularId) === item.ParticularId; }).sort(sort)
                    .forEach(function (child) { visit(child, level + 1); });
            }
            items.filter(function (x) { return !x.ParentParticularId || !items.some(function (p) { return p.ParticularId === x.ParentParticularId; }); })
                .sort(sort).forEach(function (x) { visit(x, 0); });
            items.sort(sort).forEach(function (x) { visit(x, 0); });
            return result;
        }
        $scope.DebitData = flatten("Dr");
        $scope.CreditData = flatten("Cr");
        $scope.AccountRows = [];
        for (var i = 0; i < Math.max($scope.DebitData.length, $scope.CreditData.length); i++) {
            $scope.AccountRows.push({ Debit: $scope.DebitData[i] || null, Credit: $scope.CreditData[i] || null });
        }
    };

    $scope.GetAccount = function () {
        if (!$scope.SocietyId || !$scope.FinancialYear) { $scope.ErrorMessage = "Valid SocietyId and Financial Year required."; return; }
        $scope.IsLoading = true;
        $scope.ErrorMessage = "";
        $http.get(API.Get, { params: { societyId: $scope.SocietyId, financialYear: $scope.FinancialYear, accountId: $scope.AccountId } })
            .then(function (response) {
                var data = response.data || {};
                if (data.Success === false) throw new Error(data.Message || "Unable to load account.");
                $scope.EntryId = Number((data.Header || {}).EntryId) || 0;
                $scope.EntryStatus = (data.Header || {}).EntryStatus || "Draft";
                var breakups = data.Breakups || [];
                $scope.AccountData = (data.Particulars || []).filter(function (x) { return x.IsActive !== false && x.IsActive !== 0; }).map(function (x) {
                    var item = angular.copy(x);
                    item.ParticularId = Number(item.ParticularId);
                    item.ParentParticularId = item.ParentParticularId == null ? null : Number(item.ParentParticularId);
                    item.EntrySide = String(item.EntrySide || "").trim().toUpperCase() === "DR" ? "Dr" : "Cr";
                    item.IsAmountApplicable = item.IsAmountApplicable !== false && item.IsAmountApplicable !== 0;
                    item.IsBreakupAllowed = flag(item.IsBreakupAllowed);
                    item.IsCalculated = flag(item.IsCalculated);
                    item.Amount = round(item.Amount);
                    item.Breakups = breakups.filter(function (b) { return Number(b.ParticularId) === item.ParticularId; })
                        .map(function (b) { return { BreakupId: Number(b.BreakupId) || 0, BreakupName: b.BreakupName || "", Amount: round(b.Amount) }; });
                    item.BreakupTotal = round(item.Breakups.reduce(function (s, b) { return s + round(b.Amount); }, 0));
                    item.ShowBreakup = item.IsBreakupAllowed && item.Breakups.length > 0;
                    if (item.IsBreakupAllowed && item.Breakups.length && !item.ParentParticularId) item.Amount = item.BreakupTotal;
                    return item;
                });
                $scope.BuildRows();
                $scope.CalculateAll();
                console.log("AccountData:", $scope.AccountData.length, "DebitData:", $scope.DebitData.length, "CreditData:", $scope.CreditData.length, "AccountRows:", $scope.AccountRows.length);
            }).catch(function (error) {
                $scope.ErrorMessage = (error.data && error.data.Message) || error.message || "Unable to load account.";
                console.error("GetAccount error", error);
            }).finally(function () { $scope.IsLoading = false; });
    };

    // One green + button per displayed row. Add one breakup on each eligible side.
    $scope.CanAddBothSideBreakup = function (row) {
        return !!(row &&
            ((row.Debit && row.Debit.IsBreakupAllowed) ||
                (row.Credit && row.Credit.IsBreakupAllowed)));
    };

    $scope.AddBothSideBreakup = function (row) {
        if (!row || $scope.EntryStatus === "Final" || $scope.IsSaving || $scope.IsLoading) return;

        if (row.Debit && row.Debit.IsBreakupAllowed) {
            row.Debit.ShowBreakup = true;
            $scope.AddInlineBreakup(row.Debit);
        }
        if (row.Credit && row.Credit.IsBreakupAllowed) {
            row.Credit.ShowBreakup = true;
            $scope.AddInlineBreakup(row.Credit);
        }
    };

    // Green + in the particular row opens its inline section and adds a row.
    $scope.ToggleInlineBreakup = function (item) {
        if (!item || $scope.EntryStatus === "Final" || !item.IsBreakupAllowed) return;
        item.ShowBreakup = true;
        $scope.AddInlineBreakup(item);
    };

    $scope.AddInlineBreakup = function (item) {
        if (!item || $scope.EntryStatus === "Final" || !item.IsBreakupAllowed) return;
        item.Breakups = item.Breakups || [];
        item.Breakups.push({ BreakupId: 0, BreakupName: "", Amount: 0 });
        $scope.UpdateInlineTotal(item);
    };
    // Return the longer side's breakup array so Dr/Cr/delete rows stay aligned.
    $scope.GetPairedBreakups = function (row) {
        if (!row) return [];
        var debit = row.Debit && row.Debit.Breakups || [];
        var credit = row.Credit && row.Credit.Breakups || [];
        return debit.length >= credit.length ? debit : credit;
    };

    // Delete the same breakup index from BOTH sides with one center button.
    $scope.RemoveBothSideBreakup = function (row, index) {
        if (!row || $scope.EntryStatus === "Final" || $scope.IsSaving || $scope.IsLoading) return;
        [row.Debit, row.Credit].forEach(function (item) {
            if (!item || !item.Breakups || index < 0 || index >= item.Breakups.length) return;
            item.Breakups.splice(index, 1);
            item.ShowBreakup = item.Breakups.length > 0;
            $scope.UpdateInlineTotal(item);
        });
    };

    $scope.RemoveInlineBreakup = function (item, index) {
        if (!item || $scope.EntryStatus === "Final" || !item.Breakups) return;
        item.Breakups.splice(index, 1);
        item.ShowBreakup = item.Breakups.length > 0;
        $scope.UpdateInlineTotal(item);
    };
    $scope.UpdateInlineTotal = function (item) {
        if (!item) return;
        item.BreakupTotal = round((item.Breakups || []).reduce(function (sum, b) {
            return sum + (Number(b.Amount) >= 0 && isFinite(Number(b.Amount)) ? Number(b.Amount) : 0);
        }, 0));
        if (!hasChildren(item)) item.Amount = item.BreakupTotal;
        $scope.CalculateAll();
        //$scope.UpdateAccountTotals();
    };
    //$scope.OnAmountChange = function (item) {
    //    if (!item || $scope.IsAmountReadonly(item)) return;
    //    if (!isFinite(Number(item.Amount)) || Number(item.Amount) < 0) item.Amount = 0;
    //    item.Amount = round(item.Amount);
    //    $scope.CalculateAll();
    //};
    $scope.OnAmountChange = function (item) {

        if (!item) return;

        // Existing parent / calculated amount logic
        $scope.CalculateAll();

        // Refresh bottom totals
        $scope.UpdateAccountTotals();
    };

    $scope.CalculateAll = function () {
        // Parent headings represent child sums. Breakups on a parent are not added again.
        var memo = {}, visiting = {};
        function compute(item) {
            if (memo[item.ParticularId]) return round(item.Amount);
            if (visiting[item.ParticularId]) return round(item.Amount);
            visiting[item.ParticularId] = true;
            var children = childrenOf(item.ParticularId);
            if (children.length) item.Amount = round(children.reduce(function (sum, child) { return sum + compute(child); }, 0));
            delete visiting[item.ParticularId]; memo[item.ParticularId] = true;
            return round(item.Amount);
        }
        $scope.AccountData.forEach(function (item) { compute(item); });
        var dr = 0, cr = 0;
        $scope.AccountData.forEach(function (item) {
            if (balanceType(item) || hasChildren(item) || !item.IsAmountApplicable) return;
            if (item.EntrySide === "Dr") dr += round(item.Amount);
            if (item.EntrySide === "Cr") cr += round(item.Amount);
        });
        dr = round(dr); cr = round(cr);
        var grossProfit = Math.max(0, round(cr - dr)), grossLoss = Math.max(0, round(dr - cr));
        $scope.AccountData.forEach(function (item) {
            var type = balanceType(item);
            if ($scope.AccountId === 1) {
                if (type === "grossprofit") item.Amount = grossProfit;
                if (type === "grossloss") item.Amount = grossLoss;
            } else {
                if (type === "netprofit") item.Amount = grossProfit;
                if (type === "netloss") item.Amount = grossLoss;
            }
        });
        $scope.TotalDebit = round(dr + grossProfit);
        $scope.TotalCredit = round(cr + grossLoss);
        $scope.GrandTotal = Math.max($scope.TotalDebit, $scope.TotalCredit);
        $scope.AccountData.forEach(function (item) { if (balanceType(item) === "total") item.Amount = $scope.GrandTotal; });
    };

    function validate() {
        if (!$scope.SocietyId || !$scope.FinancialYear || !$scope.AccountData.length) return "Society, FY or account particulars missing.";
        for (var i = 0; i < $scope.AccountData.length; i++) {
            var item = $scope.AccountData[i];
            if (!isFinite(Number(item.Amount)) || Number(item.Amount) < 0) return "Invalid amount: " + item.ParticularNameEnglish;
            if (!item.IsBreakupAllowed) continue;
            for (var j = 0; j < (item.Breakups || []).length; j++) {
                var b = item.Breakups[j];
                if (!String(b.BreakupName || "").trim()) return "Breakup name required: " + item.ParticularNameEnglish;
                if (b.Amount === null || b.Amount === "" || !isFinite(Number(b.Amount)) || Number(b.Amount) < 0) return "Invalid breakup amount: " + item.ParticularNameEnglish;
            }
            if (!hasChildren(item) && item.Breakups.length && Math.abs(round(item.Amount) - round(item.BreakupTotal)) > 0.001) return "Breakup total mismatch: " + item.ParticularNameEnglish;
        }
        return "";
    }
    $scope.PrepareSaveData = function (mode) {
        var details = [], breakups = [];
        $scope.AccountData.forEach(function (item) {
            details.push({ ParticularId: item.ParticularId, Amount: round(item.Amount) });
            (item.Breakups || []).forEach(function (b) {
                breakups.push({ ParticularId: item.ParticularId, BreakupId: Number(b.BreakupId) || 0, BreakupName: String(b.BreakupName || "").trim(), Amount: round(b.Amount) });
            });
        });
        return {
            EntryId: $scope.EntryId, SocietyId: $scope.SocietyId, FinancialYear: $scope.FinancialYear,
            KVSSAuditAccountId: $scope.AccountId, SaveMode: mode, Details: details, Breakups: breakups
        };
    };
    $scope.SaveAccount = function (mode) {
        if ($scope.IsSaving || $scope.IsLoading || $scope.EntryStatus === "Final") return;
        $scope.CalculateAll();
        var error = validate(); if (error) { notify(error, true); return; }
        $scope.IsSaving = true;
        var tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
        if (!tokenElement || !tokenElement.value) {
            $scope.IsSaving = false;
            notify("Security token missing. Refresh page.", true);
            return;
        }
        $http.post(API.Save, $scope.PrepareSaveData(mode), {
            headers: { "RequestVerificationToken": tokenElement.value }
        }).then(function (response) {
            var data = response.data || {};
            if (data.Success !== true) { notify(data.Message || "Unable to save account.", true); return; }
            $scope.EntryId = Number(data.EntryId) || $scope.EntryId;
            $scope.EntryStatus = data.EntryStatus || "Draft";
            notify(data.Message || "Account saved successfully.");
            if (mode === "Next" && $scope.AccountId === 1) {
                $scope.AccountId = 2;
                $scope.GetAccount();
            } else {
                // Reload persisted rows and fresh BreakupId values.
                $scope.GetAccount();
            }
        }, function (error) { notify((error.data && error.data.Message) || "Error saving account.", true); })
            .finally(function () { $scope.IsSaving = false; });
    };
    $scope.SaveAsDraft = function () { $scope.SaveAccount("Draft"); };
    $scope.SaveAndNext = function () { $scope.SaveAccount("Next"); };
    $scope.ChangeAccount = function (id) {
        if ($scope.IsLoading || $scope.IsSaving || $scope.AccountId === Number(id)) return;
        $scope.AccountId = Number(id); $scope.GetAccount();
    };
    $scope.OnReportingFYChange = function () {
        var fy = $scope.PreviousFY($scope.ReportingFY);
        if (!fy) { $scope.ErrorMessage = "Enter valid Reporting FY, e.g. 2026-27."; return; }
        if (fy === $scope.FinancialYear) return;
        $scope.FinancialYear = fy; $scope.GetAccount();
    };
    $scope.GoBack = function () { window.history.back(); };
    $scope.GetAccount();
}]);
