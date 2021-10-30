using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.Contract
{
    /*Instead of using hard coded route on each action method, we declare the route here
    and then use on action methods*/
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string ApiVersion = "v1";
        public const string Base = Root+"/"+ApiVersion;
        public const string LoginAndRegisterBase = Base+"Test";

        public static class Dashboard
        {
            public const string GetTicketsCounts = Base + "/dashboard/GetTicketsCounts";
            public const string GetFirstFiveRecords = Base + "/dashboard/GetFirstFiveList";
            public const string GetMonthlyTotRecords = Base + "/dashboard/GetMonthlyTotals/{dat}";
            public const string GetGraphWeekRecords = Base + "/dashboard/GetGraphweeklyList"; 
        }
        public static class Items
        {
            public const string GetAll = Base+"/items/getall";
            public const string GetActives = Base + "/items/getactives";
            public const string GetNonActives = Base + "/items/getnonactives";
            public const string Update = Base+ "/items/{itemId}";
            public const string Delete = Base+ "/items/{itemId}";
            public const string Get = Base+ "/items/{itemId}";
            public const string Create = Base+ "/items";
            public const string Dropdown = Base + "/items/dropdown";
            public const string Counts = Base + "/items/count";
            public const string ChkExisting = Base + "/items/checkcode/{Code}"; 
        }
        public static class AccountHeads
        {
            public const string GetAll = Base + "/accountheads/getall"; 
            public const string GetActives = Base + "/accountheads/getactives";
            public const string GetNonActives = Base + "/accountheads/getnonactives";
            public const string Update = Base + "/accountheads/{accountheadsId}";
            public const string Delete = Base + "/accountheads/{accountheadsId}";
            public const string Get = Base + "/accountheads/{accountheadsId}";
            public const string Create = Base + "/accountheads";
            public const string Dropdown = Base + "/accountheads/dropdown";
            public const string ChkExisting = Base + "/accountheads/checkcode/{Code}";
        }
        public static class AccountCode
        {
            public const string GetAll = Base + "/accountcodes/getall";
            public const string GetActives = Base + "/accountcodes/getactives";
            public const string GetNonActives = Base + "/accountcodes/getnonactives";
            public const string Update = Base + "/accountcodes/{accountcodeId}";
            public const string Delete = Base + "/accountcodes/{accountcodeId}";
            public const string Get = Base + "/accountcodes/{accountcodeId}";
            public const string Create = Base + "/accountcodes";
            public const string Dropdown = Base + "/accountcodes/dropdown";
            public const string ChkExisting = Base + "/accountcodes/checkcode/{Code}";
            public const string GetAllRepo = Base + "/accountcodes/getallrepo";
        }
        public static class Company
        {
            public const string GetAll = Base + "/company/getall";
            public const string GetActives = Base + "/company/getactives";
            public const string GetNonActives = Base + "/company/getnonactives";
            public const string Update = Base + "/company/{companyId}";
            public const string Delete = Base + "/company/{companyId}";
            public const string Get = Base + "/company/{companyId}";
            public const string Create = Base + "/company";
            public const string ChkExisting = Base + "/company/checkname/{Name}";
        }
        public static class PartyCode
        {
            public const string GetAll = Base + "/partycode/getall";
            public const string GetActives = Base + "/partycode/getactives";
            public const string GetNonActives = Base + "/partycode/getnonactives";
            public const string Update = Base + "/partycode/{partyCodeId}";
            public const string Delete = Base + "/partycode/{partyCodeId}";
            public const string Get = Base + "/partycode/{partyCodeId}";
            public const string Create = Base + "/partycode";
            public const string Dropdown = Base + "/partycode/dropdown";
            public const string ChkExisting = Base + "/partycode/checkname/{Name}";

            public const string GetRepo = Base + "/partycodeRepo/{accountCodeId}";
        }
        //=========================== Transactions =============================
        public static class Purchases
        {
            public const string GetAll = Base + "/purchases/getall";
            public const string GetActives = Base + "/purchases/getactives";
            public const string GetNonActives = Base + "/purchases/getnonactives";
            public const string Update = Base + "/purchases/{purchaseId}";
            public const string Delete = Base + "/purchases/{purchaseId}";
            public const string Get = Base + "/purchases/{purchaseId}";
            public const string Create = Base + "/purchases";
            public const string GetPurchaseItems = Base + "/purchaseItems/{purchaseId}";
            
            public const string GetDateWisePurchasesRepo = Base + "/purchasesdatewiseRepo/{date}";
        }
        public static class Supplier
        {
            public const string GetAll = Base + "/suppliers/getall";
            public const string GetActives = Base + "/suppliers/getactives";
            public const string GetNonActives = Base + "/suppliers/getnonactives";
            public const string Update = Base + "/suppliers/{supplierId}";
            public const string Delete = Base + "/suppliers/{supplierId}";
            public const string Get = Base + "/suppliers/{supplierId}";
            public const string Create = Base + "/suppliers";
            public const string Dropdown = Base + "/suppliers/dropdown";
        }
        public static class Sales
        {
            public const string GetAll = Base + "/sales/getall";
            public const string GetActives = Base + "/sales/getactives";
            public const string GetNonActives = Base + "/sales/getnonactives";
            public const string Update = Base + "/sales/{saleId}";
            public const string Delete = Base + "/sales/{saleId}";
            public const string Get = Base + "/sales/{saleId}";
            public const string Create = Base + "/sales";
            public const string GetPurchaseItems = Base + "/saleItems/{saleId}";

            public const string GetRepo = Base + "/salesRepo/{saleId}";
            public const string GetSaleRepoItems = Base + "/saleRepoItems/{saleId}";
            public const string GetDateWiseSalesRepo = Base + "/salesdatewiseRepo/{date}";
        }
        public static class CashReciept
        {
            public const string GetAll = Base + "/cashrecieved/getall";
            public const string GetActives = Base + "/cashrecieved/getactives";
            public const string GetNonActives = Base + "/cashrecieved/getnonactives";
            public const string Update = Base + "/cashrecieved/{cashRecievedId}";
            public const string Delete = Base + "/cashrecieved/{cashRecievedId}";
            public const string Get = Base + "/cashrecieved/{cashRecievedId}";
            public const string Create = Base + "/cashrecieved";
            public const string GetCashRecievedItems = Base + "/cashItems/{cashRecievedId}";

            public const string GetCashRecievedItemsRepo = Base + "/cashItemsRepo/{cashRecievedId}";
            public const string GetDateWiseCashRecievedRepo = Base + "/cashrecieveddatewiseRepo/{date}";
        }
        public static class CashPayment
        {
            public const string GetAll = Base + "/cashpayment/getall";
            public const string GetActives = Base + "/cashpayment/getactives";
            public const string GetNonActives = Base + "/cashpayment/getnonactives";
            public const string Update = Base + "/cashpayment/{cashPaymentId}";
            public const string Delete = Base + "/cashpayment/{cashPaymentId}";
            public const string Get = Base + "/cashpayment/{cashPaymentId}";
            public const string Create = Base + "/cashpayment";
            public const string GetCashPaymentItems = Base + "/cashPaymentItems/{cashPaymentId}";
            
            public const string GetCashPaymentItemsRepo = Base + "/cashPaymentItemsRepo/{cashPaymentId}";
            public const string GetDateWiseCashPaymentsRepo = Base + "/cashpaymentsdatewiseRepo/{date}";
        }
        //=========================== Stock =============================
        public static class Stock
        {
            public const string GetAll = Base + "/stock/getall";
            public const string GetActives = Base + "/stock/getactives";
            public const string GetNonActives = Base + "/stock/getnonactives";
        }

        //============================= Accoutning =======================
        public static class MonthOpenings
        {
            public const string GetAll = Base + "/monthopenings/getall";
            public const string GetActives = Base + "/monthopenings/getactives";
            public const string GetNonActives = Base + "/monthopenings/getnonactives";
            public const string Update = Base + "/monthopenings/{monthOpenId}";
            public const string Delete = Base + "/monthopenings/{monthOpenId}";
            public const string Get = Base + "/monthopenings/{monthOpenId}";
            public const string Create = Base + "/monthopenings";
            public const string Dropdown = Base + "/monthopenings/dropdown";
        }
        public static class DayOpenings
        {
            public const string GetAll = Base + "/dayopenings/getall";
            public const string GetActives = Base + "/dayopenings/getactives";
            public const string GetNonActives = Base + "/dayopenings/getnonactives";
            public const string Update = Base + "/dayopenings/{dayOpenId}";
            public const string Delete = Base + "/dayopenings/{dayOpenId}";
            public const string Get = Base + "/dayopenings/{dayOpenId}";
            public const string Create = Base + "/dayopenings";
            public const string Dropdown = Base + "/dayopenings/dropdown";
            public const string SumAllAmount = Base + "/sumofalldayopening/{dayOpenId}";
            public const string OpeningBalance = Base + "/dayopenings/openingbalance";
            public const string OpeningBalanceDateWise = Base + "/dayopeningdatewise/{dat}";
        }
        public static class PettyCash
        {
            public const string GetAll = Base + "/pettycash/getall";
            public const string GetActives = Base + "/pettycash/getactives";
            public const string GetNonActives = Base + "/pettycash/getnonactives";
            public const string Update = Base + "/pettycash/{pettyCashId}";
            public const string Delete = Base + "/pettycash/{pettyCashId}";
            public const string Get = Base + "/pettycash/{pettyCashId}";
            public const string Create = Base + "/pettycash";
            public const string Dropdown = Base + "/pettycash/dropdown";
        }
        public static class ExpenseHeads
        {
            public const string GetAll = Base + "/expenseheads/getall";
            public const string GetActives = Base + "/expenseheads/getactives";
            public const string GetNonActives = Base + "/expenseheads/getnonactives";
            public const string Update = Base + "/expenseheads/{expenseheadsId}";
            public const string Delete = Base + "/expenseheads/{expenseheadsId}";
            public const string Get = Base + "/expenseheads/{expenseheadsId}";
            public const string Create = Base + "/expenseheads";
            public const string Dropdown = Base + "/expenseheads/dropdown";
        }
        public static class Expenses
        {
            public const string GetAll = Base + "/expenses/getall";
            public const string GetActives = Base + "/expenses/getactives";
            public const string GetNonActives = Base + "/expenses/getnonactives";
            public const string Update = Base + "/expenses/{expenseId}";
            public const string Delete = Base + "/expenses/{expenseId}";
            public const string Get = Base + "/expenses/{expenseId}";
            public const string Create = Base + "/expenses";
            public const string Dropdown = Base + "/expenses/dropdown";

            public const string GetDateWiseExpensesRepo = Base + "/expensesdatewiseRepo/{date}";
        }
        // ===================== Admin ===================================
        public static class Users
        {
            public const string GetAll = Base + "/users/getall";
            public const string GetActives = Base + "/users/getactives";
            public const string GetNonActives = Base + "/users/getnonactives";
            public const string Update = Base + "/users/{userId}";
            public const string Delete = Base + "/users/{userId}";
            public const string Get = Base + "/users/{userId}";
            public const string Create = Base + "/users";
            public const string Dropdown = Base + "/users/dropdown";
        }
        public static class Roles
        {
            public const string GetAll = Base + "/role/getall";
            public const string GetActives = Base + "/role/getactives";
            public const string GetNonActives = Base + "/role/getnonactives";
            public const string Update = Base + "/role/{roleId}";
            public const string Delete = Base + "/role/{roleId}";
            public const string Get = Base + "/role/{roleId}";
            public const string Create = Base + "/roles";
            public const string Dropdown = Base + "/role/dropdown";
        }
        // ======================== Reports ==========================
        public static class Reports
        {
            public const string SOA = Base + "/reports/statementofaccount/{AccountCodeId}/{StartDate}/{EndDate}";
            
            public const string SalesRange = Base + "/reports/salesrange/{StartDate}/{EndDate}"; 
            public const string PartySales = Base + "/reports/partysales/{AccountCodeId}/{StartDate}/{EndDate}";

            public const string PurchaseRange = Base + "/reports/purchaserange/{StartDate}/{EndDate}";
            public const string CashPurchaseRange = Base + "/reports/cashpurchaserange/{StartDate}/{EndDate}";
            public const string PartyPurchase = Base + "/reports/partypurchase/{AccountCodeId}/{StartDate}/{EndDate}";

            public const string CashPaymentRange = Base + "/reports/cashpaymentrange/{StartDate}/{EndDate}";

            public const string CashRecieptRange = Base + "/reports/cashrecieptrange/{StartDate}/{EndDate}";

            public const string ExpensesRange = Base + "/reports/expensesrange/{StartDate}/{EndDate}";
        }
    }
}
