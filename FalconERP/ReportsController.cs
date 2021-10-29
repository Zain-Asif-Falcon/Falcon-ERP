using Application.Common.Utilities;
using AspNetCore.Reporting;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.Accounts;
using Domain.ViewModel.Reports;
using Domain.ViewModel.Stock;
using Jose;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace ERPSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvirnoment;
        private readonly ILogger<ReportsController> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportsController(ILogger<ReportsController> logger, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvirnoment, IConfiguration config) 
        {
            _config = config;
            _userManager = userManager;
            _logger = logger;
            _webHostEnvirnoment = webHostEnvirnoment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DayBookActivity()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> AccountStatement()
        {
            ViewBag.AccountCodes = await AccountCodeDropDownData();
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> SalesReport()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> PartySales()
        {
            ViewBag.AccountCodes = await AccountCodeDropDownData();
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> PurchasesReport()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> PartyPurchases()
        {
            ViewBag.AccountCodes = await AccountCodeDropDownData();
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> CashPurchases()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> CashRecieptsReport()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> CashPaymentsReport()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> ExpensesReport()
        {
            return await Task.Run(() => View());
        }
        //===========================================================
        public async Task<IActionResult> ItemsReportPDF()
        {
            //JsonResult js = await GetItemsData();
            //string json = new JavaScriptSerializer().Serialize(js.Value);
            IEnumerable<Item> obj = await GetItemsClassData();// JsonConvert.DeserializeObject<List<Item>>(json);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(obj)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> AccountHeadsReportPDF()
        {
            IEnumerable<AccountHead> obj = await GetAccountHeadsClassData();// JsonConvert.DeserializeObject<List<Item>>(json);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(obj)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> AccountCodesReportPDF()
        {
            IEnumerable<AccountCodes> obj = await GetAccountCodesClassData();// JsonConvert.DeserializeObject<List<Item>>(json);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(obj)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> ItemStockReportPDF()
        {
            IEnumerable<StockViewModel> obj = await GetStockClassData();// JsonConvert.DeserializeObject<List<Item>>(json);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(obj)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> SaleNoteReportPDF(int OrderId)
        {
            var ord = await GetSalesOrders(OrderId);
            IEnumerable<SalesNoteItems> itms = await GetSaleItemNoteClassData(OrderId);
            SalesInvoiceVM sale = new SalesInvoiceVM { Order = ord , Items = itms.ToList() };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(sale)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> CashRecieptReportPDF(int OrderId)
        {
            var ord = await GetCashRecieptOrders(OrderId);
            IEnumerable<CashRecieptItems> itms = await GetCashRecieptItemNoteClassData(OrderId);
            CashRecieptVM rec = new CashRecieptVM { Order = ord, Items = itms.ToList() };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(rec)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> CashPaymentReportPDF(int OrderId)
        {
            var ord = await GetCashPaymentOrders(OrderId);
            IEnumerable<CashPaymentItemsRepo> itms = await GetCashPaymentItemNoteClassData(OrderId);
            CashPaymentVM rec = new CashPaymentVM { Order = ord, Items = itms.ToList() };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(rec)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> StatementOfAccountReportPDF(int AccountCodeId, DateTime startDate, DateTime endDate)
        {
            var ord = await GetPartyDetails(AccountCodeId);
            IEnumerable<StatementOfAccountViewModel> itms = await GetAccountStatmentListClassData(AccountCodeId, startDate, endDate); ;
            SOAViewModel rec = new SOAViewModel {  Ord = ord, Items = itms.ToList() };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(rec)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> DayBookActivityPDF(DateTime date)
        {
            IEnumerable<PurchaseNoteDateWise> pur = await GetDateWisePurchasesClassData(date);
            IEnumerable<CashPaymentDateWise> pay = await GetDateWisePaymentsClassData(date);
            IEnumerable<SaleNoteDateWise> sale = await GetDateWiseSalesClassData(date);
            IEnumerable<CashRecievedDateWise> cash = await GetDateWiseCashRecieptsClassData(date);
            IEnumerable<ExpensesReport> exp = await GetDateWiseExpensesClassData(date);
            NewOpeningBalanceVM opening = await GetOpeningBalanceDateWiseClassData(date);
            DailyBookVM rec = new DailyBookVM { CashPayment = pay.ToList(), CashRecieve = cash.ToList() , Expense = exp.ToList() , PurchaseNote = pur.ToList() , SaleNote = sale.ToList() , open = opening };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(rec)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> SalesReportPDF(DateTime startDate, DateTime endDate)
        {
            IEnumerable<SalesNote> itms = await GetSalesByRangeClassData(startDate, endDate);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(itms)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> PartySalesReportPDF(int AccountCodeId, DateTime startDate, DateTime endDate)
        {
            var ord = await GetPartyDetails(AccountCodeId);
            IEnumerable<SalesGoods> itms = await GetPartySalesListClassData(AccountCodeId, startDate, endDate);
            PartySalesVM rec = new PartySalesVM { Ord = ord, Items = itms.ToList() };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(rec)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> PurchasesReportPDF(DateTime startDate, DateTime endDate)
        {
            IEnumerable<PurchasesNote> itms = await GetPurchasesByRangeClassData(startDate, endDate);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(itms)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> PartyPurchasesReportPDF(int AccountCodeId, DateTime startDate, DateTime endDate)
        {
            var ord = await GetPartyDetails(AccountCodeId);
            IEnumerable<PurchaseGood> itms = await GetPartyPurchasesListClassData(AccountCodeId, startDate, endDate);
            PartyPurchasesVM rec = new PartyPurchasesVM { Ord = ord, Items = itms.ToList() };
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(rec)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> CashPurchasesReportPDF(DateTime startDate, DateTime endDate)
        {
            IEnumerable<PurchasesNote> itms = await GetCashPurchasesClassData(startDate, endDate);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(itms)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> CashPaymentReportPDFRange(DateTime startDate, DateTime endDate)
        {
            IEnumerable<CashPaymentItemsRepo> itms = await GetCashPaymentByRangeClassData(startDate, endDate);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(itms)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> CashRecieptsReportPDF(DateTime startDate, DateTime endDate)
        {
            IEnumerable<CashRecieptItems> itms = await GetCashRecieptByRangeClassData(startDate, endDate);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(itms)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        public async Task<IActionResult> ExpensesReportPDF(DateTime startDate, DateTime endDate)
        {
            IEnumerable<ExpensesReport> itms = await GetExpensesByRangeClassData(startDate, endDate);
            var pdf = new Rotativa.AspNetCore.ViewAsPdf(itms)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

            var byteArray = await pdf.BuildFile(ControllerContext);
            return File(byteArray, "application/pdf");
        }
        //===========================================================
        public async Task<IActionResult> ItemsReport()
        {
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\Items.rdlc";
            LocalReport localReportItems = new LocalReport(path);
            JsonResult js = await GetItemsData();
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);
            localReportItems.AddDataSource("dsItems", dt);
            var resultItems = localReportItems.Execute(RenderType.Pdf);
            return File(resultItems.MainStream, "application/pdf");
        }
        public async Task<IActionResult> AccountHeadsReport()
        {
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\AccountHeads.rdlc";
            LocalReport localReportAccountHead = new LocalReport(path);
            JsonResult js = await GetAccountHeads();
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);
            localReportAccountHead.AddDataSource("dsAccountHead", dt);
            var resultAccountHead = localReportAccountHead.Execute(RenderType.Pdf);
            return File(resultAccountHead.MainStream, "application/pdf");
        }
        public async Task<IActionResult> AccountCodesReport()
        {
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\AccountCodes.rdlc";
            LocalReport localReportAccountCode = new LocalReport(path);
            JsonResult js = await GetAccountCodes();
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);
            localReportAccountCode.AddDataSource("dsAccountCodes", dt);
            var resultAccountCode = localReportAccountCode.Execute(RenderType.Pdf);
            return File(resultAccountCode.MainStream, "application/pdf");
        }
        public async Task<IActionResult> ItemsStockReport()
        {
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\ItemStock.rdlc";
            LocalReport localReportItemsStock = new LocalReport(path);
            JsonResult js = await GetStock();
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);
            localReportItemsStock.AddDataSource("dsStock", dt);
            var resultItemsStock = localReportItemsStock.Execute(RenderType.Pdf);
            return File(resultItemsStock.MainStream, "application/pdf");
        }
        public async Task<IActionResult> SaleNoteReport(int OrderId)
        {
            string mimtype = "application/pdf";
            int extension = 4;
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\SalesReport.rdlc";

            var ord = await GetSalesOrders((int)OrderId);
            Dictionary<string, string> parametersSaleNote = new Dictionary<string, string>();
            parametersSaleNote.Add("InvoiceAmount", ord.InvoiceAmount.ToString());
            parametersSaleNote.Add("PartyName", ord.Description.ToString());
            parametersSaleNote.Add("partyCode", ord.Code.ToString());
            parametersSaleNote.Add("DocumentNo", ord.DocumentNumber.ToString());
            parametersSaleNote.Add("Misc", ord.Misc.ToString());
            parametersSaleNote.Add("Carrogation", ord.Carrogation.ToString());
            parametersSaleNote.Add("Carriage", ord.Carriage.ToString());
            parametersSaleNote.Add("Cutting", ord.Cutting.ToString());
            parametersSaleNote.Add("Loading", ord.Loading.ToString());
            parametersSaleNote.Add("Labour", ord.Labour.ToString());
            parametersSaleNote.Add("TotalExpenses", ord.TotalExpenses.ToString());
            parametersSaleNote.Add("PartyBalance", ord.OpeningBalance.ToString());
            parametersSaleNote.Add("Remaining", (ord.GrandTotal - ord.PayingAmount).ToString());
            parametersSaleNote.Add("PayingAmount", ord.PayingAmount.ToString());
            parametersSaleNote.Add("GrandTotal", ord.GrandTotal.ToString());
            parametersSaleNote.Add("DateSales", ord.DateSales.Value.ToString("dd-MM-yyyy"));

            LocalReport localReportSaleNote = new LocalReport(path);
            JsonResult js = await GetSaleItemNote(OrderId);
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);

            localReportSaleNote.AddDataSource("dsSalesNote", dt);
            var resultSaleNote = localReportSaleNote.Execute(RenderType.Pdf, extension, parametersSaleNote, mimtype);
            byte[] numArray = resultSaleNote.MainStream;
            string datos = Convert.ToBase64String(numArray);
            //""application/octet-stream"

            return File(resultSaleNote.MainStream, mimtype);
        }
        public async Task<IActionResult> CashRecieptReport(int OrderId)
        {
            string mimtype = "application/pdf";
            int extension = 4;
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\CashReciept.rdlc";

            var ord = await GetCashRecieptOrders((int)OrderId);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("docNumber", ord.DocumentNumber.ToString());
            parameters.Add("docDate", ord.documentDate.Value.ToString("dd-MM-yyyy"));

            LocalReport localReportCashReciept = new LocalReport(path);
            JsonResult js = await GetCashRecieptItemNote(OrderId);
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);
            localReportCashReciept.AddDataSource("dsCashReciept", dt);
            var resultCashReciept = localReportCashReciept.Execute(RenderType.Pdf, extension, parameters, mimtype);
            //""application/octet-stream"
            return File(resultCashReciept.MainStream, mimtype);
        }
        public async Task<IActionResult> CashPaymentReport(int OrderId)
        {
            string mimtype = "application/pdf";
            int extension = 4;
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\CashPayment.rdlc";

            var ord = await GetCashPaymentOrders((int)OrderId);
            Dictionary<string, string> parametersCashPayment = new Dictionary<string, string>();
            parametersCashPayment.Add("docNum", ord.DocumentNumber.ToString());
            parametersCashPayment.Add("docDate", ord.documentDate.Value.ToString("dd-MM-yyyy"));

            LocalReport localReportCashPayment = new LocalReport(path);
            JsonResult js = await GetCashPaymentItemNote(OrderId);
            string json = new JavaScriptSerializer().Serialize(js.Value);
            dt = JsonStringToDataTable(json);
            localReportCashPayment.AddDataSource("dsCashPayment", dt);
            var resultCashPayment = localReportCashPayment.Execute(RenderType.Pdf, extension, parametersCashPayment, mimtype);
            //""application/octet-stream"
            return File(resultCashPayment.MainStream, mimtype);
        }
        
        //------------------------------ Accounting Reports ------------
        public async Task<IActionResult> DailyActivityReport(DateTime date)
        {
            string mimtype = "application/pdf";
            int extension = 4;
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\DayBookActivity.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("dttest", date.ToString("dd-MM-yyyy"));
            
            LocalReport localReportDailyActivity = new LocalReport(path);
           
            JsonResult jsPurchases = await GetDateWisePurchases(date);            
            string jsonPurchases = new JavaScriptSerializer().Serialize(jsPurchases.Value);
            var dtPurchases = new DataTable();
            if (jsonPurchases != "{\"data\":[]}")
            {                
                dtPurchases = JsonStringToDataTable(jsonPurchases);
            }
            localReportDailyActivity.AddDataSource("dsPurchases", dtPurchases);

            JsonResult jsPayments = await GetDateWisePayments(date);
            string jsonPayments = new JavaScriptSerializer().Serialize(jsPayments.Value);
            var dtPayments = new DataTable();
            if (jsonPayments != "{\"data\":[]}")
            {
                dtPayments = JsonStringToDataTable(jsonPayments);
            }
            localReportDailyActivity.AddDataSource("dsCashPayments", dtPayments);

            JsonResult jsSales = await GetDateWiseSales(date);
            string jsonSales = new JavaScriptSerializer().Serialize(jsSales.Value);
            var dtSales = new DataTable();
            if (jsonSales != "{\"data\":[]}")
            {
                dtSales = JsonStringToDataTable(jsonSales);
            }
            localReportDailyActivity.AddDataSource("dsSales", dtSales);

            JsonResult jsCashRecieve = await GetDateWiseCashReciepts(date);
            string jsonCashRecieve = new JavaScriptSerializer().Serialize(jsCashRecieve.Value);
            var dtCashRecieve = new DataTable();
            if (jsonCashRecieve != "{\"data\":[]}")
            {
                dtCashRecieve = JsonStringToDataTable(jsonCashRecieve);
            }
            localReportDailyActivity.AddDataSource("dsCashRecieved", dtCashRecieve);

            JsonResult jsExpenses = await GetDateWiseExpenses(date);
            string jsonExpenses = new JavaScriptSerializer().Serialize(jsExpenses.Value);
            var dtExpenses = new DataTable();
            if (jsonExpenses != "{\"data\":[]}")
            {
                dtExpenses = JsonStringToDataTable(jsonExpenses);
            }
            localReportDailyActivity.AddDataSource("dsExpenses", dtExpenses);

            var resultDailyActivity = localReportDailyActivity.Execute(RenderType.Pdf, extension, parameters, mimtype);
            //""application/octet-stream"
            return File(resultDailyActivity.MainStream, mimtype);
        }
        public async Task<IActionResult> StatementOfAccount(int AccountCodeId, DateTime startDate, DateTime endDate)
        {
            string mimtype = "application/pdf";
            int extension = 4;
            var dt = new DataTable();
            var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\StatementOfAccount.rdlc";

            var party = await GetPartyDetails((int)AccountCodeId);
            Dictionary<string, string> parametersStatement = new Dictionary<string, string>();
            parametersStatement.Add("AccountCode", party.AccountCode.ToString());
            parametersStatement.Add("AccountCodeName",   party.AccountCodeDescription.ToString());
            parametersStatement.Add("ContactPerson", party.contactPerson.ToString());
            parametersStatement.Add("Address", party.Address.ToString());
            parametersStatement.Add("phoneNum", party.phone.ToString());
            parametersStatement.Add("faxNum", party.fax.ToString());

            parametersStatement.Add("fromDate", startDate.ToString("dd-MM-yyyy"));
            parametersStatement.Add("toDate", endDate.ToString("dd-MM-yyyy"));

            LocalReport localReportStatement = new LocalReport(path);
            JsonResult js = await GetAccountStatmentList(AccountCodeId, startDate, endDate);
            string json = new JavaScriptSerializer().Serialize(js.Value);
            if (json != "{\"data\":[]}")
            {
                dt = JsonStringToDataTable(json);
            }
            // dt = JsonStringToDataTable(json);
            
            localReportStatement.AddDataSource("dsStatementOfAccount", dt);
            var result = localReportStatement.Execute(RenderType.Pdf, extension, parametersStatement, mimtype);
            return File(result.MainStream, mimtype);
        }
        //=====================================================
        [HttpGet]
        public async Task<JsonResult> GetItemsData()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/getactives";
            var res = await ApiCalls<Item>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetAccountHeads()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/getall";
            var res = await ApiCalls<AccountHead>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetAccountCodes()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/getallrepo";
            var res = await ApiCalls<AccountCodes>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetStock()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/stock/getall";
            var res = await ApiCalls<StockViewModel>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<SalesNote> GetSalesOrders(int id)
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/salesRepo/{id}";
                var res = ApiCalls<SalesNote>.GetById(apiUrl);
                return await res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetSaleItemNote(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/saleRepoItems/{id}";
            var res = await ApiCalls<SalesNoteItems>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<CashRecieveds> GetCashRecieptOrders(int id)
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashrecieved/{id}";
                var res = ApiCalls<CashRecieveds>.GetById(apiUrl);
                return await res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetCashRecieptItemNote(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashItemsRepo/{id}";
            var res = await ApiCalls<CashRecieptItems>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<CashPayments> GetCashPaymentOrders(int id)
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashpayment/{id}";
                var res = ApiCalls<CashPayments>.GetById(apiUrl);
                return await res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetCashPaymentItemNote(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashpaymentitemsRepo/{id}";
            var res = await ApiCalls<CashPaymentItemsRepo>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<PartyCodes> GetPartyDetails(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycodeRepo/{id}";
            var res = await ApiCalls<PartyCodes>.GetById(apiUrl);
            return res;
        }
        
        //--------------------- Accounting Reports --------------------
        [HttpGet]
        public async Task<JsonResult> GetDateWisePurchases(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/purchasesdatewiseRepo/{date}";
            var res = await ApiCalls<PurchaseNoteDateWise>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetDateWiseSales(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/salesdatewiseRepo/{date}";
            var res = await ApiCalls<SaleNoteDateWise>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetDateWisePayments(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashpaymentsdatewiseRepo/{date}";
            var res = await ApiCalls<CashPaymentDateWise>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetDateWiseCashReciepts(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashrecieveddatewiseRepo/{date}";
            var res = await ApiCalls<CashRecievedDateWise>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetDateWiseExpenses(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expensesdatewiseRepo/{date}";
            var res = await ApiCalls<ExpensesReport>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetAccountStatmentList(int? AccountCodeId, DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");
            
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/statementofaccount/{AccountCodeId}/{StartDate}/{EndDate}";
            var res = await ApiCalls<StatementOfAccountViewModel>.GetDataById(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        //==========================================================
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> AccountCodeDropDownData()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/dropdown/";
                string res = await ApiCalls<string>.GetDropDownList(apiUrl);
                List<SelectListItem> record = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(res);
                return record;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //==========================================================
        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("{\"data\":[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = (RowDataString == "null")?"":RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }
        //===================================================
        [HttpGet]
        public async Task<IEnumerable<Item>> GetItemsClassData()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/getactives";

            Task<IEnumerable<Item>> res;
            res = ApiCalls<Item>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<AccountHead>> GetAccountHeadsClassData()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/getactives";

            Task<IEnumerable<AccountHead>> res;
            res = ApiCalls<AccountHead>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<AccountCodes>> GetAccountCodesClassData()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/getallrepo";

            Task<IEnumerable<AccountCodes>> res;
            res = ApiCalls<AccountCodes>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<StockViewModel>> GetStockClassData()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/stock/getall";

            Task<IEnumerable<StockViewModel>> res;
            res = ApiCalls<StockViewModel>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SalesNoteItems>> GetSaleItemNoteClassData(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/saleRepoItems/{id}";
            Task<IEnumerable<SalesNoteItems>> res;
            res = ApiCalls<SalesNoteItems>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<CashRecieptItems>> GetCashRecieptItemNoteClassData(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashItemsRepo/{id}";
            Task<IEnumerable<CashRecieptItems>> res;
            res = ApiCalls<CashRecieptItems>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<CashPaymentItemsRepo>> GetCashPaymentItemNoteClassData(int id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashpaymentitemsRepo/{id}";
            Task<IEnumerable<CashPaymentItemsRepo>> res;
            res = ApiCalls<CashPaymentItemsRepo>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<StatementOfAccountViewModel>> GetAccountStatmentListClassData(int? AccountCodeId, DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/statementofaccount/{AccountCodeId}/{StartDate}/{EndDate}";
            Task<IEnumerable<StatementOfAccountViewModel>> res;
            res = ApiCalls<StatementOfAccountViewModel>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<PurchaseNoteDateWise>> GetDateWisePurchasesClassData(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/purchasesdatewiseRepo/{date}";
            
            Task<IEnumerable<PurchaseNoteDateWise>> res;
            res = ApiCalls<PurchaseNoteDateWise>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SaleNoteDateWise>> GetDateWiseSalesClassData(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/salesdatewiseRepo/{date}";

            Task<IEnumerable<SaleNoteDateWise>> res;
            res = ApiCalls<SaleNoteDateWise>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<CashPaymentDateWise>> GetDateWisePaymentsClassData(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashpaymentsdatewiseRepo/{date}";

            Task<IEnumerable<CashPaymentDateWise>> res;
            res = ApiCalls<CashPaymentDateWise>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<CashRecievedDateWise>> GetDateWiseCashRecieptsClassData(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/cashrecieveddatewiseRepo/{date}";

            Task<IEnumerable<CashRecievedDateWise>> res;
            res = ApiCalls<CashRecievedDateWise>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<ExpensesReport>> GetDateWiseExpensesClassData(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expensesdatewiseRepo/{date}";

            Task<IEnumerable<ExpensesReport>> res;
            res = ApiCalls<ExpensesReport>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<NewOpeningBalanceVM> GetOpeningBalanceDateWiseClassData(DateTime dat)
        {
            string date = dat.ToString("yyyy-MM-dd");
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dayopeningdatewise/{date}";

            Task<NewOpeningBalanceVM> res;
            res = ApiCalls<NewOpeningBalanceVM>.GetSingleData(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SalesNote>> GetSalesByRangeClassData(DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/salesrange/{StartDate}/{EndDate}";
            Task<IEnumerable<SalesNote>> res;
            res = ApiCalls<SalesNote>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SalesGoods>> GetPartySalesListClassData(int? AccountCodeId, DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/partysales/{AccountCodeId}/{StartDate}/{EndDate}";
            Task<IEnumerable<SalesGoods>> res;
            res = ApiCalls<SalesGoods>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<PurchaseGood>> GetPartyPurchasesListClassData(int? AccountCodeId, DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/partypurchase/{AccountCodeId}/{StartDate}/{EndDate}";
            Task<IEnumerable<PurchaseGood>> res;
            res = ApiCalls<PurchaseGood>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<PurchasesNote>> GetPurchasesByRangeClassData(DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/purchaserange/{StartDate}/{EndDate}";
            Task<IEnumerable<PurchasesNote>> res;
            res = ApiCalls<PurchasesNote>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<PurchasesNote>> GetCashPurchasesClassData(DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/cashpurchaserange/{StartDate}/{EndDate}";
            Task<IEnumerable<PurchasesNote>> res;
            res = ApiCalls<PurchasesNote>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<CashPaymentItemsRepo>> GetCashPaymentByRangeClassData(DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/cashpaymentrange/{StartDate}/{EndDate}";
            Task<IEnumerable<CashPaymentItemsRepo>> res;
            res = ApiCalls<CashPaymentItemsRepo>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<CashRecieptItems>> GetCashRecieptByRangeClassData(DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/cashrecieptrange/{StartDate}/{EndDate}";
            Task<IEnumerable<CashRecieptItems>> res;
            res = ApiCalls<CashRecieptItems>.GetDataById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<ExpensesReport>> GetExpensesByRangeClassData(DateTime? startDate, DateTime? endDate)
        {
            string StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
            string EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");

            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/reports/expensesrange/{StartDate}/{EndDate}";
            Task<IEnumerable<ExpensesReport>> res;
            res = ApiCalls<ExpensesReport>.GetDataById(apiUrl);
            return await res;
        }
        //---------------------------------------------------

        //===================================================
        public IActionResult DemoViewAsPDF()
        {
            return new ViewAsPdf("DemoViewAsPDF");
        }
    }
}
