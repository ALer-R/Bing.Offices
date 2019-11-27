using System;
using Xunit;
using Xunit.Abstractions;

namespace Bing.Offices.Mappings.Tests
{
    /// <summary>
    /// ���Ի���
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// ���
        /// </summary>
        protected ITestOutputHelper Output { get; }

        /// <summary>
        /// ��ʼ��һ��<see cref="TestBase"/>���͵�ʵ��
        /// </summary>
        /// <param name="output">���</param>
        protected TestBase(ITestOutputHelper output) => Output = output;
    }
}
