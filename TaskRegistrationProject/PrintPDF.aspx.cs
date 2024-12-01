using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskRegistrationProject
{
    public partial class PrintPDF : System.Web.UI.Page
    {
        OracleConnection conStr;
        OracleDataAdapter oda;
        DataTable dt;
        string sQuery = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            conStr = new OracleConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["RegisterDetails"];

                sQuery = "SELECT NAME,TO_CHAR(DOB,'DD/MM/YYYY') AS DOB,TO_CHAR(TRANSACTION_DATE,'DD/MM/YYYY') AS TRANSACTION_DATE,ADDRESS,CITY,STATE,ZIPCODE,COUNTRY,PHONENUMBER,EMAIL,COURSE,MEMBERSHIPTYPE,PAYMENTMETHOD,COMMENTS FROM USERINFORMATION WHERE REGISTER_ID=:REGISTER_ID";
                oda = new OracleDataAdapter(sQuery, conStr);
                oda.SelectCommand.Parameters.AddWithValue(":REGISTER_ID", Session["RegisterID"].ToString());
                oda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["NAME"].ToString();
                    txtDate.Text = dt.Rows[0]["TRANSACTION_DATE"].ToString();
                    txtDOB.Text = dt.Rows[0]["DOB"].ToString();
                    txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                    txtCity.Text = dt.Rows[0]["CITY"].ToString();
                    txtState.Text = dt.Rows[0]["STATE"].ToString();
                    txtCountry.Text = dt.Rows[0]["COUNTRY"].ToString();
                    txtZip.Text = dt.Rows[0]["ZIPCODE"].ToString();
                    txtPhone.Text = dt.Rows[0]["PHONENUMBER"].ToString();
                    txtEmail.Text = dt.Rows[0]["EMAIL"].ToString();
                    txtMembership.Text = dt.Rows[0]["MEMBERSHIPTYPE"].ToString();
                    txtCourse.Text = dt.Rows[0]["COURSE"].ToString();
                    txtPayment.Text = dt.Rows[0]["PAYMENTMETHOD"].ToString();
                    txtComments.Text = dt.Rows[0]["COMMENTS"].ToString();
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(pdfDoc, ms);

                    pdfDoc.Open();

                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD);
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    Font sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                    Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);


                    Paragraph title = new Paragraph("REGISTRATION FORM", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20f
                    };
                    pdfDoc.Add(title);

                    pdfDoc.Add(DateRow());

                    #region Personal Info
                    AddSectionHeader(pdfDoc, "PERSONAL INFORMATION", sectionFont);

                    PdfPTable perInfotable = CreateTable(2);

                    AddTableRow(perInfotable, "NAME ", txtName.Text);
                    AddTableRow(perInfotable, "DATE OF BIRTH ", txtDOB.Text);

                    pdfDoc.Add(perInfotable);
                    #endregion

                    pdfDoc.Add(new Paragraph("\n"));

                    #region Contact Info
                    AddSectionHeader(pdfDoc, "CONTACT INFORMATION", sectionFont);

                    PdfPTable contactInfoTable = CreateTable(4);

                    AddRowToTable(contactInfoTable, "ADDRESS", txtAddress.Text, normalFont, "", "", colspan: 3);
                    AddRowToTable(contactInfoTable, "CITY", txtCity.Text, normalFont, "PROVINCE/STATE", txtState.Text);
                    AddRowToTable(contactInfoTable, "ZIP CODE", txtZip.Text, normalFont, "COUNTRY", txtCountry.Text);
                    AddRowToTable(contactInfoTable, "PHONE", txtPhone.Text, normalFont, "EMAIL", txtEmail.Text);
                    AddRowToTable(contactInfoTable, "MEMBERSHIP TYPE", "Regular", normalFont, "", "", colspan: 3);

                    pdfDoc.Add(contactInfoTable);

                    #endregion

                    pdfDoc.Add(new Paragraph("\n"));

                    #region Course Info
                    AddSectionHeader(pdfDoc, "COURSE INFORMATION", sectionFont);

                    PdfPTable courseInfotable = CreateTable(2);

                    AddTableRow(courseInfotable, "COURSE NAME ", txtCourse.Text);
                    AddTableRow(courseInfotable, "PAYMENT DETAILS ", txtPayment.Text);
                    AddTableRow(courseInfotable, "COMMENTS ", txtComments.Text);

                    pdfDoc.Add(courseInfotable);
                    #endregion

                    pdfDoc.Close();

                    byte[] pdfBytes = ms.ToArray();

                    // Send the PDF to the browser for download
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=RegistrationDetails.pdf");
                    Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                    Response.AddHeader("Pragma", "no-cache");
                    Response.AddHeader("Expires", "0");

                    Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                showToast("PDF Dowloaded Successful!!");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "RedirectPage", "RedirectPage()", true);
            }
            catch (Exception ex)
            {
                // Handle the exception here (e.g., logging)
            }
        }

        private void AddTableRow(PdfPTable table, string label, string value)
        {
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
            Font valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
            labelCell.Border = PdfPCell.NO_BORDER;
            labelCell.PaddingBottom = 8f; // Space between rows
            labelCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
            valueCell.Border = PdfPCell.NO_BORDER;
            valueCell.PaddingBottom = 8f;
            valueCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(valueCell);
        }

        private PdfPTable CreateTable(int columns)
        {
            PdfPTable table = new PdfPTable(columns)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 10f
            };
            return table;
        }

        private void AddRowToTable(PdfPTable table, string col1, string col2, Font normalFont, string col3 = "", string col4 = "", int colspan = 1)
        {
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);

            PdfPCell cell1 = new PdfPCell(new Phrase(col1, boldFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 8f
            };
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(col2, normalFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 8f,
                Colspan = colspan
            };
            table.AddCell(cell2);

            if (!string.IsNullOrEmpty(col3))
            {
                PdfPCell cell3 = new PdfPCell(new Phrase(col3, boldFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 8f
                };
                table.AddCell(cell3);
            }

            if (!string.IsNullOrEmpty(col4))
            {
                PdfPCell cell4 = new PdfPCell(new Phrase(col4, normalFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 8f
                };
                table.AddCell(cell4);
            }
        }

        public Paragraph DateRow()
        {
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
            Font valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            Chunk dateLabel = new Chunk("Date: ", labelFont);
            Chunk dateValue = new Chunk(txtDate.Text, valueFont);

            Paragraph dateParagraph = new Paragraph();
            dateParagraph.Add(dateLabel);
            dateParagraph.Add(dateValue);
            dateParagraph.Alignment = Element.ALIGN_RIGHT;
            dateParagraph.SpacingAfter = 20f;

            return dateParagraph;
        }

        private void AddSectionHeader(Document doc, string title, Font font)
        {
            Paragraph sectionTitle = new Paragraph(title, font)
            {
                SpacingAfter = 5f,
            };
            doc.Add(sectionTitle);
            LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 0);
            doc.Add(line);
        }

        private void showToast(string msg, string type = "success")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "<script>toastr." + type + "('" + msg + "');</script>", false);
        }
    }
}
