using Ermolaev.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Ermolaev.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListPostavki.xaml
    /// </summary>
    public partial class PageListPostavki : Page
    {
        public PageListPostavki()
        {
            InitializeComponent();
            var currentPostavki = ErmolaevEntities.GetContext().Postavki.ToList();
            LViewPostavki.ItemsSource = currentPostavki;
            DataContext = LViewPostavki;
            CmbFiltr.Items.Add("Все продукты");
            foreach (var item in ErmolaevEntities.GetContext().Produktsiya.
                Select(x => x.nazvanie_produktsii).Distinct().ToList())
                CmbFiltr.Items.Add(item);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.Navigate(new Pages.PageAddEdit((sender as Button).DataContext as Postavki));
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxtSearch.Text;
            if (TxtSearch.Text != null)
            {
                LViewPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.
                    Where(x => x.Selskoe_predpriyatie.nazvanie_predpriyatiya.ToString().Contains(search)
                    || x.Produktsiya.nazvanie_produktsii.ToString().Contains(search)
                    || x.obem.ToString().Contains(search)
                    || x.sebestoimost.Contains(search)).ToList();
            }
        }

        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            LViewPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.OrderBy(x => x.obem).ToList();
        }

        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            LViewPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.OrderByDescending(x => x.obem).ToList();
        }

        private void CmbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFiltr.SelectedValue.ToString() == "Все продукты")
            {
                LViewPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.ToList();
            }
            else
            {                LViewPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.
                    Where(x => x.Produktsiya.nazvanie_produktsii == CmbFiltr.SelectedValue.ToString()).ToList();
            }
        }

        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            var app = new Excel.Application();

            //книга 
            Excel.Workbook wb = app.Workbooks.Add();
            //лист
            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            int indexRows = 1;
            //ячейка
            worksheet.Cells[1][indexRows] = "Номер";
            worksheet.Cells[2][indexRows] = "Предприятие";
            worksheet.Cells[3][indexRows] = "Продукт";
            worksheet.Cells[4][indexRows] = "Объём";
            worksheet.Cells[5][indexRows] = "Дата поставки";
            worksheet.Cells[6][indexRows] = "Себестоимость";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewPostavki.Items;
            //цикл по данным из списка для печати
            foreach (Postavki item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.Selskoe_predpriyatie.nazvanie_predpriyatiya;
                worksheet.Cells[3][indexRows + 1] = item.Produktsiya.nazvanie_produktsii;
                worksheet.Cells[4][indexRows + 1] = item.obem;
                worksheet.Cells[5][indexRows + 1] = item.data_postavki;
                worksheet.Cells[6][indexRows + 1] = item.sebestoimost;


                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[5][indexRows + 1]];
            range.ColumnWidth = 20; //ширина столбцов
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//выравнивание по левому краю

            //показать Excel
            app.Visible = true;
        }

        private void BtnSaveToExcelTemplate_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Шаблон.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[4, 2] = DateTime.Now.ToString();
            ws.Cells[4, 5] = 7;
            int indexRows = 6;
            //ячейка
            ws.Cells[1][indexRows] = "Номер";
            ws.Cells[2][indexRows] = "Предприятие";
            ws.Cells[3][indexRows] = "Продукт";
            ws.Cells[4][indexRows] = "Объём";
            ws.Cells[5][indexRows] = "Дата поставки";
            ws.Cells[6][indexRows] = "Себестоимость";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewPostavki.Items;
            //цикл по данным из списка для печати
            foreach (Postavki item in printItems)
            {
                ws.Cells[1][indexRows + 1] = indexRows;
                ws.Cells[2][indexRows + 1] = item.Selskoe_predpriyatie.nazvanie_predpriyatiya;
                ws.Cells[3][indexRows + 1] = item.Produktsiya.nazvanie_produktsii;
                ws.Cells[4][indexRows + 1] = item.obem;
                ws.Cells[5][indexRows + 1] = item.data_postavki;
                ws.Cells[6][indexRows + 1] = item.sebestoimost;

                indexRows++;
            }
            ws.Cells[indexRows + 2, 3] = "Подпись";
            ws.Cells[indexRows + 2, 5] = "Ермолаев Р.М.";
            excelApp.Visible = true;
        }

        private void BtnSaveToWord_Click(object sender, RoutedEventArgs e)
        {
            var allPostavki = ErmolaevEntities.GetContext().Postavki.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Поставка";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allPostavki.Count() + 1, 5);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Предприятие";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Продукт";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Объём";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "Дата поставки";
            cellRange = paymentsTable.Cell(1, 5).Range;
            cellRange.Text = "Себестоимость";

            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allPostavki.Count(); i++)
            {
                var currentPostavki = allPostavki[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentPostavki.Selskoe_predpriyatie.nazvanie_predpriyatiya;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentPostavki.Produktsiya.nazvanie_produktsii;

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = currentPostavki.obem.ToString();

                cellRange = paymentsTable.Cell(i + 2, 4).Range;
                cellRange.Text = currentPostavki.data_postavki.ToString();

                cellRange = paymentsTable.Cell(i + 2, 5).Range;
                cellRange.Text = currentPostavki.sebestoimost;
            }
            Postavki maxSebestoim = ErmolaevEntities.GetContext().Postavki
                .OrderByDescending(p => p.sebestoimost).FirstOrDefault();
            if (maxSebestoim != null)
            {
                Word.Paragraph maxSalaryParagraph = document.Paragraphs.Add();
                Word.Range maxSalaryRange = maxSalaryParagraph.Range;
                maxSalaryRange.Text = $"Самый дорогооплачиваемый оклад - {maxSebestoim.sebestoimost}";
                maxSalaryRange.Font.Color = Word.WdColor.wdColorDarkRed;
                maxSalaryRange.InsertParagraphAfter();
            }

            Postavki minSebestoim = ErmolaevEntities.GetContext().Postavki
                .OrderBy(p => p.sebestoimost).FirstOrDefault();
            if (minSebestoim != null)
            {
                Word.Paragraph minSalaryParagraph = document.Paragraphs.Add();
                Word.Range minSalaryRange = minSalaryParagraph.Range;
                minSalaryRange.Text = $"Самый малооплачиваемый оклад - {minSebestoim.sebestoimost}";
                minSalaryRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                minSalaryRange.InsertParagraphAfter();
            }

            application.Visible = true;

            document.SaveAs2(@"D:\Ермолаев\Ermolaev\bin\Debug\Test.docx");
        }

        private void BtnSaveToPDF_Click(object sender, RoutedEventArgs e)
        {
            var allPostavki = ErmolaevEntities.GetContext().Postavki.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Поставка";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allPostavki.Count() + 1, 5);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Предприятие";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Продукт";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Объём";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "Дата поставки";
            cellRange = paymentsTable.Cell(1, 5).Range;
            cellRange.Text = "Себестоимость";

            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allPostavki.Count(); i++)
            {
                var currentPostavki = allPostavki[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentPostavki.Selskoe_predpriyatie.nazvanie_predpriyatiya;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentPostavki.Produktsiya.nazvanie_produktsii;

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = currentPostavki.obem.ToString();

                cellRange = paymentsTable.Cell(i + 2, 4).Range;
                cellRange.Text = currentPostavki.data_postavki.ToString();

                cellRange = paymentsTable.Cell(i + 2, 5).Range;
                cellRange.Text = currentPostavki.sebestoimost;
            }
            Postavki maxSebestoim = ErmolaevEntities.GetContext().Postavki
                .OrderByDescending(p => p.sebestoimost).FirstOrDefault();
            if (maxSebestoim != null)
            {
                Word.Paragraph maxSalaryParagraph = document.Paragraphs.Add();
                Word.Range maxSalaryRange = maxSalaryParagraph.Range;
                maxSalaryRange.Text = $"Самый дорогооплачиваемый оклад - {maxSebestoim.sebestoimost}";
                maxSalaryRange.Font.Color = Word.WdColor.wdColorDarkRed;
                maxSalaryRange.InsertParagraphAfter();
            }

            Postavki minSebestoim = ErmolaevEntities.GetContext().Postavki
                .OrderBy(p => p.sebestoimost).FirstOrDefault();
            if (minSebestoim != null)
            {
                Word.Paragraph minSalaryParagraph = document.Paragraphs.Add();
                Word.Range minSalaryRange = minSalaryParagraph.Range;
                minSalaryRange.Text = $"Самый малооплачиваемый оклад - {minSebestoim.sebestoimost}";
                minSalaryRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                minSalaryRange.InsertParagraphAfter();
            }

            application.Visible = true;

            document.SaveAs2(@"D:\Ермолаев\Ermolaev\bin\Debug\Test.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
        {
            LViewPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.ToList();
        }
    }
}
