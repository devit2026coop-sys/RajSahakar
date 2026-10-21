using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    [LogActionFilter]
    public class FAMController : BaseController
    {

        public ActionResult AddBankDetails()
        {
            return View();
        }
        public ActionResult PLDBDetailsReport()
        {
            return View();
        }
        public ActionResult FAM_VoucherTransactionList()
        {
            return View();
        }
        public ActionResult FAM_OffilineVoucherPosting()
        {
            return View();
        }
        public ActionResult FAM_FDProductmaster()
        {
            return View();
        }
        public ActionResult FAM_FDPreMaturityRule()
        {
            return View();
        }
        public ActionResult FAM_FDIntrestCalculationChart()
        {
            return View();
        }
        public ActionResult FAM_FDGenrationAndRenuwal()
        {
            return View();
        }
        public ActionResult FAM_FDHolderAndOwnerMaster()
        {
            return View();
        }
        public ActionResult FAM_FDMatuarityRule()
        {
            return View();
        }
        // GET: FAM
        public ActionResult IssueReceipt()
        {
            return View();
        }
        public ActionResult ReceiptUtilisation()
        {
            return View();
        }
        public ActionResult SalaryHeadsMapping()
        {
            return View();
        }
        public ActionResult CostCentreMaster()
        {
            return View();
        }

        public ActionResult ReceiptHolderMaster()
        {
            return View();
        }

        public ActionResult CashandBankMaster()
        {
            return View();
        }

        public ActionResult BudgetMaster()
        {
            return View();
        }

        public ActionResult VoucherEntry()
        {
            return View();
        }
        public ActionResult CashDetail()
        {
            return View();
        }
        public ActionResult MedicalExpensess()
        {
            return View();
        }
        public ActionResult TravelExpenses()
        {
            return View();
        }

        public ActionResult BriefcaseExpenses()
        {
            return View();
        }
        public ActionResult NewspaperExpenses()
        {
            return View();
        }
        public ActionResult FixedDepositIntPosting()
        {
            return View();
        }
        public ActionResult IntpaidOnFD()
        {
            return View();
        }
        public ActionResult FDRMaturity()
        {
            return View();
        }
        public ActionResult PreMaturityFDR()
        {
            return View();
        }
        public ActionResult InstitutionDetail()
        {
            return View();
        }
        public ActionResult VoucherEntriy()
        {
            return View();
        }

        public ActionResult InterestPaidOnFD()
        {
            return View();
        }
        public ActionResult CashBankBook()
        {
            return View();
        }
        public ActionResult InvestmentConsolidatedReport()
        {
            return View();
        }
        public ActionResult InvestmentRegister()
        {
            return View();
        }
        public ActionResult InvestmentIntrest()
        {
            return View();
        }

        public ActionResult DayBook()
        {
            return View();
        }
        public ActionResult TransferBook()
        {
            return View();
        }
        public ActionResult FundingPosition()
        {
            return View();
        }
        public ActionResult AccountBalanceReport()
        {
            return View();
        }

        public ActionResult InvestmentRenewal()
        {
            return View();
        }

        public ActionResult ConsolidatedInvestmentReport()
        {
            return View();
        }

        public ActionResult InterestCalculationInvestment()
        {
            return View();
        }

        public ActionResult InvestmentBond()
        {
            return View();
        }

        public ActionResult InterestReceivable()
        {
            return View();
        }

        public ActionResult FundingPositionConfiguration()
        {
            return View();
        }
        public ActionResult TrialBalance()
        {
            return View();
        }
        public ActionResult SequencingOfFinancialStatements()
        {
            return View();
        }
        public ActionResult AccountLedgerReport()
        {
            return View();
        }

        public ActionResult AccountCodeList()
        {
            return View();
        }
        public ActionResult FinancialStatementConfiguration()
        {
            return View();
        }




    }
}