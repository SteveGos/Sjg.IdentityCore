namespace Sjg.IdentityCore.TagHelpers
{
    public class AccAuthGridPagerModel
    {
        /// <summary>
        /// Page size to pass to Data Retrieval
        /// </summary>
        public int Grid_Pagesize { get; set; }

        /// <summary>
        /// Page to retrieve to pass to Data Retrieval
        /// </summary>
        public int Grid_Page { get; set; }

        /// <summary>
        /// Number of pages - from Data Retrieval.
        /// </summary>
        public int Grid_Pagecount { get; set; }

        /// <summary>
        /// Number of page buttons in pager. Must be between 3 and 11 and an odd number.  Defaults to 5.
        /// </summary>
        public int Grid_Buttoncount { get; set; }
    }
}