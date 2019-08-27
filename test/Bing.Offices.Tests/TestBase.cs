using System.Linq;
using System.Threading.Tasks;
using Bing.Offices.Abstractions.Imports;
using Bing.Offices.Attributes;
using Bing.Offices.Extensions;
using Bing.Offices.Imports;
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
        /// ��ʼ��һ��<see cref="TestBase"/>���͵�ʵ��
        /// </summary>
        public TestBase(ITestOutputHelper output)
        {
            Output = output;
            _excelImportProvider = new ExcelImportProvider();
            _excelImportService = new ExcelImportService(_excelImportProvider);
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
