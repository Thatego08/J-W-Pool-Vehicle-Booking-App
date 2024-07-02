using DinkToPdf;
using DinkToPdf.Contracts;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{

    //Comment to disable locking
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
        [ApiController]
        public class InspectionChecklistController : ControllerBase
        {

          private readonly BookingDbContext _context;
        private readonly IInspectionListRepository _checklistRepository;
            private readonly ILogger<InspectionChecklistController> _logger;
            private readonly IConverter _pdfConverter;

        public InspectionChecklistController(IInspectionListRepository checklistRepository, ILogger<InspectionChecklistController> logger, IConverter pdfConverter, BookingDbContext context)
            {
             _context = context;
                _checklistRepository = checklistRepository ?? throw new ArgumentNullException(nameof(checklistRepository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _pdfConverter = pdfConverter ?? throw new ArgumentNullException(nameof(pdfConverter));

        }

            [HttpPost]
            [Route("AddChecklistItem")]
            public async Task<IActionResult> AddChecklistItem(InspectionListViewModel checklistVM)
            {
                if (checklistVM == null)
                {
                    return BadRequest("Checklist item model is null.");
                }

                var checklistItem = new InspectionList
                {
                    Item = checklistVM.Item,
                    IsCompleted = checklistVM.IsCompleted
                };

                try
                {
                    await _checklistRepository.AddChecklistItemAsync(checklistItem);
                    return Ok("Checklist item added successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding the checklist item.");
                    return StatusCode(500, "Internal server error, contact support.");
                }
            }

            [HttpGet]
            [Route("GetChecklist")]
            public async Task<IActionResult> GetChecklist()
            {
                try
                {
                    var results = await _checklistRepository.GetChecklistAsync();
                    return Ok(results);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while retrieving the checklist.");
                    return StatusCode(500, "Internal server error, contact support.");
                }
            }

            [HttpPut]
            [Route("EditChecklistItem/{checklistID}")]
            public async Task<IActionResult> EditChecklistItem(int checklistID, InspectionListViewModel checklistVM)
            {
                if (checklistVM == null)
                {
                    return BadRequest("Checklist item model is null.");
                }

                try
                {
                    var existingItem = await _checklistRepository.GetChecklistItemAsync(checklistID);
                    if (existingItem == null) return NotFound("Checklist item does not exist.");

                    existingItem.Item = checklistVM.Item;
                    existingItem.IsCompleted = checklistVM.IsCompleted;

                    await _checklistRepository.UpdateChecklistItemAsync(existingItem);
                    return Ok("Checklist item updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the checklist item.");
                    return StatusCode(500, "Internal server error, contact support.");
                }
            }

            [HttpGet]
            [Route("ExportCsv")]
            public async Task<IActionResult> ExportCsv()
            {
                try
                {
                    var checklists = await _checklistRepository.GetChecklistAsync();
                    var csv = ConvertToCsv(checklists);
                    var bytes = Encoding.UTF8.GetBytes(csv);
                    return File(bytes, "text/csv", "inspection_checklist.csv");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while exporting the checklist to CSV.");
                    return StatusCode(500, "Internal server error, contact support.");
                }
            }

            private string ConvertToCsv(InspectionList[] checklists)
            {
                var csv = new StringBuilder();
                csv.AppendLine("ChecklistID,Item,IsCompleted");

                foreach (var item in checklists)
                {
                    csv.AppendLine($"{item.ChecklistID},{item.Item},{item.IsCompleted}");
                }

                return csv.ToString();
            }

        [HttpGet]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf()
        {

            var inspections = _context.InspectionLists.ToList();
            if (inspections == null || !inspections.Any())
            {
                return NotFound("No inspection data found.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Inspection List"));

                foreach (var inspection in inspections)
                {
                    document.Add(new Paragraph($"- {inspection.Item}"));
                }

                document.Close();
                return File(ms.ToArray(), "application/pdf", "InspectionList.pdf");
            }
        }


        private string ConvertToHtml(InspectionList[] checklists)
        {
            var html = new StringBuilder();
            html.AppendLine("<html><body><h2>Inspection Checklist</h2><table border='1'><tr><th>ChecklistID</th><th>Item</th><th>IsCompleted</th></tr>");

            foreach (var item in checklists)
            {
                html.AppendLine($"<tr><td>{item.ChecklistID}</td><td>{item.Item}</td><td>{item.IsCompleted}</td></tr>");
            }

            html.AppendLine("</table></body></html>");
            return html.ToString();
        }

            [HttpGet]
            [Route("ExportExcel")]
            public async Task<IActionResult> ExportExcel()
            {

            var inspections = _context.InspectionLists.ToList();
            if (inspections == null || !inspections.Any())
            {
                return NotFound("No inspection data found.");
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Inspections");
                worksheet.Cells[1, 1].Value = "Item Name";

                for (int i = 0; i < inspections.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = inspections[i].Item;
                }

                var ms = new MemoryStream();
                package.SaveAs(ms);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspectionList.xlsx");
            }
        }

            private byte[] ConvertToExcel(InspectionList[] checklists)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("InspectionChecklist");
                    worksheet.Cells["A1"].Value = "ChecklistID";
                    worksheet.Cells["B1"].Value = "Item";
                    worksheet.Cells["C1"].Value = "IsCompleted";

                    for (int i = 0; i < checklists.Length; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = checklists[i].ChecklistID;
                        worksheet.Cells[i + 2, 2].Value = checklists[i].Item;
                        worksheet.Cells[i + 2, 3].Value = checklists[i].IsCompleted;
                    }

                    worksheet.Cells["A1:C1"].Style.Font.Bold = true;
                    worksheet.Cells.AutoFitColumns();

                    return package.GetAsByteArray();
                }
            }
        }
    }
