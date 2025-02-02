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
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Playwright
{
    /// <summary>
    /// <para>
    /// <see cref="IDownload"/> objects are dispatched by page via the <see cref="IPage.Download"/>
    /// event.
    /// </para>
    /// <para>
    /// If <c>downloadsPath</c> isn't specified, all the downloaded files belonging to the
    /// browser context are deleted when the browser context is closed. And all downloaded
    /// files are deleted when the browser closes.
    /// </para>
    /// <para>
    /// Download event is emitted once the download starts. Download path becomes available
    /// once download completes:
    /// </para>
    /// <code>
    /// var download = await page.RunAndWaitForDownloadAsync(async () =&gt;<br/>
    /// {<br/>
    ///     await page.ClickAsync("#downloadButton");<br/>
    /// });<br/>
    /// Console.WriteLine(await download.PathAsync());
    /// </code>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Browser context **must** be created with the <paramref name="acceptDownloads"/>
    /// set to <c>true</c> when user needs access to the downloaded content. If <paramref
    /// name="acceptDownloads"/> is not set, download events are emitted, but the actual
    /// download is not performed and user has no access to the downloaded files.
    /// </para>
    /// </remarks>
    public partial interface IDownload
    {
        /// <summary><para>Returns readable stream for current download or <c>null</c> if download failed.</para></summary>
        Task<Stream> CreateReadStreamAsync();

        /// <summary><para>Deletes the downloaded file. Will wait for the download to finish if necessary.</para></summary>
        Task DeleteAsync();

        /// <summary><para>Returns download error if any. Will wait for the download to finish if necessary.</para></summary>
        Task<string> FailureAsync();

        /// <summary>
        /// <para>
        /// Returns path to the downloaded file in case of successful download. The method will
        /// wait for the download to finish if necessary. The method throws when connected remotely.
        /// </para>
        /// </summary>
        Task<string> PathAsync();

        /// <summary>
        /// <para>
        /// Copy the download to a user-specified path. It is safe to call this method while
        /// the download is still in progress. Will wait for the download to finish if necessary.
        /// </para>
        /// </summary>
        /// <param name="path">Path where the download should be copied.</param>
        Task SaveAsAsync(string path);

        /// <summary>
        /// <para>
        /// Returns suggested filename for this download. It is typically computed by the browser
        /// from the <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Disposition"><c>Content-Disposition</c></a>
        /// response header or the <c>download</c> attribute. See the spec on <a href="https://html.spec.whatwg.org/#downloading-resources">whatwg</a>.
        /// Different browsers can use different logic for computing it.
        /// </para>
        /// </summary>
        string SuggestedFilename { get; }

        /// <summary><para>Returns downloaded url.</para></summary>
        string Url { get; }
    }
}
