/*
 * MIT License
 *
 * Copyright (c) Microsoft Corporation.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Microsoft.Playwright.NUnitTest
{
    public class BrowserTest : PlaywrightTest
    {
        public class BrowserService : IWorkerService
        {
            private IBrowserType browserType_;
            public IBrowser Browser { get; internal set; }

            public BrowserService(IBrowserType browserType)
            {
                browserType_ = browserType;
            }

            public async Task InitAsync()
            {
                Browser = await browserType_.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true
                });
            }

            public Task ResetAsync() => Task.CompletedTask;
            public Task DisposeAsync() => Browser.CloseAsync();
        };

        public IBrowser Browser { get; internal set; }

        [SetUp]
        public async Task BrowserSetup()
        {
            var service = await Services.Register("Browser", async () =>
            {
                var service = new BrowserService(BrowserType);
                await service.InitAsync();
                return service;
            });
            Browser = service.Browser;
        }
    }
}
