using Cooperative.DBConnector;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Xml;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Cooperative.Controllers;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Cooperative.Models
{
    public class CMK_BeneficaryRFTModel
    {
        public Int32 CMK_DBT_RFT_ID { get; set; }
        public Int32 FINANCIAL_YEAR_ID { get; set; }
        public Int32 SCHEME_ID { get; set; }
        public Int32 QUADRIMESTER_ID { get; set; }
        public Int32 INSTALLMENT_TYPE_ID { get; set; }
        public long CREATED_BY { get; set; }

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        public CMK_BeneficaryRFTModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
        }
        public CMK_BeneficaryRFTModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();

        }
        public bool GenerateBeneficiaryListDBT(CMK_BeneficaryRFTModel cMK_BeneficaryRFT)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_CMK_DBT_BeneficiaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FINANCIAL_YEAR_ID", DBFactoryParameterType.Int, cMK_BeneficaryRFT.FINANCIAL_YEAR_ID);
                objDBAccess.AddParameterToCommand("@QUADRIMESTER_ID", DBFactoryParameterType.Int, cMK_BeneficaryRFT.QUADRIMESTER_ID);
                objDBAccess.AddParameterToCommand("@SCHEME_ID", DBFactoryParameterType.Int, cMK_BeneficaryRFT.SCHEME_ID);
                objDBAccess.AddParameterToCommand("@INSTALLMENT_TYPE_ID", DBFactoryParameterType.Int, cMK_BeneficaryRFT.INSTALLMENT_TYPE_ID);
                objDBAccess.AddParameterToCommand("@CREATED_BY", DBFactoryParameterType.Int, cMK_BeneficaryRFT.CREATED_BY);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GenerateBLDBT");
                return objDBAccess.ExecuteCommand() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GenerateBeneficiaryListDBT(CMK_BeneficaryRFTModel pMK_BeneficaryRFT)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        public DataSet Get_RFTDBT_List()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_CMK_DBT_BeneficiaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetRFTDBT_List");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "Get_RFTDBT_List()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet Get_RFTDBT_Details(string id_enc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_CMK_DBT_BeneficiaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CMK_DBT_RFT_ID_ENC", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetRFTDBT_Details");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "Get_RFTDBT_Details(string id_enc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public bool GenerateBeneficiaryDBTFiles(string id_enc, string dbt_date)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_GenerateDebitTransactionHead", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CMK_DBT_RFT_ID_ENC", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@TRAN_DATE_TIME", DBFactoryParameterType.VARCHAR, dbt_date);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GenerateTransactionHead");
                return objDBAccess.ExecuteCommand() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GenerateBeneficiaryDBTFiles(string id_enc, string dbt_date)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        public DataSet GetDBT_HeadDetailsBY_DH_ID(string id_enc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_GenerateDebitTransactionHead", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CMK_DBT_RFT_ID_ENC", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDBTHeadDetails");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GetDBT_HeadDetailsBY_DH_ID(string id_enc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;

        }
        public DataSet GetBeneficiaryDBTFiles(string id_enc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_GenerateDebitTransactionHead", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CMK_DBT_RFT_ID_ENC", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetTransactionHead");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GetBeneficiaryDBTFiles(string id_enc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;

        }
        public bool GenerateDBT_XML(string id_enc, long userId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Generate_DBT_TransactionXML", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GenerateXML");
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_Enc", DBFactoryParameterType.NVarchar, id_enc);
                var ds = objDBAccess.GetDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    // Write XML content to a file
                    string filePath = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFTNO"].ToString()}/");
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>";
                    xmlContent += dt.Rows[0]["XML_CONTENT"].ToString();
                    xmlContent = Regex.Replace(xmlContent, @"\s+/ >", "/>");
                    xmlContent = Regex.Replace(xmlContent, @"\s+/>", "/>");
                    xmlContent = Regex.Replace(xmlContent, @"\r\n|\r|\n", "").Trim();
                    //XElement customerElement = XElement.Parse(xmlContent);
                    //customerElement.Save(filePath + $"{dt.Rows[0]["DEBIT_REFERENCE"]}.xml");
                    File.WriteAllText(filePath + $"{dt.Rows[0]["DEBIT_REFERENCE"]}.xml", xmlContent);
                    if (UpdateXmlInfo(id_enc, userId))
                    {
                        return true;
                    }
                    else
                        return false;

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GenerateDBT_XML(string id_enc, long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        private bool UpdateXmlEsignInfo(string id_enc, long userId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Generate_DBT_TransactionXML", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateXmlEsignInfo");
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_Enc", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, userId);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "UpdateXmlEsignInfo(string id_enc, long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        private bool UpdateXmlEsignEncryptedInfo(string id_enc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Generate_DBT_TransactionXML", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateXmlEsignEncryptedInfo");
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_Enc", DBFactoryParameterType.NVarchar, id_enc);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "UpdateXmlEsignEncryptedInfo(string id_enc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        private bool UpdateXmlInfo(string id_enc, long userId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Generate_DBT_TransactionXML", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateXmlInfo");
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_Enc", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, userId);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "UpdateXmlInfo(string id_enc, long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        public DataSet GetDebitHeadDetails(string id_enc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_GenerateDebitTransactionHead", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_Enc", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDebitHeadDetails");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GetDebitHeadDetails(string id_enc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public int EsignXML(string id_enc, string password, long userId)
        {
            string certPath = HttpContext.Current.Server.MapPath("~/Content/CMK_DBT/Certificate/Rajcomp_Document_signer_class_2.pfx");
            //string certPath = HttpContext.Current.Server.MapPath("~/Content/CMK_DBT/Certificate/kaushal_gupta_encryption.pfx");
            try
            {
                DataSet ds = GetDebitHeadDetails(id_enc);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    string path = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO"].ToString()}/{dt.Rows[0]["XML_FILE"].ToString()}");
                    string outPath = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO"].ToString()}/eSigned/");
                    if (!Directory.Exists(outPath))
                        Directory.CreateDirectory(outPath);
                    outPath = Path.Combine(outPath, dt.Rows[0]["XML_FILE"].ToString());
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(path);

                    X509Certificate2 cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);
                    SignXmlWithRSA_SHA256(xmlDoc, cert);
                    xmlDoc.Save(outPath);
                    if (UpdateXmlEsignInfo(id_enc, userId))
                    {
                        return 1;
                    }
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = $"EsignXML(string id_enc, long userId)" }, ex);
                if (ex.Message.Contains("network password is not correct"))
                    return 2;
            }
            return 0;
        }

        public bool EncryptXML(string id_enc)
        {
            try
            {
                DataSet ds = GetDebitHeadDetails(id_enc);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    string filePath = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO"].ToString()}/");
                    string certPubKey = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/Certificate/DBT-140326.cer");
                    string outFilePath = Path.Combine(filePath, "Encrypted");
                    if (!Directory.Exists(outFilePath))
                        Directory.CreateDirectory(outFilePath);
                    outFilePath = Path.Combine(outFilePath, dt.Rows[0]["XML_FILE"].ToString());
                    filePath = Path.Combine(filePath, "Esigned", dt.Rows[0]["XML_FILE"].ToString());

                    RSAAESEncryption.Encrypt(filePath, outFilePath, certPubKey);
                    if (UpdateXmlEsignEncryptedInfo(id_enc))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "EncryptXML(string id_enc)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
            }
            return false;
        }

        public bool DecryptResponseXML(string password)
        {
            try
            {
                string filePath = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/RJ-CMK2025-260004/Responses/");
                string pvtKey = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/Certificate/privateKey.jks");
                string outFilePath = filePath;
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                if (!Directory.Exists(outFilePath))
                    Directory.CreateDirectory(outFilePath);

                string[] files = Directory.GetFiles(Path.Combine(filePath, "Encrypted"));
                foreach (string file in files)
                {
                    RSAAESEncryption.Decrypt(Path.Combine(filePath, "Encrypted", Path.GetFileName(file)), Path.Combine(outFilePath, Path.GetFileName(file)?.Replace("_RESP", "")), pvtKey, password);
                }
                return true;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "DecryptResponseXML(string password)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
            }
            return false;
        }
        public DataSet GetBeneficiaryData(string dbt_mst_id_enc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_CMK_DBT_BeneficiaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_ENC", DBFactoryParameterType.NVarchar, dbt_mst_id_enc);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetBeneficiaryListT_List");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GetBeneficiaryData(string dbt_mst_id_enc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_enc">Here id_enc is ecrypted Debit Master ID id</param>
        /// <returns></returns>
        public System.Web.UI.WebControls.ListItem GenerateRFT_BILL(string id_enc)
        {
            var li = new System.Web.UI.WebControls.ListItem();
            try
            {
                string filePath = "", filePathMapped = "";

                DataSet ds = GetBeneficiaryData(id_enc);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataTable dtB = ds.Tables[1];
                    string pathDir = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO_DIR"]}/RFT_BILL/");
                    if (!Directory.Exists(pathDir))
                        Directory.CreateDirectory(pathDir);
                    filePath = $"{dt.Rows[0]["BILL_NO_DIR"]}-Sanction-Order.pdf";
                    filePathMapped = Path.Combine(pathDir, filePath);

                    GenerateRFT_BILL_PDF(filePathMapped, dt, dtB);
                    li.Text = filePath;
                    li.Enabled = true;
                    return li;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "GenerateRFT_BILL(string id_enc)" }, ex);
            }
            li.Text = "";
            li.Enabled = false;
            return li;
        }
        private void GenerateRFT_BILL_PDF(string filePath, DataTable dt, DataTable dtB)
        {
            string installmentName = dt.Rows[0]["INST_SHORT_NAME"].ToString();
            string amount = Convert.ToDouble(dt.Rows[0]["AMOUNT"]).ToString("0.00");
            string amountTotal = (Convert.ToDouble(dt.Rows[0]["ALLOCATED_BENEFICIARY"]) * Convert.ToDouble(dt.Rows[0]["AMOUNT"])).ToString("0.00");
            Document document = new Document(PageSize.A4, 36, 36, 36, 36);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // Title and Header
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            Font italicFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10);
            Font underlineFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, Font.UNDERLINE);


            Paragraph subtitle = new Paragraph("(Government of Rajasthan)", subtitleFont);
            subtitle.Alignment = Element.ALIGN_CENTER;
            document.Add(subtitle);
            // Title Section
            Paragraph title = new Paragraph("OFFICE OF THE REGISTRAR, COOPERATIVE SOCIETIES, RAJASTHAN, JAIPUR", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10;
            document.Add(title);

            Paragraph sanctionOrder = new Paragraph(new Chunk("Sanction Order", underlineFont));
            sanctionOrder.Alignment = Element.ALIGN_CENTER;
            sanctionOrder.SpacingAfter = 20;
            document.Add(sanctionOrder);
            // Sanction Details
            Chunk sdC0 = new Chunk("Sanction is hereby accorded as mentioned below for the ", regularFont);
            Chunk sdC01 = new Chunk($"{installmentName} installment ({amount}/- each beneficiary) ", boldFont);
            Chunk sdC1 = new Chunk("for payment of ", regularFont);
            //Chunk sdC1 = new Chunk("Sanction is hereby accorded as mentioned below for payment of ", regularFont);
            Chunk sdC2 = new Chunk($"{amountTotal}/- ({NumberToWordsConverter.ConvertAmountToWords(Convert.ToDecimal(amountTotal))}) ", boldFont);
            Chunk sdC3 = new Chunk("under ", regularFont);
            Chunk sdC4 = new Chunk($"{dt.Rows[0]["SchemeName_EN"].ToString()} ", boldFont);
            Chunk sdC5 = new Chunk($"for the F.Y. ", regularFont);
            Chunk sdC6 = new Chunk($"{dt.Rows[0]["FINANCIAL_YEAR"].ToString()} ", boldFont);
            Chunk sdC7 = new Chunk("as per details mentioned below:", regularFont);
            Paragraph sanctionDetails = new Paragraph();
            sanctionDetails.SpacingAfter = 15;
            sanctionDetails.Add(sdC0);
            sanctionDetails.Add(sdC01);
            sanctionDetails.Add(sdC1);
            sanctionDetails.Add(sdC2);
            sanctionDetails.Add(sdC3);
            sanctionDetails.Add(sdC4);
            sanctionDetails.Add(sdC5);
            sanctionDetails.Add(sdC6);
            sanctionDetails.Add(sdC7);
            document.Add(sanctionDetails);

            // Main Table
            PdfPTable mainTable = new PdfPTable(5);
            mainTable.WidthPercentage = 100;
            mainTable.SetWidths(new float[] { 1, 4, 3, 3, 3 });

            // Main Table Header
            mainTable.AddCell(new PdfPCell(new Phrase("Sr. No.", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            mainTable.AddCell(new PdfPCell(new Phrase("Name of Beneficiary", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            mainTable.AddCell(new PdfPCell(new Phrase("Bill No", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            mainTable.AddCell(new PdfPCell(new Phrase("Work Performed", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            mainTable.AddCell(new PdfPCell(new Phrase("Amount", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1 });

            // Main Table Data Row
            mainTable.AddCell(new PdfPCell(new Phrase("1", regularFont)));
            mainTable.AddCell(new PdfPCell(new Phrase("As Attached Sheet", regularFont)));
            mainTable.AddCell(new PdfPCell(new Phrase(dt.Rows[0]["BILL_NO"].ToString(), regularFont)));
            mainTable.AddCell(new PdfPCell(new Phrase($"({dt.Rows[0]["ALLOCATED_BENEFICIARY"]} beneficiaries)", regularFont)));
            mainTable.AddCell(new PdfPCell(new Phrase(amountTotal + "/-", regularFont)) { HorizontalAlignment = 1 });

            // Net Payable Row
            PdfPCell emptyCell = new PdfPCell(new Phrase(""));
            mainTable.AddCell(emptyCell);
            mainTable.AddCell(new PdfPCell(new Phrase("Net Payable Amount to Beneficiaries", boldFont)) { Colspan = 3, HorizontalAlignment = 2 });
            mainTable.AddCell(new PdfPCell(new Phrase($"{amountTotal}/-", boldFont)) { HorizontalAlignment = 1 });

            document.Add(mainTable);

            // Approval Text
            Paragraph approvalText = new Paragraph("This bears approval of competent authority.", regularFont);
            approvalText.SpacingBefore = 30;
            document.Add(approvalText);

            // Signature and Date
            Paragraph signature = new Paragraph("Authorized Signatory", regularFont);
            signature.Alignment = Element.ALIGN_RIGHT;
            signature.SpacingBefore = 20;
            document.Add(signature);

            Paragraph date = new Paragraph("Date: _____________", regularFont);
            date.Alignment = Element.ALIGN_RIGHT;
            document.Add(date);

            // Reference Note
            Paragraph refNo = new Paragraph("Ref. No:", italicFont);
            refNo.SpacingBefore = 20;
            document.Add(refNo);

            Chunk cfC1 = new Chunk($"Copy forwarded to the following for information and necessary action:\n", italicFont);
            Chunk cfC2 = new Chunk("1. PS to Registrar, Cooperative Societies\n" +
                "2. PS to MD RISL\n" +
                "3. ASNO CM Kisan\n" +
                "4. Project OIC RISL\n" +
                "5. Guard File", regularFont);
            Paragraph copyForwarded = new Paragraph();
            copyForwarded.Add(cfC1);
            copyForwarded.Add(cfC2);
            copyForwarded.SpacingBefore = 10;
            document.Add(copyForwarded);

            // Footer Signature
            Paragraph footerSignature = new Paragraph("Authorized Signatory", regularFont);
            footerSignature.Alignment = Element.ALIGN_RIGHT;
            footerSignature.SpacingBefore = 20;
            document.Add(footerSignature);

            Paragraph footerDate = new Paragraph("Date: _____________", regularFont);
            footerDate.Alignment = Element.ALIGN_RIGHT;
            document.Add(footerDate);

            document.NewPage();
            // Detailed Table for Beneficiaries
            PdfPTable detailsTable = new PdfPTable(5);
            detailsTable.WidthPercentage = 100;
            detailsTable.SetWidths(new float[] { 1, 3, 4, 4, 2 });
            detailsTable.SpacingBefore = 20;

            // Detailed Table Header
            detailsTable.AddCell(new PdfPCell(new Phrase("Sr. No", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            detailsTable.AddCell(new PdfPCell(new Phrase("District Name", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            detailsTable.AddCell(new PdfPCell(new Phrase("Name of Beneficiary", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            detailsTable.AddCell(new PdfPCell(new Phrase("Aadhaar ID", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            detailsTable.AddCell(new PdfPCell(new Phrase("Amount", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1 });

            // Example Data Rows
            if (dtB != null && dtB.Rows.Count > 0)
            {
                int counter = 0;
                foreach (DataRow dr in dtB.Rows)
                {
                    counter++;
                    detailsTable.AddCell(new PdfPCell(new Phrase(counter.ToString(), regularFont)));
                    detailsTable.AddCell(new PdfPCell(new Phrase(dr["DistrictName"].ToString(), regularFont)));
                    detailsTable.AddCell(new PdfPCell(new Phrase(dr["FarmerName"].ToString(), regularFont)));
                    detailsTable.AddCell(new PdfPCell(new Phrase(dr["AadhaarNo"].ToString(), regularFont)));
                    detailsTable.AddCell(new PdfPCell(new Phrase(dr["Amount"].ToString(), regularFont)) { HorizontalAlignment = 1 });
                }

                document.Add(detailsTable);
            }
            document.Close();
        }

        public bool UpdateRFT_BILL_Info(string id_enc, string bill_path)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_CMK_DBT_BeneficiaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateRFT_BILL");
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_ENC", DBFactoryParameterType.NVarchar, id_enc);
                //objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, userId);
                objDBAccess.AddParameterToCommand("@BILL_PATH", DBFactoryParameterType.VARCHAR, bill_path);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "UpdateRFT_BILL_Info(string id_enc, string bill_path)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        public bool UpdateRFT_BILL_eSignInfo(string id_enc, string esigned_bill_path, long userId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_CMK_DBT_BeneficiaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateRFT_BILL_eSign");
                objDBAccess.AddParameterToCommand("@CMK_DebitMasterId_ENC", DBFactoryParameterType.NVarchar, id_enc);
                objDBAccess.AddParameterToCommand("@CREATED_BY", DBFactoryParameterType.Int, userId);
                objDBAccess.AddParameterToCommand("@BILL_PATH", DBFactoryParameterType.VARCHAR, esigned_bill_path);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "UpdateRFT_BILL_eSignInfo(string id_enc, string esigned_bill_path,long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        private static void SignXmlWithRSA_SHA256(XmlDocument xmlDoc, X509Certificate2 cert)
        {
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");

            if (cert == null)
                throw new ArgumentException("cert");
            if (!cert.HasPrivateKey)
                throw new CryptographicException("The certificate does not have a private key.");
            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = cert.GetRSAPrivateKey(); // Get the RSA private key from the certificate
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
            Reference reference = new Reference();
            reference.Uri = "";
            reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedXml.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }
        public bool ReadXML(string rftNumber, int startfrom = 0)
        {

            // upload dbt response for RJ-CMK2024-250001
            string inputFile = HttpContext.Current.Server.MapPath($"~/Content/CMK_DBT/{rftNumber}/Responses/");
            DataSet dataSet = new DataSet();

            string[] files = Directory.GetFiles(inputFile);
            foreach (string file in files)
            {
                if (Convert.ToInt32(file.Replace("D:\\Cooperative\\CoopWeb\\Content\\CMK_DBT\\RJ-CMK2025-260004\\Responses\\DBTRJ00118102026", "").Replace(".xml",""))>=startfrom)
                {

                    dataSet = new DataSet();
                    using (var reader = XmlTextReader.Create($"{inputFile}/{Path.GetFileName(file)}"))
                    {
                        dataSet.ReadXml(reader);
                    }

                    // Display the data in DataSet tables
                    string json = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dataSet.Tables[2]);
                    //var responseString = "";
                    var responseString = Task.Run(() =>
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        var client = new HttpClient();
                        var formData = new MultipartFormDataContent
                        {
                           { new StringContent(json), "jsonString" }
                        };
                        var response = client.PostAsync("https://rajsahakarapp.rajasthan.gov.in/CommonServices/UploadDbtResponse", formData).Result;
                        response.EnsureSuccessStatusCode();
                        return response.Content.ReadAsStringAsync().Result;
                    }).Result;
                    if (responseString != null)
                    {
                        json = "";
                        dataSet = null;
                    }
                }
            }
            return true;
        }


        public bool UploadDbtResponse(string jsonString)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Insert_DBT_Response", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "InsertJsonResponse");
                objDBAccess.AddParameterToCommand("@json", DBFactoryParameterType.NVarchar, jsonString);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CMK_BeneficaryRFTModel", FunctionName = "UploadDbtResponse(string jsonString)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }

    }
}