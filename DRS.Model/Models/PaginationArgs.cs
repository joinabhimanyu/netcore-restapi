using System;
using System.Collections.Generic;
using System.Text;

namespace DRS.Model.Models
{
    /// <summary>
    /// Structure representing pagination argument
    /// </summary>
    public class PaginationArgs
    {
        public string Type { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// Navigatio props used for filtering entity for nested or memeber properties applied at the beginning (1)
        /// </summary>
        public FilterNavigationWrapper NavigationProperty { get; set; } = new FilterNavigationWrapper();
        /// <summary>
        /// Base filter props used for filtering entity before after navigation props (2)
        /// </summary>
        public FilterNavigationWrapper BaseFilterProps { get; set; } = new FilterNavigationWrapper();
        /// <summary>
        /// Custom filter props used for filtering entity after applying base filter props (3)
        /// </summary>
        public CustomFilterNavigationWrapper CustomFilterProps { get; set; } = new CustomFilterNavigationWrapper();
        /// <summary>
        /// Filter props used for filtering entity after applying custom filter props (4)
        /// </summary>
        public FilterNavigationWrapper FilterProps { get; set; } = new FilterNavigationWrapper();
        /// <summary>
        /// Order by prop used for ordering entity (5)
        /// </summary>
        public OrderByWrapper OrderByProp { get; set; } = new OrderByWrapper();
        /// <summary>
        /// True if any of the filteration props are nested else false
        /// </summary>
        public bool IsNavigationEnabled { get; set; } = false;
    }
    /// <summary>
    /// Structure representing order by argument
    /// </summary>
    public class OrderByWrapper
    {
        public string PropName { get; set; }
        public string Order { get; set; } = "asc";
    }
    /// <summary>
    /// Wrapper for containing filteration argument
    /// </summary>
    public class FilterNavigationWrapper
    {
        public List<FilterNavigationProps> Props { get; set; } = new List<FilterNavigationProps>();
        public bool IsMultipleValue { get; set; } = false;
    }
    /// <summary>
    /// Interface for representing structure of both FilterNavigationProps and CustomFilterNavigationProps
    /// </summary>
    public interface INavigatioPropsStructure
    {
        string PropName { get; set; }
        string PropValue { get; set; }
        bool IsNestedProp { get; set; }
    }
    /// <summary>
    /// Structure of filteration properties
    /// </summary>
    public class FilterNavigationProps : INavigatioPropsStructure
    {
        public string PropName { get; set; } = String.Empty;
        public string PropValue { get; set; } = String.Empty;
        public bool IsNestedProp { get; set; } = false;

    }
    public class CustomFilterNavigationWrapper
    {
        public List<CustomFilterNavigationProps> Props { get; set; } = new List<CustomFilterNavigationProps>();
        /// <summary>
        /// filter clause: where, where-not
        /// </summary>
        public string FilterClause { get; set; } = "where";
        /// <summary>
        /// Depicts whether the props are multiple or not
        /// </summary>
        public bool IsMultiple { get; set; } = false;
    }
    /// <summary>
    /// Structure representing custom filteration navigation wrapper
    /// </summary>
    public class CustomFilterNavigationProps : INavigatioPropsStructure
    {
        public string PropName { get; set; }
        public string PropValue { get; set; }
        public bool IsNestedProp { get; set; } = false;
        /// <summary>
        /// operator: eq, not-eq, gt-eq, gt, lt, lt-eq, contains, not-contains, starts-with
        /// </summary>
        public string Operator { get; set; } = "eq";
        /// <summary>
        /// joining clause: and, or
        /// </summary>
        public string JoiningClause { get; set; }
    }
    /// <summary>
    /// Wrapper for single argument properties
    /// </summary>
    public class GetSingleArgs
    {
        public FilterNavigationWrapper FilterProps { get; set; } = new FilterNavigationWrapper();
        public bool IsNavigationEnabled { get; set; } = false;
    }
}
