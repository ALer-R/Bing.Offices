using System.Linq;
using Bing.Offices.Abstractions.Imports;
using Bing.Offices.Attributes;
using Bing.Offices.Npoi.Imports;
using Bing.Utils.Extensions;
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
        private IExcelImportProvider _excelImportProvider;

        public TestBase(ITestOutputHelper output)
        {
            Output = output;
            _excelImportProvider = new ExcelImportProvider();
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
