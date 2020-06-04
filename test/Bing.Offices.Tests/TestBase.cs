using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bing.Offices.Attributes;
using Bing.Offices.Exports;
using Bing.Offices.Extensions;
using Bing.Offices.Imports;
using Bing.Offices.Npoi.Exports;
using Bing.Offices.Npoi.Imports;
using Bing.Offices.Tests.Models;
using Bing.Extensions;
using Bing.Offices.Metadata.Excels;
using Bing.Utils.Json;
using NPOI.SS.Formula.Functions;
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
        /// ���� - ���� ��̬����
        /// </summary>
        [Fact]
        public void Test_Import_DynamicTitle_1()
        {
            var result = _excelImportProvider.Convert<Barcode>("D:\\���������_�����ʽ_��̬����.xlsx");
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
        /// ���� - ���� ��̬����
        /// </summary>
        [Fact]
        public async Task Test_Import_DynamicTitle_2()
        {
            var workbook = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ_��̬����.xlsx",
            });
            var result = workbook.GetResult<Barcode>();
            Output.WriteLine(result.Count().ToString());
            Output.WriteLine(result.ToJson());
        }

        /// <summary>
        /// ���� - ���� ��̬����
        /// </summary>
        [Fact]
        public async Task Test_Import_DynamicTitle_More_1()
        {
            var workbook1 = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ_��̬����.xlsx",
            });
            var result1 = workbook1.GetResult<Barcode>();
            Output.WriteLine(result1.Count().ToString());
            Output.WriteLine(result1.ToJson());
            var workbook2 = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ_��̬����1.xlsx",
            });
            var result2 = workbook2.GetResult<Barcode>();
            Output.WriteLine(result2.Count().ToString());
            Output.WriteLine(result2.ToJson());
        }

        /// <summary>
        /// ���� - ������
        /// </summary>
        [Fact]
        public async Task Test_Import_Validate_1()
        {
            var workbook = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ.xlsx",
            });
            var validateResult = workbook.Validate();
            if (validateResult.Any())
            {
                Output.WriteLine(validateResult.ToJson());
                return;
            }

            var result = workbook.GetResult<Barcode>();
            Output.WriteLine(result.Count().ToString());
            Output.WriteLine(result.ToJson());
        }
        
        /// <summary>
        /// ���� - ���� �ɿ�ֵ
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_Import_NullableValue_1()
        {
            var workbook = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ.xlsx",
            });
            var validateResult = workbook.Validate();
            if (validateResult.Any())
            {
                Output.WriteLine(validateResult.ToJson());
                return;
            }

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

        /// <summary>
        /// ���� - ���� ��̬����
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_Export_DynamicTitle_1()
        {
            var workbook = await _excelImportService.ImportAsync<Barcode>(new ImportOptions()
            {
                FileUrl = "D:\\���������_�����ʽ_��̬����1.xlsx",
            });
            var result = workbook.GetResult<Barcode>();

            var bytes = await _excelExportService.ExportAsync(new ExportOptions<Barcode>()
            {
                Data = result.ToList(),
                DynamicColumns = new List<string>() { "������", "����ʱ��", "������", "��ע" }
            });
            await File.WriteAllBytesAsync($"D:\\���Ե���_{DateTime.Now:yyyyMMddHHmmss}.xlsx", bytes);
        }


        /// <summary>
        /// ���� - ���� ��������
        /// </summary>
        [Fact]
        public async Task Test_Export_IgnoreProperty()
        {
            var data = new List<ExportOrder>();
            for (int i = 0; i < 10000; i++)
            {
                DateTime? currentTime = null;
                if (i % 2 == 0)
                    currentTime = DateTime.Now;
                data.Add(new ExportOrder()
                {
                    Id = $"A{i}",
                    Name = $"��������+++++{i}",
                    Index = i + 1,
                    CreateTime = currentTime,
                    IgnoreProperty = $"��������+++++{i}",
                    NotMappedProperty = $"����ӳ������+++++{i}"
                });
            }

            var bytes = await _excelExportService.ExportAsync(new ExportOptions<ExportOrder>()
            {
                Data = data,
            });
            await File.WriteAllBytesAsync($"D:\\���Ե���_{DateTime.Now:yyyyMMddHHmmss}.xlsx", bytes);
        }

        /// <summary>
        /// ���� - ���� ��ʽ������
        /// </summary>
        [Fact]
        public async Task Test_Export_FormatProperty()
        {
            var data = new List<Bing.Offices.Tests.Models.ExportFormat>();
            for (int i = 0; i < 10000; i++)
            {
                data.Add(new Bing.Offices.Tests.Models.ExportFormat()
                {
                    Id = $"A{i}",
                    Name = $"��������+++++{i}",
                    Index = i + 1,
                    IgnoreProperty = $"��������+++++{i}",
                    NotMappedProperty = $"����ӳ������+++++{i}",
                    Money = i*1000,
                    CreateTime = DateTime.Now.AddMinutes(i)
                });
            }

            var bytes = await _excelExportService.ExportAsync(new ExportOptions<Bing.Offices.Tests.Models.ExportFormat>()
            {
                Data = data,
            });
            await File.WriteAllBytesAsync($"D:\\���Ե���_{DateTime.Now:yyyyMMddHHmmss}.xlsx", bytes);
        }


        /// <summary>
        /// ���� - ���� �����ͷ
        /// </summary>
        [Fact]
        public async Task Test_Export_HeaderRow()
        {
            var data = new List<Bing.Offices.Tests.Models.ExportFormat>();
            for (int i = 0; i < 10000; i++)
            {
                data.Add(new Bing.Offices.Tests.Models.ExportFormat()
                {
                    Id = $"A{i}",
                    Name = $"��������+++++{i}",
                    Index = i + 1,
                    IgnoreProperty = $"��������+++++{i}",
                    NotMappedProperty = $"����ӳ������+++++{i}",
                    Money = i * 1000,
                    CreateTime = DateTime.Now.AddMinutes(i)
                });
            }

            var rows = new List<Bing.Offices.Metadata.Excels.Row>();
            var row = new Bing.Offices.Metadata.Excels.Row(0);
            row.Add(new Cell("����",2,2,1));
            row.Add(new Cell("����", 4,2, 1));
            rows.Add(row); 

            var bytes = await _excelExportService.ExportAsync(new ExportOptions<Bing.Offices.Tests.Models.ExportFormat>()
            {
                HeaderRow=rows.ToList<IRow>(),
                HeaderRowIndex = 1,
                DataRowStartIndex = 2,
                Data = data,
            });
            await File.WriteAllBytesAsync($"D:\\���Ե���_{DateTime.Now:yyyyMMddHHmmss}.xlsx", bytes);
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public class Barcode
    {
        /// <summary>
        /// ϵͳ
        /// </summary>
        [ColumnName("ϵͳ���")]
        public string Id { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [ColumnName("������")]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [ColumnName("����ʱ��")]
        public string CreateDate { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [ColumnName("����")]
        public int? Age { get; set; }

        /// <summary>
        /// ��չ
        /// </summary>
        [DynamicColumn]
        public IDictionary<string, object> Extend { get; set; }
    }
}
