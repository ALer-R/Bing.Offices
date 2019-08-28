using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bing.Offices.Abstractions.Exports;
using Bing.Offices.Abstractions.Imports;
using Bing.Offices.Attributes;
using Bing.Offices.Exports;
using Bing.Offices.Extensions;
using Bing.Offices.Imports;
using Bing.Offices.Npoi.Exports;
using Bing.Offices.Npoi.Imports;
using Bing.Utils.Extensions;
using Bing.Utils.Json;
using Xunit;
using Xunit.Abstractions;

namespace Bing.Offices.Tests
{
    /// <summary>
    /// ���Ի���
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// ���
        /// </summary>
        protected ITestOutputHelper Output;

        /// <summary>
        /// Excel�����ṩ����
        /// </summary>
        private readonly IExcelImportProvider _excelImportProvider;

        /// <summary>
        /// Excel�������
        /// </summary>
        private readonly IExcelImportService _excelImportService;

        /// <summary>
        /// Excel�����ṩ����
        /// </summary>
        private readonly IExcelExportProvider _excelExportProvider;

        /// <summary>
        /// Excel��������
        /// </summary>
        private readonly IExcelExportService _excelExportService;

        /// <summary>
        /// ��ʼ��һ��<see cref="TestBase"/>���͵�ʵ��
        /// </summary>
        public TestBase(ITestOutputHelper output)
        {
            Output = output;
            _excelImportProvider = new ExcelImportProvider();
            _excelImportService = new ExcelImportService(_excelImportProvider);
            _excelExportProvider = new ExcelExportProvider();
            _excelExportService = new ExcelExportService(_excelExportProvider);
        }

        /// <summary>
        /// ���� - ����
        /// </summary>
        [Fact]
        public void Test_Import()
        {
            var result = _excelImportProvider.Convert<Barcode>("D:\\���������_�����ʽ.xlsx");
            foreach (var sheet in result.Sheets)
            {
                foreach (var header in sheet.GetHeader())
                {
                    Output.WriteLine(header.Cells.Select(x => x.Value.ToString()).Join());
                }

                foreach (var body in sheet.GetBody())
                {
                    Output.WriteLine(body.Cells.Select(x => x.Value.ToString()).Join());
                }
            }
        }

        /// <summary>
        /// ���� - ������
        /// </summary>
        [Fact]
        public async Task Test_Import_1()
        {
            var workbook = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ.xlsx",
            });
            var result = workbook.GetResult<Barcode>();
            Output.WriteLine(result.Count().ToString());
            Output.WriteLine(result.ToJson());
        }
        
        /// <summary>
        /// ���� - ����
        /// </summary>
        [Fact]
        public async Task Test_Export()
        {
            var workbook = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ.xlsx",
            });
            var result = workbook.GetResult<Barcode>();

            var bytes = await _excelExportService.ExportAsync(new ExportOptions<Barcode>()
            {
                Data = result.ToList()
            });
            await File.WriteAllBytesAsync($"D:\\���Ե���_{DateTime.Now:yyyyMMddHHmmss}.xlsx", bytes);
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public class Barcode
    {
        [ColumnName("ϵͳ���")]
        public string Id { get; set; }

        [ColumnName("������")]
        public string Code { get; set; }
    }
}
