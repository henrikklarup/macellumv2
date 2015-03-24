using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Macellum_Remake.Models
{
    /// <summary>
    /// Magic
    /// </summary>
    [MetadataType(typeof(PasswordMetaData))]
    public partial class Password
    {
        /// <summary>
        /// Not relevant here
        /// </summary>
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Used for double passwords
    /// </summary>
    public class PasswordMetaData
    {
        /// <summary>
        /// Pass field
        /// </summary>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        // ReSharper disable InconsistentNaming
        public string pass { get; set; }
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Confirmpass field
        /// </summary>
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("pass", ErrorMessage = "Fields \"Password\" and \"Confirm Password\" must be equal.")]
        public string ConfirmPassword { get; set; }
    }
}