using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public class QueryItemsByPageResult<T>
    {
        public QueryItemsByPageResult(bool isFailed)
        {
            this.IsFailed = isFailed;
        }

        /// <summary>
        /// 是否失败
        /// </summary>
        public readonly bool IsFailed;

        /// <summary>
        /// 相关查询的全部记录数量
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// 查询到的分页数据
        /// </summary>
        public IEnumerable<T> Page { get; set; }    
    }

    public delegate bool ContinueQueryNextPageFactory<T>(IEnumerable<T> totalQueriedItems, QueryItemsByPageResult<T> lastTimeQueridResult);
    public delegate IEnumerable<T> RemoveDuplicateFactory<T>(IEnumerable<T> queriedItems);

    /// <summary>
    /// 通过分页查询全部的辅助器
    /// </summary>
    public class QueryAllItemsByPageHelper<T>
    {
        public QueryAllItemsByPageHelper(Func<int, QueryItemsByPageResult<T>> pageQueryFunc)
        {
            this.PageQueryFunc = pageQueryFunc;
        }
        
        public readonly Func<int, QueryItemsByPageResult<T>> PageQueryFunc;

        public int RetryNumberWhenFailed { get; set; } = 1;

        public ContinueQueryNextPageFactory<T> ContinueQueryNextPageFunc { get; set; }

        public RemoveDuplicateFactory<T> RemoveDuplicateFunc { get; set; }

        public IEnumerable<T> QueryAllItems()
        {
            if (PageQueryFunc == null) throw new ArgumentNullException("PageQueryFunc");
            var totalQueriedItems = new List<T>();

            int pageIndex = 0;
            QueryItemsByPageResult<T> firstPageResp = null;
            while (true)
            {
                var pageResp = PageQueryFunc.Invoke(pageIndex);
                if (pageResp == null) break;
                var failedRetryNumber = RetryNumberWhenFailed;
                if (pageResp.IsFailed && failedRetryNumber > 0)
                {
                    int retryNumIndex = 0;
                    while (retryNumIndex < failedRetryNumber)
                    {
                        retryNumIndex++;
                        var innerResp = PageQueryFunc.Invoke(pageIndex);
                        if (innerResp == null) break;
                        else
                        {
                            pageResp = innerResp;
                            if (!innerResp.IsFailed) break;
                        }
                    }
                }

                if (pageIndex == 0)
                {
                    firstPageResp = pageResp;
                }

                if (pageResp.Page != null)
                {
                    totalQueriedItems.AddRange(pageResp.Page);
                }

                bool? continueQueryNextPage = null;
                if (ContinueQueryNextPageFunc != null)
                {
                    continueQueryNextPage = ContinueQueryNextPageFunc.Invoke(totalQueriedItems.ToArray(), pageResp);
                }

                // set default continueQueryNextPage
                if (continueQueryNextPage == null && pageResp.Page != null)
                {
                    continueQueryNextPage = pageResp.Page.Count() > 0;
                }

                if (continueQueryNextPage == true)
                {
                    pageIndex++;
                }
                else
                {
                    break;
                }
            }

            if (firstPageResp != null)
            {
                IEnumerable<T> tarList = totalQueriedItems.ToArray();
                if (RemoveDuplicateFunc != null)
                {
                    tarList = RemoveDuplicateFunc.Invoke(tarList);
                }
                return tarList?.ToArray();
            }

            return null;
        }
    }
}
